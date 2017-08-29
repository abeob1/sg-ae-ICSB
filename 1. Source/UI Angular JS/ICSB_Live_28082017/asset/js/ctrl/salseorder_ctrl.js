App.controller('salseorder_ctrl', ['$scope', '$rootScope', '$http', '$window', '$cookies', 'util_SERVICE', 'SO_SERVICE', 'hotkeys', '$location',

function ($scope, $rootScope, $http, $window, $cookies, US, SOS, hotkeys, $location) {
    $scope.savelable = "Save";
    $scope.savebtn = true;
    $scope.findbtn = false;
    $scope.updatebtn = false;
    $scope.loading = false;
    $scope.whilefind = false;
    $scope.btndisable = true;
    $scope.ctype = false;
    $scope.CCselected = false;
    $scope.SIselected = false;
    $rootScope.Rremark = "";
    $scope.updatedisabled = false;


    $scope.user = angular.fromJson($cookies.get('UserData'));

    //SalesQuotation_Select Fined record

    $rootScope.FindoneRecord = function (code) {


        SOS.FindRecord(code).then(function (response) {
            console.log(response); $scope.data = response.data;
            $scope.updateclick();
            if (response.data.VALIDATE[0].Status == "False") {

                alert(response.data.VALIDATE[0].Msg);
                $scope.resetall();
                $scope.findclick();
            }
        });
    }


    $scope.SetRecord = function (d) {
        $scope.data = d;
        $scope.updateclick();
    }

    //adding shortcuts
    hotkeys.add({ combo: 'alt+left', description: '', callback: function () { $scope.FirstRecord(); } });
    hotkeys.add({ combo: 'alt+right', description: '', callback: function () { $scope.LastRecord(); } });
    hotkeys.add({ combo: 'ctrl+right', description: '', callback: function () { $scope.NextRecord(); } });
    hotkeys.add({ combo: 'ctrl+left', description: '', callback: function () { $scope.PreviousRecord(); } });
    hotkeys.add({ combo: 'space+f', description: '', callback: function () { $scope.findclick(); } });
    hotkeys.add({ combo: 'space+n', description: '', callback: function () { $scope.addclick(); } });


    //today date 
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!

    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }
    $scope.today = dd + '/' + mm + '/' + yyyy;
    //today date 

    $scope.g_remark = "";
    $scope.a_remark = "";

    $scope.addressarray = [];
    $scope.SurveyorIDMaster = [];
    SOS.getetype().then(function (response) { $scope.etype = response.data.SURCRTA; });
    SOS.getSurveyCriteria().then(function (response) { $scope.SurveyCriteria = response.data.SURCRTA; });
    SOS.getSurveyorIDMaster().then(function (response) { $scope.SurveyorIDMaster = response.data.SUID; }); //get full surveyer


    $scope.geteSCname = function (code) {
        var ret = ""
        for (var i = 0; i < $scope.SurveyCriteria.length; i++) {
            if ($scope.SurveyCriteria[i].U_SURCRTACODE == code)
                ret = $scope.SurveyCriteria[i].U_SURCRTANAME;
        }
        return ret;
    }

    $scope.getegroupName = function (code) {
        var ret = ""
        for (var i = 0; i < $scope.etype.length; i++) {
            if ($scope.etype[i].U_EQTYPECODE == code)
                ret = $scope.etype[i].U_EQTYPENAME;
        }
        return ret;
    }

    $scope.getCtypeName = function (code) {
        var ret = ""
        for (var i = 0; i < $scope.ChargeType.length; i++) {
            if ($scope.ChargeType[i].U_Stype == code)
                ret = $scope.ChargeType[i].U_StypeName;
        }
        return ret;
    }



    $scope.addclick = function () {

        $scope.savebtn = true;
        $scope.findbtn = false;
        $scope.updatebtn = false;
        $scope.whilefind = false;
        $scope.ctype = false;
        $scope.updatedisabled = false;

        $scope.resetall();
    }

    $scope.findclick = function () {
        $scope.updatedisabled = false;
        $scope.savebtn = false;
        $scope.findbtn = true;
        $scope.updatebtn = false;
        $scope.resetall();
        $scope.data.ORDR[0].U_Cdate = "";
        $scope.whilefind = true;
        $scope.ctype = true;


    }


    US.getstype().then(function (response) { $scope.stype = response.data.SURTYPE; console.log($scope.stype); });
    $scope.getestypeName = function (code) {
        var ret = ""
        for (var i = 0; i < $scope.stype.length; i++) {
            if ($scope.stype[i].U_Stype == code)
                ret = $scope.stype[i].U_StypeName;
        }
        return ret;
    }




    //geting full country city values to show country & city name in table    
    US.getContinent().then(function (response) { $scope.Continent = response.data.CONT; console.log($scope.Continent); });
    US.getCityMaster().then(function (response) { $scope.CityMaster = response.data.CITY; console.log($scope.CityMaster); });
    US.getCountryMaster().then(function (response) { $scope.CountryMaster = response.data.COUNTRY; console.log($scope.CountryMaster); });
    US.getLocationMaster().then(function (response) { $scope.LocationMaster = response.data.LOC; console.log($scope.LocationMaster); });
    US.getChargeType().then(function (response) { $scope.ChargeType = response.data.CHRGETYPE; console.log($scope.ChargeType); });
    US.getIMGMaster().then(function (response) { $scope.IMO = response.data.IOM; console.log($scope.IMO); });

    //get conti full json to select dropdown while edite 
    $scope.getcontijson = function (code) {


        var ret = ""
        for (var i = 0; i < $scope.Continent.length; i++) {
            if ($scope.Continent[i].U_Conti == code)
                ret = $scope.Continent[i];
        }
        return ret;
    }

    //get country full json to select dropdown while edite 
    $scope.getcountryjson = function (code) {


        var ret = ""
        for (var i = 0; i < $scope.CountryMaster.length; i++) {
            if ($scope.CountryMaster[i].U_Country == code)
                ret = $scope.CountryMaster[i];
        }
        return ret;
    }
    //get city full json to select dropdown while edite 
    $scope.getcityjson = function (code) {


        var ret = ""
        for (var i = 0; i < $scope.CityMaster.length; i++) {
            if ($scope.CityMaster[i].U_City == code)
                ret = $scope.CityMaster[i];
        }
        console.log("City");
        console.log(ret);
        return ret;
    }

    //get Location full json to select dropdown while edite 
    $scope.getlocationjson = function (code) {


        var ret = ""
        for (var i = 0; i < $scope.LocationMaster.length; i++) {
            if ($scope.LocationMaster[i].U_Loc == code)
                ret = $scope.LocationMaster[i];
        }
        console.log(ret);
        return ret;
    }




    $scope.getContinentName = function (code) {
        var ret = ""
        for (var i = 0; i < $scope.Continent.length; i++) {
            if ($scope.Continent[i].U_Conti == code)
                ret = $scope.Continent[i].U_ContiName;
        }
        return ret;
    }

    $scope.getcountry = function (conti) {
        US.getcountry(conti).then(function (response) { $scope.country = response.data.COUNTRY; console.log($scope.country); });
    }
    $scope.getcountryName = function (code) {
        var ret = ""
        for (var i = 0; i < $scope.CountryMaster.length; i++) {
            if ($scope.CountryMaster[i].U_Country == code)
                ret = $scope.CountryMaster[i].U_CountryName;
        }
        return ret;
    }
    $scope.getlocation = function (con) {
        US.getlocation(con).then(function (response) { $scope.location = response.data.LOC; console.log($scope.location); });
    }
    $scope.getlocationName = function (code) {
        var ret = ""
        for (var i = 0; i < $scope.LocationMaster.length; i++) {
            if ($scope.LocationMaster[i].U_Loc == code)
                ret = $scope.LocationMaster[i].U_LocName;
        }
        return ret;
    }


    $scope.getcity = function (con) {
        US.getcity(con).then(function (response) { $scope.city = response.data.CITY; console.log($scope.city); });
    }
    $scope.getcityName = function (code) {
        var ret = ""
        for (var i = 0; i < $scope.CityMaster.length; i++) {
            if ($scope.CityMaster[i].U_City == code)
                ret = $scope.CityMaster[i].U_CityName;
        }
        return ret;
    }

    $scope.updateclick = function () {
        $scope.savebtn = false;
        $scope.findbtn = false;
        $scope.updatebtn = true;
        $scope.whilefind = false;
        $scope.btndisable = false;
        $scope.ctype = false;
        $scope.CCselected = true;
        $scope.SIselected = true;
        $scope.getcity($scope.data.ORDR[0].U_Country);
        $scope.getlocation($scope.data.ORDR[0].U_City);
        $scope.updatedisabled = true;
    }

    $scope.getcustomer = function (type) {
        var c, n;
        if (type == 'code') {
            c = $scope.data.ORDR[0].CardCode;
            n = "";
        }
        else {
            c = "";
            n = $scope.data.ORDR[0].CardName;
        }
        SOS.getcustomer(c, n, $scope.UserData[0].Code).then(function (response) {
            $rootScope.customer = response.data; $("#myModal").modal();
        });
    }

    //fill the cname and code 
    $rootScope.Fillcustomer = function (c, n) {
        $scope.data.ORDR[0].CardCode = c;
        $scope.data.ORDR[0].CardName = n;
        $scope.CCselected = true;

    }




    $scope.getsurveyorID = function () {

        SOS.getsurveyorID($scope.UserData[0].Code).then(function (response) {
            $rootScope.surveyor = response.data; $("#myModal2").modal();
        });
    }

    //get SurveyorName by ID
    $scope.getSurveyorName = function (id) {

        var ret = ""
        for (var i = 0; i < $scope.SurveyorIDMaster.length; i++) {
            if ($scope.SurveyorIDMaster[i].U_SCode == id)
                ret = $scope.SurveyorIDMaster[i].U_SName;
        }
        return ret;
    }

    //fill the cname and code 
    $rootScope.Fillsurveyor = function (c, n) {
        $scope.data.ORDR[0].U_SurveyorID = c;
        $scope.data.ORDR[0].U_SurveyorName = n;

        $scope.SIselected = true;

    }



    //get Previous Record

    $scope.PreviousRecord = function () {

        SOS.PreviousRecord($scope.data.ORDR[0].U_OrderNo, $scope.UserData[0].Code).then(function (response) { console.log(response); $scope.data = response.data; $scope.updateclick(); });
    }


    //get NextRecord Record

    $scope.NextRecord = function () {


        SOS.NextRecord($scope.data.ORDR[0].U_OrderNo, $scope.UserData[0].Code).then(function (response) { console.log(response); $scope.data = response.data; $scope.updateclick(); });
    }

    //SalesQuotation_LastRecord

    $scope.LastRecord = function () {


        SOS.LastRecord($scope.UserData[0].Code).then(function (response) { console.log(response); $scope.data = response.data; $scope.updateclick(); });
    }

    //SalesQuotation_FirstRecord

    $scope.FirstRecord = function () {


        SOS.FirstRecord($scope.UserData[0].Code).then(function (response) { console.log(response); $scope.data = response.data; $scope.updateclick(); });
    }

    //SalesQuotation_MultipleFindRecord


    $scope.findAllRecords = function () {

        SOS.FindMultiRecord($scope.data.ORDR[0]).then(function (response) { $rootScope.findresult = response.data; console.log($rootScope.findresult); $("#findmodal").modal(); });
    }

    $rootScope.FindRecord = function (code) {


        SOS.SalesOrder_Find(code, $scope.UserData[0].Code).then(function (response) {
            if (response.data.VALIDATE === undefined) {
                console.log(response);
                $scope.data = response.data;
                $scope.updateclick();
            }
            else {
                alert(response.data.VALIDATE[0].Msg);
            }

        });
    }






    $scope.findAddress = function (code) {
        $scope.addressarray = [];
        for (var i = 0; i < $rootScope.customer.ADDR.length; i++) {
            if ($rootScope.customer.ADDR[i].U_Ccode == code)
                $scope.addressarray.push($rootScope.customer.ADDR[i]);
        }
        console.log($scope.addressarray);
        if ($scope.addressarray.length > 0) {
            $scope.setaddress($scope.addressarray[0].U_AddrN);
            $scope.U_AddrN = $scope.addressarray[0].U_AddrN;
            //alert($scope.addressarray[0].U_AddrN);
        }
    }

    $scope.setaddress = function (name) {
        for (var i = 0; i < $scope.addressarray.length; i++) {
            if ($scope.addressarray[i].U_AddrN == name) {
                $scope.data.SQTO[0].U_Addr1 = $scope.addressarray[i].U_Addr1;
                $scope.data.SQTO[0].U_Addr2 = $scope.addressarray[i].U_Addr2;
                $scope.data.SQTO[0].U_Addr3 = $scope.addressarray[i].U_Addr3;
                $scope.data.SQTO[0].U_Addr4 = $scope.addressarray[i].U_Addr4;
                $scope.data.SQTO[0].U_Addr5 = $scope.addressarray[i].U_Addr5;
                $scope.data.SQTO[0].U_Addr6 = $scope.addressarray[i].U_Addr6;
            }
        }
    }

    $scope.data = {
        "ORDR": [{
            "U_UCode": $scope.UserData[0].Code,
            "U_OrderNo": "",
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
            "Comments": "Remarks 123"
        }],
        "SQTOGEN": [{
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
        }]
    }
    //validate genarel 
    $scope.genval = function () {
        var retval = true;
        var errormsg = "";


        if ($scope.data.SQTOGEN.length >= 1) {
            retval = false;
            errormsg = errormsg + ". Can't Add More than One Record<br/>";
        }

        if ($scope.Quantity == "" || typeof $scope.Quantity === "undefined") {
            retval = false;
            errormsg = errormsg + ". Kindly Enter Quantity<br/>";
        }
        if ($scope.U_PDate == "" || typeof $scope.U_PDate === "undefined") {
            retval = false;
            errormsg = errormsg + ". Kindly Select Date <br/>";
        }
        if ($scope.U_EQType == "" || typeof $scope.U_EQType === "undefined") {
            retval = false;
            errormsg = errormsg + ". Kindly Select Equipment Type   <br/>";
        }
        if ($scope.U_SCriteria == "" || typeof $scope.U_SCriteria === "undefined") {
            retval = false;
            errormsg = errormsg + ". Kindly Select Survey Criteria   <br/>";
        }

        if (errormsg != "")
            bootbox.alert("<div style='color:red'>" + errormsg + "</div>", null);
        return retval;
    }
    //add record to gen
    $scope.g_add = function () {
        if ($scope.genval()) {
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
        $scope.U_PDate = "";
        $scope.U_EQType = "";
        $scope.U_SCriteria = "";


    }


    $scope.g_edit = function (i) {
        // alert(i);

        $scope.data.SQTOGEN[i].edit = true;


    }
    $scope.g_ok = function (i) {

        $scope.data.SQTOGEN[i].edit = false;



    }

    $scope.g_del = function (i) {
        //alert(i);
        $scope.data.SQTOGEN.splice(i, 1);
    }
    $scope.g_copy = function (i) {

        $scope.Quantity = $scope.data.SQTOGEN[i].Quantity;
        $scope.U_PDate = $scope.data.SQTOGEN[i].U_PDate;
        $scope.U_EQType = $scope.data.SQTOGEN[i].U_EQType;
        $scope.U_SCriteria = $scope.data.SQTOGEN[i].U_SCriteria;
    }



    //validate addon 
    $scope.addval = function () {
        var retval = true;
        var errormsg = "";




        if ($scope.a_Ctype == "" || typeof $scope.a_Ctype === "undefined") {
            retval = false;
            errormsg = errormsg + ". Kindly Select Charge Type<br/>";
        }
        if ($scope.U_PDate == "" || typeof $scope.U_PDate === "undefined") {
            retval = false;
            errormsg = errormsg + ". Kindly Select Date <br/>";
        }
        if (errormsg != "")
            bootbox.alert("<div style='color:red'>" + errormsg + "</div>", null);
        return retval;
    }


    //add-on functions

    $scope.a_add = function () {
        console.log($scope.a_Ctype);
        if ($scope.addval()) {
            $scope.add = {
                "U_ChrgCode": $scope.a_Ctype.U_Stype,
                "U_ChrgName": $scope.a_Ctype.U_StypeName,
                "U_PDate": $scope.U_PDate,
                "LineStatus": ""
            };
            $scope.data.SQTOADD.push(angular.copy($scope.add));
            $scope.a_reset();
        }
    }

    $scope.a_reset = function () {

        $scope.a_Ctype = "";
        $scope.U_PDate = "";

    }


    $scope.a_edit = function (i) {
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
    $scope.a_ok = function (i) {
        //alert(i);
        $scope.data.SQTOADD[i].edit = false;

        $scope.data.SQTOADD[i].U_Continent = angular.copy($scope.data.SQTOADD[i].Ea_Conti.U_Conti);
        $scope.data.SQTOADD[i].U_Country = angular.copy($scope.data.SQTOADD[i].Ea_Country.U_Country);
        $scope.data.SQTOADD[i].U_City = angular.copy($scope.data.SQTOADD[i].Ea_City.U_City);
        $scope.data.SQTOADD[i].U_EQGroup = angular.copy($scope.data.SQTOADD[i].Ea_EQGroup.U_Loc);

        $scope.data.SQTOADD[i].C_Conti = angular.copy($scope.data.SQTOADD[i].Ea_Conti);
        $scope.data.SQTOADD[i].C_Country = angular.copy($scope.data.SQTOADD[i].Ea_Country);
        $scope.data.SQTOADD[i].C_City = angular.copy($scope.data.SQTOADD[i].Ea_City);

    }

    $scope.a_del = function (i) {
        //alert(i);
        $scope.data.SQTOADD.splice(i, 1);
    }
    $scope.a_copy = function (i) {
        $scope.a_Ctype = $scope.data.SQTOADD[i].U_Ctype;
        $scope.a_Continent = $scope.getcontijson($scope.data.SQTOADD[i].U_Continent);
        $scope.getcountry($scope.data.SQTOADD[i].U_Continent);

        $scope.a_Country = $scope.getcountryjson($scope.data.SQTOADD[i].U_Country);
        $scope.getcity($scope.data.SQTOADD[i].U_Country);

        $scope.a_city = $scope.getcityjson($scope.data.SQTOADD[i].U_City);
        $scope.getlocation($scope.data.SQTOADD[i].U_City);

        $scope.a_curency = $scope.data.SQTOADD[i].U_Currency;
        $scope.a_egroup = $scope.getlocationjson($scope.data.SQTOADD[i].U_EQGroup);
        $scope.a_rate = $scope.data.SQTOADD[i].U_Rate;
        $scope.a_uom = $scope.data.SQTOADD[i].U_UOM;
        $scope.a_gst = $scope.data.SQTOADD[i].U_GST;
        $scope.a_remark = $scope.data.SQTOADD[i].U_Remarks;
    }


    //resetall field
    $scope.resetall = function () {
        $scope.data = {
            "ORDR": [{
                "U_UCode": $scope.UserData[0].Code,
                "U_OrderNo": "",
                "U_UName": $scope.UserData[0].Name,
                "U_Cdate": $scope.today,
                "DocDate": "",
                "CardCode": "",
                "CardName": "",
                "U_SurveyorID": "",
                "NumAtCard": "",
                "U_STypeCode": "",
                "U_STypeName": "",
                "U_Country": "",
                "U_City": "",
                "U_Loc": "",
                "Comments": "",
                "Project":""
            }],
            "SQTOGEN": [],
            "SQTOADD": [],
            "ATTACHMENT": []
        };
        $scope.whilefind = false;
        $scope.btndisable = true;
        $scope.ctype = false;
        $scope.CCselected = false;
        $scope.SIselected = false;


    }

    $scope.formsavedata = function (data) {




        return data;


    }


    //validation function
    $scope.val = function () {
        var retval = true;
        var errormsg = "";

        if ($scope.CCselected == false) {
            retval = false;
            errormsg = errormsg + ". Customer Code or Name Invalid <br/>";
        }



        if ($scope.data.SQTOGEN.length == 0 || typeof $scope.data.SQTOGEN === "undefined") {
            retval = false;
            errormsg = errormsg + ". Kindly Add Record in General<br/>";
        }

        if ($scope.data.ORDR[0].U_UName == "" || typeof $scope.data.ORDR[0].U_UName === "undefined") {
            retval = false;
            errormsg = errormsg + ". Kindly Check User Name<br/>";
        }
        if ($scope.data.ORDR[0].U_Cdate == "" || $scope.data.ORDR[0].U_Cdate == "undefined") {
            retval = false;
            errormsg = errormsg + ". Kindly Select Create Date<br/>"; //alert("Kindly");
            // bootbox.alert("Hello world!", null);
        }
        if ($scope.data.ORDR[0].DocDate == "" || $scope.data.ORDR[0].DocDate == "undefined") {
            retval = false;
            errormsg = errormsg + ". Kindly  Select Order Date<br/>"; //alert("Kindly");alert("Kindly ");
        }
        if ($scope.data.ORDR[0].U_STypeCode == "" || $scope.data.ORDR[0].U_STypeCode == "undefined") {
            retval = false;
            errormsg = errormsg + ". Kindly  Select Survey Type<br/>"; //alert("Kindly");alert("Kindly ");
        }
        if ($scope.data.ORDR[0].U_Country == "" || $scope.data.ORDR[0].U_Country == "undefined") {
            retval = false;
            errormsg = errormsg + ". Kindly  Select Country<br/>"; //alert("Kindly");alert("Kindly ");
        }
        if ($scope.data.ORDR[0].U_City == "" || $scope.data.ORDR[0].U_City == "undefined") {
            retval = false;
            errormsg = errormsg + ". Kindly  Select City<br/>"; //alert("Kindly");alert("Kindly ");
        }
        if ($scope.data.ORDR[0].U_Loc == "" || $scope.data.ORDR[0].U_Loc == "undefined") {
            retval = false;
            errormsg = errormsg + ". Kindly  Select Location<br/>"; //alert("Kindly");alert("Kindly ");
        }

        if (errormsg != "")
            bootbox.alert("<div style='color:red'>" + errormsg + "</div>", null);
        return retval;
    };



    $scope.resetall();
    //save service call
    $scope.save = function () {

        if ($scope.val()) {
            $scope.savelable = "Loading..";
            $scope.savebtn = true;
            var config = {
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
                }
            }

            var parms = encodeURIComponent(JSON.stringify($scope.data));
            $http.post(US.url + "SalesOrderAdd", "value=" + parms, config)
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
           else {
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
    $scope.set_QTO_date = function () {
        var d = $scope.data.SQTO[0].U_Qdate1.split('/');
        //alert(d[1]);
        var CurrentDate = new Date(d[2], d[1], d[0]);
        CurrentDate.setMonth(CurrentDate.getMonth() + 3);
        if (CurrentDate.getMonth() == 0)
            month = '' + (12);
        else
            month = '' + (CurrentDate.getMonth());
        day = '' + CurrentDate.getDate();
        year = CurrentDate.getFullYear();


        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;

        $scope.data.SQTO[0].U_Qdate2 = day + "/" + month + "/" + year;
    }


    $scope.SQM_Approval = function () {

        if ($scope.val()) {

            //SOS.SQMApproval($scope.data.SQTO.U_Qno).then(function (response) { console.log(response); });
            SOS.aproval($scope.data.SQTO[0].U_Qno).then(function (response) {
                if (response.data.APPROVAL[0].Status == "True") {
                    $scope.addclick();
                    alert(response.data.APPROVAL[0].Msg);
                }
                else {
                    alert(response.data.APPROVAL[0].Msg);
                }
            });


        }
    }



    //approvel Reject by approver
    $rootScope.Reject = function () {

        var remarks = $rootScope.Rremark;
        var uname = $scope.UserData[0].Name;

        SOS.approver($scope.data.SQTO[0].U_Qno, 'Rejected', remarks, uname).then(function (response) {
            if (response.data.APPROVAL[0].Status == "True") {
                // $scope.addclick();
                alert(response.data.APPROVAL[0].Msg);
                window.location = "SqApproval.aspx";
            }
            else {
                alert(response.data.APPROVAL[0].Msg);
            }
        });



    }


    //approvel approved by approver
    $rootScope.Approved = function () {

        var remarks = "";
        var uname = $scope.UserData[0].Name;

        SOS.approver($scope.data.SQTO[0].U_Qno, 'Approved', remarks, uname).then(function (response) {
            if (response.data.APPROVAL[0].Status == "True") {
                //$scope.addclick();
                alert(response.data.APPROVAL[0].Msg);
                window.location = "SqApproval.aspx";

            }
            else {
                alert(response.data.APPROVAL[0].Msg);
            }
        });



    }


    //update record 
    $scope.UpdateRecord = function () {

        if ($scope.val()) {


            var config = {
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
                }
            }

            var parms = encodeURIComponent(JSON.stringify($scope.formsavedata($scope.data)));
            $http.post(US.url + "SalesOrderUpdate", "value=" + parms, config)
   .then(
       function (response) {
           // success callback
           console.log(response.data);
           if (response.data.VALIDATE[0].Status == "Ture") {
               $scope.addclick();
               alert(response.data.VALIDATE[0].Msg);
           }
           else {
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
    $scope.CopyToContract = function () {

        $cookies.put('CTC_data', JSON.stringify($scope.data));
        window.location = "Customer_Contract.aspx?CTC=true";
    }

    //disable approved btn while status Approved 
    $scope.checkSQAstatus = function (val) {
        var rval = false;
        if (val == 'Approved') {
            rval = true;
        }

        return rval;

    }

    $rootScope.gotoSurveyFrom = function (formname) {
        if (formname == "AOS") {
            $cookies.put('AOS_data', JSON.stringify($scope.data));
            window.location = "Any_Other_Survey.aspx?AOS=true";
        }
        if (formname == "ONHS") {
            $cookies.put('AOS_data', JSON.stringify($scope.data));
            window.location = "On_Hire_Survey.aspx?AOS=true";
        }
        if (formname == "CS1") {
            $cookies.put('AOS_data', JSON.stringify($scope.data));
            window.location = "CC_Survey1.aspx?AOS=true";
        }
        if (formname == "OFFHS") {
            $cookies.put('AOS_data', JSON.stringify($scope.data));
            window.location = "Off_Hire_In_Service_Survey.aspx?AOS=true";
        }

    }

    $rootScope.gotoSurveyFromByLink = function (sno) {
        SOS.gotoSurveyFromByLink(sno).then(function (response) {
            response.data.ODLN[0].U_OrderNo = $scope.data.ORDR[0].U_OrderNo;
            console.log(response);
            if (response.data.ODLN[0].U_FormName == "AOS") {
                $cookies.put('AOS_data', JSON.stringify(response.data));
                window.location = "Any_Other_Survey.aspx?AOSUpdate=true&sn=" + sno;
            }
            if (response.data.ODLN[0].U_FormName == "ONHS") {
                $cookies.put('AOS_data', JSON.stringify(response.data));
                window.location = "On_Hire_Survey.aspx?AOSUpdate=true&sn=" + sno;
            }
            if (response.data.ODLN[0].U_FormName == "CS1") {
                $cookies.put('AOS_data', JSON.stringify(response.data));
                console.log(response.data);
                window.location = "CC_Survey1.aspx?AOSUpdate=true&sn=" + sno;
            }
            if (response.data.ODLN[0].U_FormName == "OFFHS") {
                $cookies.put('AOS_data', JSON.stringify(response.data));
                window.location = "Off_Hire_In_Service_Survey.aspx?AOSUpdate=true&sn=" + sno;
            }


        });

    }


    $scope.checksurvey = function () {
        SOS.checksurvey($scope.data.ORDR[0].U_OrderNo).then(function (response) {
            console.log(response);
            $rootScope.mySdata = response.data;
            if ($rootScope.mySdata.VALIDATE === undefined) {
                $("#checkSModal").modal();
            }
        });
    }


    //Load data by Back to sale order
    if (getParameterByName('BTSO') == 'true') {
        var sn = getParameterByName('sn');
        SOS.SalesOrder_Find(sn, $scope.UserData[0].Code).then(function (response) {
            console.log(response);
            $scope.data = response.data;
            $scope.updateclick();
        });

    }

    //Load data by Back to Survey List
    if (getParameterByName('BTSL') == 'true') {
        var sn = getParameterByName('sn');
        SOS.SalesOrder_Find(sn, $scope.UserData[0].Code).then(function (response) {
            console.log(response);
            $scope.data = response.data;
            $scope.updateclick();
            $scope.checksurvey();
        });


    }




    $scope.Att_file = "";
    $scope.Fileloading = true;
    //file upload 
    $scope.uploadFile = function () {
        $scope.Fileloading = false;
        var fd = new FormData();
        fd.append('file', $scope.myFile);
        fd.append('name', "1");
        var uploadUrl = US.Host + "FileUpload.php";
        $http.post(uploadUrl, fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined, 'Process-Data': false }
        })
         .success(function (data) {
             $scope.Fileloading = true;
             console.log(data);
             $scope.myFile = []
             $("#fileopen").val("");
             $scope.ATT = {
                 "U_FileName": data.FileName,
                 "U_FilePath": data.FilePath,
                 "U_id": data.id,
                 "U_Date": data.Date
             };
             if (data.Status == "true") {
                 $scope.data.ATTACHMENT.push(angular.copy($scope.ATT));
             }
             else {
                 alert(data.errMSG);
             }
         })
         .error(function () {
             $scope.Fileloading = true;
             alert("File Upload Failed");
         });

    };


    $scope.fileBurl = US.BURL;




} ]);
