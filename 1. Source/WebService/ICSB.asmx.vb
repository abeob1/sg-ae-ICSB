﻿Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Web.Script.Services
Imports System.IO
'Gopi
' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class ICSB
    Inherits System.Web.Services.WebService
    Dim fn As New Functions
    Dim oLog As New modLogging
    Dim sFunction As String
    Dim DocEntry As String = ""
    <WebMethod()> _
    Public Function HelloWorld() As String

        Dim sErrDesc As String = ""

        '   Return      :   0 - FAILURE
        '                   1 - SUCCESS
        Dim sFuncName As String = String.Empty
        Dim iRetValue As Integer = -1
        Dim iErrCode As Integer = -1
        Dim sSQL As String = String.Empty
        Dim oDs As New DataSet
        Dim sSAPUser As String = String.Empty
        Dim sSAPPWd As String = String.Empty
        Dim sTrgtDBName As String = String.Empty


        Try
            sFuncName = "ConnectToTargetCompany()"

            Dim oCompany As New SAPbobsCOM.Company


            oCompany.Server = "10.0.20.105:30015"

            oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB

            oCompany.LicenseServer = "10.0.20.105:40000"
            oCompany.CompanyDB = "ICSB_DEV"
            oCompany.UserName = "manager1"
            oCompany.Password = "1234"
            oCompany.DbUserName = "SYSTEM"
            oCompany.DbPassword = "Sapb1hana"
            oCompany.language = SAPbobsCOM.BoSuppLangs.ln_English

            oCompany.UseTrusted = False


            iRetValue = oCompany.Connect()

            If iRetValue <> 0 Then
                oCompany.GetLastError(iErrCode, sErrDesc)

                sErrDesc = String.Format("Connection to Database ({0}) {1} {2} {3}", _
                    oCompany.CompanyDB, System.Environment.NewLine, _
                                vbTab, sErrDesc)

                Throw New ArgumentException(sErrDesc)
            End If


        Catch ex As Exception
            sErrDesc = ex.Message


        End Try


        Return "Hello World"
    End Function
    <WebMethod()> _
  <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub Login(ByVal value As String)
        sFunction = "Login"
        Try
            If PublicVariable.DEBUG_ON = 1 Then
                oLog.WriteToLogFile_Debug(value, sFunction)
            End If

            Dim sErrDesc As String = ""
            Dim SQLStr As String = ""
            Dim dt As New DataTable
            Dim SQLDT As New DataTable
            Dim RetDT As New DataTable
            Dim RetDS = New DataSet()

            Dim ds As DataSet = fn.jsontodata(value)
            If ds.Tables("WUSER").Rows.Count > 0 Then
                dt = ds.Tables("WUSER")
                Dim dr As DataRow = dt.Rows(0)
                Dim UserCode As String = dr("uid").ToString.Trim().ToUpper
                Dim passwrod As String = dr("pwd").ToString.Trim()
                Dim MenuGroup As String = ""
                Dim Email As String = ""
                Dim UserName As String = ""
                Dim sAgentCompany As String = String.Empty

                'SQLStr = "SELECT  T0.""U_MENUAGRP"",T0.""Name"", T0.""U_EMail"" FROM ""@WUSER""  T0 WHERE Upper(T0.""Code"") ='" & UserCode & "' and  T0.""U_Pwd"" ='" & passwrod & "'"
                SQLStr = "SELECT T0.""U_MENUAGRP"", T0.""Name"", T0.""U_EMail"", T1.""CardName"" AS ""AgentCompany"" FROM ""@WUSER"" T0 " & _
                         " LEFT JOIN ""OCRD"" T1 ON T1.""CardCode"" = T0.""U_ComCode"" " & _
                         " WHERE Upper(T0.""Code"") ='" & UserCode & "' and  T0.""U_Pwd"" ='" & passwrod & "'"
                SQLDT = fn.ExecuteSQLQuery(SQLStr, sErrDesc)
                If sErrDesc <> "" Then
                    Throw New Exception(sErrDesc)
                ElseIf SQLDT.Rows.Count = 0 Then
                    Throw New Exception("Invalid Username or Password")
                Else
                    Dim SQLDR As DataRow = SQLDT.Rows(0)
                    MenuGroup = SQLDR("U_MENUAGRP").ToString.Trim().ToUpper
                    Email = SQLDR("U_EMail").ToString.Trim()
                    UserName = SQLDR("Name").ToString.Trim()
                    sAgentCompany = SQLDR("AgentCompany").ToString.Trim()

                    RetDT.TableName = "VALIDATE"
                    RetDT.Columns.Add("Status")
                    RetDT.Columns.Add("Msg")
                    RetDT.Rows.Add()
                    RetDT.Rows(0)(0) = True
                    RetDT.Rows(0)(1) = ""
                    RetDS.Tables.Add(RetDT)
                    RetDT.Dispose()

                    RetDT = New DataTable
                    RetDT.TableName = "UserData"
                    RetDT.Columns.Add("Code")
                    RetDT.Columns.Add("Name")
                    RetDT.Columns.Add("Email")
                    RetDT.Columns.Add("Approver")
                    RetDT.Columns.Add("AgentCompany")
                    RetDT.Rows.Add()
                    RetDT.Rows(0)(0) = UserCode
                    RetDT.Rows(0)(1) = UserName
                    RetDT.Rows(0)(2) = Email
                    RetDT.Rows(0)(4) = sAgentCompany
                    SQLStr = "SELECT T0.""U_AppCode"" FROM ""@WAPPPROL""  T0 WHERE T0.""U_AppCode"" ='" & UserCode & "'"
                    If fn.ExecuteSQLQuery_SingleValue(SQLStr, sErrDesc) <> "" Then
                        RetDT.Rows(0)(3) = "Y"
                    Else
                        RetDT.Rows(0)(3) = "N"
                    End If
                    RetDS.Tables.Add(RetDT)
                    SQLStr = "SELECT T0.""U_MenuID"", T0.""U_MenuN"" FROM ""@WMENUGRPL""  T0 WHERE UPPER(T0.""Code"") ='" & MenuGroup & "'"
                    SQLDT = New DataTable
                    SQLDT = fn.ExecuteSQLQuery(SQLStr, sErrDesc)
                    If sErrDesc <> "" Then
                        Throw New Exception(sErrDesc)
                    ElseIf SQLDT.Rows.Count = 0 Then
                        Throw New Exception("Menu Mapping not found")
                    Else
                        RetDT = New DataTable
                        SQLDT.TableName = "MenuInfo"

                        RetDT = SQLDT.Copy()

                        RetDS.Tables.Add(RetDT)
                    End If
                    Context.Response.Output.Write(fn.ds2json(RetDS))
                End If

            Else
                Throw New Exception("WUSER Table not found!")
            End If


        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
#Region "Master"
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub AgentMaster(ByVal value As String)
        Try
            Dim Code As String = ""
            Dim Name As String = ""
            Dim OCRD As New DataTable
            OCRD = New DataTable
            Dim LEADMDS = New DataSet()
            Dim SQLDT As New DataTable
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""

            If ds.Tables("OCRD").Rows.Count > 0 Then
                OCRD = ds.Tables("OCRD")
                Dim dr As DataRow = OCRD.Rows(0)
                Code = dr.Item("U_Agentcode").ToString.Trim
                Name = dr.Item("U_Agentname").ToString.Trim
            End If

            If Code = "" Then
                Code = "%"
            ElseIf Code.Contains("*") = True Then
                Code = Code.Replace("*", "%")
            Else
                Code = "%" & Code & "%"
            End If

            If Name = "" Then
                Name = "%"
            ElseIf Name.Contains("*") = True Then
                Name = Name.Replace("*", "%")
            Else
                Name = "%" & Name & "%"
            End If
            'Dim Str As String = "SELECT T0.""CardCode"" As ""U_Ccode"", T0.""CardName"" as ""U_Cname"", T0.""Phone1"" as ""U_TelNo"", T0.""Fax"" as ""U_FaxNo"", T0.""Cellular"" as ""U_Mno"", T0.""E_Mail"" as ""U_Email"" FROM OCRD T0 WHERE IFNULL( T0.""U_WebAccess"" ,'') ='Y' and  T0.""CardType"" ='C' and  T0.""CardCode""  like '" & Code & "' and T0.""CardName"" like '" & Name & "'"
            Dim Str As String
            Str = "SELECT 'C' as ""U_Ctype"",T0.""CardCode"" As ""U_Agentcode"", T0.""CardName"" as ""U_Agentname"", T0.""Phone1"" as ""U_TelNo"", T0.""Fax"" as ""U_FaxNo"", " & _
                  " T0.""Cellular"" as ""U_Mno"", T0.""E_Mail"" as ""U_Email"" FROM OCRD T0 WHERE IFNULL( T0.""U_WebAccess"" ,'') ='Y' and  T0.""CardType"" ='S' " & _
                  " and  UPPER(T0.""CardCode"")  like '" & Code.ToUpper() & "' and UPPER(T0.""CardName"") like '" & Name.ToUpper() & "' order by ""U_Agentname"""

            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable
            Dim RetDT2 As New DataTable
            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "OCRD"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                Str = "SELECT T0.""CardCode"" as ""U_Agentcode"", T0.""Address"" ""U_AddrN"", T0.""Street"" ""U_Addr1"", T0.""Block"" ""U_Addr2"", T0.""City"" ""U_Addr3"", " & _
                      " T0.""County"" ""U_Addr4"", T0.""StreetNo"" ""U_Addr5"", T0.""GlblLocNum"" ""U_Addr6"" FROM CRD1 T0  " & _
                      " INNER JOIN OCRD T1 ON T0.""CardCode"" = T1.""CardCode"" WHERE T0.""AdresType"" ='B' and T1.""CardType"" ='S' " & _
                      " and  UPPER(T1.""CardCode"")  like '" & Code.ToUpper() & "' and  UPPER(T1.""CardName"") like '" & Name.ToUpper() & "'"

                RetDT = New DataTable
                RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
                If Errmsg <> "" Then
                    Throw New Exception(Errmsg)
                End If
                RetDT.TableName = "ADDR"
                RetDT2 = RetDT.Copy
                RetDS.Tables.Add(RetDT2)

                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub CustomerMaster_CustomerOnly(ByVal value As String)
        Try
            Dim Code As String = ""
            Dim Name As String = ""
            Dim OCRD As New DataTable
            OCRD = New DataTable
            Dim LEADMDS = New DataSet()
            Dim SQLDT As New DataTable
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""

            If ds.Tables("OCRD").Rows.Count > 0 Then
                OCRD = ds.Tables("OCRD")
                Dim dr As DataRow = OCRD.Rows(0)
                Code = dr.Item("U_Ccode").ToString.Trim
                Name = dr.Item("U_Cname").ToString.Trim
            End If

            If Code = "" Then
                Code = "%"
            ElseIf Code.Contains("*") = True Then
                Code = Code.ToUpper().Replace("*", "%")
            Else
                Code = "%" & Code.ToUpper() & "%"
            End If

            If Name = "" Then
                Name = "%"
            ElseIf Name.Contains("*") = True Then
                Name = Name.ToUpper().Replace("*", "%")
            Else
                Name = "%" & Name.ToUpper() & "%"
            End If
            'Dim Str As String = "SELECT T0.""CardCode"" As ""U_Ccode"", T0.""CardName"" as ""U_Cname"", T0.""Phone1"" as ""U_TelNo"", T0.""Fax"" as ""U_FaxNo"", T0.""Cellular"" as ""U_Mno"", T0.""E_Mail"" as ""U_Email"" FROM OCRD T0 WHERE IFNULL( T0.""U_WebAccess"" ,'') ='Y' and  T0.""CardType"" ='C' and  T0.""CardCode""  like '" & Code & "' and T0.""CardName"" like '" & Name & "'"
            Dim Str As String = "SELECT 'C' as ""U_Ctype"",T0.""CardCode"" As ""U_Ccode"", T0.""CardName"" as ""U_Cname"", T0.""Phone1"" as ""U_TelNo"", T0.""Fax"" as ""U_FaxNo"", " & _
                                " T0.""Cellular"" as ""U_Mno"", T0.""E_Mail"" as ""U_Email"" " & _
                                " FROM OCRD T0 WHERE IFNULL( T0.""U_WebAccess"" ,'') ='Y' and  T0.""CardType"" ='C' " & _
                                " and  UPPER(T0.""CardCode"")  like '" & Code.ToUpper() & "' and UPPER(T0.""CardName"") like '" & Name.Replace("'", "''").ToUpper() & "' order by ""U_Cname"""

            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable
            Dim RetDT2 As New DataTable
            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "OCRD"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                Str = "SELECT T0.""CardCode"" as ""U_Ccode"", T0.""Address"" ""U_AddrN"", T0.""Street"" ""U_Addr1"", T0.""Block"" ""U_Addr2"", T0.""City"" ""U_Addr3"", T0.""County"" ""U_Addr4"", T0.""StreetNo"" ""U_Addr5"", T0.""GlblLocNum"" ""U_Addr6"" FROM CRD1 T0  INNER JOIN OCRD T1 ON T0.""CardCode"" = T1.""CardCode"" " & _
                      " WHERE T0.""AdresType"" ='B' and T1.""CardType"" ='C' and  UPPER(T1.""CardCode"")  like '" & Code.ToUpper() & "' and  UPPER(T1.""CardName"") like '" & Name.Replace("'", "''").ToUpper() & "'"

                RetDT = New DataTable
                RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
                If Errmsg <> "" Then
                    Throw New Exception(Errmsg)
                End If
                RetDT.TableName = "ADDR"
                RetDT2 = RetDT.Copy
                RetDS.Tables.Add(RetDT2)

                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub CustomerMaster_CustomerOnly_SO(ByVal value As String)
        Try
            Dim Code As String = ""
            Dim Name As String = ""
            Dim OCRD As New DataTable
            OCRD = New DataTable
            Dim LEADMDS = New DataSet()
            Dim SQLDT As New DataTable
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""
            Dim sUid As String = ""

            If ds.Tables("OCRD").Rows.Count > 0 Then
                OCRD = ds.Tables("OCRD")
                Dim dr As DataRow = OCRD.Rows(0)
                Code = dr.Item("U_Ccode").ToString.Trim
                Name = dr.Item("U_Cname").ToString.Trim
                sUid = dr.Item("uid").ToString.Trim
            End If

            If Code = "" Then
                Code = "%"
            ElseIf Code.Contains("*") = True Then
                Code = Code.ToUpper().Replace("*", "%")
            Else
                Code = "%" & Code.ToUpper() & "%"
            End If

            If Name = "" Then
                Name = "%"
            ElseIf Name.Contains("*") = True Then
                Name = Name.ToUpper().Replace("*", "%")
            Else
                Name = "%" & Name.ToUpper() & "%"
            End If

            ''Dim Str As String = "SELECT T0.""CardCode"" As ""U_Ccode"", T0.""CardName"" as ""U_Cname"", T0.""Phone1"" as ""U_TelNo"", T0.""Fax"" as ""U_FaxNo"", T0.""Cellular"" as ""U_Mno"", T0.""E_Mail"" as ""U_Email"" FROM OCRD T0 WHERE IFNULL( T0.""U_WebAccess"" ,'') ='Y' and  T0.""CardType"" ='C' and  T0.""CardCode""  like '" & Code & "' and T0.""CardName"" like '" & Name & "'"
            'Dim Str As String = "SELECT 'C' as ""U_Ctype"",T0.""CardCode"" As ""U_Ccode"", T0.""CardName"" as ""U_Cname"", T0.""Phone1"" as ""U_TelNo"", T0.""Fax"" as ""U_FaxNo"", T0.""Cellular"" as ""U_Mno"", T0.""E_Mail"" as ""U_Email"" FROM OCRD T0 WHERE IFNULL( T0.""U_WebAccess"" ,'') ='Y' and  T0.""CardType"" ='C' and  T0.""CardCode""  like '" & Code & "' and T0.""CardName"" like '" & Name & "' order by ""U_Cname"""
            Dim Str As String = String.Empty
            Str = "SELECT T1.""CardType"" as ""U_Ctype"",T0.""U_CardCode"" AS ""U_Ccode"",T1.""CardName"" as ""U_Cname"",T1.""Phone1"" as ""U_TelNo"", " & _
                  " T1.""Fax"" as ""U_FaxNo"",T1.""Cellular"" AS ""U_Mno"",T1.""E_Mail"" as ""U_Email"" " & _
                  " FROM ""@WUSERL"" T0 INNER JOIN ""OCRD"" T1 ON T1.""CardCode"" = T0.""U_CardCode"" WHERE IFNULL(T1.""U_WebAccess"",'') = 'Y' AND T1.""CardType"" = 'C' " & _
                  " AND T0.""Code"" = '" & sUid & "' AND IFNULL(T1.""CardCode"",'') <> '' " & _
                  " AND UPPER(T1.""CardCode"")  like '" & Code.ToUpper() & "' and UPPER(T1.""CardName"") like '" & Name.ToUpper() & "' order by ""U_Cname"""

            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable
            Dim RetDT2 As New DataTable
            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "OCRD"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                Str = "SELECT T0.""CardCode"" as ""U_Ccode"", T0.""Address"" ""U_AddrN"", T0.""Street"" ""U_Addr1"", T0.""Block"" ""U_Addr2"", T0.""City"" ""U_Addr3"", T0.""County"" ""U_Addr4"", T0.""StreetNo"" ""U_Addr5"", T0.""GlblLocNum"" ""U_Addr6"" FROM CRD1 T0  INNER JOIN OCRD T1 ON T0.""CardCode"" = T1.""CardCode"" " & _
                      " WHERE T0.""AdresType"" ='B' and T1.""CardType"" ='C' and  UPPER(T1.""CardCode"")  like '" & Code.ToUpper() & "' and  UPPER(T1.""CardName"") like '" & Name.Replace("'", "''").ToUpper() & "'"

                RetDT = New DataTable
                RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
                If Errmsg <> "" Then
                    Throw New Exception(Errmsg)
                End If
                RetDT.TableName = "ADDR"
                RetDT2 = RetDT.Copy
                RetDS.Tables.Add(RetDT2)

                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub CustomerMaster(ByVal value As String)
        Try
            Dim Code As String = ""
            Dim Name As String = ""
            Dim OCRD As New DataTable
            OCRD = New DataTable
            Dim LEADMDS = New DataSet()
            Dim SQLDT As New DataTable
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""

            If ds.Tables("OCRD").Rows.Count > 0 Then
                OCRD = ds.Tables("OCRD")
                Dim dr As DataRow = OCRD.Rows(0)
                Code = dr.Item("U_Ccode").ToString.Trim
                Name = dr.Item("U_Cname").ToString.Trim
            End If

            If Code = "" Then
                Code = "%"
            ElseIf Code.Contains("*") = True Then
                Code = Code.Replace("*", "%")
            Else
                Code = "%" & Code & "%"
            End If

            If Name = "" Then
                Name = "%"
            ElseIf Name.Contains("*") = True Then
                Name = Name.Replace("*", "%")
            Else
                Name = "%" & Name & "%"
            End If
            ''Dim Str As String = "SELECT T0.""CardCode"" As ""U_Ccode"", T0.""CardName"" as ""U_Cname"", T0.""Phone1"" as ""U_TelNo"", T0.""Fax"" as ""U_FaxNo"", T0.""Cellular"" as ""U_Mno"", T0.""E_Mail"" as ""U_Email"" FROM OCRD T0 WHERE IFNULL( T0.""U_WebAccess"" ,'') ='Y' and  T0.""CardType"" ='C' and  T0.""CardCode""  like '" & Code & "' and T0.""CardName"" like '" & Name & "'"
            Dim Str As String
            Str = "SELECT 'L' as ""U_Ctype"",T0.""Code"" As ""U_Ccode"", T0.""Name"" as ""U_Cname"", T0.""U_TelNo"" as ""U_TelNo"", T0.""U_FaxNo"" as ""U_FaxNo"", " & _
                  " T0.""U_MNo"" as ""U_Mno"", T0.""U_Email"" as ""U_Email"" FROM ""@LEADM""  T0 WHERE UPPER(T0.""Code"") like '" & Code.ToUpper() & "' and  UPPER(T0.""Name"") like '" & Name.ToUpper() & "' " & _
                  " UNION ALL " & _
                  " SELECT 'C' as ""U_Ctype"",T0.""CardCode"" As ""U_Ccode"", T0.""CardName"" as ""U_Cname"", T0.""Phone1"" as ""U_TelNo"", T0.""Fax"" as ""U_FaxNo"", " & _
                  " T0.""Cellular"" as ""U_Mno"", T0.""E_Mail"" as ""U_Email"" FROM OCRD T0 WHERE IFNULL( T0.""U_WebAccess"" ,'') ='Y' and  T0.""CardType"" ='C' " & _
                  " and  UPPER(T0.""CardCode"")  like '" & Code.ToUpper() & "' and UPPER(T0.""CardName"") like '" & Name.ToUpper() & "' order by ""U_Cname""  "

            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable
            Dim RetDT2 As New DataTable
            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "OCRD"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                Str = "SELECT T0.""CardCode"" as ""U_Ccode"", T0.""Address"" ""U_AddrN"", T0.""Street"" ""U_Addr1"", T0.""Block"" ""U_Addr2"", T0.""City"" ""U_Addr3"", T0.""County"" ""U_Addr4"", T0.""StreetNo"" ""U_Addr5"", T0.""GlblLocNum"" ""U_Addr6"" FROM CRD1 T0  INNER JOIN OCRD T1 ON T0.""CardCode"" = T1.""CardCode"" WHERE T0.""AdresType"" ='B' and T1.""CardType"" ='C' and  T1.""CardCode""  like '" & Code & "' and  T1.""CardName"" like '" & Name & "' Union All SELECT T0.""Code"" as ""U_Ccode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"" " & _
                      " FROM ""@LEADM""  T0 where UPPER(T0.""Code"")  like '" & Code.ToUpper() & "' and  UPPER(T0.""Name"") like '" & Name.ToUpper() & "'"

                RetDT = New DataTable
                RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
                If Errmsg <> "" Then
                    Throw New Exception(Errmsg)
                End If
                RetDT.TableName = "ADDR"
                RetDT2 = RetDT.Copy
                RetDS.Tables.Add(RetDT2)

                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub ContinentMaster()
        Try
            Dim Errmsg As String = ""
            Dim RetDS As New DataSet
            Dim Str As String = "SELECT T0.""U_Code"" As  ""U_Conti"", T0.""U_Name"" As  ""U_ContiName"" FROM ""@CONTINENT""  T0 ORDER BY T0.""U_Name"""
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable

            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "CONT"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub ChargeTypeMaster()
        Try
            Dim Errmsg As String = ""
            Dim RetDS As New DataSet
            Dim Str As String = "SELECT T0.""ItemCode"" ""U_Stype"", T0.""ItemName"" ""U_StypeName""  FROM OITM T0 WHERE T0.""ItmsGrpCod"" ='103' ORDER BY T0.""ItemName"""
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable

            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then

                RetDT.TableName = "CHRGETYPE"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub SurveyorIDMaster(ByVal value As String)
        sFunction = "SurveyorIDMaster"
        Try
            If PublicVariable.DEBUG_ON = 1 Then
                oLog.WriteToLogFile_Debug(value, sFunction)
            End If

            Dim sErrDesc As String = ""
            Dim SQLStr As String = ""
            Dim dt As New DataTable
            Dim SQLDT As New DataTable
            Dim RetDT As New DataTable
            Dim RetDS = New DataSet()
            Dim DType As String = ""
            Dim CompCode As String = ""
            Dim ds As DataSet = fn.jsontodata(value)
            If ds.Tables("SUID").Rows.Count > 0 Then
                dt = ds.Tables("SUID")
                Dim dr As DataRow = dt.Rows(0)
                Dim UserCode As String = dr("uid").ToString.Trim().ToUpper
                SQLStr = "SELECT T0.""U_DAuthor"" FROM ""@WUSER""  T0 WHERE Upper(T0.""Code"") ='" & UserCode & "'"
                DType = fn.ExecuteSQLQuery_SingleValue(SQLStr, sErrDesc)
                If sErrDesc <> "" Then
                    Throw New Exception(sErrDesc)
                ElseIf DType = "" Then
                    Throw New Exception("No Surveyor ID Mapping Found for this user")
                End If
                If DType = "By Company" Then
                    SQLStr = "SELECT ifnull(T0.""U_ComCode"",'')  As ""U_ComCode"" FROM ""@WUSER""  T0 WHERE T0.""Code"" ='" & UserCode & "'"
                    CompCode = fn.ExecuteSQLQuery_SingleValue(SQLStr, sErrDesc)
                    If sErrDesc <> "" Then
                        Throw New Exception(sErrDesc)
                    ElseIf CompCode = "" Then
                        Throw New Exception("No Surveyor ID Mapping Found for this user")
                    End If
                ElseIf DType = "By Country" Then
                    Throw New Exception("No Surveyor ID Mapping Found for this user")
                Else
                    Throw New Exception("No Surveyor ID Mapping Found for this user")
                End If
                SQLStr = "SELECT T1.""U_SCode"", T1.""U_SName"" FROM  ""@SURVEYORID""  T0 , ""@SURVEYORIDL""  T1 WHERE T1.""Code"" = T0.""Code"" and  T0.""Code"" ='" & CompCode & "'"
                SQLDT = fn.ExecuteSQLQuery(SQLStr, sErrDesc)
                If sErrDesc <> "" Then
                    Throw New Exception(sErrDesc)
                ElseIf SQLDT.Rows.Count = 0 Then
                    Throw New Exception("No Surveyor ID Mapping Found for this user")
                Else
                    RetDT = New DataTable
                    SQLDT.TableName = "SUID"
                    RetDT = SQLDT.Copy()
                    RetDS.Tables.Add(RetDT)
                End If
                Context.Response.Output.Write(fn.ds2json(RetDS))
            End If

        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub SurveyorIDMaster_full()
        sFunction = "SurveyorIDMaster"
        Try
            If PublicVariable.DEBUG_ON = 1 Then
                '  oLog.WriteToLogFile_Debug(value, sFunction)
            End If

            Dim sErrDesc As String = ""
            Dim SQLStr As String = ""
            Dim dt As New DataTable
            Dim SQLDT As New DataTable
            Dim RetDT As New DataTable
            Dim RetDS = New DataSet()
            Dim DType As String = ""
            Dim CompCode As String = ""
            ' Dim ds As DataSet = fn.jsontodata(value)
            'If ds.Tables("SUID").Rows.Count > 0 Then
            'dt = ds.Tables("SUID")
            'Dim dr As DataRow = dt.Rows(0)
            'Dim UserCode As String = dr("uid").ToString.Trim().ToUpper
            'SQLStr = "SELECT T0.""U_DAuthor"" FROM ""@WUSER""  T0 WHERE Upper(T0.""Code"") ='" & UserCode & "'"
            'DType = fn.ExecuteSQLQuery_SingleValue(SQLStr, sErrDesc)
            'If sErrDesc <> "" Then
            '    Throw New Exception(sErrDesc)
            'ElseIf DType = "" Then
            '    Throw New Exception("No Surveyor ID Mapping Found for this user")
            'End If
            'If DType = "By Company" Then
            '    SQLStr = "SELECT ifnull(T0.""U_ComCode"",'')  As ""U_ComCode"" FROM ""@WUSER""  T0 WHERE T0.""Code"" ='" & UserCode & "'"
            '    CompCode = fn.ExecuteSQLQuery_SingleValue(SQLStr, sErrDesc)
            '    If sErrDesc <> "" Then
            '        Throw New Exception(sErrDesc)
            '    ElseIf CompCode = "" Then
            '        Throw New Exception("No Surveyor ID Mapping Found for this user")
            '    End If
            'ElseIf DType = "By Country" Then
            '    Throw New Exception("No Surveyor ID Mapping Found for this user")
            'Else
            '    Throw New Exception("No Surveyor ID Mapping Found for this user")
            'End If
            SQLStr = "SELECT T1.""U_SCode"", T1.""U_SName"" FROM  ""@SURVEYORID""  T0 , ""@SURVEYORIDL""  T1 WHERE T1.""Code"" = T0.""Code"""
            SQLDT = fn.ExecuteSQLQuery(SQLStr, sErrDesc)
            If sErrDesc <> "" Then
                Throw New Exception(sErrDesc)
            ElseIf SQLDT.Rows.Count = 0 Then
                Throw New Exception("No Surveyor ID Mapping Found for this user")
            Else
                RetDT = New DataTable
                SQLDT.TableName = "SUID"
                RetDT = SQLDT.Copy()
                RetDS.Tables.Add(RetDT)
            End If
            Context.Response.Output.Write(fn.ds2json(RetDS))
            'End If

        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub SurveyTypeMaster()
        Try
            Dim Errmsg As String = ""
            Dim RetDS As New DataSet
            Dim Str As String = "SELECT T0.""ItemCode"" ""U_Stype"", T0.""ItemName"" ""U_StypeName""  FROM OITM T0 WHERE T0.""ItmsGrpCod"" ='101' and ifnull(T0.""frozenFor"",'N') ='N' ORDER BY T0.""ItemName"""
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable

            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "SURTYPE"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub CountryMaster(ByVal value As String)
        Try

            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""
            Dim ContCode As String = ""
            Dim CONT As New DataTable

            If ds.Tables("CONT").Rows.Count > 0 Then
                CONT = ds.Tables("CONT")
                Dim dr As DataRow = CONT.Rows(0)
                ContCode = dr.Item("U_Conti").ToString.Trim

            End If


            Dim RetDS As New DataSet
            Dim Str As String = "SELECT T0.""U_CCode"" As ""U_Country"", T0.""U_CName"" As ""U_CountryName"" FROM ""@COUNTRY""  T0 WHERE T0.""U_ConCode"" ='" & ContCode & "' ORDER BY T0.""U_CName"""
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable

            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "COUNTRY"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub CountryMaster_full()
        Try


            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""
            Dim ContCode As String = ""
            Dim CONT As New DataTable



            Dim RetDS As New DataSet
            Dim Str As String = "SELECT T0.""U_CCode"" As ""U_Country"", T0.""U_CName"" As ""U_CountryName"" FROM ""@COUNTRY""  T0  ORDER BY T0.""U_CName"""
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable

            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "COUNTRY"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub LocationMaster(ByVal value As String)
        Try

            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""
            Dim City As String = ""
            Dim CONT As New DataTable

            If ds.Tables("CITY").Rows.Count > 0 Then
                CONT = ds.Tables("CITY")
                Dim dr As DataRow = CONT.Rows(0)
                City = dr.Item("U_City").ToString.Trim

            End If


            Dim RetDS As New DataSet
            Dim Str As String = "SELECT T0.""U_LCode"" ""U_Loc"", T0.""U_LName""  ""U_LocName""FROM ""@LOCATION""  T0 WHERE T0.""U_CityCode"" ='" & City & "' ORDER BY T0.""U_LCode"""
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable

            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "LOC"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub LocationMaster_full()
        Try

            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""

            Dim RetDS As New DataSet
            Dim Str As String = "SELECT T0.""U_LCode"" ""U_Loc"", T0.""U_LName""  ""U_LocName""FROM ""@LOCATION""  T0 ORDER BY T0.""U_LCode"""
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable

            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "LOC"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub CityMaster(ByVal value As String)
        Try

            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""
            Dim CounCode As String = ""
            Dim CONT As New DataTable

            If ds.Tables("COUNTRY").Rows.Count > 0 Then
                CONT = ds.Tables("COUNTRY")
                Dim dr As DataRow = CONT.Rows(0)
                CounCode = dr.Item("U_Country").ToString.Trim

            End If


            Dim RetDS As New DataSet
            Dim Str As String = "SELECT T0.""U_CCode"" ""U_City"", T0.""U_CName""  ""U_CityName""FROM ""@CITY""  T0 WHERE T0.""U_ContCode"" ='" & CounCode & "' ORDER BY T0.""U_CName"""
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable

            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "CITY"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub CityMaster_full()
        Try


            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""
            Dim CounCode As String = ""
            Dim CONT As New DataTable


            Dim RetDS As New DataSet
            Dim Str As String = "SELECT T0.""U_CCode"" ""U_City"", T0.""U_CName""  ""U_CityName""FROM ""@CITY""  T0  ORDER BY T0.""U_CName"""
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable

            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "CITY"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub EquipmentGroupMaster()
        Try


            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""

            Dim RetDS As New DataSet
            Dim Str As String = "SELECT T0.""U_EQGRPCODE"" ""U_EQGroup"", T0.""U_EQGRPNAME"" ""U_EQGroupName"" FROM ""@EQGRP""  T0 ORDER BY T0.""U_EQGRPNAME"""
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable

            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "EQGROUP"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub CurrencyMaster()
        Try

            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""

            Dim RetDS As New DataSet
            Dim Str As String = "SELECT T0.""CurrCode"" As ""U_Currency"", T0.""CurrCode"" As ""U_CurrencyName"" FROM OCRN T0 ORDER BY T0.""CurrName"""
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable

            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "CURRENCY"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub UOMMaster()
        Try

            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""

            Dim RetDS As New DataSet
            Dim Str As String = "SELECT T0.""U_UOMCODE"" As ""U_UOM"", T0.""U_UOMNAME"" As ""U_UOMName"" FROM ""@UOM""  T0 ORDER BY T0.""U_UOMNAME"""
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable

            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "UOM"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub GSTMaster_OutPut()
        Try

            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""

            Dim RetDS As New DataSet
            Dim Str As String = "SELECT T0.""Code"" As ""U_GST"", T0.""Code"" || '- '  || CAST(T0.""Rate"" AS decimal(36, 2)) As ""U_GSTName"" FROM OVTG T0 where ""Inactive""='N' and T0.""Category""='O'"
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable

            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "GST"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub Approval_Status_Master()
        Try

            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""

            Dim RetDS As New DataSet
            Dim Str As String = "SELECT T0.""Name"" FROM ""@APPR""  T0 ORDER BY T0.""Code"""
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable

            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "APPROVAL"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub Created_By()
        Try

            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""

            Dim RetDS As New DataSet
            Dim Str As String = "SELECT Distinct T1.""Name"" FROM ""@SQTO""  T0 LEFT JOIN ""@WUSER""  T1 on  T1.""Name"" = T0.""U_Uname"" WHERE T1.""U_Active"" ='Y'"
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable

            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "CREATEDBY"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub Survey_Criteria_Master()
        Try

            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""

            Dim RetDS As New DataSet
            Dim Str As String = "SELECT T0.""U_SURCRTACODE"", T0.""U_SURCRTANAME"" FROM ""@SUCRITERIA""  T0"
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable

            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "SURCRTA"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub Equipment_Type_Master()
        Try

            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""

            Dim RetDS As New DataSet
            Dim Str As String = "SELECT T0.""U_EQTYPECODE"",T0.""U_EQTYPENAME"" FROM ""@EQTYPE""  T0"
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable

            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "SURCRTA"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub Surveyor_Ex_Agent()
        Try


            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""
            Dim CounCode As String = ""
            Dim CONT As New DataTable


            Dim RetDS As New DataSet
            Dim Str As String = "SELECT T0.""U_ANAME"" FROM ""@SUEXAGENT""  T0  ORDER BY T0.""U_ANAME"""
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable

            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "EXAGENT"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub IMO_Master()
        Try

            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""

            Dim RetDS As New DataSet
            Dim Str As String = "SELECT T0.""Code"", T0.""Name"" FROM ""@IMO"" T0 ORDER BY T0.""Name"" "
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable

            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "IOM"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
#End Region
#Region "Sales Quoation"
    <WebMethod()> _
        <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub SalesQuotationAdd(ByVal value As String)
        sFunction = "SalesQuotationAdd"
        Try

            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim SQTO As New DataTable
            SQTO = New DataTable
            Dim SQTOGEN As New DataTable
            Dim SQTOADD As New DataTable
            Dim SOTOATTACH As New DataTable
            Dim SQTODS = New DataSet()
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            fn.GetSAPConnection(Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            Dim oGeneralService As SAPbobsCOM.GeneralService
            Dim oGeneralData As SAPbobsCOM.GeneralData
            Dim oSons As SAPbobsCOM.GeneralDataCollection
            Dim oSon As SAPbobsCOM.GeneralData
            Dim sCmp As SAPbobsCOM.CompanyService
            Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

            sCmp = PublicVariable.oCompany.GetCompanyService
            oGeneralService = sCmp.GetGeneralService("SQTO")
            oGeneralData = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)
            oGeneralParams = CType(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams), SAPbobsCOM.GeneralDataParams)
            ' oGeneralData = oGeneralService.GetByParams(oGeneralParams)


            If ds.Tables("SQTO").Rows.Count > 0 Then
                SQTO = ds.Tables("SQTO")
                Dim dr As DataRow = SQTO.Rows(0)
                ' Dim oRecord As SAPbobsCOM.Recordset
                'oRecord = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                'oRecord.DoQuery("select U_Qno from [@SQTO] where U_Qno='" & dr.Item("U_Qno") & "'")
                ' If oRecord.RecordCount = 0 Then
                'oGeneralData.SetProperty("U_Qno", "Open")
                oGeneralData.SetProperty("U_Status", "Open")
                oGeneralData.SetProperty("U_Uname", dr.Item("U_Uname").ToString.Trim())
                oGeneralData.SetProperty("U_Cdate", dr.Item("U_Cdate").ToString.Trim())
                oGeneralData.SetProperty("U_Qdate1", dr.Item("U_Qdate1").ToString.Trim())
                oGeneralData.SetProperty("U_Qdate2", dr.Item("U_Qdate2").ToString.Trim())
                oGeneralData.SetProperty("U_Ccode", dr.Item("U_Ccode").ToString.Trim())
                oGeneralData.SetProperty("U_Cname", dr.Item("U_Cname").ToString.Trim())
                oGeneralData.SetProperty("U_CPeriod1", dr.Item("U_CPeriod1").ToString.Trim())
                oGeneralData.SetProperty("U_CPeriod2", dr.Item("U_CPeriod2").ToString.Trim())
                oGeneralData.SetProperty("U_Pcode", dr.Item("U_Pcode").ToString.Trim())
                oGeneralData.SetProperty("U_AddrN", dr.Item("U_AddrN").ToString.Trim())
                oGeneralData.SetProperty("U_Addr1", dr.Item("U_Addr1").ToString.Trim())
                oGeneralData.SetProperty("U_Addr2", dr.Item("U_Addr2").ToString.Trim())
                oGeneralData.SetProperty("U_Addr3", dr.Item("U_Addr3").ToString.Trim())
                oGeneralData.SetProperty("U_Addr4", dr.Item("U_Addr4").ToString.Trim())
                oGeneralData.SetProperty("U_Addr5", dr.Item("U_Addr5").ToString.Trim())
                oGeneralData.SetProperty("U_Addr6", dr.Item("U_Addr6").ToString.Trim())
                Try
                    oGeneralData.SetProperty("U_TelNo", dr.Item("U_TelNo").ToString.Trim())
                    oGeneralData.SetProperty("U_FaxNo", dr.Item("U_FaxNo").ToString.Trim())
                    oGeneralData.SetProperty("U_Mno", dr.Item("U_Mno").ToString.Trim())
                Catch ex As Exception
                End Try
                oGeneralData.SetProperty("U_Email", dr.Item("U_Email").ToString.Trim())
                oGeneralData.SetProperty("U_Remarks", dr.Item("U_Remarks").ToString.Trim())
                If ds.Tables("SQTOGEN").Rows.Count > 0 Then
                    SQTOGEN = ds.Tables("SQTOGEN")
                    oSons = oGeneralData.Child("SQTOGENERAL")
                    For Each dr1 As DataRow In SQTOGEN.Rows
                        oSon = oSons.Add
                        If dr1.Item("U_Stype").ToString.Trim() = "" Then
                            Throw New Exception("The ""Survey Type"" field must not be empty; enter a value for ""Survey Type"".")
                        End If
                        oSon.SetProperty("U_Stype", dr1.Item("U_Stype").ToString.Trim())

                        If dr1.Item("U_Conti").ToString.Trim() = "" Then
                            Throw New Exception("The ""Continent"" field must not be empty; enter a value for ""Continent"".")
                        End If
                        oSon.SetProperty("U_Conti", dr1.Item("U_Conti").ToString.Trim())

                        If dr1.Item("U_Country").ToString.Trim() = "" Then
                            Throw New Exception("The ""Country"" field must not be empty; enter a value for ""Country"".")
                        End If
                        oSon.SetProperty("U_Country", dr1.Item("U_Country").ToString.Trim())

                        If dr1.Item("U_City").ToString.Trim() = "" Then
                            Throw New Exception("The ""City"" field must not be empty; enter a value for ""City"".")
                        End If
                        oSon.SetProperty("U_City", dr1.Item("U_City").ToString.Trim())

                        If dr1.Item("U_Currency").ToString.Trim() = "" Then
                            Throw New Exception("The ""Currency"" field must not be empty; enter a value for ""Currency"".")
                        End If
                        oSon.SetProperty("U_Currency", dr1.Item("U_Currency").ToString.Trim())

                        If dr1.Item("U_EQGroup").ToString.Trim() = "" Then
                            Throw New Exception("The ""Equipment Group"" field must not be empty; enter a value for ""Equipment Group"".")
                        End If
                        oSon.SetProperty("U_EQGroup", dr1.Item("U_EQGroup").ToString.Trim())
                        oSon.SetProperty("U_Rate", dr1.Item("U_Rate").ToString.Trim())
                        oSon.SetProperty("U_UOM", dr1.Item("U_UOM").ToString.Trim())
                        If dr1.Item("U_GST").ToString.Trim() = "" Then
                            Throw New Exception("The ""GST"" field must not be empty; enter a value for ""GST"".")
                        End If
                        oSon.SetProperty("U_GST", dr1.Item("U_GST").ToString.Trim())
                        oSon.SetProperty("U_Remarks", dr1.Item("U_Remarks").ToString.Trim())
                    Next
                Else
                    Throw New Exception("The ""General Tab"" must not be empty; enter a value for ""General Tab"".")
                End If

                If ds.Tables("ATTACHMENT").Rows.Count > 0 Then
                    SOTOATTACH = ds.Tables("ATTACHMENT")
                    oSons = oGeneralData.Child("SQTOATTACHMENT")
                    For Each odr As DataRow In SOTOATTACH.Rows
                        oSon = oSons.Add
                        Try
                            oSon.SetProperty("U_Path", odr.Item("U_FilePath").ToString.Trim())
                        Catch ex As Exception
                            Throw New Exception("Error while adding file path to attachment folder")
                        End Try
                        Try
                            oSon.SetProperty("U_Fname", odr.Item("U_FileName").ToString.Trim())
                        Catch ex As Exception
                            Throw New Exception("Error while adding file name to attachment folder")
                        End Try
                        Try
                            Dim dtAttachDate As Date
                            dtAttachDate = GetDateTimeValue(odr.Item("U_Date").ToString)
                            oSon.SetProperty("U_AttDate", dtAttachDate)
                        Catch ex As Exception
                            Throw New Exception("Error while adding attachment date to attachment folder")
                        End Try

                    Next
                End If

                If ds.Tables("SQTOADD").Rows.Count > 0 Then
                    SQTOADD = ds.Tables("SQTOADD")
                    oSons = oGeneralData.Child("SQTOADDON")
                    For Each dr1 As DataRow In SQTOADD.Rows
                        oSon = oSons.Add

                        If dr1.Item("U_Ctype").ToString.Trim() = "" Then
                            Throw New Exception("The ""Charge Type"" field must not be empty; enter a value for ""Charge Type"".")
                        End If
                        oSon.SetProperty("U_Ctype", dr1.Item("U_Ctype").ToString.Trim())

                        If dr1.Item("U_Continent").ToString.Trim() = "" Then
                            Throw New Exception("The ""Continent"" field must not be empty; enter a value for ""Continent"".")
                        End If
                        oSon.SetProperty("U_Continent", dr1.Item("U_Continent").ToString.Trim())

                        If dr1.Item("U_Country").ToString.Trim() = "" Then
                            Throw New Exception("The ""Country"" field must not be empty; enter a value for ""Country"".")
                        End If
                        oSon.SetProperty("U_Country", dr1.Item("U_Country").ToString.Trim())

                        If dr1.Item("U_City").ToString.Trim() = "" Then
                            Throw New Exception("The ""City"" field must not be empty; enter a value for ""City"".")
                        End If
                        oSon.SetProperty("U_City", dr1.Item("U_City").ToString.Trim())

                        If dr1.Item("U_Currency").ToString.Trim() = "" Then
                            Throw New Exception("The ""Currency"" field must not be empty; enter a value for ""Currency"".")
                        End If
                        oSon.SetProperty("U_Currency", dr1.Item("U_Currency").ToString.Trim())

                        If dr1.Item("U_EQGroup").ToString.Trim() = "" Then
                            Throw New Exception("The ""Equipment Group"" field must not be empty; enter a value for ""Equipment Group"".")
                        End If
                        oSon.SetProperty("U_EQGroup", dr1.Item("U_EQGroup").ToString.Trim())

                        oSon.SetProperty("U_Rate", dr1.Item("U_Rate").ToString.Trim())
                        oSon.SetProperty("U_UOM", dr1.Item("U_UOM").ToString.Trim())

                        If dr1.Item("U_GST").ToString.Trim() = "" Then
                            Throw New Exception("The ""GST"" field must not be empty; enter a value for ""GST"".")
                        End If
                        oSon.SetProperty("U_GST", dr1.Item("U_GST").ToString.Trim())
                        oSon.SetProperty("U_Remarks", dr1.Item("U_Remarks").ToString.Trim())
                    Next
                End If

                Dim response = oGeneralService.Add(oGeneralData)
                DocEntry = response.GetProperty("DocEntry")
            End If

            Dim RetDT As New DataTable
            RetDT.TableName = "VALIDATE"
            RetDT.Columns.Add("Status")
            RetDT.Columns.Add("Msg")
            RetDT.Rows.Add()
            RetDT.Rows(0)(0) = True
            RetDT.Rows(0)(1) = "Sales Quotation no. " & DocEntry.ToString & " Created Successfully"
            RetDS.Tables.Add(RetDT)
            Context.Response.Output.Write(fn.ds2json(RetDS))


        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
        <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub SalesQuotation_Update(ByVal value As String)
        sFunction = "SalesQuotation_Update"
        Try

            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim SQTO As New DataTable
            SQTO = New DataTable
            Dim SQTOGEN As New DataTable
            Dim SQTOADD As New DataTable
            Dim SOTOATTACH As New DataTable
            Dim SQTODS = New DataSet()
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            fn.GetSAPConnection(Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            Dim oGeneralService As SAPbobsCOM.GeneralService
            Dim oGeneralData As SAPbobsCOM.GeneralData
            Dim oSons As SAPbobsCOM.GeneralDataCollection
            Dim oSon As SAPbobsCOM.GeneralData
            Dim sCmp As SAPbobsCOM.CompanyService
            Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

            sCmp = PublicVariable.oCompany.GetCompanyService
            oGeneralService = sCmp.GetGeneralService("SQTO")
            oGeneralData = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)
            oGeneralParams = CType(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams), SAPbobsCOM.GeneralDataParams)
            ' oGeneralData = oGeneralService.GetByParams(oGeneralParams)
            '  Dim RetDT As New DataTable
            ' RetDT = fn.ExecuteSQLQuery(Query, Errmsg)
            Dim Count_Gen As String = ""
            Dim Count_Addon As String = ""
            Dim Count_Attach As String = ""
            Dim Count_Gen_Int As Integer = 0
            Dim Count_Addon_Int As Integer = 0
            Dim Count_Attach_Int As Integer = 0
            Dim Query As String = ""


            If ds.Tables("SQTO").Rows.Count > 0 Then
                SQTO = ds.Tables("SQTO")
                Dim dr As DataRow = SQTO.Rows(0)


                DocEntry = dr.Item("U_Qno").ToString.Trim()
                Query = "SELECT COunt(T0.""DocEntry"") FROM ""@SQTOGENERAL""  T0 WHERE T0.""DocEntry"" =" & DocEntry & ""
                Count_Gen = fn.ExecuteSQLQuery_SingleValue(Query, Errmsg)
                If Errmsg <> "" Then
                    Throw New Exception(Errmsg)
                End If
                If Count_Gen <> "" Then
                    Count_Gen_Int = CInt(Count_Gen)
                End If
                Query = "SELECT COunt(T0.""DocEntry"") FROM ""@SQTOADDON""  T0 WHERE T0.""DocEntry"" =" & DocEntry & ""
                Count_Addon = fn.ExecuteSQLQuery_SingleValue(Query, Errmsg)
                If Errmsg <> "" Then
                    Throw New Exception(Errmsg)
                End If
                If Count_Addon <> "" Then
                    Count_Addon_Int = CInt(Count_Addon)
                End If
                Query = "SELECT COUNT(T0.""DocEntry"") FROM ""@SQTOATTACHMENT"" T0 WHERE T0.""DocEntry"" = " & DocEntry & " "
                Count_Attach = fn.ExecuteSQLQuery_SingleValue(Query, Errmsg)
                If Errmsg <> "" Then
                    Throw New Exception(Errmsg)
                End If
                If Count_Attach <> "" Then
                    Count_Attach_Int = CInt(Count_Attach)
                End If

                oGeneralParams.SetProperty("DocEntry", dr.Item("U_Qno").ToString.Trim())
                oGeneralData = oGeneralService.GetByParams(oGeneralParams)

                'oGeneralData.SetProperty("U_Qno", dr.Item("U_Qno"))
                '  oGeneralData.SetProperty("U_Status", dr.Item("U_Status").ToString.Trim())
                oGeneralData.SetProperty("U_Uname", dr.Item("U_Uname").ToString.Trim())
                oGeneralData.SetProperty("U_Cdate", dr.Item("U_Cdate").ToString.Trim())
                oGeneralData.SetProperty("U_Qdate1", dr.Item("U_Qdate1").ToString.Trim())
                oGeneralData.SetProperty("U_Qdate2", dr.Item("U_Qdate2").ToString.Trim())
                oGeneralData.SetProperty("U_Ccode", dr.Item("U_Ccode").ToString.Trim())
                oGeneralData.SetProperty("U_Cname", dr.Item("U_Cname").ToString.Trim())
                oGeneralData.SetProperty("U_CPeriod1", dr.Item("U_CPeriod1").ToString.Trim())
                oGeneralData.SetProperty("U_CPeriod2", dr.Item("U_CPeriod2").ToString.Trim())
                oGeneralData.SetProperty("U_Pcode", dr.Item("U_Pcode").ToString.Trim())
                oGeneralData.SetProperty("U_AddrN", dr.Item("U_AddrN").ToString.Trim())
                oGeneralData.SetProperty("U_Addr1", dr.Item("U_Addr1").ToString.Trim())
                oGeneralData.SetProperty("U_Addr2", dr.Item("U_Addr2").ToString.Trim())
                oGeneralData.SetProperty("U_Addr3", dr.Item("U_Addr3").ToString.Trim())
                oGeneralData.SetProperty("U_Addr4", dr.Item("U_Addr4").ToString.Trim())
                oGeneralData.SetProperty("U_Addr5", dr.Item("U_Addr5").ToString.Trim())
                oGeneralData.SetProperty("U_Addr6", dr.Item("U_Addr6").ToString.Trim())
                Try
                    oGeneralData.SetProperty("U_TelNo", dr.Item("U_TelNo").ToString.Trim())
                    oGeneralData.SetProperty("U_FaxNo", dr.Item("U_FaxNo").ToString.Trim())
                    oGeneralData.SetProperty("U_Mno", dr.Item("U_Mno").ToString.Trim())
                Catch ex As Exception
                End Try

                'oGeneralData.SetProperty("U_TelNo", dr.Item("U_TelNo").ToString.Trim())
                'oGeneralData.SetProperty("U_FaxNo", dr.Item("U_FaxNo").ToString.Trim())
                'oGeneralData.SetProperty("U_Mno", dr.Item("U_Mno").ToString.Trim())
                oGeneralData.SetProperty("U_Email", dr.Item("U_Email").ToString.Trim())
                oGeneralData.SetProperty("U_Remarks", dr.Item("U_Remarks").ToString.Trim())

                Dim LineNum As Integer = 0
                Dim SQTOAATTACHNnewLineNo As Integer = 0
                Dim LoopRow As Integer = 0
                Dim RemoveIndex As Integer = 0

                If ds.Tables("ATTACHMENT").Rows.Count > 0 Then
                    SOTOATTACH = ds.Tables("ATTACHMENT")
                    oSons = oGeneralData.Child("SQTOATTACHMENT")

                    SQTOAATTACHNnewLineNo = SOTOATTACH.Rows.Count()
                    If SOTOATTACH.Rows.Count > Count_Attach_Int Then
                        LoopRow = SOTOATTACH.Rows.Count
                    Else
                        LoopRow = Count_Attach_Int
                    End If
                    Dim i As Integer = 0
                    RemoveIndex = (Count_Attach_Int - 1)
                    For i = 0 To LoopRow - 1
                        '                        For Each dr1 As DataRow In SQTOADD.Rows
                        LineNum = LineNum + 1
                        If SQTOAATTACHNnewLineNo = Count_Attach_Int Then
                            oSons.Item(LineNum - 1).SetProperty("U_Path", SOTOATTACH.Rows(i).Item("U_FilePath").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Fname", SOTOATTACH.Rows(i).Item("U_FileName").ToString.Trim())

                            Dim dtAttachDate As Date
                            dtAttachDate = GetDateTimeValue(SOTOATTACH.Rows(i).Item("U_Date").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_AttDate", dtAttachDate)

                        ElseIf SQTOAATTACHNnewLineNo > Count_Attach_Int Then
                            If LineNum <= Count_Attach_Int Then
                                oSons.Item(LineNum - 1).SetProperty("U_Path", SOTOATTACH.Rows(i).Item("U_FilePath").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Fname", SOTOATTACH.Rows(i).Item("U_FileName").ToString.Trim())

                                Dim dtAttachDate As Date
                                dtAttachDate = GetDateTimeValue(SOTOATTACH.Rows(i).Item("U_Date").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_AttDate", dtAttachDate)
                            Else
                                oSon = oSons.Add
                                oSon.SetProperty("U_Path", SOTOATTACH.Rows(i).Item("U_FilePath").ToString.Trim())
                                oSon.SetProperty("U_Fname", SOTOATTACH.Rows(i).Item("U_FileName").ToString.Trim())

                                Dim dtAttachDate As Date
                                dtAttachDate = GetDateTimeValue(SOTOATTACH.Rows(i).Item("U_Date").ToString.Trim())
                                oSon.SetProperty("U_AttDate", dtAttachDate)

                            End If
                        ElseIf SQTOAATTACHNnewLineNo < Count_Attach_Int Then
                            If LineNum <= SQTOAATTACHNnewLineNo Then
                                oSons.Item(LineNum - 1).SetProperty("U_Path", SOTOATTACH.Rows(i).Item("U_FilePath").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Fname", SOTOATTACH.Rows(i).Item("U_FileName").ToString.Trim())
                                Dim dtAttachDate As Date
                                dtAttachDate = GetDateTimeValue(SOTOATTACH.Rows(i).Item("U_Date").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_AttDate", dtAttachDate)
                            Else
                                oSons.Remove(RemoveIndex - 1)
                                RemoveIndex = RemoveIndex - 1
                            End If
                        End If
                        'Next
                    Next
                End If

                LineNum = 0
                Dim SQTOGENnewLineNo As Integer = 0
                LoopRow = 0
                RemoveIndex = 0

                If ds.Tables("SQTOGEN").Rows.Count > 0 Then
                    SQTOGEN = ds.Tables("SQTOGEN")
                    oSons = oGeneralData.Child("SQTOGENERAL")
                    SQTOGENnewLineNo = SQTOGEN.Rows.Count()

                    If SQTOGEN.Rows.Count > Count_Gen_Int Then
                        LoopRow = SQTOGEN.Rows.Count
                    Else
                        LoopRow = Count_Gen_Int
                    End If
                    Dim i As Integer = 0
                    RemoveIndex = (Count_Gen_Int - 1)
                    For i = 0 To LoopRow - 1
                        'For Each dr1 As DataRow In SQTOGEN.Rows
                        LineNum = LineNum + 1
                        If SQTOGENnewLineNo = Count_Gen_Int Then
                            ' oSon = oSons.Add
                            oSons.Item(LineNum - 1).SetProperty("U_Stype", SQTOGEN.Rows(i).Item("U_Stype").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Conti", SQTOGEN.Rows(i).Item("U_Conti").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Country", SQTOGEN.Rows(i).Item("U_Country").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_City", SQTOGEN.Rows(i).Item("U_City").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Currency", SQTOGEN.Rows(i).Item("U_Currency").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_EQGroup", SQTOGEN.Rows(i).Item("U_EQGroup").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Rate", SQTOGEN.Rows(i).Item("U_Rate").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_UOM", SQTOGEN.Rows(i).Item("U_UOM").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_GST", SQTOGEN.Rows(i).Item("U_GST").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Remarks", SQTOGEN.Rows(i).Item("U_Remarks").ToString.Trim())
                        ElseIf SQTOGENnewLineNo > Count_Gen_Int Then
                            If LineNum <= Count_Gen_Int Then
                                oSons.Item(LineNum - 1).SetProperty("U_Stype", SQTOGEN.Rows(i).Item("U_Stype").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Conti", SQTOGEN.Rows(i).Item("U_Conti").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Country", SQTOGEN.Rows(i).Item("U_Country").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_City", SQTOGEN.Rows(i).Item("U_City").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Currency", SQTOGEN.Rows(i).Item("U_Currency").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_EQGroup", SQTOGEN.Rows(i).Item("U_EQGroup").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Rate", SQTOGEN.Rows(i).Item("U_Rate").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_UOM", SQTOGEN.Rows(i).Item("U_UOM").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_GST", SQTOGEN.Rows(i).Item("U_GST").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Remarks", SQTOGEN.Rows(i).Item("U_Remarks").ToString.Trim())
                            Else
                                oSon = oSons.Add
                                oSon.SetProperty("U_Stype", SQTOGEN.Rows(i).Item("U_Stype").ToString.Trim())
                                oSon.SetProperty("U_Conti", SQTOGEN.Rows(i).Item("U_Conti").ToString.Trim())
                                oSon.SetProperty("U_Country", SQTOGEN.Rows(i).Item("U_Country").ToString.Trim())
                                oSon.SetProperty("U_City", SQTOGEN.Rows(i).Item("U_City").ToString.Trim())
                                oSon.SetProperty("U_Currency", SQTOGEN.Rows(i).Item("U_Currency").ToString.Trim())
                                oSon.SetProperty("U_EQGroup", SQTOGEN.Rows(i).Item("U_EQGroup").ToString.Trim())
                                oSon.SetProperty("U_Rate", SQTOGEN.Rows(i).Item("U_Rate").ToString.Trim())
                                oSon.SetProperty("U_UOM", SQTOGEN.Rows(i).Item("U_UOM").ToString.Trim())
                                oSon.SetProperty("U_GST", SQTOGEN.Rows(i).Item("U_GST").ToString.Trim())
                                oSon.SetProperty("U_Remarks", SQTOGEN.Rows(i).Item("U_Remarks").ToString.Trim())
                            End If
                        ElseIf SQTOGENnewLineNo < Count_Gen_Int Then
                            If LineNum <= SQTOGENnewLineNo Then
                                oSons.Item(LineNum - 1).SetProperty("U_Stype", SQTOGEN.Rows(i).Item("U_Stype").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Conti", SQTOGEN.Rows(i).Item("U_Conti").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Country", SQTOGEN.Rows(i).Item("U_Country").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_City", SQTOGEN.Rows(i).Item("U_City").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Currency", SQTOGEN.Rows(i).Item("U_Currency").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_EQGroup", SQTOGEN.Rows(i).Item("U_EQGroup").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Rate", SQTOGEN.Rows(i).Item("U_Rate").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_UOM", SQTOGEN.Rows(i).Item("U_UOM").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_GST", SQTOGEN.Rows(i).Item("U_GST").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Remarks", SQTOGEN.Rows(i).Item("U_Remarks").ToString.Trim())
                            Else
                                oSons.Remove(RemoveIndex - 1)
                                RemoveIndex = RemoveIndex - 1
                            End If

                        End If
                        'Next
                    Next
                End If
                LineNum = 0
                Dim SQTOADDNnewLineNo As Integer = 0
                LoopRow = 0
                RemoveIndex = 0

                If ds.Tables("SQTOADD").Rows.Count > 0 Then
                    SQTOADD = ds.Tables("SQTOADD")
                    oSons = oGeneralData.Child("SQTOADDON")

                    SQTOADDNnewLineNo = SQTOADD.Rows.Count()
                    If SQTOADD.Rows.Count > Count_Addon_Int Then
                        LoopRow = SQTOADD.Rows.Count
                    Else
                        LoopRow = Count_Addon_Int
                    End If
                    Dim i As Integer = 0
                    RemoveIndex = (Count_Addon_Int - 1)
                    For i = 0 To LoopRow - 1
                        '                        For Each dr1 As DataRow In SQTOADD.Rows
                        LineNum = LineNum + 1
                        If SQTOADDNnewLineNo = Count_Addon_Int Then
                            'oSon = oSons.Add
                            'oSon.Item(LineNum - 1).SetProperty("U_Ctype", SQTOADD.Rows(i).Item("U_Ctype"))
                            oSons.Item(LineNum - 1).SetProperty("U_Ctype", SQTOADD.Rows(i).Item("U_Ctype").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Continent", SQTOADD.Rows(i).Item("U_Continent").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Country", SQTOADD.Rows(i).Item("U_Country").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_City", SQTOADD.Rows(i).Item("U_City"))
                            oSons.Item(LineNum - 1).SetProperty("U_Currency", SQTOADD.Rows(i).Item("U_Currency").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_EQGroup", SQTOADD.Rows(i).Item("U_EQGroup").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Rate", SQTOADD.Rows(i).Item("U_Rate").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_UOM", SQTOADD.Rows(i).Item("U_UOM").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_GST", SQTOADD.Rows(i).Item("U_GST").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Remarks", SQTOADD.Rows(i).Item("U_Remarks").ToString.Trim())

                        ElseIf SQTOADDNnewLineNo > Count_Addon_Int Then
                            If LineNum <= Count_Addon_Int Then
                                oSons.Item(LineNum - 1).SetProperty("U_Ctype", SQTOADD.Rows(i).Item("U_Ctype").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Continent", SQTOADD.Rows(i).Item("U_Continent").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Country", SQTOADD.Rows(i).Item("U_Country").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_City", SQTOADD.Rows(i).Item("U_City"))
                                oSons.Item(LineNum - 1).SetProperty("U_Currency", SQTOADD.Rows(i).Item("U_Currency").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_EQGroup", SQTOADD.Rows(i).Item("U_EQGroup").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Rate", SQTOADD.Rows(i).Item("U_Rate").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_UOM", SQTOADD.Rows(i).Item("U_UOM").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_GST", SQTOADD.Rows(i).Item("U_GST").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Remarks", SQTOADD.Rows(i).Item("U_Remarks").ToString.Trim())
                            Else
                                oSon = oSons.Add
                                oSon.SetProperty("U_Ctype", SQTOADD.Rows(i).Item("U_Ctype").ToString.Trim())
                                oSon.SetProperty("U_Continent", SQTOADD.Rows(i).Item("U_Continent").ToString.Trim())
                                oSon.SetProperty("U_Country", SQTOADD.Rows(i).Item("U_Country").ToString.Trim())
                                oSon.SetProperty("U_City", SQTOADD.Rows(i).Item("U_City").ToString.Trim())
                                oSon.SetProperty("U_Currency", SQTOADD.Rows(i).Item("U_Currency").ToString.Trim())
                                oSon.SetProperty("U_EQGroup", SQTOADD.Rows(i).Item("U_EQGroup").ToString.Trim())
                                oSon.SetProperty("U_Rate", SQTOADD.Rows(i).Item("U_Rate").ToString.Trim())
                                oSon.SetProperty("U_UOM", SQTOADD.Rows(i).Item("U_UOM").ToString.Trim())
                                oSon.SetProperty("U_GST", SQTOADD.Rows(i).Item("U_GST").ToString.Trim())
                                oSon.SetProperty("U_Remarks", SQTOADD.Rows(i).Item("U_Remarks").ToString.Trim())
                            End If
                        ElseIf SQTOADDNnewLineNo < Count_Addon_Int Then
                            If LineNum <= SQTOADDNnewLineNo Then
                                oSons.Item(LineNum - 1).SetProperty("U_Ctype", SQTOADD.Rows(i).Item("U_Ctype").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Continent", SQTOADD.Rows(i).Item("U_Continent").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Country", SQTOADD.Rows(i).Item("U_Country").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_City", SQTOADD.Rows(i).Item("U_City"))
                                oSons.Item(LineNum - 1).SetProperty("U_Currency", SQTOADD.Rows(i).Item("U_Currency").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_EQGroup", SQTOADD.Rows(i).Item("U_EQGroup").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Rate", SQTOADD.Rows(i).Item("U_Rate").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_UOM", SQTOADD.Rows(i).Item("U_UOM").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_GST", SQTOADD.Rows(i).Item("U_GST").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Remarks", SQTOADD.Rows(i).Item("U_Remarks").ToString.Trim())
                            Else
                                oSons.Remove(RemoveIndex - 1)
                                RemoveIndex = RemoveIndex - 1
                            End If
                        End If
                        'Next
                    Next
                End If

                oGeneralService.Update(oGeneralData)
                DocEntry = dr.Item("U_Qno")
            End If

            Dim RetDT As New DataTable
            RetDT.TableName = "VALIDATE"
            RetDT.Columns.Add("Status")
            RetDT.Columns.Add("Msg")
            RetDT.Rows.Add()
            RetDT.Rows(0)(0) = True
            RetDT.Rows(0)(1) = "Sales Quotation no. " & DocEntry.ToString & " Updated Successfully"
            RetDS.Tables.Add(RetDT)
            Context.Response.Output.Write(fn.ds2json(RetDS))

            '-------------
            'Dim orec As SAPbobsCOM.Recordset
            'orec = ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            'Dim sSQL As String = "SELECT Idx FROM ( " & vbCrLf
            'sSQL &= "SELECT CONVERT(INT, ROW_NUMBER() OVER (ORDER BY VisOrder))-1 Idx, LineId, VisOrder FROM [@TI_SAMPLE_R] WHERE DocEntry = 58 " & vbCrLf
            'sSQL &= ") X WHERE X.LineId IN (2,5,6) ORDER BY Idx DESC  "
            'orec.DoQuery(sSQL)
            'oChildren = oGeneralData.Child("TI_SAMPLE_R")
            'While (orec.EoF) = False
            '    Dim remve As Integer = orec.Fields.Item(0).Value
            '    oChildren.Remove(remve)
            '    orec.MoveNext()
            'End While
            'oGeneralService.Update(oGeneralData)
            '--------------

        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
  <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub SalesQuotation_FirstRecord()
        sFunction = "SalesQuotation_FirstRecord"
        Try

            Dim Query As String
            Query = "SELECT Top 1 T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", " & _
                    " TO_CHAR( T0.""U_Qdate1"" , 'DD/MM/YYYY') As ""U_Qdate1"", TO_CHAR( T0.""U_Qdate2"" , 'DD/MM/YYYY') As ""U_Qdate2"", T0.""U_Ccode"", T0.""U_Cname"", " & _
                    " TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", " & _
                    " T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", " & _
                    " T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"", T0.""U_ARemarks"" FROM ""@SQTO""  T0 ORDER BY T0.""DocEntry"" ASC"
            Dim ErrMsg As String = ""
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable
            Dim RetDT2 As New DataTable
            Dim RetDT3 As New DataTable
            Dim RetDT4 As New DataTable

            Dim RetDS = New DataSet()
            RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
            If ErrMsg <> "" Then
                Throw New Exception(ErrMsg)
            End If

            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "SQTO"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)

                RetDT = New DataTable

                Query = "SELECT T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", T1.""U_Rate"", T1.""U_UOM"", T1.""U_GST"", " & _
                        " T1.""U_Remarks"" FROM ""@SQTOGENERAL""  T1 WHERE T1.""DocEntry"" = (SELECT Top 1 T0.""DocEntry""  FROM ""@SQTO""  T0 ORDER BY T0.""DocEntry"" ASC)"
                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                RetDT.TableName = "SQTOGEN"
                RetDT2 = RetDT.Copy
                RetDS.Tables.Add(RetDT2)


                RetDT = New DataTable
                Query = "SELECT T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", T0.""U_Rate"", T0.""U_UOM"", " & _
                        " T0.""U_GST"", T0.""U_Remarks"" FROM ""@SQTOADDON""  T0 WHERE T0.""DocEntry""  = (SELECT Top 1 T0.""DocNum""  FROM ""@SQTO""  T0 ORDER BY T0.""DocEntry"" ASC)"
                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                RetDT.TableName = "SQTOADD"
                RetDT3 = RetDT.Copy
                RetDS.Tables.Add(RetDT3)

                RetDT = New DataTable
                Query = "SELECT ""U_Fname"" AS ""U_FileName"",""U_Path"" AS ""U_FilePath"",""LineId"" AS ""U_id"", TO_CHAR(""U_AttDate"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                        " FROM ""@SQTOATTACHMENT"" WHERE ""DocEntry""  = (SELECT Top 1 T0.""DocNum""  FROM ""@SQTO""  T0 ORDER BY T0.""DocEntry"" ASC) "
                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                RetDT.TableName = "ATTACHMENT"
                RetDT4 = RetDT.Copy
                RetDS.Tables.Add(RetDT4)


                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                RetDT = New DataTable
                RetDT.TableName = "VALIDATE"
                RetDT.Columns.Add("Status")
                RetDT.Columns.Add("Msg")
                RetDT.Rows.Add()
                RetDT.Rows(0)(0) = True
                RetDT.Rows(0)(1) = "No Record Found"
                RetDS.Tables.Add(RetDT)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            End If
        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
  <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub SalesQuotation_LastRecord()
        sFunction = "SalesQuotation_LastRecord"
        Try
            Dim Query As String
            Query = "SELECT Top 1 T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", " & _
                    " TO_CHAR( T0.""U_Qdate1"" , 'DD/MM/YYYY') As ""U_Qdate1"", TO_CHAR( T0.""U_Qdate2"" , 'DD/MM/YYYY') As ""U_Qdate2"", T0.""U_Ccode"", T0.""U_Cname"", " & _
                    " TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", " & _
                    " T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", " & _
                    " T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"", T0.""U_ARemarks"" FROM ""@SQTO""  T0 ORDER BY T0.""DocEntry"" DESC"
            Dim ErrMsg As String = ""
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable
            Dim RetDT2 As New DataTable
            Dim RetDT3 As New DataTable
            Dim RetDT4 As New DataTable

            Dim RetDS = New DataSet()
            RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
            If ErrMsg <> "" Then
                Throw New Exception(ErrMsg)
            End If

            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "SQTO"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)

                RetDT = New DataTable

                Query = "SELECT T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", T1.""U_Rate"", T1.""U_UOM"", " & _
                        " T1.""U_GST"", T1.""U_Remarks"" FROM ""@SQTOGENERAL""  T1 " & _
                        " WHERE T1.""DocEntry"" = (SELECT Top 1 T0.""DocEntry""  FROM ""@SQTO""  T0 ORDER BY T0.""DocEntry"" DESC)"
                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                RetDT.TableName = "SQTOGEN"
                RetDT2 = RetDT.Copy
                RetDS.Tables.Add(RetDT2)

                RetDT = New DataTable
                Query = "SELECT T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", T0.""U_Rate"", T0.""U_UOM"", " & _
                        " T0.""U_GST"", T0.""U_Remarks"" FROM ""@SQTOADDON""  T0 WHERE T0.""DocEntry""  = (SELECT Top 1 T0.""DocNum""  FROM ""@SQTO""  T0 ORDER BY T0.""DocEntry"" DESC)"
                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                RetDT.TableName = "SQTOADD"
                RetDT3 = RetDT.Copy
                RetDS.Tables.Add(RetDT3)

                RetDT = New DataTable
                Query = "SELECT ""U_Fname"" AS ""U_FileName"",""U_Path"" AS ""U_FilePath"",""LineId"" AS ""U_id"", TO_CHAR(""U_AttDate"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                        " FROM ""@SQTOATTACHMENT"" WHERE ""DocEntry""  = (SELECT Top 1 T0.""DocNum""  FROM ""@SQTO""  T0 ORDER BY T0.""DocEntry"" DESC)"
                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                RetDT.TableName = "ATTACHMENT"
                RetDT4 = RetDT.Copy
                RetDS.Tables.Add(RetDT4)

                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                RetDT = New DataTable
                RetDT.TableName = "VALIDATE"
                RetDT.Columns.Add("Status")
                RetDT.Columns.Add("Msg")
                RetDT.Rows.Add()
                RetDT.Rows(0)(0) = True
                RetDT.Rows(0)(1) = "No Record Found"
                RetDS.Tables.Add(RetDT)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            End If
        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
 <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub SalesQuotation_PreviousRecord(ByVal value As String)
        sFunction = "SalesQuotation_PreviousRecord"
        Try
            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim ds As DataSet = fn.jsontodata(value)
            Dim LEADM As New DataTable
            Dim DocNum As String = ""
            If ds.Tables("SQTO").Rows.Count > 0 Then
                LEADM = ds.Tables("SQTO")
                Dim dr As DataRow = LEADM.Rows(0)
                DocNum = dr.Item("U_Qno").ToString
                Dim Query As String = ""

                Dim ErrMsg As String = ""
                Dim RetDT As New DataTable
                Dim RetDT1 As New DataTable
                Dim RetDT2 As New DataTable
                Dim RetDT3 As New DataTable
                Dim RetDT4 As New DataTable
                Dim RetDS = New DataSet()
                Dim DocEntry As String = ""

                If DocNum = "" Then
                    Query = "SELECT Top 1 T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", " & _
                            " TO_CHAR( T0.""U_Qdate1"" , 'DD/MM/YYYY') As ""U_Qdate1"", TO_CHAR( T0.""U_Qdate2"" , 'DD/MM/YYYY') As ""U_Qdate2"", T0.""U_Ccode"", " & _
                            " T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", " & _
                            " T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", " & _
                            " T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"", T0.""U_ARemarks"" " & _
                            " FROM ""@SQTO""  T0 ORDER BY T0.""DocEntry"" ASC"

                Else
                    Query = "SELECT Top 1 T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", " & _
                            " TO_CHAR( T0.""U_Qdate1"" , 'DD/MM/YYYY') As ""U_Qdate1"", TO_CHAR( T0.""U_Qdate2"" , 'DD/MM/YYYY') As ""U_Qdate2"", T0.""U_Ccode"", " & _
                            " T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", " & _
                            " T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", " & _
                            " T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"", T0.""U_ARemarks"" " & _
                            " FROM ""@SQTO"" T0  where T0.""DocEntry"" = (SELECT (T0.""DocEntry"" - 1)  ""DocEntry"" FROM ""@SQTO""  T0 WHERE T0.""DocNum"" ='" & DocNum & "')"
                End If

                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                If RetDT.Rows.Count > 0 Then

                    DocEntry = RetDT.Rows(0)(0)
                    RetDT.TableName = "SQTO"
                    RetDT1 = RetDT.Copy
                    RetDS.Tables.Add(RetDT1)

                    RetDT = New DataTable

                    Query = "SELECT T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", T1.""U_Rate"", T1.""U_UOM"", " & _
                            " T1.""U_GST"", T1.""U_Remarks"" FROM ""@SQTOGENERAL""  T1 WHERE T1.""DocEntry"" = '" & DocEntry & "'"
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "SQTOGEN"
                    RetDT2 = RetDT.Copy
                    RetDS.Tables.Add(RetDT2)


                    RetDT = New DataTable
                    Query = "SELECT T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", T0.""U_Rate"", T0.""U_UOM"", " & _
                            " T0.""U_GST"", T0.""U_Remarks"" FROM ""@SQTOADDON""  T0 WHERE T0.""DocEntry""  = '" & DocEntry & "'"
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "SQTOADD"
                    RetDT3 = RetDT.Copy
                    RetDS.Tables.Add(RetDT3)

                    RetDT = New DataTable
                    Query = "SELECT ""U_Fname"" AS ""U_FileName"",""U_Path"" AS ""U_FilePath"",""LineId"" AS ""U_id"", TO_CHAR(""U_AttDate"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                            " FROM ""@SQTOATTACHMENT"" WHERE ""DocEntry""  = '" & DocEntry & "' "
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "ATTACHMENT"
                    RetDT4 = RetDT.Copy
                    RetDS.Tables.Add(RetDT4)


                    Context.Response.Output.Write(fn.ds2json(RetDS))
                Else

                    Query = "SELECT Top 1 T0.""DocNum"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", " & _
                            " TO_CHAR( T0.""U_Qdate1"" , 'DD/MM/YYYY') As ""U_Qdate1"", TO_CHAR( T0.""U_Qdate2"" , 'DD/MM/YYYY') As ""U_Qdate2"", T0.""U_Ccode"", " & _
                            " T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", " & _
                            " T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", " & _
                            " T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@SQTO""  T0 ORDER BY T0.""DocEntry"" DESC"
                    RetDT = New DataTable
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    If RetDT.Rows.Count > 0 Then
                        DocEntry = RetDT.Rows(0)(0)
                        RetDT.TableName = "SQTO"
                        RetDT1 = RetDT.Copy
                        RetDS.Tables.Add(RetDT1)

                        RetDT = New DataTable

                        Query = "SELECT T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", T1.""U_Rate"", T1.""U_UOM"", " & _
                                " T1.""U_GST"", T1.""U_Remarks"" FROM ""@SQTOGENERAL""  T1 WHERE T1.""DocEntry"" = '" & DocEntry & "'"
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "SQTOGEN"
                        RetDT2 = RetDT.Copy
                        RetDS.Tables.Add(RetDT2)


                        RetDT = New DataTable
                        Query = "SELECT T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", T0.""U_Rate"", T0.""U_UOM"", " & _
                                " T0.""U_GST"", T0.""U_Remarks"" FROM ""@SQTOADDON""  T0 WHERE T0.""DocEntry""  = '" & DocEntry & "'"
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "SQTOADD"
                        RetDT3 = RetDT.Copy
                        RetDS.Tables.Add(RetDT3)

                        RetDT = New DataTable
                        Query = "SELECT ""U_Fname"" AS ""U_FileName"",""U_Path"" AS ""U_FilePath"",""LineId"" AS ""U_id"", TO_CHAR(""U_AttDate"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                                " FROM ""@SQTOATTACHMENT"" WHERE ""DocEntry""  = '" & DocEntry & "' "
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "ATTACHMENT"
                        RetDT4 = RetDT.Copy
                        RetDS.Tables.Add(RetDT4)

                        Context.Response.Output.Write(fn.ds2json(RetDS))

                    Else
                        RetDT = New DataTable
                        RetDT.TableName = "VALIDATE"
                        RetDT.Columns.Add("Status")
                        RetDT.Columns.Add("Msg")
                        RetDT.Rows.Add()
                        RetDT.Rows(0)(0) = True
                        RetDT.Rows(0)(1) = "No Record Found"
                        RetDS.Tables.Add(RetDT)
                        Context.Response.Output.Write(fn.ds2json(RetDS))
                    End If
                End If
            Else
                Throw New Exception("No Record Found")
            End If
        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub SalesQuotation_NextRecord(ByVal value As String)
        sFunction = "SalesQuotation_NextRecord"
        Try
            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim ds As DataSet = fn.jsontodata(value)
            Dim LEADM As New DataTable
            Dim DocNum As String = ""
            If ds.Tables("SQTO").Rows.Count > 0 Then
                LEADM = ds.Tables("SQTO")
                Dim dr As DataRow = LEADM.Rows(0)
                DocNum = dr.Item("U_Qno").ToString
                Dim Query As String = ""

                Dim ErrMsg As String = ""
                Dim RetDT As New DataTable
                Dim RetDT1 As New DataTable
                Dim RetDT2 As New DataTable
                Dim RetDT3 As New DataTable
                Dim RetDT4 As New DataTable
                Dim RetDS = New DataSet()
                Dim DocEntry As String = ""

                If DocNum = "" Then
                    Query = "SELECT Top 1 T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", TO_CHAR( T0.""U_Qdate1"" , 'DD/MM/YYYY') As ""U_Qdate1"", TO_CHAR( T0.""U_Qdate2"" , 'DD/MM/YYYY') As ""U_Qdate2"", T0.""U_Ccode"", T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"", T0.""U_ARemarks"" FROM ""@SQTO""  T0 ORDER BY T0.""DocEntry"" DESC"

                Else
                    Query = "SELECT Top 1 T0.""DocNum"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", TO_CHAR( T0.""U_Qdate1"" , 'DD/MM/YYYY') As ""U_Qdate1"", TO_CHAR( T0.""U_Qdate2"" , 'DD/MM/YYYY') As ""U_Qdate2"", T0.""U_Ccode"", T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"", T0.""U_ARemarks"" FROM ""@SQTO"" T0  where T0.""DocEntry"" =(SELECT (T0.""DocEntry"" + 1)  ""DocEntry"" FROM ""@SQTO""  T0 WHERE T0.""DocNum"" ='" & DocNum & "')"
                End If

                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                If RetDT.Rows.Count > 0 Then

                    DocEntry = RetDT.Rows(0)(0)
                    RetDT.TableName = "SQTO"
                    RetDT1 = RetDT.Copy
                    RetDS.Tables.Add(RetDT1)

                    RetDT = New DataTable

                    Query = "SELECT T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", T1.""U_Rate"", T1.""U_UOM"", T1.""U_GST"", T1.""U_Remarks"" FROM ""@SQTOGENERAL""  T1 WHERE T1.""DocEntry"" = '" & DocEntry & "'"
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "SQTOGEN"
                    RetDT2 = RetDT.Copy
                    RetDS.Tables.Add(RetDT2)


                    RetDT = New DataTable
                    Query = "SELECT T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", T0.""U_Rate"", T0.""U_UOM"", T0.""U_GST"", T0.""U_Remarks"" FROM ""@SQTOADDON""  T0 WHERE T0.""DocEntry""  = '" & DocEntry & "'"
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "SQTOADD"
                    RetDT3 = RetDT.Copy
                    RetDS.Tables.Add(RetDT3)

                    RetDT = New DataTable
                    Query = "SELECT ""U_Fname"" AS ""U_FileName"",""U_Path"" AS ""U_FilePath"",""LineId"" AS ""U_id"", TO_CHAR(""U_AttDate"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                            " FROM ""@SQTOATTACHMENT"" WHERE ""DocEntry""  = '" & DocEntry & "' "
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "ATTACHMENT"
                    RetDT4 = RetDT.Copy
                    RetDS.Tables.Add(RetDT4)

                    Context.Response.Output.Write(fn.ds2json(RetDS))
                Else

                    Query = "SELECT Top 1 T0.""DocNum"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", TO_CHAR( T0.""U_Qdate1"" , 'DD/MM/YYYY') As ""U_Qdate1"", TO_CHAR( T0.""U_Qdate2"" , 'DD/MM/YYYY') As ""U_Qdate2"", T0.""U_Ccode"", T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@SQTO""  T0 ORDER BY T0.""DocEntry"" ASC"
                    RetDT = New DataTable
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    If RetDT.Rows.Count > 0 Then
                        DocEntry = RetDT.Rows(0)(0)
                        RetDT.TableName = "SQTO"
                        RetDT1 = RetDT.Copy
                        RetDS.Tables.Add(RetDT1)

                        RetDT = New DataTable

                        Query = "SELECT T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", T1.""U_Rate"", T1.""U_UOM"", T1.""U_GST"", T1.""U_Remarks"" FROM ""@SQTOGENERAL""  T1 WHERE T1.""DocEntry"" = '" & DocEntry & "'"
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "SQTOGEN"
                        RetDT2 = RetDT.Copy
                        RetDS.Tables.Add(RetDT2)


                        RetDT = New DataTable
                        Query = "SELECT T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", T0.""U_Rate"", T0.""U_UOM"", T0.""U_GST"", T0.""U_Remarks"" FROM ""@SQTOADDON""  T0 WHERE T0.""DocEntry""  = '" & DocEntry & "'"
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "SQTOADD"
                        RetDT3 = RetDT.Copy
                        RetDS.Tables.Add(RetDT3)

                        RetDT = New DataTable
                        Query = "SELECT ""U_Fname"" AS ""U_FileName"",""U_Path"" AS ""U_FilePath"",""LineId"" AS ""U_id"", TO_CHAR(""U_AttDate"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                                " FROM ""@SQTOATTACHMENT"" WHERE ""DocEntry""  = '" & DocEntry & "' "
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "ATTACHMENT"
                        RetDT4 = RetDT.Copy
                        RetDS.Tables.Add(RetDT4)

                        Context.Response.Output.Write(fn.ds2json(RetDS))

                    Else
                        RetDT = New DataTable
                        RetDT.TableName = "VALIDATE"
                        RetDT.Columns.Add("Status")
                        RetDT.Columns.Add("Msg")
                        RetDT.Rows.Add()
                        RetDT.Rows(0)(0) = True
                        RetDT.Rows(0)(1) = "No Record Found"
                        RetDS.Tables.Add(RetDT)
                        Context.Response.Output.Write(fn.ds2json(RetDS))
                    End If
                End If
            Else
                Throw New Exception("No Record Found")
            End If
        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
 <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub SalesQuotation_FindRecord_List(ByVal value As String)
        sFunction = "SalesQuotation_FindRecord_List"
        Try
            oLog.WriteToLogFile_Debug(value, sFunction)
            Dim ds As DataSet = fn.jsontodata(value)
            Dim LEADM As New DataTable
            Dim DocNum As String = ""
            If ds.Tables("SQTO").Rows.Count > 0 Then
                LEADM = ds.Tables("SQTO")
                Dim dr As DataRow = LEADM.Rows(0)
                Try
                    DocNum = dr.Item("U_Qno").ToString.Trim()
                Catch ex As Exception

                End Try

                Dim CreateDate As String = ""
                Dim CardCode As String = ""
                Dim CardName As String = ""
                Dim ProjectCode As String = ""

                CreateDate = dr.Item("U_Cdate").ToString.Trim()
                CardCode = dr.Item("U_Ccode").ToString.Trim()
                CardName = dr.Item("U_Cname").ToString.Trim()
                ProjectCode = dr.Item("U_Pcode").ToString.Trim()

                If CreateDate = "" Then
                    CreateDate = "%"
                ElseIf CreateDate.Contains("*") = True Then
                    CreateDate = CreateDate.Replace("*", "%")
                Else
                    CreateDate = "%" & CreateDate & "%"
                End If

                If CardCode = "" Then
                    CardCode = "%"
                ElseIf CardCode.Contains("*") = True Then
                    CardCode = CardCode.Replace("*", "%")
                Else
                    CardCode = "%" & CardCode & "%"
                End If

                If CardName = "" Then
                    CardName = "%"
                ElseIf CardName.Contains("*") = True Then
                    CardName = CardName.Replace("*", "%")
                Else
                    CardName = "%" & CardName & "%"
                End If

                If ProjectCode = "" Then
                    ProjectCode = "%"
                ElseIf ProjectCode.Contains("*") = True Then
                    ProjectCode = ProjectCode.Replace("*", "%")
                Else
                    ProjectCode = "%" & ProjectCode & "%"
                End If


                Dim Query As String = ""
                Dim ErrMsg As String = ""
                Dim RetDT As New DataTable
                Dim RetDT1 As New DataTable
                Dim RetDT2 As New DataTable
                Dim RetDT3 As New DataTable
                Dim RetDT4 As New DataTable
                Dim RetDS = New DataSet()
                Dim DocEntry As String = ""

                If DocNum = "" Then
                    Query = "SELECT T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", TO_CHAR( T0.""U_Qdate1"" , 'DD/MM/YYYY') As ""U_Qdate1"", TO_CHAR( T0.""U_Qdate2"" , 'DD/MM/YYYY') As ""U_Qdate2"", T0.""U_Ccode"", T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"", T0.""U_ARemarks"" FROM ""@SQTO"" T0 where T0.""U_Ccode"" like '" & CardCode & "' and  T0.""U_Cdate"" like '" & CreateDate & "'  and  T0.""U_Cname"" like '" & CardName & "'   and  T0.""U_Pcode"" like '" & ProjectCode & "'    ORDER BY T0.""DocEntry"" DESC"

                Else
                    Query = "SELECT Top 1 T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", TO_CHAR( T0.""U_Qdate1"" , 'DD/MM/YYYY') As ""U_Qdate1"", TO_CHAR( T0.""U_Qdate2"" , 'DD/MM/YYYY') As ""U_Qdate2"", T0.""U_Ccode"", T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"", T0.""U_ARemarks"" FROM ""@SQTO"" T0  where T0.""DocEntry"" ='" & DocNum & "'"
                End If

                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                If RetDT.Rows.Count > 0 Then
                    DocEntry = RetDT.Rows(0)(0)
                    RetDT.TableName = "SQTO"
                    RetDT1 = RetDT.Copy
                    RetDS.Tables.Add(RetDT1)

                    'RetDT = New DataTable
                    'Query = "SELECT T1.""DocEntry"" ,T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", T1.""U_Rate"", T1.""U_UOM"", T1.""U_GST"", T1.""U_Remarks"" FROM ""@SQTOGENERAL""  T1 WHERE T1.""DocEntry"" = '" & DocEntry & "'"
                    'RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    'If ErrMsg <> "" Then
                    '    Throw New Exception(ErrMsg)
                    'End If
                    'RetDT.TableName = "SQTOGEN"
                    'RetDT2 = RetDT.Copy
                    'RetDS.Tables.Add(RetDT2)


                    'RetDT = New DataTable
                    'Query = "SELECT T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", T0.""U_Rate"", T0.""U_UOM"", T0.""U_GST"", T0.""U_Remarks"" FROM ""@SQTOADDON""  T0 WHERE T0.""DocEntry""  = '" & DocEntry & "'"
                    'RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    'If ErrMsg <> "" Then
                    '    Throw New Exception(ErrMsg)
                    'End If
                    'RetDT.TableName = "SQTOADD"
                    'RetDT3 = RetDT.Copy
                    'RetDS.Tables.Add(RetDT3)

                    Context.Response.Output.Write(fn.ds2json(RetDS))

                Else
                    Throw New Exception("No Record Found")
                End If
            Else
                Throw New Exception("No Record Found")
            End If
        Catch ex As Exception
            oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
  <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub SalesQuotation_FindRecord(ByVal value As String)
        sFunction = "SalesQuotation_FindRecord"
        Try
            oLog.WriteToLogFile_Debug(value, sFunction)
            Dim ds As DataSet = fn.jsontodata(value)
            Dim LEADM As New DataTable
            Dim DocNum As String = ""
            If ds.Tables("SQTO").Rows.Count > 0 Then
                LEADM = ds.Tables("SQTO")
                Dim dr As DataRow = LEADM.Rows(0)
                DocNum = dr.Item("U_Qno").ToString
                'Dim CreateDate As String = ""
                'Dim CardCode As String = ""
                'Dim CardName As String = ""
                'Dim ProjectCode As String = ""

                'CreateDate = dr.Item("U_Cdate").ToString
                'CardCode = dr.Item("U_Ccode").ToString
                'CardName = dr.Item("U_Cname").ToString
                'ProjectCode = dr.Item("U_Pcode").ToString


                'If CreateDate = "" Then
                '    CreateDate = "%"
                'ElseIf CreateDate.Contains("*") = True Then
                '    CreateDate = CreateDate.Replace("*", "%")
                'Else
                '    CreateDate = "%" & CreateDate & "%"
                'End If

                'If CardCode = "" Then
                '    CardCode = "%"
                'ElseIf CardCode.Contains("*") = True Then
                '    CardCode = CardCode.Replace("*", "%")
                'Else
                '    CardCode = "%" & CardCode & "%"
                'End If

                'If CardName = "" Then
                '    CardName = "%"
                'ElseIf CardName.Contains("*") = True Then
                '    CardName = CardName.Replace("*", "%")
                'Else
                '    CardName = "%" & CardName & "%"
                'End If

                'If ProjectCode = "" Then
                '    ProjectCode = "%"
                'ElseIf ProjectCode.Contains("*") = True Then
                '    ProjectCode = ProjectCode.Replace("*", "%")
                'Else
                '    ProjectCode = "%" & ProjectCode & "%"
                'End If


                Dim Query As String = ""
                Dim ErrMsg As String = ""
                Dim RetDT As New DataTable
                Dim RetDT1 As New DataTable
                Dim RetDT2 As New DataTable
                Dim RetDT3 As New DataTable
                Dim RetDT4 As New DataTable
                Dim RetDS = New DataSet()
                Dim DocEntry As String = ""

                If DocNum = "" Then

                    Throw New Exception("No Record Found")
                    ' Query = "SELECT Top 1 T0.""DocNum"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", TO_CHAR( T0.""U_Qdate1"" , 'DD/MM/YYYY') As ""U_Qdate1"", TO_CHAR( T0.""U_Qdate2"" , 'DD/MM/YYYY') As ""U_Qdate2"", T0.""U_Ccode"", T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@SQTO""  where T0.""U_Ccode"" like '" & CardCode & "' and  T0.""U_Cdate"" like '" & DocNum & "'  and  T0.""U_Cname"" like '" & CardName & "'   and  T0.""U_Pcode"" like '" & ProjectCode & "'   T0 ORDER BY T0.""DocEntry"" DESC"

                Else
                    Query = "SELECT Top 1 T0.""DocNum"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", TO_CHAR( T0.""U_Qdate1"" , 'DD/MM/YYYY') As ""U_Qdate1"", TO_CHAR( T0.""U_Qdate2"" , 'DD/MM/YYYY') As ""U_Qdate2"", T0.""U_Ccode"", T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"", T0.""U_ARemarks"" FROM ""@SQTO"" T0  where T0.""DocEntry"" ='" & DocNum & "'"
                End If

                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                If RetDT.Rows.Count > 0 Then

                    DocEntry = RetDT.Rows(0)(0)
                    RetDT.TableName = "SQTO"
                    RetDT1 = RetDT.Copy
                    RetDS.Tables.Add(RetDT1)

                    RetDT = New DataTable

                    Query = "SELECT T1.""DocEntry"" ,T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", T1.""U_Rate"", T1.""U_UOM"", T1.""U_GST"", T1.""U_Remarks"" FROM ""@SQTOGENERAL""  T1 WHERE T1.""DocEntry"" = '" & DocEntry & "'"
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "SQTOGEN"
                    RetDT2 = RetDT.Copy
                    RetDS.Tables.Add(RetDT2)


                    RetDT = New DataTable
                    Query = "SELECT T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", T0.""U_Rate"", T0.""U_UOM"", T0.""U_GST"", T0.""U_Remarks"" FROM ""@SQTOADDON""  T0 WHERE T0.""DocEntry""  = '" & DocEntry & "'"
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "SQTOADD"
                    RetDT3 = RetDT.Copy
                    RetDS.Tables.Add(RetDT3)

                    RetDT = New DataTable
                    Query = "SELECT ""U_Fname"" AS ""U_FileName"",""U_Path"" AS ""U_FilePath"",""LineId"" AS ""U_id"", TO_CHAR(""U_AttDate"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                            " FROM ""@SQTOATTACHMENT"" WHERE ""DocEntry""  = '" & DocEntry & "' "
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "ATTACHMENT"
                    RetDT4 = RetDT.Copy
                    RetDS.Tables.Add(RetDT4)

                    Context.Response.Output.Write(fn.ds2json(RetDS))
                Else
                    Throw New Exception("No Record Found")
                    Query = "SELECT Top 1 T0.""DocNum"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", TO_CHAR( T0.""U_Qdate1"" , 'DD/MM/YYYY') As ""U_Qdate1"", TO_CHAR( T0.""U_Qdate2"" , 'DD/MM/YYYY') As ""U_Qdate2"", T0.""U_Ccode"", T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@SQTO""  T0 ORDER BY T0.""DocEntry"" DESC"
                    RetDT = New DataTable
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    If RetDT.Rows.Count > 0 Then
                        DocEntry = RetDT.Rows(0)(0)
                        RetDT.TableName = "SQTO"
                        RetDT1 = RetDT.Copy
                        RetDS.Tables.Add(RetDT1)

                        RetDT = New DataTable

                        Query = "SELECT T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", T1.""U_Rate"", T1.""U_UOM"", T1.""U_GST"", T1.""U_Remarks"" FROM ""@SQTOGENERAL""  T1 WHERE T1.""DocEntry"" = '" & DocEntry & "'"
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "SQTOGEN"
                        RetDT2 = RetDT.Copy
                        RetDS.Tables.Add(RetDT2)


                        RetDT = New DataTable
                        Query = "SELECT T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", T0.""U_Rate"", T0.""U_UOM"", T0.""U_GST"", T0.""U_Remarks"" FROM ""@SQTOADDON""  T0 WHERE T0.""DocEntry""  = '" & DocEntry & "'"
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "SQTOADD"
                        RetDT3 = RetDT.Copy
                        RetDS.Tables.Add(RetDT3)

                        RetDT = New DataTable
                        Query = "SELECT ""U_Fname"" AS ""U_FileName"",""U_Path"" AS ""U_FilePath"",""LineId"" AS ""U_id"", TO_CHAR(""U_AttDate"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                                " FROM ""@SQTOATTACHMENT"" WHERE ""DocEntry""  = '" & DocEntry & "' "
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "ATTACHMENT"
                        RetDT4 = RetDT.Copy
                        RetDS.Tables.Add(RetDT4)

                        Context.Response.Output.Write(fn.ds2json(RetDS))

                    Else
                        RetDT = New DataTable
                        RetDT.TableName = "VALIDATE"
                        RetDT.Columns.Add("Status")
                        RetDT.Columns.Add("Msg")
                        RetDT.Rows.Add()
                        RetDT.Rows(0)(0) = True
                        RetDT.Rows(0)(1) = "No Record Found"
                        RetDS.Tables.Add(RetDT)
                        Context.Response.Output.Write(fn.ds2json(RetDS))
                    End If
                End If
            Else
                Throw New Exception("No Record Found")
            End If
        Catch ex As Exception
            oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub Sales_Quotation_Approval_Status(ByVal value As String)
        Try
            Dim FromCreateDt As String = ""
            Dim ToCreateDt As String = ""
            Dim CreateBy As String = ""
            Dim FromCode As String = ""
            Dim ToCode As String = ""
            Dim ApprovalSt As String = ""
            Dim dtFromDate As Date
            Dim dtToDate As Date

            Dim Code As String = ""
            Dim Name As String = ""
            Dim OCRD As New DataTable
            OCRD = New DataTable
            Dim LEADMDS = New DataSet()
            Dim SQLDT As New DataTable
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""

            If ds.Tables("APPROVAL").Rows.Count > 0 Then
                OCRD = ds.Tables("APPROVAL")
                Dim dr As DataRow = OCRD.Rows(0)
                FromCreateDt = dr.Item("CdateFrom").ToString.Trim
                ToCreateDt = dr.Item("CdateTo").ToString.Trim
                CreateBy = dr.Item("CreateBy").ToString.Trim
                FromCode = dr.Item("FromCode").ToString.Trim
                ToCode = dr.Item("ToCode").ToString.Trim
                ApprovalSt = dr.Item("ApprovalSt").ToString.Trim
            End If

            If FromCreateDt = "" Then
                'FromCreateDt = "2000-01-01"
            Else
                dtFromDate = GetDateTimeValue(FromCreateDt)
            End If
            If ToCreateDt = "" Then
                'ToCreateDt = "9999-12-31"
            Else
                dtToDate = GetDateTimeValue(ToCreateDt)
            End If
            If CreateBy = "" Then
                CreateBy = "%"
            End If
            If ApprovalSt = "" Then
                ApprovalSt = "Pending"
            End If
            If FromCode = "" Then
                FromCode = fn.ExecuteSQLQuery_SingleValue("SELECT Top 1 T0.""Code"" As ""U_Ccode"" FROM ""@LEADM""  T0  UNION ALL SELECT T0.""CardCode"" FROM OCRD T0 WHERE IFNULL( T0.""U_WebAccess"" ,'') ='Y' and  T0.""CardType"" ='C'  order by ""U_Ccode""", sErrDesc)
                If sErrDesc <> "" Then
                    Throw New Exception(sErrDesc)
                End If

            End If

            If ToCode = "" Then
                ToCode = fn.ExecuteSQLQuery_SingleValue("SELECT Top 1 T0.""Code"" As ""U_Ccode"" FROM ""@LEADM""  T0  UNION ALL SELECT T0.""CardCode"" FROM OCRD T0 WHERE IFNULL( T0.""U_WebAccess"" ,'') ='Y' and  T0.""CardType"" ='C'  order by ""U_Ccode"" desc", sErrDesc)
                If sErrDesc <> "" Then
                    Throw New Exception(sErrDesc)
                End If
            End If

            'Dim Str As String = "SELECT T0.""CardCode"" As ""U_Ccode"", T0.""CardName"" as ""U_Cname"", T0.""Phone1"" as ""U_TelNo"", T0.""Fax"" as ""U_FaxNo"", T0.""Cellular"" as ""U_Mno"", T0.""E_Mail"" as ""U_Email"" FROM OCRD T0 WHERE IFNULL( T0.""U_WebAccess"" ,'') ='Y' and  T0.""CardType"" ='C' and  T0.""CardCode""  like '" & Code & "' and T0.""CardName"" like '" & Name & "'"
            Dim Str As String = "SELECT T0.""DocNum"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As  ""U_Cdate"", T0.""U_Uname"", T0.""U_Ccode"", T0.""U_Cname"", " & _
                                " TO_CHAR( T0.""U_Qdate1"" , 'DD/MM/YYYY') as ""U_Qdate1"", TO_CHAR( T0.""U_Qdate2"" , 'DD/MM/YYYY') as ""U_Qdate2"", T0.""U_Status"", " & _
                                " ifnull(T0.""U_ApprovedBy"",'') ""U_ApprovedBy"" , TO_CHAR( T0.""U_ApprovedDt"" , 'DD/MM/YYYY') as ""U_ApprovedDt"" " & _
                                " FROM ""@SQTO""  T0 WHERE T0.""U_Status"" ='" & ApprovalSt & "' " & _
                                " and T0.""U_Cdate"" >= (CASE WHEN IFNULL('" & FromCreateDt & "','') = '' THEN T0.""U_Cdate"" ELSE '" & dtFromDate.ToString("yyyy-MM-dd") & "' END) " & _
                                " and T0.""U_Cdate"" <= (CASE WHEN IFNULL('" & ToCreateDt & "','') = '' THEN T0.""U_Cdate"" ELSE '" & dtToDate.ToString("yyyy-MM-dd") & "' END)   " & _
                                " and T0.""U_Uname"" like '" & CreateBy & "'  and  T0.""U_Ccode"" between '" & FromCode & "' and '" & ToCode & "'"

            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable
            Dim RetDT2 As New DataTable
            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "APPROVAL"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
 <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub Sales_Quotation_Approval_Submit(ByVal value As String)
        sFunction = "Sales_Quotation_Approval_Submit"

        Try
            oLog.WriteToLogFile_Debug(value, sFunction)
            Dim ds As DataSet = fn.jsontodata(value)
            Dim LEADM As New DataTable
            Dim DocNum As String = ""
            If ds.Tables("APPROVAL").Rows.Count > 0 Then
                LEADM = ds.Tables("APPROVAL")
                Dim dr As DataRow = LEADM.Rows(0)
                DocNum = dr.Item("U_Qno").ToString
            End If

            Dim Str As String = ""
            Dim Errmsg As String = ""
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable
            Dim RetDT2 As New DataTable
            Dim RetDS As New DataSet
            Str = "update  ""@SQTO"" set ""U_Status""='Pending' where ""DocNum"" ='" & DocNum & "'"
            Errmsg = fn.ExecuteNonQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            RetDT.TableName = "APPROVAL"
            RetDT.Columns.Add("Status")
            RetDT.Columns.Add("Msg")
            RetDT.Rows.Add()
            RetDT.Rows(0)(0) = True
            RetDT.Rows(0)(1) = "Sales Quotation no. " & DocNum.ToString & " Pending for Approval"
            RetDS.Tables.Add(RetDT)
            RetDT.Dispose()
            Context.Response.Output.Write(fn.ds2json(RetDS))

        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
 <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub Sales_Quotation_Approver_Submit(ByVal value As String)
        sFunction = "Sales_Quotation_Approver_Submit"

        Try
            oLog.WriteToLogFile_Debug(value, sFunction)
            Dim ds As DataSet = fn.jsontodata(value)
            Dim LEADM As New DataTable
            Dim DocNum As String = ""
            Dim Status As String = ""
            Dim Remarks As String = ""
            Dim UserName As String = ""
            If ds.Tables("APPROVAL").Rows.Count > 0 Then
                LEADM = ds.Tables("APPROVAL")
                Dim dr As DataRow = LEADM.Rows(0)
                DocNum = dr.Item("U_Qno").ToString
                Status = dr.Item("U_Status").ToString
                Remarks = dr.Item("U_Remarks").ToString
                UserName = dr.Item("U_Uname").ToString
            End If
            Dim ApprovalDt As String = Format(Now.Date, "yyyy-MM-dd")
            Dim Str As String = ""
            Dim Errmsg As String = ""
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable
            Dim RetDT2 As New DataTable
            Dim RetDS As New DataSet
            Str = "update  ""@SQTO"" set ""U_Status""='" & Status & "',""U_ApprovedBy""='" & UserName & "',""U_ApprovedDt""='" & ApprovalDt & "',""U_ARemarks""='" & Remarks & "' where ""DocNum"" ='" & DocNum & "'"
            Errmsg = fn.ExecuteNonQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            RetDT.TableName = "APPROVAL"
            RetDT.Columns.Add("Status")
            RetDT.Columns.Add("Msg")
            RetDT.Rows.Add()
            RetDT.Rows(0)(0) = True
            RetDT.Rows(0)(1) = "Sales Quotation no. " & DocNum.ToString & " " & Status & ""
            RetDS.Tables.Add(RetDT)
            RetDT.Dispose()
            Context.Response.Output.Write(fn.ds2json(RetDS))

        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
#End Region
#Region "Lead"
    <WebMethod()> _
  <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub LeadAdd(ByVal value As String)

        sFunction = "LeadAdd"
        Try
            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim LEADM As New DataTable
            LEADM = New DataTable
            Dim LEADMDS = New DataSet()
            Dim SQLDT As New DataTable
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""
            Dim LeadCode As String = ""
            ' Dim oCompany3 As New SAPbobsCOM.Company
            ' Fn.GetSystemIntializeInfo(Errmsg, oCompany3)
            fn.GetSAPConnection(Errmsg)

            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            Dim SQLStr As String = ""
            ' udo object creation
            Dim oGeneralService As SAPbobsCOM.GeneralService
            Dim oGeneralData As SAPbobsCOM.GeneralData
            Dim oSons As SAPbobsCOM.GeneralDataCollection
            Dim oSon As SAPbobsCOM.GeneralData
            Dim sCmp As SAPbobsCOM.CompanyService
            sCmp = PublicVariable.oCompany.GetCompanyService
            oGeneralService = sCmp.GetGeneralService("LEADM")
            oGeneralData = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)
            If ds.Tables("LEADM").Rows.Count > 0 Then
                LEADM = ds.Tables("LEADM")
                Dim dr As DataRow = LEADM.Rows(0)
                SQLStr = "SELECT COUNT(T0.""Code"")+1  As ""Code"" FROM ""@LEADM""  T0"
                SQLDT = fn.ExecuteSQLQuery(SQLStr, sErrDesc)
                If sErrDesc <> "" Then
                    Throw New Exception(sErrDesc)
                End If
                If dr.Item("Name").ToString.Trim() = "" Then
                    Throw New Exception("The ""Name"" field must not be empty; enter a value for ""Name"".")
                End If
                Dim SQLDR As DataRow = SQLDT.Rows(0)
                LeadCode = "L000" & SQLDR.Item("Code").ToString.Trim()
                oGeneralData.SetProperty("Code", LeadCode)
                oGeneralData.SetProperty("Name", dr.Item("Name").ToString.Trim())
                oGeneralData.SetProperty("U_TelNo", dr.Item("U_TelNo").ToString.Trim())
                oGeneralData.SetProperty("U_FaxNo", dr.Item("U_FaxNo").ToString.Trim())
                oGeneralData.SetProperty("U_MNo", dr.Item("U_MNo").ToString.Trim())
                oGeneralData.SetProperty("U_Email", dr.Item("U_Email").ToString.Trim())

                oGeneralData.SetProperty("U_Addr1", dr.Item("U_Addr1").ToString.Trim())
                oGeneralData.SetProperty("U_Addr2", dr.Item("U_Addr2").ToString.Trim())
                oGeneralData.SetProperty("U_Addr3", dr.Item("U_Addr3").ToString.Trim())
                oGeneralData.SetProperty("U_Addr4", dr.Item("U_Addr4").ToString.Trim())
                oGeneralData.SetProperty("U_Addr5", dr.Item("U_Addr5").ToString.Trim())
                oGeneralData.SetProperty("U_Addr6", dr.Item("U_Addr6").ToString.Trim())

                Dim response = oGeneralService.Add(oGeneralData)
                DocEntry = response.GetProperty("Code")
            End If

            Dim RetDT As New DataTable
            RetDT.TableName = "VALIDATE"
            RetDT.Columns.Add("Status")
            RetDT.Columns.Add("Msg")
            RetDT.Rows.Add()
            RetDT.Rows(0)(0) = True
            RetDT.Rows(0)(1) = "Lead No. " & DocEntry.ToString & " Created Successfully"
            RetDS.Tables.Add(RetDT)
            Context.Response.Output.Write(fn.ds2json(RetDS))
        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
   <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub LeadUpdate(ByVal value As String)
        Try
            Dim LEADM As New DataTable
            LEADM = New DataTable
            Dim LEADMDS = New DataSet()
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            fn.GetSAPConnection(Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            Dim oGeneralService As SAPbobsCOM.GeneralService
            Dim oGeneralData As SAPbobsCOM.GeneralData
            Dim oSons As SAPbobsCOM.GeneralDataCollection
            Dim oSon As SAPbobsCOM.GeneralData
            Dim sCmp As SAPbobsCOM.CompanyService
            sCmp = PublicVariable.oCompany.GetCompanyService
            oGeneralService = sCmp.GetGeneralService("LEADM")
            oGeneralData = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)
            If ds.Tables("LEADM").Rows.Count > 0 Then
                LEADM = ds.Tables("LEADM")
                Dim dr As DataRow = LEADM.Rows(0)
                Dim Query As String = "SELECT T0.""Code"" FROM  ""@LEADM""  T0 WHERE T0.""Code"" ='" & dr.Item("Code") & "'"
                Dim RetDTT As New DataTable
                RetDTT = fn.ExecuteSQLQuery(Query, Errmsg)
                If Errmsg <> "" Then
                    Throw New Exception(Errmsg)
                End If
                Dim dr1 As DataRow = RetDTT.Rows(0)
                Dim Code As String = dr1.Item("Code").ToString
                If RetDTT.Rows.Count > 0 Then
                    oGeneralData.SetProperty("Code", Code)
                    If dr.Item("Name").ToString.Trim() = "" Then
                        Throw New Exception("The ""Name"" field must not be empty; enter a value for ""Name"".")
                    End If
                    oGeneralData.SetProperty("Name", dr.Item("Name").ToString.Trim())
                    oGeneralData.SetProperty("U_TelNo", dr.Item("U_TelNo").ToString.Trim())
                    oGeneralData.SetProperty("U_FaxNo", dr.Item("U_FaxNo").ToString.Trim())
                    oGeneralData.SetProperty("U_MNo", dr.Item("U_MNo").ToString.Trim())
                    oGeneralData.SetProperty("U_Email", dr.Item("U_Email").ToString.Trim())
                    oGeneralData.SetProperty("U_Addr1", dr.Item("U_Addr1").ToString.Trim())
                    oGeneralData.SetProperty("U_Addr2", dr.Item("U_Addr2").ToString.Trim())
                    oGeneralData.SetProperty("U_Addr3", dr.Item("U_Addr3").ToString.Trim())
                    oGeneralData.SetProperty("U_Addr4", dr.Item("U_Addr4").ToString.Trim())
                    oGeneralData.SetProperty("U_Addr5", dr.Item("U_Addr5").ToString.Trim())
                    oGeneralData.SetProperty("U_Addr6", dr.Item("U_Addr6").ToString.Trim())
                    oGeneralService.Update(oGeneralData)
                    DocEntry = Code
                Else
                    RetDTT = New DataTable
                    RetDTT.TableName = "VALIDATE"
                    RetDTT.Columns.Add("Status")
                    RetDTT.Columns.Add("Msg")
                    RetDTT.Rows.Add()
                    RetDTT.Rows(0)(0) = True
                    RetDTT.Rows(0)(1) = "No Record Found"
                    RetDS.Tables.Add(RetDTT)
                    Context.Response.Output.Write(fn.ds2json(RetDS))
                End If
            End If
            Dim RetDT As New DataTable
            RetDT.TableName = "VALIDATE"
            RetDT.Columns.Add("Status")
            RetDT.Columns.Add("Msg")
            RetDT.Rows.Add()
            RetDT.Rows(0)(0) = True
            RetDT.Rows(0)(1) = "Lead Master No. " & DocEntry.ToString & " Update Successfully"
            RetDS.Tables.Add(RetDT)
            Context.Response.Output.Write(fn.ds2json(RetDS))

        Catch ex As Exception
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
   <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub LeadLastRecord()
        Try
            Dim Query As String = "SELECT TOP 1  T0.""Code"", T0.""Name"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_MNo"", T0.""U_Email"",T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"" FROM ""@LEADM""  T0 ORDER BY  T0.""DocEntry"" desc"
            Dim ErrMsg As String = ""
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable
            Dim RetDS = New DataSet()
            RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
            If ErrMsg <> "" Then
                Throw New Exception(ErrMsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "LEADM"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)

                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                RetDT = New DataTable
                RetDT.TableName = "VALIDATE"
                RetDT.Columns.Add("Status")
                RetDT.Columns.Add("Msg")
                RetDT.Rows.Add()
                RetDT.Rows(0)(0) = True
                RetDT.Rows(0)(1) = "No Record Found"
                RetDS.Tables.Add(RetDT)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            End If
        Catch ex As Exception
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
   <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub LeadFirstRecord()
        Try
            Dim Query As String = "SELECT TOP 1  T0.""Code"", T0.""Name"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_MNo"", T0.""U_Email"",T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"" FROM ""@LEADM""  T0 ORDER BY  T0.""DocEntry"" Asc"
            Dim ErrMsg As String = ""
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable
            Dim RetDS = New DataSet()
            RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
            If ErrMsg <> "" Then
                Throw New Exception(ErrMsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "LEADM"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                RetDT = New DataTable
                RetDT.TableName = "VALIDATE"
                RetDT.Columns.Add("Status")
                RetDT.Columns.Add("Msg")
                RetDT.Rows.Add()
                RetDT.Rows(0)(0) = True
                RetDT.Rows(0)(1) = "No Record Found"
                RetDS.Tables.Add(RetDT)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            End If
        Catch ex As Exception
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
   <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub LeadPreviousRecord(ByVal value As String)
        Try
            Dim LEADM As New DataTable
            LEADM = New DataTable
            Dim LEADMDS = New DataSet()
            Dim RetDS = New DataSet()
            Dim RetDT1 As New DataTable
            Dim ds As DataSet = fn.jsontodata(value)
            Dim ErrMsg As String = ""
            Dim RetDT As New DataTable
            Dim LeadCode As String = ""
            Dim SqlStr As String = ""
            If ds.Tables("LEADM").Rows.Count > 0 Then
                LEADM = ds.Tables("LEADM")
                Dim dr As DataRow = LEADM.Rows(0)
                LeadCode = dr.Item("Code").ToString
                If LeadCode = "" Then
                    SqlStr = "SELECT TOP 1  T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"",T0.""Code"", T0.""Name"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_MNo"", T0.""U_Email"" FROM ""@LEADM""  T0 ORDER BY  T0.""DocEntry"" Asc"
                Else
                    SqlStr = "select Top 1 T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"",T0.""Code"", T0.""Name"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_MNo"",  T0.""U_Email""  from ""@LEADM"" T0 WHERE T0.""DocEntry"" =(select (""DocEntry"" -1) DocEntry from ""@LEADM"" WHERE ""Code""='" & LeadCode & "')"
                End If

                RetDT = fn.ExecuteSQLQuery(SqlStr, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                If RetDT.Rows.Count = 0 Then
                    SqlStr = "SELECT TOP 1  T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"",T0.""Code"", T0.""Name"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_MNo"", T0.""U_Email"" FROM ""@LEADM""  T0 ORDER BY  T0.""DocEntry"" desc"
                    RetDT = New DataTable
                    RetDT = fn.ExecuteSQLQuery(SqlStr, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    If RetDT.Rows.Count = 0 Then
                        Throw New Exception("No Recored found")
                    Else
                        RetDT.TableName = "LEADM"
                        RetDT1 = RetDT.Copy()
                        RetDS.Tables.Add(RetDT1)
                        Context.Response.Output.Write(fn.ds2json(RetDS))
                    End If
                Else
                    RetDT.TableName = "LEADM"
                    RetDT1 = RetDT.Copy()
                    RetDS.Tables.Add(RetDT1)
                    Context.Response.Output.Write(fn.ds2json(RetDS))
                End If
            Else
                Throw New Exception("No Record Found")
            End If

        Catch ex As Exception
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
  <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub LeadNextRecord(ByVal value As String)
        Try
            Dim LEADM As New DataTable
            LEADM = New DataTable
            Dim LEADMDS = New DataSet()
            Dim RetDS = New DataSet()
            Dim RetDT1 As New DataTable
            Dim ds As DataSet = fn.jsontodata(value)
            Dim ErrMsg As String = ""
            Dim RetDT As New DataTable
            Dim LeadCode As String = ""
            Dim SqlStr As String = ""
            If ds.Tables("LEADM").Rows.Count > 0 Then
                LEADM = ds.Tables("LEADM")
                Dim dr As DataRow = LEADM.Rows(0)
                LeadCode = dr.Item("Code").ToString
                If LeadCode = "" Then
                    SqlStr = "SELECT TOP 1  T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"",T0.""Code"", T0.""Name"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_MNo"", T0.""U_Email"" FROM ""@LEADM""  T0 ORDER BY  T0.""DocEntry"" desc"
                Else
                    SqlStr = "select Top 1 T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"",T0.""Code"", T0.""Name"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_MNo"",  T0.""U_Email""  from ""@LEADM"" T0 WHERE T0.""DocEntry"" =(select (""DocEntry"" +1) DocEntry from ""@LEADM"" WHERE ""Code""='" & LeadCode & "')"
                End If

                RetDT = fn.ExecuteSQLQuery(SqlStr, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                If RetDT.Rows.Count = 0 Then
                    SqlStr = "SELECT TOP 1  T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"",T0.""Code"", T0.""Name"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_MNo"", T0.""U_Email"" FROM ""@LEADM""  T0 ORDER BY  T0.""DocEntry"" Asc"
                    RetDT = New DataTable
                    RetDT = fn.ExecuteSQLQuery(SqlStr, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    If RetDT.Rows.Count = 0 Then
                        Throw New Exception("No Recored found")
                    Else
                        RetDT.TableName = "LEADM"
                        RetDT1 = RetDT.Copy()
                        RetDS.Tables.Add(RetDT1)
                        Context.Response.Output.Write(fn.ds2json(RetDS))
                    End If
                Else
                    RetDT.TableName = "LEADM"
                    RetDT1 = RetDT.Copy()
                    RetDS.Tables.Add(RetDT1)
                    Context.Response.Output.Write(fn.ds2json(RetDS))
                End If
            Else
                Throw New Exception("No Record Found")
            End If

        Catch ex As Exception
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
  <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub LeadFindRecord(ByVal value As String)
        Try
            Dim Code As String = ""
            Dim Name As String = ""
            Dim TelePhoneNo As String = ""
            Dim FaxNo As String = ""
            Dim EMail As String = ""
            Dim MobileNo As String = ""

            Dim LEADM As New DataTable
            LEADM = New DataTable
            Dim LEADMDS = New DataSet()
            Dim SQLDT As New DataTable
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""
            If ds.Tables("LEADM").Rows.Count > 0 Then
                LEADM = ds.Tables("LEADM")
                Dim dr As DataRow = LEADM.Rows(0)
                Code = dr.Item("Code").ToString.Trim
                Name = dr.Item("Name").ToString.Trim
                TelePhoneNo = dr.Item("U_TelNo").ToString.Trim
                FaxNo = dr.Item("U_FaxNo").ToString.Trim
                EMail = dr.Item("U_Email").ToString.Trim
                MobileNo = dr.Item("U_MNo").ToString.Trim
            End If

            If Code = "" Then
                Code = "%"
            ElseIf Code.Contains("*") = True Then
                Code = Code.Replace("*", "%")
            Else
                Code = "%" & Code & "%"
            End If

            If Name = "" Then
                Name = "%"
            ElseIf Name.Contains("*") = True Then
                Name = Name.Replace("*", "%")
            Else
                Name = "%" & Name & "%"
            End If

            If TelePhoneNo = "" Then
                TelePhoneNo = "%"
            ElseIf TelePhoneNo.Contains("*") = True Then
                TelePhoneNo = TelePhoneNo.Replace("*", "%")
            Else
                TelePhoneNo = "%" & TelePhoneNo & "%"
            End If

            If FaxNo = "" Then
                FaxNo = "%"
            ElseIf FaxNo.Contains("*") = True Then
                FaxNo = FaxNo.Replace("*", "%")
            Else
                FaxNo = "%" & FaxNo & "%"
            End If

            If EMail = "" Then
                EMail = "%"
            ElseIf EMail.Contains("*") = True Then
                EMail = EMail.Replace("*", "%")
            Else
                EMail = "%" & EMail & "%"
            End If

            If MobileNo = "" Then
                MobileNo = "%"
            ElseIf MobileNo.Contains("*") = True Then
                MobileNo = MobileNo.Replace("*", "%")
            Else
                MobileNo = "%" & MobileNo & "%"
            End If

            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable
            Dim Query As String = "SELECT T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"",T0.""Code"", T0.""Name"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_MNo"",  T0.""U_Email"" FROM ""@LEADM""  T0 WHERE T0.""Code"" like '" & Code & "' and T0.""Name"" Like '" & Name & "'   and  T0.""U_TelNo""  Like '" & TelePhoneNo & "' and  T0.""U_FaxNo"" Like '" & FaxNo & "' and  T0.""U_MNo"" Like '" & MobileNo & "' and  T0.""U_Email"" Like '" & EMail & "'"
            RetDT = fn.ExecuteSQLQuery(Query, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "LEADM"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                RetDT = New DataTable
                RetDT.TableName = "VALIDATE"
                RetDT.Columns.Add("Status")
                RetDT.Columns.Add("Msg")
                RetDT.Rows.Add()
                RetDT.Rows(0)(0) = True
                RetDT.Rows(0)(1) = "No Record Found"
                RetDS.Tables.Add(RetDT)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            End If
        Catch ex As Exception
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
#End Region
#Region "Customer Contract"
    <WebMethod()> _
        <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub CustomerContractAdd(ByVal value As String)
        sFunction = "CustomerContract"
        Try

            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim SQTO As New DataTable
            SQTO = New DataTable
            Dim SQTOGEN As New DataTable
            Dim SQTOADD As New DataTable
            Dim SOTOATTACH As New DataTable
            Dim SQTODS = New DataSet()
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            fn.GetSAPConnection(Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            Dim oGeneralService As SAPbobsCOM.GeneralService
            Dim oGeneralData As SAPbobsCOM.GeneralData
            Dim oSons As SAPbobsCOM.GeneralDataCollection
            Dim oSon As SAPbobsCOM.GeneralData
            Dim sCmp As SAPbobsCOM.CompanyService
            Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

            sCmp = PublicVariable.oCompany.GetCompanyService
            oGeneralService = sCmp.GetGeneralService("CCON")
            oGeneralData = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)
            oGeneralParams = CType(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams), SAPbobsCOM.GeneralDataParams)
            ' oGeneralData = oGeneralService.GetByParams(oGeneralParams)
            Dim QutoNo As String = ""
            Dim sErrMsg As String = ""
            If ds.Tables("CCON").Rows.Count > 0 Then
                SQTO = ds.Tables("CCON")
                Dim dr As DataRow = SQTO.Rows(0)
                ' Dim oRecord As SAPbobsCOM.Recordset
                'oRecord = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                'oRecord.DoQuery("select U_Qno from [@SQTO] where U_Qno='" & dr.Item("U_Qno") & "'")
                ' If oRecord.RecordCount = 0 Then
                'oGeneralData.SetProperty("U_Qno", "Open")
                ' Try
                QutoNo = dr.Item("U_QutoNo").ToString.Trim()
                If QutoNo <> "" Then
                    Dim SQLStr As String = "update ""@SQTO""  set ""U_Status""='Closed' where ""DocEntry"" ='" & QutoNo & "'"
                    SQLStr = fn.ExecuteNonQuery(SQLStr, sErrMsg)
                    oGeneralData.SetProperty("U_QutoNo", dr.Item("U_QutoNo").ToString.Trim())
                End If

                'Catch ex As Exception
                'End Try

                oGeneralData.SetProperty("U_Status", "Open")
                oGeneralData.SetProperty("U_Uname", dr.Item("U_Uname").ToString.Trim())
                oGeneralData.SetProperty("U_Cdate", dr.Item("U_Cdate").ToString.Trim())
                'oGeneralData.SetProperty("U_Qdate1", dr.Item("U_Qdate1").ToString.Trim())
                'oGeneralData.SetProperty("U_Qdate2", dr.Item("U_Qdate2").ToString.Trim())
                oGeneralData.SetProperty("U_Ccode", dr.Item("U_Ccode").ToString.Trim())
                oGeneralData.SetProperty("U_Cname", dr.Item("U_Cname").ToString.Trim())
                oGeneralData.SetProperty("U_CPeriod1", dr.Item("U_CPeriod1").ToString.Trim())
                oGeneralData.SetProperty("U_CPeriod2", dr.Item("U_CPeriod2").ToString.Trim())
                oGeneralData.SetProperty("U_Pcode", dr.Item("U_Pcode").ToString.Trim())
                oGeneralData.SetProperty("U_AddrN", dr.Item("U_AddrN").ToString.Trim())
                oGeneralData.SetProperty("U_Addr1", dr.Item("U_Addr1").ToString.Trim())
                oGeneralData.SetProperty("U_Addr2", dr.Item("U_Addr2").ToString.Trim())
                oGeneralData.SetProperty("U_Addr3", dr.Item("U_Addr3").ToString.Trim())
                oGeneralData.SetProperty("U_Addr4", dr.Item("U_Addr4").ToString.Trim())
                oGeneralData.SetProperty("U_Addr5", dr.Item("U_Addr5").ToString.Trim())
                oGeneralData.SetProperty("U_Addr6", dr.Item("U_Addr6").ToString.Trim())
                Try
                    oGeneralData.SetProperty("U_TelNo", dr.Item("U_TelNo").ToString.Trim())
                    oGeneralData.SetProperty("U_FaxNo", dr.Item("U_FaxNo").ToString.Trim())
                    oGeneralData.SetProperty("U_Mno", dr.Item("U_Mno").ToString.Trim())
                Catch ex As Exception
                End Try
                oGeneralData.SetProperty("U_Email", dr.Item("U_Email").ToString.Trim())
                oGeneralData.SetProperty("U_Remarks", dr.Item("U_Remarks").ToString.Trim())
                If ds.Tables("CCONGEN").Rows.Count > 0 Then
                    SQTOGEN = ds.Tables("CCONGEN")
                    oSons = oGeneralData.Child("CCONGENERAL")
                    For Each dr1 As DataRow In SQTOGEN.Rows
                        oSon = oSons.Add
                        If dr1.Item("U_Stype").ToString.Trim() = "" Then
                            Throw New Exception("The ""Survey Type"" field must not be empty; enter a value for ""Survey Type"".")
                        End If
                        oSon.SetProperty("U_Stype", dr1.Item("U_Stype").ToString.Trim())

                        If dr1.Item("U_Conti").ToString.Trim() = "" Then
                            Throw New Exception("The ""Continent"" field must not be empty; enter a value for ""Continent"".")
                        End If
                        oSon.SetProperty("U_Conti", dr1.Item("U_Conti").ToString.Trim())

                        If dr1.Item("U_Country").ToString.Trim() = "" Then
                            Throw New Exception("The ""Country"" field must not be empty; enter a value for ""Country"".")
                        End If
                        oSon.SetProperty("U_Country", dr1.Item("U_Country").ToString.Trim())

                        If dr1.Item("U_City").ToString.Trim() = "" Then
                            Throw New Exception("The ""City"" field must not be empty; enter a value for ""City"".")
                        End If
                        oSon.SetProperty("U_City", dr1.Item("U_City").ToString.Trim())

                        If dr1.Item("U_Currency").ToString.Trim() = "" Then
                            Throw New Exception("The ""Currency"" field must not be empty; enter a value for ""Currency"".")
                        End If
                        oSon.SetProperty("U_Currency", dr1.Item("U_Currency").ToString.Trim())

                        If dr1.Item("U_EQGroup").ToString.Trim() = "" Then
                            Throw New Exception("The ""Equipment Group"" field must not be empty; enter a value for ""Equipment Group"".")
                        End If
                        oSon.SetProperty("U_EQGroup", dr1.Item("U_EQGroup").ToString.Trim())
                        oSon.SetProperty("U_Rate", dr1.Item("U_Rate").ToString.Trim())
                        oSon.SetProperty("U_RRate", dr1.Item("U_RRate").ToString.Trim())
                        oSon.SetProperty("U_UOM", dr1.Item("U_UOM").ToString.Trim())
                        If dr1.Item("U_GST").ToString.Trim() = "" Then
                            Throw New Exception("The ""GST"" field must not be empty; enter a value for ""GST"".")
                        End If
                        oSon.SetProperty("U_GST", dr1.Item("U_GST").ToString.Trim())
                        oSon.SetProperty("U_Remarks", dr1.Item("U_Remarks").ToString.Trim())
                    Next
                Else
                    Throw New Exception("The ""General Tab"" must not be empty; enter a value for ""General Tab"".")
                End If
                If ds.Tables("CCONADD").Rows.Count > 0 Then
                    SQTOADD = ds.Tables("CCONADD")
                    oSons = oGeneralData.Child("CCONADDON")
                    For Each dr1 As DataRow In SQTOADD.Rows
                        oSon = oSons.Add

                        If dr1.Item("U_Ctype").ToString.Trim() = "" Then
                            Throw New Exception("The ""Charge Type"" field must not be empty; enter a value for ""Charge Type"".")
                        End If
                        oSon.SetProperty("U_Ctype", dr1.Item("U_Ctype").ToString.Trim())

                        If dr1.Item("U_Continent").ToString.Trim() = "" Then
                            Throw New Exception("The ""Continent"" field must not be empty; enter a value for ""Continent"".")
                        End If
                        oSon.SetProperty("U_Continent", dr1.Item("U_Continent").ToString.Trim())

                        If dr1.Item("U_Country").ToString.Trim() = "" Then
                            Throw New Exception("The ""Country"" field must not be empty; enter a value for ""Country"".")
                        End If
                        oSon.SetProperty("U_Country", dr1.Item("U_Country").ToString.Trim())

                        If dr1.Item("U_City").ToString.Trim() = "" Then
                            Throw New Exception("The ""City"" field must not be empty; enter a value for ""City"".")
                        End If
                        oSon.SetProperty("U_City", dr1.Item("U_City").ToString.Trim())

                        If dr1.Item("U_Currency").ToString.Trim() = "" Then
                            Throw New Exception("The ""Currency"" field must not be empty; enter a value for ""Currency"".")
                        End If
                        oSon.SetProperty("U_Currency", dr1.Item("U_Currency").ToString.Trim())

                        If dr1.Item("U_EQGroup").ToString.Trim() = "" Then
                            Throw New Exception("The ""Equipment Group"" field must not be empty; enter a value for ""Equipment Group"".")
                        End If
                        oSon.SetProperty("U_EQGroup", dr1.Item("U_EQGroup").ToString.Trim())

                        oSon.SetProperty("U_Rate", dr1.Item("U_Rate").ToString.Trim())
                        oSon.SetProperty("U_UOM", dr1.Item("U_UOM").ToString.Trim())

                        If dr1.Item("U_GST").ToString.Trim() = "" Then
                            Throw New Exception("The ""GST"" field must not be empty; enter a value for ""GST"".")
                        End If
                        oSon.SetProperty("U_GST", dr1.Item("U_GST").ToString.Trim())
                        oSon.SetProperty("U_Remarks", dr1.Item("U_Remarks").ToString.Trim())
                    Next
                End If
                If ds.Tables("ATTACHMENT").Rows.Count > 0 Then
                    SOTOATTACH = ds.Tables("ATTACHMENT")
                    oSons = oGeneralData.Child("CCONATTACHMENT")
                    For Each odr As DataRow In SOTOATTACH.Rows
                        oSon = oSons.Add
                        Try
                            oSon.SetProperty("U_Path", odr.Item("U_FilePath").ToString.Trim())
                        Catch ex As Exception
                            Throw New Exception("Error while adding file path to attachment folder")
                        End Try
                        Try
                            oSon.SetProperty("U_Fname", odr.Item("U_FileName").ToString.Trim())
                        Catch ex As Exception
                            Throw New Exception("Error while adding file name to attachment folder")
                        End Try
                        Try
                            Dim dtAttachDate As Date
                            dtAttachDate = GetDateTimeValue(odr.Item("U_Date").ToString)
                            oSon.SetProperty("U_AttDate", dtAttachDate)
                        Catch ex As Exception
                            Throw New Exception("Error while adding attachment date to attachment folder")
                        End Try

                    Next
                End If

                Dim response = oGeneralService.Add(oGeneralData)
                DocEntry = response.GetProperty("DocEntry")
            End If

            Dim RetDT As New DataTable
            RetDT.TableName = "VALIDATE"
            RetDT.Columns.Add("Status")
            RetDT.Columns.Add("Msg")
            RetDT.Rows.Add()
            RetDT.Rows(0)(0) = True
            RetDT.Rows(0)(1) = "Customer Contract no. " & DocEntry.ToString & " Created Successfully"
            RetDS.Tables.Add(RetDT)
            Context.Response.Output.Write(fn.ds2json(RetDS))


        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
        <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub CustomerContract_Update(ByVal value As String)
        sFunction = "CustomerContract_Update"
        Try

            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim SQTO As New DataTable
            SQTO = New DataTable
            Dim SQTOGEN As New DataTable
            Dim SQTOADD As New DataTable
            Dim SQTOATTACH As New DataTable
            Dim SQTODS = New DataSet()
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            fn.GetSAPConnection(Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            Dim oGeneralService As SAPbobsCOM.GeneralService
            Dim oGeneralData As SAPbobsCOM.GeneralData
            Dim oSons As SAPbobsCOM.GeneralDataCollection
            Dim oSon As SAPbobsCOM.GeneralData
            Dim sCmp As SAPbobsCOM.CompanyService
            Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

            sCmp = PublicVariable.oCompany.GetCompanyService
            oGeneralService = sCmp.GetGeneralService("CCON")
            oGeneralData = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)
            oGeneralParams = CType(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams), SAPbobsCOM.GeneralDataParams)
            ' oGeneralData = oGeneralService.GetByParams(oGeneralParams)
            '  Dim RetDT As New DataTable
            ' RetDT = fn.ExecuteSQLQuery(Query, Errmsg)
            Dim Count_Gen As String = ""
            Dim Count_Addon As String = ""
            Dim Count_Attach As String = ""
            Dim Count_Gen_Int As Integer = 0
            Dim Count_Addon_Int As Integer = 0
            Dim Count_Attach_Int As Integer = 0
            Dim Query As String = ""


            If ds.Tables("CCON").Rows.Count > 0 Then
                SQTO = ds.Tables("CCON")
                Dim dr As DataRow = SQTO.Rows(0)


                DocEntry = dr.Item("U_Qno").ToString.Trim()
                Query = "SELECT COunt(T0.""DocEntry"") FROM ""@CCONGENERAL""  T0 WHERE T0.""DocEntry"" =" & DocEntry & ""
                Count_Gen = fn.ExecuteSQLQuery_SingleValue(Query, Errmsg)
                If Errmsg <> "" Then
                    Throw New Exception(Errmsg)
                End If
                If Count_Gen <> "" Then
                    Count_Gen_Int = CInt(Count_Gen)
                End If
                Query = "SELECT COunt(T0.""DocEntry"") FROM ""@CCONADDON""  T0 WHERE T0.""DocEntry"" =" & DocEntry & ""
                Count_Addon = fn.ExecuteSQLQuery_SingleValue(Query, Errmsg)
                If Errmsg <> "" Then
                    Throw New Exception(Errmsg)
                End If
                If Count_Addon <> "" Then
                    Count_Addon_Int = CInt(Count_Addon)
                End If

                Query = "SELECT COUNT(T0.""DocEntry"") FROM ""@CCONATTACHMENT"" T0 WHERE T0.""DocEntry"" = '" & DocEntry & "' "
                Count_Attach = fn.ExecuteSQLQuery_SingleValue(Query, Errmsg)
                If Errmsg <> "" Then
                    Throw New Exception(Errmsg)
                End If
                If Count_Attach <> "" Then
                    Count_Attach_Int = CInt(Count_Attach)
                End If

                oGeneralParams.SetProperty("DocEntry", dr.Item("U_Qno").ToString.Trim())
                oGeneralData = oGeneralService.GetByParams(oGeneralParams)

                'oGeneralData.SetProperty("U_Qno", dr.Item("U_Qno"))
                '  oGeneralData.SetProperty("U_Status", dr.Item("U_Status").ToString.Trim())
                oGeneralData.SetProperty("U_Uname", dr.Item("U_Uname").ToString.Trim())
                oGeneralData.SetProperty("U_Cdate", dr.Item("U_Cdate").ToString.Trim())
                'oGeneralData.SetProperty("U_Qdate1", dr.Item("U_Qdate1").ToString.Trim())
                'oGeneralData.SetProperty("U_Qdate2", dr.Item("U_Qdate2").ToString.Trim())
                oGeneralData.SetProperty("U_Ccode", dr.Item("U_Ccode").ToString.Trim())
                oGeneralData.SetProperty("U_Cname", dr.Item("U_Cname").ToString.Trim())
                oGeneralData.SetProperty("U_CPeriod1", dr.Item("U_CPeriod1").ToString.Trim())
                oGeneralData.SetProperty("U_CPeriod2", dr.Item("U_CPeriod2").ToString.Trim())
                oGeneralData.SetProperty("U_Pcode", dr.Item("U_Pcode").ToString.Trim())
                oGeneralData.SetProperty("U_AddrN", dr.Item("U_AddrN").ToString.Trim())
                oGeneralData.SetProperty("U_Addr1", dr.Item("U_Addr1").ToString.Trim())
                oGeneralData.SetProperty("U_Addr2", dr.Item("U_Addr2").ToString.Trim())
                oGeneralData.SetProperty("U_Addr3", dr.Item("U_Addr3").ToString.Trim())
                oGeneralData.SetProperty("U_Addr4", dr.Item("U_Addr4").ToString.Trim())
                oGeneralData.SetProperty("U_Addr5", dr.Item("U_Addr5").ToString.Trim())
                oGeneralData.SetProperty("U_Addr6", dr.Item("U_Addr6").ToString.Trim())
                Try
                    oGeneralData.SetProperty("U_TelNo", dr.Item("U_TelNo").ToString.Trim())
                    oGeneralData.SetProperty("U_FaxNo", dr.Item("U_FaxNo").ToString.Trim())
                    oGeneralData.SetProperty("U_Mno", dr.Item("U_Mno").ToString.Trim())
                Catch ex As Exception
                End Try

                'oGeneralData.SetProperty("U_TelNo", dr.Item("U_TelNo").ToString.Trim())
                'oGeneralData.SetProperty("U_FaxNo", dr.Item("U_FaxNo").ToString.Trim())
                'oGeneralData.SetProperty("U_Mno", dr.Item("U_Mno").ToString.Trim())
                oGeneralData.SetProperty("U_Email", dr.Item("U_Email").ToString.Trim())
                oGeneralData.SetProperty("U_Remarks", dr.Item("U_Remarks").ToString.Trim())


                Dim LineNum As Integer = 0
                Dim SQTOGENnewLineNo As Integer = 0
                Dim LoopRow As Integer = 0
                Dim RemoveIndex As Integer = 0
                If ds.Tables("CCONGEN").Rows.Count > 0 Then
                    SQTOGEN = ds.Tables("CCONGEN")
                    oSons = oGeneralData.Child("CCONGENERAL")
                    SQTOGENnewLineNo = SQTOGEN.Rows.Count()

                    If SQTOGEN.Rows.Count > Count_Gen_Int Then
                        LoopRow = SQTOGEN.Rows.Count
                    Else
                        LoopRow = Count_Gen_Int
                    End If
                    Dim i As Integer = 0
                    RemoveIndex = (Count_Gen_Int - 1)
                    For i = 0 To LoopRow - 1
                        'For Each dr1 As DataRow In SQTOGEN.Rows
                        LineNum = LineNum + 1
                        If SQTOGENnewLineNo = Count_Gen_Int Then
                            ' oSon = oSons.Add
                            oSons.Item(LineNum - 1).SetProperty("U_Stype", SQTOGEN.Rows(i).Item("U_Stype").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Conti", SQTOGEN.Rows(i).Item("U_Conti").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Country", SQTOGEN.Rows(i).Item("U_Country").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_City", SQTOGEN.Rows(i).Item("U_City").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Currency", SQTOGEN.Rows(i).Item("U_Currency").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_EQGroup", SQTOGEN.Rows(i).Item("U_EQGroup").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Rate", SQTOGEN.Rows(i).Item("U_Rate").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_RRate", SQTOGEN.Rows(i).Item("U_RRate").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_UOM", SQTOGEN.Rows(i).Item("U_UOM").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_GST", SQTOGEN.Rows(i).Item("U_GST").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Remarks", SQTOGEN.Rows(i).Item("U_Remarks").ToString.Trim())
                        ElseIf SQTOGENnewLineNo > Count_Gen_Int Then
                            If LineNum <= Count_Gen_Int Then
                                oSons.Item(LineNum - 1).SetProperty("U_Stype", SQTOGEN.Rows(i).Item("U_Stype").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Conti", SQTOGEN.Rows(i).Item("U_Conti").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Country", SQTOGEN.Rows(i).Item("U_Country").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_City", SQTOGEN.Rows(i).Item("U_City").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Currency", SQTOGEN.Rows(i).Item("U_Currency").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_EQGroup", SQTOGEN.Rows(i).Item("U_EQGroup").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Rate", SQTOGEN.Rows(i).Item("U_Rate").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_RRate", SQTOGEN.Rows(i).Item("U_RRate").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_UOM", SQTOGEN.Rows(i).Item("U_UOM").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_GST", SQTOGEN.Rows(i).Item("U_GST").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Remarks", SQTOGEN.Rows(i).Item("U_Remarks").ToString.Trim())
                            Else
                                oSon = oSons.Add
                                oSon.SetProperty("U_Stype", SQTOGEN.Rows(i).Item("U_Stype").ToString.Trim())
                                oSon.SetProperty("U_Conti", SQTOGEN.Rows(i).Item("U_Conti").ToString.Trim())
                                oSon.SetProperty("U_Country", SQTOGEN.Rows(i).Item("U_Country").ToString.Trim())
                                oSon.SetProperty("U_City", SQTOGEN.Rows(i).Item("U_City").ToString.Trim())
                                oSon.SetProperty("U_Currency", SQTOGEN.Rows(i).Item("U_Currency").ToString.Trim())
                                oSon.SetProperty("U_EQGroup", SQTOGEN.Rows(i).Item("U_EQGroup").ToString.Trim())
                                oSon.SetProperty("U_Rate", SQTOGEN.Rows(i).Item("U_Rate").ToString.Trim())
                                oSon.SetProperty("U_RRate", SQTOGEN.Rows(i).Item("U_RRate").ToString.Trim())
                                oSon.SetProperty("U_UOM", SQTOGEN.Rows(i).Item("U_UOM").ToString.Trim())
                                oSon.SetProperty("U_GST", SQTOGEN.Rows(i).Item("U_GST").ToString.Trim())
                                oSon.SetProperty("U_Remarks", SQTOGEN.Rows(i).Item("U_Remarks").ToString.Trim())
                            End If
                        ElseIf SQTOGENnewLineNo < Count_Gen_Int Then
                            If LineNum <= SQTOGENnewLineNo Then
                                oSons.Item(LineNum - 1).SetProperty("U_Stype", SQTOGEN.Rows(i).Item("U_Stype").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Conti", SQTOGEN.Rows(i).Item("U_Conti").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Country", SQTOGEN.Rows(i).Item("U_Country").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_City", SQTOGEN.Rows(i).Item("U_City").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Currency", SQTOGEN.Rows(i).Item("U_Currency").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_EQGroup", SQTOGEN.Rows(i).Item("U_EQGroup").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Rate", SQTOGEN.Rows(i).Item("U_Rate").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_RRate", SQTOGEN.Rows(i).Item("U_RRate").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_UOM", SQTOGEN.Rows(i).Item("U_UOM").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_GST", SQTOGEN.Rows(i).Item("U_GST").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Remarks", SQTOGEN.Rows(i).Item("U_Remarks").ToString.Trim())
                            Else
                                oSons.Remove(RemoveIndex - 1)
                                RemoveIndex = RemoveIndex - 1
                            End If

                        End If
                        'Next
                    Next
                End If
                LineNum = 0
                Dim SQTOADDNnewLineNo As Integer = 0
                LoopRow = 0
                RemoveIndex = 0

                If ds.Tables("CCONADD").Rows.Count > 0 Then
                    SQTOADD = ds.Tables("CCONADD")
                    oSons = oGeneralData.Child("CCONADDON")

                    SQTOADDNnewLineNo = SQTOADD.Rows.Count()
                    If SQTOADD.Rows.Count > Count_Addon_Int Then
                        LoopRow = SQTOADD.Rows.Count
                    Else
                        LoopRow = Count_Addon_Int
                    End If
                    Dim i As Integer = 0
                    RemoveIndex = (Count_Addon_Int - 1)
                    For i = 0 To LoopRow - 1
                        '                        For Each dr1 As DataRow In SQTOADD.Rows
                        LineNum = LineNum + 1
                        If SQTOADDNnewLineNo = Count_Addon_Int Then
                            'oSon = oSons.Add
                            'oSon.Item(LineNum - 1).SetProperty("U_Ctype", SQTOADD.Rows(i).Item("U_Ctype"))
                            oSons.Item(LineNum - 1).SetProperty("U_Ctype", SQTOADD.Rows(i).Item("U_Ctype").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Continent", SQTOADD.Rows(i).Item("U_Continent").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Country", SQTOADD.Rows(i).Item("U_Country").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_City", SQTOADD.Rows(i).Item("U_City"))
                            oSons.Item(LineNum - 1).SetProperty("U_Currency", SQTOADD.Rows(i).Item("U_Currency").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_EQGroup", SQTOADD.Rows(i).Item("U_EQGroup").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Rate", SQTOADD.Rows(i).Item("U_Rate").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_UOM", SQTOADD.Rows(i).Item("U_UOM").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_GST", SQTOADD.Rows(i).Item("U_GST").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Remarks", SQTOADD.Rows(i).Item("U_Remarks").ToString.Trim())

                        ElseIf SQTOADDNnewLineNo > Count_Addon_Int Then
                            If LineNum <= Count_Addon_Int Then
                                oSons.Item(LineNum - 1).SetProperty("U_Ctype", SQTOADD.Rows(i).Item("U_Ctype").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Continent", SQTOADD.Rows(i).Item("U_Continent").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Country", SQTOADD.Rows(i).Item("U_Country").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_City", SQTOADD.Rows(i).Item("U_City"))
                                oSons.Item(LineNum - 1).SetProperty("U_Currency", SQTOADD.Rows(i).Item("U_Currency").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_EQGroup", SQTOADD.Rows(i).Item("U_EQGroup").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Rate", SQTOADD.Rows(i).Item("U_Rate").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_UOM", SQTOADD.Rows(i).Item("U_UOM").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_GST", SQTOADD.Rows(i).Item("U_GST").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Remarks", SQTOADD.Rows(i).Item("U_Remarks").ToString.Trim())
                            Else
                                oSon = oSons.Add
                                oSon.SetProperty("U_Ctype", SQTOADD.Rows(i).Item("U_Ctype").ToString.Trim())
                                oSon.SetProperty("U_Continent", SQTOADD.Rows(i).Item("U_Continent").ToString.Trim())
                                oSon.SetProperty("U_Country", SQTOADD.Rows(i).Item("U_Country").ToString.Trim())
                                oSon.SetProperty("U_City", SQTOADD.Rows(i).Item("U_City").ToString.Trim())
                                oSon.SetProperty("U_Currency", SQTOADD.Rows(i).Item("U_Currency").ToString.Trim())
                                oSon.SetProperty("U_EQGroup", SQTOADD.Rows(i).Item("U_EQGroup").ToString.Trim())
                                oSon.SetProperty("U_Rate", SQTOADD.Rows(i).Item("U_Rate").ToString.Trim())
                                oSon.SetProperty("U_UOM", SQTOADD.Rows(i).Item("U_UOM").ToString.Trim())
                                oSon.SetProperty("U_GST", SQTOADD.Rows(i).Item("U_GST").ToString.Trim())
                                oSon.SetProperty("U_Remarks", SQTOADD.Rows(i).Item("U_Remarks").ToString.Trim())
                            End If
                        ElseIf SQTOADDNnewLineNo < Count_Addon_Int Then
                            If LineNum <= SQTOADDNnewLineNo Then
                                oSons.Item(LineNum - 1).SetProperty("U_Ctype", SQTOADD.Rows(i).Item("U_Ctype").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Continent", SQTOADD.Rows(i).Item("U_Continent").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Country", SQTOADD.Rows(i).Item("U_Country").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_City", SQTOADD.Rows(i).Item("U_City"))
                                oSons.Item(LineNum - 1).SetProperty("U_Currency", SQTOADD.Rows(i).Item("U_Currency").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_EQGroup", SQTOADD.Rows(i).Item("U_EQGroup").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Rate", SQTOADD.Rows(i).Item("U_Rate").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_UOM", SQTOADD.Rows(i).Item("U_UOM").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_GST", SQTOADD.Rows(i).Item("U_GST").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Remarks", SQTOADD.Rows(i).Item("U_Remarks").ToString.Trim())
                            Else
                                oSons.Remove(RemoveIndex - 1)
                                RemoveIndex = RemoveIndex - 1
                            End If
                        End If
                        'Next
                    Next
                End If

                LineNum = 0
                Dim SQTOATTACHNnewLineNo As Integer = 0
                LoopRow = 0
                RemoveIndex = 0

                If ds.Tables("ATTACHMENT").Rows.Count > 0 Then
                    SQTOATTACH = ds.Tables("ATTACHMENT")
                    oSons = oGeneralData.Child("CCONATTACHMENT")

                    SQTOATTACHNnewLineNo = SQTOATTACH.Rows.Count()
                    If SQTOATTACH.Rows.Count > Count_Attach_Int Then
                        LoopRow = SQTOATTACH.Rows.Count
                    Else
                        LoopRow = Count_Attach_Int
                    End If
                    Dim i As Integer = 0
                    RemoveIndex = (Count_Attach_Int - 1)
                    For i = 0 To LoopRow - 1
                        '                        For Each dr1 As DataRow In SQTOADD.Rows
                        LineNum = LineNum + 1
                        If SQTOATTACHNnewLineNo = Count_Attach_Int Then
                            oSons.Item(LineNum - 1).SetProperty("U_Path", SQTOATTACH.Rows(i).Item("U_FilePath").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Fname", SQTOATTACH.Rows(i).Item("U_FileName").ToString.Trim())

                            Dim dtAttachDate As Date
                            dtAttachDate = GetDateTimeValue(SQTOATTACH.Rows(i).Item("U_Date").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_AttDate", dtAttachDate)

                        ElseIf SQTOATTACHNnewLineNo > Count_Attach_Int Then
                            If LineNum <= Count_Attach_Int Then
                                oSons.Item(LineNum - 1).SetProperty("U_Path", SQTOATTACH.Rows(i).Item("U_FilePath").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Fname", SQTOATTACH.Rows(i).Item("U_FileName").ToString.Trim())

                                Dim dtAttachDate As Date
                                dtAttachDate = GetDateTimeValue(SQTOATTACH.Rows(i).Item("U_Date").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_AttDate", dtAttachDate)
                            Else
                                oSon = oSons.Add
                                oSon.SetProperty("U_Path", SQTOATTACH.Rows(i).Item("U_FilePath").ToString.Trim())
                                oSon.SetProperty("U_Fname", SQTOATTACH.Rows(i).Item("U_FileName").ToString.Trim())

                                Dim dtAttachDate As Date
                                dtAttachDate = GetDateTimeValue(SQTOATTACH.Rows(i).Item("U_Date").ToString.Trim())
                                oSon.SetProperty("U_AttDate", dtAttachDate)
                            End If
                        ElseIf SQTOATTACHNnewLineNo < Count_Attach_Int Then
                            If LineNum <= SQTOATTACHNnewLineNo Then
                                oSons.Item(LineNum - 1).SetProperty("U_Path", SQTOATTACH.Rows(i).Item("U_FilePath").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Fname", SQTOATTACH.Rows(i).Item("U_FileName").ToString.Trim())

                                Dim dtAttachDate As Date
                                dtAttachDate = GetDateTimeValue(SQTOATTACH.Rows(i).Item("U_Date").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_AttDate", dtAttachDate)
                            Else
                                oSons.Remove(RemoveIndex - 1)
                                RemoveIndex = RemoveIndex - 1
                            End If
                        End If
                        'Next
                    Next
                End If

                oGeneralService.Update(oGeneralData)
                DocEntry = dr.Item("U_Qno")
            End If

            Dim RetDT As New DataTable
            RetDT.TableName = "VALIDATE"
            RetDT.Columns.Add("Status")
            RetDT.Columns.Add("Msg")
            RetDT.Rows.Add()
            RetDT.Rows(0)(0) = True
            RetDT.Rows(0)(1) = "Customer Contract no. " & DocEntry.ToString & " Updated Successfully"
            RetDS.Tables.Add(RetDT)
            Context.Response.Output.Write(fn.ds2json(RetDS))

            '-------------
            'Dim orec As SAPbobsCOM.Recordset
            'orec = ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            'Dim sSQL As String = "SELECT Idx FROM ( " & vbCrLf
            'sSQL &= "SELECT CONVERT(INT, ROW_NUMBER() OVER (ORDER BY VisOrder))-1 Idx, LineId, VisOrder FROM [@TI_SAMPLE_R] WHERE DocEntry = 58 " & vbCrLf
            'sSQL &= ") X WHERE X.LineId IN (2,5,6) ORDER BY Idx DESC  "
            'orec.DoQuery(sSQL)
            'oChildren = oGeneralData.Child("TI_SAMPLE_R")
            'While (orec.EoF) = False
            '    Dim remve As Integer = orec.Fields.Item(0).Value
            '    oChildren.Remove(remve)
            '    orec.MoveNext()
            'End While
            'oGeneralService.Update(oGeneralData)
            '--------------

        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
  <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub CustomerContract_FirstRecord()
        sFunction = "CustomerContract_FirstRecord"
        Try

            Dim Query As String
            Query = "SELECT Top 1 T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", " & _
                    " T0.""U_Ccode"", T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", " & _
                    " T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", " & _
                    " T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@CCON""  T0 ORDER BY T0.""DocEntry"" ASC"
            Dim ErrMsg As String = ""
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable
            Dim RetDT2 As New DataTable
            Dim RetDT3 As New DataTable
            Dim RetDT4 As New DataTable

            Dim RetDS = New DataSet()
            RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
            If ErrMsg <> "" Then
                Throw New Exception(ErrMsg)
            End If

            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "CCON"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)

                RetDT = New DataTable

                Query = "SELECT T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", T1.""U_Rate"",T1.""U_RRate"", T1.""U_UOM"", " & _
                        " T1.""U_GST"", T1.""U_Remarks"" FROM ""@CCONGENERAL""  T1 WHERE T1.""DocEntry"" = (SELECT Top 1 T0.""DocEntry""  FROM ""@CCON""  T0 ORDER BY T0.""DocEntry"" ASC)"
                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                RetDT.TableName = "CCONGEN"
                RetDT2 = RetDT.Copy
                RetDS.Tables.Add(RetDT2)


                RetDT = New DataTable
                Query = "SELECT T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", T0.""U_Rate"", T0.""U_UOM"", " & _
                        " T0.""U_GST"", T0.""U_Remarks"" FROM ""@CCONADDON""  T0 WHERE T0.""DocEntry""  = (SELECT Top 1 T0.""DocNum""  FROM ""@CCON""  T0 ORDER BY T0.""DocEntry"" ASC)"
                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                RetDT.TableName = "CCONADD"
                RetDT3 = RetDT.Copy
                RetDS.Tables.Add(RetDT3)

                RetDT = New DataTable
                Query = "SELECT ""U_Fname"" AS ""U_FileName"",""U_Path"" AS ""U_FilePath"",""LineId"" AS ""U_id"", TO_CHAR(""U_AttDate"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                        " FROM ""@CCONATTACHMENT"" WHERE ""DocEntry"" = (SELECT Top 1 T0.""DocEntry""  FROM ""@CCON""  T0 ORDER BY T0.""DocEntry"" ASC) "
                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                RetDT.TableName = "ATTACHMENT"
                RetDT4 = RetDT.Copy
                RetDS.Tables.Add(RetDT4)


                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                RetDT = New DataTable
                RetDT.TableName = "VALIDATE"
                RetDT.Columns.Add("Status")
                RetDT.Columns.Add("Msg")
                RetDT.Rows.Add()
                RetDT.Rows(0)(0) = True
                RetDT.Rows(0)(1) = "No Record Found"
                RetDS.Tables.Add(RetDT)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            End If
        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
  <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub CustomerContract_LastRecord()
        sFunction = "CustomerContract_LastRecord"
        Try
            Dim Query As String
            Query = "SELECT Top 1 T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"",  T0.""U_Ccode"", " & _
                    " T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", " & _
                    " T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", " & _
                    " T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@CCON""  T0 ORDER BY T0.""DocEntry"" DESC"
            Dim ErrMsg As String = ""
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable
            Dim RetDT2 As New DataTable
            Dim RetDT3 As New DataTable
            Dim RetDT4 As New DataTable

            Dim RetDS = New DataSet()
            RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
            If ErrMsg <> "" Then
                Throw New Exception(ErrMsg)
            End If

            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "CCON"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)

                RetDT = New DataTable

                Query = "SELECT T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", T1.""U_Rate"",T1.""U_RRate"", T1.""U_UOM"", T1.""U_GST"", " & _
                        " T1.""U_Remarks"" FROM ""@CCONGENERAL""  T1 WHERE T1.""DocEntry"" = (SELECT Top 1 T0.""DocEntry""  FROM ""@CCON""  T0 ORDER BY T0.""DocEntry"" DESC)"
                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                RetDT.TableName = "CCONGEN"
                RetDT2 = RetDT.Copy
                RetDS.Tables.Add(RetDT2)


                RetDT = New DataTable
                Query = "SELECT T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", T0.""U_Rate"", T0.""U_UOM"", " & _
                        " T0.""U_GST"", T0.""U_Remarks"" FROM ""@CCONADDON""  T0 " & _
                        " WHERE T0.""DocEntry""  = (SELECT Top 1 T0.""DocNum""  FROM ""@CCON""  T0 ORDER BY T0.""DocEntry"" DESC)"
                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                RetDT.TableName = "CCONADD"
                RetDT3 = RetDT.Copy
                RetDS.Tables.Add(RetDT3)

                RetDT = New DataTable
                Query = "SELECT ""U_Fname"" AS ""U_FileName"",""U_Path"" AS ""U_FilePath"",""LineId"" AS ""U_id"", TO_CHAR(""U_AttDate"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                        " FROM ""@CCONATTACHMENT"" WHERE ""DocEntry"" = (SELECT Top 1 T0.""DocEntry""  FROM ""@CCON""  T0 ORDER BY T0.""DocEntry"" DESC) ORDER BY ""LineId"" "
                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                RetDT.TableName = "ATTACHMENT"
                RetDT4 = RetDT.Copy
                RetDS.Tables.Add(RetDT4)

                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                RetDT = New DataTable
                RetDT.TableName = "VALIDATE"
                RetDT.Columns.Add("Status")
                RetDT.Columns.Add("Msg")
                RetDT.Rows.Add()
                RetDT.Rows(0)(0) = True
                RetDT.Rows(0)(1) = "No Record Found"
                RetDS.Tables.Add(RetDT)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            End If
        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
 <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub CustomerContract_PreviousRecord(ByVal value As String)
        sFunction = "CustomerContract_PreviousRecord"
        Try
            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim ds As DataSet = fn.jsontodata(value)
            Dim LEADM As New DataTable
            Dim DocNum As String = ""
            If ds.Tables("CCON").Rows.Count > 0 Then
                LEADM = ds.Tables("CCON")
                Dim dr As DataRow = LEADM.Rows(0)
                DocNum = dr.Item("U_Qno").ToString
                Dim Query As String = ""

                Dim ErrMsg As String = ""
                Dim RetDT As New DataTable
                Dim RetDT1 As New DataTable
                Dim RetDT2 As New DataTable
                Dim RetDT3 As New DataTable
                Dim RetDT4 As New DataTable
                Dim RetDS = New DataSet()
                Dim DocEntry As String = ""

                If DocNum = "" Then
                    Query = "SELECT Top 1 T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"",  T0.""U_Ccode"", T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@CCON""  T0 ORDER BY T0.""DocEntry"" ASC"

                Else
                    Query = "SELECT Top 1 T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"",  T0.""U_Ccode"", T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@CCON"" T0  where T0.""DocEntry"" =(SELECT (T0.""DocEntry"" - 1)  ""DocEntry"" FROM ""@CCON""  T0 WHERE T0.""DocNum"" ='" & DocNum & "')"
                End If

                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                If RetDT.Rows.Count > 0 Then

                    DocEntry = RetDT.Rows(0)(0)
                    RetDT.TableName = "CCON"
                    RetDT1 = RetDT.Copy
                    RetDS.Tables.Add(RetDT1)

                    RetDT = New DataTable

                    Query = "SELECT T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", T1.""U_Rate"",T1.""U_RRate"", T1.""U_UOM"", T1.""U_GST"", T1.""U_Remarks"" FROM ""@CCONGENERAL""  T1 WHERE T1.""DocEntry"" = '" & DocEntry & "'"
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "CCONGEN"
                    RetDT2 = RetDT.Copy
                    RetDS.Tables.Add(RetDT2)


                    RetDT = New DataTable
                    Query = "SELECT T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", T0.""U_Rate"", T0.""U_UOM"", T0.""U_GST"", T0.""U_Remarks"" FROM ""@CCONADDON""  T0 WHERE T0.""DocEntry""  = '" & DocEntry & "'"
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "CCONADD"
                    RetDT3 = RetDT.Copy
                    RetDS.Tables.Add(RetDT3)

                    RetDT = New DataTable
                    Query = "SELECT ""U_Fname"" AS ""U_FileName"",""U_Path"" AS ""U_FilePath"",""LineId"" AS ""U_id"", TO_CHAR(""U_AttDate"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                            " FROM ""@CCONATTACHMENT"" WHERE ""DocEntry"" = '" & DocEntry & "' "
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "ATTACHMENT"
                    RetDT4 = RetDT.Copy
                    RetDS.Tables.Add(RetDT4)

                    Context.Response.Output.Write(fn.ds2json(RetDS))
                Else

                    Query = "SELECT Top 1 T0.""DocNum"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"",  T0.""U_Ccode"", T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@CCON""  T0 ORDER BY T0.""DocEntry"" DESC"
                    RetDT = New DataTable
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    If RetDT.Rows.Count > 0 Then
                        DocEntry = RetDT.Rows(0)(0)
                        RetDT.TableName = "CCON"
                        RetDT1 = RetDT.Copy
                        RetDS.Tables.Add(RetDT1)

                        RetDT = New DataTable

                        Query = "SELECT T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", T1.""U_Rate"",T1.""U_RRate"", T1.""U_UOM"", T1.""U_GST"", T1.""U_Remarks"" FROM ""@CCONGENERAL""  T1 WHERE T1.""DocEntry"" = '" & DocEntry & "'"
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "CCONGEN"
                        RetDT2 = RetDT.Copy
                        RetDS.Tables.Add(RetDT2)


                        RetDT = New DataTable
                        Query = "SELECT T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", T0.""U_Rate"", T0.""U_UOM"", T0.""U_GST"", T0.""U_Remarks"" FROM ""@CCONADDON""  T0 WHERE T0.""DocEntry""  = '" & DocEntry & "'"
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "CCONADD"
                        RetDT3 = RetDT.Copy
                        RetDS.Tables.Add(RetDT3)

                        RetDT = New DataTable
                        Query = "SELECT ""U_Fname"" AS ""U_FileName"",""U_Path"" AS ""U_FilePath"",""LineId"" AS ""U_id"", TO_CHAR(""U_AttDate"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                                " FROM ""@CCONATTACHMENT"" WHERE ""DocEntry"" = '" & DocEntry & "' "
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "ATTACHMENT"
                        RetDT4 = RetDT.Copy
                        RetDS.Tables.Add(RetDT4)

                        Context.Response.Output.Write(fn.ds2json(RetDS))

                    Else
                        RetDT = New DataTable
                        RetDT.TableName = "VALIDATE"
                        RetDT.Columns.Add("Status")
                        RetDT.Columns.Add("Msg")
                        RetDT.Rows.Add()
                        RetDT.Rows(0)(0) = True
                        RetDT.Rows(0)(1) = "No Record Found"
                        RetDS.Tables.Add(RetDT)
                        Context.Response.Output.Write(fn.ds2json(RetDS))
                    End If
                End If
            Else
                Throw New Exception("No Record Found")
            End If
        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub CustomerContract_NextRecord(ByVal value As String)
        sFunction = "CustomerContract_NextRecord"
        Try
            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim ds As DataSet = fn.jsontodata(value)
            Dim LEADM As New DataTable
            Dim DocNum As String = ""
            If ds.Tables("CCON").Rows.Count > 0 Then
                LEADM = ds.Tables("CCON")
                Dim dr As DataRow = LEADM.Rows(0)
                DocNum = dr.Item("U_Qno").ToString
                Dim Query As String = ""

                Dim ErrMsg As String = ""
                Dim RetDT As New DataTable
                Dim RetDT1 As New DataTable
                Dim RetDT2 As New DataTable
                Dim RetDT3 As New DataTable
                Dim RetDT4 As New DataTable
                Dim RetDS = New DataSet()
                Dim DocEntry As String = ""

                If DocNum = "" Then
                    Query = "SELECT Top 1 T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"",  T0.""U_Ccode"", T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@CCON""  T0 ORDER BY T0.""DocEntry"" DESC"

                Else
                    Query = "SELECT Top 1 T0.""DocNum"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"",T0.""U_Ccode"", T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@CCON"" T0  where T0.""DocEntry"" =(SELECT (T0.""DocEntry"" + 1)  ""DocEntry"" FROM ""@CCON""  T0 WHERE T0.""DocNum"" ='" & DocNum & "')"
                End If

                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                If RetDT.Rows.Count > 0 Then

                    DocEntry = RetDT.Rows(0)(0)
                    RetDT.TableName = "CCON"
                    RetDT1 = RetDT.Copy
                    RetDS.Tables.Add(RetDT1)

                    RetDT = New DataTable

                    Query = "SELECT T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", T1.""U_Rate"",T1.""U_RRate"", T1.""U_UOM"", T1.""U_GST"", T1.""U_Remarks"" FROM ""@CCONGENERAL""  T1 WHERE T1.""DocEntry"" = '" & DocEntry & "'"
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "CCONGEN"
                    RetDT2 = RetDT.Copy
                    RetDS.Tables.Add(RetDT2)


                    RetDT = New DataTable
                    Query = "SELECT T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", T0.""U_Rate"", T0.""U_UOM"", T0.""U_GST"", T0.""U_Remarks"" FROM ""@CCONADDON""  T0 WHERE T0.""DocEntry""  = '" & DocEntry & "'"
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "CCONADD"
                    RetDT3 = RetDT.Copy
                    RetDS.Tables.Add(RetDT3)

                    RetDT = New DataTable
                    Query = "SELECT ""U_Fname"" AS ""U_FileName"",""U_Path"" AS ""U_FilePath"",""LineId"" AS ""U_id"", TO_CHAR(""U_AttDate"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                            " FROM ""@CCONATTACHMENT"" WHERE ""DocEntry"" = '" & DocEntry & "' "
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "ATTACHMENT"
                    RetDT4 = RetDT.Copy
                    RetDS.Tables.Add(RetDT4)

                    Context.Response.Output.Write(fn.ds2json(RetDS))
                Else

                    Query = "SELECT Top 1 T0.""DocNum"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"",  T0.""U_Ccode"", T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@CCON""  T0 ORDER BY T0.""DocEntry"" ASC"
                    RetDT = New DataTable
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    If RetDT.Rows.Count > 0 Then
                        DocEntry = RetDT.Rows(0)(0)
                        RetDT.TableName = "CCON"
                        RetDT1 = RetDT.Copy
                        RetDS.Tables.Add(RetDT1)

                        RetDT = New DataTable

                        Query = "SELECT T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", T1.""U_Rate"",T1.""U_RRate"", T1.""U_UOM"", T1.""U_GST"", T1.""U_Remarks"" FROM ""@CCONGENERAL""  T1 WHERE T1.""DocEntry"" = '" & DocEntry & "'"
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "CCONGEN"
                        RetDT2 = RetDT.Copy
                        RetDS.Tables.Add(RetDT2)


                        RetDT = New DataTable
                        Query = "SELECT T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", T0.""U_Rate"", T0.""U_UOM"", T0.""U_GST"", T0.""U_Remarks"" FROM ""@CCONADDON""  T0 WHERE T0.""DocEntry""  = '" & DocEntry & "'"
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "CCONADD"
                        RetDT3 = RetDT.Copy
                        RetDS.Tables.Add(RetDT3)

                        RetDT = New DataTable
                        Query = "SELECT ""U_Fname"" AS ""U_FileName"",""U_Path"" AS ""U_FilePath"",""LineId"" AS ""U_id"", TO_CHAR(""U_AttDate"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                                " FROM ""@CCONATTACHMENT"" WHERE ""DocEntry"" = '" & DocEntry & "' "
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "ATTACHMENT"
                        RetDT4 = RetDT.Copy
                        RetDS.Tables.Add(RetDT4)

                        Context.Response.Output.Write(fn.ds2json(RetDS))

                    Else
                        RetDT = New DataTable
                        RetDT.TableName = "VALIDATE"
                        RetDT.Columns.Add("Status")
                        RetDT.Columns.Add("Msg")
                        RetDT.Rows.Add()
                        RetDT.Rows(0)(0) = True
                        RetDT.Rows(0)(1) = "No Record Found"
                        RetDS.Tables.Add(RetDT)
                        Context.Response.Output.Write(fn.ds2json(RetDS))
                    End If
                End If
            Else
                Throw New Exception("No Record Found")
            End If
        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
 <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub CustomerContract_FindRecord_List(ByVal value As String)
        sFunction = "CustomerContract_FindRecord_List"
        Try
            oLog.WriteToLogFile_Debug(value, sFunction)
            Dim ds As DataSet = fn.jsontodata(value)
            Dim LEADM As New DataTable
            Dim DocNum As String = ""
            If ds.Tables("CCON").Rows.Count > 0 Then
                LEADM = ds.Tables("CCON")
                Dim dr As DataRow = LEADM.Rows(0)
                Try
                    DocNum = dr.Item("U_Qno").ToString.Trim()
                Catch ex As Exception

                End Try

                Dim CreateDate As String = ""
                Dim CardCode As String = ""
                Dim CardName As String = ""
                Dim ProjectCode As String = ""

                CreateDate = dr.Item("U_Cdate").ToString.Trim()
                CardCode = dr.Item("U_Ccode").ToString.Trim()
                CardName = dr.Item("U_Cname").ToString.Trim()
                ProjectCode = dr.Item("U_Pcode").ToString.Trim()

                If CreateDate = "" Then
                    CreateDate = "%"
                ElseIf CreateDate.Contains("*") = True Then
                    CreateDate = CreateDate.Replace("*", "%")
                Else
                    CreateDate = "%" & CreateDate & "%"
                End If

                If CardCode = "" Then
                    CardCode = "%"
                ElseIf CardCode.Contains("*") = True Then
                    CardCode = CardCode.Replace("*", "%")
                Else
                    CardCode = "%" & CardCode & "%"
                End If

                If CardName = "" Then
                    CardName = "%"
                ElseIf CardName.Contains("*") = True Then
                    CardName = CardName.Replace("*", "%")
                Else
                    CardName = "%" & CardName & "%"
                End If

                If ProjectCode = "" Then
                    ProjectCode = "%"
                ElseIf ProjectCode.Contains("*") = True Then
                    ProjectCode = ProjectCode.Replace("*", "%")
                Else
                    ProjectCode = "%" & ProjectCode & "%"
                End If


                Dim Query As String = ""
                Dim ErrMsg As String = ""
                Dim RetDT As New DataTable
                Dim RetDT1 As New DataTable
                Dim RetDT2 As New DataTable
                Dim RetDT3 As New DataTable
                Dim RetDT4 As New DataTable
                Dim RetDS = New DataSet()
                Dim DocEntry As String = ""

                If DocNum = "" Then
                    Query = "SELECT T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", T0.""U_Ccode"", " & _
                            " T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", " & _
                            " T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", " & _
                            " T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" " & _
                            " FROM ""@CCON"" T0 " & _
                            " where UPPER(T0.""U_Ccode"") like '" & CardCode.ToUpper() & "' " & _
                            " and  T0.""U_Cdate"" like '" & CreateDate & "'  and  UPPER(T0.""U_Cname"") like '" & CardName.Replace("'", "''").ToUpper() & "' " & _
                            " and  T0.""U_Pcode"" like '" & ProjectCode & "'    ORDER BY T0.""DocEntry"" DESC"

                Else
                    Query = "SELECT Top 1 T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"",T0.""U_Ccode"", " & _
                            " T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", " & _
                            " T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", " & _
                            " T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@CCON"" T0  where T0.""DocEntry"" ='" & DocNum & "'"
                End If

                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                If RetDT.Rows.Count > 0 Then
                    DocEntry = RetDT.Rows(0)(0)
                    RetDT.TableName = "CCON"
                    RetDT1 = RetDT.Copy
                    RetDS.Tables.Add(RetDT1)

                    'RetDT = New DataTable
                    'Query = "SELECT T1.""DocEntry"" ,T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", T1.""U_Rate"", T1.""U_UOM"", T1.""U_GST"", T1.""U_Remarks"" FROM ""@CCONGENERAL""  T1 WHERE T1.""DocEntry"" = '" & DocEntry & "'"
                    'RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    'If ErrMsg <> "" Then
                    '    Throw New Exception(ErrMsg)
                    'End If
                    'RetDT.TableName = "CCONGEN"
                    'RetDT2 = RetDT.Copy
                    'RetDS.Tables.Add(RetDT2)


                    'RetDT = New DataTable
                    'Query = "SELECT T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", T0.""U_Rate"", T0.""U_UOM"", T0.""U_GST"", T0.""U_Remarks"" FROM ""@CCONADDON""  T0 WHERE T0.""DocEntry""  = '" & DocEntry & "'"
                    'RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    'If ErrMsg <> "" Then
                    '    Throw New Exception(ErrMsg)
                    'End If
                    'RetDT.TableName = "CCONADD"
                    'RetDT3 = RetDT.Copy
                    'RetDS.Tables.Add(RetDT3)

                    Context.Response.Output.Write(fn.ds2json(RetDS))

                Else
                    Throw New Exception("No Record Found")
                End If
            Else
                Throw New Exception("No Record Found")
            End If
        Catch ex As Exception
            oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
  <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub CustomerContract_FindRecord(ByVal value As String)
        sFunction = "CustomerContract_FindRecord"
        Try
            oLog.WriteToLogFile_Debug(value, sFunction)
            Dim ds As DataSet = fn.jsontodata(value)
            Dim LEADM As New DataTable
            Dim DocNum As String = ""
            If ds.Tables("CCON").Rows.Count > 0 Then
                LEADM = ds.Tables("CCON")
                Dim dr As DataRow = LEADM.Rows(0)
                DocNum = dr.Item("U_Qno").ToString
                'Dim CreateDate As String = ""
                'Dim CardCode As String = ""
                'Dim CardName As String = ""
                'Dim ProjectCode As String = ""

                'CreateDate = dr.Item("U_Cdate").ToString
                'CardCode = dr.Item("U_Ccode").ToString
                'CardName = dr.Item("U_Cname").ToString
                'ProjectCode = dr.Item("U_Pcode").ToString


                'If CreateDate = "" Then
                '    CreateDate = "%"
                'ElseIf CreateDate.Contains("*") = True Then
                '    CreateDate = CreateDate.Replace("*", "%")
                'Else
                '    CreateDate = "%" & CreateDate & "%"
                'End If

                'If CardCode = "" Then
                '    CardCode = "%"
                'ElseIf CardCode.Contains("*") = True Then
                '    CardCode = CardCode.Replace("*", "%")
                'Else
                '    CardCode = "%" & CardCode & "%"
                'End If

                'If CardName = "" Then
                '    CardName = "%"
                'ElseIf CardName.Contains("*") = True Then
                '    CardName = CardName.Replace("*", "%")
                'Else
                '    CardName = "%" & CardName & "%"
                'End If

                'If ProjectCode = "" Then
                '    ProjectCode = "%"
                'ElseIf ProjectCode.Contains("*") = True Then
                '    ProjectCode = ProjectCode.Replace("*", "%")
                'Else
                '    ProjectCode = "%" & ProjectCode & "%"
                'End If


                Dim Query As String = ""
                Dim ErrMsg As String = ""
                Dim RetDT As New DataTable
                Dim RetDT1 As New DataTable
                Dim RetDT2 As New DataTable
                Dim RetDT3 As New DataTable
                Dim RetDT4 As New DataTable
                Dim RetDS = New DataSet()
                Dim DocEntry As String = ""

                If DocNum = "" Then

                    Throw New Exception("No Record Found")
                    ' Query = "SELECT Top 1 T0.""DocNum"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", TO_CHAR( T0.""U_Qdate1"" , 'DD/MM/YYYY') As ""U_Qdate1"", TO_CHAR( T0.""U_Qdate2"" , 'DD/MM/YYYY') As ""U_Qdate2"", T0.""U_Ccode"", T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@CCON""  where T0.""U_Ccode"" like '" & CardCode & "' and  T0.""U_Cdate"" like '" & DocNum & "'  and  T0.""U_Cname"" like '" & CardName & "'   and  T0.""U_Pcode"" like '" & ProjectCode & "'   T0 ORDER BY T0.""DocEntry"" DESC"

                Else
                    Query = "SELECT Top 1 T0.""DocNum"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"",  T0.""U_Ccode"", T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@CCON"" T0  where T0.""DocEntry"" ='" & DocNum & "'"
                End If

                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                If RetDT.Rows.Count > 0 Then

                    DocEntry = RetDT.Rows(0)(0)
                    RetDT.TableName = "CCON"
                    RetDT1 = RetDT.Copy
                    RetDS.Tables.Add(RetDT1)

                    RetDT = New DataTable

                    Query = "SELECT T1.""DocEntry"" ,T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", T1.""U_Rate"",T1.""U_RRate"", T1.""U_UOM"", T1.""U_GST"", T1.""U_Remarks"" FROM ""@CCONGENERAL""  T1 WHERE T1.""DocEntry"" = '" & DocEntry & "'"
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "CCONGEN"
                    RetDT2 = RetDT.Copy
                    RetDS.Tables.Add(RetDT2)


                    RetDT = New DataTable
                    Query = "SELECT T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", T0.""U_Rate"", T0.""U_UOM"", T0.""U_GST"", T0.""U_Remarks"" FROM ""@CCONADDON""  T0 WHERE T0.""DocEntry""  = '" & DocEntry & "'"
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "CCONADD"
                    RetDT3 = RetDT.Copy
                    RetDS.Tables.Add(RetDT3)

                    RetDT = New DataTable
                    Query = "SELECT ""U_Fname"" AS ""U_FileName"",""U_Path"" AS ""U_FilePath"",""LineId"" AS ""U_id"", TO_CHAR(""U_AttDate"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                            " FROM ""@CCONATTACHMENT"" WHERE ""DocEntry"" = '" & DocEntry & "' "
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "ATTACHMENT"
                    RetDT4 = RetDT.Copy
                    RetDS.Tables.Add(RetDT4)

                    Context.Response.Output.Write(fn.ds2json(RetDS))
                Else
                    Throw New Exception("No Record Found")
                    Query = "SELECT Top 1 T0.""DocNum"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", T0.""U_Ccode"", T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@CCON""  T0 ORDER BY T0.""DocEntry"" DESC"
                    RetDT = New DataTable
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    If RetDT.Rows.Count > 0 Then
                        DocEntry = RetDT.Rows(0)(0)
                        RetDT.TableName = "CCON"
                        RetDT1 = RetDT.Copy
                        RetDS.Tables.Add(RetDT1)

                        RetDT = New DataTable

                        Query = "SELECT T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", T1.""U_Rate"",T1.""U_RRate"", T1.""U_UOM"", T1.""U_GST"", T1.""U_Remarks"" FROM ""@CCONGENERAL""  T1 WHERE T1.""DocEntry"" = '" & DocEntry & "'"
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "CCONGEN"
                        RetDT2 = RetDT.Copy
                        RetDS.Tables.Add(RetDT2)


                        RetDT = New DataTable
                        Query = "SELECT T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", T0.""U_Rate"", T0.""U_UOM"", T0.""U_GST"", T0.""U_Remarks"" FROM ""@CCONADDON""  T0 WHERE T0.""DocEntry""  = '" & DocEntry & "'"
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "CCONADD"
                        RetDT3 = RetDT.Copy
                        RetDS.Tables.Add(RetDT3)

                        RetDT = New DataTable
                        Query = "SELECT ""U_Fname"" AS ""U_FileName"",""U_Path"" AS ""U_FilePath"",""LineId"" AS ""U_id"", TO_CHAR(""U_AttDate"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                                " FROM ""@CCONATTACHMENT"" WHERE ""DocEntry"" = '" & DocEntry & "' "
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "ATTACHMENT"
                        RetDT4 = RetDT.Copy
                        RetDS.Tables.Add(RetDT4)

                        Context.Response.Output.Write(fn.ds2json(RetDS))

                    Else
                        RetDT = New DataTable
                        RetDT.TableName = "VALIDATE"
                        RetDT.Columns.Add("Status")
                        RetDT.Columns.Add("Msg")
                        RetDT.Rows.Add()
                        RetDT.Rows(0)(0) = True
                        RetDT.Rows(0)(1) = "No Record Found"
                        RetDS.Tables.Add(RetDT)
                        Context.Response.Output.Write(fn.ds2json(RetDS))
                    End If
                End If
            Else
                Throw New Exception("No Record Found")
            End If
        Catch ex As Exception
            oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
#End Region
#Region "Agent Contract"
    <WebMethod()> _
        <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub AgentContractAdd(ByVal value As String)
        sFunction = "AgentContract"
        Try

            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim SQTO As New DataTable
            SQTO = New DataTable
            Dim SQTOGEN As New DataTable
            Dim SQTOADD As New DataTable
            Dim SQTOATTACH As New DataTable
            Dim SQTODS = New DataSet()
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            fn.GetSAPConnection(Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            Dim oGeneralService As SAPbobsCOM.GeneralService
            Dim oGeneralData As SAPbobsCOM.GeneralData
            Dim oSons As SAPbobsCOM.GeneralDataCollection
            Dim oSon As SAPbobsCOM.GeneralData
            Dim sCmp As SAPbobsCOM.CompanyService
            Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

            sCmp = PublicVariable.oCompany.GetCompanyService
            oGeneralService = sCmp.GetGeneralService("ACON")
            oGeneralData = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)
            oGeneralParams = CType(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams), SAPbobsCOM.GeneralDataParams)
            ' oGeneralData = oGeneralService.GetByParams(oGeneralParams)


            If ds.Tables("ACON").Rows.Count > 0 Then
                SQTO = ds.Tables("ACON")
                Dim dr As DataRow = SQTO.Rows(0)
                ' Dim oRecord As SAPbobsCOM.Recordset
                'oRecord = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                'oRecord.DoQuery("select U_Qno from [@SQTO] where U_Qno='" & dr.Item("U_Qno") & "'")
                ' If oRecord.RecordCount = 0 Then
                'oGeneralData.SetProperty("U_Qno", "Open")
                oGeneralData.SetProperty("U_Status", "Open")
                oGeneralData.SetProperty("U_Uname", dr.Item("U_Uname").ToString.Trim())
                oGeneralData.SetProperty("U_Cdate", dr.Item("U_Cdate").ToString.Trim())
                'oGeneralData.SetProperty("U_Qdate1", dr.Item("U_Qdate1").ToString.Trim())
                'oGeneralData.SetProperty("U_Qdate2", dr.Item("U_Qdate2").ToString.Trim())
                oGeneralData.SetProperty("U_Agentcode", dr.Item("U_Agentcode").ToString.Trim())
                oGeneralData.SetProperty("U_Agentname", dr.Item("U_Agentname").ToString.Trim())
                oGeneralData.SetProperty("U_CPeriod1", dr.Item("U_CPeriod1").ToString.Trim())
                oGeneralData.SetProperty("U_CPeriod2", dr.Item("U_CPeriod2").ToString.Trim())
                oGeneralData.SetProperty("U_Pcode", dr.Item("U_Pcode").ToString.Trim())
                oGeneralData.SetProperty("U_AddrN", dr.Item("U_AddrN").ToString.Trim())
                oGeneralData.SetProperty("U_Addr1", dr.Item("U_Addr1").ToString.Trim())
                oGeneralData.SetProperty("U_Addr2", dr.Item("U_Addr2").ToString.Trim())
                oGeneralData.SetProperty("U_Addr3", dr.Item("U_Addr3").ToString.Trim())
                oGeneralData.SetProperty("U_Addr4", dr.Item("U_Addr4").ToString.Trim())
                oGeneralData.SetProperty("U_Addr5", dr.Item("U_Addr5").ToString.Trim())
                oGeneralData.SetProperty("U_Addr6", dr.Item("U_Addr6").ToString.Trim())
                Try
                    oGeneralData.SetProperty("U_TelNo", dr.Item("U_TelNo").ToString.Trim())
                    oGeneralData.SetProperty("U_FaxNo", dr.Item("U_FaxNo").ToString.Trim())
                    oGeneralData.SetProperty("U_Mno", dr.Item("U_Mno").ToString.Trim())
                Catch ex As Exception
                End Try
                oGeneralData.SetProperty("U_Email", dr.Item("U_Email").ToString.Trim())
                oGeneralData.SetProperty("U_Remarks", dr.Item("U_Remarks").ToString.Trim())
                If ds.Tables("ACONGEN").Rows.Count > 0 Then
                    SQTOGEN = ds.Tables("ACONGEN")
                    oSons = oGeneralData.Child("ACONGENERAL")
                    For Each dr1 As DataRow In SQTOGEN.Rows
                        oSon = oSons.Add
                        If dr1.Item("U_Ccode").ToString.Trim() = "" Then
                            Throw New Exception("The ""Customer Code"" field must not be empty; enter a value for ""Customer Code"".")
                        End If
                        oSon.SetProperty("U_Ccode", dr1.Item("U_Ccode").ToString.Trim())
                        If dr1.Item("U_Cname").ToString.Trim() = "" Then
                            Throw New Exception("The ""Customer Name"" field must not be empty; enter a value for ""Customer Name"".")
                        End If
                        oSon.SetProperty("U_Cname", dr1.Item("U_Cname").ToString.Trim())

                        If dr1.Item("U_Stype").ToString.Trim() = "" Then
                            Throw New Exception("The ""Survey Type"" field must not be empty; enter a value for ""Survey Type"".")
                        End If
                        oSon.SetProperty("U_Stype", dr1.Item("U_Stype").ToString.Trim())

                        If dr1.Item("U_Conti").ToString.Trim() = "" Then
                            Throw New Exception("The ""Continent"" field must not be empty; enter a value for ""Continent"".")
                        End If
                        oSon.SetProperty("U_Conti", dr1.Item("U_Conti").ToString.Trim())

                        If dr1.Item("U_Country").ToString.Trim() = "" Then
                            Throw New Exception("The ""Country"" field must not be empty; enter a value for ""Country"".")
                        End If
                        oSon.SetProperty("U_Country", dr1.Item("U_Country").ToString.Trim())

                        If dr1.Item("U_City").ToString.Trim() = "" Then
                            Throw New Exception("The ""City"" field must not be empty; enter a value for ""City"".")
                        End If
                        oSon.SetProperty("U_City", dr1.Item("U_City").ToString.Trim())

                        If dr1.Item("U_Currency").ToString.Trim() = "" Then
                            Throw New Exception("The ""Currency"" field must not be empty; enter a value for ""Currency"".")
                        End If
                        oSon.SetProperty("U_Currency", dr1.Item("U_Currency").ToString.Trim())

                        If dr1.Item("U_EQGroup").ToString.Trim() = "" Then
                            Throw New Exception("The ""Equipment Group"" field must not be empty; enter a value for ""Equipment Group"".")
                        End If
                        oSon.SetProperty("U_EQGroup", dr1.Item("U_EQGroup").ToString.Trim())
                        oSon.SetProperty("U_Rate", dr1.Item("U_Rate").ToString.Trim())
                        oSon.SetProperty("U_UOM", dr1.Item("U_UOM").ToString.Trim())
                        If dr1.Item("U_GST").ToString.Trim() = "" Then
                            Throw New Exception("The ""GST"" field must not be empty; enter a value for ""GST"".")
                        End If
                        oSon.SetProperty("U_GST", dr1.Item("U_GST").ToString.Trim())
                        oSon.SetProperty("U_Remarks", dr1.Item("U_Remarks").ToString.Trim())
                    Next
                Else
                    Throw New Exception("The ""General Tab"" must not be empty; enter a value for ""General Tab"".")
                End If
                If ds.Tables("ACONADD").Rows.Count > 0 Then
                    SQTOADD = ds.Tables("ACONADD")
                    oSons = oGeneralData.Child("ACONADDON")
                    For Each dr1 As DataRow In SQTOADD.Rows
                        oSon = oSons.Add
                        If dr1.Item("U_Ccode").ToString.Trim() = "" Then
                            Throw New Exception("The ""Customer Code"" field must not be empty; enter a value for ""Customer Code"".")
                        End If
                        oSon.SetProperty("U_Ccode", dr1.Item("U_Ccode").ToString.Trim())
                        If dr1.Item("U_Cname").ToString.Trim() = "" Then
                            Throw New Exception("The ""Customer Name"" field must not be empty; enter a value for ""Customer Name"".")
                        End If
                        oSon.SetProperty("U_Cname", dr1.Item("U_Cname").ToString.Trim())

                        If dr1.Item("U_Ctype").ToString.Trim() = "" Then
                            Throw New Exception("The ""Charge Type"" field must not be empty; enter a value for ""Charge Type"".")
                        End If
                        oSon.SetProperty("U_Ctype", dr1.Item("U_Ctype").ToString.Trim())

                        If dr1.Item("U_Continent").ToString.Trim() = "" Then
                            Throw New Exception("The ""Continent"" field must not be empty; enter a value for ""Continent"".")
                        End If
                        oSon.SetProperty("U_Continent", dr1.Item("U_Continent").ToString.Trim())

                        If dr1.Item("U_Country").ToString.Trim() = "" Then
                            Throw New Exception("The ""Country"" field must not be empty; enter a value for ""Country"".")
                        End If
                        oSon.SetProperty("U_Country", dr1.Item("U_Country").ToString.Trim())

                        If dr1.Item("U_City").ToString.Trim() = "" Then
                            Throw New Exception("The ""City"" field must not be empty; enter a value for ""City"".")
                        End If
                        oSon.SetProperty("U_City", dr1.Item("U_City").ToString.Trim())

                        If dr1.Item("U_Currency").ToString.Trim() = "" Then
                            Throw New Exception("The ""Currency"" field must not be empty; enter a value for ""Currency"".")
                        End If
                        oSon.SetProperty("U_Currency", dr1.Item("U_Currency").ToString.Trim())

                        If dr1.Item("U_EQGroup").ToString.Trim() = "" Then
                            Throw New Exception("The ""Equipment Group"" field must not be empty; enter a value for ""Equipment Group"".")
                        End If
                        oSon.SetProperty("U_EQGroup", dr1.Item("U_EQGroup").ToString.Trim())

                        oSon.SetProperty("U_Rate", dr1.Item("U_Rate").ToString.Trim())
                        oSon.SetProperty("U_UOM", dr1.Item("U_UOM").ToString.Trim())

                        If dr1.Item("U_GST").ToString.Trim() = "" Then
                            Throw New Exception("The ""GST"" field must not be empty; enter a value for ""GST"".")
                        End If
                        oSon.SetProperty("U_GST", dr1.Item("U_GST").ToString.Trim())
                        oSon.SetProperty("U_Remarks", dr1.Item("U_Remarks").ToString.Trim())
                    Next
                End If
                If ds.Tables("ATTACHMENT").Rows.Count > 0 Then
                    SQTOATTACH = ds.Tables("ATTACHMENT")
                    oSons = oGeneralData.Child("ACONATTACHMENT")
                    For Each odr As DataRow In SQTOATTACH.Rows
                        oSon = oSons.Add
                        Try
                            oSon.SetProperty("U_Path", odr.Item("U_FilePath").ToString.Trim())
                        Catch ex As Exception
                            Throw New Exception("Error while adding file path to attachment folder")
                        End Try
                        Try
                            oSon.SetProperty("U_Fname", odr.Item("U_FileName").ToString.Trim())
                        Catch ex As Exception
                            Throw New Exception("Error while adding file name to attachment folder")
                        End Try
                        Try
                            Dim dtAttachDate As Date
                            dtAttachDate = GetDateTimeValue(odr.Item("U_Date").ToString)
                            oSon.SetProperty("U_AttDate", dtAttachDate)
                        Catch ex As Exception
                            Throw New Exception("Error while adding attachment date to attachment folder")
                        End Try

                    Next
                End If

                Dim response = oGeneralService.Add(oGeneralData)
                DocEntry = response.GetProperty("DocEntry")
            End If

            Dim RetDT As New DataTable
            RetDT.TableName = "VALIDATE"
            RetDT.Columns.Add("Status")
            RetDT.Columns.Add("Msg")
            RetDT.Rows.Add()
            RetDT.Rows(0)(0) = True
            RetDT.Rows(0)(1) = "Agent Contract no. " & DocEntry.ToString & " Created Successfully"
            RetDS.Tables.Add(RetDT)
            Context.Response.Output.Write(fn.ds2json(RetDS))


        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
        <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub AgentContract_Update(ByVal value As String)
        sFunction = "AgentContract_Update"
        Try

            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim SQTO As New DataTable
            SQTO = New DataTable
            Dim SQTOGEN As New DataTable
            Dim SQTOADD As New DataTable
            Dim SQTOATTACH As New DataTable
            Dim SQTODS = New DataSet()
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            fn.GetSAPConnection(Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            Dim oGeneralService As SAPbobsCOM.GeneralService
            Dim oGeneralData As SAPbobsCOM.GeneralData
            Dim oSons As SAPbobsCOM.GeneralDataCollection
            Dim oSon As SAPbobsCOM.GeneralData
            Dim sCmp As SAPbobsCOM.CompanyService
            Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

            sCmp = PublicVariable.oCompany.GetCompanyService
            oGeneralService = sCmp.GetGeneralService("ACON")
            oGeneralData = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)
            oGeneralParams = CType(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams), SAPbobsCOM.GeneralDataParams)
            ' oGeneralData = oGeneralService.GetByParams(oGeneralParams)
            '  Dim RetDT As New DataTable
            ' RetDT = fn.ExecuteSQLQuery(Query, Errmsg)
            Dim Count_Gen As String = ""
            Dim Count_Addon As String = ""
            Dim Count_Attach As String = ""
            Dim Count_Gen_Int As Integer = 0
            Dim Count_Addon_Int As Integer = 0
            Dim Count_Attach_Int As Integer = 0
            Dim Query As String = ""


            If ds.Tables("ACON").Rows.Count > 0 Then
                SQTO = ds.Tables("ACON")
                Dim dr As DataRow = SQTO.Rows(0)


                DocEntry = dr.Item("U_Qno").ToString.Trim()
                Query = "SELECT COunt(T0.""DocEntry"") FROM ""@ACONGENERAL""  T0 WHERE T0.""DocEntry"" =" & DocEntry & ""
                Count_Gen = fn.ExecuteSQLQuery_SingleValue(Query, Errmsg)
                If Errmsg <> "" Then
                    Throw New Exception(Errmsg)
                End If
                If Count_Gen <> "" Then
                    Count_Gen_Int = CInt(Count_Gen)
                End If
                Query = "SELECT COunt(T0.""DocEntry"") FROM ""@ACONADDON""  T0 WHERE T0.""DocEntry"" =" & DocEntry & ""
                Count_Addon = fn.ExecuteSQLQuery_SingleValue(Query, Errmsg)
                If Errmsg <> "" Then
                    Throw New Exception(Errmsg)
                End If
                If Count_Addon <> "" Then
                    Count_Addon_Int = CInt(Count_Addon)
                End If

                Query = "SELECT COUNT(T0.""DocEntry"") FROM ""@ACONATTACHMENT"" T0 WHERE T0.""DocEntry"" = '" & DocEntry & "' "
                Count_Attach = fn.ExecuteSQLQuery_SingleValue(Query, Errmsg)
                If Errmsg <> "" Then
                    Throw New Exception(Errmsg)
                End If
                If Count_Attach <> "" Then
                    Count_Attach_Int = CInt(Count_Attach)
                End If

                oGeneralParams.SetProperty("DocEntry", dr.Item("U_Qno").ToString.Trim())
                oGeneralData = oGeneralService.GetByParams(oGeneralParams)

                'oGeneralData.SetProperty("U_Qno", dr.Item("U_Qno"))
                '  oGeneralData.SetProperty("U_Status", dr.Item("U_Status").ToString.Trim())
                oGeneralData.SetProperty("U_Uname", dr.Item("U_Uname").ToString.Trim())
                oGeneralData.SetProperty("U_Cdate", dr.Item("U_Cdate").ToString.Trim())
                'oGeneralData.SetProperty("U_Qdate1", dr.Item("U_Qdate1").ToString.Trim())
                'oGeneralData.SetProperty("U_Qdate2", dr.Item("U_Qdate2").ToString.Trim())
                oGeneralData.SetProperty("U_Agentcode", dr.Item("U_Agentcode").ToString.Trim())
                oGeneralData.SetProperty("U_Agentname", dr.Item("U_Agentname").ToString.Trim())

                oGeneralData.SetProperty("U_CPeriod1", dr.Item("U_CPeriod1").ToString.Trim())
                oGeneralData.SetProperty("U_CPeriod2", dr.Item("U_CPeriod2").ToString.Trim())
                oGeneralData.SetProperty("U_Pcode", dr.Item("U_Pcode").ToString.Trim())
                oGeneralData.SetProperty("U_AddrN", dr.Item("U_AddrN").ToString.Trim())
                oGeneralData.SetProperty("U_Addr1", dr.Item("U_Addr1").ToString.Trim())
                oGeneralData.SetProperty("U_Addr2", dr.Item("U_Addr2").ToString.Trim())
                oGeneralData.SetProperty("U_Addr3", dr.Item("U_Addr3").ToString.Trim())
                oGeneralData.SetProperty("U_Addr4", dr.Item("U_Addr4").ToString.Trim())
                oGeneralData.SetProperty("U_Addr5", dr.Item("U_Addr5").ToString.Trim())
                oGeneralData.SetProperty("U_Addr6", dr.Item("U_Addr6").ToString.Trim())
                Try
                    oGeneralData.SetProperty("U_TelNo", dr.Item("U_TelNo").ToString.Trim())
                    oGeneralData.SetProperty("U_FaxNo", dr.Item("U_FaxNo").ToString.Trim())
                    oGeneralData.SetProperty("U_Mno", dr.Item("U_Mno").ToString.Trim())
                Catch ex As Exception
                End Try

                'oGeneralData.SetProperty("U_TelNo", dr.Item("U_TelNo").ToString.Trim())
                'oGeneralData.SetProperty("U_FaxNo", dr.Item("U_FaxNo").ToString.Trim())
                'oGeneralData.SetProperty("U_Mno", dr.Item("U_Mno").ToString.Trim())
                oGeneralData.SetProperty("U_Email", dr.Item("U_Email").ToString.Trim())
                oGeneralData.SetProperty("U_Remarks", dr.Item("U_Remarks").ToString.Trim())


                Dim LineNum As Integer = 0
                Dim SQTOGENnewLineNo As Integer = 0
                Dim LoopRow As Integer = 0
                Dim RemoveIndex As Integer = 0
                If ds.Tables("ACONGEN").Rows.Count > 0 Then
                    SQTOGEN = ds.Tables("ACONGEN")
                    oSons = oGeneralData.Child("ACONGENERAL")
                    SQTOGENnewLineNo = SQTOGEN.Rows.Count()

                    If SQTOGEN.Rows.Count > Count_Gen_Int Then
                        LoopRow = SQTOGEN.Rows.Count
                    Else
                        LoopRow = Count_Gen_Int
                    End If
                    Dim i As Integer = 0
                    RemoveIndex = (Count_Gen_Int - 1)
                    For i = 0 To LoopRow - 1
                        'For Each dr1 As DataRow In SQTOGEN.Rows
                        LineNum = LineNum + 1
                        If SQTOGENnewLineNo = Count_Gen_Int Then
                            ' oSon = oSons.Add
                            oSons.Item(LineNum - 1).SetProperty("U_Ccode", SQTOGEN.Rows(i).Item("U_Ccode").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Cname", SQTOGEN.Rows(i).Item("U_Cname").ToString.Trim())

                            oSons.Item(LineNum - 1).SetProperty("U_Stype", SQTOGEN.Rows(i).Item("U_Stype").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Conti", SQTOGEN.Rows(i).Item("U_Conti").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Country", SQTOGEN.Rows(i).Item("U_Country").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_City", SQTOGEN.Rows(i).Item("U_City").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Currency", SQTOGEN.Rows(i).Item("U_Currency").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_EQGroup", SQTOGEN.Rows(i).Item("U_EQGroup").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Rate", SQTOGEN.Rows(i).Item("U_Rate").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_UOM", SQTOGEN.Rows(i).Item("U_UOM").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_GST", SQTOGEN.Rows(i).Item("U_GST").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Remarks", SQTOGEN.Rows(i).Item("U_Remarks").ToString.Trim())
                        ElseIf SQTOGENnewLineNo > Count_Gen_Int Then
                            If LineNum <= Count_Gen_Int Then
                                oSons.Item(LineNum - 1).SetProperty("U_Ccode", SQTOGEN.Rows(i).Item("U_Ccode").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Cname", SQTOGEN.Rows(i).Item("U_Cname").ToString.Trim())

                                oSons.Item(LineNum - 1).SetProperty("U_Stype", SQTOGEN.Rows(i).Item("U_Stype").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Conti", SQTOGEN.Rows(i).Item("U_Conti").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Country", SQTOGEN.Rows(i).Item("U_Country").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_City", SQTOGEN.Rows(i).Item("U_City").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Currency", SQTOGEN.Rows(i).Item("U_Currency").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_EQGroup", SQTOGEN.Rows(i).Item("U_EQGroup").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Rate", SQTOGEN.Rows(i).Item("U_Rate").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_UOM", SQTOGEN.Rows(i).Item("U_UOM").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_GST", SQTOGEN.Rows(i).Item("U_GST").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Remarks", SQTOGEN.Rows(i).Item("U_Remarks").ToString.Trim())
                            Else
                                oSon = oSons.Add
                                oSon.SetProperty("U_Ccode", SQTOGEN.Rows(i).Item("U_Ccode").ToString.Trim())
                                oSon.SetProperty("U_Cname", SQTOGEN.Rows(i).Item("U_Cname").ToString.Trim())

                                oSon.SetProperty("U_Stype", SQTOGEN.Rows(i).Item("U_Stype").ToString.Trim())
                                oSon.SetProperty("U_Conti", SQTOGEN.Rows(i).Item("U_Conti").ToString.Trim())
                                oSon.SetProperty("U_Country", SQTOGEN.Rows(i).Item("U_Country").ToString.Trim())
                                oSon.SetProperty("U_City", SQTOGEN.Rows(i).Item("U_City").ToString.Trim())
                                oSon.SetProperty("U_Currency", SQTOGEN.Rows(i).Item("U_Currency").ToString.Trim())
                                oSon.SetProperty("U_EQGroup", SQTOGEN.Rows(i).Item("U_EQGroup").ToString.Trim())
                                oSon.SetProperty("U_Rate", SQTOGEN.Rows(i).Item("U_Rate").ToString.Trim())
                                oSon.SetProperty("U_UOM", SQTOGEN.Rows(i).Item("U_UOM").ToString.Trim())
                                oSon.SetProperty("U_GST", SQTOGEN.Rows(i).Item("U_GST").ToString.Trim())
                                oSon.SetProperty("U_Remarks", SQTOGEN.Rows(i).Item("U_Remarks").ToString.Trim())
                            End If
                        ElseIf SQTOGENnewLineNo < Count_Gen_Int Then
                            If LineNum <= SQTOGENnewLineNo Then
                                oSons.Item(LineNum - 1).SetProperty("U_Ccode", SQTOGEN.Rows(i).Item("U_Ccode").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Cname", SQTOGEN.Rows(i).Item("U_Cname").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Stype", SQTOGEN.Rows(i).Item("U_Stype").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Conti", SQTOGEN.Rows(i).Item("U_Conti").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Country", SQTOGEN.Rows(i).Item("U_Country").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_City", SQTOGEN.Rows(i).Item("U_City").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Currency", SQTOGEN.Rows(i).Item("U_Currency").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_EQGroup", SQTOGEN.Rows(i).Item("U_EQGroup").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Rate", SQTOGEN.Rows(i).Item("U_Rate").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_UOM", SQTOGEN.Rows(i).Item("U_UOM").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_GST", SQTOGEN.Rows(i).Item("U_GST").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Remarks", SQTOGEN.Rows(i).Item("U_Remarks").ToString.Trim())
                            Else
                                oSons.Remove(RemoveIndex - 1)
                                RemoveIndex = RemoveIndex - 1
                            End If

                        End If
                        'Next
                    Next
                End If
                LineNum = 0
                Dim SQTOADDNnewLineNo As Integer = 0
                LoopRow = 0
                RemoveIndex = 0

                If ds.Tables("ACONADD").Rows.Count > 0 Then
                    SQTOADD = ds.Tables("ACONADD")
                    oSons = oGeneralData.Child("ACONADDON")

                    SQTOADDNnewLineNo = SQTOADD.Rows.Count()
                    If SQTOADD.Rows.Count > Count_Addon_Int Then
                        LoopRow = SQTOADD.Rows.Count
                    Else
                        LoopRow = Count_Addon_Int
                    End If
                    Dim i As Integer = 0
                    RemoveIndex = (Count_Addon_Int - 1)
                    For i = 0 To LoopRow - 1
                        '                        For Each dr1 As DataRow In SQTOADD.Rows
                        LineNum = LineNum + 1
                        If SQTOADDNnewLineNo = Count_Addon_Int Then
                            'oSon = oSons.Add
                            'oSon.Item(LineNum - 1).SetProperty("U_Ctype", SQTOADD.Rows(i).Item("U_Ctype"))
                            oSons.Item(LineNum - 1).SetProperty("U_Ccode", SQTOGEN.Rows(i).Item("U_Ccode").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Cname", SQTOGEN.Rows(i).Item("U_Cname").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Ctype", SQTOADD.Rows(i).Item("U_Ctype").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Continent", SQTOADD.Rows(i).Item("U_Continent").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Country", SQTOADD.Rows(i).Item("U_Country").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_City", SQTOADD.Rows(i).Item("U_City"))
                            oSons.Item(LineNum - 1).SetProperty("U_Currency", SQTOADD.Rows(i).Item("U_Currency").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_EQGroup", SQTOADD.Rows(i).Item("U_EQGroup").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Rate", SQTOADD.Rows(i).Item("U_Rate").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_UOM", SQTOADD.Rows(i).Item("U_UOM").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_GST", SQTOADD.Rows(i).Item("U_GST").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Remarks", SQTOADD.Rows(i).Item("U_Remarks").ToString.Trim())

                        ElseIf SQTOADDNnewLineNo > Count_Addon_Int Then
                            If LineNum <= Count_Addon_Int Then
                                oSons.Item(LineNum - 1).SetProperty("U_Ccode", SQTOGEN.Rows(i).Item("U_Ccode").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Cname", SQTOGEN.Rows(i).Item("U_Cname").ToString.Trim())

                                oSons.Item(LineNum - 1).SetProperty("U_Ctype", SQTOADD.Rows(i).Item("U_Ctype").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Continent", SQTOADD.Rows(i).Item("U_Continent").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Country", SQTOADD.Rows(i).Item("U_Country").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_City", SQTOADD.Rows(i).Item("U_City"))
                                oSons.Item(LineNum - 1).SetProperty("U_Currency", SQTOADD.Rows(i).Item("U_Currency").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_EQGroup", SQTOADD.Rows(i).Item("U_EQGroup").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Rate", SQTOADD.Rows(i).Item("U_Rate").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_UOM", SQTOADD.Rows(i).Item("U_UOM").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_GST", SQTOADD.Rows(i).Item("U_GST").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Remarks", SQTOADD.Rows(i).Item("U_Remarks").ToString.Trim())
                            Else
                                oSon = oSons.Add

                                oSon.SetProperty("U_Ccode", SQTOGEN.Rows(i).Item("U_Ccode").ToString.Trim())
                                oSon.SetProperty("U_Cname", SQTOGEN.Rows(i).Item("U_Cname").ToString.Trim())

                                oSon.SetProperty("U_Ctype", SQTOADD.Rows(i).Item("U_Ctype").ToString.Trim())
                                oSon.SetProperty("U_Continent", SQTOADD.Rows(i).Item("U_Continent").ToString.Trim())
                                oSon.SetProperty("U_Country", SQTOADD.Rows(i).Item("U_Country").ToString.Trim())
                                oSon.SetProperty("U_City", SQTOADD.Rows(i).Item("U_City").ToString.Trim())
                                oSon.SetProperty("U_Currency", SQTOADD.Rows(i).Item("U_Currency").ToString.Trim())
                                oSon.SetProperty("U_EQGroup", SQTOADD.Rows(i).Item("U_EQGroup").ToString.Trim())
                                oSon.SetProperty("U_Rate", SQTOADD.Rows(i).Item("U_Rate").ToString.Trim())
                                oSon.SetProperty("U_UOM", SQTOADD.Rows(i).Item("U_UOM").ToString.Trim())
                                oSon.SetProperty("U_GST", SQTOADD.Rows(i).Item("U_GST").ToString.Trim())
                                oSon.SetProperty("U_Remarks", SQTOADD.Rows(i).Item("U_Remarks").ToString.Trim())
                            End If
                        ElseIf SQTOADDNnewLineNo < Count_Addon_Int Then
                            If LineNum <= SQTOADDNnewLineNo Then
                                oSons.Item(LineNum - 1).SetProperty("U_Ccode", SQTOGEN.Rows(i).Item("U_Ccode").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Cname", SQTOGEN.Rows(i).Item("U_Cname").ToString.Trim())

                                oSons.Item(LineNum - 1).SetProperty("U_Ctype", SQTOADD.Rows(i).Item("U_Ctype").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Continent", SQTOADD.Rows(i).Item("U_Continent").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Country", SQTOADD.Rows(i).Item("U_Country").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_City", SQTOADD.Rows(i).Item("U_City"))
                                oSons.Item(LineNum - 1).SetProperty("U_Currency", SQTOADD.Rows(i).Item("U_Currency").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_EQGroup", SQTOADD.Rows(i).Item("U_EQGroup").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Rate", SQTOADD.Rows(i).Item("U_Rate").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_UOM", SQTOADD.Rows(i).Item("U_UOM").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_GST", SQTOADD.Rows(i).Item("U_GST").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Remarks", SQTOADD.Rows(i).Item("U_Remarks").ToString.Trim())
                            Else
                                oSons.Remove(RemoveIndex - 1)
                                RemoveIndex = RemoveIndex - 1
                            End If
                        End If
                        'Next
                    Next
                End If

                LineNum = 0
                Dim SQTOATTACHNnewLineNo As Integer = 0
                LoopRow = 0
                RemoveIndex = 0

                If ds.Tables("ATTACHMENT").Rows.Count > 0 Then
                    SQTOATTACH = ds.Tables("ATTACHMENT")
                    oSons = oGeneralData.Child("ACONATTACHMENT")

                    SQTOATTACHNnewLineNo = SQTOATTACH.Rows.Count()
                    If SQTOATTACH.Rows.Count > Count_Attach_Int Then
                        LoopRow = SQTOATTACH.Rows.Count
                    Else
                        LoopRow = Count_Attach_Int
                    End If
                    Dim i As Integer = 0
                    RemoveIndex = (Count_Attach_Int - 1)
                    For i = 0 To LoopRow - 1
                        '                        For Each dr1 As DataRow In SQTOADD.Rows
                        LineNum = LineNum + 1
                        If SQTOATTACHNnewLineNo = Count_Attach_Int Then
                            oSons.Item(LineNum - 1).SetProperty("U_Path", SQTOATTACH.Rows(i).Item("U_FilePath").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_Fname", SQTOATTACH.Rows(i).Item("U_FileName").ToString.Trim())

                            Dim dtAttachDate As Date
                            dtAttachDate = GetDateTimeValue(SQTOATTACH.Rows(i).Item("U_Date").ToString.Trim())
                            oSons.Item(LineNum - 1).SetProperty("U_AttDate", dtAttachDate)

                        ElseIf SQTOATTACHNnewLineNo > Count_Attach_Int Then
                            If LineNum <= Count_Attach_Int Then
                                oSons.Item(LineNum - 1).SetProperty("U_Path", SQTOATTACH.Rows(i).Item("U_FilePath").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Fname", SQTOATTACH.Rows(i).Item("U_FileName").ToString.Trim())

                                Dim dtAttachDate As Date
                                dtAttachDate = GetDateTimeValue(SQTOATTACH.Rows(i).Item("U_Date").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_AttDate", dtAttachDate)
                            Else
                                oSon = oSons.Add
                                oSon.SetProperty("U_Path", SQTOATTACH.Rows(i).Item("U_FilePath").ToString.Trim())
                                oSon.SetProperty("U_Fname", SQTOATTACH.Rows(i).Item("U_FileName").ToString.Trim())

                                Dim dtAttachDate As Date
                                dtAttachDate = GetDateTimeValue(SQTOATTACH.Rows(i).Item("U_Date").ToString.Trim())
                                oSon.SetProperty("U_AttDate", dtAttachDate)
                            End If
                        ElseIf SQTOATTACHNnewLineNo < Count_Attach_Int Then
                            If LineNum <= SQTOATTACHNnewLineNo Then
                                oSons.Item(LineNum - 1).SetProperty("U_Path", SQTOATTACH.Rows(i).Item("U_FilePath").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_Fname", SQTOATTACH.Rows(i).Item("U_FileName").ToString.Trim())

                                Dim dtAttachDate As Date
                                dtAttachDate = GetDateTimeValue(SQTOATTACH.Rows(i).Item("U_Date").ToString.Trim())
                                oSons.Item(LineNum - 1).SetProperty("U_AttDate", dtAttachDate)
                            Else
                                oSons.Remove(RemoveIndex - 1)
                                RemoveIndex = RemoveIndex - 1
                            End If
                        End If
                        'Next
                    Next
                End If

                oGeneralService.Update(oGeneralData)
                DocEntry = dr.Item("U_Qno")
            End If

            Dim RetDT As New DataTable
            RetDT.TableName = "VALIDATE"
            RetDT.Columns.Add("Status")
            RetDT.Columns.Add("Msg")
            RetDT.Rows.Add()
            RetDT.Rows(0)(0) = True
            RetDT.Rows(0)(1) = "Agent Contract no. " & DocEntry.ToString & " Updated Successfully"
            RetDS.Tables.Add(RetDT)
            Context.Response.Output.Write(fn.ds2json(RetDS))

            '-------------
            'Dim orec As SAPbobsCOM.Recordset
            'orec = ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            'Dim sSQL As String = "SELECT Idx FROM ( " & vbCrLf
            'sSQL &= "SELECT CONVERT(INT, ROW_NUMBER() OVER (ORDER BY VisOrder))-1 Idx, LineId, VisOrder FROM [@TI_SAMPLE_R] WHERE DocEntry = 58 " & vbCrLf
            'sSQL &= ") X WHERE X.LineId IN (2,5,6) ORDER BY Idx DESC  "
            'orec.DoQuery(sSQL)
            'oChildren = oGeneralData.Child("TI_SAMPLE_R")
            'While (orec.EoF) = False
            '    Dim remve As Integer = orec.Fields.Item(0).Value
            '    oChildren.Remove(remve)
            '    orec.MoveNext()
            'End While
            'oGeneralService.Update(oGeneralData)
            '--------------

        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
  <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub AgentContract_FirstRecord()
        sFunction = "AgentContract_FirstRecord"
        Try

            Dim Query As String
            Query = "SELECT Top 1 T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", T0.""U_Agentcode"", " & _
                    " T0.""U_Agentname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", " & _
                    " T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", " & _
                    " T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@ACON""  T0 ORDER BY T0.""DocEntry"" ASC"
            Dim ErrMsg As String = ""
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable
            Dim RetDT2 As New DataTable
            Dim RetDT3 As New DataTable
            Dim RetDT4 As New DataTable

            Dim RetDS = New DataSet()
            RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
            If ErrMsg <> "" Then
                Throw New Exception(ErrMsg)
            End If

            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "ACON"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)

                RetDT = New DataTable

                Query = "SELECT T1.""U_Ccode"", T1.""U_Cname"",T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", " & _
                        " T1.""U_Rate"", T1.""U_UOM"", T1.""U_GST"", T1.""U_Remarks"" FROM ""@ACONGENERAL""  T1 " & _
                        " WHERE T1.""DocEntry"" = (SELECT Top 1 T0.""DocEntry""  FROM ""@ACON""  T0 ORDER BY T0.""DocEntry"" ASC)"
                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                RetDT.TableName = "ACONGEN"
                RetDT2 = RetDT.Copy
                RetDS.Tables.Add(RetDT2)


                RetDT = New DataTable
                Query = "SELECT T0.""U_Ccode"", T0.""U_Cname"",T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", " & _
                        " T0.""U_Rate"", T0.""U_UOM"", T0.""U_GST"", T0.""U_Remarks"" FROM ""@ACONADDON""  T0 " & _
                        " WHERE T0.""DocEntry""  = (SELECT Top 1 T0.""DocNum""  FROM ""@ACON""  T0 ORDER BY T0.""DocEntry"" ASC)"
                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                RetDT.TableName = "ACONADD"
                RetDT3 = RetDT.Copy
                RetDS.Tables.Add(RetDT3)

                RetDT = New DataTable
                Query = "SELECT ""U_Fname"" AS ""U_FileName"",""U_Path"" AS ""U_FilePath"",""LineId"" AS ""U_id"", TO_CHAR(""U_AttDate"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                        " FROM ""@ACONATTACHMENT"" WHERE ""DocEntry"" = (SELECT Top 1 T0.""DocEntry""  FROM ""@ACON""  T0 ORDER BY T0.""DocEntry"" ASC) "
                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                RetDT.TableName = "ATTACHMENT"
                RetDT4 = RetDT.Copy
                RetDS.Tables.Add(RetDT4)

                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                RetDT = New DataTable
                RetDT.TableName = "VALIDATE"
                RetDT.Columns.Add("Status")
                RetDT.Columns.Add("Msg")
                RetDT.Rows.Add()
                RetDT.Rows(0)(0) = True
                RetDT.Rows(0)(1) = "No Record Found"
                RetDS.Tables.Add(RetDT)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            End If
        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
  <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub AgentContract_LastRecord()
        sFunction = "AgentContract_LastRecord"
        Try 'T1.""U_Ccode"", T1.""U_Cname"",
            Dim Query As String
            Query = "SELECT Top 1 T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"",  T0.""U_Agentcode"", " & _
                    " T0.""U_Agentname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", " & _
                    " T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", " & _
                    " T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@ACON""  T0 ORDER BY T0.""DocEntry"" DESC"
            Dim ErrMsg As String = ""
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable
            Dim RetDT2 As New DataTable
            Dim RetDT3 As New DataTable
            Dim RetDT4 As New DataTable

            Dim RetDS = New DataSet()
            RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
            If ErrMsg <> "" Then
                Throw New Exception(ErrMsg)
            End If

            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "ACON"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)

                RetDT = New DataTable

                Query = "SELECT T1.""U_Ccode"", T1.""U_Cname"",T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", " & _
                        " T1.""U_Rate"", T1.""U_UOM"", T1.""U_GST"", T1.""U_Remarks"" FROM ""@ACONGENERAL""  T1 " & _
                        " WHERE T1.""DocEntry"" = (SELECT Top 1 T0.""DocEntry""  FROM ""@ACON""  T0 ORDER BY T0.""DocEntry"" DESC)"
                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                RetDT.TableName = "ACONGEN"
                RetDT2 = RetDT.Copy
                RetDS.Tables.Add(RetDT2)


                RetDT = New DataTable
                Query = "SELECT T0.""U_Ccode"", T0.""U_Cname"",T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", " & _
                        " T0.""U_Rate"", T0.""U_UOM"", T0.""U_GST"", T0.""U_Remarks"" FROM ""@ACONADDON""  T0 " & _
                        " WHERE T0.""DocEntry""  = (SELECT Top 1 T0.""DocNum""  FROM ""@ACON""  T0 ORDER BY T0.""DocEntry"" DESC)"
                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                RetDT.TableName = "ACONADD"
                RetDT3 = RetDT.Copy
                RetDS.Tables.Add(RetDT3)

                RetDT = New DataTable
                Query = "SELECT ""U_Fname"" AS ""U_FileName"",""U_Path"" AS ""U_FilePath"",""LineId"" AS ""U_id"", TO_CHAR(""U_AttDate"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                        " FROM ""@ACONATTACHMENT"" WHERE ""DocEntry"" = (SELECT Top 1 T0.""DocEntry""  FROM ""@ACON""  T0 ORDER BY T0.""DocEntry"" DESC) "
                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                RetDT.TableName = "ATTACHMENT"
                RetDT4 = RetDT.Copy
                RetDS.Tables.Add(RetDT4)

                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                RetDT = New DataTable
                RetDT.TableName = "VALIDATE"
                RetDT.Columns.Add("Status")
                RetDT.Columns.Add("Msg")
                RetDT.Rows.Add()
                RetDT.Rows(0)(0) = True
                RetDT.Rows(0)(1) = "No Record Found"
                RetDS.Tables.Add(RetDT)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            End If
        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
 <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub AgentContract_PreviousRecord(ByVal value As String)
        sFunction = "AgentContract_PreviousRecord"
        Try
            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim ds As DataSet = fn.jsontodata(value)
            Dim LEADM As New DataTable
            Dim DocNum As String = ""
            If ds.Tables("ACON").Rows.Count > 0 Then
                LEADM = ds.Tables("ACON")
                Dim dr As DataRow = LEADM.Rows(0)
                DocNum = dr.Item("U_Qno").ToString
                Dim Query As String = ""

                Dim ErrMsg As String = ""
                Dim RetDT As New DataTable
                Dim RetDT1 As New DataTable
                Dim RetDT2 As New DataTable
                Dim RetDT3 As New DataTable
                Dim RetDT4 As New DataTable
                Dim RetDS = New DataSet()
                Dim DocEntry As String = ""

                If DocNum = "" Then
                    Query = "SELECT Top 1 T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"",   T0.""U_Agentcode"", T0.""U_Agentname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@ACON""  T0 ORDER BY T0.""DocEntry"" ASC"

                Else
                    Query = "SELECT Top 1 T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"",   T0.""U_Agentcode"", T0.""U_Agentname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@ACON"" T0  where T0.""DocEntry"" =(SELECT (T0.""DocEntry"" - 1)  ""DocEntry"" FROM ""@ACON""  T0 WHERE T0.""DocNum"" ='" & DocNum & "')"
                End If

                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                If RetDT.Rows.Count > 0 Then

                    DocEntry = RetDT.Rows(0)(0)
                    RetDT.TableName = "ACON"
                    RetDT1 = RetDT.Copy
                    RetDS.Tables.Add(RetDT1)

                    RetDT = New DataTable

                    Query = "SELECT T1.""U_Ccode"", T1.""U_Cname"",T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", " & _
                            " T1.""U_Rate"", T1.""U_UOM"", T1.""U_GST"", T1.""U_Remarks"" FROM ""@ACONGENERAL""  T1 WHERE T1.""DocEntry"" = '" & DocEntry & "'"
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "ACONGEN"
                    RetDT2 = RetDT.Copy
                    RetDS.Tables.Add(RetDT2)


                    RetDT = New DataTable
                    Query = "SELECT T0.""U_Ccode"", T0.""U_Cname"",T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", " & _
                            " T0.""U_Rate"", T0.""U_UOM"", T0.""U_GST"", T0.""U_Remarks"" FROM ""@ACONADDON""  T0 WHERE T0.""DocEntry""  = '" & DocEntry & "'"
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "ACONADD"
                    RetDT3 = RetDT.Copy
                    RetDS.Tables.Add(RetDT3)

                    RetDT = New DataTable
                    Query = "SELECT ""U_Fname"" AS ""U_FileName"",""U_Path"" AS ""U_FilePath"",""LineId"" AS ""U_id"", TO_CHAR(""U_AttDate"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                            " FROM ""@ACONATTACHMENT"" WHERE ""DocEntry"" = '" & DocEntry & "' "
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "ATTACHMENT"
                    RetDT4 = RetDT.Copy
                    RetDS.Tables.Add(RetDT4)

                    Context.Response.Output.Write(fn.ds2json(RetDS))
                Else

                    Query = "SELECT Top 1 T0.""DocNum"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"",  T0.""U_Agentcode"", T0.""U_Agentname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@ACON""  T0 ORDER BY T0.""DocEntry"" DESC"
                    RetDT = New DataTable
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    If RetDT.Rows.Count > 0 Then
                        DocEntry = RetDT.Rows(0)(0)
                        RetDT.TableName = "ACON"
                        RetDT1 = RetDT.Copy
                        RetDS.Tables.Add(RetDT1)

                        RetDT = New DataTable

                        Query = "SELECT T1.""U_Ccode"", T1.""U_Cname"",T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", " & _
                                " T1.""U_Rate"", T1.""U_UOM"", T1.""U_GST"", T1.""U_Remarks"" FROM ""@ACONGENERAL""  T1 WHERE T1.""DocEntry"" = '" & DocEntry & "'"
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "ACONGEN"
                        RetDT2 = RetDT.Copy
                        RetDS.Tables.Add(RetDT2)


                        RetDT = New DataTable
                        Query = "SELECT T0.""U_Ccode"", T0.""U_Cname"",T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", " & _
                                " T0.""U_Rate"", T0.""U_UOM"", T0.""U_GST"", T0.""U_Remarks"" FROM ""@ACONADDON""  T0 WHERE T0.""DocEntry""  = '" & DocEntry & "'"
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "ACONADD"
                        RetDT3 = RetDT.Copy
                        RetDS.Tables.Add(RetDT3)

                        RetDT = New DataTable
                        Query = "SELECT ""U_Fname"" AS ""U_FileName"",""U_Path"" AS ""U_FilePath"",""LineId"" AS ""U_id"", TO_CHAR(""U_AttDate"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                                " FROM ""@ACONATTACHMENT"" WHERE ""DocEntry"" = '" & DocEntry & "' "
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "ATTACHMENT"
                        RetDT4 = RetDT.Copy
                        RetDS.Tables.Add(RetDT4)

                        Context.Response.Output.Write(fn.ds2json(RetDS))

                    Else
                        RetDT = New DataTable
                        RetDT.TableName = "VALIDATE"
                        RetDT.Columns.Add("Status")
                        RetDT.Columns.Add("Msg")
                        RetDT.Rows.Add()
                        RetDT.Rows(0)(0) = True
                        RetDT.Rows(0)(1) = "No Record Found"
                        RetDS.Tables.Add(RetDT)
                        Context.Response.Output.Write(fn.ds2json(RetDS))
                    End If
                End If
            Else
                Throw New Exception("No Record Found")
            End If
        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub AgentContract_NextRecord(ByVal value As String)
        sFunction = "AgentContract_NextRecord"
        Try
            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim ds As DataSet = fn.jsontodata(value)
            Dim LEADM As New DataTable
            Dim DocNum As String = ""
            If ds.Tables("ACON").Rows.Count > 0 Then
                LEADM = ds.Tables("ACON")
                Dim dr As DataRow = LEADM.Rows(0)
                DocNum = dr.Item("U_Qno").ToString
                Dim Query As String = ""

                Dim ErrMsg As String = ""
                Dim RetDT As New DataTable
                Dim RetDT1 As New DataTable
                Dim RetDT2 As New DataTable
                Dim RetDT3 As New DataTable
                Dim RetDT4 As New DataTable
                Dim RetDS = New DataSet()
                Dim DocEntry As String = ""

                If DocNum = "" Then
                    Query = "SELECT Top 1 T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"",  T0.""U_Ccode"", T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@ACON""  T0 ORDER BY T0.""DocEntry"" DESC"

                Else
                    Query = "SELECT Top 1 T0.""DocNum"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"",T0.""U_Agentcode"", T0.""U_Agentname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@ACON"" T0  where T0.""DocEntry"" =(SELECT (T0.""DocEntry"" + 1)  ""DocEntry"" FROM ""@ACON""  T0 WHERE T0.""DocNum"" ='" & DocNum & "')"
                End If

                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                If RetDT.Rows.Count > 0 Then

                    DocEntry = RetDT.Rows(0)(0)
                    RetDT.TableName = "ACON"
                    RetDT1 = RetDT.Copy
                    RetDS.Tables.Add(RetDT1)

                    RetDT = New DataTable

                    Query = "SELECT T1.""U_Ccode"", T1.""U_Cname"",T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", " & _
                            " T1.""U_Rate"", T1.""U_UOM"", T1.""U_GST"", T1.""U_Remarks"" FROM ""@ACONGENERAL""  T1 WHERE T1.""DocEntry"" = '" & DocEntry & "'"
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "ACONGEN"
                    RetDT2 = RetDT.Copy
                    RetDS.Tables.Add(RetDT2)


                    RetDT = New DataTable
                    Query = "SELECT T0.""U_Ccode"", T0.""U_Cname"",T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", " & _
                            " T0.""U_Rate"", T0.""U_UOM"", T0.""U_GST"", T0.""U_Remarks"" FROM ""@ACONADDON""  T0 WHERE T0.""DocEntry""  = '" & DocEntry & "'"
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "ACONADD"
                    RetDT3 = RetDT.Copy
                    RetDS.Tables.Add(RetDT3)

                    RetDT = New DataTable
                    Query = "SELECT ""U_Fname"" AS ""U_FileName"",""U_Path"" AS ""U_FilePath"",""LineId"" AS ""U_id"", TO_CHAR(""U_AttDate"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                            " FROM ""@ACONATTACHMENT"" WHERE ""DocEntry"" = '" & DocEntry & "' "
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "ATTACHMENT"
                    RetDT4 = RetDT.Copy
                    RetDS.Tables.Add(RetDT4)

                    Context.Response.Output.Write(fn.ds2json(RetDS))
                Else

                    Query = "SELECT Top 1 T0.""DocNum"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"",'DD/MM/YYYY') As ""U_Cdate"", T0.""U_Agentcode"", " & _
                            " T0.""U_Agentname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", " & _
                            " T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", " & _
                            " T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@ACON""  T0 ORDER BY T0.""DocEntry"" ASC"
                    RetDT = New DataTable
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    If RetDT.Rows.Count > 0 Then
                        DocEntry = RetDT.Rows(0)(0)
                        RetDT.TableName = "ACON"
                        RetDT1 = RetDT.Copy
                        RetDS.Tables.Add(RetDT1)

                        RetDT = New DataTable

                        Query = "SELECT T1.""U_Ccode"", T1.""U_Cname"",T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", " & _
                                " T1.""U_Rate"", T1.""U_UOM"", T1.""U_GST"", T1.""U_Remarks"" FROM ""@ACONGENERAL""  T1 WHERE T1.""DocEntry"" = '" & DocEntry & "'"
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "ACONGEN"
                        RetDT2 = RetDT.Copy
                        RetDS.Tables.Add(RetDT2)


                        RetDT = New DataTable
                        Query = "SELECT T0.""U_Ccode"", T0.""U_Cname"",T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", " & _
                                " T0.""U_Rate"", T0.""U_UOM"", T0.""U_GST"", T0.""U_Remarks"" FROM ""@ACONADDON""  T0 WHERE T0.""DocEntry""  = '" & DocEntry & "'"
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "ACONADD"
                        RetDT3 = RetDT.Copy
                        RetDS.Tables.Add(RetDT3)

                        RetDT = New DataTable
                        Query = "SELECT ""U_Fname"" AS ""U_FileName"",""U_Path"" AS ""U_FilePath"",""LineId"" AS ""U_id"", TO_CHAR(""U_AttDate"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                                " FROM ""@ACONATTACHMENT"" WHERE ""DocEntry"" = '" & DocEntry & "' "
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "ATTACHMENT"
                        RetDT4 = RetDT.Copy
                        RetDS.Tables.Add(RetDT4)

                        Context.Response.Output.Write(fn.ds2json(RetDS))

                    Else
                        RetDT = New DataTable
                        RetDT.TableName = "VALIDATE"
                        RetDT.Columns.Add("Status")
                        RetDT.Columns.Add("Msg")
                        RetDT.Rows.Add()
                        RetDT.Rows(0)(0) = True
                        RetDT.Rows(0)(1) = "No Record Found"
                        RetDS.Tables.Add(RetDT)
                        Context.Response.Output.Write(fn.ds2json(RetDS))
                    End If
                End If
            Else
                Throw New Exception("No Record Found")
            End If
        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
 <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub AgentContract_FindRecord_List(ByVal value As String)
        sFunction = "AgentContract_FindRecord_List"
        Try
            oLog.WriteToLogFile_Debug(value, sFunction)
            Dim ds As DataSet = fn.jsontodata(value)
            Dim LEADM As New DataTable
            Dim DocNum As String = ""
            If ds.Tables("ACON").Rows.Count > 0 Then
                LEADM = ds.Tables("ACON")
                Dim dr As DataRow = LEADM.Rows(0)
                Try
                    DocNum = dr.Item("U_Qno").ToString.Trim()
                Catch ex As Exception

                End Try

                Dim CreateDate As String = ""
                Dim CardCode As String = ""
                Dim CardName As String = ""
                Dim ProjectCode As String = ""

                CreateDate = dr.Item("U_Cdate").ToString.Trim()
                CardCode = dr.Item("U_Agentcode").ToString.Trim()
                CardName = dr.Item("U_Agentname").ToString.Trim()
                ProjectCode = dr.Item("U_Pcode").ToString.Trim()

                If CreateDate = "" Then
                    CreateDate = "%"
                ElseIf CreateDate.Contains("*") = True Then
                    CreateDate = CreateDate.Replace("*", "%")
                Else
                    CreateDate = "%" & CreateDate & "%"
                End If

                If CardCode = "" Then
                    CardCode = "%"
                ElseIf CardCode.Contains("*") = True Then
                    CardCode = CardCode.Replace("*", "%")
                Else
                    CardCode = "%" & CardCode & "%"
                End If

                If CardName = "" Then
                    CardName = "%"
                ElseIf CardName.Contains("*") = True Then
                    CardName = CardName.Replace("*", "%")
                Else
                    CardName = "%" & CardName & "%"
                End If

                If ProjectCode = "" Then
                    ProjectCode = "%"
                ElseIf ProjectCode.Contains("*") = True Then
                    ProjectCode = ProjectCode.Replace("*", "%")
                Else
                    ProjectCode = "%" & ProjectCode & "%"
                End If


                Dim Query As String = ""
                Dim ErrMsg As String = ""
                Dim RetDT As New DataTable
                Dim RetDT1 As New DataTable
                Dim RetDT2 As New DataTable
                Dim RetDT3 As New DataTable
                Dim RetDT4 As New DataTable
                Dim RetDS = New DataSet()
                Dim DocEntry As String = ""

                If DocNum = "" Then
                    Query = "SELECT T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", T0.""U_Agentcode"", T0.""U_Agentname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@ACON"" T0 where T0.""U_Agentcode"" like '" & CardCode & "' and  T0.""U_Cdate"" like '" & CreateDate & "'  and  T0.""U_Agentname"" like '" & CardName & "'   and  T0.""U_Pcode"" like '" & ProjectCode & "'    ORDER BY T0.""DocEntry"" DESC"

                Else
                    Query = "SELECT Top 1 T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"",T0.""U_Agentcode"", T0.""U_Agentname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@ACON"" T0  where T0.""DocEntry"" ='" & DocNum & "'"
                End If

                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                If RetDT.Rows.Count > 0 Then
                    DocEntry = RetDT.Rows(0)(0)
                    RetDT.TableName = "ACON"
                    RetDT1 = RetDT.Copy
                    RetDS.Tables.Add(RetDT1)

                    'RetDT = New DataTable
                    'Query = "SELECT T1.""DocEntry"" ,T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", T1.""U_Rate"", T1.""U_UOM"", T1.""U_GST"", T1.""U_Remarks"" FROM ""@ACONGENERAL""  T1 WHERE T1.""DocEntry"" = '" & DocEntry & "'"
                    'RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    'If ErrMsg <> "" Then
                    '    Throw New Exception(ErrMsg)
                    'End If
                    'RetDT.TableName = "ACONGEN"
                    'RetDT2 = RetDT.Copy
                    'RetDS.Tables.Add(RetDT2)


                    'RetDT = New DataTable
                    'Query = "SELECT T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", T0.""U_Rate"", T0.""U_UOM"", T0.""U_GST"", T0.""U_Remarks"" FROM ""@ACONADDON""  T0 WHERE T0.""DocEntry""  = '" & DocEntry & "'"
                    'RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    'If ErrMsg <> "" Then
                    '    Throw New Exception(ErrMsg)
                    'End If
                    'RetDT.TableName = "ACONADD"
                    'RetDT3 = RetDT.Copy
                    'RetDS.Tables.Add(RetDT3)

                    Context.Response.Output.Write(fn.ds2json(RetDS))

                Else
                    Throw New Exception("No Record Found")
                End If
            Else
                Throw New Exception("No Record Found")
            End If
        Catch ex As Exception
            oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
  <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub AgentContract_FindRecord(ByVal value As String)
        sFunction = "AgentContract_FindRecord"
        Try
            oLog.WriteToLogFile_Debug(value, sFunction)
            Dim ds As DataSet = fn.jsontodata(value)
            Dim LEADM As New DataTable
            Dim DocNum As String = ""
            If ds.Tables("ACON").Rows.Count > 0 Then
                LEADM = ds.Tables("ACON")
                Dim dr As DataRow = LEADM.Rows(0)
                DocNum = dr.Item("U_Qno").ToString
                'Dim CreateDate As String = ""
                'Dim CardCode As String = ""
                'Dim CardName As String = ""
                'Dim ProjectCode As String = ""

                'CreateDate = dr.Item("U_Cdate").ToString
                'CardCode = dr.Item("U_Ccode").ToString
                'CardName = dr.Item("U_Cname").ToString
                'ProjectCode = dr.Item("U_Pcode").ToString


                'If CreateDate = "" Then
                '    CreateDate = "%"
                'ElseIf CreateDate.Contains("*") = True Then
                '    CreateDate = CreateDate.Replace("*", "%")
                'Else
                '    CreateDate = "%" & CreateDate & "%"
                'End If

                'If CardCode = "" Then
                '    CardCode = "%"
                'ElseIf CardCode.Contains("*") = True Then
                '    CardCode = CardCode.Replace("*", "%")
                'Else
                '    CardCode = "%" & CardCode & "%"
                'End If

                'If CardName = "" Then
                '    CardName = "%"
                'ElseIf CardName.Contains("*") = True Then
                '    CardName = CardName.Replace("*", "%")
                'Else
                '    CardName = "%" & CardName & "%"
                'End If

                'If ProjectCode = "" Then
                '    ProjectCode = "%"
                'ElseIf ProjectCode.Contains("*") = True Then
                '    ProjectCode = ProjectCode.Replace("*", "%")
                'Else
                '    ProjectCode = "%" & ProjectCode & "%"
                'End If


                Dim Query As String = ""
                Dim ErrMsg As String = ""
                Dim RetDT As New DataTable
                Dim RetDT1 As New DataTable
                Dim RetDT2 As New DataTable
                Dim RetDT3 As New DataTable
                Dim RetDT4 As New DataTable
                Dim RetDS = New DataSet()
                Dim DocEntry As String = ""

                If DocNum = "" Then

                    Throw New Exception("No Record Found")
                    ' Query = "SELECT Top 1 T0.""DocNum"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", TO_CHAR( T0.""U_Qdate1"" , 'DD/MM/YYYY') As ""U_Qdate1"", TO_CHAR( T0.""U_Qdate2"" , 'DD/MM/YYYY') As ""U_Qdate2"", T0.""U_Ccode"", T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@ACON""  where T0.""U_Ccode"" like '" & CardCode & "' and  T0.""U_Cdate"" like '" & DocNum & "'  and  T0.""U_Cname"" like '" & CardName & "'   and  T0.""U_Pcode"" like '" & ProjectCode & "'   T0 ORDER BY T0.""DocEntry"" DESC"

                Else
                    Query = "SELECT Top 1 T0.""DocNum"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"",   T0.""U_Agentcode"", T0.""U_Agentname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@ACON"" T0  where T0.""DocEntry"" ='" & DocNum & "'"
                End If

                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                If RetDT.Rows.Count > 0 Then

                    DocEntry = RetDT.Rows(0)(0)
                    RetDT.TableName = "ACON"
                    RetDT1 = RetDT.Copy
                    RetDS.Tables.Add(RetDT1)

                    RetDT = New DataTable

                    Query = "SELECT T1.""U_Ccode"", T1.""U_Cname"",T1.""DocEntry"" ,T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", " & _
                            " T1.""U_EQGroup"", T1.""U_Rate"", T1.""U_UOM"", T1.""U_GST"", T1.""U_Remarks"" FROM ""@ACONGENERAL""  T1 WHERE T1.""DocEntry"" = '" & DocEntry & "'"
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "ACONGEN"
                    RetDT2 = RetDT.Copy
                    RetDS.Tables.Add(RetDT2)

                    RetDT = New DataTable
                    Query = "SELECT T0.""U_Ccode"", T0.""U_Cname"",T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", " & _
                            " T0.""U_Rate"", T0.""U_UOM"", T0.""U_GST"", T0.""U_Remarks"" FROM ""@ACONADDON""  T0 WHERE T0.""DocEntry""  = '" & DocEntry & "'"
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "ACONADD"
                    RetDT3 = RetDT.Copy
                    RetDS.Tables.Add(RetDT3)

                    RetDT = New DataTable
                    Query = "SELECT ""U_Fname"" AS ""U_FileName"",""U_Path"" AS ""U_FilePath"",""LineId"" AS ""U_id"", TO_CHAR(""U_AttDate"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                            " FROM ""@ACONATTACHMENT"" WHERE ""DocEntry"" = '" & DocEntry & "' "
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "ATTACHMENT"
                    RetDT4 = RetDT.Copy
                    RetDS.Tables.Add(RetDT4)

                    Context.Response.Output.Write(fn.ds2json(RetDS))
                Else
                    Throw New Exception("No Record Found")
                    Query = "SELECT Top 1 T0.""DocNum"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"",T0.""U_Agentcode"", T0.""U_Agentname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@ACON""  T0 ORDER BY T0.""DocEntry"" DESC"
                    RetDT = New DataTable
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    If RetDT.Rows.Count > 0 Then
                        DocEntry = RetDT.Rows(0)(0)
                        RetDT.TableName = "ACON"
                        RetDT1 = RetDT.Copy
                        RetDS.Tables.Add(RetDT1)

                        RetDT = New DataTable

                        Query = "SELECT T1.""U_Ccode"", T1.""U_Cname"",T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", T1.""U_Rate"", T1.""U_UOM"", T1.""U_GST"", T1.""U_Remarks"" FROM ""@ACONGENERAL""  T1 WHERE T1.""DocEntry"" = '" & DocEntry & "'"
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "ACONGEN"
                        RetDT2 = RetDT.Copy
                        RetDS.Tables.Add(RetDT2)


                        RetDT = New DataTable
                        Query = "SELECT T0.""U_Ccode"", T0.""U_Cname"",T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", T0.""U_Rate"", T0.""U_UOM"", T0.""U_GST"", T0.""U_Remarks"" FROM ""@ACONADDON""  T0 WHERE T0.""DocEntry""  = '" & DocEntry & "'"
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "ACONADD"
                        RetDT3 = RetDT.Copy
                        RetDS.Tables.Add(RetDT3)

                        RetDT = New DataTable
                        Query = "SELECT ""U_Fname"" AS ""U_FileName"",""U_Path"" AS ""U_FilePath"",""LineId"" AS ""U_id"", TO_CHAR(""U_AttDate"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                                " FROM ""@ACONATTACHMENT"" WHERE ""DocEntry"" = '" & DocEntry & "' "
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "ATTACHMENT"
                        RetDT4 = RetDT.Copy
                        RetDS.Tables.Add(RetDT4)

                        Context.Response.Output.Write(fn.ds2json(RetDS))

                    Else
                        RetDT = New DataTable
                        RetDT.TableName = "VALIDATE"
                        RetDT.Columns.Add("Status")
                        RetDT.Columns.Add("Msg")
                        RetDT.Rows.Add()
                        RetDT.Rows(0)(0) = True
                        RetDT.Rows(0)(1) = "No Record Found"
                        RetDS.Tables.Add(RetDT)
                        Context.Response.Output.Write(fn.ds2json(RetDS))
                    End If
                End If
            Else
                Throw New Exception("No Record Found")
            End If
        Catch ex As Exception
            oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
#End Region
#Region "Sales Order"
    <WebMethod()> _
          <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub SalesOrderAdd(ByVal value As String)
        sFunction = "SalesOrderAdd"
        Try

            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(PublicVariable.sTempfilePath, sFunction)
            Dim SQTO As New DataTable
            SQTO = New DataTable
            Dim SQTOGEN As New DataTable
            Dim SQTOADD As New DataTable
            Dim SOTOATTACH As New DataTable
            Dim SQTODS = New DataSet()
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            fn.GetSAPConnection(Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            Dim STypeCode As String = ""
            Dim STypeName As String = ""
            Dim RetCode As Integer = 0
            Dim SONo As String = ""
            Dim errMessage As String = ""
            Dim CardCode As String = ""
            Dim DocDate As String = ""
            Dim ProjectCode As String = ""
            Dim Country As String = ""
            Dim City As String = ""
            Dim EqupTye As String = ""
            Dim Rt As String = ""
            Dim SQLStr As String = ""

            Dim oSO As SAPbobsCOM.Documents
            oSO = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)



            If ds.Tables("ORDR").Rows.Count > 0 Then
                SQTO = ds.Tables("ORDR")
                Dim dr As DataRow = SQTO.Rows(0)
                oSO.UserFields.Fields.Item("U_UCode").Value = dr.Item("U_UCode").ToString.Trim() 'dr.Item("U_UName").ToString.Trim()
                oSO.UserFields.Fields.Item("U_UName").Value = dr.Item("U_UName").ToString.Trim()
                oSO.UserFields.Fields.Item("U_Cdate").Value = GetDateTimeValue(dr.Item("U_Cdate").ToString)
                DocDate = DateConvert(dr.Item("DocDate").ToString)
                'oSO.DocDate = DateConvert(dr.Item("DocDate").ToString)
                oSO.DocDate = GetDateTimeValue(dr.Item("DocDate").ToString)
                oSO.DocDueDate = GetDateTimeValue(dr.Item("DocDate").ToString)
                oSO.CardCode = dr.Item("CardCode").ToString
                CardCode = dr.Item("CardCode").ToString
                oSO.CardName = dr.Item("CardName").ToString
                oSO.UserFields.Fields.Item("U_SurveyorID").Value = dr.Item("U_SurveyorID").ToString.Trim()
                oSO.NumAtCard = dr.Item("NumAtCard").ToString.Trim
                STypeCode = dr.Item("U_STypeCode")
                STypeName = dr.Item("U_STypeName")
                oSO.UserFields.Fields.Item("U_STypeCode").Value = dr.Item("U_STypeCode").ToString.Trim()
                oSO.UserFields.Fields.Item("U_U_STypeName").Value = dr.Item("U_STypeName").ToString.Trim()
                oSO.UserFields.Fields.Item("U_Country").Value = dr.Item("U_Country").ToString.Trim()
                Country = dr.Item("U_Country").ToString.Trim()
                oSO.UserFields.Fields.Item("U_City").Value = dr.Item("U_City").ToString.Trim()
                City = dr.Item("U_City").ToString.Trim()
                oSO.UserFields.Fields.Item("U_Loc").Value = dr.Item("U_Loc").ToString.Trim()
                oSO.Comments = dr.Item("Comments").ToString.Trim

                Dim oAttachEntry As Integer = 0
                Dim bAttachAdd As Boolean = False
                If ds.Tables("ATTACHMENT").Rows.Count > 0 Then
                    SOTOATTACH = ds.Tables("ATTACHMENT")
                    Dim oAttach As SAPbobsCOM.Attachments2
                    oAttach = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2)

                    For Each odr As DataRow In SOTOATTACH.Rows

                        Dim sSourcePath, sFileName, sFileExt As String
                        sSourcePath = odr.Item("U_FilePath").ToString.Substring(0, 3)
                        sFileName = odr.Item("U_FileName").ToString.Trim()
                        sFileExt = Path.GetExtension(sFileName)
                        sFileExt = sFileExt.Replace(".", "")
                        If sFileName <> "" Then
                            oAttach.Lines.SourcePath = PublicVariable.sTempfilePath
                            oAttach.Lines.FileName = sFileName.Replace(Path.GetExtension(sFileName), "")
                            oAttach.Lines.FileExtension = sFileExt
                            oAttach.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES
                            oAttach.Lines.Add()
                            bAttachAdd = True
                        End If
                    Next
                    If bAttachAdd = True Then
                        If oAttach.Add() = 0 Then
                            oAttachEntry = PublicVariable.oCompany.GetNewObjectKey()
                            oSO.AttachmentEntry = oAttachEntry
                        Else
                            Throw New Exception("Error while adding attachment /" & PublicVariable.oCompany.GetLastErrorDescription)
                        End If
                    End If
                End If

                If ds.Tables("SQTOGEN").Rows.Count > 0 Then
                    SQTOGEN = ds.Tables("SQTOGEN")
                    For Each dr1 As DataRow In SQTOGEN.Rows
                        oSO.Lines.ItemCode = STypeCode
                        oSO.Lines.Quantity = dr1.Item("Quantity")
                        oSO.Lines.UserFields.Fields.Item("U_PDate").Value = GetDateTimeValue(dr1.Item("U_PDate").ToString.Trim())
                        oSO.Lines.UserFields.Fields.Item("U_EQType").Value = dr1.Item("U_EQType").ToString.Trim()
                        EqupTye = dr1.Item("U_EQType").ToString.Trim()
                        oSO.Lines.UserFields.Fields.Item("U_SCriteria").Value = dr1.Item("U_SCriteria").ToString.Trim()
                        oSO.Lines.UserFields.Fields.Item("U_ReSurvey").Value = dr1.Item("U_ReSurvey").ToString.Trim()
                        Dim sResurvey As String = String.Empty
                        sResurvey = dr1.Item("U_ReSurvey").ToString.Trim()

                        If sResurvey.ToUpper = "NO" Then
                            SQLStr = "SELECT Top 1  T1.""U_Rate"",T1.""U_Currency"" " & _
                                 " FROM ""@CCON""  T0 " & _
                                 " Left Join ""@CCONGENERAL""  T1 ON T1.""DocEntry"" = T0.""DocEntry"" " & _
                                 " Left Join ""@EQTYPE"" T2 ON T2.""U_EQCODE""=T1.""U_EQGroup"" " & _
                                 " WHERE T0.""U_Status"" ='Open' and  T0.""U_Ccode"" ='" & CardCode & "' and  T0.""U_CPeriod1"" <='" & DocDate & "' and  T0.""U_CPeriod2"" >='" & DocDate & "'   " & _
                                 " and T1.""U_Stype""='" & STypeCode & "' and  T1.""U_Country""='" & Country & "' and T1.""U_City""='" & City & "' and T2.""U_EQTYPECODE""='" & EqupTye & "'"
                        Else
                            SQLStr = "SELECT Top 1  T1.""U_RRate"" AS ""U_Rate"",T1.""U_Currency"" " & _
                                 " FROM ""@CCON""  T0 " & _
                                 " Left Join ""@CCONGENERAL""  T1 ON T1.""DocEntry"" = T0.""DocEntry"" " & _
                                 " Left Join ""@EQTYPE"" T2 ON T2.""U_EQCODE""=T1.""U_EQGroup"" " & _
                                 " WHERE T0.""U_Status"" ='Open' and  T0.""U_Ccode"" ='" & CardCode & "' and  T0.""U_CPeriod1"" <='" & DocDate & "' and  T0.""U_CPeriod2"" >='" & DocDate & "'   " & _
                                 " and T1.""U_Stype""='" & STypeCode & "' and  T1.""U_Country""='" & Country & "' and T1.""U_City""='" & City & "' and T2.""U_EQTYPECODE""='" & EqupTye & "'"
                        End If

                        'Rt = fn.ExecuteSQLQuery_SingleValue(SQLStr, Errmsg)
                        'If Rt = "" Then
                        '    Throw New Exception("No Valid Contract Found!")
                        'End If
                        'oSO.Lines.UnitPrice = CDec(Rt)
                        'oSO.Lines.Add()

                        Dim dtPrice As DataTable
                        If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug("Executing Query " & SQLStr, sFunction)
                        dtPrice = fn.ExecuteSQLQuery(SQLStr, Errmsg)
                        Dim sCurrency As String = String.Empty
                        Try
                            Dim oDr As DataRow = dtPrice.Rows(0)
                            Rt = oDr.Item("U_Rate").ToString.Trim()
                            sCurrency = oDr.Item("U_Currency").ToString.Trim()
                        Catch ex As Exception
                            Throw New Exception("Cannot determine price/Contract not found")
                        End Try
                        oSO.Lines.UnitPrice = CDec(Rt)
                        oSO.Lines.Currency = sCurrency
                        oSO.Lines.Add()
                    Next
                Else
                    Throw New Exception("The ""General Tab"" must not be empty; enter a value for ""General Tab"".")
                End If
                RetCode = oSO.Add
                If RetCode <> 0 Then
                    PublicVariable.oCompany.GetLastError(RetCode, errMessage)
                    Throw New Exception(errMessage)
                Else
                    SONo = PublicVariable.oCompany.GetNewObjectKey()
                End If
            Else
                Throw New Exception("No Input Data.")
            End If

            Dim RetDT As New DataTable
            RetDT.TableName = "VALIDATE"
            RetDT.Columns.Add("Status")
            RetDT.Columns.Add("Msg")
            RetDT.Rows.Add()
            RetDT.Rows(0)(0) = True
            RetDT.Rows(0)(1) = "Sales Order no. " & SONo.ToString & " Created Successfully"
            RetDS.Tables.Add(RetDT)
            Context.Response.Output.Write(fn.ds2json(RetDS))


        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
         <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub SalesOrderUpdate(ByVal value As String)
        sFunction = "SalesOrderUpdate"
        Try

            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim SQTO As New DataTable
            SQTO = New DataTable
            Dim SQTOGEN As New DataTable
            Dim SQTOADD As New DataTable
            Dim SOTOATTACH As New DataTable
            Dim SQTODS = New DataSet()
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            fn.GetSAPConnection(Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            Dim STypeCode As String = ""
            Dim STypeName As String = ""
            Dim RetCode As Integer = 0
            Dim SONo As String = ""
            Dim errMessage As String = ""
            Dim CardCode As String = ""
            Dim DocDate As String = ""
            Dim ProjectCode As String = ""
            Dim Country As String = ""
            Dim City As String = ""
            Dim EqupTye As String = ""
            Dim Rt As String = ""
            Dim SQLStr As String = ""

            Dim oSO As SAPbobsCOM.Documents
            oSO = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
            If ds.Tables("ORDR").Rows.Count > 0 Then
                SQTO = ds.Tables("ORDR")
                Dim dr As DataRow = SQTO.Rows(0)

                If oSO.GetByKey(dr.Item("U_OrderNo")) = True Then

                Else
                    Throw New Exception("No Record Found!")
                End If

                oSO.NumAtCard = dr.Item("NumAtCard").ToString.Trim
                oSO.UserFields.Fields.Item("U_UpdatedBy_UserCode").Value = dr.Item("U_UCode").ToString.Trim()
                oSO.UserFields.Fields.Item("U_UpdateBy_UserName").Value = dr.Item("U_UName").ToString.Trim()
                oSO.Comments = dr.Item("Comments").ToString.Trim

                Dim iAttachEntry As Integer = 0
                Dim bAttchAdd As Boolean = False
                Dim bFileExists As Boolean = False
                If ds.Tables("ATTACHMENT").Rows.Count > 0 Then
                    SOTOATTACH = ds.Tables("ATTACHMENT")
                    Dim oAttach As SAPbobsCOM.Attachments2
                    oAttach = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2)

                    If oAttach.GetByKey(oSO.AttachmentEntry) Then
                        For Each odr As DataRow In SOTOATTACH.Rows
                            Dim sSourcePath, sFileName, sFileExt As String
                            sSourcePath = odr.Item("U_FilePath").ToString.Substring(0, 3)
                            sFileName = odr.Item("U_FileName").ToString.Trim()
                            sFileExt = Path.GetExtension(sFileName)
                            sFileExt = sFileExt.Replace(".", "")

                            If sFileName <> "" Then
                                Dim sQuery As String = String.Empty
                                Dim oRecordSet As SAPbobsCOM.Recordset
                                oRecordSet = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                sQuery = "SELECT * FROM ""ATC1"" WHERE ""AbsEntry"" = '" & oSO.AttachmentEntry & "' AND ""FileName"" = '" & sFileName.Replace(Path.GetExtension(sFileName), "") & "' "
                                oRecordSet.DoQuery(sQuery)
                                If oRecordSet.RecordCount > 0 Then
                                Else
                                    oAttach.Lines.Add()
                                    oAttach.Lines.SourcePath = PublicVariable.sTempfilePath
                                    oAttach.Lines.FileName = sFileName.Replace(Path.GetExtension(sFileName), "")
                                    oAttach.Lines.FileExtension = sFileExt
                                    oAttach.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES
                                End If
                            End If
                        Next
                        If oAttach.Update() = 0 Then
                            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug("Attachment updated successfully", sFunction)
                        Else
                            Throw New Exception("Error while updating attachment /" & PublicVariable.oCompany.GetLastErrorDescription)
                        End If
                    Else
                        For Each odr As DataRow In SOTOATTACH.Rows
                            Dim sSourcePath, sFileName, sFileExt As String
                            sSourcePath = odr.Item("U_FilePath").ToString.Substring(0, 3)
                            sFileName = odr.Item("U_FileName").ToString.Trim()
                            sFileExt = Path.GetExtension(sFileName)
                            sFileExt = sFileExt.Replace(".", "")
                            If sFileName <> "" Then
                                oAttach.Lines.SourcePath = PublicVariable.sTempfilePath
                                oAttach.Lines.FileName = sFileName.Replace(Path.GetExtension(sFileName), "")
                                oAttach.Lines.FileExtension = sFileExt
                                oAttach.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES
                                oAttach.Lines.Add()
                                bAttchAdd = True
                            End If
                        Next
                        If bAttchAdd = True Then
                            If oAttach.Add() = 0 Then
                                iAttachEntry = PublicVariable.oCompany.GetNewObjectKey()
                                oSO.AttachmentEntry = iAttachEntry
                            Else
                                Throw New Exception("Error while adding attachment /" & PublicVariable.oCompany.GetLastErrorDescription)
                            End If
                        End If
                    End If
                End If

                If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug("Adding general data", sFunction)

                If ds.Tables("SQTOGEN").Rows.Count > 0 Then
                    SQTOGEN = ds.Tables("SQTOGEN")
                    For Each dr1 As DataRow In SQTOGEN.Rows
                        Dim iLine As Integer = 0
                        'oSO.Lines.ItemCode = STypeCode
                        oSO.Lines.SetCurrentLine(iLine)
                        oSO.Lines.Quantity = dr1.Item("Quantity")
                        oSO.Lines.UserFields.Fields.Item("U_PDate").Value = GetDateTimeValue(dr1.Item("U_PDate").ToString.Trim())
                        oSO.Lines.UserFields.Fields.Item("U_ReSurvey").Value = dr1.Item("U_ReSurvey").ToString.Trim()

                        Dim sResurvey As String = String.Empty
                        If sResurvey.ToUpper = "NO" Then
                            SQLStr = "SELECT Top 1  T1.""U_RRate"" AS ""U_Rate"",T1.""U_Currency"" " & _
                                 " FROM ""@CCON""  T0 " & _
                                 " Left Join ""@CCONGENERAL""  T1 ON T1.""DocEntry"" = T0.""DocEntry"" " & _
                                 " Left Join ""@EQTYPE"" T2 ON T2.""U_EQCODE""=T1.""U_EQGroup"" " & _
                                 " WHERE T0.""U_Status"" ='Open' and  T0.""U_Ccode"" ='" & CardCode & "' and  T0.""U_CPeriod1"" <='" & DocDate & "' and  T0.""U_CPeriod2"" >='" & DocDate & "'   " & _
                                 " and T1.""U_Stype""='" & STypeCode & "' and  T1.""U_Country""='" & Country & "' and T1.""U_City""='" & City & "' and T2.""U_EQTYPECODE""='" & EqupTye & "'"
                        End If
                        Dim dtPrice As DataTable
                        If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug("Executing Query " & SQLStr, sFunction)
                        dtPrice = fn.ExecuteSQLQuery(SQLStr, Errmsg)
                        Dim sCurrency As String = String.Empty
                        Try
                            Dim oDr As DataRow = dtPrice.Rows(0)
                            Rt = oDr.Item("U_Rate").ToString.Trim()
                            sCurrency = oDr.Item("U_Currency").ToString.Trim()
                        Catch ex As Exception
                            Throw New Exception("Cannot determine price/Contract not found")
                        End Try
                        oSO.Lines.UnitPrice = CDec(Rt)
                        oSO.Lines.Currency = sCurrency
                        iLine = iLine + 1
                        oSO.Lines.Add()
                    Next
                Else
                    Throw New Exception("The ""General Tab"" must not be empty; enter a value for ""General Tab"".")
                End If
                RetCode = oSO.Update()
                If RetCode <> 0 Then
                    PublicVariable.oCompany.GetLastError(RetCode, errMessage)
                    Throw New Exception(errMessage)
                Else
                    SONo = dr.Item("U_OrderNo").ToString ' PublicVariable.oCompany.GetNewObjectKey()
                End If
            Else
                Throw New Exception("No Input Data.")
            End If

            Dim RetDT As New DataTable
            RetDT.TableName = "VALIDATE"
            RetDT.Columns.Add("Status")
            RetDT.Columns.Add("Msg")
            RetDT.Rows.Add()
            RetDT.Rows(0)(0) = True
            RetDT.Rows(0)(1) = "Sales Order no. " & SONo.ToString & " Updated Successfully"
            RetDS.Tables.Add(RetDT)
            Context.Response.Output.Write(fn.ds2json(RetDS))


        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
   <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub SalesOrder_FirstRecord(ByVal value As String)
        sFunction = "SalesOrder_FirstRecord"
        Try

            Dim SQTO As New DataTable
            SQTO = New DataTable
            Dim SQTOGEN As New DataTable
            Dim SQTOADD As New DataTable
            Dim SQTODS = New DataSet()
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim UserCode As String = ""
            If ds.Tables("ORDR").Rows.Count > 0 Then
                SQTO = ds.Tables("ORDR")
                Dim dr As DataRow = SQTO.Rows(0)
                UserCode = dr.Item("U_UCode").ToString.Trim().ToUpper
            Else
                Throw New Exception("No Record Found!")
            End If
            Dim SQLStr As String = ""
            Dim sErrDesc As String = ""
            Dim DType As String = ""
            Dim CompCode As String = ""

            SQLStr = "SELECT T0.""U_DAuthor"" FROM ""@WUSER""  T0 WHERE Upper(T0.""Code"") ='" & UserCode & "'"
            DType = fn.ExecuteSQLQuery_SingleValue(SQLStr, sErrDesc)
            If sErrDesc <> "" Then
                Throw New Exception(sErrDesc)
            ElseIf DType = "" Then
                Throw New Exception("No Record Found!")
            End If
            Dim Query As String = String.Empty
            If DType = "By Company" Then
                SQLStr = "SELECT ifnull(T0.""U_ComCode"",'')  As ""U_ComCode"" FROM ""@WUSER""  T0 WHERE T0.""Code"" ='" & UserCode & "'"
                CompCode = fn.ExecuteSQLQuery_SingleValue(SQLStr, sErrDesc)
                If sErrDesc <> "" Then
                    Throw New Exception(sErrDesc)
                ElseIf CompCode = "" Then
                    Throw New Exception("No Record Found!")
                End If
                Query = "SELECT Top 1 T0.""DocEntry"" as ""U_OrderNo"",T0.""DocNum"" AS ""SODocNum"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", " & _
                        " T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY') As ""DocDate"",T0.""U_IMO"", " & _
                        " T0.""CardCode"",  T0.""CardName"", T0.""U_SurveyorID"", T0.""NumAtCard"",T0.""U_STypeCode"",  T0.""U_U_STypeName""  As  U_STypeName, T0.""U_Country"", " & _
                        " T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"",T2.""U_HYPERLINK_BEFORE"" AS ""U_HLinkBef"",T2.""U_HYPERLINK_AFTER"" AS ""U_HLinkAft"" " & _
                        " FROM ""ORDR"" T0 " & _
                        " INNER JOIN ""RDR1"" T1 ON T1.""DocEntry"" = T0.""DocEntry"" " & _
                        " LEFT JOIN ""@SURVEYTYPE_HLINK"" T2 ON UPPER(T2.""U_SURVEYTYPENAME"") = UPPER(T1.""Dscription"") " & _
                        " where T0.""U_UCode"" in (SELECT T0.""Code"" FROM ""@WUSER""  T0 WHERE T0.""U_ComCode""  ='" & CompCode & "') ORDER BY T0.""DocEntry"" ASC"
            ElseIf DType = "By Country" Then
                Query = "SELECT Top 1 T0.""DocEntry"" as ""U_OrderNo"",T0.""DocNum"" AS ""SODocNum"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", " & _
                         " T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY') As ""DocDate"",T0.""U_IMO"", " & _
                         " T0.""CardCode"",  T0.""CardName"", T0.""U_SurveyorID"", T0.""NumAtCard"",T0.""U_STypeCode"",  T0.""U_U_STypeName""  As  U_STypeName, T0.""U_Country"", " & _
                         " T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"",T2.""U_HYPERLINK_BEFORE"" AS ""U_HLinkBef"",T2.""U_HYPERLINK_AFTER"" AS ""U_HLinkAft"" " & _
                         " FROM ORDR T0 " & _
                         " INNER JOIN ""RDR1"" T1 ON T1.""DocEntry"" = T0.""DocEntry"" " & _
                         " LEFT JOIN ""@SURVEYTYPE_HLINK"" T2 ON UPPER(T2.""U_SURVEYTYPENAME"") = UPPER(T1.""Dscription"") " & _
                         " where T0.""CardCode"" in (SELECT DISTINCT ""CardCode"" FROM ""CRD1"" WHERE ""Country"" IN (SELECT ""U_CCode"" FROM ""@COUNTRY"")) ORDER BY T0.""DocEntry"" ASC"
            Else
                Throw New Exception("No Record Found!")
            End If

            Dim ErrMsg As String = ""
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable
            Dim RetDT2 As New DataTable
            Dim RetDT3 As New DataTable
            Dim RetDT4 As New DataTable
            Dim DocEntry As String = ""

            RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
            If ErrMsg <> "" Then
                Throw New Exception(ErrMsg)
            End If

            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "ORDR"
                Dim dr As DataRow = RetDT.Rows(0)
                DocEntry = dr.Item("U_OrderNo").ToString.Trim()

                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)

                RetDT = New DataTable

                Query = "SELECT T1.""Quantity"", TO_CHAR( T1.""U_PDate"" , 'DD/MM/YYYY') ""U_PDate"", T1.""U_EQType"", T1.""U_SCriteria"",IFNULL(T1.""U_ReSurvey"",'NO') AS ""U_ReSurvey"", " & _
                        " T1.""LineNum"", CASE WHEN T1.""LineStatus"" = 'O' THEN 'Open' WHEN T1.""LineStatus"" = 'C' THEN 'Closed' END AS ""LineStatus"", T1.""OpenQty"" " & _
                        " FROM ""RDR1""  T1 WHERE T1.""DocEntry"" ='" & DocEntry & "'"
                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                RetDT.TableName = "SQTOGEN"
                RetDT2 = RetDT.Copy
                RetDS.Tables.Add(RetDT2)

                RetDT = New DataTable
                Query = "SELECT T1.""FileName"" ||'.'|| T1.""FileExt"" AS ""U_FileName"",T1.""trgtPath""||'\'||T1.""FileName"" ||'.'|| T1.""FileExt"" AS ""U_FilePath"", " & _
                        " T1.""Line"" AS ""U_id"", TO_CHAR(T1.""Date"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                        " FROM ""ORDR""  T0 INNER JOIN ""ATC1"" T1 ON T1.""AbsEntry"" =  T0.""AtcEntry"" " & _
                        " WHERE T0.""DocEntry"" = '" & DocEntry & "' "
                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                RetDT.TableName = "ATTACHMENT"
                RetDT3 = RetDT.Copy
                RetDS.Tables.Add(RetDT3)

                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                RetDT = New DataTable
                RetDT.TableName = "VALIDATE"
                RetDT.Columns.Add("Status")
                RetDT.Columns.Add("Msg")
                RetDT.Rows.Add()
                RetDT.Rows(0)(0) = True
                RetDT.Rows(0)(1) = "No Record Found"
                RetDS.Tables.Add(RetDT)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            End If
        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub SalesOrder_LastRecord(ByVal value As String)
        sFunction = "SalesOrder_LastRecord"
        Try

            Dim SQTO As New DataTable
            SQTO = New DataTable
            Dim SQTOGEN As New DataTable
            Dim SQTOADD As New DataTable
            Dim SQTODS = New DataSet()
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim UserCode As String = ""
            If ds.Tables("ORDR").Rows.Count > 0 Then
                SQTO = ds.Tables("ORDR")
                Dim dr As DataRow = SQTO.Rows(0)
                UserCode = dr.Item("U_UCode").ToString.Trim().ToUpper
            Else
                Throw New Exception("No Record Found!")
            End If
            Dim SQLStr As String = ""
            Dim sErrDesc As String = ""
            Dim DType As String = ""
            Dim CompCode As String = ""

            SQLStr = "SELECT T0.""U_DAuthor"" FROM ""@WUSER""  T0 WHERE Upper(T0.""Code"") ='" & UserCode & "'"
            DType = fn.ExecuteSQLQuery_SingleValue(SQLStr, sErrDesc)
            If sErrDesc <> "" Then
                Throw New Exception(sErrDesc)
            ElseIf DType = "" Then
                Throw New Exception("No Record Found!")
            End If
            Dim Query As String = String.Empty
            If DType = "By Company" Then
                SQLStr = "SELECT ifnull(T0.""U_ComCode"",'')  As ""U_ComCode"" FROM ""@WUSER""  T0 WHERE T0.""Code"" ='" & UserCode & "'"
                CompCode = fn.ExecuteSQLQuery_SingleValue(SQLStr, sErrDesc)
                If sErrDesc <> "" Then
                    Throw New Exception(sErrDesc)
                ElseIf CompCode = "" Then
                    Throw New Exception("No Record Found!")
                End If
                Query = "SELECT Top 1 T0.""DocEntry"" as ""U_OrderNo"",T0.""DocNum"" AS ""SODocNum"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", " & _
                        " T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY') As ""DocDate"",T0.""U_IMO"",  " & _
                        " T0.""CardCode"",  T0.""CardName"", T0.""U_SurveyorID"", T0.""NumAtCard"",T0.""U_STypeCode"",  T0.""U_U_STypeName"" As U_STypeName, T0.""U_Country"", " & _
                        " T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"",T2.""U_HYPERLINK_BEFORE"" AS ""U_HLinkBef"",T2.""U_HYPERLINK_AFTER"" AS ""U_HLinkAft""  " & _
                        " FROM ORDR T0 " & _
                        " INNER JOIN RDR1 T1 ON T1.""DocEntry"" = T0.""DocEntry"" " & _
                        " LEFT JOIN ""@SURVEYTYPE_HLINK"" T2 ON UPPER(T2.""U_SURVEYTYPENAME"") = UPPER(T1.""Dscription"") " & _
                        " where T0.""U_UCode"" in (SELECT T0.""Code"" FROM ""@WUSER""  T0 WHERE T0.""U_ComCode""  ='" & CompCode & "') ORDER BY T0.""DocEntry"" DESC"
            ElseIf DType = "By Country" Then
                Query = "SELECT Top 1 T0.""DocEntry"" as ""U_OrderNo"",T0.""DocNum"" AS ""SODocNum"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", " & _
                         " T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY') As ""DocDate"",T0.""U_IMO"",  " & _
                         " T0.""CardCode"",  T0.""CardName"", T0.""U_SurveyorID"", T0.""NumAtCard"",T0.""U_STypeCode"",  T0.""U_U_STypeName"" As U_STypeName, T0.""U_Country"", " & _
                         " T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"",T2.""U_HYPERLINK_BEFORE"" AS ""U_HLinkBef"",T2.""U_HYPERLINK_AFTER"" AS ""U_HLinkAft"" " & _
                         " FROM ORDR T0  " & _
                         " INNER JOIN RDR1 T1 ON T1.""DocEntry"" = T0.""DocEntry"" " & _
                         " LEFT JOIN ""@SURVEYTYPE_HLINK"" T2 ON UPPER(T2.""U_SURVEYTYPENAME"") = UPPER(T1.""Dscription"") " & _
                         " where T0.""CardCode"" in (SELECT DISTINCT ""CardCode"" FROM ""CRD1"" WHERE ""Country"" IN (SELECT ""U_CCode"" FROM ""@COUNTRY"")) ORDER BY T0.""DocEntry"" DESC"
            Else
                Throw New Exception("No Record Found!")
            End If

            Dim ErrMsg As String = ""
            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable
            Dim RetDT2 As New DataTable
            Dim RetDT3 As New DataTable
            Dim RetDT4 As New DataTable
            Dim DocEntry As String = ""

            RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
            If ErrMsg <> "" Then
                Throw New Exception(ErrMsg)
            End If

            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "ORDR"
                Dim dr As DataRow = RetDT.Rows(0)
                DocEntry = dr.Item("U_OrderNo").ToString.Trim()

                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)

                RetDT = New DataTable

                Query = "SELECT T1.""Quantity"", TO_CHAR( T1.""U_PDate"" , 'DD/MM/YYYY') ""U_PDate"", T1.""U_EQType"", T1.""U_SCriteria"",IFNULL(T1.""U_ReSurvey"",'NO') AS ""U_ReSurvey"", " & _
                        " T1.""LineNum"",CASE WHEN T1.""LineStatus"" = 'O' THEN 'Open' WHEN T1.""LineStatus"" = 'C' THEN 'Closed' END AS ""LineStatus"", T1.""OpenQty"" " & _
                        " FROM ""RDR1""  T1 WHERE T1.""DocEntry"" ='" & DocEntry & "'"
                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                RetDT.TableName = "SQTOGEN"
                RetDT2 = RetDT.Copy
                RetDS.Tables.Add(RetDT2)

                RetDT = New DataTable
                Query = "SELECT T1.""FileName"" ||'.'|| T1.""FileExt"" AS ""U_FileName"",T1.""trgtPath""||'\'||T1.""FileName"" ||'.'|| T1.""FileExt"" AS ""U_FilePath"", " & _
                        " T1.""Line"" AS ""U_id"", TO_CHAR(T1.""Date"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                        " FROM ""ORDR""  T0 INNER JOIN ""ATC1"" T1 ON T1.""AbsEntry"" =  T0.""AtcEntry"" " & _
                        " WHERE T0.""DocEntry"" = '" & DocEntry & "' "
                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                RetDT.TableName = "ATTACHMENT"
                RetDT3 = RetDT.Copy
                RetDS.Tables.Add(RetDT3)

                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                RetDT = New DataTable
                RetDT.TableName = "VALIDATE"
                RetDT.Columns.Add("Status")
                RetDT.Columns.Add("Msg")
                RetDT.Rows.Add()
                RetDT.Rows(0)(0) = True
                RetDT.Rows(0)(1) = "No Record Found"
                RetDS.Tables.Add(RetDT)
                Context.Response.Output.Write(fn.ds2json(RetDS))
            End If
        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub SalesOrder_PreviousRecord(ByVal value As String)
        sFunction = "SalesOrder_PreviousRecord"
        Try
            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim ds As DataSet = fn.jsontodata(value)
            Dim LEADM As New DataTable
            Dim DocNum As String = ""
            Dim UserCode As String = ""
            If ds.Tables("ORDR").Rows.Count > 0 Then
                LEADM = ds.Tables("ORDR")
                Dim dr As DataRow = LEADM.Rows(0)
                DocNum = dr.Item("U_OrderNo").ToString
                UserCode = dr.Item("U_UCode").ToString.Trim().ToUpper
                Dim SQLStr As String = ""
                Dim sErrDesc As String = ""
                Dim DType As String = ""
                Dim CompCode As String = ""
                SQLStr = "SELECT T0.""U_DAuthor"" FROM ""@WUSER""  T0 WHERE Upper(T0.""Code"") ='" & UserCode & "'"
                DType = fn.ExecuteSQLQuery_SingleValue(SQLStr, sErrDesc)
                If sErrDesc <> "" Then
                    Throw New Exception(sErrDesc)
                ElseIf DType = "" Then
                    Throw New Exception("No Record Found!")
                End If

                Dim Query As String = ""
                If DType = "By Company" Then
                    SQLStr = "SELECT ifnull(T0.""U_ComCode"",'')  As ""U_ComCode"" FROM ""@WUSER""  T0 WHERE T0.""Code"" ='" & UserCode & "'"
                    CompCode = fn.ExecuteSQLQuery_SingleValue(SQLStr, sErrDesc)
                    If sErrDesc <> "" Then
                        Throw New Exception(sErrDesc)
                    ElseIf CompCode = "" Then
                        Throw New Exception("No Record Found!")
                    End If
                    If DocNum = "" Then
                        Query = "SELECT Top 1 T0.""DocEntry"" as ""U_OrderNo"",T0.""DocNum"" AS ""SODocNum"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", " & _
                                " TO_CHAR(T0.""DocDate"",'DD/MM/YYYY') As ""DocDate"", T0.""CardCode"",T0.""CardName"",T0.""U_SurveyorID"",T0.""NumAtCard"",T0.""U_STypeCode"", " & _
                                " T0.""U_U_STypeName""  As  U_STypeName, T0.""U_Country"", T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"",T0.""U_IMO"", " & _
                                " T2.""U_HYPERLINK_BEFORE"" AS ""U_HLinkBef"",T2.""U_HYPERLINK_AFTER"" AS ""U_HLinkAft"" " & _
                                " FROM ORDR T0 " & _
                                " INNER JOIN RDR1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" " & _
                                " LEFT JOIN ""@SURVEYTYPE_HLINK"" T2 ON UPPER(T2.""U_SURVEYTYPENAME"") = UPPER(T1.""Dscription"") " & _
                                " where T0.""U_UCode"" in (SELECT T0.""Code"" FROM ""@WUSER""  T0 WHERE T0.""U_ComCode""  ='" & CompCode & "') ORDER BY T0.""DocEntry"" ASC"
                    Else
                        Query = "SELECT Top 1 T0.""DocEntry"" as ""U_OrderNo"",T0.""DocNum"" AS ""SODocNum"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", " & _
                                " TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY') As ""DocDate"",T0.""U_IMO"",T0.""CardCode"",T0.""CardName"",T0.""U_SurveyorID"",T0.""NumAtCard"",T0.""U_STypeCode"", " & _
                                " T0.""U_U_STypeName""  As  U_STypeName, T0.""U_Country"", T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"", " & _
                                " T2.""U_HYPERLINK_BEFORE"" AS ""U_HLinkBef"",T2.""U_HYPERLINK_AFTER"" AS ""U_HLinkAft"" " & _
                                " FROM ORDR T0 " & _
                                " INNER JOIN RDR1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" " & _
                                " LEFT JOIN ""@SURVEYTYPE_HLINK"" T2 ON UPPER(T2.""U_SURVEYTYPENAME"") = UPPER(T1.""Dscription"") " & _
                                " where T0.""DocEntry"" = (SELECT	Top 1 ifnull(T0.""DocEntry"",0) ""DocEntry"" " & _
                                "                          FROM ORDR T0 where T0.""U_UCode"" in (SELECT T0.""Code"" FROM ""@WUSER"" T0 WHERE T0.""U_ComCode"" ='" & CompCode & "' ) " & _
                                "                          and  T0.""DocEntry""<'" & DocNum & "' ORDER BY T0.""DocEntry"" DESC)"
                    End If
                ElseIf DType = "By Country" Then
                    If DocNum = "" Then
                        'Query = "SELECT Top 1 T0.""DocEntry"" as ""U_OrderNo"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", " & _
                        '        " TO_CHAR(T0.""DocDate"",'DD/MM/YYYY') As ""DocDate"", T0.""CardCode"",T0.""CardName"",T0.""U_SurveyorID"",T0.""NumAtCard"",T0.""U_STypeCode"", " & _
                        '        " T0.""U_U_STypeName""  As  U_STypeName, T0.""U_Country"", T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"" " & _
                        '        " FROM ORDR T0 " & _
                        '        " where T0.""U_UCode"" in (SELECT T0.""Code"" FROM ""@WUSER""  T0 WHERE T0.""U_ComCode""  ='" & CompCode & "') " & _
                        '        " AND T0.""CardCode"" IN (SELECT DISTINCT ""CardCode"" FROM ""CRD1"" WHERE ""Country"" IN (SELECT ""U_CCode"" FROM ""@COUNTRY"")) ORDER BY T0.""DocEntry"" ASC"

                        Query = "SELECT Top 1 T0.""DocEntry"" as ""U_OrderNo"",T0.""DocNum"" AS ""SODocNum"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", " & _
                                " TO_CHAR(T0.""DocDate"",'DD/MM/YYYY') As ""DocDate"",T0.""U_IMO"", T0.""CardCode"",T0.""CardName"",T0.""U_SurveyorID"",T0.""NumAtCard"",T0.""U_STypeCode"", " & _
                                " T0.""U_U_STypeName""  As  U_STypeName, T0.""U_Country"", T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"", " & _
                                " T2.""U_HYPERLINK_BEFORE"" AS ""U_HLinkBef"",T2.""U_HYPERLINK_AFTER"" AS ""U_HLinkAft"" " & _
                                " FROM ORDR T0 " & _
                                " INNER JOIN RDR1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" " & _
                                " LEFT JOIN ""@SURVEYTYPE_HLINK"" T2 ON UPPER(T2.""U_SURVEYTYPENAME"") = UPPER(T1.""Dscription"") " & _
                                " where T0.""CardCode"" IN (SELECT DISTINCT ""CardCode"" FROM ""CRD1"" WHERE ""Country"" IN (SELECT ""U_CCode"" FROM ""@COUNTRY"")) ORDER BY T0.""DocEntry"" ASC"
                    Else
                        'Query = "SELECT Top 1 T0.""DocEntry"" as ""U_OrderNo"",T0.""DocNum"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", " & _
                        '        " TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY') As ""DocDate"",T0.""CardCode"",T0.""CardName"",T0.""U_SurveyorID"",T0.""NumAtCard"",T0.""U_STypeCode"", " & _
                        '        " T0.""U_U_STypeName""  As  U_STypeName, T0.""U_Country"", T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"" " & _
                        '        " FROM ORDR T0 where T0.""DocEntry"" = (SELECT	Top 1 ifnull(T0.""DocEntry"",0) ""DocEntry"" " & _
                        '        "                                       FROM ORDR T0 where T0.""U_UCode"" in (SELECT T0.""Code"" FROM ""@WUSER"" T0 WHERE T0.""U_ComCode"" ='" & CompCode & "' ) " & _
                        '        "                                       and  T0.""DocEntry""<'" & DocNum & "' ORDER BY T0.""DocEntry"" DESC) " & _
                        '        " AND T0.""CardCode"" IN (SELECT DISTINCT ""CardCode"" FROM ""CRD1"" WHERE ""Country"" IN (SELECT ""U_CCode"" FROM ""@COUNTRY"")) "

                        Query = "SELECT Top 1 T0.""DocEntry"" as ""U_OrderNo"",T0.""DocNum"" AS ""SODocNum"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", " & _
                                " TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY') As ""DocDate"",T0.""U_IMO"", T0.""CardCode"",T0.""CardName"",T0.""U_SurveyorID"",T0.""NumAtCard"",T0.""U_STypeCode"", " & _
                                " T0.""U_U_STypeName""  As  U_STypeName, T0.""U_Country"", T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"", " & _
                                " T2.""U_HYPERLINK_BEFORE"" AS ""U_HLinkBef"",T2.""U_HYPERLINK_AFTER"" AS ""U_HLinkAft"" " & _
                                " FROM ORDR T0 " & _
                                " INNER JOIN RDR1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" " & _
                                " LEFT JOIN ""@SURVEYTYPE_HLINK"" T2 ON UPPER(T2.""U_SURVEYTYPENAME"") = UPPER(T1.""Dscription"") " & _
                                " where T0.""DocEntry"" = (SELECT	Top 1 ifnull(T0.""DocEntry"",0) ""DocEntry"" " & _
                                "                                       FROM ORDR T0 where T0.""DocEntry""<'" & DocNum & "' " & _
                                "                                       AND T0.""CardCode"" IN (SELECT DISTINCT ""CardCode"" FROM ""CRD1"" WHERE ""Country"" IN (SELECT ""U_CCode"" FROM ""@COUNTRY"")) " & _
                                "                                       ORDER BY T0.""DocEntry"" DESC) " & _
                                " AND T0.""CardCode"" IN (SELECT DISTINCT ""CardCode"" FROM ""CRD1"" WHERE ""Country"" IN (SELECT ""U_CCode"" FROM ""@COUNTRY"")) "
                    End If
                Else
                    Throw New Exception("No Record Found!")
                End If

                Dim RetDT As New DataTable
                Dim RetDT1 As New DataTable
                Dim RetDT2 As New DataTable
                Dim RetDT3 As New DataTable
                Dim RetDS As New DataSet
                Dim ErrMsg As String
                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                If RetDT.Rows.Count > 0 Then
                    DocEntry = RetDT.Rows(0)(0)
                    RetDT.TableName = "ORDR"
                    Dim dr1 As DataRow = RetDT.Rows(0)
                    DocEntry = dr1.Item("U_OrderNo").ToString.Trim()
                    RetDT1 = RetDT.Copy
                    RetDS.Tables.Add(RetDT1)
                    RetDT = New DataTable

                    Query = "SELECT T1.""Quantity"", TO_CHAR( T1.""U_PDate"" , 'DD/MM/YYYY') ""U_PDate"", T1.""U_EQType"", T1.""U_SCriteria"",IFNULL(T1.""U_ReSurvey"",'NO') AS ""U_ReSurvey"", " & _
                            " T1.""LineNum"",CASE WHEN T1.""LineStatus"" = 'O' THEN 'Open' WHEN T1.""LineStatus"" = 'C' THEN 'Closed' END AS ""LineStatus"", T1.""OpenQty"" " & _
                            " FROM ""RDR1""  T1 WHERE T1.""DocEntry"" ='" & DocEntry & "'"
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If

                    RetDT.TableName = "SQTOGEN"
                    RetDT2 = RetDT.Copy
                    RetDS.Tables.Add(RetDT2)

                    RetDT = New DataTable
                    Query = "SELECT T1.""FileName"" ||'.'|| T1.""FileExt"" AS ""U_FileName"",T1.""trgtPath""||'\'||T1.""FileName"" ||'.'|| T1.""FileExt"" AS ""U_FilePath"", " & _
                            " T1.""Line"" AS ""U_id"", TO_CHAR(T1.""Date"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                            " FROM ""ORDR""  T0 INNER JOIN ""ATC1"" T1 ON T1.""AbsEntry"" =  T0.""AtcEntry"" " & _
                            " WHERE T0.""DocEntry"" = '" & DocEntry & "' "
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "ATTACHMENT"
                    RetDT3 = RetDT.Copy
                    RetDS.Tables.Add(RetDT3)

                    Context.Response.Output.Write(fn.ds2json(RetDS))
                Else
                    Query = "SELECT Top 1 T0.""DocEntry"" as ""U_OrderNo"",T0.""DocNum"" AS ""SODocNum"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", " & _
                            " TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY') As ""DocDate"", T0.""U_IMO"", T0.""CardCode"",  T0.""CardName"", T0.""U_SurveyorID"", T0.""NumAtCard"",T0.""U_STypeCode"", " & _
                            " T0.""U_U_STypeName""  As  U_STypeName, T0.""U_Country"", T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"", " & _
                            " T2.""U_HYPERLINK_BEFORE"" AS ""U_HLinkBef"",T2.""U_HYPERLINK_AFTER"" AS ""U_HLinkAft"" " & _
                            " FROM ORDR T0 " & _
                            " INNER JOIN RDR1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" " & _
                            " LEFT JOIN ""@SURVEYTYPE_HLINK"" T2 ON UPPER(T2.""U_SURVEYTYPENAME"") = UPPER(T1.""Dscription"") " & _
                            " where T0.""U_UCode"" in (SELECT T0.""Code"" FROM ""@WUSER""  T0 WHERE T0.""U_ComCode""  ='" & CompCode & "') " & _
                            " AND T0.""CardCode"" IN (SELECT DISTINCT ""CardCode"" FROM ""CRD1"" WHERE ""Country"" IN (SELECT ""U_CCode"" FROM ""@COUNTRY"")) ORDER BY T0.""DocEntry"" DESC"
                    RetDT = New DataTable
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    If RetDT.Rows.Count > 0 Then
                        RetDT.TableName = "ORDR"
                        Dim dr1 As DataRow = RetDT.Rows(0)
                        DocEntry = dr1.Item("U_OrderNo").ToString.Trim()
                        RetDT1 = RetDT.Copy
                        RetDS.Tables.Add(RetDT1)

                        RetDT = New DataTable

                        Query = "SELECT T1.""Quantity"", TO_CHAR( T1.""U_PDate"" , 'DD/MM/YYYY') ""U_PDate"", T1.""U_EQType"", T1.""U_SCriteria"",IFNULL(T1.""U_ReSurvey"",'NO') AS ""U_ReSurvey"", " & _
                                " T1.""LineNum"",CASE WHEN T1.""LineStatus"" = 'O' THEN 'Open' WHEN T1.""LineStatus"" = 'C' THEN 'Closed' END AS ""LineStatus"", T1.""OpenQty"" " & _
                                " FROM ""RDR1""  T1 WHERE T1.""DocEntry"" ='" & DocEntry & "'"
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "SQTOGEN"
                        RetDT2 = RetDT.Copy
                        RetDS.Tables.Add(RetDT2)

                        RetDT = New DataTable
                        Query = "SELECT T1.""FileName"" ||'.'|| T1.""FileExt"" AS ""U_FileName"",T1.""trgtPath""||'\'||T1.""FileName"" ||'.'|| T1.""FileExt"" AS ""U_FilePath"", " & _
                                " T1.""Line"" AS ""U_id"", TO_CHAR(T1.""Date"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                                " FROM ""ORDR""  T0 INNER JOIN ""ATC1"" T1 ON T1.""AbsEntry"" =  T0.""AtcEntry"" " & _
                                " WHERE T0.""DocEntry"" = '" & DocEntry & "' "
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "ATTACHMENT"
                        RetDT3 = RetDT.Copy
                        RetDS.Tables.Add(RetDT3)

                        Context.Response.Output.Write(fn.ds2json(RetDS))

                    Else
                        RetDT = New DataTable
                        RetDT.TableName = "VALIDATE"
                        RetDT.Columns.Add("Status")
                        RetDT.Columns.Add("Msg")
                        RetDT.Rows.Add()
                        RetDT.Rows(0)(0) = True
                        RetDT.Rows(0)(1) = "No Record Found"
                        RetDS.Tables.Add(RetDT)
                        Context.Response.Output.Write(fn.ds2json(RetDS))
                    End If
                End If
            Else
                Throw New Exception("No Record Found")
            End If
        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub SalesOrder_NextRecord(ByVal value As String)
        sFunction = "SalesOrder_NextRecord"
        Try
            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim ds As DataSet = fn.jsontodata(value)
            Dim LEADM As New DataTable
            Dim DocNum As String = ""
            Dim UserCode As String = ""
            If ds.Tables("ORDR").Rows.Count > 0 Then
                LEADM = ds.Tables("ORDR")
                Dim dr As DataRow = LEADM.Rows(0)
                DocNum = dr.Item("U_OrderNo").ToString
                UserCode = dr.Item("U_UCode").ToString.Trim().ToUpper
                Dim SQLStr As String = ""
                Dim sErrDesc As String = ""
                Dim DType As String = ""
                Dim CompCode As String = ""
                SQLStr = "SELECT T0.""U_DAuthor"" FROM ""@WUSER""  T0 WHERE Upper(T0.""Code"") ='" & UserCode & "'"
                DType = fn.ExecuteSQLQuery_SingleValue(SQLStr, sErrDesc)
                If sErrDesc <> "" Then
                    Throw New Exception(sErrDesc)
                ElseIf DType = "" Then
                    Throw New Exception("No Record Found!")
                End If

                Dim Query As String = ""
                If DType = "By Company" Then
                    SQLStr = "SELECT ifnull(T0.""U_ComCode"",'')  As ""U_ComCode"" FROM ""@WUSER""  T0 WHERE T0.""Code"" ='" & UserCode & "'"
                    CompCode = fn.ExecuteSQLQuery_SingleValue(SQLStr, sErrDesc)
                    If sErrDesc <> "" Then
                        Throw New Exception(sErrDesc)
                    ElseIf CompCode = "" Then
                        Throw New Exception("No Record Found!")
                    End If

                    If DocNum = "" Then

                        Query = "SELECT Top 1 T0.""DocEntry"" as ""U_OrderNo"",T0.""DocNum"" AS ""SODocNum"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", " & _
                                " TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY') As ""DocDate"", T0.""U_IMO"", T0.""CardCode"",  T0.""CardName"", T0.""U_SurveyorID"", T0.""NumAtCard"",T0.""U_STypeCode"", " & _
                                " T0.""U_U_STypeName""  As  U_STypeName, T0.""U_Country"", T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"", " & _
                                " T2.""U_HYPERLINK_BEFORE"" AS ""U_HLinkBef"",T2.""U_HYPERLINK_AFTER"" AS ""U_HLinkAft"" " & _
                                " FROM ORDR T0 " & _
                                " INNER JOIN RDR1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" " & _
                                " LEFT JOIN ""@SURVEYTYPE_HLINK"" T2 ON UPPER(T2.""U_SURVEYTYPENAME"") = UPPER(T1.""Dscription"") " & _
                                " where T0.""U_UCode"" in (SELECT T0.""Code"" FROM ""@WUSER""  T0 WHERE T0.""U_ComCode""  ='" & CompCode & "') ORDER BY T0.""DocEntry"" DESC"
                    Else
                        Query = "SELECT Top 1 T0.""DocEntry"" as ""U_OrderNo"",T0.""DocNum"" AS ""SODocNum"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", " & _
                                " TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY') As ""DocDate"", T0.""U_IMO"", T0.""CardCode"",  T0.""CardName"", T0.""U_SurveyorID"", T0.""NumAtCard"",T0.""U_STypeCode"", " & _
                                " T0.""U_U_STypeName""  As  U_STypeName, T0.""U_Country"", T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"", " & _
                                " T2.""U_HYPERLINK_BEFORE"" AS ""U_HLinkBef"",T2.""U_HYPERLINK_AFTER"" AS ""U_HLinkAft"" " & _
                                " FROM ORDR T0 " & _
                                " INNER JOIN RDR1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" " & _
                                " LEFT JOIN ""@SURVEYTYPE_HLINK"" T2 ON UPPER(T2.""U_SURVEYTYPENAME"") = UPPER(T1.""Dscription"") " & _
                                " where T0.""DocEntry"" = (SELECT	Top 1 ifnull(T0.""DocEntry"",0) ""DocEntry"" " & _
                                "                                       FROM ORDR T0 where T0.""U_UCode"" in (SELECT T0.""Code"" FROM ""@WUSER"" T0 WHERE T0.""U_ComCode"" ='" & CompCode & "') " & _
                                "                                       AND T0.""DocEntry"" > '" & DocNum & "' ORDER BY T0.""DocEntry"" ASC)"
                    End If
                ElseIf DType = "By Country" Then
                    If DocNum = "" Then
                        'Query = "SELECT Top 1 T0.""DocEntry"" as ""U_OrderNo"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", " & _
                        '        " TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY') As ""DocDate"",  T0.""CardCode"",  T0.""CardName"", T0.""U_SurveyorID"", T0.""NumAtCard"",T0.""U_STypeCode"", " & _
                        '        " T0.""U_U_STypeName""  As  U_STypeName, T0.""U_Country"", T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"" " & _
                        '        " FROM ORDR T0 " & _
                        '        " where T0.""U_UCode"" in (SELECT T0.""Code"" FROM ""@WUSER""  T0 WHERE T0.""U_ComCode""  ='" & CompCode & "') " & _
                        '        " AND T0.""CardCode"" IN (SELECT DISTINCT ""CardCode"" FROM ""CRD1"" WHERE ""Country"" IN (SELECT ""U_CCode"" FROM ""@COUNTRY"")) ORDER BY T0.""DocEntry"" DESC"

                        Query = "SELECT Top 1 T0.""DocEntry"" as ""U_OrderNo"",T0.""DocNum"" AS ""SODocNum"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", " & _
                               " TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY') As ""DocDate"", T0.""U_IMO"", T0.""CardCode"",  T0.""CardName"", T0.""U_SurveyorID"", T0.""NumAtCard"",T0.""U_STypeCode"", " & _
                               " T0.""U_U_STypeName""  As  U_STypeName, T0.""U_Country"", T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"", " & _
                               " T2.""U_HYPERLINK_BEFORE"" AS ""U_HLinkBef"",T2.""U_HYPERLINK_AFTER"" AS ""U_HLinkAft"" " & _
                               " FROM ORDR T0 " & _
                               " INNER JOIN RDR1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" " & _
                               " LEFT JOIN ""@SURVEYTYPE_HLINK"" T2 ON UPPER(T2.""U_SURVEYTYPENAME"") = UPPER(T1.""Dscription"") " & _
                               " where T0.""CardCode"" IN (SELECT DISTINCT ""CardCode"" FROM ""CRD1"" WHERE ""Country"" IN (SELECT ""U_CCode"" FROM ""@COUNTRY"")) ORDER BY T0.""DocEntry"" DESC"
                    Else
                        'Query = "SELECT Top 1 T0.""DocEntry"" as ""U_OrderNo"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", " & _
                        '        " TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY') As ""DocDate"",  T0.""CardCode"",  T0.""CardName"", T0.""U_SurveyorID"", T0.""NumAtCard"",T0.""U_STypeCode"", " & _
                        '        " T0.""U_U_STypeName""  As  U_STypeName, T0.""U_Country"", T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"" " & _
                        '        " FROM ORDR T0 where T0.""DocEntry"" = (SELECT	Top 1 ifnull(T0.""DocEntry"",0) ""DocEntry"" " & _
                        '        "                                       FROM ORDR T0 where T0.""U_UCode"" in (SELECT T0.""Code"" FROM ""@WUSER"" T0 WHERE T0.""U_ComCode"" ='" & CompCode & "') " & _
                        '        "                                       AND T0.""DocEntry"" > '" & DocNum & "' ORDER BY T0.""DocEntry"" ASC) " & _
                        '        " AND T0.""CardCode"" IN (SELECT DISTINCT ""CardCode"" FROM ""CRD1"" WHERE ""Country"" IN (SELECT ""U_CCode"" FROM ""@COUNTRY"")) "

                        Query = "SELECT Top 1 T0.""DocEntry"" as ""U_OrderNo"",T0.""DocNum"" AS ""SODocNum"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", " & _
                                " TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY') As ""DocDate"", T0.""U_IMO"", T0.""CardCode"",  T0.""CardName"", T0.""U_SurveyorID"", T0.""NumAtCard"",T0.""U_STypeCode"", " & _
                                " T0.""U_U_STypeName""  As  U_STypeName, T0.""U_Country"", T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"", " & _
                                " T2.""U_HYPERLINK_BEFORE"" AS ""U_HLinkBef"",T2.""U_HYPERLINK_AFTER"" AS ""U_HLinkAft"" " & _
                                " FROM ORDR T0 " & _
                                " INNER JOIN RDR1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" " & _
                                " LEFT JOIN ""@SURVEYTYPE_HLINK"" T2 ON UPPER(T2.""U_SURVEYTYPENAME"") = UPPER(T1.""Dscription"") " & _
                                " where T0.""DocEntry"" = (SELECT	Top 1 ifnull(T0.""DocEntry"",0) ""DocEntry"" " & _
                                "                                       FROM ORDR T0 where T0.""DocEntry"" > '" & DocNum & "' " & _
                                "                                       AND T0.""CardCode"" IN (SELECT DISTINCT ""CardCode"" FROM ""CRD1"" WHERE ""Country"" IN (SELECT ""U_CCode"" FROM ""@COUNTRY""))  " & _
                                "                                       ORDER BY T0.""DocEntry"" ASC) " & _
                                " AND T0.""CardCode"" IN (SELECT DISTINCT ""CardCode"" FROM ""CRD1"" WHERE ""Country"" IN (SELECT ""U_CCode"" FROM ""@COUNTRY"")) "
                    End If
                Else
                    Throw New Exception("No Record Found!")
                End If

                Dim RetDT As New DataTable
                Dim RetDT1 As New DataTable
                Dim RetDT2 As New DataTable
                Dim RetDT3 As New DataTable
                Dim RetDS As New DataSet
                Dim ErrMsg As String = ""
                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                If RetDT.Rows.Count > 0 Then
                    DocEntry = RetDT.Rows(0)(0)
                    RetDT.TableName = "ORDR"
                    Dim dr1 As DataRow = RetDT.Rows(0)
                    DocEntry = dr1.Item("U_OrderNo").ToString.Trim()
                    RetDT1 = RetDT.Copy
                    RetDS.Tables.Add(RetDT1)
                    RetDT = New DataTable

                    Query = "SELECT T1.""Quantity"", TO_CHAR( T1.""U_PDate"" , 'DD/MM/YYYY') ""U_PDate"", T1.""U_EQType"", T1.""U_SCriteria"",IFNULL(T1.""U_ReSurvey"",'NO') AS ""U_ReSurvey"", " & _
                            " T1.""LineNum"",CASE WHEN T1.""LineStatus"" = 'O' THEN 'Open' WHEN T1.""LineStatus"" = 'C' THEN 'Closed' END AS ""LineStatus"", T1.""OpenQty"" " & _
                            " FROM ""RDR1""  T1 WHERE T1.""DocEntry"" ='" & DocEntry & "'"
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If

                    RetDT.TableName = "SQTOGEN"
                    RetDT2 = RetDT.Copy
                    RetDS.Tables.Add(RetDT2)

                    RetDT = New DataTable
                    Query = "SELECT T1.""FileName"" ||'.'|| T1.""FileExt"" AS ""U_FileName"",T1.""trgtPath""||'\'||T1.""FileName"" ||'.'|| T1.""FileExt"" AS ""U_FilePath"", " & _
                            " T1.""Line"" AS ""U_id"", TO_CHAR(T1.""Date"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                            " FROM ""ORDR""  T0 INNER JOIN ""ATC1"" T1 ON T1.""AbsEntry"" =  T0.""AtcEntry"" " & _
                            " WHERE T0.""DocEntry"" = '" & DocEntry & "' "
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "ATTACHMENT"
                    RetDT3 = RetDT.Copy
                    RetDS.Tables.Add(RetDT3)

                    Context.Response.Output.Write(fn.ds2json(RetDS))
                Else

                    Query = "SELECT Top 1 T0.""DocEntry"" as ""U_OrderNo"",T0.""DocNum"" AS ""SODocNum"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", " & _
                            " TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY') As ""DocDate"", T0.""U_IMO"", T0.""CardCode"",  T0.""CardName"", T0.""U_SurveyorID"", T0.""NumAtCard"",T0.""U_STypeCode"", " & _
                            " T0.""U_U_STypeName""  As  U_STypeName, T0.""U_Country"", T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"", " & _
                            " T2.""U_HYPERLINK_BEFORE"" AS ""U_HLinkBef"",T2.""U_HYPERLINK_AFTER"" AS ""U_HLinkAft"" " & _
                            " FROM ORDR T0 " & _
                            " INNER JOIN RDR1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" " & _
                            " LEFT JOIN ""@SURVEYTYPE_HLINK"" T2 ON UPPER(T2.""U_SURVEYTYPENAME"") = UPPER(T1.""Dscription"") " & _
                            " where T0.""U_UCode"" in (SELECT T0.""Code"" FROM ""@WUSER""  T0 WHERE T0.""U_ComCode""  ='" & CompCode & "') ORDER BY T0.""DocEntry"" ASC"
                    RetDT = New DataTable
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    If RetDT.Rows.Count > 0 Then
                        RetDT.TableName = "ORDR"
                        Dim dr1 As DataRow = RetDT.Rows(0)
                        DocEntry = dr1.Item("U_OrderNo").ToString.Trim()
                        RetDT1 = RetDT.Copy
                        RetDS.Tables.Add(RetDT1)

                        RetDT = New DataTable

                        Query = "SELECT T1.""Quantity"", TO_CHAR( T1.""U_PDate"" , 'DD/MM/YYYY') ""U_PDate"", T1.""U_EQType"", T1.""U_SCriteria"",IFNULL(T1.""U_ReSurvey"",'NO') AS ""U_ReSurvey"", " & _
                                " T1.""LineNum"",CASE WHEN T1.""LineStatus"" = 'O' THEN 'Open' WHEN T1.""LineStatus"" = 'C' THEN 'Closed' END AS ""LineStatus"", T1.""OpenQty"" FROM ""RDR1""  T1 WHERE T1.""DocEntry"" ='" & DocEntry & "'"
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "SQTOGEN"
                        RetDT2 = RetDT.Copy
                        RetDS.Tables.Add(RetDT2)

                        RetDT = New DataTable
                        Query = "SELECT T1.""FileName"" ||'.'|| T1.""FileExt"" AS ""U_FileName"",T1.""trgtPath""||'\'||T1.""FileName"" ||'.'|| T1.""FileExt"" AS ""U_FilePath"", " & _
                                " T1.""Line"" AS ""U_id"", TO_CHAR(T1.""Date"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                                " FROM ""ORDR""  T0 INNER JOIN ""ATC1"" T1 ON T1.""AbsEntry"" =  T0.""AtcEntry"" " & _
                                " WHERE T0.""DocEntry"" = '" & DocEntry & "' "
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "ATTACHMENT"
                        RetDT3 = RetDT.Copy
                        RetDS.Tables.Add(RetDT3)

                        Context.Response.Output.Write(fn.ds2json(RetDS))

                    Else
                        RetDT = New DataTable
                        RetDT.TableName = "VALIDATE"
                        RetDT.Columns.Add("Status")
                        RetDT.Columns.Add("Msg")
                        RetDT.Rows.Add()
                        RetDT.Rows(0)(0) = True
                        RetDT.Rows(0)(1) = "No Record Found"
                        RetDS.Tables.Add(RetDT)
                        Context.Response.Output.Write(fn.ds2json(RetDS))
                    End If
                End If
            Else
                Throw New Exception("No Record Found")
            End If
        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub

    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub SalesOrder_Find(ByVal value As String)
        sFunction = "SalesOrder_Find"
        Try
            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim ds As DataSet = fn.jsontodata(value)
            Dim LEADM As New DataTable
            Dim DocNum As String = ""
            Dim UserCode As String = ""
            If ds.Tables("ORDR").Rows.Count > 0 Then
                LEADM = ds.Tables("ORDR")
                Dim dr As DataRow = LEADM.Rows(0)
                DocNum = dr.Item("U_OrderNo").ToString
                UserCode = dr.Item("U_UCode").ToString.Trim().ToUpper
                Dim SQLStr As String = ""
                Dim sErrDesc As String = ""
                Dim DType As String = ""
                Dim CompCode As String = ""
                SQLStr = "SELECT T0.""U_DAuthor"" FROM ""@WUSER""  T0 WHERE Upper(T0.""Code"") ='" & UserCode & "'"
                DType = fn.ExecuteSQLQuery_SingleValue(SQLStr, sErrDesc)
                If sErrDesc <> "" Then
                    Throw New Exception(sErrDesc)
                ElseIf DType = "" Then
                    Throw New Exception("No Record Found!")
                End If
                Dim Query As String = ""
                If DType = "By Company" Then
                    SQLStr = "SELECT ifnull(T0.""U_ComCode"",'')  As ""U_ComCode"" FROM ""@WUSER""  T0 WHERE T0.""Code"" ='" & UserCode & "'"
                    CompCode = fn.ExecuteSQLQuery_SingleValue(SQLStr, sErrDesc)
                    If sErrDesc <> "" Then
                        Throw New Exception(sErrDesc)
                    ElseIf CompCode = "" Then
                        Throw New Exception("No Record Found!")
                    End If
                    If DocNum = "" Then

                        Query = "SELECT Top 1 T0.""DocEntry"" as ""U_OrderNo"",T0.""DocNum"" AS ""SODocNum"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", " & _
                                " T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY') As ""DocDate"", " & _
                                " T0.""CardCode"",T0.""CardName"",T0.""U_SurveyorID"",T0.""NumAtCard"",T0.""U_STypeCode"",T0.""U_U_STypeName"" As U_STypeName, T0.""U_Country"",  " & _
                                " T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"",T2.""U_HYPERLINK_BEFORE"" AS ""U_HLinkBef"",T2.""U_HYPERLINK_AFTER"" AS ""U_HLinkAft""  " & _
                                " FROM ORDR T0 " & _
                                " INNER JOIN RDR1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" " & _
                                " LEFT JOIN ""@SURVEYTYPE_HLINK"" T2 ON UPPER(T2.""U_SURVEYTYPENAME"") = UPPER(T1.""Dscription"") " & _
                                " where T0.""U_UCode"" in (SELECT T0.""Code"" FROM ""@WUSER""  T0 WHERE T0.""U_ComCode""  ='" & CompCode & "') ORDER BY T0.""DocEntry"" ASC"
                    Else

                        'Query = "SELECT Top 1 T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"",  T0.""U_Ccode"", T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@CCON"" T0  where T0.""DocEntry"" =(SELECT (T0.""DocEntry"" - 1)  ""DocEntry"" FROM ""@CCON""  T0 WHERE T0.""DocNum"" ='" & DocNum & "')"
                        Query = "SELECT Top 1 T0.""DocEntry"" as ""U_OrderNo"",T0.""DocNum"" AS ""SODocNum"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", " & _
                                " T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", TO_CHAR( T0.""DocDate"",'DD/MM/YYYY') As ""DocDate"", " & _
                                " T0.""CardCode"",T0.""CardName"", T0.""U_SurveyorID"", T0.""NumAtCard"",T0.""U_STypeCode"",T0.""U_U_STypeName"" As U_STypeName, T0.""U_Country"", " & _
                                " T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"",T2.""U_HYPERLINK_BEFORE"" AS ""U_HLinkBef"",T2.""U_HYPERLINK_AFTER"" AS ""U_HLinkAft""  " & _
                                " FROM ORDR T0 " & _
                                " INNER JOIN RDR1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" " & _
                                " LEFT JOIN ""@SURVEYTYPE_HLINK"" T2 ON UPPER(T2.""U_SURVEYTYPENAME"") = UPPER(T1.""Dscription"") " & _
                                " where T0.""DocEntry"" = (SELECT	Top 1 ifnull(T0.""DocEntry"",0) ""DocEntry"" FROM ORDR T0 " & _
                                "                          where T0.""U_UCode"" in (SELECT T0.""Code"" FROM ""@WUSER"" T0 WHERE T0.""U_ComCode"" ='" & CompCode & "' ) " & _
                                "                          and  T0.""DocEntry"" = '" & DocNum & "' ORDER BY T0.""DocEntry"" DESC)"
                    End If
                ElseIf DType = "By Country" Then
                    If DocNum = "" Then
                        'Query = "SELECT Top 1 T0.""DocEntry"" as ""U_OrderNo"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", " & _
                        '        " T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY') As ""DocDate"", " & _
                        '        " T0.""CardCode"",T0.""CardName"",T0.""U_SurveyorID"",T0.""NumAtCard"",T0.""U_STypeCode"",T0.""U_U_STypeName"" As U_STypeName, T0.""U_Country"",  " & _
                        '        " T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"" FROM ORDR T0 " & _
                        '        " where T0.""U_UCode"" in (SELECT T0.""Code"" FROM ""@WUSER""  T0 WHERE T0.""U_ComCode""  ='" & CompCode & "') " & _
                        '        " AND T0.""CardCode"" IN (SELECT DISTINCT ""CardCode"" FROM ""CRD1"" WHERE ""Country"" IN (SELECT ""U_CCode"" FROM ""@COUNTRY"")) ORDER BY T0.""DocEntry"" ASC"
                        Query = "SELECT Top 1 T0.""DocEntry"" as ""U_OrderNo"",T0.""DocNum"" AS ""SODocNum"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", " & _
                                " T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY') As ""DocDate"", " & _
                                " T0.""CardCode"",T0.""CardName"",T0.""U_SurveyorID"",T0.""NumAtCard"",T0.""U_STypeCode"",T0.""U_U_STypeName"" As U_STypeName, T0.""U_Country"",  " & _
                                " T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"",T2.""U_HYPERLINK_BEFORE"" AS ""U_HLinkBef"",T2.""U_HYPERLINK_AFTER"" AS ""U_HLinkAft""  " & _
                                " FROM ORDR T0 " & _
                                " INNER JOIN RDR1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" " & _
                                " LEFT JOIN ""@SURVEYTYPE_HLINK"" T2 ON UPPER(T2.""U_SURVEYTYPENAME"") = UPPER(T1.""Dscription"") " & _
                                " where T0.""CardCode"" IN (SELECT DISTINCT ""CardCode"" FROM ""CRD1"" WHERE ""Country"" IN (SELECT ""U_CCode"" FROM ""@COUNTRY"")) ORDER BY T0.""DocEntry"" ASC"
                    Else
                        ''Query = "SELECT Top 1 T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"",  T0.""U_Ccode"", T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@CCON"" T0  where T0.""DocEntry"" =(SELECT (T0.""DocEntry"" - 1)  ""DocEntry"" FROM ""@CCON""  T0 WHERE T0.""DocNum"" ='" & DocNum & "')"
                        'Query = "SELECT Top 1 T0.""DocEntry"" as ""U_OrderNo"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", " & _
                        '        " T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", TO_CHAR( T0.""DocDate"",'DD/MM/YYYY') As ""DocDate"", " & _
                        '        " T0.""CardCode"",T0.""CardName"", T0.""U_SurveyorID"", T0.""NumAtCard"",T0.""U_STypeCode"",T0.""U_U_STypeName"" As U_STypeName, T0.""U_Country"", " & _
                        '        " T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"" FROM ORDR T0 " & _
                        '        " where T0.""DocEntry"" = (SELECT	Top 1 ifnull(T0.""DocEntry"",0) ""DocEntry"" FROM ORDR T0 " & _
                        '        "                          where T0.""U_UCode"" in (SELECT T0.""Code"" FROM ""@WUSER"" T0 WHERE T0.""U_ComCode"" ='" & CompCode & "' ) " & _
                        '        "                          and  T0.""DocEntry"" = '" & DocNum & "' ORDER BY T0.""DocEntry"" DESC) " & _
                        '        " AND T0.""CardCode"" IN (SELECT DISTINCT ""CardCode"" FROM ""CRD1"" WHERE ""Country"" IN (SELECT ""U_CCode"" FROM ""@COUNTRY"")) "
                        Query = "SELECT Top 1 T0.""DocEntry"" as ""U_OrderNo"",T0.""DocNum"" AS ""SODocNum"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", " & _
                                " T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", TO_CHAR( T0.""DocDate"",'DD/MM/YYYY') As ""DocDate"", " & _
                                " T0.""CardCode"",T0.""CardName"", T0.""U_SurveyorID"", T0.""NumAtCard"",T0.""U_STypeCode"",T0.""U_U_STypeName"" As U_STypeName, T0.""U_Country"", " & _
                                " T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"",T2.""U_HYPERLINK_BEFORE"" AS ""U_HLinkBef"",T2.""U_HYPERLINK_AFTER"" AS ""U_HLinkAft"" " & _
                                " FROM ORDR T0 " & _
                                " INNER JOIN RDR1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" " & _
                                " LEFT JOIN ""@SURVEYTYPE_HLINK"" T2 ON UPPER(T2.""U_SURVEYTYPENAME"") = UPPER(T1.""Dscription"") " & _
                                " where T0.""DocEntry"" = (SELECT	Top 1 ifnull(T0.""DocEntry"",0) ""DocEntry"" FROM ORDR T0 " & _
                                "                          where T0.""DocEntry"" = '" & DocNum & "' ORDER BY T0.""DocEntry"" DESC) " & _
                                " AND T0.""CardCode"" IN (SELECT DISTINCT ""CardCode"" FROM ""CRD1"" WHERE ""Country"" IN (SELECT ""U_CCode"" FROM ""@COUNTRY"")) "
                    End If
                Else
                    Throw New Exception("No Record Found!")
                End If



                Dim RetDT As New DataTable
                Dim RetDT1 As New DataTable
                Dim RetDT2 As New DataTable
                Dim RetDT3 As New DataTable
                Dim RetDS As New DataSet
                Dim ErrMsg As String
                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                If RetDT.Rows.Count > 0 Then
                    DocEntry = RetDT.Rows(0)(0)
                    RetDT.TableName = "ORDR"
                    Dim dr1 As DataRow = RetDT.Rows(0)
                    DocEntry = dr1.Item("U_OrderNo").ToString.Trim()
                    RetDT1 = RetDT.Copy
                    RetDS.Tables.Add(RetDT1)
                    RetDT = New DataTable

                    Query = "SELECT T1.""Quantity"", TO_CHAR( T1.""U_PDate"" , 'DD/MM/YYYY') ""U_PDate"", T1.""U_EQType"", T1.""U_SCriteria"",IFNULL(T1.""U_ReSurvey"",'NO') AS ""U_ReSurvey"", " & _
                            " T1.""LineNum"",CASE WHEN T1.""LineStatus"" = 'O' THEN 'Open' WHEN T1.""LineStatus"" = 'C' THEN 'Closed' END AS ""LineStatus"", T1.""OpenQty"" FROM ""RDR1""  T1 WHERE T1.""DocEntry"" ='" & DocEntry & "'"
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If

                    RetDT.TableName = "SQTOGEN"
                    RetDT2 = RetDT.Copy
                    RetDS.Tables.Add(RetDT2)

                    RetDT = New DataTable
                    Query = "SELECT T1.""FileName"" ||'.'|| T1.""FileExt"" AS ""U_FileName"",T1.""trgtPath""||'\'||T1.""FileName"" ||'.'|| T1.""FileExt"" AS ""U_FilePath"", " & _
                            " T1.""Line"" AS ""U_id"", TO_CHAR(T1.""Date"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                            " FROM ""ORDR""  T0 INNER JOIN ""ATC1"" T1 ON T1.""AbsEntry"" =  T0.""AtcEntry"" " & _
                            " WHERE T0.""DocEntry"" = '" & DocEntry & "' "
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    RetDT.TableName = "ATTACHMENT"
                    RetDT3 = RetDT.Copy
                    RetDS.Tables.Add(RetDT3)

                    Context.Response.Output.Write(fn.ds2json(RetDS))
                Else
                    Query = "SELECT Top 1 T0.""DocEntry"" as ""U_OrderNo"",T0.""DocNum"" AS ""SODocNum"", T0.""DocStatus"" as ""U_Status"", T0.""U_UCode"",T0.""U_UName"", " & _
                            " TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", TO_CHAR( T0.""DocDate"",'DD/MM/YYYY') As ""DocDate"", T0.""CardCode"", T0.""CardName"", " & _
                            " T0.""U_SurveyorID"", T0.""NumAtCard"",T0.""U_STypeCode"",  T0.""U_U_STypeName""  As  U_STypeName, T0.""U_Country"", T0.""U_City"", T0.""U_Loc"", " & _
                            " T0.""Project"", T0.""Comments"",T2.""U_HYPERLINK_BEFORE"" AS ""U_HLinkBef"",T2.""U_HYPERLINK_AFTER"" AS ""U_HLinkAft"" " & _
                            " FROM ORDR T0 " & _
                            " INNER JOIN RDR1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" " & _
                            " LEFT JOIN ""@SURVEYTYPE_HLINK"" T2 ON UPPER(T2.""U_SURVEYTYPENAME"") = UPPER(T1.""Dscription"") " & _
                            " where T0.""U_UCode"" in (SELECT T0.""Code"" FROM ""@WUSER""  T0 WHERE T0.""U_ComCode""  ='" & CompCode & "') ORDER BY T0.""DocEntry"" DESC"
                    RetDT = New DataTable
                    RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    If ErrMsg <> "" Then
                        Throw New Exception(ErrMsg)
                    End If
                    If RetDT.Rows.Count > 0 Then
                        RetDT.TableName = "ORDR"
                        Dim dr1 As DataRow = RetDT.Rows(0)
                        DocEntry = dr1.Item("U_OrderNo").ToString.Trim()
                        RetDT1 = RetDT.Copy
                        RetDS.Tables.Add(RetDT1)

                        RetDT = New DataTable

                        Query = "SELECT T1.""Quantity"", TO_CHAR( T1.""U_PDate"" , 'DD/MM/YYYY') ""U_PDate"", T1.""U_EQType"", T1.""U_SCriteria"",IFNULL(T1.""U_ReSurvey"",'NO') AS ""U_ReSurvey"", " & _
                                " T1.""LineNum"",CASE WHEN T1.""LineStatus"" = 'O' THEN 'Open' WHEN T1.""LineStatus"" = 'C' THEN 'Closed' END AS ""LineStatus"", T1.""OpenQty"" FROM ""RDR1""  T1 WHERE T1.""DocEntry"" ='" & DocEntry & "'"
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "SQTOGEN"
                        RetDT2 = RetDT.Copy
                        RetDS.Tables.Add(RetDT2)

                        RetDT = New DataTable
                        Query = "SELECT T1.""FileName"" ||'.'|| T1.""FileExt"" AS ""U_FileName"",T1.""trgtPath""||'\'||T1.""FileName"" ||'.'|| T1.""FileExt"" AS ""U_FilePath"", " & _
                                " T1.""Line"" AS ""U_id"", TO_CHAR(T1.""Date"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                                " FROM ""ORDR""  T0 INNER JOIN ""ATC1"" T1 ON T1.""AbsEntry"" =  T0.""AtcEntry"" " & _
                                " WHERE T0.""DocEntry"" = '" & DocEntry & "' "
                        RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                        If ErrMsg <> "" Then
                            Throw New Exception(ErrMsg)
                        End If
                        RetDT.TableName = "ATTACHMENT"
                        RetDT3 = RetDT.Copy
                        RetDS.Tables.Add(RetDT3)

                        Context.Response.Output.Write(fn.ds2json(RetDS))

                    Else
                        RetDT = New DataTable
                        RetDT.TableName = "VALIDATE"
                        RetDT.Columns.Add("Status")
                        RetDT.Columns.Add("Msg")
                        RetDT.Rows.Add()
                        RetDT.Rows(0)(0) = True
                        RetDT.Rows(0)(1) = "No Record Found"
                        RetDS.Tables.Add(RetDT)
                        Context.Response.Output.Write(fn.ds2json(RetDS))
                    End If
                End If
            Else
                Throw New Exception("No Record Found")
            End If
        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
 <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub SalesOrder_FindRecord_List(ByVal value As String)
        sFunction = "SalesOrder_FindRecord_List"
        Try
            oLog.WriteToLogFile_Debug(value, sFunction)
            Dim ds As DataSet = fn.jsontodata(value)
            Dim LEADM As New DataTable
            Dim DocNum As String = ""
            Dim UserCode As String = ""
            If ds.Tables("ORDR").Rows.Count > 0 Then
                LEADM = ds.Tables("ORDR")
                Dim dr As DataRow = LEADM.Rows(0)
                Try
                    DocNum = dr.Item("U_OrderNo").ToString.Trim()
                    UserCode = dr.Item("U_UCode").ToString.Trim().ToUpper
                Catch ex As Exception

                End Try

                Dim CreateDate As String = ""
                Dim CardCode As String = ""
                Dim CardName As String = ""
                Dim ProjectCode As String = ""

                CreateDate = dr.Item("U_Cdate").ToString.Trim()
                CardCode = dr.Item("CardCode").ToString.Trim()
                CardName = dr.Item("CardName").ToString.Trim()
                ProjectCode = dr.Item("Project").ToString.Trim()

                If CreateDate = "" Then
                    CreateDate = "%"
                ElseIf CreateDate.Contains("*") = True Then
                    CreateDate = CreateDate.Replace("*", "%")
                Else
                    CreateDate = "%" & CreateDate & "%"
                End If

                If CardCode = "" Then
                    CardCode = "%"
                ElseIf CardCode.Contains("*") = True Then
                    CardCode = CardCode.Replace("*", "%")
                Else
                    CardCode = "%" & CardCode & "%"
                End If

                If CardName = "" Then
                    CardName = "%"
                ElseIf CardName.Contains("*") = True Then
                    CardName = CardName.Replace("*", "%")
                Else
                    CardName = "%" & CardName & "%"
                End If

                If ProjectCode = "" Then
                    ProjectCode = "%"
                ElseIf ProjectCode.Contains("*") = True Then
                    ProjectCode = ProjectCode.Replace("*", "%")
                Else
                    ProjectCode = "%" & ProjectCode & "%"
                End If

                Dim SQLStr As String = ""
                Dim sErrDesc As String = ""
                Dim DType As String = ""
                Dim CompCode As String = ""
                SQLStr = "SELECT T0.""U_DAuthor"" FROM ""@WUSER""  T0 WHERE Upper(T0.""Code"") ='" & UserCode & "'"
                DType = fn.ExecuteSQLQuery_SingleValue(SQLStr, sErrDesc)
                If sErrDesc <> "" Then
                    Throw New Exception(sErrDesc)
                ElseIf DType = "" Then
                    Throw New Exception("No Record Found!")
                End If

                Dim Query As String = ""
                Dim ErrMsg As String = ""
                Dim RetDT As New DataTable
                Dim RetDT1 As New DataTable
                Dim RetDT2 As New DataTable
                Dim RetDT3 As New DataTable
                Dim RetDT4 As New DataTable
                Dim RetDS = New DataSet()
                Dim DocEntry As String = ""

                'Dim Query As String = ""
                If DType = "By Company" Then
                    SQLStr = "SELECT ifnull(T0.""U_ComCode"",'')  As ""U_ComCode"" FROM ""@WUSER""  T0 WHERE T0.""Code"" ='" & UserCode & "'"
                    CompCode = fn.ExecuteSQLQuery_SingleValue(SQLStr, sErrDesc)
                    If sErrDesc <> "" Then
                        Throw New Exception(sErrDesc)
                    ElseIf CompCode = "" Then
                        Throw New Exception("No Record Found!")
                    End If
                    If DocNum = "" Then
                        Query = "SELECT T0.""DocEntry"" as ""U_OrderNo"",T0.""DocNum"" AS ""SODocNum"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", " & _
                                " T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY') As ""DocDate"", " & _
                                " T0.""CardCode"",T0.""CardName"",T0.""U_SurveyorID"",T0.""NumAtCard"",T0.""U_STypeCode"",T0.""U_U_STypeName"" As U_STypeName, T0.""U_Country"",  " & _
                                " T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"",T2.""U_HYPERLINK_BEFORE"" AS ""U_HLinkBef"",T2.""U_HYPERLINK_AFTER"" AS ""U_HLinkAft"" " & _
                                " FROM ORDR T0 " & _
                                " INNER JOIN RDR1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" " & _
                                " LEFT JOIN ""@SURVEYTYPE_HLINK"" T2 ON UPPER(T2.""U_SURVEYTYPENAME"") = UPPER(T1.""Dscription"") " & _
                                " where T0.""U_UCode"" in (SELECT T0.""Code"" FROM ""@WUSER""  T0 WHERE T0.""U_ComCode""  ='" & CompCode & "') " & _
                                " AND UPPER(IFNULL(T0.""CardCode"",'')) like '" & CardCode.ToUpper() & "' AND IFNULL(T0.""U_Cdate"",'') LIKE '" & CreateDate & "' AND UPPER(IFNULL(T0.""CardName"",'')) LIKE '" & CardName.ToUpper().Replace("'", "''") & "' " & _
                                " AND IFNULL(T0.""Project"",'') LIKE '" & ProjectCode & "' ORDER BY T0.""DocEntry"" ASC "
                    Else
                        'Query = "SELECT Top 1 T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"",  T0.""U_Ccode"", T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@CCON"" T0  where T0.""DocEntry"" =(SELECT (T0.""DocEntry"" - 1)  ""DocEntry"" FROM ""@CCON""  T0 WHERE T0.""DocNum"" ='" & DocNum & "')"
                        Query = "SELECT T0.""DocEntry"" as ""U_OrderNo"",T0.""DocNum"" AS ""SODocNum"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", " & _
                                " T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", TO_CHAR( T0.""DocDate"",'DD/MM/YYYY') As ""DocDate"", " & _
                                " T0.""CardCode"",T0.""CardName"", T0.""U_SurveyorID"", T0.""NumAtCard"",T0.""U_STypeCode"",T0.""U_U_STypeName"" As U_STypeName, T0.""U_Country"", " & _
                                " T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"",T2.""U_HYPERLINK_BEFORE"" AS ""U_HLinkBef"",T2.""U_HYPERLINK_AFTER"" AS ""U_HLinkAft"" " & _
                                " FROM ORDR T0 " & _
                                " INNER JOIN RDR1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" " & _
                                " LEFT JOIN ""@SURVEYTYPE_HLINK"" T2 ON UPPER(T2.""U_SURVEYTYPENAME"") = UPPER(T1.""Dscription"") " & _
                                " where T0.""DocEntry"" IN (SELECT ifnull(T0.""DocEntry"",0) ""DocEntry"" FROM ORDR T0 " & _
                                "                          where T0.""U_UCode"" in (SELECT T0.""Code"" FROM ""@WUSER"" T0 WHERE T0.""U_ComCode"" ='" & CompCode & "' ) " & _
                                "                          and  T0.""DocEntry"" LIKE '" & DocNum & "' ORDER BY T0.""DocEntry"" DESC) " & _
                                " AND UPPER(IFNULL(T0.""CardCode"",'')) like '" & CardCode.ToUpper() & "' AND IFNULL(T0.""U_Cdate"",'') LIKE '" & CreateDate & "' AND UPPER(IFNULL(T0.""CardName"",'')) LIKE '" & CardName.ToUpper().Replace("'", "''") & "' " & _
                                " AND IFNULL(T0.""Project"",'') LIKE '" & ProjectCode & "' ORDER BY T0.""DocEntry"" ASC "
                    End If
                ElseIf DType = "By Country" Then
                    If DocNum = "" Then
                        Query = "SELECT T0.""DocEntry"" as ""U_OrderNo"",T0.""DocNum"" AS ""SODocNum"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", " & _
                                " T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY') As ""DocDate"", " & _
                                " T0.""CardCode"",T0.""CardName"",T0.""U_SurveyorID"",T0.""NumAtCard"",T0.""U_STypeCode"",T0.""U_U_STypeName"" As U_STypeName, T0.""U_Country"",  " & _
                                " T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"",T2.""U_HYPERLINK_BEFORE"" AS ""U_HLinkBef"",T2.""U_HYPERLINK_AFTER"" AS ""U_HLinkAft"" " & _
                                " FROM ORDR T0 " & _
                                " INNER JOIN RDR1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" " & _
                                " LEFT JOIN ""@SURVEYTYPE_HLINK"" T2 ON UPPER(T2.""U_SURVEYTYPENAME"") = UPPER(T1.""Dscription"") " & _
                                " where T0.""U_UCode"" in (SELECT T0.""Code"" FROM ""@WUSER""  T0 WHERE T0.""U_ComCode""  ='" & CompCode & "') " & _
                                " AND T0.""CardCode"" IN (SELECT DISTINCT ""CardCode"" FROM ""CRD1"" WHERE ""Country"" IN (SELECT ""U_CCode"" FROM ""@COUNTRY"")) " & _
                                " AND UPPER(IFNULL(T0.""CardCode"",'')) like '" & CardCode.ToUpper() & "' AND IFNULL(T0.""U_Cdate"",'') LIKE '" & CreateDate & "' AND UPPER(IFNULL(T0.""CardName"",'')) LIKE '" & CardName.ToUpper().Replace("'", "''") & "' " & _
                                " AND IFNULL(T0.""Project"",'') LIKE '" & ProjectCode & "' ORDER BY T0.""DocEntry"" ASC "

                    Else
                        'Query = "SELECT Top 1 T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"",  T0.""U_Ccode"", T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@CCON"" T0  where T0.""DocEntry"" =(SELECT (T0.""DocEntry"" - 1)  ""DocEntry"" FROM ""@CCON""  T0 WHERE T0.""DocNum"" ='" & DocNum & "')"
                        Query = "SELECT T0.""DocEntry"" as ""U_OrderNo"",T0.""DocNum"" AS ""SODocNum"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", " & _
                                " T0.""U_UCode"",T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", TO_CHAR( T0.""DocDate"",'DD/MM/YYYY') As ""DocDate"", " & _
                                " T0.""CardCode"",T0.""CardName"", T0.""U_SurveyorID"", T0.""NumAtCard"",T0.""U_STypeCode"",T0.""U_U_STypeName"" As U_STypeName, T0.""U_Country"", " & _
                                " T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""Comments"",T2.""U_HYPERLINK_BEFORE"" AS ""U_HLinkBef"",T2.""U_HYPERLINK_AFTER"" AS ""U_HLinkAft"" " & _
                                " FROM ORDR T0 " & _
                                " INNER JOIN RDR1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" " & _
                                " LEFT JOIN ""@SURVEYTYPE_HLINK"" T2 ON UPPER(T2.""U_SURVEYTYPENAME"") = UPPER(T1.""Dscription"") " & _
                                " where T0.""DocEntry"" IN (SELECT ifnull(T0.""DocEntry"",0) ""DocEntry"" FROM ORDR T0 " & _
                                "                          where T0.""U_UCode"" in (SELECT T0.""Code"" FROM ""@WUSER"" T0 WHERE T0.""U_ComCode"" ='" & CompCode & "' ) " & _
                                "                          and  T0.""DocEntry"" LIKE '" & DocNum & "' ORDER BY T0.""DocEntry"" DESC) " & _
                                " AND T0.""CardCode"" IN (SELECT DISTINCT ""CardCode"" FROM ""CRD1"" WHERE ""Country"" IN (SELECT ""U_CCode"" FROM ""@COUNTRY"")) " & _
                                " AND UPPER(IFNULL(T0.""CardCode"",'')) like '" & CardCode.ToUpper() & "' AND IFNULL(T0.""U_Cdate"",'') LIKE '" & CreateDate & "' AND UPPER(IFNULL(T0.""CardName"",'')) LIKE '" & CardName.ToUpper().Replace("'", "''") & "' " & _
                                " AND IFNULL(T0.""Project"",'') LIKE '" & ProjectCode & "' ORDER BY T0.""DocEntry"" ASC "
                    End If
                Else
                    Throw New Exception("No Record Found!")
                End If

                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                If ErrMsg <> "" Then
                    Throw New Exception(ErrMsg)
                End If
                If RetDT.Rows.Count > 0 Then
                    DocEntry = RetDT.Rows(0)(0)
                    RetDT.TableName = "ORDR"
                    RetDT1 = RetDT.Copy
                    RetDS.Tables.Add(RetDT1)

                    'RetDT = New DataTable
                    'Query = "SELECT T1.""DocEntry"" ,T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", T1.""U_Rate"", T1.""U_UOM"", T1.""U_GST"", T1.""U_Remarks"" FROM ""@SQTOGENERAL""  T1 WHERE T1.""DocEntry"" = '" & DocEntry & "'"
                    'RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    'If ErrMsg <> "" Then
                    '    Throw New Exception(ErrMsg)
                    'End If
                    'RetDT.TableName = "SQTOGEN"
                    'RetDT2 = RetDT.Copy
                    'RetDS.Tables.Add(RetDT2)


                    'RetDT = New DataTable
                    'Query = "SELECT T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", T0.""U_Rate"", T0.""U_UOM"", T0.""U_GST"", T0.""U_Remarks"" FROM ""@SQTOADDON""  T0 WHERE T0.""DocEntry""  = '" & DocEntry & "'"
                    'RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
                    'If ErrMsg <> "" Then
                    '    Throw New Exception(ErrMsg)
                    'End If
                    'RetDT.TableName = "SQTOADD"
                    'RetDT3 = RetDT.Copy
                    'RetDS.Tables.Add(RetDT3)

                    Context.Response.Output.Write(fn.ds2json(RetDS))

                Else
                    Throw New Exception("No Record Found")
                End If
            Else
                Throw New Exception("No Record Found")
            End If
        Catch ex As Exception
            oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    '    <WebMethod()> _
    '<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    '    Public Sub SalesOrder_FindRecord_List(value As String)
    '        sFunction = "SalesOrder_FindRecord_List"
    '        Try
    '            oLog.WriteToLogFile_Debug(value, sFunction)
    '            Dim ds As DataSet = fn.jsontodata(value)
    '            Dim LEADM As New DataTable
    '            Dim DocNum As String = ""
    '            Dim UserCode As String = ""
    '            If ds.Tables("ORDR").Rows.Count > 0 Then
    '                LEADM = ds.Tables("ORDR")
    '                Dim dr As DataRow = LEADM.Rows(0)
    '                Try
    '                    DocNum = dr.Item("U_OrderNo").ToString
    '                Catch ex As Exception
    '                End Try

    '                UserCode = dr.Item("U_UCode").ToString.Trim().ToUpper

    '                Dim CreateDate As String = ""
    '                Dim CardCode As String = ""
    '                Dim CardName As String = ""
    '                Dim ProjectCode As String = ""

    '                CreateDate = dr.Item("U_Cdate").ToString.Trim()
    '                CardCode = dr.Item("U_Ccode").ToString.Trim()
    '                CardName = dr.Item("U_Cname").ToString.Trim()
    '                ProjectCode = dr.Item("U_Pcode").ToString.Trim()

    '                If CreateDate = "" Then
    '                    CreateDate = "%"
    '                ElseIf CreateDate.Contains("*") = True Then
    '                    CreateDate = CreateDate.Replace("*", "%")
    '                Else
    '                    CreateDate = "%" & CreateDate & "%"
    '                End If

    '                If CardCode = "" Then
    '                    CardCode = "%"
    '                ElseIf CardCode.Contains("*") = True Then
    '                    CardCode = CardCode.Replace("*", "%")
    '                Else
    '                    CardCode = "%" & CardCode & "%"
    '                End If

    '                If CardName = "" Then
    '                    CardName = "%"
    '                ElseIf CardName.Contains("*") = True Then
    '                    CardName = CardName.Replace("*", "%")
    '                Else
    '                    CardName = "%" & CardName & "%"
    '                End If

    '                If ProjectCode = "" Then
    '                    ProjectCode = "%"
    '                ElseIf ProjectCode.Contains("*") = True Then
    '                    ProjectCode = ProjectCode.Replace("*", "%")
    '                Else
    '                    ProjectCode = "%" & ProjectCode & "%"
    '                End If


    '                Dim Query As String = ""
    '                Dim ErrMsg As String = ""
    '                Dim RetDT As New DataTable
    '                Dim RetDT1 As New DataTable
    '                Dim RetDT2 As New DataTable
    '                Dim RetDT3 As New DataTable
    '                Dim RetDT4 As New DataTable
    '                Dim RetDS = New DataSet()
    '                Dim DocEntry As String = ""

    '                If DocNum = "" Then
    '                    Query = "SELECT T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"", T0.""U_Ccode"", T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@CCON"" T0 where T0.""U_Ccode"" like '" & CardCode & "' and  T0.""U_Cdate"" like '" & CreateDate & "'  and  T0.""U_Cname"" like '" & CardName & "'   and  T0.""U_Pcode"" like '" & ProjectCode & "'    ORDER BY T0.""DocEntry"" DESC"
    '                Else
    '                    Query = "SELECT Top 1 T0.""DocEntry"" As ""U_Qno"", T0.""U_Status"", T0.""U_Uname"",TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY') As ""U_Cdate"",T0.""U_Ccode"", T0.""U_Cname"", TO_CHAR( T0.""U_CPeriod1"" , 'DD/MM/YYYY') As ""U_CPeriod1"", TO_CHAR( T0.""U_CPeriod2"" , 'DD/MM/YYYY') As ""U_CPeriod2"", T0.""U_Pcode"", T0.""U_AddrN"", T0.""U_Addr1"", T0.""U_Addr2"", T0.""U_Addr3"", T0.""U_Addr4"", T0.""U_Addr5"", T0.""U_Addr6"", T0.""U_TelNo"", T0.""U_FaxNo"", T0.""U_Mno"", T0.""U_Email"", T0.""U_Remarks"" FROM ""@CCON"" T0  where T0.""DocEntry"" ='" & DocNum & "'"
    '                End If

    '                RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
    '                If ErrMsg <> "" Then
    '                    Throw New Exception(ErrMsg)
    '                End If
    '                If RetDT.Rows.Count > 0 Then
    '                    DocEntry = RetDT.Rows(0)(0)
    '                    RetDT.TableName = "CCON"
    '                    RetDT1 = RetDT.Copy
    '                    RetDS.Tables.Add(RetDT1)

    '                    'RetDT = New DataTable
    '                    'Query = "SELECT T1.""DocEntry"" ,T1.""U_Stype"", T1.""U_Conti"", T1.""U_Country"", T1.""U_City"", T1.""U_Currency"", T1.""U_EQGroup"", T1.""U_Rate"", T1.""U_UOM"", T1.""U_GST"", T1.""U_Remarks"" FROM ""@CCONGENERAL""  T1 WHERE T1.""DocEntry"" = '" & DocEntry & "'"
    '                    'RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
    '                    'If ErrMsg <> "" Then
    '                    '    Throw New Exception(ErrMsg)
    '                    'End If
    '                    'RetDT.TableName = "CCONGEN"
    '                    'RetDT2 = RetDT.Copy
    '                    'RetDS.Tables.Add(RetDT2)


    '                    'RetDT = New DataTable
    '                    'Query = "SELECT T0.""U_Ctype"", T0.""U_Continent"", T0.""U_Country"", T0.""U_City"", T0.""U_EQGroup"", T0.""U_Currency"", T0.""U_Rate"", T0.""U_UOM"", T0.""U_GST"", T0.""U_Remarks"" FROM ""@CCONADDON""  T0 WHERE T0.""DocEntry""  = '" & DocEntry & "'"
    '                    'RetDT = fn.ExecuteSQLQuery(Query, ErrMsg)
    '                    'If ErrMsg <> "" Then
    '                    '    Throw New Exception(ErrMsg)
    '                    'End If
    '                    'RetDT.TableName = "CCONADD"
    '                    'RetDT3 = RetDT.Copy
    '                    'RetDS.Tables.Add(RetDT3)

    '                    Context.Response.Output.Write(fn.ds2json(RetDS))

    '                Else
    '                    Throw New Exception("No Record Found")
    '                End If
    '            Else
    '                Throw New Exception("No Record Found")
    '            End If
    '        Catch ex As Exception
    '            oLog.WriteToLogFile(ex.Message, sFunction)
    '            fn.ErrorHandling(ex.Message.ToString.Trim())
    '            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
    '        End Try
    '    End Sub
#End Region
#Region "Survey Tyep"

    <WebMethod()> _
         <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub AnyOther_SurveyTyepAdd(ByVal value As String)
        sFunction = "AnyOther_SurveyTyepAdd"
        Try

            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim SQTO As New DataTable
            SQTO = New DataTable
            Dim SQTOGEN As New DataTable
            Dim SQTOADD As New DataTable
            Dim SOTOATTACH As New DataTable
            Dim SQTODS = New DataSet()
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            fn.GetSAPConnection(Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            Dim STypeCode As String = ""
            Dim STypeName As String = ""
            Dim RetCode As Integer = 0
            Dim SONo As String = ""
            Dim errMessage As String = ""
            Dim CardCode As String = ""
            Dim DocDate As String = ""
            Dim ProjectCode As String = ""
            Dim Country As String = ""
            Dim City As String = ""
            Dim EqupTye As String = ""
            Dim Rt As String = ""
            Dim SQLStr As String = ""

            Dim oSO As SAPbobsCOM.Documents
            oSO = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
            Dim oDO As SAPbobsCOM.Documents
            oDO = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes)


            If ds.Tables("ODLN").Rows.Count > 0 Then
                SQTO = ds.Tables("ODLN")
                Dim dr As DataRow = SQTO.Rows(0)

                If oSO.GetByKey(dr.Item("U_OrderNo")) = True Then

                Else
                    Throw New Exception("No Record Found!")
                End If

                oDO.UserFields.Fields.Item("U_UCode").Value = dr.Item("U_UCode").ToString.Trim()
                oDO.UserFields.Fields.Item("U_UName").Value = dr.Item("U_UName").ToString.Trim()
                oDO.UserFields.Fields.Item("U_Cdate").Value = dr.Item("U_Cdate")
                oDO.DocDate = DateConvert(dr.Item("DocDate").ToString)
                oDO.DocDueDate = DateConvert(dr.Item("DocDate").ToString)
                oDO.CardCode = oSO.CardCode
                oDO.CardName = oSO.CardName

                oDO.UserFields.Fields.Item("U_FormName").Value = dr.Item("U_FormName").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SResult").Value = dr.Item("U_SResult").ToString.Trim()

                oDO.UserFields.Fields.Item("U_SurveyorID").Value = oSO.UserFields.Fields.Item("U_SurveyorID").Value
                oDO.NumAtCard = oSO.NumAtCard 'dr.Item("NumAtCard").ToString.Trim
                oDO.UserFields.Fields.Item("U_SuvExAgent").Value = dr.Item("U_SuvExAgent").ToString.Trim()
                oDO.UserFields.Fields.Item("U_NoPh").Value = dr.Item("U_NoPh").ToString.Trim()
                oDO.UserFields.Fields.Item("U_EqNo").Value = dr.Item("U_EqNo").ToString.Trim()

                '  STypeCode = dr.Item("U_STypeCode")
                ' STypeName = dr.Item("U_STypeName")
                oDO.UserFields.Fields.Item("U_STypeCode").Value = oSO.UserFields.Fields.Item("U_STypeCode").Value
                oDO.UserFields.Fields.Item("U_U_STypeName").Value = oSO.UserFields.Fields.Item("U_U_STypeName").Value
                oDO.UserFields.Fields.Item("U_Country").Value = oSO.UserFields.Fields.Item("U_Country").Value
                oDO.UserFields.Fields.Item("U_City").Value = oSO.UserFields.Fields.Item("U_City").Value
                oDO.UserFields.Fields.Item("U_Loc").Value = oSO.UserFields.Fields.Item("U_Loc").Value
                oDO.Comments = oSO.Comments
                oDO.Lines.ItemCode = oSO.Lines.ItemCode
                oDO.Lines.Quantity = 1
                oDO.Lines.UserFields.Fields.Item("U_PDate").Value = oSO.Lines.UserFields.Fields.Item("U_PDate").Value
                oDO.Lines.UserFields.Fields.Item("U_EQType").Value = oSO.Lines.UserFields.Fields.Item("U_EQType").Value
                'EqupTye = dr1.Item("U_EQType").ToString.Trim()
                oDO.Lines.UserFields.Fields.Item("U_SCriteria").Value = oSO.Lines.UserFields.Fields.Item("U_SCriteria").Value
                oDO.Lines.BaseType = 17 '23 ' - 'Sales Quotation'
                oDO.Lines.BaseEntry = dr.Item("U_OrderNo")
                oDO.Lines.BaseLine = 0

                oDO.Lines.UnitPrice = oSO.Lines.UnitPrice
                oDO.Lines.Currency = oSO.Lines.Currency
                oDO.Lines.Add()

                Dim oAttachEntry As Integer = 0
                Dim bAttachAdd As Boolean = False
                If ds.Tables("ATTACHMENT").Rows.Count > 0 Then
                    SOTOATTACH = ds.Tables("ATTACHMENT")
                    Dim oAttach As SAPbobsCOM.Attachments2
                    oAttach = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2)

                    For Each odr As DataRow In SOTOATTACH.Rows

                        Dim sSourcePath, sFileName, sFileExt As String
                        sFileName = odr.Item("U_FileName").ToString.Trim()
                        If sFileName <> "" Then
                            sFileExt = Path.GetExtension(sFileName)
                            sFileExt = sFileExt.Replace(".", "")
                            sSourcePath = odr.Item("U_FilePath").ToString.Substring(0, 3)
                            oAttach.Lines.SourcePath = PublicVariable.sTempfilePath
                            oAttach.Lines.FileName = sFileName.Replace(Path.GetExtension(sFileName), "")
                            oAttach.Lines.FileExtension = sFileExt
                            oAttach.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES
                            oAttach.Lines.Add()
                            bAttachAdd = True
                        End If
                    Next
                    If bAttachAdd = True Then
                        If oAttach.Add() = 0 Then
                            oAttachEntry = PublicVariable.oCompany.GetNewObjectKey()
                            oDO.AttachmentEntry = oAttachEntry
                        Else
                            Throw New Exception("Error while adding attachment /" & PublicVariable.oCompany.GetLastErrorDescription)
                        End If
                    End If
                End If

                RetCode = oDO.Add
                If RetCode <> 0 Then
                    PublicVariable.oCompany.GetLastError(RetCode, errMessage)
                    Throw New Exception(errMessage)
                Else
                    SONo = PublicVariable.oCompany.GetNewObjectKey()
                End If
            Else
                Throw New Exception("No Input Data.")
            End If



            Dim RetDT As New DataTable
            RetDT.TableName = "VALIDATE"
            RetDT.Columns.Add("Status")
            RetDT.Columns.Add("Msg")
            RetDT.Rows.Add()
            RetDT.Rows(0)(0) = True
            RetDT.Rows(0)(1) = "Survey no. " & SONo.ToString & " Created Successfully"
            RetDS.Tables.Add(RetDT)
            Context.Response.Output.Write(fn.ds2json(RetDS))


        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
     <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub OnHire_SurveyTyepAdd(ByVal value As String)
        sFunction = "OnHire_SurveyTyepAdd"
        Try

            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim SQTO As New DataTable
            SQTO = New DataTable
            Dim SQTOGEN As New DataTable
            Dim SQTOADD As New DataTable
            Dim SOTOATTACH As New DataTable
            Dim SQTODS = New DataSet()
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            fn.GetSAPConnection(Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            Dim STypeCode As String = ""
            Dim STypeName As String = ""
            Dim RetCode As Integer = 0
            Dim SONo As String = ""
            Dim errMessage As String = ""
            Dim CardCode As String = ""
            Dim DocDate As String = ""
            Dim ProjectCode As String = ""
            Dim Country As String = ""
            Dim City As String = ""
            Dim EqupTye As String = ""
            Dim Rt As String = ""
            Dim SQLStr As String = ""

            Dim oSO As SAPbobsCOM.Documents
            oSO = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
            Dim oDO As SAPbobsCOM.Documents
            oDO = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes)


            If ds.Tables("ODLN").Rows.Count > 0 Then
                SQTO = ds.Tables("ODLN")
                Dim dr As DataRow = SQTO.Rows(0)

                If oSO.GetByKey(dr.Item("U_OrderNo")) = True Then

                Else
                    Throw New Exception("No Record Found!")
                End If

                oDO.UserFields.Fields.Item("U_UCode").Value = dr.Item("U_UCode").ToString.Trim()
                oDO.UserFields.Fields.Item("U_UName").Value = dr.Item("U_UName").ToString.Trim()
                oDO.UserFields.Fields.Item("U_Cdate").Value = dr.Item("U_Cdate")
                oDO.DocDate = DateConvert(dr.Item("DocDate").ToString)
                oDO.DocDueDate = DateConvert(dr.Item("DocDate").ToString)
                oDO.CardCode = oSO.CardCode
                oDO.CardName = oSO.CardName

                oDO.UserFields.Fields.Item("U_FormName").Value = dr.Item("U_FormName").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SResult").Value = dr.Item("U_SResult").ToString.Trim()
                Try
                    oDO.UserFields.Fields.Item("U_DOM").Value = DateConvert(dr.Item("U_DOM").ToString.Trim())
                Catch ex As Exception

                End Try
                Try
                    oDO.UserFields.Fields.Item("U_MGW").Value = dr.Item("U_MGW").ToString.Trim()
                Catch ex As Exception

                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Tare").Value = dr.Item("U_Tare").ToString.Trim()
                Catch ex As Exception

                End Try
                Try
                    oDO.UserFields.Fields.Item("U_ACEP").Value = dr.Item("U_ACEP").ToString.Trim()
                Catch ex As Exception

                End Try
                Try
                    oDO.UserFields.Fields.Item("U_CSC").Value = DateConvert(dr.Item("U_CSC").ToString.Trim())
                Catch ex As Exception

                End Try

                oDO.UserFields.Fields.Item("U_SurveyorID").Value = oSO.UserFields.Fields.Item("U_SurveyorID").Value
                oDO.NumAtCard = oSO.NumAtCard 'dr.Item("NumAtCard").ToString.Trim
                oDO.UserFields.Fields.Item("U_SuvExAgent").Value = dr.Item("U_SuvExAgent").ToString.Trim()
                oDO.UserFields.Fields.Item("U_NoPh").Value = dr.Item("U_NoPh").ToString.Trim()
                oDO.UserFields.Fields.Item("U_EqNo").Value = dr.Item("U_EqNo").ToString.Trim()

                '  STypeCode = dr.Item("U_STypeCode")
                ' STypeName = dr.Item("U_STypeName")
                oDO.UserFields.Fields.Item("U_STypeCode").Value = oSO.UserFields.Fields.Item("U_STypeCode").Value
                oDO.UserFields.Fields.Item("U_U_STypeName").Value = oSO.UserFields.Fields.Item("U_U_STypeName").Value
                oDO.UserFields.Fields.Item("U_Country").Value = oSO.UserFields.Fields.Item("U_Country").Value
                oDO.UserFields.Fields.Item("U_City").Value = oSO.UserFields.Fields.Item("U_City").Value
                oDO.UserFields.Fields.Item("U_Loc").Value = oSO.UserFields.Fields.Item("U_Loc").Value

                oDO.Comments = oSO.Comments
                oDO.Lines.ItemCode = oSO.Lines.ItemCode
                oDO.Lines.Quantity = 1
                oDO.Lines.UserFields.Fields.Item("U_PDate").Value = oSO.Lines.UserFields.Fields.Item("U_PDate").Value
                oDO.Lines.UserFields.Fields.Item("U_EQType").Value = oSO.Lines.UserFields.Fields.Item("U_EQType").Value
                'EqupTye = dr1.Item("U_EQType").ToString.Trim()
                oDO.Lines.UserFields.Fields.Item("U_SCriteria").Value = oSO.Lines.UserFields.Fields.Item("U_SCriteria").Value
                oDO.Lines.BaseType = 17 '23 ' - 'Sales Quotation'
                oDO.Lines.BaseEntry = dr.Item("U_OrderNo")
                oDO.Lines.BaseLine = 0

                oDO.Lines.UnitPrice = oSO.Lines.UnitPrice
                oDO.Lines.Currency = oSO.Lines.Currency
                oDO.Lines.Add()

                Dim oAttachEntry As Integer = 0
                Dim bAttachAdd As Boolean = False
                If ds.Tables("ATTACHMENT").Rows.Count > 0 Then
                    SOTOATTACH = ds.Tables("ATTACHMENT")
                    Dim oAttach As SAPbobsCOM.Attachments2
                    oAttach = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2)

                    For Each odr As DataRow In SOTOATTACH.Rows

                        Dim sSourcePath, sFileName, sFileExt As String
                        sFileName = odr.Item("U_FileName").ToString.Trim()
                        If sFileName <> "" Then
                            sFileExt = Path.GetExtension(sFileName)
                            sFileExt = sFileExt.Replace(".", "")
                            sSourcePath = odr.Item("U_FilePath").ToString.Substring(0, 3)
                            oAttach.Lines.SourcePath = PublicVariable.sTempfilePath
                            oAttach.Lines.FileName = sFileName.Replace(Path.GetExtension(sFileName), "")
                            oAttach.Lines.FileExtension = sFileExt
                            oAttach.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES
                            oAttach.Lines.Add()
                            bAttachAdd = True
                        End If
                    Next
                    If bAttachAdd = True Then
                        If oAttach.Add() = 0 Then
                            oAttachEntry = PublicVariable.oCompany.GetNewObjectKey()
                            oDO.AttachmentEntry = oAttachEntry
                        Else
                            Throw New Exception("Error while adding attachment /" & PublicVariable.oCompany.GetLastErrorDescription)
                        End If
                    End If
                End If

                RetCode = oDO.Add
                If RetCode <> 0 Then
                    PublicVariable.oCompany.GetLastError(RetCode, errMessage)
                    Throw New Exception(errMessage)
                Else
                    SONo = PublicVariable.oCompany.GetNewObjectKey()
                End If
            Else
                Throw New Exception("No Input Data.")
            End If

            Dim RetDT As New DataTable
            RetDT.TableName = "VALIDATE"
            RetDT.Columns.Add("Status")
            RetDT.Columns.Add("Msg")
            RetDT.Rows.Add()
            RetDT.Rows(0)(0) = True
            RetDT.Rows(0)(1) = "Survey no. " & SONo.ToString & " Created Successfully"
            RetDS.Tables.Add(RetDT)
            Context.Response.Output.Write(fn.ds2json(RetDS))


        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
         <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub OffHire_SurveyTyepAdd(ByVal value As String)
        sFunction = "OffHire_SurveyTyepAdd"
        Try

            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim SQTO As New DataTable
            SQTO = New DataTable
            Dim SQTOGEN As New DataTable
            Dim SQTOADD As New DataTable
            Dim SOTOATTACH As New DataTable
            Dim SQTODS = New DataSet()
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            fn.GetSAPConnection(Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            Dim STypeCode As String = ""
            Dim STypeName As String = ""
            Dim RetCode As Integer = 0
            Dim SONo As String = ""
            Dim errMessage As String = ""
            Dim CardCode As String = ""
            Dim DocDate As String = ""
            Dim ProjectCode As String = ""
            Dim Country As String = ""
            Dim City As String = ""
            Dim EqupTye As String = ""
            Dim Rt As String = ""
            Dim SQLStr As String = ""

            Dim oSO As SAPbobsCOM.Documents
            oSO = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
            Dim oDO As SAPbobsCOM.Documents
            oDO = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes)


            If ds.Tables("ODLN").Rows.Count > 0 Then
                SQTO = ds.Tables("ODLN")
                Dim dr As DataRow = SQTO.Rows(0)

                If oSO.GetByKey(dr.Item("U_OrderNo")) = True Then

                Else
                    Throw New Exception("No Record Found!")
                End If

                oDO.UserFields.Fields.Item("U_UCode").Value = dr.Item("U_UCode").ToString.Trim()
                oDO.UserFields.Fields.Item("U_UName").Value = dr.Item("U_UName").ToString.Trim()
                oDO.UserFields.Fields.Item("U_Cdate").Value = dr.Item("U_Cdate")
                oDO.DocDate = DateConvert(dr.Item("DocDate").ToString)
                oDO.DocDueDate = DateConvert(dr.Item("DocDate").ToString)
                oDO.CardCode = oSO.CardCode
                oDO.CardName = oSO.CardName

                Try
                    oDO.UserFields.Fields.Item("U_Esti_Org").Value = dr.Item("U_Esti_Org").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Esti_Rev").Value = dr.Item("U_Esti_Rev").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Esti_Diff").Value = dr.Item("U_Esti_Diff").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Curr_Org").Value = dr.Item("U_Curr_Org").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Curr_Rev").Value = dr.Item("U_Curr_Rev").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Curr_diff").Value = dr.Item("U_Curr_diff").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Impac_Org").Value = dr.Item("U_Impac_Org").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Impac_Rev").Value = dr.Item("U_Impac_Rev").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Impac_diff").Value = dr.Item("U_Impac_diff").ToString.Trim()
                Catch ex As Exception

                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Wear_org").Value = dr.Item("U_Wear_org").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Wear_Rev").Value = dr.Item("U_Wear_Rev").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Wear_Diff").Value = dr.Item("U_Wear_Diff").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Cust_Org").Value = dr.Item("U_Cust_Org").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Cust_Rev").Value = dr.Item("U_Cust_Rev").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Cust_Diff").Value = dr.Item("U_Cust_Diff").ToString.Trim()
                Catch ex As Exception

                End Try

                Try
                    oDO.UserFields.Fields.Item("U_Impor_Org").Value = dr.Item("U_Impor_Org").ToString.Trim()
                Catch ex As Exception

                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Impor_Rev").Value = dr.Item("U_Impor_Rev").ToString.Trim()
                Catch ex As Exception

                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Import_Diff").Value = dr.Item("U_Import_Diff").ToString.Trim()
                Catch ex As Exception
                End Try

                Try
                    oDO.UserFields.Fields.Item("U_Sum_Org").Value = dr.Item("U_Sum_Org").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Sum_Rev").Value = dr.Item("U_Sum_Rev").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Sum_Diff").Value = dr.Item("U_Sum_Diff").ToString.Trim()
                Catch ex As Exception
                End Try

                oDO.UserFields.Fields.Item("U_FormName").Value = dr.Item("U_FormName").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SResult").Value = dr.Item("U_SResult").ToString.Trim()

                oDO.UserFields.Fields.Item("U_SurveyorID").Value = oSO.UserFields.Fields.Item("U_SurveyorID").Value
                oDO.NumAtCard = oSO.NumAtCard 'dr.Item("NumAtCard").ToString.Trim
                oDO.UserFields.Fields.Item("U_SuvExAgent").Value = dr.Item("U_SuvExAgent").ToString.Trim()
                oDO.UserFields.Fields.Item("U_NoPh").Value = dr.Item("U_NoPh").ToString.Trim()
                oDO.UserFields.Fields.Item("U_EqNo").Value = dr.Item("U_EqNo").ToString.Trim()

                '  STypeCode = dr.Item("U_STypeCode")
                ' STypeName = dr.Item("U_STypeName")
                oDO.UserFields.Fields.Item("U_STypeCode").Value = oSO.UserFields.Fields.Item("U_STypeCode").Value
                oDO.UserFields.Fields.Item("U_U_STypeName").Value = oSO.UserFields.Fields.Item("U_U_STypeName").Value
                oDO.UserFields.Fields.Item("U_Country").Value = oSO.UserFields.Fields.Item("U_Country").Value
                oDO.UserFields.Fields.Item("U_City").Value = oSO.UserFields.Fields.Item("U_City").Value
                oDO.UserFields.Fields.Item("U_Loc").Value = oSO.UserFields.Fields.Item("U_Loc").Value
                oDO.Comments = oSO.Comments
                oDO.Lines.ItemCode = oSO.Lines.ItemCode
                oDO.Lines.Quantity = 1
                oDO.Lines.UserFields.Fields.Item("U_PDate").Value = oSO.Lines.UserFields.Fields.Item("U_PDate").Value
                oDO.Lines.UserFields.Fields.Item("U_EQType").Value = oSO.Lines.UserFields.Fields.Item("U_EQType").Value
                'EqupTye = dr1.Item("U_EQType").ToString.Trim()
                oDO.Lines.UserFields.Fields.Item("U_SCriteria").Value = oSO.Lines.UserFields.Fields.Item("U_SCriteria").Value
                oDO.Lines.BaseType = 17 '23 ' - 'Sales Quotation'
                oDO.Lines.BaseEntry = dr.Item("U_OrderNo")
                oDO.Lines.BaseLine = 0

                oDO.Lines.UnitPrice = oSO.Lines.UnitPrice
                oDO.Lines.Currency = oSO.Lines.Currency
                oDO.Lines.Add()

                Dim oAttachEntry As Integer = 0
                Dim bAttachAdd As Boolean = False
                If ds.Tables("ATTACHMENT").Rows.Count > 0 Then
                    SOTOATTACH = ds.Tables("ATTACHMENT")
                    Dim oAttach As SAPbobsCOM.Attachments2
                    oAttach = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2)

                    For Each odr As DataRow In SOTOATTACH.Rows

                        Dim sSourcePath, sFileName, sFileExt As String
                        sFileName = odr.Item("U_FileName").ToString.Trim()
                        If sFileName <> "" Then
                            sFileExt = Path.GetExtension(sFileName)
                            sFileExt = sFileExt.Replace(".", "")
                            sSourcePath = odr.Item("U_FilePath").ToString.Substring(0, 3)
                            oAttach.Lines.SourcePath = PublicVariable.sTempfilePath
                            oAttach.Lines.FileName = sFileName.Replace(Path.GetExtension(sFileName), "")
                            oAttach.Lines.FileExtension = sFileExt
                            oAttach.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES
                            oAttach.Lines.Add()
                            bAttachAdd = True
                        End If
                    Next
                    If bAttachAdd = True Then
                        If oAttach.Add() = 0 Then
                            oAttachEntry = PublicVariable.oCompany.GetNewObjectKey()
                            oDO.AttachmentEntry = oAttachEntry
                        Else
                            Throw New Exception("Error while adding attachment /" & PublicVariable.oCompany.GetLastErrorDescription)
                        End If
                    End If
                End If

                RetCode = oDO.Add
                If RetCode <> 0 Then
                    PublicVariable.oCompany.GetLastError(RetCode, errMessage)
                    Throw New Exception(errMessage)
                Else
                    SONo = PublicVariable.oCompany.GetNewObjectKey()
                End If
            Else
                Throw New Exception("No Input Data.")
            End If

            Dim RetDT As New DataTable
            RetDT.TableName = "VALIDATE"
            RetDT.Columns.Add("Status")
            RetDT.Columns.Add("Msg")
            RetDT.Rows.Add()
            RetDT.Rows(0)(0) = True
            RetDT.Rows(0)(1) = "Survey no. " & SONo.ToString & " Created Successfully"
            RetDS.Tables.Add(RetDT)
            Context.Response.Output.Write(fn.ds2json(RetDS))


        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
         <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub Cleaning1_SurveyTyepAdd(ByVal value As String)
        sFunction = "Cleaning1_SurveyTyepAdd"
        Try

            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim SQTO As New DataTable
            SQTO = New DataTable
            Dim SQTOGEN As New DataTable
            Dim SQTOADD As New DataTable
            Dim SOTOATTACH As New DataTable
            Dim SQTODS = New DataSet()
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            fn.GetSAPConnection(Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            Dim STypeCode As String = ""
            Dim STypeName As String = ""
            Dim RetCode As Integer = 0
            Dim SONo As String = ""
            Dim errMessage As String = ""
            Dim CardCode As String = ""
            Dim DocDate As String = ""
            Dim ProjectCode As String = ""
            Dim Country As String = ""
            Dim City As String = ""
            Dim EqupTye As String = ""
            Dim Rt As String = ""
            Dim SQLStr As String = ""

            Dim oSO As SAPbobsCOM.Documents
            oSO = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
            Dim oDO As SAPbobsCOM.Documents
            oDO = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes)


            If ds.Tables("ODLN").Rows.Count > 0 Then
                SQTO = ds.Tables("ODLN")
                Dim dr As DataRow = SQTO.Rows(0)

                If oSO.GetByKey(dr.Item("U_OrderNo")) = True Then

                Else
                    Throw New Exception("No Record Found!")
                End If

                oDO.UserFields.Fields.Item("U_UCode").Value = dr.Item("U_UCode").ToString.Trim()
                oDO.UserFields.Fields.Item("U_UName").Value = dr.Item("U_UName").ToString.Trim()
                oDO.UserFields.Fields.Item("U_Cdate").Value = GetDateTimeValue(dr.Item("U_Cdate").ToString)
                oDO.DocDate = DateConvert(dr.Item("DocDate").ToString)
                oDO.DocDueDate = DateConvert(dr.Item("DocDate").ToString)
                oDO.CardCode = oSO.CardCode
                oDO.CardName = oSO.CardName

                oDO.UserFields.Fields.Item("U_FormName").Value = dr.Item("U_FormName").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SResult").Value = dr.Item("U_SResult").ToString.Trim()

                oDO.UserFields.Fields.Item("U_EX_Fram").Value = dr.Item("U_EX_Fram").ToString.Trim()
                oDO.UserFields.Fields.Item("U_EX_Man").Value = dr.Item("U_EX_Man").ToString.Trim()
                oDO.UserFields.Fields.Item("U_EX_Ser").Value = dr.Item("U_EX_Ser").ToString.Trim()
                oDO.UserFields.Fields.Item("U_EX_Car").Value = dr.Item("U_EX_Car").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Free").Value = dr.Item("U_INT_Free").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Clean").Value = dr.Item("U_INT_Clean").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Dry").Value = dr.Item("U_INT_Dry").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Pitt").Value = dr.Item("U_INT_Pitt").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Disc").Value = dr.Item("U_INT_Disc").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Val").Value = dr.Item("U_VAL_Val").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Bott").Value = dr.Item("U_VAL_Bott").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Man").Value = dr.Item("U_VAL_Man").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Syp").Value = dr.Item("U_VAL_Syp").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Tank").Value = dr.Item("U_VAL_Tank").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Avail").Value = dr.Item("U_VAL_Avail").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Steam").Value = dr.Item("U_VAL_Steam").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Gas").Value = dr.Item("U_VAL_Gas").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SEAL_MAN").Value = dr.Item("U_SEAL_MAN").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SEAL_AIR").Value = dr.Item("U_SEAL_AIR").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SEAL_BOTT").Value = dr.Item("U_SEAL_BOTT").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SEAL_LAST").Value = dr.Item("U_SEAL_LAST").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SEAL_NEXT").Value = dr.Item("U_SEAL_NEXT").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_SPIL").Value = dr.Item("U_VAL_SPIL").ToString.Trim()
                oDO.UserFields.Fields.Item("U_PayLoad").Value = dr.Item("U_PayLoad").ToString.Trim()
                oDO.UserFields.Fields.Item("U_Capacity").Value = dr.Item("U_Capacity").ToString.Trim()
                oDO.UserFields.Fields.Item("U_CSCNum").Value = dr.Item("U_CSCNum").ToString.Trim()
                oDO.UserFields.Fields.Item("U_IMO").Value = dr.Item("U_IMO").ToString.Trim()
                Try
                    'oDO.UserFields.Fields.Item("U_DOM").Value = DateConvert(dr.Item("U_DOM").ToString.Trim())
                    oDO.UserFields.Fields.Item("U_DOM").Value = (dr.Item("U_DOM").ToString.Trim())
                Catch ex As Exception
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile("Error while Adding DOM", sFunction)
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_MGW").Value = dr.Item("U_MGW").ToString.Trim()
                Catch ex As Exception
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile("Error while Adding MGW", sFunction)
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Tare").Value = dr.Item("U_Tare").ToString.Trim()
                Catch ex As Exception
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile("Error while adding Tare", sFunction)
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_ACEP").Value = dr.Item("U_ACEP").ToString.Trim()
                Catch ex As Exception
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile("Error while adding ACEP", sFunction)
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
                End Try
                Try
                    'oDO.UserFields.Fields.Item("U_CSC").Value = DateConvert(dr.Item("U_CSC").ToString.Trim())
                    oDO.UserFields.Fields.Item("U_CSC").Value = (dr.Item("U_CSC").ToString.Trim())
                Catch ex As Exception
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile("Error while Adding CSC", sFunction)
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
                End Try

                oDO.UserFields.Fields.Item("U_SurveyorID").Value = oSO.UserFields.Fields.Item("U_SurveyorID").Value
                oDO.NumAtCard = dr.Item("NumAtCard").ToString.Trim
                oDO.UserFields.Fields.Item("U_SuvExAgent").Value = dr.Item("U_SuvExAgent").ToString.Trim()
                oDO.UserFields.Fields.Item("U_NoPh").Value = dr.Item("U_NoPh").ToString.Trim()
                oDO.UserFields.Fields.Item("U_EqNo").Value = dr.Item("U_EqNo").ToString.Trim()

                '  STypeCode = dr.Item("U_STypeCode")
                ' STypeName = dr.Item("U_STypeName")
                oDO.UserFields.Fields.Item("U_STypeCode").Value = oSO.UserFields.Fields.Item("U_STypeCode").Value
                oDO.UserFields.Fields.Item("U_U_STypeName").Value = oSO.UserFields.Fields.Item("U_U_STypeName").Value
                oDO.UserFields.Fields.Item("U_Country").Value = oSO.UserFields.Fields.Item("U_Country").Value
                oDO.UserFields.Fields.Item("U_City").Value = oSO.UserFields.Fields.Item("U_City").Value
                oDO.UserFields.Fields.Item("U_Loc").Value = oSO.UserFields.Fields.Item("U_Loc").Value
                oDO.Comments = dr.Item("Comments").ToString.Trim()

                For i As Integer = 0 To oSO.Lines.Count
                    If i > 0 Then
                        oDO.Lines.Add()
                    End If
                    oDO.Lines.ItemCode = oSO.Lines.ItemCode
                    oDO.Lines.Quantity = 1
                    oDO.Lines.UserFields.Fields.Item("U_PDate").Value = oSO.Lines.UserFields.Fields.Item("U_PDate").Value
                    oDO.Lines.UserFields.Fields.Item("U_EQType").Value = oSO.Lines.UserFields.Fields.Item("U_EQType").Value
                    oDO.Lines.UserFields.Fields.Item("U_SCriteria").Value = oSO.Lines.UserFields.Fields.Item("U_SCriteria").Value
                    oDO.Lines.UserFields.Fields.Item("U_ReSurvey").Value = oSO.Lines.UserFields.Fields.Item("U_ReSurvey").Value
                    oDO.Lines.BaseType = 17
                    oDO.Lines.BaseEntry = dr.Item("U_OrderNo")
                    oDO.Lines.BaseLine = oSO.Lines.LineNum
                    oDO.Lines.UnitPrice = oSO.Lines.UnitPrice
                    oDO.Lines.Currency = oSO.Lines.Currency
                Next

                Dim oAttachEntry As Integer = 0
                Dim bAttachAdd As Boolean = False
                If ds.Tables("ATTACHMENT").Rows.Count > 0 Then
                    SOTOATTACH = ds.Tables("ATTACHMENT")
                    Dim oAttach As SAPbobsCOM.Attachments2
                    oAttach = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2)

                    For Each odr As DataRow In SOTOATTACH.Rows

                        Dim sSourcePath, sFileName, sFileExt As String
                        sFileName = odr.Item("U_FileName").ToString.Trim()
                        If sFileName <> "" Then
                            sFileExt = Path.GetExtension(sFileName)
                            sFileExt = sFileExt.Replace(".", "")
                            sSourcePath = odr.Item("U_FilePath").ToString.Substring(0, 3)
                            oAttach.Lines.SourcePath = PublicVariable.sTempfilePath
                            oAttach.Lines.FileName = sFileName.Replace(Path.GetExtension(sFileName), "")
                            oAttach.Lines.FileExtension = sFileExt
                            oAttach.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES
                            oAttach.Lines.Add()
                            bAttachAdd = True
                        End If
                    Next
                    If bAttachAdd = True Then
                        If oAttach.Add() = 0 Then
                            oAttachEntry = PublicVariable.oCompany.GetNewObjectKey()
                            oDO.AttachmentEntry = oAttachEntry
                        Else
                            Throw New Exception("Error while adding attachment /" & PublicVariable.oCompany.GetLastErrorDescription)
                        End If
                    End If
                End If

                RetCode = oDO.Add
                If RetCode <> 0 Then
                    PublicVariable.oCompany.GetLastError(RetCode, errMessage)
                    Throw New Exception(errMessage)
                Else
                    SONo = PublicVariable.oCompany.GetNewObjectKey()
                End If
            Else
                Throw New Exception("No Input Data.")
            End If

            Dim RetDT As New DataTable
            RetDT.TableName = "VALIDATE"
            RetDT.Columns.Add("Status")
            RetDT.Columns.Add("Msg")
            RetDT.Rows.Add()
            RetDT.Rows(0)(0) = True
            RetDT.Rows(0)(1) = "Survey no. " & SONo.ToString & " Created Successfully"
            RetDS.Tables.Add(RetDT)
            Context.Response.Output.Write(fn.ds2json(RetDS))


        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
         <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub Cleaning1_NewFormat_SurveyTyepAdd(ByVal value As String)
        sFunction = "Cleaning1_NewFormat_SurveyTyepAdd"
        Try

            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim SQTO As New DataTable
            SQTO = New DataTable
            Dim SQTOGEN As New DataTable
            Dim SQTOADD As New DataTable
            Dim SOTOATTACH As New DataTable
            Dim SQTODS = New DataSet()
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            fn.GetSAPConnection(Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            Dim STypeCode As String = ""
            Dim STypeName As String = ""
            Dim RetCode As Integer = 0
            Dim SONo As String = ""
            Dim errMessage As String = ""
            Dim CardCode As String = ""
            Dim DocDate As String = ""
            Dim ProjectCode As String = ""
            Dim Country As String = ""
            Dim City As String = ""
            Dim EqupTye As String = ""
            Dim Rt As String = ""
            Dim SQLStr As String = ""

            Dim oSO As SAPbobsCOM.Documents
            oSO = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
            Dim oDO As SAPbobsCOM.Documents
            oDO = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes)


            If ds.Tables("ODLN").Rows.Count > 0 Then
                SQTO = ds.Tables("ODLN")
                Dim dr As DataRow = SQTO.Rows(0)

                If oSO.GetByKey(dr.Item("U_OrderNo")) = True Then

                Else
                    Throw New Exception("No Record Found!")
                End If

                oDO.UserFields.Fields.Item("U_UCode").Value = dr.Item("U_UCode").ToString.Trim()
                oDO.UserFields.Fields.Item("U_UName").Value = dr.Item("U_UName").ToString.Trim()
                oDO.UserFields.Fields.Item("U_Cdate").Value = GetDateTimeValue(dr.Item("U_Cdate").ToString)
                oDO.DocDate = DateConvert(dr.Item("DocDate").ToString)
                oDO.DocDueDate = DateConvert(dr.Item("DocDate").ToString)
                oDO.CardCode = oSO.CardCode
                oDO.CardName = oSO.CardName

                oDO.UserFields.Fields.Item("U_FormName").Value = dr.Item("U_FormName").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SResult").Value = dr.Item("U_SResult").ToString.Trim()

                oDO.UserFields.Fields.Item("U_EX_Fram").Value = dr.Item("U_EX_Fram").ToString.Trim()
                oDO.UserFields.Fields.Item("U_FORMAT").Value = "2"
                oDO.UserFields.Fields.Item("U_EX_Man").Value = dr.Item("U_EX_Man").ToString.Trim()
                oDO.UserFields.Fields.Item("U_EX_Ser").Value = dr.Item("U_EX_Ser").ToString.Trim()
                oDO.UserFields.Fields.Item("U_EX_Car").Value = dr.Item("U_EX_Car").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Free").Value = dr.Item("U_INT_Free").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Clean").Value = dr.Item("U_INT_Clean").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Dry").Value = dr.Item("U_INT_Dry").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Pitt").Value = dr.Item("U_INT_Pitt").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Disc").Value = dr.Item("U_INT_Disc").ToString.Trim()

                oDO.UserFields.Fields.Item("U_INT_Cleanliness").Value = dr.Item("U_INT_Cleanliness").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Rust").Value = dr.Item("U_INT_Rust").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Deposits").Value = dr.Item("U_INT_Deposits").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_DamagedCoating").Value = dr.Item("U_INT_DamagedCoating").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_GasketMainHole").Value = dr.Item("U_INT_GasketMainHole").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_OilDirtFree").Value = dr.Item("U_INT_OilDirtFree").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_SteamCoil").Value = dr.Item("U_INT_SteamCoil").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Steam_Cleaned").Value = dr.Item("U_INT_Steam_Cleaned").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_GasktDschrgval").Value = dr.Item("U_INT_GasktDschrgval").ToString.Trim()

                oDO.UserFields.Fields.Item("U_VAL_Val").Value = dr.Item("U_VAL_Val").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Bott").Value = dr.Item("U_VAL_Bott").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Man").Value = dr.Item("U_VAL_Man").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Syp").Value = dr.Item("U_VAL_Syp").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Tank").Value = dr.Item("U_VAL_Tank").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Avail").Value = dr.Item("U_VAL_Avail").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Steam").Value = dr.Item("U_VAL_Steam").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Gas").Value = dr.Item("U_VAL_Gas").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SEAL_MAN").Value = dr.Item("U_SEAL_MAN").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SEAL_AIR").Value = dr.Item("U_SEAL_AIR").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SEAL_BOTT").Value = dr.Item("U_SEAL_BOTT").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SEAL_LAST").Value = dr.Item("U_SEAL_LAST").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SEAL_NEXT").Value = dr.Item("U_SEAL_NEXT").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_SPIL").Value = dr.Item("U_VAL_SPIL").ToString.Trim()
                oDO.UserFields.Fields.Item("U_PayLoad").Value = dr.Item("U_PayLoad").ToString.Trim()
                oDO.UserFields.Fields.Item("U_Capacity").Value = dr.Item("U_Capacity").ToString.Trim()
                oDO.UserFields.Fields.Item("U_CSCNum").Value = dr.Item("U_CSCNum").ToString.Trim()
                oDO.UserFields.Fields.Item("U_IMO").Value = dr.Item("U_IMO").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Cleanliness").Value = dr.Item("U_INT_Cleanliness").ToString.Trim()
                Try
                    'oDO.UserFields.Fields.Item("U_DOM").Value = DateConvert(dr.Item("U_DOM").ToString.Trim())
                    oDO.UserFields.Fields.Item("U_DOM").Value = (dr.Item("U_DOM").ToString.Trim())
                Catch ex As Exception
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile("Error while Adding DOM", sFunction)
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_MGW").Value = dr.Item("U_MGW").ToString.Trim()
                Catch ex As Exception
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile("Error while Adding MGW", sFunction)
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Tare").Value = dr.Item("U_Tare").ToString.Trim()
                Catch ex As Exception
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile("Error while adding Tare", sFunction)
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_ACEP").Value = dr.Item("U_ACEP").ToString.Trim()
                Catch ex As Exception
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile("Error while adding ACEP", sFunction)
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
                End Try
                Try
                    'oDO.UserFields.Fields.Item("U_CSC").Value = DateConvert(dr.Item("U_CSC").ToString.Trim())
                    oDO.UserFields.Fields.Item("U_CSC").Value = (dr.Item("U_CSC").ToString.Trim())
                Catch ex As Exception
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile("Error while Adding CSC", sFunction)
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
                End Try

                oDO.UserFields.Fields.Item("U_SurveyorID").Value = oSO.UserFields.Fields.Item("U_SurveyorID").Value
                oDO.NumAtCard = dr.Item("NumAtCard").ToString.Trim
                oDO.UserFields.Fields.Item("U_SuvExAgent").Value = dr.Item("U_SuvExAgent").ToString.Trim()
                oDO.UserFields.Fields.Item("U_NoPh").Value = dr.Item("U_NoPh").ToString.Trim()
                oDO.UserFields.Fields.Item("U_EqNo").Value = dr.Item("U_EqNo").ToString.Trim()

                '  STypeCode = dr.Item("U_STypeCode")
                ' STypeName = dr.Item("U_STypeName")
                oDO.UserFields.Fields.Item("U_STypeCode").Value = oSO.UserFields.Fields.Item("U_STypeCode").Value
                oDO.UserFields.Fields.Item("U_U_STypeName").Value = oSO.UserFields.Fields.Item("U_U_STypeName").Value
                oDO.UserFields.Fields.Item("U_Country").Value = oSO.UserFields.Fields.Item("U_Country").Value
                oDO.UserFields.Fields.Item("U_City").Value = oSO.UserFields.Fields.Item("U_City").Value
                oDO.UserFields.Fields.Item("U_Loc").Value = oSO.UserFields.Fields.Item("U_Loc").Value
                oDO.Comments = dr.Item("Comments").ToString.Trim()
                oDO.Lines.ItemCode = oSO.Lines.ItemCode
                oDO.Lines.Quantity = 1
                oDO.Lines.UserFields.Fields.Item("U_PDate").Value = oSO.Lines.UserFields.Fields.Item("U_PDate").Value
                oDO.Lines.UserFields.Fields.Item("U_EQType").Value = oSO.Lines.UserFields.Fields.Item("U_EQType").Value
                'EqupTye = dr1.Item("U_EQType").ToString.Trim()
                oDO.Lines.UserFields.Fields.Item("U_SCriteria").Value = oSO.Lines.UserFields.Fields.Item("U_SCriteria").Value
                oDO.Lines.BaseType = 17 '23 ' - 'Sales Quotation'
                oDO.Lines.BaseEntry = dr.Item("U_OrderNo")
                oDO.Lines.BaseLine = 0

                oDO.Lines.UnitPrice = oSO.Lines.UnitPrice
                oDO.Lines.Currency = oSO.Lines.Currency
                oDO.Lines.Add()

                Dim oAttachEntry As Integer = 0
                Dim bAttachAdd As Boolean = False
                If ds.Tables("ATTACHMENT").Rows.Count > 0 Then
                    SOTOATTACH = ds.Tables("ATTACHMENT")
                    Dim oAttach As SAPbobsCOM.Attachments2
                    oAttach = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2)

                    For Each odr As DataRow In SOTOATTACH.Rows

                        Dim sSourcePath, sFileName, sFileExt As String
                        sFileName = odr.Item("U_FileName").ToString.Trim()
                        If sFileName <> "" Then
                            sFileExt = Path.GetExtension(sFileName)
                            sFileExt = sFileExt.Replace(".", "")
                            sSourcePath = odr.Item("U_FilePath").ToString.Substring(0, 3)
                            oAttach.Lines.SourcePath = PublicVariable.sTempfilePath
                            oAttach.Lines.FileName = sFileName.Replace(Path.GetExtension(sFileName), "")
                            oAttach.Lines.FileExtension = sFileExt
                            oAttach.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES
                            oAttach.Lines.Add()
                            bAttachAdd = True
                        End If
                    Next
                    If bAttachAdd = True Then
                        If oAttach.Add() = 0 Then
                            oAttachEntry = PublicVariable.oCompany.GetNewObjectKey()
                            oDO.AttachmentEntry = oAttachEntry
                        Else
                            Throw New Exception("Error while adding attachment /" & PublicVariable.oCompany.GetLastErrorDescription)
                        End If
                    End If
                End If

                RetCode = oDO.Add
                If RetCode <> 0 Then
                    PublicVariable.oCompany.GetLastError(RetCode, errMessage)
                    Throw New Exception(errMessage)
                Else
                    SONo = PublicVariable.oCompany.GetNewObjectKey()
                End If
            Else
                Throw New Exception("No Input Data.")
            End If

            Dim RetDT As New DataTable
            RetDT.TableName = "VALIDATE"
            RetDT.Columns.Add("Status")
            RetDT.Columns.Add("Msg")
            RetDT.Rows.Add()
            RetDT.Rows(0)(0) = True
            RetDT.Rows(0)(1) = "Survey no. " & SONo.ToString & " Created Successfully"
            RetDS.Tables.Add(RetDT)
            Context.Response.Output.Write(fn.ds2json(RetDS))


        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub

    <WebMethod()> _
         <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub AnyOther_SurveyTyepUpdate(ByVal value As String)
        sFunction = "AnyOther_SurveyTyepUpdate"
        Try

            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim SQTO As New DataTable
            SQTO = New DataTable
            Dim SQTOGEN As New DataTable
            Dim SQTOADD As New DataTable
            Dim SOTOATTACH As New DataTable
            Dim SQTODS = New DataSet()
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            fn.GetSAPConnection(Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            Dim STypeCode As String = ""
            Dim STypeName As String = ""
            Dim RetCode As Integer = 0
            Dim SONo As String = ""
            Dim errMessage As String = ""
            Dim CardCode As String = ""
            Dim DocDate As String = ""
            Dim ProjectCode As String = ""
            Dim Country As String = ""
            Dim City As String = ""
            Dim EqupTye As String = ""
            Dim Rt As String = ""
            Dim SQLStr As String = ""

            'Dim oSO As SAPbobsCOM.Documents
            'oSO = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
            Dim oDO As SAPbobsCOM.Documents
            oDO = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes)


            If ds.Tables("ODLN").Rows.Count > 0 Then
                SQTO = ds.Tables("ODLN")
                Dim dr As DataRow = SQTO.Rows(0)

                If oDO.GetByKey(dr.Item("U_SurvyNo")) = True Then

                Else
                    Throw New Exception("No Record Found!")
                End If

                'oDO.UserFields.Fields.Item("U_UCode").Value = dr.Item("U_UCode").ToString.Trim()
                'oDO.UserFields.Fields.Item("U_UName").Value = dr.Item("U_UName").ToString.Trim()
                'oDO.UserFields.Fields.Item("U_Cdate").Value = dr.Item("U_Cdate")
                'oDO.DocDate = DateConvert(dr.Item("DocDate").ToString)
                'oDO.DocDueDate = DateConvert(dr.Item("DocDate").ToString)
                'oDO.CardCode = oSO.CardCode
                'oDO.CardName = oSO.CardName

                ' oDO.UserFields.Fields.Item("U_FormName").Value = dr.Item("U_FormName").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SResult").Value = dr.Item("U_SResult").ToString.Trim()

                '  oDO.UserFields.Fields.Item("U_SurveyorID").Value = oSO.UserFields.Fields.Item("U_SurveyorID").Value
                'oDO.NumAtCard = oSO.NumAtCard 'dr.Item("NumAtCard").ToString.Trim
                'oDO.UserFields.Fields.Item("U_SuvExAgent").Value = dr.Item("U_SuvExAgent").ToString.Trim()
                oDO.UserFields.Fields.Item("U_NoPh").Value = dr.Item("U_NoPh").ToString.Trim()
                oDO.UserFields.Fields.Item("U_EqNo").Value = dr.Item("U_EqNo").ToString.Trim()

                '  STypeCode = dr.Item("U_STypeCode")
                ' STypeName = dr.Item("U_STypeName")
                'oDO.UserFields.Fields.Item("U_STypeCode").Value = oSO.UserFields.Fields.Item("U_STypeCode").Value
                'oDO.UserFields.Fields.Item("U_U_STypeName").Value = oSO.UserFields.Fields.Item("U_U_STypeName").Value
                'oDO.UserFields.Fields.Item("U_Country").Value = oSO.UserFields.Fields.Item("U_Country").Value
                'oDO.UserFields.Fields.Item("U_City").Value = oSO.UserFields.Fields.Item("U_City").Value
                'oDO.UserFields.Fields.Item("U_Loc").Value = oSO.UserFields.Fields.Item("U_Loc").Value
                'oDO.Comments = oSO.Comments
                'oDO.Lines.ItemCode = oSO.Lines.ItemCode
                'oDO.Lines.Quantity = 1
                'oDO.Lines.UserFields.Fields.Item("U_PDate").Value = oSO.Lines.UserFields.Fields.Item("U_PDate").Value
                'oDO.Lines.UserFields.Fields.Item("U_EQType").Value = oSO.Lines.UserFields.Fields.Item("U_EQType").Value
                ''EqupTye = dr1.Item("U_EQType").ToString.Trim()
                'oDO.Lines.UserFields.Fields.Item("U_SCriteria").Value = oSO.Lines.UserFields.Fields.Item("U_SCriteria").Value
                'oDO.Lines.BaseType = 17 '23 ' - 'Sales Quotation'
                'oDO.Lines.BaseEntry = dr.Item("U_OrderNo")
                'oDO.Lines.BaseLine = 0

                'oDO.Lines.UnitPrice = oSO.Lines.UnitPrice
                'oDO.Lines.Add()

                Dim iAttachEntry As Integer = 0
                Dim bAttchAdd As Boolean = False
                Dim bFileExists As Boolean = False
                If ds.Tables("ATTACHMENT").Rows.Count > 0 Then
                    SOTOATTACH = ds.Tables("ATTACHMENT")
                    Dim oAttach As SAPbobsCOM.Attachments2
                    oAttach = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2)
                    Dim oRecordSet As SAPbobsCOM.Recordset
                    oRecordSet = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

                    If oAttach.GetByKey(oDO.AttachmentEntry) Then
                        For Each odr As DataRow In SOTOATTACH.Rows
                            Dim sSourcePath, sFileName, sFileExt As String
                            sFileName = odr.Item("U_FileName").ToString.Trim()
                            If sFileName <> "" Then
                                sFileExt = Path.GetExtension(sFileName)
                                sFileExt = sFileExt.Replace(".", "")
                                sSourcePath = odr.Item("U_FilePath").ToString.Substring(0, 3)
                                Dim sQuery As String = String.Empty
                                sQuery = "SELECT * FROM ""ATC1"" WHERE ""AbsEntry"" = '" & oDO.AttachmentEntry & "' AND ""FileName"" = '" & sFileName.Replace(Path.GetExtension(sFileName), "") & "' "
                                oRecordSet.DoQuery(sQuery)
                                If oRecordSet.RecordCount > 0 Then
                                Else
                                    oAttach.Lines.Add()
                                    oAttach.Lines.SourcePath = PublicVariable.sTempfilePath
                                    oAttach.Lines.FileName = sFileName.Replace(Path.GetExtension(sFileName), "")
                                    oAttach.Lines.FileExtension = sFileExt
                                    oAttach.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES
                                End If
                            End If
                        Next
                        If oAttach.Update() = 0 Then
                            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug("Attachment updated successfully", sFunction)
                        Else
                            Throw New Exception("Error while updating attachment /" & PublicVariable.oCompany.GetLastErrorDescription)
                        End If
                    Else
                        For Each odr As DataRow In SOTOATTACH.Rows
                            Dim sSourcePath, sFileName, sFileExt As String
                            sFileName = odr.Item("U_FileName").ToString.Trim()
                            If sFileName <> "" Then
                                sFileExt = Path.GetExtension(sFileName)
                                sFileExt = sFileExt.Replace(".", "")
                                sSourcePath = odr.Item("U_FilePath").ToString.Substring(0, 3)
                                oAttach.Lines.SourcePath = PublicVariable.sTempfilePath
                                oAttach.Lines.FileName = sFileName.Replace(Path.GetExtension(sFileName), "")
                                oAttach.Lines.FileExtension = sFileExt
                                oAttach.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES
                                oAttach.Lines.Add()
                                bAttchAdd = True
                            End If
                        Next
                        If bAttchAdd = True Then
                            If oAttach.Add() = 0 Then
                                iAttachEntry = PublicVariable.oCompany.GetNewObjectKey()
                                oDO.AttachmentEntry = iAttachEntry
                            Else
                                Throw New Exception("Error while adding attachment /" & PublicVariable.oCompany.GetLastErrorDescription)
                            End If
                        End If
                    End If
                End If

                RetCode = oDO.Update
                If RetCode <> 0 Then
                    PublicVariable.oCompany.GetLastError(RetCode, errMessage)
                    Throw New Exception(errMessage)
                Else
                    SONo = PublicVariable.oCompany.GetNewObjectKey()
                End If
            Else
                Throw New Exception("No Input Data.")
            End If

            Dim RetDT As New DataTable
            RetDT.TableName = "VALIDATE"
            RetDT.Columns.Add("Status")
            RetDT.Columns.Add("Msg")
            RetDT.Rows.Add()
            RetDT.Rows(0)(0) = True
            RetDT.Rows(0)(1) = "Survey no. " & SONo.ToString & " Updated Successfully"
            RetDS.Tables.Add(RetDT)
            Context.Response.Output.Write(fn.ds2json(RetDS))


        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub OnHire_SurveyTyepUpdate(ByVal value As String)
        sFunction = "OnHire_SurveyTyepUpdate"
        Try

            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim SQTO As New DataTable
            SQTO = New DataTable
            Dim SQTOGEN As New DataTable
            Dim SQTOADD As New DataTable
            Dim SOTOATTACH As New DataTable
            Dim SQTODS = New DataSet()
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            fn.GetSAPConnection(Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            Dim STypeCode As String = ""
            Dim STypeName As String = ""
            Dim RetCode As Integer = 0
            Dim SONo As String = ""
            Dim errMessage As String = ""
            Dim CardCode As String = ""
            Dim DocDate As String = ""
            Dim ProjectCode As String = ""
            Dim Country As String = ""
            Dim City As String = ""
            Dim EqupTye As String = ""
            Dim Rt As String = ""
            Dim SQLStr As String = ""

            'Dim oSO As SAPbobsCOM.Documents
            'oSO = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
            Dim oDO As SAPbobsCOM.Documents
            oDO = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes)


            If ds.Tables("ODLN").Rows.Count > 0 Then
                SQTO = ds.Tables("ODLN")
                Dim dr As DataRow = SQTO.Rows(0)

                If oDO.GetByKey(dr.Item("U_SurvyNo")) = True Then

                Else
                    Throw New Exception("No Record Found!")
                End If

                'oDO.UserFields.Fields.Item("U_UCode").Value = dr.Item("U_UCode").ToString.Trim()
                'oDO.UserFields.Fields.Item("U_UName").Value = dr.Item("U_UName").ToString.Trim()
                'oDO.UserFields.Fields.Item("U_Cdate").Value = dr.Item("U_Cdate")
                'oDO.DocDate = DateConvert(dr.Item("DocDate").ToString)
                'oDO.DocDueDate = DateConvert(dr.Item("DocDate").ToString)
                'oDO.CardCode = oSO.CardCode
                'oDO.CardName = oSO.CardName

                'oDO.UserFields.Fields.Item("U_FormName").Value = dr.Item("U_FormName").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SResult").Value = dr.Item("U_SResult").ToString.Trim()
                Try
                    oDO.UserFields.Fields.Item("U_DOM").Value = DateConvert(dr.Item("U_DOM").ToString.Trim())
                Catch ex As Exception

                End Try
                Try
                    oDO.UserFields.Fields.Item("U_MGW").Value = dr.Item("U_MGW").ToString.Trim()
                Catch ex As Exception

                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Tare").Value = dr.Item("U_Tare").ToString.Trim()
                Catch ex As Exception

                End Try
                Try
                    oDO.UserFields.Fields.Item("U_ACEP").Value = dr.Item("U_ACEP").ToString.Trim()
                Catch ex As Exception

                End Try
                Try
                    oDO.UserFields.Fields.Item("U_CSC").Value = DateConvert(dr.Item("U_CSC").ToString.Trim())
                Catch ex As Exception

                End Try




                'oDO.UserFields.Fields.Item("U_SurveyorID").Value = oSO.UserFields.Fields.Item("U_SurveyorID").Value
                'oDO.NumAtCard = oSO.NumAtCard 'dr.Item("NumAtCard").ToString.Trim
                'oDO.UserFields.Fields.Item("U_SuvExAgent").Value = dr.Item("U_SuvExAgent").ToString.Trim()
                oDO.UserFields.Fields.Item("U_NoPh").Value = dr.Item("U_NoPh").ToString.Trim()
                oDO.UserFields.Fields.Item("U_EqNo").Value = dr.Item("U_EqNo").ToString.Trim()

                '  STypeCode = dr.Item("U_STypeCode")
                ' STypeName = dr.Item("U_STypeName")
                'oDO.UserFields.Fields.Item("U_STypeCode").Value = oSO.UserFields.Fields.Item("U_STypeCode").Value
                'oDO.UserFields.Fields.Item("U_U_STypeName").Value = oSO.UserFields.Fields.Item("U_U_STypeName").Value
                'oDO.UserFields.Fields.Item("U_Country").Value = oSO.UserFields.Fields.Item("U_Country").Value
                'oDO.UserFields.Fields.Item("U_City").Value = oSO.UserFields.Fields.Item("U_City").Value
                'oDO.UserFields.Fields.Item("U_Loc").Value = oSO.UserFields.Fields.Item("U_Loc").Value

                'oDO.Comments = oSO.Comments
                'oDO.Lines.ItemCode = oSO.Lines.ItemCode
                'oDO.Lines.Quantity = 1
                'oDO.Lines.UserFields.Fields.Item("U_PDate").Value = oSO.Lines.UserFields.Fields.Item("U_PDate").Value
                'oDO.Lines.UserFields.Fields.Item("U_EQType").Value = oSO.Lines.UserFields.Fields.Item("U_EQType").Value
                'EqupTye = dr1.Item("U_EQType").ToString.Trim()
                'oDO.Lines.UserFields.Fields.Item("U_SCriteria").Value = oSO.Lines.UserFields.Fields.Item("U_SCriteria").Value
                'oDO.Lines.BaseType = 17 '23 ' - 'Sales Quotation'
                'oDO.Lines.BaseEntry = dr.Item("U_OrderNo")
                'oDO.Lines.BaseLine = 0

                'oDO.Lines.UnitPrice = oSO.Lines.UnitPrice
                'oDO.Lines.Add()

                Dim iAttachEntry As Integer = 0
                Dim bAttchAdd As Boolean = False
                Dim bFileExists As Boolean = False
                If ds.Tables("ATTACHMENT").Rows.Count > 0 Then
                    SOTOATTACH = ds.Tables("ATTACHMENT")
                    Dim oAttach As SAPbobsCOM.Attachments2
                    oAttach = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2)
                    Dim oRecordSet As SAPbobsCOM.Recordset
                    oRecordSet = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

                    If oAttach.GetByKey(oDO.AttachmentEntry) Then
                        For Each odr As DataRow In SOTOATTACH.Rows
                            Dim sSourcePath, sFileName, sFileExt As String
                            sFileName = odr.Item("U_FileName").ToString.Trim()
                            If sFileName <> "" Then
                                sFileExt = Path.GetExtension(sFileName)
                                sFileExt = sFileExt.Replace(".", "")
                                sSourcePath = odr.Item("U_FilePath").ToString.Substring(0, 3)
                                Dim sQuery As String = String.Empty
                                sQuery = "SELECT * FROM ""ATC1"" WHERE ""AbsEntry"" = '" & oDO.AttachmentEntry & "' AND ""FileName"" = '" & sFileName.Replace(Path.GetExtension(sFileName), "") & "' "
                                oRecordSet.DoQuery(sQuery)
                                If oRecordSet.RecordCount > 0 Then
                                Else
                                    oAttach.Lines.Add()
                                    oAttach.Lines.SourcePath = PublicVariable.sTempfilePath
                                    oAttach.Lines.FileName = sFileName.Replace(Path.GetExtension(sFileName), "")
                                    oAttach.Lines.FileExtension = sFileExt
                                    oAttach.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES
                                End If
                            End If
                        Next
                        If oAttach.Update() = 0 Then
                            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug("Attachment updated successfully", sFunction)
                        Else
                            Throw New Exception("Error while updating attachment /" & PublicVariable.oCompany.GetLastErrorDescription)
                        End If
                    Else
                        For Each odr As DataRow In SOTOATTACH.Rows
                            Dim sSourcePath, sFileName, sFileExt As String
                            sFileName = odr.Item("U_FileName").ToString.Trim()
                            If sFileName <> "" Then
                                sFileExt = Path.GetExtension(sFileName)
                                sFileExt = sFileExt.Replace(".", "")
                                sSourcePath = odr.Item("U_FilePath").ToString.Substring(0, 3)
                                oAttach.Lines.SourcePath = PublicVariable.sTempfilePath
                                oAttach.Lines.FileName = sFileName.Replace(Path.GetExtension(sFileName), "")
                                oAttach.Lines.FileExtension = sFileExt
                                oAttach.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES
                                oAttach.Lines.Add()
                                bAttchAdd = True
                            End If
                        Next
                        If bAttchAdd = True Then
                            If oAttach.Add() = 0 Then
                                iAttachEntry = PublicVariable.oCompany.GetNewObjectKey()
                                oDO.AttachmentEntry = iAttachEntry
                            Else
                                Throw New Exception("Error while adding attachment /" & PublicVariable.oCompany.GetLastErrorDescription)
                            End If
                        End If
                    End If
                End If

                RetCode = oDO.Update()
                If RetCode <> 0 Then
                    PublicVariable.oCompany.GetLastError(RetCode, errMessage)
                    Throw New Exception(errMessage)
                Else
                    SONo = PublicVariable.oCompany.GetNewObjectKey()
                End If
            Else
                Throw New Exception("No Input Data.")
            End If

            Dim RetDT As New DataTable
            RetDT.TableName = "VALIDATE"
            RetDT.Columns.Add("Status")
            RetDT.Columns.Add("Msg")
            RetDT.Rows.Add()
            RetDT.Rows(0)(0) = True
            RetDT.Rows(0)(1) = "Survey no. " & SONo.ToString & " Updated Successfully"
            RetDS.Tables.Add(RetDT)
            Context.Response.Output.Write(fn.ds2json(RetDS))


        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
         <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub OffHire_SurveyTyepUpdate(ByVal value As String)
        sFunction = "OffHire_SurveyTyepUpdate"
        Try

            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim SQTO As New DataTable
            SQTO = New DataTable
            Dim SQTOGEN As New DataTable
            Dim SQTOADD As New DataTable
            Dim SOTOATTACH As New DataTable
            Dim SQTODS = New DataSet()
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            fn.GetSAPConnection(Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            Dim STypeCode As String = ""
            Dim STypeName As String = ""
            Dim RetCode As Integer = 0
            Dim SONo As String = ""
            Dim errMessage As String = ""
            Dim CardCode As String = ""
            Dim DocDate As String = ""
            Dim ProjectCode As String = ""
            Dim Country As String = ""
            Dim City As String = ""
            Dim EqupTye As String = ""
            Dim Rt As String = ""
            Dim SQLStr As String = ""

            'Dim oSO As SAPbobsCOM.Documents
            'oSO = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
            Dim oDO As SAPbobsCOM.Documents
            oDO = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes)


            If ds.Tables("ODLN").Rows.Count > 0 Then
                SQTO = ds.Tables("ODLN")
                Dim dr As DataRow = SQTO.Rows(0)

                If oDO.GetByKey(dr.Item("U_SurvyNo")) = True Then

                Else
                    Throw New Exception("No Record Found!")
                End If

                'oDO.UserFields.Fields.Item("U_UCode").Value = dr.Item("U_UCode").ToString.Trim()
                'oDO.UserFields.Fields.Item("U_UName").Value = dr.Item("U_UName").ToString.Trim()
                'oDO.UserFields.Fields.Item("U_Cdate").Value = dr.Item("U_Cdate")
                'oDO.DocDate = DateConvert(dr.Item("DocDate").ToString)
                'oDO.DocDueDate = DateConvert(dr.Item("DocDate").ToString)
                'oDO.CardCode = oSO.CardCode
                'oDO.CardName = oSO.CardName

                Try
                    oDO.UserFields.Fields.Item("U_Esti_Org").Value = dr.Item("U_Esti_Org").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Esti_Rev").Value = dr.Item("U_Esti_Rev").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Esti_Diff").Value = dr.Item("U_Esti_Diff").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Curr_Org").Value = dr.Item("U_Curr_Org").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Curr_Rev").Value = dr.Item("U_Curr_Rev").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Curr_diff").Value = dr.Item("U_Curr_diff").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Impac_Org").Value = dr.Item("U_Impac_Org").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Impac_Rev").Value = dr.Item("U_Impac_Rev").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Impac_diff").Value = dr.Item("U_Impac_diff").ToString.Trim()
                Catch ex As Exception

                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Wear_org").Value = dr.Item("U_Wear_org").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Wear_Rev").Value = dr.Item("U_Wear_Rev").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Wear_Diff").Value = dr.Item("U_Wear_Diff").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Cust_Org").Value = dr.Item("U_Cust_Org").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Cust_Rev").Value = dr.Item("U_Cust_Rev").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Cust_Diff").Value = dr.Item("U_Cust_Diff").ToString.Trim()
                Catch ex As Exception

                End Try

                Try
                    oDO.UserFields.Fields.Item("U_Impor_Org").Value = dr.Item("U_Impor_Org").ToString.Trim()
                Catch ex As Exception

                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Impor_Rev").Value = dr.Item("U_Impor_Rev").ToString.Trim()
                Catch ex As Exception

                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Import_Diff").Value = dr.Item("U_Import_Diff").ToString.Trim()
                Catch ex As Exception
                End Try

                Try
                    oDO.UserFields.Fields.Item("U_Sum_Org").Value = dr.Item("U_Sum_Org").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Sum_Rev").Value = dr.Item("U_Sum_Rev").ToString.Trim()
                Catch ex As Exception
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Sum_Diff").Value = dr.Item("U_Sum_Diff").ToString.Trim()
                Catch ex As Exception
                End Try



                'oDO.UserFields.Fields.Item("U_FormName").Value = dr.Item("U_FormName").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SResult").Value = dr.Item("U_SResult").ToString.Trim()

                'oDO.UserFields.Fields.Item("U_SurveyorID").Value = oSO.UserFields.Fields.Item("U_SurveyorID").Value
                'oDO.NumAtCard = oSO.NumAtCard 'dr.Item("NumAtCard").ToString.Trim
                'oDO.UserFields.Fields.Item("U_SuvExAgent").Value = dr.Item("U_SuvExAgent").ToString.Trim()
                oDO.UserFields.Fields.Item("U_NoPh").Value = dr.Item("U_NoPh").ToString.Trim()
                oDO.UserFields.Fields.Item("U_EqNo").Value = dr.Item("U_EqNo").ToString.Trim()

                '  STypeCode = dr.Item("U_STypeCode")
                ' STypeName = dr.Item("U_STypeName")
                'oDO.UserFields.Fields.Item("U_STypeCode").Value = oSO.UserFields.Fields.Item("U_STypeCode").Value
                'oDO.UserFields.Fields.Item("U_U_STypeName").Value = oSO.UserFields.Fields.Item("U_U_STypeName").Value
                'oDO.UserFields.Fields.Item("U_Country").Value = oSO.UserFields.Fields.Item("U_Country").Value
                'oDO.UserFields.Fields.Item("U_City").Value = oSO.UserFields.Fields.Item("U_City").Value
                'oDO.UserFields.Fields.Item("U_Loc").Value = oSO.UserFields.Fields.Item("U_Loc").Value
                'oDO.Comments = oSO.Comments
                'oDO.Lines.ItemCode = oSO.Lines.ItemCode
                'oDO.Lines.Quantity = 1
                'oDO.Lines.UserFields.Fields.Item("U_PDate").Value = oSO.Lines.UserFields.Fields.Item("U_PDate").Value
                'oDO.Lines.UserFields.Fields.Item("U_EQType").Value = oSO.Lines.UserFields.Fields.Item("U_EQType").Value
                ''EqupTye = dr1.Item("U_EQType").ToString.Trim()
                'oDO.Lines.UserFields.Fields.Item("U_SCriteria").Value = oSO.Lines.UserFields.Fields.Item("U_SCriteria").Value
                'oDO.Lines.BaseType = 17 '23 ' - 'Sales Quotation'
                'oDO.Lines.BaseEntry = dr.Item("U_OrderNo")
                'oDO.Lines.BaseLine = 0

                'oDO.Lines.UnitPrice = oSO.Lines.UnitPrice
                'oDO.Lines.Add()

                Dim iAttachEntry As Integer = 0
                Dim bAttchAdd As Boolean = False
                Dim bFileExists As Boolean = False
                If ds.Tables("ATTACHMENT").Rows.Count > 0 Then
                    SOTOATTACH = ds.Tables("ATTACHMENT")
                    Dim oAttach As SAPbobsCOM.Attachments2
                    oAttach = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2)
                    Dim oRecordSet As SAPbobsCOM.Recordset
                    oRecordSet = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

                    If oAttach.GetByKey(oDO.AttachmentEntry) Then
                        For Each odr As DataRow In SOTOATTACH.Rows
                            Dim sSourcePath, sFileName, sFileExt As String
                            sFileName = odr.Item("U_FileName").ToString.Trim()
                            If sFileName <> "" Then
                                sFileExt = Path.GetExtension(sFileName)
                                sFileExt = sFileExt.Replace(".", "")
                                sSourcePath = odr.Item("U_FilePath").ToString.Substring(0, 3)
                                Dim sQuery As String = String.Empty
                                sQuery = "SELECT * FROM ""ATC1"" WHERE ""AbsEntry"" = '" & oDO.AttachmentEntry & "' AND ""FileName"" = '" & sFileName.Replace(Path.GetExtension(sFileName), "") & "' "
                                oRecordSet.DoQuery(sQuery)
                                If oRecordSet.RecordCount > 0 Then
                                Else
                                    oAttach.Lines.Add()
                                    oAttach.Lines.SourcePath = PublicVariable.sTempfilePath
                                    oAttach.Lines.FileName = sFileName.Replace(Path.GetExtension(sFileName), "")
                                    oAttach.Lines.FileExtension = sFileExt
                                    oAttach.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES
                                End If
                            End If
                        Next
                        If oAttach.Update() = 0 Then
                            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug("Attachment updated successfully", sFunction)
                        Else
                            Throw New Exception("Error while updating attachment /" & PublicVariable.oCompany.GetLastErrorDescription)
                        End If
                    Else
                        For Each odr As DataRow In SOTOATTACH.Rows
                            Dim sSourcePath, sFileName, sFileExt As String
                            sFileName = odr.Item("U_FileName").ToString.Trim()
                            If sFileName <> "" Then
                                sFileExt = Path.GetExtension(sFileName)
                                sFileExt = sFileExt.Replace(".", "")
                                sSourcePath = odr.Item("U_FilePath").ToString.Substring(0, 3)
                                oAttach.Lines.SourcePath = PublicVariable.sTempfilePath
                                oAttach.Lines.FileName = sFileName.Replace(Path.GetExtension(sFileName), "")
                                oAttach.Lines.FileExtension = sFileExt
                                oAttach.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES
                                oAttach.Lines.Add()
                                bAttchAdd = True
                            End If
                        Next
                        If bAttchAdd = True Then
                            If oAttach.Add() = 0 Then
                                iAttachEntry = PublicVariable.oCompany.GetNewObjectKey()
                                oDO.AttachmentEntry = iAttachEntry
                            Else
                                Throw New Exception("Error while adding attachment /" & PublicVariable.oCompany.GetLastErrorDescription)
                            End If
                        End If
                    End If
                End If

                RetCode = oDO.Update
                If RetCode <> 0 Then
                    PublicVariable.oCompany.GetLastError(RetCode, errMessage)
                    Throw New Exception(errMessage)
                Else
                    SONo = PublicVariable.oCompany.GetNewObjectKey()
                End If
            Else
                Throw New Exception("No Input Data.")
            End If

            Dim RetDT As New DataTable
            RetDT.TableName = "VALIDATE"
            RetDT.Columns.Add("Status")
            RetDT.Columns.Add("Msg")
            RetDT.Rows.Add()
            RetDT.Rows(0)(0) = True
            RetDT.Rows(0)(1) = "Survey no. " & SONo.ToString & " Updated Successfully"
            RetDS.Tables.Add(RetDT)
            Context.Response.Output.Write(fn.ds2json(RetDS))


        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
         <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub Cleaning1_SurveyTyepUpdate(ByVal value As String)
        sFunction = "Cleaning1_SurveyTyepUpdate"
        Try

            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim SQTO As New DataTable
            SQTO = New DataTable
            Dim SQTOGEN As New DataTable
            Dim SQTOADD As New DataTable
            Dim SOTOATTACH As New DataTable
            Dim SQTODS = New DataSet()
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            fn.GetSAPConnection(Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            Dim STypeCode As String = ""
            Dim STypeName As String = ""
            Dim RetCode As Integer = 0
            Dim SONo As String = ""
            Dim errMessage As String = ""
            Dim CardCode As String = ""
            Dim DocDate As String = ""
            Dim ProjectCode As String = ""
            Dim Country As String = ""
            Dim City As String = ""
            Dim EqupTye As String = ""
            Dim Rt As String = ""
            Dim SQLStr As String = ""

            'Dim oSO As SAPbobsCOM.Documents
            'oSO = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
            Dim oDO As SAPbobsCOM.Documents
            oDO = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes)


            If ds.Tables("ODLN").Rows.Count > 0 Then
                SQTO = ds.Tables("ODLN")
                Dim dr As DataRow = SQTO.Rows(0)

                If oDO.GetByKey(dr.Item("U_SurvyNo")) = True Then

                Else
                    Throw New Exception("No Record Found!")
                End If


                oDO.DocDate = DateConvert(dr.Item("DocDate").ToString)
                oDO.UserFields.Fields.Item("U_SResult").Value = dr.Item("U_SResult").ToString.Trim()

                oDO.UserFields.Fields.Item("U_EX_Fram").Value = dr.Item("U_EX_Fram").ToString.Trim()
                oDO.UserFields.Fields.Item("U_EX_Man").Value = dr.Item("U_EX_Man").ToString.Trim()
                oDO.UserFields.Fields.Item("U_EX_Ser").Value = dr.Item("U_EX_Ser").ToString.Trim()
                oDO.UserFields.Fields.Item("U_EX_Car").Value = dr.Item("U_EX_Car").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Free").Value = dr.Item("U_INT_Free").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Clean").Value = dr.Item("U_INT_Clean").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Dry").Value = dr.Item("U_INT_Dry").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Pitt").Value = dr.Item("U_INT_Pitt").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Disc").Value = dr.Item("U_INT_Disc").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Val").Value = dr.Item("U_VAL_Val").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Bott").Value = dr.Item("U_VAL_Bott").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Man").Value = dr.Item("U_VAL_Man").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Syp").Value = dr.Item("U_VAL_Syp").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Tank").Value = dr.Item("U_VAL_Tank").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Avail").Value = dr.Item("U_VAL_Avail").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Steam").Value = dr.Item("U_VAL_Steam").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Gas").Value = dr.Item("U_VAL_Gas").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SEAL_MAN").Value = dr.Item("U_SEAL_MAN").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SEAL_AIR").Value = dr.Item("U_SEAL_AIR").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SEAL_BOTT").Value = dr.Item("U_SEAL_BOTT").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SEAL_LAST").Value = dr.Item("U_SEAL_LAST").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SEAL_NEXT").Value = dr.Item("U_SEAL_NEXT").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_SPIL").Value = dr.Item("U_VAL_SPIL").ToString.Trim()
                oDO.UserFields.Fields.Item("U_PayLoad").Value = dr.Item("U_PayLoad").ToString.Trim()
                oDO.UserFields.Fields.Item("U_Capacity").Value = dr.Item("U_Capacity").ToString.Trim()
                oDO.UserFields.Fields.Item("U_CSCNum").Value = dr.Item("U_CSCNum").ToString.Trim()
                oDO.UserFields.Fields.Item("U_IMO").Value = dr.Item("U_IMO").ToString.Trim()

                Try
                    'oDO.UserFields.Fields.Item("U_DOM").Value = DateConvert(dr.Item("U_DOM").ToString.Trim())
                    oDO.UserFields.Fields.Item("U_DOM").Value = (dr.Item("U_DOM").ToString.Trim())
                Catch ex As Exception
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile("Error while updating DOM", sFunction)
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_MGW").Value = dr.Item("U_MGW").ToString.Trim()
                Catch ex As Exception
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile("Error while updating MGW", sFunction)
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Tare").Value = dr.Item("U_Tare").ToString.Trim()
                Catch ex As Exception
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile("Error while updating Tare", sFunction)
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_ACEP").Value = dr.Item("U_ACEP").ToString.Trim()
                Catch ex As Exception
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile("Error while updating ACEP", sFunction)
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
                End Try
                Try
                    'oDO.UserFields.Fields.Item("U_CSC").Value = DateConvert(dr.Item("U_CSC").ToString.Trim())
                    oDO.UserFields.Fields.Item("U_CSC").Value = (dr.Item("U_CSC").ToString.Trim())
                Catch ex As Exception
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile("Error while updating CSC", sFunction)
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
                End Try

                'oDO.UserFields.Fields.Item("U_SurveyorID").Value = oSO.UserFields.Fields.Item("U_SurveyorID").Value
                oDO.NumAtCard = dr.Item("NumAtCard").ToString.Trim
                'oDO.UserFields.Fields.Item("U_SuvExAgent").Value = dr.Item("U_SuvExAgent").ToString.Trim()
                oDO.UserFields.Fields.Item("U_NoPh").Value = dr.Item("U_NoPh").ToString.Trim()
                oDO.UserFields.Fields.Item("U_EqNo").Value = dr.Item("U_EqNo").ToString.Trim()

                oDO.Comments = dr.Item("Comments").ToString.Trim()

                Dim iAttachEntry As Integer = 0
                Dim bAttchAdd As Boolean = False
                Dim bFileExists As Boolean = False
                If ds.Tables("ATTACHMENT").Rows.Count > 0 Then
                    SOTOATTACH = ds.Tables("ATTACHMENT")
                    Dim oAttach As SAPbobsCOM.Attachments2
                    oAttach = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2)
                    Dim oRecordSet As SAPbobsCOM.Recordset
                    oRecordSet = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

                    If oAttach.GetByKey(oDO.AttachmentEntry) Then
                        For Each odr As DataRow In SOTOATTACH.Rows
                            Dim sSourcePath, sFileName, sFileExt As String
                            sFileName = odr.Item("U_FileName").ToString.Trim()
                            If sFileName <> "" Then
                                sFileExt = Path.GetExtension(sFileName)
                                sFileExt = sFileExt.Replace(".", "")
                                sSourcePath = odr.Item("U_FilePath").ToString.Substring(0, 3)
                                Dim sQuery As String = String.Empty
                                sQuery = "SELECT * FROM ""ATC1"" WHERE ""AbsEntry"" = '" & oDO.AttachmentEntry & "' AND ""FileName"" = '" & sFileName.Replace(Path.GetExtension(sFileName), "") & "' "
                                oRecordSet.DoQuery(sQuery)
                                If oRecordSet.RecordCount > 0 Then
                                Else
                                    oAttach.Lines.Add()
                                    oAttach.Lines.SourcePath = PublicVariable.sTempfilePath
                                    oAttach.Lines.FileName = sFileName.Replace(Path.GetExtension(sFileName), "")
                                    oAttach.Lines.FileExtension = sFileExt
                                    oAttach.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES
                                End If
                            End If
                        Next
                        If oAttach.Update() = 0 Then
                            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug("Attachment updated successfully", sFunction)
                        Else
                            Throw New Exception("Error while updating attachment /" & PublicVariable.oCompany.GetLastErrorDescription)
                        End If
                    Else
                        For Each odr As DataRow In SOTOATTACH.Rows
                            Dim sSourcePath, sFileName, sFileExt As String
                            sFileName = odr.Item("U_FileName").ToString.Trim()
                            If sFileName <> "" Then
                                sFileExt = Path.GetExtension(sFileName)
                                sFileExt = sFileExt.Replace(".", "")
                                sSourcePath = odr.Item("U_FilePath").ToString.Substring(0, 3)
                                oAttach.Lines.SourcePath = PublicVariable.sTempfilePath
                                oAttach.Lines.FileName = sFileName.Replace(Path.GetExtension(sFileName), "")
                                oAttach.Lines.FileExtension = sFileExt
                                oAttach.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES
                                oAttach.Lines.Add()
                                bAttchAdd = True
                            End If
                        Next
                        If bAttchAdd = True Then
                            If oAttach.Add() = 0 Then
                                iAttachEntry = PublicVariable.oCompany.GetNewObjectKey()
                                oDO.AttachmentEntry = iAttachEntry
                            Else
                                Throw New Exception("Error while adding attachment /" & PublicVariable.oCompany.GetLastErrorDescription)
                            End If
                        End If
                    End If
                End If

                RetCode = oDO.Update
                If RetCode <> 0 Then
                    PublicVariable.oCompany.GetLastError(RetCode, errMessage)
                    Throw New Exception(errMessage)
                Else
                    SONo = PublicVariable.oCompany.GetNewObjectKey()
                End If
            Else
                Throw New Exception("No Input Data.")
            End If

            Dim RetDT As New DataTable
            RetDT.TableName = "VALIDATE"
            RetDT.Columns.Add("Status")
            RetDT.Columns.Add("Msg")
            RetDT.Rows.Add()
            RetDT.Rows(0)(0) = True
            RetDT.Rows(0)(1) = "Survey no. " & SONo.ToString & " Updated Successfully"
            RetDS.Tables.Add(RetDT)
            Context.Response.Output.Write(fn.ds2json(RetDS))


        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
         <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub Cleaning1_NewFormat_SurveyTyepUpdate(ByVal value As String)
        sFunction = "Cleaning1_NewFormat_SurveyTyepUpdate"
        Try

            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, sFunction)
            Dim SQTO As New DataTable
            SQTO = New DataTable
            Dim SQTOGEN As New DataTable
            Dim SQTOADD As New DataTable
            Dim SOTOATTACH As New DataTable
            Dim SQTODS = New DataSet()
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            fn.GetSAPConnection(Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            Dim STypeCode As String = ""
            Dim STypeName As String = ""
            Dim RetCode As Integer = 0
            Dim SONo As String = ""
            Dim errMessage As String = ""
            Dim CardCode As String = ""
            Dim DocDate As String = ""
            Dim ProjectCode As String = ""
            Dim Country As String = ""
            Dim City As String = ""
            Dim EqupTye As String = ""
            Dim Rt As String = ""
            Dim SQLStr As String = ""

            'Dim oSO As SAPbobsCOM.Documents
            'oSO = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
            Dim oDO As SAPbobsCOM.Documents
            oDO = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes)


            If ds.Tables("ODLN").Rows.Count > 0 Then
                SQTO = ds.Tables("ODLN")
                Dim dr As DataRow = SQTO.Rows(0)

                If oDO.GetByKey(dr.Item("U_SurvyNo")) = True Then

                Else
                    Throw New Exception("No Record Found!")
                End If

                ' oDO.UserFields.Fields.Item("U_UCode").Value = dr.Item("U_UCode").ToString.Trim()
                'oDO.UserFields.Fields.Item("U_UName").Value = dr.Item("U_UName").ToString.Trim()
                'oDO.UserFields.Fields.Item("U_Cdate").Value = dr.Item("U_Cdate")
                oDO.DocDate = DateConvert(dr.Item("DocDate").ToString)
                'oDO.DocDueDate = DateConvert(dr.Item("DocDate").ToString)
                'oDO.CardCode = oSO.CardCode
                'oDO.CardName = oSO.CardName

                'oDO.UserFields.Fields.Item("U_FormName").Value = dr.Item("U_FormName").ToString.Trim()
                oDO.UserFields.Fields.Item("U_FORMAT").Value = "2"
                oDO.UserFields.Fields.Item("U_SResult").Value = dr.Item("U_SResult").ToString.Trim()

                oDO.UserFields.Fields.Item("U_EX_Fram").Value = dr.Item("U_EX_Fram").ToString.Trim()
                oDO.UserFields.Fields.Item("U_EX_Man").Value = dr.Item("U_EX_Man").ToString.Trim()
                oDO.UserFields.Fields.Item("U_EX_Ser").Value = dr.Item("U_EX_Ser").ToString.Trim()
                oDO.UserFields.Fields.Item("U_EX_Car").Value = dr.Item("U_EX_Car").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Free").Value = dr.Item("U_INT_Free").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Clean").Value = dr.Item("U_INT_Clean").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Dry").Value = dr.Item("U_INT_Dry").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Pitt").Value = dr.Item("U_INT_Pitt").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Disc").Value = dr.Item("U_INT_Disc").ToString.Trim()

                oDO.UserFields.Fields.Item("U_INT_Cleanliness").Value = dr.Item("U_INT_Cleanliness").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Rust").Value = dr.Item("U_INT_Rust").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Deposits").Value = dr.Item("U_INT_Deposits").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_DamagedCoating").Value = dr.Item("U_INT_DamagedCoating").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_GasketMainHole").Value = dr.Item("U_INT_GasketMainHole").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_OilDirtFree").Value = dr.Item("U_INT_OilDirtFree").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_SteamCoil").Value = dr.Item("U_INT_SteamCoil").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Steam_Cleaned").Value = dr.Item("U_INT_Steam_Cleaned").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_GasktDschrgval").Value = dr.Item("U_INT_GasktDschrgval").ToString.Trim()


                oDO.UserFields.Fields.Item("U_VAL_Val").Value = dr.Item("U_VAL_Val").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Bott").Value = dr.Item("U_VAL_Bott").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Man").Value = dr.Item("U_VAL_Man").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Syp").Value = dr.Item("U_VAL_Syp").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Tank").Value = dr.Item("U_VAL_Tank").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Avail").Value = dr.Item("U_VAL_Avail").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Steam").Value = dr.Item("U_VAL_Steam").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_Gas").Value = dr.Item("U_VAL_Gas").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SEAL_MAN").Value = dr.Item("U_SEAL_MAN").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SEAL_AIR").Value = dr.Item("U_SEAL_AIR").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SEAL_BOTT").Value = dr.Item("U_SEAL_BOTT").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SEAL_LAST").Value = dr.Item("U_SEAL_LAST").ToString.Trim()
                oDO.UserFields.Fields.Item("U_SEAL_NEXT").Value = dr.Item("U_SEAL_NEXT").ToString.Trim()
                oDO.UserFields.Fields.Item("U_VAL_SPIL").Value = dr.Item("U_VAL_SPIL").ToString.Trim()
                oDO.UserFields.Fields.Item("U_PayLoad").Value = dr.Item("U_PayLoad").ToString.Trim()
                oDO.UserFields.Fields.Item("U_Capacity").Value = dr.Item("U_Capacity").ToString.Trim()
                oDO.UserFields.Fields.Item("U_CSCNum").Value = dr.Item("U_CSCNum").ToString.Trim()
                oDO.UserFields.Fields.Item("U_IMO").Value = dr.Item("U_IMO").ToString.Trim()
                oDO.UserFields.Fields.Item("U_INT_Cleanliness").Value = dr.Item("U_INT_Cleanliness").ToString.Trim()

                Try
                    'oDO.UserFields.Fields.Item("U_DOM").Value = DateConvert(dr.Item("U_DOM").ToString.Trim())
                    oDO.UserFields.Fields.Item("U_DOM").Value = (dr.Item("U_DOM").ToString.Trim())
                Catch ex As Exception
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile("Error while updating DOM", sFunction)
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_MGW").Value = dr.Item("U_MGW").ToString.Trim()
                Catch ex As Exception
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile("Error while updating MGW", sFunction)
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_Tare").Value = dr.Item("U_Tare").ToString.Trim()
                Catch ex As Exception
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile("Error while updating Tare", sFunction)
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
                End Try
                Try
                    oDO.UserFields.Fields.Item("U_ACEP").Value = dr.Item("U_ACEP").ToString.Trim()
                Catch ex As Exception
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile("Error while updating ACEP", sFunction)
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
                End Try
                Try
                    'oDO.UserFields.Fields.Item("U_CSC").Value = DateConvert(dr.Item("U_CSC").ToString.Trim())
                    oDO.UserFields.Fields.Item("U_CSC").Value = (dr.Item("U_CSC").ToString.Trim())
                Catch ex As Exception
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile("Error while updating CSC", sFunction)
                    If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
                End Try

                'oDO.UserFields.Fields.Item("U_SurveyorID").Value = oSO.UserFields.Fields.Item("U_SurveyorID").Value
                oDO.NumAtCard = dr.Item("NumAtCard").ToString.Trim
                'oDO.UserFields.Fields.Item("U_SuvExAgent").Value = dr.Item("U_SuvExAgent").ToString.Trim()
                oDO.UserFields.Fields.Item("U_NoPh").Value = dr.Item("U_NoPh").ToString.Trim()
                oDO.UserFields.Fields.Item("U_EqNo").Value = dr.Item("U_EqNo").ToString.Trim()

                '  STypeCode = dr.Item("U_STypeCode")
                ' STypeName = dr.Item("U_STypeName")
                'oDO.UserFields.Fields.Item("U_STypeCode").Value = oSO.UserFields.Fields.Item("U_STypeCode").Value
                'oDO.UserFields.Fields.Item("U_U_STypeName").Value = oSO.UserFields.Fields.Item("U_U_STypeName").Value
                'oDO.UserFields.Fields.Item("U_Country").Value = oSO.UserFields.Fields.Item("U_Country").Value
                'oDO.UserFields.Fields.Item("U_City").Value = oSO.UserFields.Fields.Item("U_City").Value
                'oDO.UserFields.Fields.Item("U_Loc").Value = oSO.UserFields.Fields.Item("U_Loc").Value
                oDO.Comments = dr.Item("Comments").ToString.Trim()
                'oDO.Lines.ItemCode = oSO.Lines.ItemCode
                'oDO.Lines.Quantity = 1
                'oDO.Lines.UserFields.Fields.Item("U_PDate").Value = oSO.Lines.UserFields.Fields.Item("U_PDate").Value
                'oDO.Lines.UserFields.Fields.Item("U_EQType").Value = oSO.Lines.UserFields.Fields.Item("U_EQType").Value
                ''EqupTye = dr1.Item("U_EQType").ToString.Trim()
                'oDO.Lines.UserFields.Fields.Item("U_SCriteria").Value = oSO.Lines.UserFields.Fields.Item("U_SCriteria").Value
                'oDO.Lines.BaseType = 17 '23 ' - 'Sales Quotation'
                'oDO.Lines.BaseEntry = dr.Item("U_OrderNo")
                'oDO.Lines.BaseLine = 0

                'oDO.Lines.UnitPrice = oSO.Lines.UnitPrice
                'oDO.Lines.Add()

                Dim iAttachEntry As Integer = 0
                Dim bAttchAdd As Boolean = False
                Dim bFileExists As Boolean = False
                If ds.Tables("ATTACHMENT").Rows.Count > 0 Then
                    SOTOATTACH = ds.Tables("ATTACHMENT")
                    Dim oAttach As SAPbobsCOM.Attachments2
                    oAttach = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2)
                    Dim oRecordSet As SAPbobsCOM.Recordset
                    oRecordSet = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

                    If oAttach.GetByKey(oDO.AttachmentEntry) Then
                        For Each odr As DataRow In SOTOATTACH.Rows
                            Dim sSourcePath, sFileName, sFileExt As String
                            sFileName = odr.Item("U_FileName").ToString.Trim()
                            If sFileName <> "" Then
                                sFileExt = Path.GetExtension(sFileName)
                                sFileExt = sFileExt.Replace(".", "")
                                sSourcePath = odr.Item("U_FilePath").ToString.Substring(0, 3)
                                Dim sQuery As String = String.Empty
                                sQuery = "SELECT * FROM ""ATC1"" WHERE ""AbsEntry"" = '" & oDO.AttachmentEntry & "' AND ""FileName"" = '" & sFileName.Replace(Path.GetExtension(sFileName), "") & "' "
                                oRecordSet.DoQuery(sQuery)
                                If oRecordSet.RecordCount > 0 Then
                                Else
                                    oAttach.Lines.Add()
                                    oAttach.Lines.SourcePath = PublicVariable.sTempfilePath
                                    oAttach.Lines.FileName = sFileName.Replace(Path.GetExtension(sFileName), "")
                                    oAttach.Lines.FileExtension = sFileExt
                                    oAttach.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES
                                End If
                            End If
                        Next
                        If oAttach.Update() = 0 Then
                            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug("Attachment updated successfully", sFunction)
                        Else
                            Throw New Exception("Error while updating attachment /" & PublicVariable.oCompany.GetLastErrorDescription)
                        End If
                    Else
                        For Each odr As DataRow In SOTOATTACH.Rows
                            Dim sSourcePath, sFileName, sFileExt As String
                            sFileName = odr.Item("U_FileName").ToString.Trim()
                            If sFileName <> "" Then
                                sFileExt = Path.GetExtension(sFileName)
                                sFileExt = sFileExt.Replace(".", "")
                                sSourcePath = odr.Item("U_FilePath").ToString.Substring(0, 3)
                                oAttach.Lines.SourcePath = PublicVariable.sTempfilePath
                                oAttach.Lines.FileName = sFileName.Replace(Path.GetExtension(sFileName), "")
                                oAttach.Lines.FileExtension = sFileExt
                                oAttach.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES
                                oAttach.Lines.Add()
                                bAttchAdd = True
                            End If
                        Next
                        If bAttchAdd = True Then
                            If oAttach.Add() = 0 Then
                                iAttachEntry = PublicVariable.oCompany.GetNewObjectKey()
                                oDO.AttachmentEntry = iAttachEntry
                            Else
                                Throw New Exception("Error while adding attachment /" & PublicVariable.oCompany.GetLastErrorDescription)
                            End If
                        End If
                    End If
                End If

                RetCode = oDO.Update
                If RetCode <> 0 Then
                    PublicVariable.oCompany.GetLastError(RetCode, errMessage)
                    Throw New Exception(errMessage)
                Else
                    SONo = PublicVariable.oCompany.GetNewObjectKey()
                End If
            Else
                Throw New Exception("No Input Data.")
            End If

            Dim RetDT As New DataTable
            RetDT.TableName = "VALIDATE"
            RetDT.Columns.Add("Status")
            RetDT.Columns.Add("Msg")
            RetDT.Rows.Add()
            RetDT.Rows(0)(0) = True
            RetDT.Rows(0)(1) = "Survey no. " & SONo.ToString & " Updated Successfully"
            RetDS.Tables.Add(RetDT)
            Context.Response.Output.Write(fn.ds2json(RetDS))


        Catch ex As Exception
            If PublicVariable.ERROR_ON = 1 Then oLog.WriteToLogFile(ex.Message, sFunction)
            fn.ErrorHandling(ex.Message.ToString.Trim())
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub

    <WebMethod()> _
  <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub SurveyTyep_Find(ByVal value As String)
        Try
            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, "SurveyTyep_Find")

            Dim Code As String = ""
            Dim Name As String = ""
            Dim OCRD As New DataTable
            OCRD = New DataTable
            Dim LEADMDS = New DataSet()
            Dim SQLDT As New DataTable
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""
            Dim SoNo As String = ""

            If ds.Tables("ODLN").Rows.Count > 0 Then
                OCRD = ds.Tables("ODLN")
                Dim dr As DataRow = OCRD.Rows(0)
                DocEntry = dr.Item("U_SurvyNo")
            End If


            'Dim Str As String = "SELECT Top 1 T0.""DocEntry"" ""U_SurveyNo"", T0.""DocStatus"" as ""U_Status"", T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY')  As ""U_Cdate"", TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY')  As ""DocDate"", T0.""CardCode"", T0.""CardName"", T0.""U_SurveyorID"" , T0.""NumAtCard"",T1.""ItemCode"" as ""U_STypeCode"", T0.""U_Country"", T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""U_SuvExAgent"",T1.""U_EQType"" as ""U_Eqtype"",  T0.""U_EqNo"", T1.""U_SCriteria"",T0.""U_SResult"",T0.""U_NoPh"", T0.""Comments"",T0.""U_FormName"",TO_CHAR( T0.""U_DOM"" , 'DD/MM/YYYY') As ""U_DOM"",T0.""U_MGW"",T0.""U_Tare"",T0.""U_ACEP"",TO_CHAR( T0.""U_CSC"" , 'DD/MM/YYYY') As ""U_CSC"",""U_EX_Fram"",""U_EX_Man"",""U_EX_Ser"",""U_EX_Car"",""U_INT_Free"",""U_INT_Clean"",""U_INT_Dry"",""U_INT_Pitt"",""U_INT_Disc"",""U_VAL_Val"",""U_VAL_Bott"",""U_VAL_Man"",""U_VAL_Syp"",""U_VAL_Tank"",""U_VAL_Avail"",""U_VAL_Steam"",""U_VAL_Gas"",""U_SEAL_MAN"",""U_SEAL_AIR"",""U_SEAL_BOTT"",""U_SEAL_LAST"",""U_SEAL_NEXT"",""U_SEAL_NEXT"" FROM ODLN T0  INNER JOIN DLN1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" WHERE T0.""DocEntry"" ='" & DocEntry & "'"
            Dim Str As String = String.Empty
            'Str = "SELECT Top 1 T0.""DocEntry"" ""U_SurveyNo"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY')  As ""U_Cdate"", " & _
            '      " TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY')  As ""DocDate"", T0.""CardCode"", T0.""CardName"", T0.""U_SurveyorID"" , T0.""NumAtCard"",T1.""ItemCode"" as ""U_STypeCode"", " & _
            '      " T0.""U_Country"", T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""U_SuvExAgent"",T1.""U_EQType"" as ""U_Eqtype"",  T0.""U_EqNo"", T1.""U_SCriteria"", " & _
            '      " T0.""U_SResult"",T0.""U_NoPh"", T0.""Comments"",T0.""U_FormName"",T0.""U_DOM"",T0.""U_MGW"",T0.""U_Tare"",T0.""U_ACEP"",T0.""U_CSCNum"",T0.""U_CSC"",""U_EX_Fram"",T0.""U_IMO"", " & _
            '      " ""U_EX_Man"",""U_EX_Ser"",""U_EX_Car"",""U_INT_Free"",""U_INT_Clean"",""U_INT_Dry"",""U_INT_Pitt"",""U_INT_Disc"",""U_VAL_Val"",""U_VAL_Bott"",""U_VAL_SPIL"",""U_Capacity"", " & _
            '      " ""U_VAL_Man"",""U_VAL_Syp"",""U_VAL_Tank"",""U_VAL_Avail"",""U_VAL_Steam"",""U_VAL_Gas"",""U_SEAL_MAN"",""U_SEAL_AIR"",""U_SEAL_BOTT"",""U_SEAL_LAST"",""U_PayLoad"", " & _
            '      " ""U_SEAL_NEXT"",""U_SEAL_NEXT"" FROM ODLN T0  INNER JOIN DLN1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" WHERE T0.""DocEntry"" ='" & DocEntry & "'"
            Str = "SELECT Top 1 T0.""DocEntry"" ""U_SurveyNo"",T0.""DocNum"",T1.""BaseEntry"" AS ""U_OrderNo"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY')  As ""U_Cdate"", " & _
                  " TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY')  As ""DocDate"", T0.""CardCode"", T0.""CardName"", T0.""U_SurveyorID"" , T0.""NumAtCard"",T1.""ItemCode"" as ""U_STypeCode"", " & _
                  " T0.""U_Country"", T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""U_SuvExAgent"",T1.""U_EQType"" as ""U_Eqtype"",  T0.""U_EqNo"", T1.""U_SCriteria"", " & _
                  " T0.""U_SResult"",T0.""U_NoPh"", T0.""Comments"",T0.""U_FormName"",T0.""U_DOM"",T0.""U_MGW"",T0.""U_Tare"",T0.""U_ACEP"",T0.""U_CSCNum"",T0.""U_CSC"",""U_EX_Fram"",T0.""U_IMO"", " & _
                  " ""U_EX_Man"",""U_EX_Ser"",""U_EX_Car"",""U_INT_Free"",""U_INT_Clean"",""U_INT_Dry"",""U_INT_Pitt"",""U_INT_Disc"",""U_VAL_Val"",""U_VAL_Bott"",""U_VAL_SPIL"",""U_Capacity"", " & _
                  " ""U_VAL_Man"",""U_VAL_Syp"",""U_VAL_Tank"",""U_VAL_Avail"",""U_VAL_Steam"",""U_VAL_Gas"",""U_SEAL_MAN"",""U_SEAL_AIR"",""U_SEAL_BOTT"",""U_SEAL_LAST"",""U_PayLoad"", " & _
                  " ""U_INT_Cleanliness"",""U_INT_Rust"",""U_INT_Deposits"",""U_INT_DamagedCoating"",""U_INT_GasketMainHole"",""U_INT_OilDirtFree"",""U_INT_SteamCoil"",""U_INT_Steam_Cleaned"",""U_INT_GasktDschrgval"", " & _
                  " ""U_SEAL_NEXT"",""U_SEAL_NEXT"",T2.""U_HYPERLINK_BEFORE"" AS ""U_HLinkBef"",T2.""U_HYPERLINK_AFTER"" AS ""U_HLinkAft"",IFNULL(T0.""U_FORMAT"",1) AS ""FormatNo"", " & _
                  " T0.""U_Esti_Org"",T0.""U_Esti_Rev"",T0.""U_Esti_Diff"",T0.""U_Curr_Org"",T0.""U_Curr_Rev"",T0.""U_Curr_diff"",T0.""U_Impac_Org"",T0.""U_Impac_Rev"", " & _
                  " T0.""U_Impac_diff"",T0.""U_Wear_org"",T0.""U_Wear_Rev"",T0.""U_Wear_Diff"",T0.""U_Cust_Org"",T0.""U_Cust_Rev"",T0.""U_Cust_Diff"",T0.""U_Impor_Org"", " & _
                  " T0.""U_Impor_Rev"",T0.""U_Import_Diff"",T0.""U_Sum_Org"",T0.""U_Sum_Rev"",T0.""U_Sum_Diff"" " & _
                  " FROM ODLN T0 " & _
                  " INNER JOIN DLN1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" " & _
                  " LEFT JOIN ""@SURVEYTYPE_HLINK"" T2 ON UPPER(T2.""U_SURVEYTYPENAME"") = UPPER(T1.""Dscription"") " & _
                  " WHERE T0.""DocEntry"" ='" & DocEntry & "'"

            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable
            Dim RetDT2 As New DataTable
            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "ODLN"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                'Str = "SELECT T0.""DocEntry"" AS ""Survey_No"", T0.""U_UName"" As ""User_Name"",TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY') ""Survey_Date"", T0.""CardCode"" as ""Customer_Code"", T0.""CardName"" as ""Customer_Name"", T1.""Dscription"" As ""Survey_Type"", T0.""U_Loc"" As ""Location"" FROM ODLN T0  INNER JOIN DLN1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" WHERE T1.""BaseType"" =17 and  T1.""BaseEntry"" ='" & DocEntry & "'"

                'RetDT = New DataTable
                'RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
                'If Errmsg <> "" Then
                '    Throw New Exception(Errmsg)
                'End If
                'RetDT.TableName = "DETAILS"
                'RetDT2 = RetDT.Copy
                'RetDS.Tables.Add(RetDT2)

                RetDT = New DataTable
                Dim Query As String = String.Empty
                Query = "SELECT T0.""DocEntry"", T1.""FileName"" ||'.'|| T1.""FileExt"" AS ""U_FileName"",T1.""trgtPath""||'\'||T1.""FileName"" ||'.'|| T1.""FileExt"" AS ""U_FilePath"", " & _
                        " T1.""Line"" AS ""U_id"", TO_CHAR(T1.""Date"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                        " FROM ""ODLN"" T0 INNER JOIN ""ATC1"" T1 ON T1.""AbsEntry"" =  T0.""AtcEntry"" " & _
                        " WHERE T0.""DocEntry"" = '" & DocEntry & "' "
                If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(Query, "SurveyTyep_Find")
                RetDT = fn.ExecuteSQLQuery(Query, Errmsg)
                If Errmsg <> "" Then
                    Throw New Exception(Errmsg)
                End If
                RetDT.TableName = "ATTACHMENT"
                RetDT2 = RetDT.Copy
                RetDS.Tables.Add(RetDT2)

                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
 <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub DamageHistory(ByVal value As String)
        Try
            Dim EquNo As String = ""
            Dim OCRD As New DataTable
            OCRD = New DataTable
            Dim LEADMDS = New DataSet()
            Dim SQLDT As New DataTable
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""
            Dim SoNo As String = ""

            If ds.Tables("ODLN").Rows.Count > 0 Then
                OCRD = ds.Tables("ODLN")
                Dim dr As DataRow = OCRD.Rows(0)
                EquNo = dr.Item("U_EqNo")
            End If


            Dim Str As String = "SELECT Top 20 T0.""DocEntry"" as U_SurvyNo,TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY')  As ""DocDate"", T0.""U_SurveyorID"",T1.""Dscription"" U_SurveyType, T0.""U_Country"", T0.""U_City"", T0.""U_Loc"", T0.""NumAtCard"" Cust_ref, T0.""U_SResult"" FROM ODLN T0  INNER JOIN DLN1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" WHERE T0.""U_EqNo"" ='" & EquNo & "' ORDER BY T0.""DocDate"" desc"

            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable
            Dim RetDT2 As New DataTable
            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "ODLN"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                'Str = "SELECT T0.""DocEntry"" AS ""Survey_No"", T0.""U_UName"" As ""User_Name"",TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY') ""Survey_Date"", T0.""CardCode"" as ""Customer_Code"", T0.""CardName"" as ""Customer_Name"", T1.""Dscription"" As ""Survey_Type"", T0.""U_Loc"" As ""Location"" FROM ODLN T0  INNER JOIN DLN1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" WHERE T1.""BaseType"" =17 and  T1.""BaseEntry"" ='" & DocEntry & "'"

                'RetDT = New DataTable
                'RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
                'If Errmsg <> "" Then
                '    Throw New Exception(Errmsg)
                'End If
                'RetDT.TableName = "DETAILS"
                'RetDT2 = RetDT.Copy
                'RetDS.Tables.Add(RetDT2)

                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
<ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub DamageHistory_Full(ByVal value As String)
        Try
            Dim EquNo As String = ""
            Dim OCRD As New DataTable
            OCRD = New DataTable
            Dim LEADMDS = New DataSet()
            Dim SQLDT As New DataTable
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""
            Dim SoNo As String = ""

            If ds.Tables("ODLN").Rows.Count > 0 Then
                OCRD = ds.Tables("ODLN")
                Dim dr As DataRow = OCRD.Rows(0)
                EquNo = dr.Item("U_EqNo")
            End If


            Dim Str As String = "SELECT Top 1000 T0.""DocEntry"" as U_SurvyNo,TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY')  As ""DocDate"", T0.""U_SurveyorID"",T1.""Dscription"" U_SurveyType, T0.""U_Country"", T0.""U_City"", T0.""U_Loc"", T0.""NumAtCard"" Cust_ref, T0.""U_SResult"" FROM ODLN T0  INNER JOIN DLN1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" WHERE T0.""U_EqNo"" ='" & EquNo & "' ORDER BY T0.""DocDate"" desc"

            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable
            Dim RetDT2 As New DataTable
            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "ODLN"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                'Str = "SELECT T0.""DocEntry"" AS ""Survey_No"", T0.""U_UName"" As ""User_Name"",TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY') ""Survey_Date"", T0.""CardCode"" as ""Customer_Code"", T0.""CardName"" as ""Customer_Name"", T1.""Dscription"" As ""Survey_Type"", T0.""U_Loc"" As ""Location"" FROM ODLN T0  INNER JOIN DLN1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" WHERE T1.""BaseType"" =17 and  T1.""BaseEntry"" ='" & DocEntry & "'"

                'RetDT = New DataTable
                'RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
                'If Errmsg <> "" Then
                '    Throw New Exception(Errmsg)
                'End If
                'RetDT.TableName = "DETAILS"
                'RetDT2 = RetDT.Copy
                'RetDS.Tables.Add(RetDT2)

                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
  <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub SalesOrder_SurveyDetails(ByVal value As String)
        Try
            If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(value, "SalesOrder_SurveyDetails")

            Dim Code As String = ""
            Dim Name As String = ""
            Dim OCRD As New DataTable
            OCRD = New DataTable
            Dim LEADMDS = New DataSet()
            Dim SQLDT As New DataTable
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""
            Dim SoNo As String = ""
            Dim iFormCount As Integer = 0

            If ds.Tables("ORDR").Rows.Count > 0 Then
                OCRD = ds.Tables("ORDR")
                Dim dr As DataRow = OCRD.Rows(0)
                DocEntry = dr.Item("U_OrderNo")
            End If

            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable
            Dim RetDT2 As New DataTable
            Dim RetDT3 As New DataTable
            Dim iItemCount As String = String.Empty

            'Dim Str As String = "SELECT Top 1  T0.""DocEntry"", T0.""OpenQty"",IFNULL(T1.""U_FormLink"",'AOS') ""FormName"" FROM RDR1 T0 LEFT JOIN OITM T1 on T0.""ItemCode""=T1.""ItemCode"" WHERE T0.""DocEntry"" ='" & DocEntry & "'"
            Dim Str As String = String.Empty

            RetDT = New DataTable
            Str = "SELECT COUNT(""ItemCode"") AS ""ItemCount"" FROM(SELECT 'OFH' AS ""ItemCode"" FROM DUMMY UNION SELECT 'ONH' AS ""ItemCode"" FROM DUMMY)T " & _
                  " WHERE ""ItemCode"" = (SELECT TOP 1 ""ItemCode"" FROM ""RDR1"" WHERE ""DocEntry"" = '" & DocEntry & "')"
            iItemCount = fn.ExecuteSQLQuery_SingleValue(Str, sErrDesc)

            RetDT = New DataTable
            Str = "SELECT Top 1  T0.""DocEntry"",T0.""ItemCode"", T0.""OpenQty"",IFNULL(T1.""U_FormLink"",'AOS') ""FormName"",IFNULL(COUNT(T2.""U_SURVEYTYPE""),0) AS ""FormatNo"",'" & iItemCount & "' AS ""ItemCount"" " & _
                                " FROM RDR1 T0 " & _
                                " LEFT JOIN OITM T1 on T0.""ItemCode""=T1.""ItemCode"" " & _
                                " LEFT JOIN ""@SURVEYTYPE_FORM"" T2 ON UPPER(T2.""U_SURVEYTYPE"") = UPPER(T1.""ItemCode"") WHERE T0.""DocEntry"" ='" & DocEntry & "' " & _
                                " GROUP BY T0.""DocEntry"",T0.""ItemCode"", T0.""OpenQty"",T1.""U_FormLink"" "

            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "OCRD"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)
                'Str = "SELECT T0.""DocEntry"" AS ""Survey_No"",T0.""U_EqNo"" AS ""Container_Num"", T0.""U_SResult"" As ""Survey_Result"" ,T0.""U_UName"" As ""User_Name"", " & _
                '      " TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY') ""Survey_Date"", T0.""CardCode"" as ""Customer_Code"", T0.""CardName"" as ""Customer_Name"", " & _
                '      " T1.""Dscription"" As ""Survey_Type"", T0.""U_Loc"" As ""Location"" FROM ODLN T0  INNER JOIN DLN1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" " & _
                '      " WHERE T1.""BaseType"" =17 and  T1.""BaseEntry"" ='" & DocEntry & "' ORDER BY T0.""DocEntry"" "
                Str = "SELECT T0.""DocEntry"" AS ""Survey_No"",T0.""DocNum"",T0.""U_EqNo"" AS ""Container_Num"", T0.""U_SResult"" As ""Survey_Result"" ,T0.""U_UName"" As ""User_Name"", " & _
                      " CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"",IFNULL(T0.""U_FORMAT"",1) AS ""FormatNo"", " & _
                      " TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY') ""Survey_Date"", T0.""CardCode"" as ""Customer_Code"", T0.""CardName"" as ""Customer_Name"",T0.""U_IMO"", " & _
                      " T1.""Dscription"" As ""Survey_Type"", T0.""U_Loc"" As ""Location"",T2.""U_HYPERLINK_BEFORE"" AS ""U_HLinkBef"",T2.""U_HYPERLINK_AFTER"" AS ""U_HLinkAft"", " & _
                      " T0.""U_Esti_Org"",T0.""U_Esti_Rev"",T0.""U_Esti_Diff"",T0.""U_Curr_Org"",T0.""U_Curr_Rev"",T0.""U_Curr_diff"",T0.""U_Impac_Org"",T0.""U_Impac_Rev"", " & _
                      " T0.""U_Impac_diff"",T0.""U_Wear_org"",T0.""U_Wear_Rev"",T0.""U_Wear_Diff"",T0.""U_Cust_Org"",T0.""U_Cust_Rev"",T0.""U_Cust_Diff"",T0.""U_Impor_Org"", " & _
                      " T0.""U_Impor_Rev"",T0.""U_Import_Diff"",T0.""U_Sum_Org"",T0.""U_Sum_Rev"",T0.""U_Sum_Diff"" " & _
                      " FROM ODLN T0  INNER JOIN DLN1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" " & _
                      " LEFT JOIN ""@SURVEYTYPE_HLINK"" T2 ON UPPER(T2.""U_SURVEYTYPENAME"") = UPPER(T1.""Dscription"") " & _
                      " WHERE T1.""BaseType"" =17 and  T1.""BaseEntry"" ='" & DocEntry & "' ORDER BY T0.""DocEntry"" "

                RetDT = New DataTable
                RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
                If Errmsg <> "" Then
                    Throw New Exception(Errmsg)
                End If
                RetDT.TableName = "DETAILS"
                RetDT2 = RetDT.Copy
                RetDS.Tables.Add(RetDT2)


                RetDT = New DataTable
                Dim Query As String = String.Empty
                Query = "SELECT T0.""DocEntry"", T1.""FileName"" ||'.'|| T1.""FileExt"" AS ""U_FileName"",T1.""trgtPath""||'\'||T1.""FileName"" ||'.'|| T1.""FileExt"" AS ""U_FilePath"", " & _
                        " T1.""Line"" AS ""U_id"", TO_CHAR(T1.""Date"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                        " FROM ""ODLN"" T0 INNER JOIN ""ATC1"" T1 ON T1.""AbsEntry"" =  T0.""AtcEntry"" " & _
                        " INNER JOIN ""DLN1"" T2 ON T2.""DocEntry"" = T0.""DocEntry"" " & _
                        " WHERE T2.""BaseEntry"" = '" & DocEntry & "' "
                RetDT = fn.ExecuteSQLQuery(Query, Errmsg)
                If Errmsg <> "" Then
                    Throw New Exception(Errmsg)
                End If
                RetDT.TableName = "ATTACHMENT"
                RetDT2 = RetDT.Copy
                RetDS.Tables.Add(RetDT2)


                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
  <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub GetSurveyFormat(ByVal value As String)
        Try
            Dim Code As String = ""
            Dim Name As String = ""
            Dim OCRD As New DataTable
            OCRD = New DataTable
            Dim LEADMDS = New DataSet()
            Dim SQLDT As New DataTable
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""
            Dim SoNo As String = ""
            Dim iFormCount As Integer = 0

            If ds.Tables("ODLN").Rows.Count > 0 Then
                OCRD = ds.Tables("ODLN")
                Dim dr As DataRow = OCRD.Rows(0)
                DocEntry = dr.Item("U_SurvyNo")
            End If


            Dim Str As String
            Str = "SELECT Top 1 T0.""DocEntry"" ""U_SurveyNo"",T0.""DocNum"",T1.""BaseEntry"" AS ""U_OrderNo"",CASE WHEN T0.""DocStatus"" = 'O' THEN 'Open' WHEN T0.""DocStatus"" = 'C' THEN 'Closed' END as ""U_Status"", T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY')  As ""U_Cdate"", " & _
                  " TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY')  As ""DocDate"", T0.""CardCode"", T0.""CardName"", T0.""U_SurveyorID"" , T0.""NumAtCard"",T1.""ItemCode"" as ""U_STypeCode"", " & _
                  " T0.""U_Country"", T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""U_SuvExAgent"",T1.""U_EQType"" as ""U_Eqtype"",  T0.""U_EqNo"", T1.""U_SCriteria"", " & _
                  " T0.""U_SResult"",T0.""U_NoPh"", T0.""Comments"",T0.""U_FormName"",T0.""U_DOM"",T0.""U_MGW"",T0.""U_Tare"",T0.""U_ACEP"",T0.""U_CSCNum"",T0.""U_CSC"",""U_EX_Fram"",T0.""U_IMO"", " & _
                  " ""U_EX_Man"",""U_EX_Ser"",""U_EX_Car"",""U_INT_Free"",""U_INT_Clean"",""U_INT_Dry"",""U_INT_Pitt"",""U_INT_Disc"",""U_VAL_Val"",""U_VAL_Bott"",""U_VAL_SPIL"",""U_Capacity"", " & _
                  " ""U_INT_Cleanliness"",""U_INT_Rust"",""U_INT_Deposits"",""U_INT_DamagedCoating"",""U_INT_GasketMainHole"",""U_INT_OilDirtFree"",""U_INT_SteamCoil"",""U_INT_Steam_Cleaned"",""U_INT_GasktDschrgval"", " & _
                  " ""U_VAL_Man"",""U_VAL_Syp"",""U_VAL_Tank"",""U_VAL_Avail"",""U_VAL_Steam"",""U_VAL_Gas"",""U_SEAL_MAN"",""U_SEAL_AIR"",""U_SEAL_BOTT"",""U_SEAL_LAST"",""U_PayLoad"", " & _
                  " ""U_SEAL_NEXT"",""U_SEAL_NEXT"",T2.""U_HYPERLINK_BEFORE"" AS ""U_HLinkBef"",T2.""U_HYPERLINK_AFTER"" AS ""U_HLinkAft"",IFNULL(T0.""U_FORMAT"",1) AS ""FormatNo"" " & _
                  " FROM ODLN T0 " & _
                  " INNER JOIN DLN1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" " & _
                  " LEFT JOIN ""@SURVEYTYPE_HLINK"" T2 ON UPPER(T2.""U_SURVEYTYPENAME"") = UPPER(T1.""Dscription"") " & _
                  " WHERE T0.""DocEntry"" ='" & DocEntry & "'"

            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable
            Dim RetDT2 As New DataTable
            Dim RetDT3 As New DataTable
            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "ODLN"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)

                RetDT = New DataTable
                Dim Query As String = String.Empty
                Query = "SELECT T0.""DocEntry"", T1.""FileName"" ||'.'|| T1.""FileExt"" AS ""U_FileName"",T1.""trgtPath""||'\'||T1.""FileName"" ||'.'|| T1.""FileExt"" AS ""U_FilePath"", " & _
                        " T1.""Line"" AS ""U_id"", TO_CHAR(T1.""Date"", 'DD/MM/YYYY') AS ""U_Date"" " & _
                        " FROM ""ODLN"" T0 INNER JOIN ""ATC1"" T1 ON T1.""AbsEntry"" =  T0.""AtcEntry"" " & _
                        " WHERE T0.""DocEntry"" = '" & DocEntry & "' "
                If PublicVariable.DEBUG_ON = 1 Then oLog.WriteToLogFile_Debug(Query, "SurveyTyep_Find")
                RetDT = fn.ExecuteSQLQuery(Query, Errmsg)
                If Errmsg <> "" Then
                    Throw New Exception(Errmsg)
                End If
                RetDT.TableName = "ATTACHMENT"
                RetDT2 = RetDT.Copy
                RetDS.Tables.Add(RetDT2)

                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub

#End Region
#Region "Survey List"
    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub GetSurveyNo(ByVal value As String)
        Dim OCRD As New DataTable
        OCRD = New DataTable
        Dim ds As DataSet = fn.jsontodata(value)
        Dim Errmsg As String = ""
        Dim RetDS = New DataSet()
        Dim sSurveyNo As String = String.Empty
        Dim sUserCode As String = String.Empty

        Try
            If ds.Tables("ODLN").Rows.Count > 0 Then
                OCRD = ds.Tables("ODLN")
                Dim dr As DataRow = OCRD.Rows(0)
                sSurveyNo = dr.Item("U_SurvyNo").ToString.Trim()
                sUserCode = dr.Item("uid").ToString.Trim().ToUpper
            End If

            Dim sSQL As String = String.Empty
            Dim DType As String = String.Empty
            Dim sCompCode As String = String.Empty

            If sSurveyNo = "" Then
                sSurveyNo = "%"
            ElseIf sSurveyNo.Contains("*") = True Then
                sSurveyNo = sSurveyNo.Replace("*", "%")
            Else
                sSurveyNo = "%" & sSurveyNo & "%"
            End If

            sSQL = "SELECT T0.""U_DAuthor"",ifnull(T0.""U_ComCode"",'')  As ""U_ComCode"" FROM ""@WUSER""  T0 WHERE Upper(T0.""Code"") ='" & sUserCode & "'"
            Dim dt As New DataTable
            dt = fn.ExecuteSQLQuery(sSQL, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If dt.Rows.Count > 0 Then
                Dim dr As DataRow = dt.Rows(0)
                DType = dr.Item("U_DAuthor").ToString.Trim()
                sCompCode = dr.Item("U_ComCode").ToString.Trim()
            End If

            Dim Str As String = String.Empty
            If DType = "By Company" Then
                If sCompCode = "" Then
                    Throw New Exception("No Record Found!")
                End If
                If sSurveyNo = "" Then
                    Str = "SELECT T0.""DocNum"" AS ""U_SurvyNo"" FROM ODLN T0 " & _
                          " WHERE T0.""U_UCode"" in (SELECT T0.""Code"" FROM ""@WUSER""  T0 WHERE T0.""U_ComCode""  ='" & sCompCode & "') ORDER BY T0.""DocNum"" ASC"
                Else
                    Str = "SELECT T0.""DocNum"" AS ""U_SurvyNo"" FROM ODLN T0 WHERE T0.""DocNum"" like '" & sSurveyNo & "' " & _
                          " AND T0.""U_UCode"" in (SELECT T0.""Code"" FROM ""@WUSER""  T0 WHERE T0.""U_ComCode""  ='" & sCompCode & "') ORDER BY T0.""DocNum"" ASC"
                End If
            ElseIf DType = "By Country" Then
                If sSurveyNo = "" Then
                    Str = "SELECT T0.""DocNum"" AS ""U_SurvyNo"" FROM ODLN T0 " & _
                         " WHERE T0.""CardCode"" in (SELECT DISTINCT ""CardCode"" FROM ""CRD1"" WHERE ""Country"" IN (SELECT ""U_CCode"" FROM ""@COUNTRY"")) ORDER BY T0.""DocNum"" ASC"
                Else
                    Str = "SELECT T0.""DocNum"" AS ""U_SurvyNo"" FROM ODLN T0 WHERE T0.""DocNum"" like '" & sSurveyNo & "' " & _
                         " AND T0.""CardCode"" in (SELECT DISTINCT ""CardCode"" FROM ""CRD1"" WHERE ""Country"" IN (SELECT ""U_CCode"" FROM ""@COUNTRY"")) ORDER BY T0.""DocNum"" ASC"
                End If

            ElseIf DType = "" Then
                Throw New Exception("No Record Found!")
            End If

            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable
            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "ODLN"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)

                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub GetSalesOrderNo(ByVal value As String)
        Dim OCRD As New DataTable
        OCRD = New DataTable
        Dim ds As DataSet = fn.jsontodata(value)
        Dim Errmsg As String = ""
        Dim RetDS = New DataSet()
        Dim sOrderNo As String = String.Empty
        Dim sUserCode As String = String.Empty

        Try
            If ds.Tables("ODLN").Rows.Count > 0 Then
                OCRD = ds.Tables("ODLN")
                Dim dr As DataRow = OCRD.Rows(0)
                sOrderNo = dr.Item("U_OrderNo").ToString.Trim()
                sUserCode = dr.Item("uid").ToString.Trim().ToUpper
            End If

            Dim sSQL As String = String.Empty
            Dim DType As String = String.Empty
            Dim sCompCode As String = String.Empty

            If sOrderNo = "" Then
                sOrderNo = "%"
            ElseIf sOrderNo.Contains("*") = True Then
                sOrderNo = sOrderNo.Replace("*", "%")
            Else
                sOrderNo = "%" & sOrderNo & "%"
            End If

            sSQL = "SELECT T0.""U_DAuthor"",ifnull(T0.""U_ComCode"",'')  As ""U_ComCode"" FROM ""@WUSER""  T0 WHERE Upper(T0.""Code"") ='" & sUserCode & "'"
            Dim dt As New DataTable
            dt = fn.ExecuteSQLQuery(sSQL, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If dt.Rows.Count > 0 Then
                Dim dr As DataRow = dt.Rows(0)
                DType = dr.Item("U_DAuthor").ToString.Trim()
                sCompCode = dr.Item("U_ComCode").ToString.Trim()
            End If

            Dim Str As String = String.Empty
            If DType = "By Company" Then
                If sCompCode = "" Then
                    Throw New Exception("No Record Found!")
                End If
                If sOrderNo = "" Then
                    Str = "SELECT T0.""DocNum"" AS ""U_OrderNo"" FROM ORDR T0 " & _
                          " WHERE T0.""U_UCode"" in (SELECT T0.""Code"" FROM ""@WUSER""  T0 WHERE T0.""U_ComCode""  ='" & sCompCode & "') ORDER BY T0.""DocNum"" ASC"
                Else
                    Str = "SELECT T0.""DocNum"" AS ""U_OrderNo"" FROM ORDR T0 WHERE T0.""DocNum"" like '" & sOrderNo & "' " & _
                          " AND T0.""U_UCode"" in (SELECT T0.""Code"" FROM ""@WUSER""  T0 WHERE T0.""U_ComCode""  ='" & sCompCode & "') ORDER BY T0.""DocNum"" ASC"
                End If
            ElseIf DType = "By Country" Then
                If sOrderNo = "" Then
                    Str = "SELECT T0.""DocNum"" AS ""U_OrderNo"" FROM ORDR T0 " & _
                         " WHERE T0.""CardCode"" in (SELECT DISTINCT ""CardCode"" FROM ""CRD1"" WHERE ""Country"" IN (SELECT ""U_CCode"" FROM ""@COUNTRY"")) ORDER BY T0.""DocNum"" ASC"
                Else
                    Str = "SELECT T0.""DocNum"" AS ""U_OrderNo"" FROM ORDR T0 WHERE T0.""DocNum"" like '" & sOrderNo & "' " & _
                         " AND T0.""CardCode"" in (SELECT DISTINCT ""CardCode"" FROM ""CRD1"" WHERE ""Country"" IN (SELECT ""U_CCode"" FROM ""@COUNTRY"")) ORDER BY T0.""DocNum"" ASC"
                End If

            ElseIf DType = "" Then
                Throw New Exception("No Record Found!")
            End If

            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable
            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "ODLN"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)

                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
    <WebMethod()> _
  <ScriptMethod(ResponseFormat:=ResponseFormat.Json)> _
    Public Sub GetSurveyList(ByVal value As String)
        Try
            Dim Code As String = ""
            Dim Name As String = ""
            Dim OCRD As New DataTable
            OCRD = New DataTable
            Dim LEADMDS = New DataSet()
            Dim SQLDT As New DataTable
            Dim RetDS = New DataSet()
            Dim ds As DataSet = fn.jsontodata(value)
            Dim Errmsg As String = ""
            Dim sErrDesc As String = ""
            Dim SoNo As String = ""
            Dim sSurveyNo As String = String.Empty
            Dim sOrderNo As String = String.Empty
            Dim sCustCode As String = String.Empty
            Dim sCustName As String = String.Empty
            Dim sSurveyDtFrom As String = String.Empty
            Dim sSurveyDtTo As String = String.Empty
            Dim sContainerNo As String = String.Empty

            If ds.Tables("ODLN").Rows.Count > 0 Then
                OCRD = ds.Tables("ODLN")
                Dim dr As DataRow = OCRD.Rows(0)
                sSurveyNo = dr.Item("U_SurvyNo").ToString.Trim()
                sOrderNo = dr.Item("U_OrderNo").ToString.Trim()
                sCustCode = dr.Item("U_Ccode").ToString.Trim()
                sCustName = dr.Item("U_Cname").ToString.Trim()
                sSurveyDtFrom = dr.Item("U_SrvyDatFrm").ToString.Trim()
                sSurveyDtTo = dr.Item("U_SrvyDatTo").ToString.Trim()
                sContainerNo = dr.Item("U_ContainerNo").ToString.Trim()
            End If

            If sSurveyNo = "" Then
                sSurveyNo = "%"
            ElseIf sSurveyNo.Contains("*") = True Then
                sSurveyNo = sSurveyNo.Replace("*", "%")
            Else
                sSurveyNo = "%" & sSurveyNo & "%"
            End If

            If sOrderNo = "" Then
                sOrderNo = "%"
            ElseIf sOrderNo.Contains("*") = True Then
                sOrderNo = sOrderNo.Replace("*", "%")
            Else
                sOrderNo = "%" & sOrderNo & "%"
            End If

            If sCustCode = "" Then
                sCustCode = "%"
            ElseIf sCustCode.Contains("*") = True Then
                sCustCode = sCustCode.Replace("*", "%")
            Else
                sCustCode = "%" & sCustCode & "%"
            End If

            If sCustName = "" Then
                sCustName = "%"
            ElseIf sCustName.Contains("*") = True Then
                sCustName = sCustName.Replace("*", "%")
            Else
                sCustName = "%" & sCustName & "%"
            End If

            If sSurveyDtFrom = "" Then
                sSurveyDtFrom = "%"
            ElseIf sSurveyDtFrom.Contains("*") = True Then
                sSurveyDtFrom = sSurveyDtFrom.Replace("*", "%")
            Else
                sSurveyDtFrom = "%" & sSurveyDtFrom & "%"
            End If

            If sSurveyDtTo = "" Then
                sSurveyDtTo = "%"
            ElseIf sSurveyDtTo.Contains("*") = True Then
                sSurveyDtTo = sSurveyDtTo.Replace("*", "%")
            Else
                sSurveyDtTo = "%" & sSurveyDtTo & "%"
            End If

            If sContainerNo = "" Then
                sContainerNo = "%"
            ElseIf sContainerNo.Contains("*") = True Then
                sContainerNo = sContainerNo.Replace("*", "%")
            Else
                sContainerNo = "%" & sContainerNo & "%"
            End If

            Dim dSurveyDtFrom As Date = GetDateTimeValue(sSurveyDtFrom)
            Dim dSurveyDtTo As Date = GetDateTimeValue(sSurveyDtTo)

            'Dim Str As String = "SELECT Top 1 T0.""DocEntry"" ""U_SurveyNo"", T0.""DocStatus"" as ""U_Status"", T0.""U_UName"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY')  As ""U_Cdate"", TO_CHAR( T0.""DocDate"" , 'DD/MM/YYYY')  As ""DocDate"", T0.""CardCode"", T0.""CardName"", T0.""U_SurveyorID"" , T0.""NumAtCard"",T1.""ItemCode"" as ""U_STypeCode"", T0.""U_Country"", T0.""U_City"", T0.""U_Loc"", T0.""Project"", T0.""U_SuvExAgent"",T1.""U_EQType"" as ""U_Eqtype"",  T0.""U_EqNo"", T1.""U_SCriteria"",T0.""U_SResult"",T0.""U_NoPh"", T0.""Comments"",T0.""U_FormName"",TO_CHAR( T0.""U_DOM"" , 'DD/MM/YYYY') As ""U_DOM"",T0.""U_MGW"",T0.""U_Tare"",T0.""U_ACEP"",TO_CHAR( T0.""U_CSC"" , 'DD/MM/YYYY') As ""U_CSC"",""U_EX_Fram"",""U_EX_Man"",""U_EX_Ser"",""U_EX_Car"",""U_INT_Free"",""U_INT_Clean"",""U_INT_Dry"",""U_INT_Pitt"",""U_INT_Disc"",""U_VAL_Val"",""U_VAL_Bott"",""U_VAL_Man"",""U_VAL_Syp"",""U_VAL_Tank"",""U_VAL_Avail"",""U_VAL_Steam"",""U_VAL_Gas"",""U_SEAL_MAN"",""U_SEAL_AIR"",""U_SEAL_BOTT"",""U_SEAL_LAST"",""U_SEAL_NEXT"",""U_SEAL_NEXT"" FROM ODLN T0  INNER JOIN DLN1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" WHERE T0.""DocEntry"" ='" & DocEntry & "'"
            Dim Str As String = String.Empty
            'Str = "SELECT T0.""DocEntry"" as ""Survey_No"",T0.""DocNum"",T1.""BaseEntry"" AS ""Order_No"",T1.""BaseRef"" AS ""SODocNum"", T0.""U_UName"" AS ""User_Name"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY')  As ""Survey_Date"", " & _
            '      " T0.""CardCode"" AS ""Customer_Code"", T0.""CardName"" AS ""Customer_Name"",T1.""Dscription"" as ""Survey_Type"", " & _
            '      " T0.""U_Country"" AS ""Location"",T0.""U_SResult"" AS ""Survey_Result"" " & _
            '      " FROM ODLN T0  INNER JOIN DLN1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" " & _
            '      " WHERE T0.""DocNum"" = (CASE WHEN IFNULL('" & sSurveyNo & "','') = '' THEN T0.""DocNum"" ELSE '" & sSurveyNo & "' END) " & _
            '      " AND T1.""BaseRef"" = (CASE WHEN IFNULL('" & sOrderNo & "','') = '' THEN T1.""BaseRef"" ELSE '" & sOrderNo & "' END) " & _
            '      " AND UPPER(T0.""CardCode"") = (CASE WHEN IFNULL('" & sCustCode.ToUpper() & "','') = '' THEN UPPER(T0.""CardCode"") ELSE '" & sCustCode.ToUpper() & "' END) " & _
            '      " AND UPPER(T0.""CardName"") = (CASE WHEN IFNULL('" & sCustName.ToUpper() & "','') = '' THEN UPPER(T0.""CardName"") ELSE '" & sCustName.ToUpper() & "' END) " & _
            '      " AND T0.""U_Cdate"" >= (CASE WHEN IFNULL('" & sSurveyDtFrom & "','') = '' THEN T0.""U_Cdate"" ELSE '" & dSurveyDtFrom.ToString("yyyy-MM-dd") & "' END) " & _
            '      " AND T0.""U_Cdate"" <= (CASE WHEN IFNULL('" & sSurveyDtTo & "','') = '' THEN T0.""U_Cdate"" ELSE '" & dSurveyDtTo.ToString("yyyy-MM-dd") & "' END) " & _
            '      " AND UPPER(T0.""U_EqNo"") = (CASE WHEN IFNULL('" & sContainerNo.ToUpper() & "','') = '' THEN UPPER(T0.""U_EqNo"") ELSE '" & sContainerNo.ToUpper() & "' END) "

            Str = "SELECT T0.""DocEntry"" as ""Survey_No"",T0.""DocNum"",T1.""BaseEntry"" AS ""Order_No"",T1.""BaseRef"" AS ""SODocNum"", T0.""U_UName"" AS ""User_Name"", TO_CHAR( T0.""U_Cdate"" , 'DD/MM/YYYY')  As ""Survey_Date"", " & _
                 " T0.""CardCode"" AS ""Customer_Code"", T0.""CardName"" AS ""Customer_Name"",T1.""Dscription"" as ""Survey_Type"", " & _
                 " T0.""U_Country"" AS ""Location"",T0.""U_SResult"" AS ""Survey_Result"" " & _
                 " FROM ODLN T0  INNER JOIN DLN1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" " & _
                 " WHERE T0.""DocNum"" LIKE '" & sSurveyNo & "' AND T1.""BaseRef"" LIKE '" & sOrderNo & "' " & _
                 " AND UPPER(T0.""CardCode"") LIKE '" & sCustCode.ToUpper() & "' AND UPPER(T0.""CardName"") LIKE '" & sCustName.ToUpper() & "' " & _
                 " AND T0.""U_Cdate"" LIKE '" & sSurveyDtFrom & "' AND T0.""U_Cdate"" LIKE '" & sSurveyDtTo & "' " & _
                 " AND UPPER(T0.""U_EqNo"") LIKE '" & sContainerNo.ToUpper() & "' " & _
                 " ORDER BY T1.""BaseRef"",T0.""DocEntry"""

            Dim RetDT As New DataTable
            Dim RetDT1 As New DataTable
            Dim RetDT2 As New DataTable
            RetDT = fn.ExecuteSQLQuery(Str, Errmsg)
            If Errmsg <> "" Then
                Throw New Exception(Errmsg)
            End If
            If RetDT.Rows.Count > 0 Then
                RetDT.TableName = "ODLN"
                RetDT1 = RetDT.Copy
                RetDS.Tables.Add(RetDT1)

                Context.Response.Output.Write(fn.ds2json(RetDS))
            Else
                Throw New Exception("No Records Found")
            End If
        Catch ex As Exception
            Context.Response.Output.Write(fn.ds2json(ErrorHandling(ex.Message.ToString)))
        End Try
    End Sub
#End Region
    Public Function DateConvert(ByVal St As String) As String
        Dim dt As String = ""
        dt = St.Substring(6, 4) & "-" & St.Substring(3, 2) & "-" & St.Substring(0, 2)
        Return dt
    End Function
    Public Function ErrorHandling(ByVal ErrMsg As String) As DataSet
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim dsNew = New DataSet()
        dt.TableName = "VALIDATE"
        dt.Columns.Add("Status")
        dt.Columns.Add("Msg")
        dt.Rows.Add()
        dt.Rows(0)(0) = False
        dt.Rows(0)(1) = ErrMsg
        dsNew.Tables.Add(dt)
        dt.Dispose()
        Return dsNew
    End Function

    Public Function GetDateTimeValue(ByVal DateString As String) As DateTime
        Dim sErrDesc As String = String.Empty
        fn.GetSAPConnection(sErrDesc)
        If sErrDesc <> "" Then
            Throw New Exception(sErrDesc)
        End If

        Dim objBridge As SAPbobsCOM.SBObob
        '   objBridge = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
        objBridge = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
        Return objBridge.Format_StringToDate(DateString).Fields.Item(0).Value
    End Function
End Class