App.controller('salsequote_ctrl', ['$scope', '$rootScope', '$http', '$window', '$cookies','util_SERVICE','SQ_SERVICE','hotkeys','$location',

function ($scope, $rootScope, $http, $window, $cookies,US,SQS,hotkeys,$location) {
    $scope.savelable = "Save";
    $scope.savebtn = true;
    $scope.findbtn= false;
    $scope.updatebtn= false;
    $scope.loading= false;   
    $scope.whilefind = false;
    $scope.btndisable = true;
    $scope.ctype=false;
    $scope.CCselected = false;
    $rootScope.Rremark = "";

    $scope.g_rate= 0;
    $scope.g_Rrate= 0;
     

    $scope.user = angular.fromJson($cookies.get('UserData'));

    //SalesQuotation_Select Fined record

   $rootScope.FindoneRecord = function(code)
    {   
    
    
         SQS.FindRecord(code).then(function (response) {console.log(response);$scope.data=response.data;
         $scope.updateclick();
          if (response.data.VALIDATE[0].Status == "False") {          
          
            alert(response.data.VALIDATE[0].Msg);   
            $scope.resetall();
            $scope.findclick();
       }
       });
    }

    
    
    if(getParameterByName('sqapp')=='true' && $scope.user[0].Approver == "Y" )
    {
        $rootScope.FindoneRecord(getParameterByName('docno'));
        $scope.sqapp = true;
    }
    else{
        //alert("Not allowed to do Approval operation");
        $scope.sqapp = false;
    }
   //adding shortcuts
    hotkeys.add({combo: 'alt+left',description: '',callback: function() {$scope.FirstRecord();}});
    hotkeys.add({combo: 'alt+right',description: '',callback: function() {$scope.LastRecord();}});
    hotkeys.add({combo: 'ctrl+right',description: '',callback: function() {$scope.NextRecord();}});
    hotkeys.add({combo: 'ctrl+left',description: '',callback: function() {$scope.PreviousRecord();}});
    hotkeys.add({combo: 'space+f',description: '',callback: function() {$scope.findclick();}});
    hotkeys.add({combo: 'space+n',description: '',callback: function() {$scope.addclick();}});


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
    US.getegroup().then(function (response) { $scope.egroup = response.data.EQGROUP; console.log($scope.egroup); });
    US.getChargeType().then(function (response) { $scope.ChargeType = response.data.CHRGETYPE; console.log($scope.ChargeType); });
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

         $scope.resetall();
    }


    $scope.duplicateclick = function()
    {
        
         $scope.savebtn = true;
         $scope.findbtn= false;
         $scope.updatebtn= false;
         $scope.whilefind = false;
         $scope.ctype=false;

         $scope.data.SQTO[0].U_Qno = "";
         $scope.data.SQTO[0].U_Status = "";
        $scope.data.SQTO[0].U_AddrN = "";
        $scope.data.SQTO[0].U_Addr1 = "";
        $scope.data.SQTO[0].U_Addr2 = "";
        $scope.data.SQTO[0].U_Addr3 = "";
        $scope.data.SQTO[0].U_Addr4 = "";
        $scope.data.SQTO[0].U_Addr5 = "";
        $scope.data.SQTO[0].U_Addr6 = "";
        $scope.data.SQTO[0].U_TelNo = "";
        $scope.data.SQTO[0].U_FaxNo = "";
        $scope.data.SQTO[0].U_Mno = "";
        $scope.data.SQTO[0].U_Email = "";
        $scope.data.SQTO[0].U_Cname = "";
        $scope.data.SQTO[0].U_Ccode = "";


        $scope.whilefind = false;
        $scope.btndisable = true;
        $scope.ctype=false;
    }

     $scope.findclick = function()
    {
        
         $scope.savebtn = false;
         $scope.findbtn= true;
         $scope.updatebtn= false;
         $scope.resetall();
         $scope.data.SQTO[0].U_Cdate = "";
         $scope.whilefind = true;
         $scope.ctype=true;


    }
     $scope.getegroupName = function(code)
    {
         var ret = ""
         for (var i=0; i<$scope.egroup.length; i++) {
          if($scope.egroup[i].U_EQGroup == code)
            ret = $scope.egroup[i].U_EQGroupName;
        }
        return ret;
    }

    US.getcurency().then(function (response) { $scope.curency = response.data.CURRENCY; console.log($scope.curency); });
     $scope.getecurencyName = function(code)
    {
         var ret = ""
         for (var i=0; i<$scope.curency.length; i++) {
          if($scope.curency[i].U_Currency == code)
            ret = $scope.curency[i].U_CurrencyName;
        }
        return ret;
    }
    
    
     US.getGST().then(function (response) { $scope.GST = response.data.GST; console.log($scope.GST); });
     $scope.getGSTName = function(code)
    {
         var ret = ""
         for (var i=0; i<$scope.GST.length; i++) {
          if($scope.GST[i].U_GST == code)
            ret = $scope.GST[i].U_GSTName;
        }
        return ret;
    }



     US.getUOM().then(function (response) { $scope.UOM = response.data.UOM; console.log($scope.UOM); });
     $scope.getuomname = function(code)
    {
         var ret = ""
         for (var i=0; i<$scope.UOM.length; i++) {
          if($scope.UOM[i].U_UOM == code)
            ret = $scope.UOM[i].U_UOMName;
        }
        return ret;
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
    }

    $scope.getcustomer = function(type)
    {
    var c,n;
    if(type=='code')
    {
        c = $scope.data.SQTO[0].U_Ccode;
        n="";
    }
    else{
        c =""; 
        n=$scope.data.SQTO[0].U_Cname;
    }
         SQS.getcustomer(c,n).then(function (response) { $rootScope.customer = response.data; console.log($rootScope.customer); $("#myModal").modal(); 
                $scope.data.SQTO[0].U_Addr1 ="";
                $scope.data.SQTO[0].U_Addr2 ="";
                $scope.data.SQTO[0].U_Addr3 ="";
                $scope.data.SQTO[0].U_Addr4 ="";
                $scope.data.SQTO[0].U_Addr5 ="";
                $scope.data.SQTO[0].U_Addr6 ="";
                $scope.addressarray = []; });
    }

   

    //get Previous Record

    $scope.PreviousRecord = function()
    {
    
         SQS.PreviousRecord($scope.data.SQTO[0].U_Qno).then(function (response) {console.log(response);$scope.data=response.data; $scope.updateclick();});
    }


     //get NextRecord Record

    $scope.NextRecord = function()
    {
   
    
         SQS.NextRecord($scope.data.SQTO[0].U_Qno).then(function (response) {console.log(response);$scope.data=response.data;$scope.updateclick();});
    }

    //SalesQuotation_LastRecord

    $scope.LastRecord = function()
    {
   
    
         SQS.LastRecord().then(function (response) {console.log(response);$scope.data=response.data;$scope.updateclick();});
    }

    //SalesQuotation_FirstRecord

    $scope.FirstRecord = function()
    {
   
    
         SQS.FirstRecord().then(function (response) {console.log(response);$scope.data=response.data;$scope.updateclick();});
    }

    //SalesQuotation_MultipleFindRecord

    $scope.FindRecord = function()
    {   
    
         SQS.FindMultiRecord($scope.data.SQTO[0]).then(function (response) {$rootScope.findresult = response.data; console.log($rootScope.findresult); $("#findmodal").modal();
         
       });
    }

    


    //get one record from multiple
    $rootScope.findone = function (i) {
        //alert(i);
        //console.log($rootScope.fulldata.LEADM[i].Code);
        $scope.CCselected = true;
        $scope.data.SQTO[0].U_Ccode = $rootScope.customer.OCRD[i].U_Ccode;
        $scope.data.SQTO[0].U_Cname = $rootScope.customer.OCRD[i].U_Cname;
        $scope.data.SQTO[0].U_TelNo = $rootScope.customer.OCRD[i].U_TelNo;
        $scope.data.SQTO[0].U_FaxNo = $rootScope.customer.OCRD[i].U_FaxNo;
        $scope.data.SQTO[0].U_Mno = $rootScope.customer.OCRD[i].U_MNo;
        $scope.data.SQTO[0].U_Email = $rootScope.customer.OCRD[i].U_Email;

        
        //disable field depends up on c_type
        if($rootScope.customer.OCRD[i].U_Ctype=="L")
        {
            $scope.ctype=true;            
            $scope.data.SQTO[0].U_CPeriod1 ="";
            $scope.data.SQTO[0].U_CPeriod2 ="";
        }
        else
        {
            $scope.ctype=false;
        }

      $scope.findAddress($rootScope.customer.OCRD[i].U_Ccode);

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
        "SQTO": [{
            "U_Qno": "1103",
            "U_Status": "OPEN",
            "U_Uname": $scope.UserData[0].Code,
            "U_Cdate": "20-03-2016",
            "U_Qdate1": "20-03- 2016",
            "U_Qdate2": "20-03-2016",
            "U_Ccode": "0005",
            "U_Cname": "RAM",
            "U_CPeriod1": "20-03-2016",
            "U_CPeriod2": "20-03 -2018",
            "U_Pcode": "RRD1",
            "U_AddrN": "ajithnagar",
            "U_Addr1": "annanagar",
            "U_Addr2": "annanagar",
            "U_Addr3": "annanagar",
            "U_Addr4": "annanagar",
            "U_Addr5": "annanagar",
            "U_Addr6": "annanagar",
            "U_TelNo": "5210422",
            "U_FaxNo": "52120422",
            "U_Mno": "9629476950",
            "U_Email": "madeswaran1986@gmail.com",
            "U_Remarks": "madeswaran1986@gmail.com"
        }],
        "SQTOGEN": [{
            "U_Stype": "General",
            "U_Conti": "56",
            "U_Country": "India",
            "U_City": "namakkal",
            "U_Currency": "INR",
            "U_EQGroup": "medical",
            "U_Rate": "56",
            "U_RRate": "56",
            "U_UOM": "UNIT",
            "U_GST": "GST 2 %",
            "U_Remarks": "Testing data values"
        }, {
            "U_Stype": "Sub",
            "U_Conti": "20",
            "U_Country": "Signapore",
            "U_City": "namakkal",
            "U_Currency": "INR",
            "U_EQGroup": "medical",
            "U_Rate": "56",
            "U_UOM": "UNIT",
            "U_GST": "GST 4 %",
            "U_Remarks": "Testing data values2"
        }, {
            "U_Stype": "General",
            "U_Conti": "56",
            "U_Country": "India",
            "U_City": "namakkal",
            "U_Currency": "INR",
            "U_EQGroup": "medical",
            "U_Rate": "56",
            "U_UOM": "UNIT",
            "U_GST": "GST 2 %",
            "U_Remarks": "Testing data values"
        }, {
            "U_Stype": "Sub",
            "U_Conti": "20",
            "U_Country": "Signapore",
            "U_City": "namakkal",
            "U_Currency": "INR",
            "U_EQGroup": "medical",
            "U_Rate": "56",
            "U_UOM": "UNIT",
            "U_GST": "GST 4 %",
            "U_Remarks": "Testing data values2"
        }],
        "SQTOADD": [{
            "U_Ctype": "General",
            "U_Continent": "56",
            "U_Country": "India",
            "U_City": "namakkal",
            "U_Currency": "INR",
            "U_EQGroup": "medical",
            "U_Rate": "56",
            "U_UOM": "UNIT",
            "U_GST": "GST 2 %",
            "U_Remarks": "Testing data values"
        }, {
            "U_Ctype": "Sub",
            "U_Continent": "20",
            "U_Country": "Signapore",
            "U_City": "namakkal",
            "U_Currency": "INR",
            "U_EQGroup": "medical",
            "U_Rate": "56",
            "U_UOM": "UNIT",
            "U_GST": "GST 4 %",
            "U_Remarks": "Testing data values2"
        }, {
            "U_Ctype": "General",
            "U_Continent": "56",
            "U_Country": "India",
            "U_City": "namakkal",
            "U_Currency": "INR",
            "U_EQGroup": "medical",
            "U_Rate": "56",
            "U_UOM": "UNIT",
            "U_GST": "GST 2 %",
            "U_Remarks": "Testing data values"
        }, {
            "U_Ctype": "Sub",
            "U_Continent": "20",
            "U_Country": "Signapore",
            "U_City": "namakkal",
            "U_Currency": "INR",
            "U_EQGroup": "medical",
            "U_Rate": "56",
            "U_UOM": "UNIT",
            "U_GST": "GST 4 %",
            "U_Remarks": "Testing data values2"
        }],
        "ATTACHMENT":[{}]
    }
    //validate genarel 
    $scope.genval = function()
    {
     var retval = true;
     var errormsg = "";

     
     
        
         if($scope.g_Stype=="" || typeof $scope.g_Stype==="undefined")
        {
            retval = false;
            errormsg = errormsg+". Kindly Select Survey Type<br/>";
        }
        if($scope.g_Continent=="" || typeof $scope.g_Continent==="undefined")
        {
            retval = false;
            errormsg = errormsg+". Kindly Select Continent <br/>";
        }
        if($scope.g_Country=="" || typeof $scope.g_Country==="undefined")
        {
            retval = false;
            errormsg = errormsg+". Kindly Select Country  <br/>";
        }
        if($scope.g_city=="" || typeof $scope.g_city==="undefined")
        {
            retval = false;
            errormsg = errormsg+". Kindly Select City  <br/>";
        }
        if($scope.g_curency=="" || typeof $scope.g_curency==="undefined")
        {
            retval = false;
            errormsg = errormsg+". Kindly Select Currency  <br/>";
        }
         if($scope.g_egroup=="" || typeof $scope.g_egroup==="undefined")
        {
            retval = false;
            errormsg = errormsg+". Kindly Select Equipment Group   <br/>";
        }
        if($scope.g_uom=="" || typeof $scope.g_uom==="undefined")
        {
            retval = false;
            errormsg = errormsg+". Kindly Select UOM   <br/>";
        }
        if($scope.g_gst=="" || typeof $scope.g_gst==="undefined")
        {
            retval = false;
            errormsg = errormsg+". Kindly Select GST <br/>";
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
            "U_Stype": $scope.g_Stype,
            "U_Conti": angular.copy($scope.g_Continent.U_Conti),
            "U_Country": angular.copy($scope.g_Country.U_Country),
            "U_City": angular.copy($scope.g_city.U_City),
            "C_Conti": $scope.g_Continent,
            "C_Country": $scope.g_Country,
            "C_City": $scope.g_city,
            "U_Currency": $scope.g_curency,
            "U_EQGroup": $scope.g_egroup,
            "U_Rate": $scope.g_rate,
            "U_RRate": $scope.g_Rrate,
            "U_UOM": $scope.g_uom,
            "U_GST": $scope.g_gst,
            "U_Remarks": $scope.g_remark,
        };
        console.log($scope.gen);
        
        $scope.data.SQTOGEN.push(angular.copy($scope.gen));
        console.log($scope.data.SQTOGEN);
        $scope.g_reset();
        }
    }

    $scope.g_reset = function () {

            $scope.g_Stype = "";
            $scope.g_Continent= "";
            $scope.g_Country= "";
            $scope.g_city= "";
            $scope.g_curency= "";
            $scope.g_egroup= "";
            $scope.g_rate= 0;
            $scope.g_Rrate=0;
            $scope.g_uom= "";
            $scope.g_gst= "";
            $scope.g_remark= "";
        
        
    }


    $scope.g_edit = function(i)
    {
       // alert(i);

        $scope.data.SQTOGEN[i].edit = true;

        $scope.data.SQTOGEN[i].E_Conti = $scope.getcontijson($scope.data.SQTOGEN[i].U_Conti);
        $scope.getcountry($scope.data.SQTOGEN[i].E_Conti.U_Conti);

        $scope.data.SQTOGEN[i].E_Country = $scope.getcountryjson($scope.data.SQTOGEN[i].U_Country);
        $scope.getcity($scope.data.SQTOGEN[i].E_Country.U_Country);
        
        $scope.data.SQTOGEN[i].E_City = $scope.getcityjson($scope.data.SQTOGEN[i].U_City);
        
    }
    $scope.g_ok = function(i)
    {
       console.log( $scope.data.SQTOGEN[i].E_Conti);
        $scope.data.SQTOGEN[i].edit = false;
        $scope.data.SQTOGEN[i].U_Conti= angular.copy( $scope.data.SQTOGEN[i].E_Conti.U_Conti);
        $scope.data.SQTOGEN[i].U_Country= angular.copy( $scope.data.SQTOGEN[i].E_Country.U_Country);
        $scope.data.SQTOGEN[i].U_City= angular.copy( $scope.data.SQTOGEN[i].E_City.U_City);

        $scope.data.SQTOGEN[i].C_Conti= angular.copy( $scope.data.SQTOGEN[i].E_Conti);
        $scope.data.SQTOGEN[i].C_Country= angular.copy( $scope.data.SQTOGEN[i].E_Country);
        $scope.data.SQTOGEN[i].C_City= angular.copy( $scope.data.SQTOGEN[i].E_City);

       
    }

    $scope.g_del = function(i)
    {
        //alert(i);
        $scope.data.SQTOGEN.splice(i, 1); 
    }
     $scope.g_copy = function(i)
    {  

    


            $scope.g_Stype = $scope.data.SQTOGEN[i].U_Stype;
            $scope.g_Continent= $scope.getcontijson($scope.data.SQTOGEN[i].U_Conti);
            $scope.getcountry($scope.g_Continent.U_Conti);
            $scope.g_Country=  $scope.getcountryjson($scope.data.SQTOGEN[i].U_Country);
            $scope.getcity($scope.g_Country.U_Country);
            $scope.g_city= $scope.getcityjson($scope.data.SQTOGEN[i].U_City);
            $scope.g_curency= $scope.data.SQTOGEN[i].U_Currency;
            $scope.g_egroup= $scope.data.SQTOGEN[i].U_EQGroup;
            $scope.g_rate= $scope.data.SQTOGEN[i].U_Rate;
            $scope.g_Rrate= $scope.data.SQTOGEN[i].U_RRate;
            $scope.g_uom= $scope.data.SQTOGEN[i].U_UOM;
            $scope.g_gst= $scope.data.SQTOGEN[i].U_GST;
            $scope.g_remark= $scope.data.SQTOGEN[i].U_Remarks;
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
             $scope.data = {
            "SQTO": [{
                "U_Qno": "",
                "U_Status": "",
                "U_Uname": $scope.UserData[0].Name,
                "U_Cdate": $scope.today,
                "U_Qdate1": "",
                "U_Qdate2": "",
                "U_Ccode": "",
                "U_Cname": "",
                "U_CPeriod1": "",
                "U_CPeriod2": "",
                "U_Pcode": "",
                "U_AddrN": "",
                "U_Addr1": "",
                "U_Addr2": "",
                "U_Addr3": "",
                "U_Addr4": "",
                "U_Addr5": "",
                "U_Addr6": "",
                "U_TelNo": "",
                "U_FaxNo": "",
                "U_Mno": "",
                "U_Email": "",
                "U_Remarks": ""
            }],
            "SQTOGEN": [],
            "SQTOADD": [],
            "ATTACHMENT": []
        };
        $scope.whilefind = false;
        $scope.btndisable = true;
        $scope.ctype=false;

       
    }

    $scope.formsavedata = function(data)
    {
        for (var i=0; i<data.SQTOGEN.length; i++) {
          delete data.SQTOGEN[i].C_Conti;
          delete data.SQTOGEN[i].C_Country;
          delete data.SQTOGEN[i].C_City;

          delete data.SQTOGEN[i].E_Conti;
          delete data.SQTOGEN[i].E_Country;
          delete data.SQTOGEN[i].E_City;
            if(data.SQTOGEN[i].U_RRate===undefined || data.SQTOGEN[i].U_RRate==null)
            data.SQTOGEN[i].U_RRate=0;
         if(data.SQTOGEN[i].U_Rate===undefined || data.SQTOGEN[i].U_Rate==null)
            data.SQTOGEN[i].U_Rate=0;
          data.SQTOGEN[i].U_Rate = data.SQTOGEN[i].U_Rate.toFixed(2);
          data.SQTOGEN[i].U_RRate = data.SQTOGEN[i].U_RRate.toFixed(2);
          
          
        }

        for (var i=0; i<data.SQTOADD.length; i++) {
          delete data.SQTOADD[i].C_Conti;
          delete data.SQTOADD[i].C_Country;
          delete data.SQTOADD[i].C_City;

          delete data.SQTOADD[i].Ea_Conti;
          delete data.SQTOADD[i].Ea_Country;
          delete data.SQTOADD[i].Ea_City;
          delete data.SQTOADD[i].Ea_EQGroup
            if(data.SQTOADD[i].U_RRate===undefined || data.SQTOADD[i].U_RRate==null)
            data.SQTOADD[i].U_RRate=0;
         if(data.SQTOADD[i].U_Rate===undefined || data.SQTOADD[i].U_Rate==null)
            data.SQTOADD[i].U_Rate=0;
          data.SQTOADD[i].U_Rate = data.SQTOADD[i].U_Rate.toFixed(2);
          data.SQTOADD[i].U_RRate =data.SQTOADD[i].U_RRate.toFixed(2);


          data

        }

        return data;

        
    }


    //validation function
    $scope.val  = function()
    {
     var retval = true;
     var errormsg = "";

     if($scope.CCselected==false)
        {
            retval = false;
            errormsg = errormsg+". Customer Code or Name Invalid <br/>";
        }
     
     if($scope.data.SQTOGEN.length== 0 || typeof $scope.data.SQTOGEN==="undefined")
        {
            retval = false;
            errormsg = errormsg+". Kindly Add Record in General<br/>";
        }
        
         if($scope.data.SQTO[0].U_Uname=="" || typeof $scope.data.SQTO[0].U_Uname==="undefined")
        {
            retval = false;
            errormsg = errormsg+". Kindly Enter User Name<br/>";
        }
         if($scope.data.SQTO[0].U_Qdate1=="" || typeof $scope.data.SQTO[0].U_Qdate1==="undefined" || $scope.data.SQTO[0].U_Qdate2=="" || typeof $scope.data.SQTO[0].U_Qdate2==="undefined" )
        {
            retval = false;
            errormsg = errormsg+". Kindly Select Quote Validity Date<br/>";
        }
        if(($scope.data.SQTO[0].U_CPeriod1=="" || typeof $scope.data.SQTO[0].U_CPeriod1==="undefined" || $scope.data.SQTO[0].U_CPeriod2=="" || typeof $scope.data.SQTO[0].U_CPeriod2==="undefined") && $scope.ctype==false )
        {
            retval = false;
            errormsg = errormsg+". Kindly Select Contract Period Date<br/>";
        }
        if($scope.data.SQTO[0].U_Cdate=="" || $scope.data.SQTO[0].U_Cdate=="undefined")
        {
            retval = false;
             errormsg = errormsg+". Kindly Select Create Date<br/>";//alert("Kindly");
           // bootbox.alert("Hello world!", null);
        }
        if($scope.data.SQTO[0].U_Ccode=="" || $scope.data.SQTO[0].U_Ccode=="undefined")
        {
            retval = false;
            errormsg = errormsg+". Kindly  Select Customer Code<br/>";//alert("Kindly");alert("Kindly ");
        }
        if(errormsg!="")
            bootbox.alert("<div style='color:red'>"+errormsg+"</div>", null);
        return retval;
    };


    $scope.resetall();
    //save service call
    $scope.save = function()
    {
    
    if($scope.val())
    {
        $scope.savelable = "Loading..";
        $scope.savebtn = true;
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var parms = encodeURIComponent(JSON.stringify($scope.formsavedata($scope.data)));
        $http.post(US.url+"SalesQuotationAdd", "value=" + parms, config)
   .then(
       function (response) {
           // success callback
           console.log(response.data);
           if (response.data.VALIDATE[0].Status == "True") {
              $scope.savelable = "Save";
              $scope.savebtn = true;
              $scope.resetall();
              alert(response.data.VALIDATE[0].Msg);
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



    //set_Quote Validity to date
    $scope.set_QTO_date = function()
    {
        var d = $scope.data.SQTO[0].U_Qdate1.split('/');
        //alert(d[1]);
        var CurrentDate = new Date(d[2],d[1],d[0]);
        console.log(d[2]+d[1]+d[0]);
        console.log(CurrentDate.getFullYear());
        var yearBefore = CurrentDate.getFullYear();
        CurrentDate.setMonth(CurrentDate.getMonth() + 3);
        var yearAfter = CurrentDate.getFullYear();
        
        if(CurrentDate.getMonth()==0)
        {
             month = '' + (12);
             year = yearBefore;
             }
        else
        {
             month = '' + (CurrentDate.getMonth());
              year = yearAfter;
             }
        day = '' + CurrentDate.getDate();
        

        
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

        $scope.data.SQTO[0].U_Qdate2 =day+"/"+month+"/"+year;
    }


    $scope.SQM_Approval = function()
    {
    
        if($scope.val())
        {

        //SQS.SQMApproval($scope.data.SQTO.U_Qno).then(function (response) { console.log(response); });
        SQS.aproval($scope.data.SQTO[0].U_Qno).then(function (response) {
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

        SQS.approver($scope.data.SQTO[0].U_Qno,'Rejected',remarks,uname).then(function (response) {
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

        SQS.approver($scope.data.SQTO[0].U_Qno,'Approved',remarks,uname).then(function (response) {
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
        $http.post(US.url+"SalesQuotation_Update", "value=" + parms, config)
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

   $scope.Att_file="";
   $scope.Fileloading = true;
   //file upload 
   $scope.uploadFile = function() {
   $scope.Fileloading = false;
   var fd = new FormData();
         fd.append('file', $scope.myFile);
         fd.append('name', "1");
         var uploadUrl = US.Host+"FileUpload.php";
         $http.post(uploadUrl, fd, {
             transformRequest: angular.identity,
             headers: {'Content-Type': undefined,'Process-Data': false}
         })
         .success(function(data){
         $scope.Fileloading = true;
            console.log(data);
            $scope.myFile = []
            $("#fileopen").val("");
            $scope.ATT =  {
            "U_FileName": data.FileName,
            "U_FilePath": data.FilePath,
            "U_id": data.id,
            "U_Date": data.Date
        };
        if(data.Status=="true")
        {
            $scope.data.ATTACHMENT.push(angular.copy($scope.ATT));
            }
            else{
                alert(data.errMSG);
            }
         })
         .error(function(){
            $scope.Fileloading = true;
            alert("File Upload Failed");
         });

};

   
  $scope.fileBurl = US.BURL;

} ]);
