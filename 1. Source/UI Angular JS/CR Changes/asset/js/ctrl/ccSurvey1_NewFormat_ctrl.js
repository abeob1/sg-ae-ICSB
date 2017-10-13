App.controller('ccSurvey1_NewFormat_ctrl', ['$scope', '$rootScope', '$http', '$window', '$cookies','util_SERVICE','css1_NewFormat_service','hotkeys','$location',

function ($scope, $rootScope, $http, $window, $cookies,US,CSS,hotkeys,$location) {
    $scope.savelable = "Save";
    $scope.savebtn = true;
    $scope.findbtn= false;
    $scope.updatebtn= false;
    $scope.loading= false;   
    $scope.whilefind = false;
    $scope.btndisable = true;
    $scope.ctype=false;
    $scope.CCselected = false;
    $scope.SIselected = false;
    $scope.SAselected = false;
    $rootScope.Rremark = "";
    $scope.updatedisabled = false;



    //today date 
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth()+1; //January is 0!

    var yyyy = today.getFullYear();
    if(dd<10){
        dd='0'+dd
    } 
    if(mm<10){
        mm='0'+mm
    } 
    $scope.today =dd+'/'+mm+'/'+yyyy;
    //today date 



    
    $scope.checkEQNO= function(e)
    {
    
        if(e==8)
        {
        }
        else
        {
            if($scope.data.ODLN[0].U_EqNo.length==4)
            {
               $scope.data.ODLN[0].U_EqNo=$scope.data.ODLN[0].U_EqNo+" ";
            }
            if($scope.data.ODLN[0].U_EqNo.length==11)
            {
               $scope.data.ODLN[0].U_EqNo=$scope.data.ODLN[0].U_EqNo+" ";
            }
        }
    }


     

    $scope.user = angular.fromJson($cookies.get('UserData'));

    //SalesQuotation_Select Fined record

   $rootScope.FindoneRecord = function(code)
    {   
    
    
         CSS.FindRecord(code).then(function (response) {console.log(response);$scope.data=response.data;
         $scope.updateclick();
          if (response.data.VALIDATE[0].Status == "False") {          
          
            alert(response.data.VALIDATE[0].Msg);   
            $scope.resetall();
            $scope.findclick();
       }
       });
    }


    //today date 
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth()+1; //January is 0!

    var yyyy = today.getFullYear();
    if(dd<10){
        dd='0'+dd
    } 
    if(mm<10){
        mm='0'+mm
    } 
    $scope.today =dd+'/'+mm+'/'+yyyy;
    //today date 
    
    $scope.g_remark = "";  
    $scope.a_remark = "";  

    $scope.addressarray = [];
    $scope.SurveyorIDMaster = [];
    CSS.getetype().then(function (response) { $scope.etype = response.data.SURCRTA; });
    CSS.getSurveyCriteria().then(function (response) { $scope.SurveyCriteria = response.data.SURCRTA;  });
    CSS.getSurveyorIDMaster().then(function (response) { $scope.SurveyorIDMaster = response.data.SUID; });//get full surveyer
    
    
     $scope.geteSCname = function(code)
    {
         var ret = ""
         for (var i=0; i<$scope.SurveyCriteria.length; i++) {
          if($scope.SurveyCriteria[i].U_SURCRTACODE == code)
            ret = $scope.SurveyCriteria[i].U_SURCRTANAME;
        }
        return ret;
    }

     $scope.getegroupName = function(code)
    {
         var ret = ""
         for (var i=0; i<$scope.etype.length; i++) {
          if($scope.etype[i].U_EQTYPECODE == code)
            ret = $scope.etype[i].U_EQTYPENAME;
        }
        return ret;
    }
    
    $scope.getCtypeName = function(code)
    {
         var ret = ""
         for (var i=0; i<$scope.ChargeType.length; i++) {
          if($scope.ChargeType[i].U_Stype == code)
            ret = $scope.ChargeType[i].U_StypeName;
        }
        return ret;
    }
    


    $scope.addclick = function()
    {
        
         $scope.savebtn = true;
         $scope.findbtn= false;
         $scope.updatebtn= false;
         $scope.whilefind = false;
         $scope.ctype=false;
         $scope.updatedisabled = false;

         $scope.resetall();
    }

     $scope.findclick = function()
    {
        
         $scope.savebtn = false;
         $scope.findbtn= true;
         $scope.updatebtn= false;
         $scope.resetall();
         $scope.whilefind = true;
         $scope.ctype=true;


    }
    

     US.getstype().then(function (response) { $scope.stype = response.data.SURTYPE; console.log($scope.stype); });
     $scope.getestypeName = function(code)
    {
         var ret = ""
         for (var i=0; i<$scope.stype.length; i++) {
          if($scope.stype[i].U_Stype == code)
            ret = $scope.stype[i].U_StypeName;
        }
        return ret;
    }
    
   


    //geting full country city values to show country & city name in table    
    US.getContinent().then(function (response) { $scope.Continent = response.data.CONT; console.log($scope.Continent); });
    US.getCityMaster().then(function (response) { $scope.CityMaster = response.data.CITY; console.log($scope.CityMaster); });
    US.getCountryMaster().then(function (response) { $scope.CountryMaster = response.data.COUNTRY; console.log($scope.CountryMaster); });
    US.getLocationMaster().then(function (response) { $scope.LocationMaster = response.data.LOC; console.log($scope.LocationMaster); });
    US.getIMGMaster().then(function (response) { $scope.IMO = response.data.IOM; console.log($scope.IMO); });
    

    //get conti full json to select dropdown while edite 
    $scope.getcontijson = function(code)
    {
    
   
        var ret = ""
         for (var i=0; i<$scope.Continent.length; i++) {
          if($scope.Continent[i].U_Conti == code)
            ret = $scope.Continent[i];
        }
        return ret;
    }

    //get country full json to select dropdown while edite 
    $scope.getcountryjson = function(code)
    {
    
   
        var ret = ""
         for (var i=0; i<$scope.CountryMaster.length; i++) {
          if($scope.CountryMaster[i].U_Country == code)
            ret = $scope.CountryMaster[i];
        }
        return ret;
    }
    //get city full json to select dropdown while edite 
    $scope.getcityjson = function(code)
    {
    
   
        var ret = ""
         for (var i=0; i<$scope.CityMaster.length; i++) {
          if($scope.CityMaster[i].U_City == code)
            ret = $scope.CityMaster[i];
        }
        console.log("City");
         console.log(ret);
        return ret;
    }

    //get Location full json to select dropdown while edite 
    $scope.getlocationjson = function(code)
    {
    
   
        var ret = ""
         for (var i=0; i<$scope.LocationMaster.length; i++) {
          if($scope.LocationMaster[i].U_Loc == code)
            ret = $scope.LocationMaster[i];
        }
        console.log(ret);
        return ret;
    }
    

    
        
    $scope.getContinentName = function(code)
    {
         var ret = ""
         for (var i=0; i<$scope.Continent.length; i++) {
          if($scope.Continent[i].U_Conti == code)
            ret = $scope.Continent[i].U_ContiName;
        }
        return ret;
    }

    $scope.getcountry = function(conti)
    {
        US.getcountry(conti).then(function (response) { $scope.country = response.data.COUNTRY; console.log($scope.country); });
    }
    $scope.getcountryName = function(code)
    {
         var ret = ""
         for (var i=0; i<$scope.CountryMaster.length; i++) {
          if($scope.CountryMaster[i].U_Country == code)
            ret = $scope.CountryMaster[i].U_CountryName;
        }
        return ret;
    }
    $scope.getlocation = function(con)
    {
        US.getlocation(con).then(function (response) { $scope.location = response.data.LOC; console.log($scope.location); });
    }
    $scope.getlocationName = function(code)
    {
         var ret = ""
         for (var i=0; i<$scope.LocationMaster.length; i++) {
          if($scope.LocationMaster[i].U_Loc == code)
            ret = $scope.LocationMaster[i].U_LocName;
        }
        return ret;
    }


     $scope.getcity = function(con)
    {
        US.getcity(con).then(function (response) { $scope.city = response.data.CITY; console.log($scope.city); });
    }
    $scope.getcityName = function(code)
    {
         var ret = ""
         for (var i=0; i<$scope.CityMaster.length; i++) {
          if($scope.CityMaster[i].U_City == code)
            ret = $scope.CityMaster[i].U_CityName;
        }
        return ret;
    }
    
    $scope.updateclick = function()
    {
         $scope.savebtn = false;
         $scope.findbtn= false;
         $scope.updatebtn= true;
         $scope.whilefind = false;
         $scope.btndisable = false;
         $scope.ctype=false;
         $scope.CCselected = true;
         $scope.SIselected= true;
         $scope.getcity($scope.data.ODLN[0].U_Country);
         $scope.getlocation($scope.data.ODLN[0].U_City);
         $scope.updatedisabled = true;
    }

    $scope.getcustomer = function(type)
    {
    var c,n;
    if(type=='code')
    {
        c = $scope.data.ODLN[0].CardCode;
        n="";
    }
    else{
        c =""; 
        n=$scope.data.ODLN[0].CardName;
    }
         CSS.getcustomer(c,n).then(function (response) { $rootScope.customer = response.data; $("#myModal").modal(); 
                });
    }

    //fill the cname and code 
    $rootScope.Fillcustomer = function(c,n)
    {
        $scope.data.ODLN[0].CardCode = c;
        $scope.data.ODLN[0].CardName = n;
        $scope.CCselected = true;

    }




     $scope.getsurveyorID = function()
    {
 
         CSS.getsurveyorID($scope.UserData[0].Code).then(function (response) { $rootScope.surveyor = response.data; $("#myModal2").modal(); 
                });
    }


    
    //getsurvey Agent
    $scope.getsurveyAgent = function()
    {
 
         CSS.getsurveyAgent($scope.UserData[0].Code).then(function (response) { $rootScope.surveyorAgent = response.data; $("#Modal3").modal(); 
                });
    }


    //get SurveyorName by ID
    $scope.getSurveyorName = function(id)
    {
   
        var ret = ""
         for (var i=0; i<$scope.SurveyorIDMaster.length; i++) {
          if($scope.SurveyorIDMaster[i].U_SCode == id)
            ret = $scope.SurveyorIDMaster[i].U_SName;
        }
        return ret;
    }

    //fill the cname and code 
    $rootScope.Fillsurveyor = function(c,n)
    {
        $scope.data.ODLN[0].U_SurveyorID = c;
       // $scope.data.ODLN[0].U_SurveyorName = n;
        
        $scope.SIselected = true;

    }
     //fill the Agent name 
    $rootScope.FillsurveyorAgent = function(n)
    {
        $scope.data.ODLN[0].U_SuvExAgent = n;
        
        $scope.SAselected = true;

    }
   



    
   
   $scope.findAddress = function(code)
   {
         $scope.addressarray = [];
        for (var i=0; i<$rootScope.customer.ADDR.length; i++) {
          if($rootScope.customer.ADDR[i].U_Ccode == code)
            $scope.addressarray.push($rootScope.customer.ADDR[i]);
        }
        console.log($scope.addressarray);
        if($scope.addressarray.length > 0)
        {
            $scope.setaddress($scope.addressarray[0].U_AddrN);
            $scope.U_AddrN = $scope.addressarray[0].U_AddrN;
            //alert($scope.addressarray[0].U_AddrN);
            }
   }

   $scope.setaddress = function(name)
   {
         for (var i=0; i<$scope.addressarray.length; i++) {
          if($scope.addressarray[i].U_AddrN == name)
          {
                $scope.data.SQTO[0].U_Addr1 =$scope.addressarray[i].U_Addr1;
                $scope.data.SQTO[0].U_Addr2 =$scope.addressarray[i].U_Addr2;
                $scope.data.SQTO[0].U_Addr3 =$scope.addressarray[i].U_Addr3;
                $scope.data.SQTO[0].U_Addr4 =$scope.addressarray[i].U_Addr4;
                $scope.data.SQTO[0].U_Addr5 =$scope.addressarray[i].U_Addr5;
                $scope.data.SQTO[0].U_Addr6 =$scope.addressarray[i].U_Addr6;
            }
        }
   }

    $scope.data = {
	"ODLN": [{
		"U_UCode": "",
        "U_OrderNo":37,
		"U_UName": $scope.UserData[0].Name,
		"U_Cdate": "2016-06-29",
		"DocDate": "2016-06-29",
		"CardCode": "ACSLSGS",
		"CardName": "Allport Cargo Services Logistics Pt",
		"U_SurveyorID": "ACSLSGS",
        "U_SurveyorName": "",
		"NumAtCard": "ACSLSGS",
		"U_STypeCode": "CDC",
		"U_STypeName": "Condition Check Survey",
		"U_Country": "IN",
		"U_City": "INMAA",
		"U_Loc": "MAA001",
        "U_SuvExAgent":"",
        "U_Eqtype":"",
        "U_EqNo":"",
        "U_NoPh":" ",
        "U_SCriteria":"",
        "U_SResult":"",
		"Comments": "Remarks 123",
		"U_FormName": "OFFHS",
		"U_EX_Fram": "NO",
		"U_EX_Man": "NO",
		"U_EX_Ser": "NO",
		"U_EX_Car": "NO",
		"U_INT_Free": "FIT",
		"U_INT_Cleanliness": "FIT",
        "U_INT_Rust": "FIT",
        "U_INT_Deposits": "FIT",
		"U_INT_Clean": "FIT",
		"U_INT_Dry": "FIT",
		"U_INT_Pitt": "FIT",
        "U_INT_DamagedCoating": "FIT",
        "U_INT_GasketMainHole": "FIT",
		"U_INT_Disc": "FIT",
        "U_INT_OilDirtFree": "FIT",
        "U_INT_SteamCoil": "FIT",
        "U_INT_Steam_Cleaned": "FIT",
        "U_INT_GasktDschrgval": "FIT",
		"U_VAL_Val": "NO",
		"U_VAL_Bott": "YES",
		"U_VAL_Man": "NO",
		"U_VAL_Syp": "NO",
		"U_VAL_Tank": "NO",
		"U_VAL_Avail": "NO",
		"U_VAL_Steam": "NO",
		"U_VAL_Gas": "NO",
		"U_SEAL_MAN": "NO",
		"U_SEAL_AIR": "NO",
		"U_SEAL_BOTT": "NO",
		"U_SEAL_LAST": "NO",
		"U_SEAL_NEXT": "NO",
        "U_VAL_SPIL" : "",
        "U_DOM": "",
		"U_MGW": "",
		"U_Tare": "",
		"U_ACEP": "",
		"U_CSC": ""
	}]
}

$scope.afterSave = function()
{
    //$scope.data.ODLN[0].U_Cdate ="";
    //$scope.data.ODLN[0].DocDate="";
    //$scope.data.ODLN[0].NumAtCard="";
    //$scope.data.ODLN[0].U_SuvExAgent="";
    $scope.data.ODLN[0].U_EqNo="";
    $scope.data.ODLN[0].U_NoPh=" ";
    $scope.data.ODLN[0].U_CSCNum = "";
    $scope.data.ODLN[0].U_CSC = "";
    $scope.data.ODLN[0].U_DOM = "";
    $scope.data.ODLN[0].U_MGW = "";
    $scope.data.ODLN[0].U_PayLoad = "";
    $scope.data.ODLN[0].U_Tare = "";
    $scope.data.ODLN[0].U_Capacity = "";
        //$scope.data.ODLN[0].U_SCriteria="",
        $scope.data.ODLN[0].U_SResult="",
		$scope.data.ODLN[0].Comments="",
		$scope.data.ODLN[0].U_EX_Fram="NO";
		$scope.data.ODLN[0].U_EX_Man="NO";
		$scope.data.ODLN[0].U_EX_Ser="NO";
		$scope.data.ODLN[0].U_EX_Car="NO";
		$scope.data.ODLN[0].U_INT_Free="FIT";
		$scope.data.ODLN[0].U_INT_Clean="FIT";
        $scope.data.ODLN[0].U_INT_Cleanliness = "FIT";
        $scope.data.ODLN[0].U_INT_Rust = "FIT";
        $scope.data.ODLN[0].U_INT_Deposits = "FIT";
		$scope.data.ODLN[0].U_INT_Dry="FIT";
		$scope.data.ODLN[0].U_INT_Pitt="FIT"; 
        $scope.data.ODLN[0].U_INT_DamagedCoating = "FIT";
        $scope.data.ODLN[0].U_INT_GasketMainHole = "FIT";
		$scope.data.ODLN[0].U_INT_Disc="FIT";
        $scope.data.ODLN[0].U_INT_OilDirtFree="FIT";
        $scope.data.ODLN[0].U_INT_SteamCoil="FIT";
        $scope.data.ODLN[0].U_INT_Steam_Cleaned = "FIT";
        $scope.data.ODLN[0].U_INT_GasktDschrgval = "FIT";
		$scope.data.ODLN[0].U_VAL_Val="YES";
		$scope.data.ODLN[0].U_VAL_Bott="NO";
		$scope.data.ODLN[0].U_VAL_Man="NO";
		$scope.data.ODLN[0].U_VAL_Syp="NO";
		$scope.data.ODLN[0].U_VAL_Tank="NO";
		$scope.data.ODLN[0].U_VAL_Avail="YES";
		$scope.data.ODLN[0].U_VAL_Steam="NO";
		$scope.data.ODLN[0].U_VAL_Gas="NO";
		$scope.data.ODLN[0].U_SEAL_MAN="";
		$scope.data.ODLN[0].U_SEAL_AIR="";
		$scope.data.ODLN[0].U_SEAL_BOTT="";
		$scope.data.ODLN[0].U_SEAL_LAST="";
		$scope.data.ODLN[0].U_SEAL_NEXT="UNKNOWN";
        $scope.data.ODLN[0].U_VAL_SPIL="";
        $scope.data.ODLN[0].U_DOM="";
		$scope.data.ODLN[0].U_MGW="";
		$scope.data.ODLN[0].U_Tare="";
		$scope.data.ODLN[0].U_ACEP="";
		$scope.data.ODLN[0].U_CSC="";
       
        
$scope.SAselected = true;
}
    //validate genarel 
    $scope.genval = function()
    {
     var retval = true;
     var errormsg = "";

     
        if( $scope.data.SQTOGEN.length>=1)
        {
            retval = false;
            errormsg = errormsg+". Can't Add More than One Record<br/>";
        }
        
         if($scope.Quantity=="" || typeof $scope.Quantity==="undefined")
        {
            retval = false;
            errormsg = errormsg+". Kindly Enter Quantity<br/>";
        }
        if($scope.U_PDate=="" || typeof $scope.U_PDate==="undefined")
        {
            retval = false;
            errormsg = errormsg+". Kindly Select Date <br/>";
        }        
         if($scope.U_EQType=="" || typeof $scope.U_EQType==="undefined")
        {
            retval = false;
            errormsg = errormsg+". Kindly Select Equipment Type   <br/>";
        }
        if($scope.U_SCriteria=="" || typeof $scope.U_SCriteria==="undefined")
        {
            retval = false;
            errormsg = errormsg+". Kindly Select Survey Criteria   <br/>";
        }
        
        if(errormsg!="")
            bootbox.alert("<div style='color:red'>"+errormsg+"</div>", null);
        return retval;
    }
    //add record to gen
    $scope.g_add = function () {
    if($scope.genval())
        {
        $scope.gen = {
            "Quantity": angular.copy($scope.Quantity),
            "U_PDate": angular.copy($scope.U_PDate),
            "U_EQType": angular.copy($scope.U_EQType),
            "U_SCriteria": angular.copy($scope.U_SCriteria)
           
        };
        console.log($scope.gen);
        
        $scope.data.SQTOGEN.push(angular.copy($scope.gen));
        console.log($scope.data.SQTOGEN);
        $scope.g_reset();
        }
    }

    $scope.g_reset = function () {

            $scope.Quantity = "";
            $scope.U_PDate= "";
            $scope.U_EQType= "";
            $scope.U_SCriteria= "";           
        
        
    }


    $scope.g_edit = function(i)
    {
       // alert(i);

        $scope.data.SQTOGEN[i].edit = true;

        
    }
    $scope.g_ok = function(i)
    {
      
        $scope.data.SQTOGEN[i].edit = false;
       

       
    }

    $scope.g_del = function(i)
    {
        //alert(i);
        $scope.data.SQTOGEN.splice(i, 1); 
    }
     $scope.g_copy = function(i)
    {  

            $scope.Quantity = $scope.data.SQTOGEN[i].Quantity;
            $scope.U_PDate= $scope.data.SQTOGEN[i].U_PDate;            
            $scope.U_EQType=  $scope.data.SQTOGEN[i].U_EQType;
            $scope.U_SCriteria= $scope.data.SQTOGEN[i].U_SCriteria;
    }



     //validate addon 
    $scope.addval = function()
    {
     var retval = true;
     var errormsg = "";

     
     
        
         if($scope.a_Ctype=="" || typeof $scope.a_Ctype==="undefined")
        {
            retval = false;
            errormsg = errormsg+". Kindly Select Charge Type<br/>";
        }
        if($scope.a_Continent=="" || typeof $scope.a_Continent==="undefined")
        {
            retval = false;
            errormsg = errormsg+". Kindly Select Continent <br/>";
        }
        if($scope.a_Country=="" || typeof $scope.a_Country==="undefined")
        {
            retval = false;
            errormsg = errormsg+". Kindly Select Country  <br/>";
        }
        if($scope.a_city=="" || typeof $scope.a_city==="undefined")
        {
            retval = false;
            errormsg = errormsg+". Kindly Select City  <br/>";
        }
        if($scope.a_curency=="" || typeof $scope.a_curency==="undefined")
        {
            retval = false;
            errormsg = errormsg+". Kindly Select Currency  <br/>";
        }
         if($scope.a_egroup=="" || typeof $scope.a_egroup==="undefined")
        {
            retval = false;
            errormsg = errormsg+". Kindly Select Location  <br/>";
        }
        if($scope.a_uom=="" || typeof $scope.a_uom==="undefined")
        {
            retval = false;
            errormsg = errormsg+". Kindly Select UOM   <br/>";
        }
        if($scope.a_gst=="" || typeof $scope.a_gst==="undefined")
        {
            retval = false;
            errormsg = errormsg+". Kindly Select GST <br/>";
        }
        if(errormsg!="")
            bootbox.alert("<div style='color:red'>"+errormsg+"</div>", null);
        return retval;
    }


    //add-on functions
    
    $scope.a_add = function () {

    if($scope.addval())
    {
        $scope.add = {
            "U_Ctype": $scope.a_Ctype,
            "U_Continent": $scope.a_Continent.U_Conti,
            "U_Country": $scope.a_Country.U_Country,
            "U_City": $scope.a_city.U_City,
            "C_Conti": $scope.a_Continent,
            "C_Country": $scope.a_Country,
            "C_City": $scope.a_city,
            "U_Currency": $scope.a_curency,
            "U_EQGroup": $scope.a_egroup.U_Loc,
            "U_Rate": $scope.a_rate,
            "U_UOM": $scope.a_uom,
            "U_GST": $scope.a_gst,
            "U_Remarks": $scope.a_remark,
        };
        $scope.data.SQTOADD.push(angular.copy($scope.add));
        $scope.a_reset();
     }
    }

    $scope.a_reset = function () {

            $scope.a_Ctype = "";
            $scope.a_Continent= "";
            $scope.a_Country= "";
            $scope.a_city= "";
            $scope.a_curency= "";
            $scope.a_egroup= "";
            $scope.a_rate= "";
            $scope.a_uom= "";
            $scope.a_gst= "";
            $scope.a_remark= "";
        
        
    }


    $scope.a_edit = function(i)
    {
       // alert(i); 

        $scope.data.SQTOADD[i].edit = true;
        $scope.data.SQTOADD[i].Ea_Conti = $scope.getcontijson($scope.data.SQTOADD[i].U_Continent);
        $scope.getcountry($scope.data.SQTOADD[i].Ea_Conti.U_Conti);

        $scope.data.SQTOADD[i].Ea_Country = $scope.getcountryjson($scope.data.SQTOADD[i].U_Country);
        $scope.getcity($scope.data.SQTOADD[i].Ea_Country.U_Country);

        $scope.data.SQTOADD[i].Ea_City = $scope.getcityjson($scope.data.SQTOADD[i].U_City);
        $scope.getlocation($scope.data.SQTOADD[i].Ea_City.U_City);

        $scope.data.SQTOADD[i].Ea_EQGroup = $scope.getlocationjson($scope.data.SQTOADD[i].U_EQGroup);

    }
    $scope.a_ok = function(i)
    {
        //alert(i);
        $scope.data.SQTOADD[i].edit = false;
        
        $scope.data.SQTOADD[i].U_Continent= angular.copy( $scope.data.SQTOADD[i].Ea_Conti.U_Conti);
        $scope.data.SQTOADD[i].U_Country= angular.copy( $scope.data.SQTOADD[i].Ea_Country.U_Country);
        $scope.data.SQTOADD[i].U_City= angular.copy( $scope.data.SQTOADD[i].Ea_City.U_City);
        $scope.data.SQTOADD[i].U_EQGroup= angular.copy( $scope.data.SQTOADD[i].Ea_EQGroup.U_Loc);

        $scope.data.SQTOADD[i].C_Conti= angular.copy( $scope.data.SQTOADD[i].Ea_Conti);
        $scope.data.SQTOADD[i].C_Country= angular.copy( $scope.data.SQTOADD[i].Ea_Country);
        $scope.data.SQTOADD[i].C_City= angular.copy( $scope.data.SQTOADD[i].Ea_City);

    }

    $scope.a_del = function(i)
    {
        //alert(i);
        $scope.data.SQTOADD.splice(i, 1); 
    }
     $scope.a_copy = function(i)
    { 
            $scope.a_Ctype = $scope.data.SQTOADD[i].U_Ctype;
            $scope.a_Continent= $scope.getcontijson($scope.data.SQTOADD[i].U_Continent);
            $scope.getcountry($scope.data.SQTOADD[i].U_Continent);

            $scope.a_Country=  $scope.getcountryjson($scope.data.SQTOADD[i].U_Country);
            $scope.getcity($scope.data.SQTOADD[i].U_Country);

            $scope.a_city= $scope.getcityjson($scope.data.SQTOADD[i].U_City);
            $scope.getlocation($scope.data.SQTOADD[i].U_City);

            $scope.a_curency= $scope.data.SQTOADD[i].U_Currency;
            $scope.a_egroup= $scope.getlocationjson($scope.data.SQTOADD[i].U_EQGroup);
            $scope.a_rate= $scope.data.SQTOADD[i].U_Rate;
            $scope.a_uom= $scope.data.SQTOADD[i].U_UOM;
            $scope.a_gst= $scope.data.SQTOADD[i].U_GST;
            $scope.a_remark= $scope.data.SQTOADD[i].U_Remarks;
    }
    

    //resetall field
    $scope.resetall = function()
    {
           
        $scope.CCselected=false;
        $scope.SIselected=false;

       
    }

    $scope.formsavedata = function(data)
    {
        

        

        return data;

        
    }


   //validation function
    $scope.val  = function()
    {
     var retval = true;
     var errormsg = "";

     
     if($scope.SIselected==false)
        {
            retval = false;
            errormsg = errormsg+". Surveyor Id Invalid <br/>";
        }
        if($scope.SAselected==false)
        {
            retval = false;
            errormsg = errormsg+". Surveyor Agent Invalid <br/>";
        }
       


         if($scope.data.ODLN[0].U_EqNo=="" || $scope.data.ODLN[0].U_EqNo===undefined)
        {
            retval = false;
            errormsg = errormsg+". Kindly Enter Equipment Number <br/>";
        }
        else if($scope.data.ODLN[0].U_EqNo.length!=13)
        {
            retval = false;
            errormsg = errormsg+". Kindly Enter Valide 13 digit Equipment Number <br/>";
        }

        if($scope.data.ODLN[0].U_SResult=="" || $scope.data.ODLN[0].U_SResult===undefined)
        {
            retval = false;
            errormsg = errormsg+". Kindly  Select Survey Result<br/>";
        }
         if($scope.data.ODLN[0].U_Cdate=="" || typeof $scope.data.ODLN[0].U_Cdate==="undefined")
        {
            retval = false;
            errormsg = errormsg+". Kindly Select Create Date<br/>";
        }
        if($scope.data.ODLN[0].DocDate=="" || typeof $scope.data.ODLN[0].DocDate==="undefined")
        {
            retval = false;
            errormsg = errormsg+". Kindly Select Survey Date<br/>";
        }
       

        if(errormsg!="")
            bootbox.alert("<div style='color:red'>"+errormsg+"</div>", null);
        return retval;
    };



    //$scope.resetall();

    //save service call
    $scope.save = function()
    {
    
    if($scope.val())
    {
        $scope.data.ODLN[0].U_FormName = "CS1";
        $scope.savelable = "Loading..";
        $scope.savebtn = true;
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var parms = encodeURIComponent(JSON.stringify($scope.data));
        $http.post(US.url+"Cleaning1_NewFormat_SurveyTyepAdd", "value=" + parms, config)
   .then(
       function (response) {
           // success callback
           console.log(response.data);
           if (response.data.VALIDATE[0].Status == "True") {
              $scope.savelable = "Save";
              $scope.savebtn = true;
              $scope.resetall();
              alert(response.data.VALIDATE[0].Msg);
              $scope.afterSave();
           }
            else
            {
               alert(response.data.VALIDATE[0].Msg);
                $scope.savelable = "Save";
                $scope.savebtn = true;
               }
       },
       function (response) {
           // failure callback

      
       }
    );
    
     }
    }

    $scope.Survey_Result_changed = function()
    {
    
        if($scope.data.ODLN[0].U_SResult=="Accepted")
        {
            $scope.data.ODLN[0].U_EX_Fram="YES";
		    $scope.data.ODLN[0].U_EX_Man="YES";
		    $scope.data.ODLN[0].U_EX_Ser="YES";
		    $scope.data.ODLN[0].U_EX_Car="YES";
		    $scope.data.ODLN[0].U_INT_Free="FIT";
		    $scope.data.ODLN[0].U_INT_Clean="FITTED";
		    $scope.data.ODLN[0].U_INT_Dry="FITTED";
		    $scope.data.ODLN[0].U_INT_Pitt="FITTED";
		    $scope.data.ODLN[0].U_INT_Disc="FITTED";
		   
        }
        else
        {
            $scope.data.ODLN[0].U_EX_Fram="NO";
		    $scope.data.ODLN[0].U_EX_Man="NO";
		    $scope.data.ODLN[0].U_EX_Ser="NO";
		    $scope.data.ODLN[0].U_EX_Car="NO";
		    $scope.data.ODLN[0].U_INT_Free="NOT FITTED";
		    $scope.data.ODLN[0].U_INT_Clean="NOT FITTED";
		    $scope.data.ODLN[0].U_INT_Dry="NOT FITTED";
		    $scope.data.ODLN[0].U_INT_Pitt="NOT FITTED";
		    $scope.data.ODLN[0].U_INT_Disc="NOT FITTED";

        }
    }

    //set_Quote Validity to date
    $scope.set_QTO_date = function()
    {
        var d = $scope.data.SQTO[0].U_Qdate1.split('/');
        //alert(d[1]);
        var CurrentDate = new Date(d[2],d[1],d[0]);
        CurrentDate.setMonth(CurrentDate.getMonth() + 3);
        if(CurrentDate.getMonth()==0)
             month = '' + (12);
        else
             month = '' + (CurrentDate.getMonth());
        day = '' + CurrentDate.getDate();
        year = CurrentDate.getFullYear();

        
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

        $scope.data.SQTO[0].U_Qdate2 =day+"/"+month+"/"+year;
    }


    $scope.SQM_Approval = function()
    {
    
        if($scope.val())
        {

        //CSS.SQMApproval($scope.data.SQTO.U_Qno).then(function (response) { console.log(response); });
        CSS.aproval($scope.data.SQTO[0].U_Qno).then(function (response) {
        if (response.data.APPROVAL[0].Status == "True") {             
               $scope.addclick();
              alert(response.data.APPROVAL[0].Msg);
           }
            else
            {
               alert(response.data.APPROVAL[0].Msg);               
               }});


        }
    }



    //approvel Reject by approver
     $rootScope.Reject = function()
    {
    
        var remarks = $rootScope.Rremark;
        var uname = $scope.UserData[0].Name;

        CSS.approver($scope.data.SQTO[0].U_Qno,'Rejected',remarks,uname).then(function (response) {
        if (response.data.APPROVAL[0].Status == "True") {             
              // $scope.addclick();
              alert(response.data.APPROVAL[0].Msg);
               window.location = "SqApproval.aspx";
           }
            else
            {
               alert(response.data.APPROVAL[0].Msg);               
               }});


        
    }


    //approvel approved by approver
     $rootScope.Approved = function()
    {
    
        var remarks = "";
        var uname = $scope.UserData[0].Name;

        CSS.approver($scope.data.SQTO[0].U_Qno,'Approved',remarks,uname).then(function (response) {
        if (response.data.APPROVAL[0].Status == "True") {             
               //$scope.addclick();
              alert(response.data.APPROVAL[0].Msg);
               window.location = "SqApproval.aspx";
              
           }
            else
            {
               alert(response.data.APPROVAL[0].Msg);               
               }});


        
    }


    //update record 
     $scope.UpdateRecord = function()
    {
    
    if($scope.val())
    {
        
        
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var parms = encodeURIComponent(JSON.stringify($scope.formsavedata($scope.data)));
        $http.post(US.url+"ICSB.asmx/SalesOrderUpdate", "value=" + parms, config)
   .then(
       function (response) {
           // success callback
           console.log(response.data);
           if (response.data.VALIDATE[0].Status == "Ture") {             
               $scope.addclick();
              alert(response.data.VALIDATE[0].Msg);
             
           }
            else
            {
               alert(response.data.VALIDATE[0].Msg);               
               }
       },
       function (response) {
           // failure callback

      
       }
    );
    
     }
    }




    //Copy data To Contract page 
   $scope.CopyToContract = function()
   {
  
        $cookies.put('CTC_data', JSON.stringify($scope.data));
        window.location = "Customer_Contract.aspx?CTC=true";
   }

   //disable approved btn while status Approved 
   $scope.checkSQAstatus = function(val)
   {
       var rval = false;       
       if(val=='Approved')
       {
            rval = true;
       }

       return rval;
            
   }
   
   
   
     //getDamageHistory
    $scope.getDamageHistory = function(eno)
    {
    
    if(true)
    {
       
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }
        $scope.input = {  "ODLN": [{  "U_EqNo": eno }] };
        var parms = encodeURIComponent(JSON.stringify($scope.input));
        $http.post(US.url+"DamageHistory", "value=" + parms, config)
   .then(
       function (response) {
           // success callback
           console.log(response.data);
           $scope.DamageHistory = response.data;
           
       },
       function (response) {
           // failure callback

      
       }
    );
    
     }
    }

    //Back to salesorder and popup survey list 
    $scope.SurveyList =function()
   {
       var BackURL = "Sales_Order.aspx?BTSL=true&sn="+$scope.data.ODLN[0].U_OrderNo;
        window.location = BackURL;
   }

   //Back to salesorder 
    $scope.backToSO =function()
   {
       var BackURL = "Sales_Order.aspx?BTSO=true&sn="+$scope.data.ODLN[0].U_OrderNo;
        window.location = BackURL;
   }


   
     //Update survey call
    $scope.update = function()
    {
    $scope.data.ODLN[0].U_FormName = "OFFHS";
    if($scope.val())
    {
        
        
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var parms = encodeURIComponent(JSON.stringify($scope.data));
        $http.post(US.url+"Cleaning1_NewFormat_SurveyTyepUpdate", "value=" + parms, config)
   .then(
       function (response) {
           // success callback
           console.log(response.data);
           if (response.data.VALIDATE[0].Status == "True") {
              
              //$scope.resetall();
              alert(response.data.VALIDATE[0].Msg);
              //$scope.afterSave();
           }
            else
            {
               alert(response.data.VALIDATE[0].Msg);
               
                
               }
       },
       function (response) {
           // failure callback

      
       }
    );
    
     }
    }
   

   //check the page loading from copy to contract 
    //CTC = Copy to Contract
    //Keep It Here
    if(getParameterByName('AOS')=='true')
    {
        $scope.CTCdata = angular.fromJson($cookies.get('AOS_data'));
       console.log($scope.CTCdata);
       $scope.data.ODLN[0] = angular.copy($scope.CTCdata.ORDR[0]);
       $scope.data.ODLN[0].U_Eqtype = angular.copy($scope.CTCdata.SQTOGEN[0].U_EQType);
       $scope.data.ODLN[0].U_SCriteria = angular.copy($scope.CTCdata.SQTOGEN[0].U_SCriteria);
      // $scope.data.CCON[0].U_QutoNo = angular.copy($scope.CTCdata.SQTO[0].U_Qno);
      // $scope.data.CCONGEN = angular.copy($scope.CTCdata.SQTOGEN);
       //$scope.data.CCONADD = angular.copy($scope.CTCdata.SQTOADD);
       $scope.data.ODLN[0].DocDate = angular.copy($scope.CTCdata.SQTOGEN[0].U_PDate);
       $scope.data.ODLN[0].U_NoPh = "";
       $scope.data.ODLN[0].U_Status = "";
       $scope.data.ODLN[0].U_SEAL_NEXT = "UNKNOWN";
       $scope.data.ODLN[0].U_SuvExAgent=angular.copy($scope.user[0].AgentCompany);
       $scope.SIselected = true;
	    $scope.SAselected = true;
       //$scope.SAselected = true;
       //$scope.SalseQuoteNo = $scope.CTCdata.SQTO[0].U_Qno;

        $scope.getcity($scope.data.ODLN[0].U_Country);
         $scope.getlocation($scope.data.ODLN[0].U_City);

         $scope.afterSave();

         $scope.Showsavebtn = true;
         $scope.updatebtn = false;
         $scope.getDamageHistory($scope.data.ODLN[0].U_EqNo);
    }


    
    //check the page loading from copy to contract 
    //CTC = Copy to Contract
    //Keep It Here
    if(getParameterByName('AOSUpdate')=='true')
    {
       $scope.CTCdata = angular.fromJson($cookies.get('AOS_data'));
       console.log("CRCDATA");
       console.log($scope.CTCdata);
       $scope.data.ODLN[0] = angular.copy($scope.CTCdata.ODLN[0]);
       $scope.data.ODLN[0].U_SurvyNo = getParameterByName('sn');
       $scope.SIselected = true;
       $scope.SAselected = true;
       //$scope.SalseQuoteNo = $scope.CTCdata.SQTO[0].U_Qno;

        $scope.getcity($scope.data.ODLN[0].U_Country);
        $scope.getlocation($scope.data.ODLN[0].U_City);

        $scope.Showsavebtn = false;
        $scope.updatebtn = true;

        $scope.getDamageHistory($scope.data.ODLN[0].U_EqNo);
    }



} ]);
