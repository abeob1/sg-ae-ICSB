Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Net

Public Class OnhireReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim connection As String = ConfigurationManager.ConnectionStrings("dbconnection").ToString()
        Dim TempStorage As String = ConfigurationManager.ConnectionStrings("TempStorage").ToString()
        Dim dbInfo As String() = connection.Split(";"c)
        Dim strServer As String = dbInfo(3).Split("="c).Last()
        'ConfigurationSettings.AppSettings["SQLserver"].ToString();
        Dim strDBName As String = dbInfo(4).Split("="c).Last()
        'ConfigurationSettings.AppSettings["SQLdatabaseName"].ToString();
        Dim strUID As String = dbInfo(1).Split("="c).Last()
        'ConfigurationSettings.AppSettings["SQLUserName"].ToString();
        Dim strPassword As String = dbInfo(2).Split("="c).Last()
        'ConfigurationSettings.AppSettings["SQLPassword"].ToString();
        Dim rpt As ReportDocument = Nothing
        rpt = New ReportDocument()
        rpt.Load(Server.MapPath("~/Reports/") + "Web_OnHire_Report.rpt")
        rpt.SetDatabaseLogon(strUID, strPassword)


        Dim sDocKey As String = Convert.ToString(Request.QueryString("key"))
        'Dim sDocKey As String = "4"

        Dim pval1 As CrystalDecisions.Shared.ParameterValues = New ParameterValues()
        Dim pdisval1 As New ParameterDiscreteValue()
        pdisval1.Value = sDocKey
        pval1.Add(pdisval1)

        rpt.DataDefinition.ParameterFields("DOCKEY").ApplyCurrentValues(pval1)

        'ReportDocument rpt = new ReportDocument();
        'rpt.Load(@"C:\Users\user3\Desktop\Report1.rpt");

        'rpt.SetDataSource(datatablesource);
        ' string sLocation = @"C:\";
        Dim rptExportOption As ExportOptions
        Dim rptFileDestOption As New DiskFileDestinationOptions()
        Dim rptFormatOption As New PdfRtfWordFormatOptions()
        Dim datetimeString As String = String.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now)
        Dim sFileName As String = Convert.ToString("OnHireReport ") & datetimeString
        Dim sLocation As String = Convert.ToString(TempStorage)
        Dim reportFileName As String = (sLocation & sFileName) + ".pdf"
        rptFileDestOption.DiskFileName = reportFileName
        rptExportOption = rpt.ExportOptions
        If True Then
            rptExportOption.ExportDestinationType = ExportDestinationType.DiskFile
            'if we want to generate the report as PDF, change the ExportFormatType as "ExportFormatType.PortableDocFormat"
            'if we want to generate the report as Excel, change the ExportFormatType as "ExportFormatType.Excel"
            rptExportOption.ExportFormatType = ExportFormatType.PortableDocFormat
            rptExportOption.ExportDestinationOptions = rptFileDestOption
            rptExportOption.ExportFormatOptions = rptFormatOption
        End If

        rpt.Export()
        rpt.Dispose()
        rpt.Close()

        Dim path As String = reportFileName
        'Server.MapPath("~\\E:\\karthikeyan\\venky\\pdf\\aaaa.PDF");
        Dim client As New WebClient()
        Dim buffer As [Byte]() = client.DownloadData(path)
        If buffer IsNot Nothing Then
            Response.ContentType = "application/pdf"
            Response.AddHeader("content-length", buffer.Length.ToString())
            Response.BinaryWrite(buffer)
        End If
        If (System.IO.File.Exists(reportFileName)) Then
            System.IO.File.Delete(reportFileName)
        End If

    End Sub

End Class