App.service('css1_NewFormat_service', ['$http', 'util_SERVICE', function ($http, US) {



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
    this.getcustomer = function (code, name) {

        var data = {
            "OCRD": [{
                "U_Ccode": code,
                "U_Cname": name
            }]
        };
        var parms = encodeURIComponent(JSON.stringify(data));
        var promise = $http.post(this.url + "CustomerMaster_CustomerOnly", "value=" + parms, this.config)
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



    //get surveyor Agent
    this.getsurveyAgent = function (uid) {

        var data = {
            "SUID": [{
                "uid": uid
            }]
        };
        var parms = encodeURIComponent(JSON.stringify(data));
        var promise = $http.post(this.url + "Surveyor_Ex_Agent", "value=" + parms, this.config)
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



} ]);

