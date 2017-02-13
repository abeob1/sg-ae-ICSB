Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class Report
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim connection As String = ConfigurationManager.ConnectionStrings("dbconnection").ToString()
        Dim dbInfo As String() = connection.Split(";"c)
        Dim strServer As String = dbInfo(3).Split("="c)(1)
        'ConfigurationSettings.AppSettings["SQLserver"].ToString();
        Dim strDBName As String = dbInfo(4).Split("="c)(1)
        'ConfigurationSettings.AppSettings["SQLdatabaseName"].ToString();
        Dim strUID As String = dbInfo(1).Split("="c)(1)
        'ConfigurationSettings.AppSettings["SQLUserName"].ToString();
        Dim strPassword As String = dbInfo(2).Split("="c)(1)
        'ConfigurationSettings.AppSettings["SQLPassword"].ToString();
        Dim myReportDocument As ReportDocument
        myReportDocument = New ReportDocument()
        myReportDocument.Load(Server.MapPath("~/Reports/") + "SalesOrder.rpt")
        'myReportDocument.SetDatabaseLogon(strUID, strPassword, strServer, strDBName)
        myReportDocument.SetDatabaseLogon(strUID, strPassword)


        Dim sDocKey As String = Convert.ToString(Request.QueryString("key"))
        'Dim sToDate As String = Convert.ToString(Request.Cookies(Constants.ABRToDate).Value)

        Dim pval1 As CrystalDecisions.Shared.ParameterValues = New ParameterValues()
        'Dim pval2 As CrystalDecisions.Shared.ParameterValues = New ParameterValues()
        Dim pdisval1 As New ParameterDiscreteValue()
        pdisval1.Value = sDocKey
        pval1.Add(pdisval1)
        'Dim pdisval2 As New ParameterDiscreteValue()
        'pdisval2.Value = sToDate
        'pval2.Add(pdisval2)

        myReportDocument.DataDefinition.ParameterFields("DOCKEY").ApplyCurrentValues(pval1)
        'myReportDocument.DataDefinition.ParameterFields("@todate").ApplyCurrentValues(pval2)


        CrystalReportViewer1.ReportSource = myReportDocument
        myReportDocument.Close()
        myReportDocument.Dispose()
        CrystalReportViewer1 = Nothing
    End Sub

End Class