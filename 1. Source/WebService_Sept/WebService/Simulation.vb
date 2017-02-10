Public Class Simulation

    Public Function Simulate_OCRD(CardType As String) As DataSet
        Dim dtJE = New DataSet
        dtJE.Tables.Add()
        dtJE.Tables(0).Columns.Add("CardCode", GetType(String))
        dtJE.Tables(0).Columns.Add("CardName", GetType(String))

        Dim dr As DataRow
        dr = dtJE.Tables(0).NewRow
        dr("CardCode") = "C00001"
        dr("CardName") = "Nguyen Van A"
        dtJE.Tables(0).Rows.Add(dr)

        dr = dtJE.Tables(0).NewRow
        dr("CardCode") = "C00002"
        dr("CardName") = "Nguyen Van B"
        dtJE.Tables(0).Rows.Add(dr)

        Return dtJE
    End Function
    Public Function Simulate_OITM() As DataSet
        Dim dtJE = New DataSet
        dtJE.Tables.Add()
        dtJE.Tables(0).Columns.Add("ItemCode", GetType(String))
        dtJE.Tables(0).Columns.Add("ItemName", GetType(String))

        Dim dr As DataRow
        dr = dtJE.Tables(0).NewRow
        dr("ItemCode") = "ITM00001"
        dr("ItemName") = "Bia 3333"
        dtJE.Tables(0).Rows.Add(dr)

        dr = dtJE.Tables(0).NewRow
        dr("ItemCode") = "ITM00002"
        dr("ItemName") = "Bia Tiger"
        dtJE.Tables(0).Rows.Add(dr)

        Return dtJE
    End Function
    Public Function Simulate_OWHS() As DataSet
        Dim dtJE = New DataSet
        dtJE.Tables.Add()
        dtJE.Tables(0).Columns.Add("WhsCode", GetType(String))
        dtJE.Tables(0).Columns.Add("WhsName", GetType(String))

        Dim dr As DataRow
        dr = dtJE.Tables(0).NewRow
        dr("WhsCode") = "01"
        dr("WhsName") = "warehouse 1"
        dtJE.Tables(0).Rows.Add(dr)

        dr = dtJE.Tables(0).NewRow
        dr("WhsCode") = "02"
        dr("WhsName") = "warehouse 2"
        dtJE.Tables(0).Rows.Add(dr)

        Return dtJE
    End Function
    Public Function Simulate_OTVG() As DataSet
        Dim dtJE = New DataSet
        dtJE.Tables.Add()
        dtJE.Tables(0).Columns.Add("Code", GetType(String))
        dtJE.Tables(0).Columns.Add("Name", GetType(String))
        dtJE.Tables(0).Columns.Add("Rate", GetType(Double))
        Dim dr As DataRow
        dr = dtJE.Tables(0).NewRow
        dr("Code") = "P1"
        dr("Name") = "VAT IN 10"
        dr("Rate") = 10
        dtJE.Tables(0).Rows.Add(dr)

        dr = dtJE.Tables(0).NewRow
        dr("Code") = "P2"
        dr("Name") = "VAT IN 5"
        dr("Rate") = 5
        dtJE.Tables(0).Rows.Add(dr)

        Return dtJE
    End Function
    Public Function Simulate_OHEM() As DataSet
        Dim dtJE = New DataSet
        dtJE.Tables.Add()
        dtJE.Tables(0).Columns.Add("Code", GetType(String))
        dtJE.Tables(0).Columns.Add("FirstName", GetType(String))
        dtJE.Tables(0).Columns.Add("MidName", GetType(String))
        dtJE.Tables(0).Columns.Add("LastName", GetType(String))

        Dim dr As DataRow
        dr = dtJE.Tables(0).NewRow
        dr("Code") = "1"
        dr("FirstName") = "TRUONG"
        dr("MidName") = "THAI"
        dr("LastName") = "THUY"
        dtJE.Tables(0).Rows.Add(dr)

        dr = dtJE.Tables(0).NewRow
        dr("Code") = "2"
        dr("FirstName") = "PHAM"
        dr("MidName") = "THI"
        dr("LastName") = "LIEU"
        dtJE.Tables(0).Rows.Add(dr)

        Return dtJE
    End Function
    Public Function Simulate_OSLP() As DataSet
        Dim dtJE = New DataSet
        dtJE.Tables.Add()
        dtJE.Tables(0).Columns.Add("Code", GetType(String))
        dtJE.Tables(0).Columns.Add("Name", GetType(String))

        Dim dr As DataRow
        dr = dtJE.Tables(0).NewRow
        dr("Code") = "1"
        dr("Name") = "TRUONG THUY"
        dtJE.Tables(0).Rows.Add(dr)

        dr = dtJE.Tables(0).NewRow
        dr("Code") = "2"
        dr("Name") = "PHAM LIEU"
        dtJE.Tables(0).Rows.Add(dr)

        Return dtJE
    End Function
    Public Function Simulate_OACT() As DataSet
        Dim dtJE = New DataSet
        dtJE.Tables.Add()
        dtJE.Tables(0).Columns.Add("AcctCode", GetType(String))
        dtJE.Tables(0).Columns.Add("AcctName", GetType(String))
        dtJE.Tables(0).Columns.Add("FrgName", GetType(String))

        Dim dr As DataRow
        dr = dtJE.Tables(0).NewRow
        dr("AcctCode") = "155"
        dr("AcctName") = "Tai khoan Kho"
        dr("FrgName") = "Inventory Account"
        dtJE.Tables(0).Rows.Add(dr)

        dr = dtJE.Tables(0).NewRow
        dr("AcctCode") = "155"
        dr("AcctName") = "Tai khoan Kho"
        dr("FrgName") = "Inventory Account"
        dtJE.Tables(0).Rows.Add(dr)

        Return dtJE
    End Function
    Public Function Simulate_OCPR(ByVal CardCode As String) As DataSet
        Dim dtJE = New DataSet
        dtJE.Tables.Add()
        dtJE.Tables(0).Columns.Add("IsDefault", GetType(String))
        dtJE.Tables(0).Columns.Add("Code", GetType(String))
        dtJE.Tables(0).Columns.Add("FirstName", GetType(String))
        dtJE.Tables(0).Columns.Add("LastName", GetType(String))

        Dim dr As DataRow
        dr = dtJE.Tables(0).NewRow
        dr("IsDefault") = "Y"
        dr("Code") = "1"
        dr("FirstName") = "TRUONG"
        'dr("MidName") = "THAI"
        dr("LastName") = "THUY"
        dtJE.Tables(0).Rows.Add(dr)

        dr = dtJE.Tables(0).NewRow
        dr("IsDefault") = "N"
        dr("Code") = "2"
        dr("FirstName") = "PHAM"
        'dr("MidName") = "THI"
        dr("LastName") = "LIEU"
        dtJE.Tables(0).Rows.Add(dr)

        Return dtJE
    End Function
    Public Function Simulate_OPRJ() As DataSet
        Dim dtJE = New DataSet
        dtJE.Tables.Add()
        dtJE.Tables(0).Columns.Add("PrjCode", GetType(String))
        dtJE.Tables(0).Columns.Add("PrjName", GetType(String))        

        Dim dr As DataRow
        dr = dtJE.Tables(0).NewRow
        dr("PrjCode") = "110"
        dr("PrjName") = "Project 1"
        dtJE.Tables(0).Rows.Add(dr)

        dr = dtJE.Tables(0).NewRow
        dr("PrjCode") = "220"
        dr("PrjName") = "Project 2"
        dtJE.Tables(0).Rows.Add(dr)

        Return dtJE
    End Function
    Public Function Simulate_OSHP() As DataSet
        Dim dtJE = New DataSet
        dtJE.Tables.Add()
        dtJE.Tables(0).Columns.Add("TrnspCode", GetType(String))
        dtJE.Tables(0).Columns.Add("TrnspName", GetType(String))

        Dim dr As DataRow
        dr = dtJE.Tables(0).NewRow
        dr("TrnspCode") = "1"
        dr("TrnspName") = "Giao hang - Thu tien"
        dtJE.Tables(0).Rows.Add(dr)

        dr = dtJE.Tables(0).NewRow
        dr("TrnspCode") = "2"
        dr("TrnspName") = "Thu Tien - Giao hang"
        dtJE.Tables(0).Rows.Add(dr)

        Return dtJE
    End Function
    Public Function Simulate_BPCurrency(CardCode) As DataSet
        Dim dtJE = New DataSet
        dtJE.Tables.Add()
        dtJE.Tables(0).Columns.Add("CurrCode", GetType(String))

        Dim dr As DataRow
        dr = dtJE.Tables(0).NewRow
        dr("CurrCode") = "VND"
        dtJE.Tables(0).Rows.Add(dr)

        dr = dtJE.Tables(0).NewRow
        dr("CurrCode") = "USD"
        dtJE.Tables(0).Rows.Add(dr)

        Return dtJE
    End Function
    Public Function Simulate_Promotion(UserID As String, ItemCode As String, CardCode As String, _
                                 Quantity As Double, DocDate As Date, Amount As Double) As DataSet
        Dim dtJE = New DataSet
        dtJE.Tables.Add()
        dtJE.Tables(0).Columns.Add("ID", GetType(String))
        dtJE.Tables(0).Columns.Add("ProCode", GetType(String))
        dtJE.Tables(0).Columns.Add("ProName", GetType(String))
        dtJE.Tables(0).Columns.Add("ProValue", GetType(Double))
        dtJE.Tables(0).Columns.Add("ProQty", GetType(Double))
        dtJE.Tables(0).Columns.Add("ProTrf", GetType(String))
        dtJE.Tables(0).Columns.Add("ItemCode", GetType(String))
        dtJE.Tables(0).Columns.Add("ItemName", GetType(String))
        dtJE.Tables(0).Columns.Add("Condition", GetType(Integer))
        dtJE.Tables(0).Columns.Add("IsReplace", GetType(Integer))
        dtJE.Tables(0).Columns.Add("Sole", GetType(Double))
        dtJE.Tables(0).Columns.Add("ChkCond", GetType(Integer))
        dtJE.Tables(0).Columns.Add("HeadDscAmt", GetType(Double))
        dtJE.Tables(0).Columns.Add("HeadDscPer", GetType(Double))
        dtJE.Tables(0).Columns.Add("WhsCode", GetType(String))

        Dim dr As DataRow
        dr = dtJE.Tables(0).NewRow
        dr("ID") = "1"
        dr("ProCode") = "Scenario1"
        dr("ProName") = "Mua 5 tang 1, nhan tien hoac san pham"
        dr("ProValue") = 8163
        dr("ProQty") = 1
        dr("ProTrf") = 0
        dr("ItemCode") = "ITM00001"
        dr("ItemName") = "Mi 3M"
        dr("Condition") = 1
        dr("IsReplace") = 0
        dr("Sole") = 0
        dr("ChkCond") = 1
        dr("HeadDscAmt") = 0
        dr("HeadDscPer") = 0
        dr("WhsCode") = "02"
        dtJE.Tables(0).Rows.Add(dr)

        Return dtJE
    End Function
    Public Function Simulate_GetDefaultInfo(UserID As String, ByVal cardCode As String, ByVal itemCode As String, _
                                       ByVal amount As Single, ByVal refDate As Date) As DataSet
        Dim dtJE = New DataSet
        dtJE.Tables.Add()
        dtJE.Tables(0).Columns.Add("UnitPrice", GetType(String))
        dtJE.Tables(0).Columns.Add("Discount", GetType(String))
        dtJE.Tables(0).Columns.Add("PriceAfDi", GetType(String))
        dtJE.Tables(0).Columns.Add("WhsCode", GetType(String))
        dtJE.Tables(0).Columns.Add("TaxCode", GetType(String))
        dtJE.Tables(0).Columns.Add("TaxRate", GetType(Double))

        Dim dr As DataRow
        dr = dtJE.Tables(0).NewRow
        dr("UnitPrice") = 200000
        dr("Discount") = 3.5
        dr("PriceAfDi") = 193000
        dr("WhsCode") = "01"
        dr("TaxCode") = "S10"
        dr("TaxRate") = 10
        dtJE.Tables(0).Rows.Add(dr)

        Return dtJE
    End Function
    Public Function Simulate_GetDeafaultBP(UserID As String, CardType As String) As DataSet
        Dim dtJE = New DataSet
        dtJE.Tables.Add()
        dtJE.Tables(0).Columns.Add("CardCode", GetType(String))
        dtJE.Tables(0).Columns.Add("CardName", GetType(String))

        Dim dr As DataRow
        dr = dtJE.Tables(0).NewRow
        dr("CardCode") = "V00001"
        dr("CardName") = "Default BP"
        dtJE.Tables(0).Rows.Add(dr)

        Return dtJE
    End Function
    Public Function Simulate_OIDC() As DataSet
        Dim dtJE = New DataSet
        dtJE.Tables.Add()
        dtJE.Tables(0).Columns.Add("Code", GetType(String))
        dtJE.Tables(0).Columns.Add("Name", GetType(String))

        Dim dr As DataRow
        dr = dtJE.Tables(0).NewRow
        dr("Code") = "1"
        dr("Name") = "Hoa don huy"
        dtJE.Tables(0).Rows.Add(dr)

        dr = dtJE.Tables(0).NewRow
        dr("Code") = "2"
        dr("Name") = "Hoa don tra"
        dtJE.Tables(0).Rows.Add(dr)

        Return dtJE
    End Function
    Public Function Simulate_OCTG() As DataSet
        Dim dtJE = New DataSet
        dtJE.Tables.Add()
        dtJE.Tables(0).Columns.Add("GroupNum", GetType(String))
        dtJE.Tables(0).Columns.Add("PymntGroup", GetType(String))

        Dim dr As DataRow
        dr = dtJE.Tables(0).NewRow
        dr("GroupNum") = "1"
        dr("PymntGroup") = "30days"
        dtJE.Tables(0).Rows.Add(dr)

        dr = dtJE.Tables(0).NewRow
        dr("GroupNum") = "2"
        dr("PymntGroup") = "60days"
        dtJE.Tables(0).Rows.Add(dr)

        Return dtJE
    End Function
    Public Function Simulate_OITB() As DataSet
        Dim dtJE = New DataSet
        dtJE.Tables.Add()
        dtJE.Tables(0).Columns.Add("ItmsGrpCod", GetType(String))
        dtJE.Tables(0).Columns.Add("ItmsGrpNam", GetType(String))

        Dim dr As DataRow
        dr = dtJE.Tables(0).NewRow
        dr("ItmsGrpCod") = "100"
        dr("ItmsGrpNam") = "Group 1"
        dtJE.Tables(0).Rows.Add(dr)

        dr = dtJE.Tables(0).NewRow
        dr("ItmsGrpCod") = "101"
        dr("ItmsGrpNam") = "Group 2"
        dtJE.Tables(0).Rows.Add(dr)

        Return dtJE
    End Function
    Public Function Simulate_OPLN() As DataSet
        Dim dtJE = New DataSet
        dtJE.Tables.Add()
        dtJE.Tables(0).Columns.Add("ListNum", GetType(String))
        dtJE.Tables(0).Columns.Add("ListName", GetType(String))

        Dim dr As DataRow
        dr = dtJE.Tables(0).NewRow
        dr("ListNum") = "1"
        dr("ListName") = "Price List 1"
        dtJE.Tables(0).Rows.Add(dr)

        dr = dtJE.Tables(0).NewRow
        dr("ItmsGrpCod") = "2"
        dr("ListNum") = "Price List 2"
        dtJE.Tables(0).Rows.Add(dr)

        Return dtJE
    End Function
    Public Function Simulate_OMRC() As DataSet
        Dim dtJE = New DataSet
        dtJE.Tables.Add()
        dtJE.Tables(0).Columns.Add("FirmCode", GetType(String))
        dtJE.Tables(0).Columns.Add("FirmName", GetType(String))

        Dim dr As DataRow
        dr = dtJE.Tables(0).NewRow
        dr("FirmCode") = "1"
        dr("FirmName") = "Nokia"
        dtJE.Tables(0).Rows.Add(dr)

        dr = dtJE.Tables(0).NewRow
        dr("FirmCode") = "2"
        dr("FirmName") = "Samsung"
        dtJE.Tables(0).Rows.Add(dr)

        Return dtJE
    End Function
    Public Function Simulate_OOND() As DataSet
        Dim dtJE = New DataSet
        dtJE.Tables.Add()
        dtJE.Tables(0).Columns.Add("IndCode", GetType(String))
        dtJE.Tables(0).Columns.Add("IndName", GetType(String))

        Dim dr As DataRow
        dr = dtJE.Tables(0).NewRow
        dr("IndCode") = "1"
        dr("IndName") = "Retail"
        dtJE.Tables(0).Rows.Add(dr)

        dr = dtJE.Tables(0).NewRow
        dr("IndCode") = "2"
        dr("IndName") = "Whole Sales"
        dtJE.Tables(0).Rows.Add(dr)

        Return dtJE
    End Function
    Public Function Simulate_OADM() As DataSet
        Dim dtJE = New DataSet
        dtJE.Tables.Add()
        dtJE.Tables(0).Columns.Add("CompnyName", GetType(String))
        dtJE.Tables(0).Columns.Add("DecSep", GetType(String))
        dtJE.Tables(0).Columns.Add("ThousSep", GetType(String))
        dtJE.Tables(0).Columns.Add("SumDec", GetType(Double))
        dtJE.Tables(0).Columns.Add("PriceDec", GetType(Double))
        dtJE.Tables(0).Columns.Add("QtyDec", GetType(Double))
        dtJE.Tables(0).Columns.Add("PercentDec", GetType(Double))
        dtJE.Tables(0).Columns.Add("RateDec", GetType(Double))

        Dim dr As DataRow
        dr = dtJE.Tables(0).NewRow
        dr("CompnyName") = "DMS DEMO"
        dr("DecSep") = "."
        dr("ThousSep") = ","
        dr("SumDec") = 0
        dr("PriceDec") = 0
        dr("QtyDec") = 0
        dr("PercentDec") = 2
        dr("RateDec") = 2
        dtJE.Tables(0).Rows.Add(dr)

        Return dtJE
    End Function
    Public Function Simulate_OPRC() As DataSet
        Dim dtJE = New DataSet
        dtJE.Tables.Add()
        dtJE.Tables(0).Columns.Add("PrcCode", GetType(String))
        dtJE.Tables(0).Columns.Add("PrcName", GetType(String))

        Dim dr As DataRow
        dr = dtJE.Tables(0).NewRow
        dr("PrcCode") = "AA"
        dr("PrcName") = "AA"
        dtJE.Tables(0).Rows.Add(dr)

        dr = dtJE.Tables(0).NewRow
        dr("PrcCode") = "BB"
        dr("PrcName") = "BB"
        dtJE.Tables(0).Rows.Add(dr)

        Return dtJE
    End Function
    Public Function Simulate_OPOR() As String
        Dim str As String
        str = "<BOM><BO><AdmInfo><Object>22</Object></AdmInfo><OPOR><row><DocEntry>3</DocEntry><DocNum>3</DocNum><DocType>I</DocType><Handwrtten>N</Handwrtten><Printed>N</Printed><DocDate>20120803</DocDate><DocDueDate>20120803</DocDueDate><CardCode>V00001</CardCode><CardName>CTY TNHH VIETHUNG FOOD</CardName><Address></Address><NumAtCard></NumAtCard><DocCur>VND</DocCur><DocRate>1.000000</DocRate><DocTotal>55000000.000000</DocTotal><Ref1>3</Ref1><Ref2></Ref2><Comments></Comments><JrnlMemo>Purchase Orders - V00001</JrnlMemo><GroupNum>-1</GroupNum><DocTime>1355</DocTime><SlpCode>-1</SlpCode><TrnspCode>-1</TrnspCode><Confirmed>Y</Confirmed><ImportEnt>0</ImportEnt><SummryType>N</SummryType><CntctCode>0</CntctCode><ShowSCN>N</ShowSCN><Series>13</Series><TaxDate>20120803</TaxDate><PartSupply></PartSupply><ObjType>22</ObjType><ShipToCode></ShipToCode><Indicator></Indicator><LicTradNum></LicTradNum><DiscPrcnt>0.000000</DiscPrcnt><PaymentRef></PaymentRef><CreateDate>20120803</CreateDate><UpdateDate>20120803</UpdateDate><FinncPriod>8</FinncPriod><TransId>0</TransId><VatSum>5000000.000000</VatSum><VatSumSy>5000000.000000</VatSumSy><VatSumFC>0.000000</VatSumFC><NetProc>N</NetProc><DocTotalFC>0.000000</DocTotalFC><DocTotalSy>55000000.000000</DocTotalSy><Form1099>0</Form1099><Box1099></Box1099><RevisionPo>N</RevisionPo><ReqDate></ReqDate><CancelDate></CancelDate><BlockDunn>N</BlockDunn><submitted>N</submitted><Segment>0</Segment><PickStatus>N</PickStatus><Pick>N</Pick><PeyMethod></PeyMethod><PayBlock>N</PayBlock><PayBlckRef>0</PayBlckRef><CntrlBnk></CntrlBnk><MaxDscn>N</MaxDscn><Reserve>N</Reserve><Project></Project><FromDate></FromDate><ToDate></ToDate><UpdInvnt>O</UpdInvnt><Rounding>N</Rounding><CorrExt></CorrExt><CorrInv>0</CorrInv><NCorrInv>0</NCorrInv><DeferrTax>N</DeferrTax><LetterNum></LetterNum><WTApplied>0.000000</WTApplied><WTAppliedF>0.000000</WTAppliedF><BoeReserev>N</BoeReserev><AgentCode></AgentCode><WTAppliedS>0.000000</WTAppliedS><EquVatSum>0.000000</EquVatSum><EquVatSumF>0.000000</EquVatSumF><EquVatSumS>0.000000</EquVatSumS><Installmnt>1</Installmnt><VATFirst>N</VATFirst><NnSbAmnt>0.000000</NnSbAmnt><NnSbAmntSC>0.000000</NnSbAmntSC><NbSbAmntFC>0.000000</NbSbAmntFC><ExepAmnt>0.000000</ExepAmnt><ExepAmntSC>0.000000</ExepAmntSC><ExepAmntFC>0.000000</ExepAmntFC><BaseAmnt>0.000000</BaseAmnt><BaseAmntSC>0.000000</BaseAmntSC><BaseAmntFC>0.000000</BaseAmntFC><WTSum>0.000000</WTSum><WTSumSC>0.000000</WTSumSC><WTSumFC>0.000000</WTSumFC><VatDate></VatDate><OwnerCode>0</OwnerCode><FolioPref></FolioPref><FolioNum>0</FolioNum><DocSubType>--</DocSubType><BPChCode></BPChCode><BPChCntc>0</BPChCntc><Address2>VIETNAM</Address2><DocStatus>C</DocStatus><PIndicator>Default</PIndicator><PayToCode></PayToCode><ManualNum></ManualNum><UseShpdGd>N</UseShpdGd><IsPaytoBnk>N</IsPaytoBnk><BnkCntry></BnkCntry><BankCode></BankCode><BnkAccount></BnkAccount><BnkBranch></BnkBranch><BPLId>0</BPLId><DpmPrcnt>0.000000</DpmPrcnt><isIns>N</isIns><LangCode>8</LangCode><TrackNo></TrackNo><PickRmrk></PickRmrk><ClsDate></ClsDate><SeqCode>0</SeqCode><Serial>0</Serial><SeriesStr></SeriesStr><SubStr></SubStr><Model>0</Model><UseCorrVat>N</UseCorrVat><DiscSum>0.000000</DiscSum><DpmAmnt>0.000000</DpmAmnt><DpmPrcnt>0.000000</DpmPrcnt><Posted>Y</Posted><DpmAmntSC>0.000000</DpmAmntSC><DpmAmntFC>0.000000</DpmAmntFC><VatPercent>0.000000</VatPercent><SrvGpPrcnt>0.000000</SrvGpPrcnt><Header></Header><Footer></Footer><RoundDif>0.000000</RoundDif><RoundDifFC>0.000000</RoundDifFC><RoundDifSy>0.000000</RoundDifSy><CANCELED>N</CANCELED><SignMsg></SignMsg><SignDigest></SignDigest><CertifNum></CertifNum><KeyVersion>0</KeyVersion><CtlAccount>331111</CtlAccount><InsurOp347>N</InsurOp347><IgnRelDoc>N</IgnRelDoc><Checker>0</Checker><Payee>0</Payee><ExtraMonth>0</ExtraMonth><ExtraDays>0</ExtraDays><CdcOffset>0</CdcOffset><PayDuMonth>N</PayDuMonth><NTSApprov>N</NTSApprov><NTSWebSite>0</NTSWebSite><NTSeTaxNo></NTSeTaxNo><NTSApprNo></NTSApprNo><EDocGenTyp>N</EDocGenTyp><ESeries>0</ESeries><EDocNum></EDocNum><EDocExpFrm>0</EDocExpFrm><EDocStatus>C</EDocStatus><EDocErrCod></EDocErrCod><EDocErrMsg></EDocErrMsg><DpmStatus>O</DpmStatus><PQTGrpSer>0</PQTGrpSer><PQTGrpNum>0</PQTGrpNum><PQTGrpHW>N</PQTGrpHW><ReopOriDoc></ReopOriDoc><ReopManCls></ReopManCls><OnlineQuo>N</OnlineQuo><POSEqNum></POSEqNum><POSManufSN></POSManufSN><POSCashN>0</POSCashN><DpmAsDscnt>N</DpmAsDscnt><ClosingOpt>1</ClosingOpt><SpecDate></SpecDate><OpenForLaC>Y</OpenForLaC><WddStatus>-</WddStatus><DiscSumFC>0.000000</DiscSumFC><DiscSumSy>0.000000</DiscSumSy><GTSRlvnt>N</GTSRlvnt><BPLName></BPLName><VATRegNum></VATRegNum><U_CXS_MRP>N</U_CXS_MRP><U_Inv_Reason></U_Inv_Reason><U_UserID></U_UserID></row></OPOR><POR1><row><LineNum>0</LineNum><ItemCode>ITM00001</ItemCode><Dscription>65/30 3 miền Mì tôm chua cay</Dscription><Quantity>100.000000</Quantity><ShipDate>20120803</ShipDate><Price>100000.000000</Price><PriceAfVAT>110000.000000</PriceAfVAT><Currency>VND</Currency><Rate>0.000000</Rate><DiscPrcnt>0.000000</DiscPrcnt><VendorNum></VendorNum><SerialNum></SerialNum><WhsCode>01</WhsCode><SlpCode>-1</SlpCode><Commission>0.000000</Commission><TreeType>N</TreeType><AcctCode>156111</AcctCode><UseBaseUn>N</UseBaseUn><SubCatNum></SubCatNum><OcrCode></OcrCode><Project></Project><CodeBars></CodeBars><VatGroup>P2</VatGroup><Height1>0.000000</Height1><Hght1Unit>0</Hght1Unit><Height2>0.000000</Height2><Hght2Unit>0</Hght2Unit><Length1>0.000000</Length1><Len1Unit>0</Len1Unit><length2>0.000000</length2><Len2Unit>0</Len2Unit><Weight1>0.000000</Weight1><Wght1Unit>0</Wght1Unit><Weight2>0.000000</Weight2><Wght2Unit>0</Wght2Unit><Factor1>1.000000</Factor1><Factor2>1.000000</Factor2><Factor3>1.000000</Factor3><Factor4>1.000000</Factor4><BaseType>-1</BaseType><BaseEntry>0</BaseEntry><BaseLine>0</BaseLine><Volume>0.000000</Volume><VolUnit>4</VolUnit><Width1>0.000000</Width1><Wdth1Unit>0</Wdth1Unit><Width2>0.000000</Width2><Wdth2Unit>0</Wdth2Unit><Address></Address><TaxCode></TaxCode><TaxType>Y</TaxType><TaxStatus>Y</TaxStatus><PickStatus>N</PickStatus><PickOty>0.000000</PickOty><PickIdNo>0</PickIdNo><OrigItem></OrigItem><BackOrdr></BackOrdr><FreeTxt></FreeTxt><TrnsCode>-1</TrnsCode><PoTrgNum>0</PoTrgNum><PoTrgEntry></PoTrgEntry><PoLineNum>0</PoLineNum><CEECFlag>S</CEECFlag><ToStock>0.000000</ToStock><ToDiff>0.000000</ToDiff><VatAppld>1000000.000000</VatAppld><VatAppldFC>0.000000</VatAppldFC><VatAppldSC>1000000.000000</VatAppldSC><WtLiable>N</WtLiable><DeferrTax>N</DeferrTax><EquVatPer>0.000000</EquVatPer><EquVatSum>0.000000</EquVatSum><EquVatSumF>0.000000</EquVatSumF><EquVatSumS>0.000000</EquVatSumS><LineVat>1000000.000000</LineVat><LineVatlF>0.000000</LineVatlF><LineVatS>1000000.000000</LineVatS><unitMsr>EA</unitMsr><NumPerMsr>1.000000</NumPerMsr><LineTotal>10000000.000000</LineTotal><VatPrcnt>10.000000</VatPrcnt><VatSum>1000000.000000</VatSum><ConsumeFCT>N</ConsumeFCT><ExciseAmt>0.000000</ExciseAmt><TaxPerUnit>0.000000</TaxPerUnit><TotInclTax>0.000000</TotInclTax><CountryOrg></CountryOrg><SWW></SWW><TranType></TranType><DistribExp>Y</DistribExp><ShipToCode></ShipToCode><TotalFrgn>0.000000</TotalFrgn><TotalSumSy>10000000.000000</TotalSumSy><LstBINMPr>0.000000</LstBINMPr><LstByDsFc>0.000000</LstByDsFc><LstByDsSc>0.000000</LstByDsSc><LstByDsSum>0.000000</LstByDsSum><StckDstFc>0.000000</StckDstFc><StckDstSc>0.000000</StckDstSc><StckDstSum>0.000000</StckDstSum><StckINMPr>0.000000</StckINMPr><PickStatus>N</PickStatus><VatWoDpm>0.000000</VatWoDpm><VatWoDpmFc>0.000000</VatWoDpmFc><VatWoDpmSc>0.000000</VatWoDpmSc><CFOPCode></CFOPCode><CSTCode></CSTCode><Usage>0</Usage><TaxOnly>N</TaxOnly><VisOrder>0</VisOrder><BaseOpnQty>0.000000</BaseOpnQty><PriceBefDi>100000.000000</PriceBefDi><LineStatus>C</LineStatus><PackQty>100.000000</PackQty><FreeTxt></FreeTxt><LineType>R</LineType><CogsOcrCod></CogsOcrCod><CogsAcct></CogsAcct><ChgAsmBoMW>N</ChgAsmBoMW><GrossBuyPr>0.000000</GrossBuyPr><GrossBase>0</GrossBase><GPTtlBasPr>0.000000</GPTtlBasPr><OcrCode2></OcrCode2><OcrCode3></OcrCode3><OcrCode4></OcrCode4><OcrCode5></OcrCode5><Text></Text><LocCode>0</LocCode><ActDelDate></ActDelDate><OpenQty>0.000000</OpenQty><OpenSum>10000000.000000</OpenSum><OpenSumFC>0.000000</OpenSumFC><OpenSumSys>10000000.000000</OpenSumSys><ExLineNo></ExLineNo><PQTReqDate></PQTReqDate><PQTReqQty>0.000000</PQTReqQty><CogsOcrCo2></CogsOcrCo2><CogsOcrCo3></CogsOcrCo3><CogsOcrCo4></CogsOcrCo4><CogsOcrCo5></CogsOcrCo5><CSTfIPI></CSTfIPI><CSTfPIS></CSTfPIS><CSTfCOFINS></CSTfCOFINS><CredOrigin></CredOrigin><NoInvtryMv>N</NoInvtryMv><AgrNo>0</AgrNo><AgrLnNum>0</AgrLnNum><ShipToDesc>VIETNAM</ShipToDesc><ActBaseEnt>0</ActBaseEnt><ActBaseLn>0</ActBaseLn><DocEntry>3</DocEntry><U_CXS_MRP_RECMD>N</U_CXS_MRP_RECMD><U_ProID></U_ProID><U_OrgPrice>0.000000</U_OrgPrice><U_OrgQty>0.000000</U_OrgQty><U_ProLine></U_ProLine><U_Pro_Sole>0.000000</U_Pro_Sole><U_Pro_UseSole></U_Pro_UseSole><U_Pro_ByPro>0.000000</U_Pro_ByPro><U_Campaign></U_Campaign></row><row><LineNum>1</LineNum><ItemCode>ITM00002</ItemCode><Dscription>65/30 3 miền Mì tôm hùm</Dscription><Quantity>100.000000</Quantity><ShipDate>20120803</ShipDate><Price>100000.000000</Price><PriceAfVAT>110000.000000</PriceAfVAT><Currency>VND</Currency><Rate>0.000000</Rate><DiscPrcnt>0.000000</DiscPrcnt><VendorNum></VendorNum><SerialNum></SerialNum><WhsCode>01</WhsCode><SlpCode>-1</SlpCode><Commission>0.000000</Commission><TreeType>N</TreeType><AcctCode>156111</AcctCode><UseBaseUn>N</UseBaseUn><SubCatNum></SubCatNum><OcrCode></OcrCode><Project></Project><CodeBars></CodeBars><VatGroup>P2</VatGroup><Height1>0.000000</Height1><Hght1Unit>0</Hght1Unit><Height2>0.000000</Height2><Hght2Unit>0</Hght2Unit><Length1>0.000000</Length1><Len1Unit>0</Len1Unit><length2>0.000000</length2><Len2Unit>0</Len2Unit><Weight1>0.000000</Weight1><Wght1Unit>0</Wght1Unit><Weight2>0.000000</Weight2><Wght2Unit>0</Wght2Unit><Factor1>1.000000</Factor1><Factor2>1.000000</Factor2><Factor3>1.000000</Factor3><Factor4>1.000000</Factor4><BaseType>-1</BaseType><BaseEntry>0</BaseEntry><BaseLine>0</BaseLine><Volume>0.000000</Volume><VolUnit>4</VolUnit><Width1>0.000000</Width1><Wdth1Unit>0</Wdth1Unit><Width2>0.000000</Width2><Wdth2Unit>0</Wdth2Unit><Address></Address><TaxCode></TaxCode><TaxType>Y</TaxType><TaxStatus>Y</TaxStatus><PickStatus>N</PickStatus><PickOty>0.000000</PickOty><PickIdNo>0</PickIdNo><OrigItem></OrigItem><BackOrdr></BackOrdr><FreeTxt></FreeTxt><TrnsCode>-1</TrnsCode><PoTrgNum>0</PoTrgNum><PoTrgEntry></PoTrgEntry><PoLineNum>0</PoLineNum><CEECFlag>S</CEECFlag><ToStock>0.000000</ToStock><ToDiff>0.000000</ToDiff><VatAppld>1000000.000000</VatAppld><VatAppldFC>0.000000</VatAppldFC><VatAppldSC>1000000.000000</VatAppldSC><WtLiable>N</WtLiable><DeferrTax>N</DeferrTax><EquVatPer>0.000000</EquVatPer><EquVatSum>0.000000</EquVatSum><EquVatSumF>0.000000</EquVatSumF><EquVatSumS>0.000000</EquVatSumS><LineVat>1000000.000000</LineVat><LineVatlF>0.000000</LineVatlF><LineVatS>1000000.000000</LineVatS><unitMsr>EA</unitMsr><NumPerMsr>1.000000</NumPerMsr><LineTotal>10000000.000000</LineTotal><VatPrcnt>10.000000</VatPrcnt><VatSum>1000000.000000</VatSum><ConsumeFCT>N</ConsumeFCT><ExciseAmt>0.000000</ExciseAmt><TaxPerUnit>0.000000</TaxPerUnit><TotInclTax>0.000000</TotInclTax><CountryOrg></CountryOrg><SWW></SWW><TranType></TranType><DistribExp>Y</DistribExp><ShipToCode></ShipToCode><TotalFrgn>0.000000</TotalFrgn><TotalSumSy>10000000.000000</TotalSumSy><LstBINMPr>0.000000</LstBINMPr><LstByDsFc>0.000000</LstByDsFc><LstByDsSc>0.000000</LstByDsSc><LstByDsSum>0.000000</LstByDsSum><StckDstFc>0.000000</StckDstFc><StckDstSc>0.000000</StckDstSc><StckDstSum>0.000000</StckDstSum><StckINMPr>0.000000</StckINMPr><PickStatus>N</PickStatus><VatWoDpm>0.000000</VatWoDpm><VatWoDpmFc>0.000000</VatWoDpmFc><VatWoDpmSc>0.000000</VatWoDpmSc><CFOPCode></CFOPCode><CSTCode></CSTCode><Usage>0</Usage><TaxOnly>N</TaxOnly><VisOrder>1</VisOrder><BaseOpnQty>0.000000</BaseOpnQty><PriceBefDi>100000.000000</PriceBefDi><LineStatus>C</LineStatus><PackQty>100.000000</PackQty><FreeTxt></FreeTxt><LineType>R</LineType><CogsOcrCod></CogsOcrCod><CogsAcct></CogsAcct><ChgAsmBoMW>N</ChgAsmBoMW><GrossBuyPr>0.000000</GrossBuyPr><GrossBase>0</GrossBase><GPTtlBasPr>0.000000</GPTtlBasPr><OcrCode2></OcrCode2><OcrCode3></OcrCode3><OcrCode4></OcrCode4><OcrCode5></OcrCode5><Text></Text><LocCode>0</LocCode><ActDelDate></ActDelDate><OpenQty>0.000000</OpenQty><OpenSum>10000000.000000</OpenSum><OpenSumFC>0.000000</OpenSumFC><OpenSumSys>10000000.000000</OpenSumSys><ExLineNo></ExLineNo><PQTReqDate></PQTReqDate><PQTReqQty>0.000000</PQTReqQty><CogsOcrCo2></CogsOcrCo2><CogsOcrCo3></CogsOcrCo3><CogsOcrCo4></CogsOcrCo4><CogsOcrCo5></CogsOcrCo5><CSTfIPI></CSTfIPI><CSTfPIS></CSTfPIS><CSTfCOFINS></CSTfCOFINS><CredOrigin></CredOrigin><NoInvtryMv>N</NoInvtryMv><AgrNo>0</AgrNo><AgrLnNum>0</AgrLnNum><ShipToDesc>VIETNAM</ShipToDesc><ActBaseEnt>0</ActBaseEnt><ActBaseLn>0</ActBaseLn><DocEntry>3</DocEntry><U_CXS_MRP_RECMD>N</U_CXS_MRP_RECMD><U_ProID></U_ProID><U_OrgPrice>0.000000</U_OrgPrice><U_OrgQty>0.000000</U_OrgQty><U_ProLine></U_ProLine><U_Pro_Sole>0.000000</U_Pro_Sole><U_Pro_UseSole></U_Pro_UseSole><U_Pro_ByPro>0.000000</U_Pro_ByPro><U_Campaign></U_Campaign></row><row><LineNum>2</LineNum><ItemCode>ITM00003</ItemCode><Dscription>65/30 3 miền Mì chay lá đa</Dscription><Quantity>100.000000</Quantity><ShipDate>20120803</ShipDate><Price>100000.000000</Price><PriceAfVAT>110000.000000</PriceAfVAT><Currency>VND</Currency><Rate>0.000000</Rate><DiscPrcnt>0.000000</DiscPrcnt><VendorNum></VendorNum><SerialNum></SerialNum><WhsCode>01</WhsCode><SlpCode>-1</SlpCode><Commission>0.000000</Commission><TreeType>N</TreeType><AcctCode>156111</AcctCode><UseBaseUn>N</UseBaseUn><SubCatNum></SubCatNum><OcrCode></OcrCode><Project></Project><CodeBars></CodeBars><VatGroup>P2</VatGroup><Height1>0.000000</Height1><Hght1Unit>0</Hght1Unit><Height2>0.000000</Height2><Hght2Unit>0</Hght2Unit><Length1>0.000000</Length1><Len1Unit>0</Len1Unit><length2>0.000000</length2><Len2Unit>0</Len2Unit><Weight1>0.000000</Weight1><Wght1Unit>0</Wght1Unit><Weight2>0.000000</Weight2><Wght2Unit>0</Wght2Unit><Factor1>1.000000</Factor1><Factor2>1.000000</Factor2><Factor3>1.000000</Factor3><Factor4>1.000000</Factor4><BaseType>-1</BaseType><BaseEntry>0</BaseEntry><BaseLine>0</BaseLine><Volume>0.000000</Volume><VolUnit>4</VolUnit><Width1>0.000000</Width1><Wdth1Unit>0</Wdth1Unit><Width2>0.000000</Width2><Wdth2Unit>0</Wdth2Unit><Address></Address><TaxCode></TaxCode><TaxType>Y</TaxType><TaxStatus>Y</TaxStatus><PickStatus>N</PickStatus><PickOty>0.000000</PickOty><PickIdNo>0</PickIdNo><OrigItem></OrigItem><BackOrdr></BackOrdr><FreeTxt></FreeTxt><TrnsCode>-1</TrnsCode><PoTrgNum>0</PoTrgNum><PoTrgEntry></PoTrgEntry><PoLineNum>0</PoLineNum><CEECFlag>S</CEECFlag><ToStock>0.000000</ToStock><ToDiff>0.000000</ToDiff><VatAppld>1000000.000000</VatAppld><VatAppldFC>0.000000</VatAppldFC><VatAppldSC>1000000.000000</VatAppldSC><WtLiable>N</WtLiable><DeferrTax>N</DeferrTax><EquVatPer>0.000000</EquVatPer><EquVatSum>0.000000</EquVatSum><EquVatSumF>0.000000</EquVatSumF><EquVatSumS>0.000000</EquVatSumS><LineVat>1000000.000000</LineVat><LineVatlF>0.000000</LineVatlF><LineVatS>1000000.000000</LineVatS><unitMsr>EA</unitMsr><NumPerMsr>1.000000</NumPerMsr><LineTotal>10000000.000000</LineTotal><VatPrcnt>10.000000</VatPrcnt><VatSum>1000000.000000</VatSum><ConsumeFCT>N</ConsumeFCT><ExciseAmt>0.000000</ExciseAmt><TaxPerUnit>0.000000</TaxPerUnit><TotInclTax>0.000000</TotInclTax><CountryOrg></CountryOrg><SWW></SWW><TranType></TranType><DistribExp>Y</DistribExp><ShipToCode></ShipToCode><TotalFrgn>0.000000</TotalFrgn><TotalSumSy>10000000.000000</TotalSumSy><LstBINMPr>0.000000</LstBINMPr><LstByDsFc>0.000000</LstByDsFc><LstByDsSc>0.000000</LstByDsSc><LstByDsSum>0.000000</LstByDsSum><StckDstFc>0.000000</StckDstFc><StckDstSc>0.000000</StckDstSc><StckDstSum>0.000000</StckDstSum><StckINMPr>0.000000</StckINMPr><PickStatus>N</PickStatus><VatWoDpm>0.000000</VatWoDpm><VatWoDpmFc>0.000000</VatWoDpmFc><VatWoDpmSc>0.000000</VatWoDpmSc><CFOPCode></CFOPCode><CSTCode></CSTCode><Usage>0</Usage><TaxOnly>N</TaxOnly><VisOrder>2</VisOrder><BaseOpnQty>0.000000</BaseOpnQty><PriceBefDi>100000.000000</PriceBefDi><LineStatus>C</LineStatus><PackQty>100.000000</PackQty><FreeTxt></FreeTxt><LineType>R</LineType><CogsOcrCod></CogsOcrCod><CogsAcct></CogsAcct><ChgAsmBoMW>N</ChgAsmBoMW><GrossBuyPr>0.000000</GrossBuyPr><GrossBase>0</GrossBase><GPTtlBasPr>0.000000</GPTtlBasPr><OcrCode2></OcrCode2><OcrCode3></OcrCode3><OcrCode4></OcrCode4><OcrCode5></OcrCode5><Text></Text><LocCode>0</LocCode><ActDelDate></ActDelDate><OpenQty>0.000000</OpenQty><OpenSum>10000000.000000</OpenSum><OpenSumFC>0.000000</OpenSumFC><OpenSumSys>10000000.000000</OpenSumSys><ExLineNo></ExLineNo><PQTReqDate></PQTReqDate><PQTReqQty>0.000000</PQTReqQty><CogsOcrCo2></CogsOcrCo2><CogsOcrCo3></CogsOcrCo3><CogsOcrCo4></CogsOcrCo4><CogsOcrCo5></CogsOcrCo5><CSTfIPI></CSTfIPI><CSTfPIS></CSTfPIS><CSTfCOFINS></CSTfCOFINS><CredOrigin></CredOrigin><NoInvtryMv>N</NoInvtryMv><AgrNo>0</AgrNo><AgrLnNum>0</AgrLnNum><ShipToDesc>VIETNAM</ShipToDesc><ActBaseEnt>0</ActBaseEnt><ActBaseLn>0</ActBaseLn><DocEntry>3</DocEntry><U_CXS_MRP_RECMD>N</U_CXS_MRP_RECMD><U_ProID></U_ProID><U_OrgPrice>0.000000</U_OrgPrice><U_OrgQty>0.000000</U_OrgQty><U_ProLine></U_ProLine><U_Pro_Sole>0.000000</U_Pro_Sole><U_Pro_UseSole></U_Pro_UseSole><U_Pro_ByPro>0.000000</U_Pro_ByPro><U_Campaign></U_Campaign></row><row><LineNum>3</LineNum><ItemCode>ITM00004</ItemCode><Dscription>65/30 3 miền Mì thịt bằm xào hành</Dscription><Quantity>100.000000</Quantity><ShipDate>20120803</ShipDate><Price>100000.000000</Price><PriceAfVAT>110000.000000</PriceAfVAT><Currency>VND</Currency><Rate>0.000000</Rate><DiscPrcnt>0.000000</DiscPrcnt><VendorNum></VendorNum><SerialNum></SerialNum><WhsCode>01</WhsCode><SlpCode>-1</SlpCode><Commission>0.000000</Commission><TreeType>N</TreeType><AcctCode>156111</AcctCode><UseBaseUn>N</UseBaseUn><SubCatNum></SubCatNum><OcrCode></OcrCode><Project></Project><CodeBars></CodeBars><VatGroup>P2</VatGroup><Height1>0.000000</Height1><Hght1Unit>0</Hght1Unit><Height2>0.000000</Height2><Hght2Unit>0</Hght2Unit><Length1>0.000000</Length1><Len1Unit>0</Len1Unit><length2>0.000000</length2><Len2Unit>0</Len2Unit><Weight1>0.000000</Weight1><Wght1Unit>0</Wght1Unit><Weight2>0.000000</Weight2><Wght2Unit>0</Wght2Unit><Factor1>1.000000</Factor1><Factor2>1.000000</Factor2><Factor3>1.000000</Factor3><Factor4>1.000000</Factor4><BaseType>-1</BaseType><BaseEntry>0</BaseEntry><BaseLine>0</BaseLine><Volume>0.000000</Volume><VolUnit>4</VolUnit><Width1>0.000000</Width1><Wdth1Unit>0</Wdth1Unit><Width2>0.000000</Width2><Wdth2Unit>0</Wdth2Unit><Address></Address><TaxCode></TaxCode><TaxType>Y</TaxType><TaxStatus>Y</TaxStatus><PickStatus>N</PickStatus><PickOty>0.000000</PickOty><PickIdNo>0</PickIdNo><OrigItem></OrigItem><BackOrdr></BackOrdr><FreeTxt></FreeTxt><TrnsCode>-1</TrnsCode><PoTrgNum>0</PoTrgNum><PoTrgEntry></PoTrgEntry><PoLineNum>0</PoLineNum><CEECFlag>S</CEECFlag><ToStock>0.000000</ToStock><ToDiff>0.000000</ToDiff><VatAppld>1000000.000000</VatAppld><VatAppldFC>0.000000</VatAppldFC><VatAppldSC>1000000.000000</VatAppldSC><WtLiable>N</WtLiable><DeferrTax>N</DeferrTax><EquVatPer>0.000000</EquVatPer><EquVatSum>0.000000</EquVatSum><EquVatSumF>0.000000</EquVatSumF><EquVatSumS>0.000000</EquVatSumS><LineVat>1000000.000000</LineVat><LineVatlF>0.000000</LineVatlF><LineVatS>1000000.000000</LineVatS><unitMsr>EA</unitMsr><NumPerMsr>1.000000</NumPerMsr><LineTotal>10000000.000000</LineTotal><VatPrcnt>10.000000</VatPrcnt><VatSum>1000000.000000</VatSum><ConsumeFCT>N</ConsumeFCT><ExciseAmt>0.000000</ExciseAmt><TaxPerUnit>0.000000</TaxPerUnit><TotInclTax>0.000000</TotInclTax><CountryOrg></CountryOrg><SWW></SWW><TranType></TranType><DistribExp>Y</DistribExp><ShipToCode></ShipToCode><TotalFrgn>0.000000</TotalFrgn><TotalSumSy>10000000.000000</TotalSumSy><LstBINMPr>0.000000</LstBINMPr><LstByDsFc>0.000000</LstByDsFc><LstByDsSc>0.000000</LstByDsSc><LstByDsSum>0.000000</LstByDsSum><StckDstFc>0.000000</StckDstFc><StckDstSc>0.000000</StckDstSc><StckDstSum>0.000000</StckDstSum><StckINMPr>0.000000</StckINMPr><PickStatus>N</PickStatus><VatWoDpm>0.000000</VatWoDpm><VatWoDpmFc>0.000000</VatWoDpmFc><VatWoDpmSc>0.000000</VatWoDpmSc><CFOPCode></CFOPCode><CSTCode></CSTCode><Usage>0</Usage><TaxOnly>N</TaxOnly><VisOrder>3</VisOrder><BaseOpnQty>0.000000</BaseOpnQty><PriceBefDi>100000.000000</PriceBefDi><LineStatus>C</LineStatus><PackQty>100.000000</PackQty><FreeTxt></FreeTxt><LineType>R</LineType><CogsOcrCod></CogsOcrCod><CogsAcct></CogsAcct><ChgAsmBoMW>N</ChgAsmBoMW><GrossBuyPr>0.000000</GrossBuyPr><GrossBase>0</GrossBase><GPTtlBasPr>0.000000</GPTtlBasPr><OcrCode2></OcrCode2><OcrCode3></OcrCode3><OcrCode4></OcrCode4><OcrCode5></OcrCode5><Text></Text><LocCode>0</LocCode><ActDelDate></ActDelDate><OpenQty>0.000000</OpenQty><OpenSum>10000000.000000</OpenSum><OpenSumFC>0.000000</OpenSumFC><OpenSumSys>10000000.000000</OpenSumSys><ExLineNo></ExLineNo><PQTReqDate></PQTReqDate><PQTReqQty>0.000000</PQTReqQty><CogsOcrCo2></CogsOcrCo2><CogsOcrCo3></CogsOcrCo3><CogsOcrCo4></CogsOcrCo4><CogsOcrCo5></CogsOcrCo5><CSTfIPI></CSTfIPI><CSTfPIS></CSTfPIS><CSTfCOFINS></CSTfCOFINS><CredOrigin></CredOrigin><NoInvtryMv>N</NoInvtryMv><AgrNo>0</AgrNo><AgrLnNum>0</AgrLnNum><ShipToDesc>VIETNAM</ShipToDesc><ActBaseEnt>0</ActBaseEnt><ActBaseLn>0</ActBaseLn><DocEntry>3</DocEntry><U_CXS_MRP_RECMD>N</U_CXS_MRP_RECMD><U_ProID></U_ProID><U_OrgPrice>0.000000</U_OrgPrice><U_OrgQty>0.000000</U_OrgQty><U_ProLine></U_ProLine><U_Pro_Sole>0.000000</U_Pro_Sole><U_Pro_UseSole></U_Pro_UseSole><U_Pro_ByPro>0.000000</U_Pro_ByPro><U_Campaign></U_Campaign></row><row><LineNum>4</LineNum><ItemCode>ITM00005</ItemCode><Dscription>65/30 3 miền Mì bò xào rau thơm</Dscription><Quantity>100.000000</Quantity><ShipDate>20120803</ShipDate><Price>100000.000000</Price><PriceAfVAT>110000.000000</PriceAfVAT><Currency>VND</Currency><Rate>0.000000</Rate><DiscPrcnt>0.000000</DiscPrcnt><VendorNum></VendorNum><SerialNum></SerialNum><WhsCode>01</WhsCode><SlpCode>-1</SlpCode><Commission>0.000000</Commission><TreeType>N</TreeType><AcctCode>156111</AcctCode><UseBaseUn>N</UseBaseUn><SubCatNum></SubCatNum><OcrCode></OcrCode><Project></Project><CodeBars></CodeBars><VatGroup>P2</VatGroup><Height1>0.000000</Height1><Hght1Unit>0</Hght1Unit><Height2>0.000000</Height2><Hght2Unit>0</Hght2Unit><Length1>0.000000</Length1><Len1Unit>0</Len1Unit><length2>0.000000</length2><Len2Unit>0</Len2Unit><Weight1>0.000000</Weight1><Wght1Unit>0</Wght1Unit><Weight2>0.000000</Weight2><Wght2Unit>0</Wght2Unit><Factor1>1.000000</Factor1><Factor2>1.000000</Factor2><Factor3>1.000000</Factor3><Factor4>1.000000</Factor4><BaseType>-1</BaseType><BaseEntry>0</BaseEntry><BaseLine>0</BaseLine><Volume>0.000000</Volume><VolUnit>4</VolUnit><Width1>0.000000</Width1><Wdth1Unit>0</Wdth1Unit><Width2>0.000000</Width2><Wdth2Unit>0</Wdth2Unit><Address></Address><TaxCode></TaxCode><TaxType>Y</TaxType><TaxStatus>Y</TaxStatus><PickStatus>N</PickStatus><PickOty>0.000000</PickOty><PickIdNo>0</PickIdNo><OrigItem></OrigItem><BackOrdr></BackOrdr><FreeTxt></FreeTxt><TrnsCode>-1</TrnsCode><PoTrgNum>0</PoTrgNum><PoTrgEntry></PoTrgEntry><PoLineNum>0</PoLineNum><CEECFlag>S</CEECFlag><ToStock>0.000000</ToStock><ToDiff>0.000000</ToDiff><VatAppld>1000000.000000</VatAppld><VatAppldFC>0.000000</VatAppldFC><VatAppldSC>1000000.000000</VatAppldSC><WtLiable>N</WtLiable><DeferrTax>N</DeferrTax><EquVatPer>0.000000</EquVatPer><EquVatSum>0.000000</EquVatSum><EquVatSumF>0.000000</EquVatSumF><EquVatSumS>0.000000</EquVatSumS><LineVat>1000000.000000</LineVat><LineVatlF>0.000000</LineVatlF><LineVatS>1000000.000000</LineVatS><unitMsr>EA</unitMsr><NumPerMsr>1.000000</NumPerMsr><LineTotal>10000000.000000</LineTotal><VatPrcnt>10.000000</VatPrcnt><VatSum>1000000.000000</VatSum><ConsumeFCT>N</ConsumeFCT><ExciseAmt>0.000000</ExciseAmt><TaxPerUnit>0.000000</TaxPerUnit><TotInclTax>0.000000</TotInclTax><CountryOrg></CountryOrg><SWW></SWW><TranType></TranType><DistribExp>Y</DistribExp><ShipToCode></ShipToCode><TotalFrgn>0.000000</TotalFrgn><TotalSumSy>10000000.000000</TotalSumSy><LstBINMPr>0.000000</LstBINMPr><LstByDsFc>0.000000</LstByDsFc><LstByDsSc>0.000000</LstByDsSc><LstByDsSum>0.000000</LstByDsSum><StckDstFc>0.000000</StckDstFc><StckDstSc>0.000000</StckDstSc><StckDstSum>0.000000</StckDstSum><StckINMPr>0.000000</StckINMPr><PickStatus>N</PickStatus><VatWoDpm>0.000000</VatWoDpm><VatWoDpmFc>0.000000</VatWoDpmFc><VatWoDpmSc>0.000000</VatWoDpmSc><CFOPCode></CFOPCode><CSTCode></CSTCode><Usage>0</Usage><TaxOnly>N</TaxOnly><VisOrder>4</VisOrder><BaseOpnQty>0.000000</BaseOpnQty><PriceBefDi>100000.000000</PriceBefDi><LineStatus>C</LineStatus><PackQty>100.000000</PackQty><FreeTxt></FreeTxt><LineType>R</LineType><CogsOcrCod></CogsOcrCod><CogsAcct></CogsAcct><ChgAsmBoMW>N</ChgAsmBoMW><GrossBuyPr>0.000000</GrossBuyPr><GrossBase>0</GrossBase><GPTtlBasPr>0.000000</GPTtlBasPr><OcrCode2></OcrCode2><OcrCode3></OcrCode3><OcrCode4></OcrCode4><OcrCode5></OcrCode5><Text></Text><LocCode>0</LocCode><ActDelDate></ActDelDate><OpenQty>0.000000</OpenQty><OpenSum>10000000.000000</OpenSum><OpenSumFC>0.000000</OpenSumFC><OpenSumSys>10000000.000000</OpenSumSys><ExLineNo></ExLineNo><PQTReqDate></PQTReqDate><PQTReqQty>0.000000</PQTReqQty><CogsOcrCo2></CogsOcrCo2><CogsOcrCo3></CogsOcrCo3><CogsOcrCo4></CogsOcrCo4><CogsOcrCo5></CogsOcrCo5><CSTfIPI></CSTfIPI><CSTfPIS></CSTfPIS><CSTfCOFINS></CSTfCOFINS><CredOrigin></CredOrigin><NoInvtryMv>N</NoInvtryMv><AgrNo>0</AgrNo><AgrLnNum>0</AgrLnNum><ShipToDesc>VIETNAM</ShipToDesc><ActBaseEnt>0</ActBaseEnt><ActBaseLn>0</ActBaseLn><DocEntry>3</DocEntry><U_CXS_MRP_RECMD>N</U_CXS_MRP_RECMD><U_ProID></U_ProID><U_OrgPrice>0.000000</U_OrgPrice><U_OrgQty>0.000000</U_OrgQty><U_ProLine></U_ProLine><U_Pro_Sole>0.000000</U_Pro_Sole><U_Pro_UseSole></U_Pro_UseSole><U_Pro_ByPro>0.000000</U_Pro_ByPro><U_Campaign></U_Campaign></row></POR1><POR12><row><TaxId0></TaxId0><TaxId1></TaxId1><TaxId2></TaxId2><TaxId3></TaxId3><TaxId4></TaxId4><TaxId5></TaxId5><TaxId6></TaxId6><TaxId7></TaxId7><TaxId8></TaxId8><TaxId9></TaxId9><State></State><County></County><Incoterms></Incoterms><Vehicle></Vehicle><VidState></VidState><NfRef></NfRef><Carrier></Carrier><QoP>0</QoP><PackDesc></PackDesc><Brand></Brand><NoSU>0</NoSU><NetWeight>0.000000</NetWeight><GrsWeight>0.000000</GrsWeight><StreetS></StreetS><BlockS></BlockS><BuildingS></BuildingS><CityS></CityS><ZipCodeS></ZipCodeS><CountyS></CountyS><StateS></StateS><CountryS>VN</CountryS><StreetB></StreetB><BlockB></BlockB><BuildingB></BuildingB><CityB></CityB><ZipCodeB></ZipCodeB><CountyB></CountyB><StateB></StateB><CountryB></CountryB><ImpORExp></ImpORExp></row></POR12><POR12><row><StreetS></StreetS><StreetNoS></StreetNoS><BlockS></BlockS><BuildingS></BuildingS><CityS></CityS><ZipCodeS></ZipCodeS><CountyS></CountyS><StateS></StateS><CountryS>VN</CountryS><AddrTypeS></AddrTypeS><StreetB></StreetB><StreetNoB></StreetNoB><BlockB></BlockB><BuildingB></BuildingB><CityB></CityB><ZipCodeB></ZipCodeB><CountyB></CountyB><StateB></StateB><CountryB></CountryB><AddrTypeB></AddrTypeB></row></POR12></BO></BOM>"
        Return str
    End Function
    Public Function Simulate_CreateTransaction() As DataSet
        Dim dtJE = New DataSet
        dtJE.Tables.Add()
        dtJE.Tables(0).Columns.Add("ErrCode", GetType(String))
        dtJE.Tables(0).Columns.Add("ErrMsg", GetType(String))

        Dim dr As DataRow
        dr = dtJE.Tables(0).NewRow
        dr("ErrCode") = "-1000"
        dr("ErrMsg") = "GL Account is missing"
        dtJE.Tables(0).Rows.Add(dr)

        Return dtJE
    End Function
End Class
