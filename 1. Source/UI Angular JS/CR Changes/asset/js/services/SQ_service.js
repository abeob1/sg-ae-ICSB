App.service('SQ_SERVICE', ['$http', 'util_SERVICE', function ($http, US) {



    this.url = US.url;
    this.config = US.config;


    //get all SurveyTypeMaster
    this.getcustomer = function (code, name) {

        var data = {
            "OCRD": [{
                "U_Ccode": code,
                "U_Cname": name
            }]
        };
        var parms = encodeURIComponent(JSON.stringify(data));
        var promise = $http.post(this.url + "CustomerMaster", "value=" + parms, this.config)
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

    this.PreviousRecord = function (val) {
        var rdata = [];
        var data = { "SQTO": [{ "U_Qno": val}] };

        var parms = encodeURIComponent(JSON.stringify(data));
        var promise = $http.post(this.url + "SalesQuotation_PreviousRecord", "value=" + parms, this.config)
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


    this.NextRecord = function (val) {
      
        var rdata = [];
        var data = { "SQTO": [{ "U_Qno": val}] };

        var parms = encodeURIComponent(JSON.stringify(data));
        var promise = $http.post(this.url + "SalesQuotation_NextRecord", "value=" + parms, this.config)
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
        var data = { "SQTO": [{ "U_Qno": val}] };

        var parms = encodeURIComponent(JSON.stringify(data));
        var promise = $http.post(this.url + "SalesQuotation_LastRecord", "value=" + "", this.config)
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
        var data = { "SQTO": [{ "U_Qno": val}] };

        var parms = encodeURIComponent(JSON.stringify(data));
        var promise = $http.post(this.url + "SalesQuotation_FirstRecord", "value=" + "", this.config)
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

    this.FindMultiRecord = function (s) {

        var rdata = [];
        var data = { "SQTO": [ { "U_Qno": s.U_Qno, "U_Cdate": s.U_Cdate, "U_Ccode": s.U_Ccode, "U_Cname": s.U_Cname, "U_Pcode": s.U_Pcode, } ] };
        


        var parms = encodeURIComponent(JSON.stringify(data));
        var promise = $http.post(this.url + "SalesQuotation_FindRecord_List", "value=" + parms, this.config)
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

