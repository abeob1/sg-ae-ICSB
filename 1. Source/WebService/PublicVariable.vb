Class PublicVariable
    'Connection
    Public Shared oCompany As SAPbobsCOM.Company
    Public Shared Simulate As Boolean = False
    Public Shared sSQLServer As String = ""
    Public Shared sSQLDBName As String = ""
    Public Shared sSQLDBUser As String = ""
    Public Shared sSQLDBPwd As String = ""
    Public Shared sSAPLicenseManager As String = ""
    Public Shared sSQLType As String = ""
    Public Shared sSAPUserID As String = ""
    Public Shared sSAPPwd As String = ""
    Public Shared LogPath As String = "E:\Abeo\ICSB\Gopi"

    Public Shared DEBUG_ON As Int16 = 1
    Public Shared ERROR_ON As Int16 = 1
    Public Shared DEBUG_OFF As Int16 = 0
    Public Shared RTN_SUCCESS As Int16 = 1
    Public Shared RTN_ERROR As Int16 = 0
    Public Shared p_iDebugMode As Int16 = DEBUG_ON
    Public Shared p_iErrDispMethod As Int16
    Public Shared p_iDeleteDebugLog As Int16 = DEBUG_ON
    ' Public Shared oCompanyInfo As SAPbobsCOM.Company = New SAPbobsCOM.Company
End Class

