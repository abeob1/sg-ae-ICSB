App.service('SO_SERVICE', ['$http', 'util_SERVICE', function ($http, US) {



    this.url = US.url;
    this.config = US.config;



    //get all Equipment Type
    this.getetype = function () {
       
        var parms = "";
        var promise = $http.post(this.url + "Equipment_Type_Master", "value=" + parms, this.config)
   .success(function (response) {
       if (response.returnStatus == 1) {
           return response;
       } else {
           //alert('Not Connecting to server');
           return false;
       }
   });
        return promise;

    };


    //get all Survey_Criteria_Master
    this.getSurveyCriteria = function () {
       
        var parms = "";
        var promise = $http.post(this.url + "Survey_Criteria_Master", "value=" + parms, this.config)
   .success(function (response) {
       if (response.returnStatus == 1) {
           return response;
       } else {
           //alert('Not Connecting to server');
           return false;
       }
   });
        return promise;

    };

    
     //get all surveyor ID
    this.getSurveyorIDMaster = function () {

        var data = {
	"SUID": [{
		"uid": username
	}]
};
        var parms = encodeURIComponent(JSON.stringify(data));
        var promise = $http.post(this.url + "SurveyorIDMaster_full", "value=" + "", this.config)
   .success(function (response) {
       if (response.returnStatus == 1) {
           return response;
       } else {
           //alert('Not Connecting to server');
           return false;
       }
   });
        return promise;

    };

    //Find form name by link
    this.gotoSurveyFromByLink = function (val) {

        var data = {  "ODLN": [{  "U_SurvyNo": val }] }
        var parms = encodeURIComponent(JSON.stringify(data));
        var promise = $http.post(this.url + "SurveyTyep_Find", "value=" + parms, this.config)
   .success(function (response) {
       if (response.returnStatus == 1) {
           return response;
       } else {
           //alert('Not Connecting to server');
           return false;
       }
   });
        return promise;

    };


     //get surveyor ID by username
    this.getsurveyorID = function (username) {

        var data = {
	"SUID": [{
		"uid": username
	}]
};
        var parms = encodeURIComponent(JSON.stringify(data));
        var promise = $http.post(this.url + "SurveyorIDMaster", "value=" + parms, this.config)
   .success(function (response) {
       if (response.returnStatus == 1) {
           return response;
       } else {
           //alert('Not Connecting to server');
           return false;
       }
   });
        return promise;

    };





    //get all customer
    this.getcustomer = function (code, name,uid) {

        var data = {
            "OCRD": [{
                "U_Ccode": code,
                "U_Cname": name,
                "uid":uid
            }]
        };
        var parms = encodeURIComponent(JSON.stringify(data));
        var promise = $http.post(this.url + "CustomerMaster_CustomerOnly_SO", "value=" + parms, this.config)
   .success(function (response) {
       if (response.returnStatus == 1) {
           return response;
       } else {
           //alert('Not Connecting to server');
           return false;
       }
   });
        return promise;

    };

    this.PreviousRecord = function (val,userId) {
        var rdata = [];
        var data = {  "ORDR": [{ "U_UCode": userId,"U_OrderNo":val }]};

        var parms = encodeURIComponent(JSON.stringify(data));
        var promise = $http.post(this.url + "SalesOrder_PreviousRecord", "value=" + parms, this.config)
   .success(function (response) {
       if (response.returnStatus == 1) {
           return response;
       } else {
           //alert('Not Connecting to server');
           return false;
       }
   });
        return promise;
    };


    this.NextRecord = function (val,userId) {
      
        var rdata = [];
        var data = {  "ORDR": [{ "U_UCode": userId,"U_OrderNo":val }]};

        var parms = encodeURIComponent(JSON.stringify(data));
        var promise = $http.post(this.url + "SalesOrder_NextRecord", "value=" + parms, this.config)
   .success(function (response) {
       if (response.returnStatus == 1) {
           return response;
       } else {
           //alert('Not Connecting to server');
           return false;
       }
   });
        return promise;
    };


    this.LastRecord = function (val) {

        var rdata = [];
        var data = {  "ORDR": [{"U_UCode": val }] };

        var parms = encodeURIComponent(JSON.stringify(data));
        var promise = $http.post(this.url + "SalesOrder_LastRecord", "value=" + parms, this.config)
   .success(function (response) {
       if (response.returnStatus == 1) {
           return response;
       } else {
           //alert('Not Connecting to server');
           return false;
       }
   });
        return promise;
    };



    this.SalesOrder_Find = function (sn,UN) {

        var rdata = [];
        var data = {"ORDR": [{"U_UCode": UN,"U_OrderNo": sn}]}

        var parms = encodeURIComponent(JSON.stringify(data));
        var promise = $http.post(this.url + "SalesOrder_Find", "value=" + parms, this.config)
   .success(function (response) {
       if (response.returnStatus == 1) {
           return response;
       } else {
           //alert('Not Connecting to server');
           return false;
       }
   });
        return promise;
    };

    this.FirstRecord = function (val) {

        var rdata = [];
        var data = {  "ORDR": [{"U_UCode": val }] };

        var parms = encodeURIComponent(JSON.stringify(data));
        var promise = $http.post(this.url + "SalesOrder_FirstRecord", "value=" + parms, this.config)
   .success(function (response) {
       if (response.returnStatus == 1) {
           return response;
       } else {
           //alert('Not Connecting to server');
           return false;
       }
   });
        return promise;
    };

    this.FindMultiRecord = function (s,uid) {

        var rdata = [];
        var data = { "ORDR":[s] };
        //var data = {"ORDR":[{"U_UCode":uid,"U_OrderNo":parseInt(s.U_Qno)}]};



        var parms = encodeURIComponent(JSON.stringify(data));
        var promise = $http.post(this.url + "SalesOrder_FindRecord_List", "value=" + parms, this.config)
   .success(function (response) {
       if (response.returnStatus == 1) {
           return response;
       } else {
           //alert('Not Connecting to server');
           return false;
       }
   });
        return promise;
    };



    this.FindRecord = function (val) {

        var rdata = [];
        var data = { "SQTO": [{ "U_Qno": val}] };
       

        var parms = encodeURIComponent(JSON.stringify(data));
        var promise = $http.post(this.url + "SalesQuotation_FindRecord", "value=" + parms, this.config)
   .success(function (response) {
       if (response.returnStatus == 1) {
           return response;
       } else {
           //alert('Not Connecting to server');
           return false;
       }
   });
        return promise;
    };




    // SQ approval

   this.aproval = function (val) {

        var rdata = [];
        var data = { "APPROVAL": [{ "U_Qno": val}] };      
       

        var parms = encodeURIComponent(JSON.stringify(data));
        var promise = $http.post(this.url + "Sales_Quotation_Approval_Submit", "value=" + parms, this.config)
   .success(function (response) {
       if (response.returnStatus == 1) {
           return response;
       } else {
           //alert('Not Connecting to server');
           return false;
       }
   });
        return promise;
    };


      // checksurveychecksurvey 

   this.checksurvey = function (val) {

        var rdata = [];
        var data = {  "ORDR": [{  "U_OrderNo": val }] };      
       

        var parms = encodeURIComponent(JSON.stringify(data));
        var promise = $http.post(this.url + "SalesOrder_SurveyDetails", "value=" + parms, this.config)
   .success(function (response) {
       if (response.returnStatus == 1) {
           return response;
       } else {
           //alert('Not Connecting to server');
           return false;
       }
   });
        return promise;
    };



    // SQA approval by approver

   this.approver = function (code,status,remark,uname) {

        var rdata = [];
        var data = {"APPROVAL": [{"U_Qno": code ,"U_Status": status ,"U_Remarks": remark,"U_Uname": uname}]}
       

        var parms = encodeURIComponent(JSON.stringify(data));
        var promise = $http.post(this.url + "Sales_Quotation_Approver_Submit", "value=" + parms, this.config)
   .success(function (response) {
       if (response.returnStatus == 1) {
           return response;
       } else {
           //alert('Not Connecting to server');
           return false;
       }
   });
        return promise;
    };

     

    this.updateUserRol = function (s, userid) {
        var rdata = [];
        var data = {
            "requestType": "authorisation",
            "subRequestType": "updateRoleId",
            "systemId": US.systemId,
            "sessionId": US.gsid(),
            "authKey": US.authKey,
            "userId": US.userId,
            "roleName": US.roleName,
            "requestId": US.grequestId(),
            "updateRoleCategory": {
                "updatedUserId": parseInt(userid),
                "userRoleid": s,
                "purcategoryId": [1, 3],
                "defaultPurCategory": 3
            }
        };


        var promise = $http.get(url + JSON.stringify(data)).success(function (response) {
            if (US.eh(response)) {
                return response;
                //alert(response.status);
            } else {
                //alert('Not Connecting to server');
                return false;
            }
        });
        return promise;
    };


} ]);

