Public Class SAP_Functions
    Public Function Create_IncommingPayment(ARDocEntry As Integer, UserID As String) As String
        Dim str As String = ""
        Dim RetVal As Long
        Dim lErrCode As Integer
        Dim sErrMsg As String
        Dim oPayment As SAPbobsCOM.Payments
        Dim oInvoice As SAPbobsCOM.Documents

        oPayment = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments)
        oInvoice = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices)

        oPayment.CardCode = oInvoice.CardCode
        oPayment.CashAccount = GetCashAccount(UserID)
        oPayment.DocDate = oInvoice.DocDate
        oPayment.TaxDate = oInvoice.DocDate

        oPayment.Invoices.DocEntry = ARDocEntry
        oPayment.Invoices.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_Invoice
        oPayment.CashSum = oInvoice.DocTotal
        RetVal = oPayment.Add
        If RetVal <> 0 Then
            PublicVariable.oCompany.GetLastError(lErrCode, sErrMsg)
            str = sErrMsg
        End If
        Return str
    End Function
    Public Function GetLastKey(UserID As String, ObjType As String) As String
        Try
            Dim str As String
            str = "Select top(1) LinkAct_3 ObjKey from OACP where isnull(LinkAct_3,'')<>''"
            Dim dt As DataTable
            Dim connect As New Connection()
            connect.setDB(UserID)
            dt = connect.ObjectGetAll_Query_SAP(str).Tables(0)

            If dt.Rows.Count > 0 Then
                Return dt.Rows(0).Item("ObjKey").ToString
            Else
                Return ""
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function
    Private Function GetCashAccount(UserID As String) As String
        Try
            Dim str As String
            str = "Select top(1) LinkAct_3 CashAccount from OACP where isnull(LinkAct_3,'')<>''"
            Dim dt As DataTable
            Dim connect As New Connection()
            connect.setDB(UserID)
            dt = connect.ObjectGetAll_Query_SAP(str).Tables(0)

            If dt.Rows.Count > 0 Then
                Return dt.Rows(0).Item("CashAccount").ToString
            Else
                Return ""
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function
    Public Function GetPriceAfterDiscount(ByVal cardCode As String, ByVal itemCode As String, ByVal amount As Single, ByVal refDate As Date, UserID As String) As Double
        Dim connect As New Connection()
        If Connection.bConnect = False Then

            If connect.connectDB(UserID) <> "" Then
                Return 0
            End If
        End If
        Dim vObj As SAPbobsCOM.SBObob
        Dim rs As SAPbobsCOM.Recordset
        vObj = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
        rs = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        rs = vObj.GetItemPrice(cardCode, itemCode, amount, refDate)
        Return rs.Fields.Item("Price").Value
    End Function
    Public Function GetGrossPrice(ByVal cardCode As String, ByVal itemCode As String, UserID As String) As Double
        Dim connect As New Connection()
        connect.setDB(UserID)
        Dim dt As DataTable
        Dim str As String = ""
        str = " select isnull(t0.Price,0) GrossPrice from ITM1 T0 "
        str = str & " join OCRD T1 on T0.PriceList=T1.ListNum "
        str = str & " where T0.ItemCode='" + itemCode + "' and T1.CardCode='" + cardCode + "'"
        dt = connect.ObjectGetAll_Query_SAP(str).Tables(0)
        If dt.Rows.Count = 0 Then
            Return 0
        Else
            Return dt.Rows(0).Item("GrossPrice")
        End If
    End Function
    Public Function GetDefaultLineInfo(UserID As String, ByVal cardCode As String, ByVal itemCode As String, _
                                       ByVal amount As Single, ByVal refDate As Date) As DataSet
        Dim ds = New DataSet
        ds.Tables.Add()
        ds.Tables(0).Columns.Add("UnitPrice", GetType(Double))
        ds.Tables(0).Columns.Add("Discount", GetType(Double))
        ds.Tables(0).Columns.Add("PriceAfDi", GetType(Double))
        ds.Tables(0).Columns.Add("WhsCode", GetType(String))
        ds.Tables(0).Columns.Add("TaxCode", GetType(String))
        ds.Tables(0).Columns.Add("TaxRate", GetType(Double))

        Dim GrossPrice As Double = GetGrossPrice(cardCode, itemCode, UserID)
        Dim NetPrice As Double = GetPriceAfterDiscount(cardCode, itemCode, amount, refDate, UserID)
        Dim Discount As Double = 0
        Dim WhsCode As String =  GetDefaultWarehouse(UserID)
        If WhsCode = "" Then WhsCode = "01"
        Dim TaxCode As String = ""
        Dim TaxRate As Double = 0
        Dim dstax As DataSet = GetDefaultTaxCode(itemCode, cardCode)
        If Not IsNothing(dstax) Then
            If dstax.Tables.Count > 0 Then
                If dstax.Tables(0).Rows.Count > 0 Then
                    TaxCode = dstax.Tables(0).Rows(0).Item("Code").ToString
                    TaxRate = dstax.Tables(0).Rows(0).Item("Rate").ToString
                End If
            End If
        End If
        If GrossPrice = 0 Then
            Discount = 0
        Else
            Discount = (GrossPrice - NetPrice) * 100 / GrossPrice
        End If

        Dim dr As DataRow
        dr = ds.Tables(0).NewRow
        dr("UnitPrice") = GrossPrice
        dr("Discount") = Discount
        dr("PriceAfDi") = NetPrice
        dr("WhsCode") = WhsCode
        dr("TaxCode") = TaxCode
        dr("TaxRate") = TaxRate
        ds.Tables(0).Rows.Add(dr)

        Return ds
    End Function
    Public Function GetDefaultBP(UserID As String, Cardtype As String) As DataSet
        Try
            Dim str As String
            If Cardtype = "C" Then
                str = "Select top(1) T1.CardCode,T1.CardName  from OACP T0 "
                str = str + " join OCRD T1 on T0.DfltCard=T1.CardCode"
                str = str + " where isnull(DfltCard,'')<>''"
            Else
                str = "Select 'V00001' CardCode, 'Default BP' CardName"
            End If

            Dim dt As DataSet
            Dim connect As New Connection()
            connect.setDB(UserID)
            dt = connect.ObjectGetAll_Query_SAP(str)

            Return dt
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    
    Public Function GetDefaultWarehouse(UserID As String) As String
      Dim str As String = ""
        Try
            str = "select top(1) T0.WhsCode from OWHS T0 join OLCT T1 on T0.Location=T1.Code where T1.Location='" + UserID + "' and T0.U_WhsType=1"

            Dim ors As SAPbobsCOM.Recordset
            ors = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            ors.DoQuery(str)
            If ors.RecordCount = 1 Then
                Return ors.Fields.Item("WhsCode").Value.ToString
            Else
                Return ""
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function
    Public Function GetDefaultTaxCode(ItemCode As String, CardCode As String) As DataSet
        Dim str As String = ""
        Try
            str = "select Code,Name,Rate from ovtg where Code=("
            str = str + " select case when (select CardType from ocrd where cardcode='" + CardCode + "')='C' then VatGourpSa else VatGroupPu end TaxCode from OITM where ItemCode='" + ItemCode + "'"
            str = str + " )"

            Dim ors As SAPbobsCOM.Recordset
            ors = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            ors.DoQuery(str)
            Return ConvertRS2DT(ors)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function GetPromotionWarehouse(UserID As String) As String
        Dim str As String = ""
        Try
            str = "select top(1) T0.WhsCode from OWHS T0 join OLCT T1 on T0.Location=T1.Code where T1.Location='" + UserID + "' and T0.U_WhsType=2"

            Dim ors As SAPbobsCOM.Recordset
            ors = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            ors.DoQuery(str)
            If ors.RecordCount = 1 Then
                Return ors.Fields.Item("WhsCode").Value.ToString
            Else
                Return ""
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function
    Public Function ConvertRS2DT(ByVal RS As SAPbobsCOM.Recordset) As DataSet
        Dim dtTable As New DataSet
        dtTable.Tables.Add()
        Dim NewCol As DataColumn
        Dim NewRow As DataRow
        Dim ColCount As Integer
        Try
            For ColCount = 0 To RS.Fields.Count - 1
                Dim dataType As String = "System."
                Select Case RS.Fields.Item(ColCount).Type
                    Case SAPbobsCOM.BoFieldTypes.db_Alpha
                        dataType = dataType & "String"
                    Case SAPbobsCOM.BoFieldTypes.db_Date
                        dataType = dataType & "DateTime"
                    Case SAPbobsCOM.BoFieldTypes.db_Float
                        dataType = dataType & "Double"
                    Case SAPbobsCOM.BoFieldTypes.db_Memo
                        dataType = dataType & "String"
                    Case SAPbobsCOM.BoFieldTypes.db_Numeric
                        dataType = dataType & "Decimal"
                    Case Else
                        dataType = dataType & "String"
                End Select

                NewCol = New DataColumn(RS.Fields.Item(ColCount).Name, System.Type.GetType(dataType))
                dtTable.Tables(0).Columns.Add(NewCol)
            Next
            RS.MoveFirst()
            Do Until RS.EoF

                NewRow = dtTable.Tables(0).NewRow
                'populate each column in the row we're creating
                For ColCount = 0 To RS.Fields.Count - 1

                    NewRow.Item(RS.Fields.Item(ColCount).Name) = RS.Fields.Item(ColCount).Value

                Next

                'Add the row to the datatable
                dtTable.Tables(0).Rows.Add(NewRow)

                RS.MoveNext()
            Loop
            Return dtTable
        Catch ex As Exception
            MsgBox(ex.ToString & Chr(10) & "Error converting SAP Recordset to DataTable", MsgBoxStyle.Exclamation)
            Return Nothing
        End Try
    End Function
    Public Function GetPromotion(UserID As String, ItemCode As String, CardCode As String, _
                                 Quantity As Double, DocDate As Date, Amount As Double) As DataSet
        Try
            Dim str As String
            str = "exec sp_Promotion_Get '" & ItemCode & "'," & CStr(Quantity) & ",'" & CStr(DocDate) & "'," & CStr(Amount) & ",'" & CardCode & "'"
            Dim dt As DataSet
            Dim connect As New Connection()
            connect.setDB(UserID)
            dt = connect.ObjectGetAll_Query_SAP(str)
            Return dt
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function GetGopyFromTo(Type As Integer, ObjType As String) As DataSet
        Dim ds = New DataSet
        ds.Tables.Add()
        ds.Tables(0).Columns.Add("Code", GetType(String))
        ds.Tables(0).Columns.Add("Name", GetType(String))
        Dim dr As DataRow

        Select Case Type
            Case 1 ' Copy To
                Select Case ObjType
                    Case "22"
                        dr = ds.Tables(0).NewRow
                        dr("Code") = "AA"
                        dr("Name") = "GRPO"
                        ds.Tables(0).Rows.Add(dr)

                        dr = ds.Tables(0).NewRow
                        dr("Code") = "AA"
                        dr("Name") = "AP Invoice"
                        ds.Tables(0).Rows.Add(dr)
                End Select
            Case 2 ' Copy From
                Select Case ObjType
                    Case "22"
                        dr = ds.Tables(0).NewRow
                        dr("Code") = "AA"
                        dr("Name") = "Purchase Quotation"
                        ds.Tables(0).Rows.Add(dr)
                End Select
        End Select
        Return ds
    End Function
    Public Function GetMaxDocEntry(ObjType As String, UserID As String, TableName As String, KeyName As String) As String
        Dim dt As DataSet
        Dim Str As String = ""
        Dim connect As New Connection()

        dt = connect.ObjectGetAll_Query_SAP("select MAX(" + KeyName + ") DocKey from " + TableName)

        If IsNothing(dt) Then
            Return ""
        End If
        If dt.Tables(0).Rows.Count > 0 Then
            Return dt.Tables(0).Rows(0).Item(KeyName)
        Else
            Return ""
        End If

    End Function
    Public Function ReturnMessage(ErrCode As Integer, ErrMsg As String) As DataSet
        Dim dtJE = New DataSet
        dtJE.Tables.Add()
        dtJE.Tables(0).Columns.Add("ErrCode", GetType(Integer))
        dtJE.Tables(0).Columns.Add("ErrMsg", GetType(String))

        Dim dr As DataRow
        dr = dtJE.Tables(0).NewRow
        dr("ErrCode") = ErrCode
        dr("ErrMsg") = ErrMsg
        dtJE.Tables(0).Rows.Add(dr)

        Return dtJE
    End Function
    

    Public Function CreateUDF(ByVal tableName As String, ByVal fieldName As String, _
                              ByVal desc As String, ByVal fieldType As SAPbobsCOM.BoFieldTypes, _
                              ByVal Size As Integer, UserID As String) As String
        Try
            Dim connect As New Connection()
            If Connection.bConnect = False Then
                If Not connect.connectDB(UserID) Then
                    Return "Connect SAP failed"
                End If
            End If

            Dim oUdfMD As SAPbobsCOM.UserFieldsMD
            oUdfMD = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)


            oUdfMD.TableName = tableName
            oUdfMD.Name = fieldName
            oUdfMD.Type = fieldType
            oUdfMD.Description = desc
            oUdfMD.EditSize = Size
            Dim lRetCode As Integer = oUdfMD.Add()
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUdfMD)
            oUdfMD = Nothing
            Return ""
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function
    Private Function AddUDT(ByVal tableID As String, ByVal tableName As String, UserID As String) As Boolean
        Dim mst_ErrNumber As String
        Dim min_ErrMsg As Integer
        Dim oUserTablesMD As SAPbobsCOM.UserTablesMD
        Dim connect As New Connection()
        If Connection.bConnect = False Then

            If Not connect.connectDB(UserID) Then
                Return "Connect SAP failed"
            End If
        End If

        oUserTablesMD = PublicVariable.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables)

        If Not oUserTablesMD.GetByKey(tableID) Then
            oUserTablesMD.TableName = tableID
            oUserTablesMD.TableDescription = tableName
            oUserTablesMD.TableType = SAPbobsCOM.BoUTBTableType.bott_NoObject
            If oUserTablesMD.Add <> 0 Then
                PublicVariable.oCompany.GetLastError(min_ErrMsg, mst_ErrNumber)
                Return False
            End If
        End If
        System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserTablesMD)
        Return True
    End Function
End Class
