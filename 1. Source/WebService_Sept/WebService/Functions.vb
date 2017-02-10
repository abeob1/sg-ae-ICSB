Imports Microsoft.VisualBasic
Imports Newtonsoft.Json
Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data.Common
Imports System.Net.Mail
Imports System.Net.Mime
Imports System.IO
Imports System



Public Class Functions
    Dim modLog As New modLogging
    Dim DEBUG_ON As Int32 = PublicVariable.DEBUG_ON
    Dim DEBUG_OFF As Int32 = PublicVariable.DEBUG_OFF
    Dim p_iDebugMode As Int16 = DEBUG_ON
    Dim p_iDeleteDebugLog As Int16 = PublicVariable.p_iDeleteDebugLog
    Public Function ds2json(ds As DataSet) As String
        Return JsonConvert.SerializeObject(ds, Formatting.Indented)
    End Function
    Public Function dt2json(ds As DataTable) As String
        Return JsonConvert.SerializeObject(ds, Formatting.Indented)
    End Function
    Public Function String2json(str As String) As String
        Return JsonConvert.SerializeObject(str, Formatting.Indented)
    End Function
    Public Function jsontodata(sjon As String) As DataSet
        Dim data As DataSet = JsonConvert.DeserializeObject(Of DataSet)(sjon)
        Return data
    End Function
    Public Function ExecuteSQLQuery(ByVal sQuery As String, ByRef sErrDesc As String) As DataTable

        '**************************************************************
        ' Function      : ExecuteHANAQuery
        ' Purpose       : Execute HANA
        ' Parameters    : ByVal sSQL - string command Text
        ' Author        : Sri
        ' Date          : 
        ' Change        :
        '**************************************************************
        If PublicVariable.sSQLServer = "" Then
            If GetSystemIntializeInfo(sErrDesc) = 0 Then
                Throw New Exception(sErrDesc)
            End If
        End If

        Dim sFuncName As String = String.Empty
        Dim sConstr As String = "DRIVER={HDBODBC32};UID=" & PublicVariable.sSQLDBUser & ";PWD=" & PublicVariable.sSQLDBPwd & ";SERVERNODE=" & PublicVariable.sSQLServer & ";CS=" & PublicVariable.sSQLDBName
        Dim oCon As New Odbc.OdbcConnection(sConstr)
        Dim oCmd As New Odbc.OdbcCommand
        Dim oDs As New DataSet
        If p_iDebugMode = DEBUG_ON Then Call modLog.WriteToLogFile_Debug("Starting Function", sFuncName)
        Try
            sFuncName = "ExecuteQuery()"
            oCon.Open()
            oCmd.CommandType = CommandType.Text
            oCmd.CommandText = sQuery
            oCmd.Connection = oCon
            oCmd.CommandTimeout = 0
            Dim da As New Odbc.OdbcDataAdapter(oCmd)
            da.Fill(oDs)
            If p_iDebugMode = DEBUG_ON Then Call modLog.WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
        Catch ex As Exception
            If p_iDebugMode = DEBUG_ON Then Call modLog.WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Call modLog.WriteToLogFile(ex.Message, sFuncName)
        Finally
            oCon.Dispose()
        End Try
        Return oDs.Tables(0)
    End Function

    Public Function ExecuteNonQuery(ByVal sQuery As String, ByRef sErrDesc As String) As String

        '**************************************************************
        ' Function      : ExecuteHANAQuery
        ' Purpose       : Execute HANA
        ' Parameters    : ByVal sSQL - string command Text
        ' Author        : Sri
        ' Date          : 
        ' Change        :
        '**************************************************************
        If PublicVariable.sSQLServer = "" Then
            If GetSystemIntializeInfo(sErrDesc) = 0 Then
                Throw New Exception(sErrDesc)
            End If
        End If

        Dim sFuncName As String = String.Empty
        Dim sConstr As String = "DRIVER={HDBODBC32};UID=" & PublicVariable.sSQLDBUser & ";PWD=" & PublicVariable.sSQLDBPwd & ";SERVERNODE=" & PublicVariable.sSQLServer & ";CS=" & PublicVariable.sSQLDBName



        Dim oCon As New Odbc.OdbcConnection(sConstr)
        Dim oCmd As New Odbc.OdbcCommand
        Dim oDs As New DataSet
        If p_iDebugMode = DEBUG_ON Then Call modLog.WriteToLogFile_Debug("Starting Function", sFuncName)
        Try

            sFuncName = "ExecuteNonQuery()"
            oCon.Open()
            oCmd.CommandType = CommandType.Text
            oCmd.CommandText = sQuery
            oCmd.Connection = oCon
            oCmd.CommandTimeout = 0
            oCmd.ExecuteNonQuery()
            sErrDesc = ""
            Return sErrDesc
            If p_iDebugMode = DEBUG_ON Then Call modLog.WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
        Catch ex As Exception
            If p_iDebugMode = DEBUG_ON Then Call modLog.WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Call modLog.WriteToLogFile(ex.Message, sFuncName)
        Finally
            oCon.Dispose()
        End Try
        Return ""
    End Function
    Public Function ExecuteSQLQuery_SingleValue(ByVal sQuery As String, ByRef sErrDesc As String) As String

        '**************************************************************
        ' Function      : ExecuteHANAQuery
        ' Purpose       : Execute HANA
        ' Parameters    : ByVal sSQL - string command Text
        ' Author        : Sri
        ' Date          : 
        ' Change        :
        '**************************************************************
        If PublicVariable.sSQLServer = "" Then
            If GetSystemIntializeInfo(sErrDesc) = 0 Then
                Throw New Exception(sErrDesc)
            End If
        End If

        Dim sFuncName As String = String.Empty
        Dim sConstr As String = "DRIVER={HDBODBC32};UID=" & PublicVariable.sSQLDBUser & ";PWD=" & PublicVariable.sSQLDBPwd & ";SERVERNODE=" & PublicVariable.sSQLServer & ";CS=" & PublicVariable.sSQLDBName
        Dim oCon As New Odbc.OdbcConnection(sConstr)
        Dim oCmd As New Odbc.OdbcCommand
        Dim oDs As New DataSet
        If p_iDebugMode = DEBUG_ON Then Call modLog.WriteToLogFile_Debug("Starting Function", sFuncName)
        Try
            sFuncName = "ExecuteQuery()"
            oCon.Open()
            oCmd.CommandType = CommandType.Text
            oCmd.CommandText = sQuery
            oCmd.Connection = oCon
            oCmd.CommandTimeout = 0
            Dim da As New Odbc.OdbcDataAdapter(oCmd)
            da.Fill(oDs)
            If p_iDebugMode = DEBUG_ON Then Call modLog.WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
        Catch ex As Exception
            If p_iDebugMode = DEBUG_ON Then Call modLog.WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            Call modLog.WriteToLogFile(ex.Message, sFuncName)
        Finally
            oCon.Dispose()
        End Try
        If oDs.Tables(0).Rows.Count > 0 Then
            Return oDs.Tables(0).Rows(0)(0).ToString
        Else
            Return ""
        End If


    End Function


    Public Function GetSystemIntializeInfo(ByRef sErrDesc As String) As Long

        ' **********************************************************************************
        '   Function    :   GetSystemIntializeInfo()
        '   Purpose     :   This function will be providing to proceed the initializing 
        '                   variable control during the system start-up
        '               
        '   Parameters  :   ByRef oCompDef As CompanyDefault
        '                       oCompDef =  set the Company Default structure
        '                   ByRef sErrDesc AS String 
        '                       sErrDesc = Error Description to be returned to calling function
        '               
        '   Return      :   0 - FAILURE
        '                   1 - SUCCESS
        '   Author      :   JOHN
        '   Date        :   October 2015
        ' **********************************************************************************

        Dim sFuncName As String = String.Empty
        Dim sConnection As String = String.Empty
        
        Try
           

            sFuncName = "GetSystemIntializeInfo()"
            If p_iDebugMode = DEBUG_ON Then Call modLog.WriteToLogFile_Debug("Starting Function", sFuncName)

            Dim strConnect As String = ""
            Dim sCon As String = ""
            Dim MyArr As Array

            strConnect = "LogPath"
            sCon = System.Configuration.ConfigurationSettings.AppSettings.Get(strConnect)
            MyArr = sCon.Split(";")
            PublicVariable.LogPath = MyArr(0).ToString()

            strConnect = "SAPConnect"
            sCon = System.Configuration.ConfigurationSettings.AppSettings.Get(strConnect)
            MyArr = sCon.Split(";")
            PublicVariable.sSQLServer = MyArr(3).ToString()
            PublicVariable.sSQLDBName = MyArr(0).ToString()
            PublicVariable.sSQLDBUser = MyArr(4).ToString()
            PublicVariable.sSQLDBPwd = MyArr(5).ToString()
            PublicVariable.sSQLType = MyArr(7).ToString()
            PublicVariable.sSAPLicenseManager = MyArr(6).ToString()
            PublicVariable.sSAPUserID = MyArr(1).ToString()
            PublicVariable.sSAPPwd = MyArr(2).ToString()

            If p_iDebugMode = DEBUG_ON Then Call modLog.WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
            GetSystemIntializeInfo = PublicVariable.RTN_SUCCESS

        Catch ex As Exception
            modLog.WriteToLogFile(ex.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call modLog.WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            GetSystemIntializeInfo = PublicVariable.RTN_ERROR
        End Try
    End Function

    Public Function GetSAPConnection(ByRef sErrDesc As String) As Long

        ' **********************************************************************************
        '   Function    :   GetSystemIntializeInfo()
        '   Purpose     :   This function will be providing to proceed the initializing 
        '                   variable control during the system start-up
        '               
        '   Parameters  :   ByRef oCompDef As CompanyDefault
        '                       oCompDef =  set the Company Default structure
        '                   ByRef sErrDesc AS String 
        '                       sErrDesc = Error Description to be returned to calling function
        '               
        '   Return      :   0 - FAILURE
        '                   1 - SUCCESS
        '   Author      :   JOHN
        '   Date        :   October 2015
        ' **********************************************************************************

        Dim sFuncName As String = String.Empty
        Dim sConnection As String = String.Empty
        Dim lRetCode As Integer

        Try
            If PublicVariable.sSQLServer = "" Then
                If GetSystemIntializeInfo(sErrDesc) = 0 Then
                    Throw New Exception(sErrDesc)
                End If
            End If

            'PublicVariable.oCompany = New SAPbobsCOM.Company
            If IsNothing(PublicVariable.oCompany) Then
                PublicVariable.oCompany = New SAPbobsCOM.Company
            End If
            If PublicVariable.oCompany.Connected = False Then

                sFuncName = "GetSAPConnection()"
                If p_iDebugMode = DEBUG_ON Then Call modLog.WriteToLogFile_Debug("Starting Function", sFuncName)

                Dim strConnect As String = ""
                Dim sCon As String = ""

                PublicVariable.oCompany.Server = PublicVariable.sSQLServer
                PublicVariable.oCompany.CompanyDB = PublicVariable.sSQLDBName
                PublicVariable.oCompany.DbUserName = PublicVariable.sSQLDBUser
                PublicVariable.oCompany.DbPassword = PublicVariable.sSQLDBPwd
                PublicVariable.oCompany.LicenseServer = PublicVariable.sSAPLicenseManager
                PublicVariable.oCompany.UserName = PublicVariable.sSAPUserID
                PublicVariable.oCompany.Password = PublicVariable.sSAPPwd
                PublicVariable.oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB
                PublicVariable.oCompany.language = SAPbobsCOM.BoSuppLangs.ln_English
                PublicVariable.oCompany.UseTrusted = False

                lRetCode = PublicVariable.oCompany.Connect
                If lRetCode <> 0 Then
                    PublicVariable.oCompany.GetLastError(lRetCode, sErrDesc)

                    Return PublicVariable.RTN_SUCCESS

                Else
                    modLog.WriteToLogFile_Debug("ConnectSAPDB OK", sFuncName)
                    Return PublicVariable.RTN_ERROR

                End If
                If p_iDebugMode = DEBUG_ON Then Call modLog.WriteToLogFile_Debug("Completed with SUCCESS", sFuncName)
                GetSAPConnection = PublicVariable.RTN_SUCCESS
            Else
                Return PublicVariable.RTN_SUCCESS
            End If
        Catch ex As Exception
            modLog.WriteToLogFile(ex.Message, sFuncName)
            If p_iDebugMode = DEBUG_ON Then Call modLog.WriteToLogFile_Debug("Completed with ERROR", sFuncName)
            GetSAPConnection = PublicVariable.RTN_ERROR
        End Try
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
    Public Shared Sub writelog(ByVal str As String)
        Dim oWrite As IO.StreamWriter
        Dim FPath As String
        'FPath = System.
        FPath = "E:\Abeo\ICSB\WebService_Matesh\bin" + "\logfile.txt"
        If IO.File.Exists(FPath) Then
            oWrite = IO.File.AppendText(FPath)
        Else
            oWrite = IO.File.CreateText(FPath)
        End If
        oWrite.Write(Now.ToString() + ":" + str + vbCrLf)
        oWrite.Close()
    End Sub

End Class
