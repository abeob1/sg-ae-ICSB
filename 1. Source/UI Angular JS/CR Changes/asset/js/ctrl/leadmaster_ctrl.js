App.controller('leadmaster_ctrl', ['$scope', '$rootScope', '$http', '$window', '$cookies', 'hotkeys','util_SERVICE',

function ($scope, $rootScope, $http, $window, $cookies, hotkeys,US) {

    $scope.savelable = "Save";
    $scope.savebtn = false;
    $scope.code = true;
    $scope.data = { "LEADM": [{ "Code": "", "Name": "", "U_TelNo": "", "U_FaxNo": "", "U_MNo": "", "U_Email": "",
        "U_Addr1": "",
        "U_Addr2": "",
        "U_Addr3": "",
        "U_Addr4": "",
        "U_Addr5": "",
        "U_Addr6": ""
    }]
    };
    $rootScope.fulldata = '';
    $scope.update = false;
    $scope.findbtn = false;
    $scope.saveb = true;



    //adding shortcuts
    hotkeys.add({ combo: 'alt+left', description: '', callback: function () { $scope.LeadFirstRecord(); } });
    hotkeys.add({ combo: 'alt+right', description: '', callback: function () { $scope.LeadLastRecord(); } });
    hotkeys.add({ combo: 'ctrl+right', description: '', callback: function () { $scope.LeadNextRecord(); } });
    hotkeys.add({ combo: 'ctrl+left', description: '', callback: function () { $scope.LeadPreviousRecord(); } });
    hotkeys.add({ combo: 'space+f', description: '', callback: function () { reset(); $scope.findbtn = true; $scope.saveb = false; $scope.update = false; $scope.code = false; } });
    hotkeys.add({ combo: 'space+n', description: '', callback: function () { $scope.reset(); } });

    $scope.reset = function () {

        $scope.data.LEADM[0].Code = "";
        $scope.data.LEADM[0].Name = "";
        $scope.data.LEADM[0].U_TelNo = "";
        $scope.data.LEADM[0].U_FaxNo = "";
        $scope.data.LEADM[0].U_MNo = "";
        $scope.data.LEADM[0].U_Email = "";
        $scope.data.LEADM[0].U_Addr1 = "";
        $scope.data.LEADM[0].U_Addr2 = "";
        $scope.data.LEADM[0].U_Addr3 = "";
        $scope.data.LEADM[0].U_Addr4 = "";
        $scope.data.LEADM[0].U_Addr5 = "";
        $scope.data.LEADM[0].U_Addr6 = "";

        $scope.saveb = true;
        $scope.findbtn = false;
        $scope.update = false;
        $scope.code = true;



    }


    $scope.g_edit = function (i) {
        // alert(i);
        $scope.data.SQTOGEN[i].edit = true;
    }
    $scope.g_ok = function (i) {
        // alert(i);
        $scope.data.SQTOGEN[i].edit = false;
    }

    $scope.g_del = function (i) {
        //alert(i);
        $scope.data.SQTOGEN.splice(i, 1);
    }

    $scope.save = function () {

        if ($scope.data.LEADM[0].Name == "") {
            alert("Kindly Enter Name.");
        }
        else {

                $scope.savelable = "Loading..";
                $scope.savebtn = true;
                var config = {
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
                    }
                }

                var parms = JSON.stringify($scope.data);
                $http.post(US.url + "LeadAdd", "value=" + parms, config)
       .then(
           function (response) {
               // success callback
               console.log(response.data);
               if (response.data.VALIDATE[0].Status == "True") {
                   $scope.savelable = "Save";
                   $scope.savebtn = false;
                   $scope.reset();
                   alert(response.data.VALIDATE[0].Msg);
               }
               else {
                   $scope.savelable = "Save";
                   $scope.savebtn = false;
                   //$scope.reset();
                   alert(response.data.VALIDATE[0].Msg);
               }
           },
           function (response) {
               // failure callback

           }
        );
        }
    }


    //findrecord

    $scope.find = function () {
        $scope.savelable = "Loading..";
        $scope.savebtn = true;
        //$scope.input = { "LEADM": [{ "Code": "001", "Name": "", "U_TelNo": "", "U_FaxNo": "", "U_MNo": "", "U_Email": ""}] }


        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var parms = JSON.stringify($scope.data);
        $http.post(US.url + "LeadFindRecord", "value=" + parms, config)
   .then(
       function (response) {
           // success callback
           console.log(response.data);
           if (response.data.VALIDATE === undefined) {
               $scope.savelable = "Save";
               $scope.savebtn = false;
               $rootScope.fulldata = response.data;
               if ($rootScope.fulldata.LEADM.length > 1) {
                   //alert("more");
                   $("#myModal").modal()
               }
               else {
                   //alert("one");
                   $rootScope.findone(0);
               }
               //$scope.reset();
               //alert(response.data.VALIDATE[0].Msg);
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

    //get one record from multiple
    $rootScope.findone = function (i) {
        //alert(i);
        //console.log($rootScope.fulldata.LEADM[i].Code);
        $scope.data.LEADM[0].Code = $rootScope.fulldata.LEADM[i].Code;
        $scope.data.LEADM[0].Name = $rootScope.fulldata.LEADM[i].Name;
        $scope.data.LEADM[0].U_TelNo = $rootScope.fulldata.LEADM[i].U_TelNo;
        $scope.data.LEADM[0].U_FaxNo = $rootScope.fulldata.LEADM[i].U_FaxNo;
        $scope.data.LEADM[0].U_MNo = $rootScope.fulldata.LEADM[i].U_MNo;
        $scope.data.LEADM[0].U_Email = $rootScope.fulldata.LEADM[i].U_Email;
        $scope.data.LEADM[0].U_Addr1 = $rootScope.fulldata.LEADM[i].U_Addr1;
        $scope.data.LEADM[0].U_Addr2 = $rootScope.fulldata.LEADM[i].U_Addr2;
        $scope.data.LEADM[0].U_Addr3 = $rootScope.fulldata.LEADM[i].U_Addr3;
        $scope.data.LEADM[0].U_Addr4 = $rootScope.fulldata.LEADM[i].U_Addr4;
        $scope.data.LEADM[0].U_Addr5 = $rootScope.fulldata.LEADM[i].U_Addr5;
        $scope.data.LEADM[0].U_Addr6 = $rootScope.fulldata.LEADM[i].U_Addr6;

        $scope.saveb = false;
        $scope.findbtn = false;
        $scope.update = true;
        $scope.code = true;

    }





    //LeadLastRecord
    $scope.LeadLastRecord = function () {
        //$scope.savelable = "Loading..";
        $scope.savebtn = true;
        //$scope.input = { "LEADM": [{ "Code": "001", "Name": "", "U_TelNo": "", "U_FaxNo": "", "U_MNo": "", "U_Email": ""}] }


        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var parms = JSON.stringify($scope.data);
        $http.post(US.url + "LeadLastRecord", "value=" + "", config)
   .then(
       function (response) {
           // success callback
           console.log(response.data);
           if (response.data.VALIDATE === undefined) {
               $scope.savelable = "Save";
               $scope.savebtn = false;
               $rootScope.fulldata = response.data;
               if ($rootScope.fulldata.LEADM.length > 1) {
                   //alert("more");
                   $("#myModal").modal()
               }
               else {
                   //alert("one");
                   $rootScope.findone(0);
               }
               //$scope.reset();
               //alert(response.data.VALIDATE[0].Msg);
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



    //LeadFirstRecord
    $scope.LeadFirstRecord = function () {
        //$scope.savelable = "Loading..";
        $scope.savebtn = true;
        $scope.code = true;
        //$scope.input = { "LEADM": [{ "Code": "001", "Name": "", "U_TelNo": "", "U_FaxNo": "", "U_MNo": "", "U_Email": ""}] }


        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var parms = JSON.stringify($scope.data);
        $http.post(US.url + "LeadFirstRecord", "value=" + "", config)
   .then(
       function (response) {
           // success callback
           console.log(response.data);
           if (response.data.VALIDATE === undefined) {
               $scope.savelable = "Save";
               $scope.savebtn = false;
               $rootScope.fulldata = response.data;
               if ($rootScope.fulldata.LEADM.length > 1) {
                   //alert("more");
                   $("#myModal").modal()
               }
               else {
                   //alert("one");
                   $rootScope.findone(0);
               }
               //$scope.reset();
               //alert(response.data.VALIDATE[0].Msg);
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



    //LeadNextRecord
    $scope.LeadNextRecord = function () {
        //$scope.savelable = "Loading..";
        $scope.savebtn = true;
        $scope.code = true;
        //$scope.input = { "LEADM": [{ "Code": "001", "Name": "", "U_TelNo": "", "U_FaxNo": "", "U_MNo": "", "U_Email": ""}] }


        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var parms = JSON.stringify({ "LEADM": [{ "Code": $scope.data.LEADM[0].Code}] });
        $http.post(US.url + "LeadNextRecord", "value=" + parms, config)
   .then(
       function (response) {
           // success callback
           console.log(response.data);
           if (response.data.VALIDATE === undefined) {
               $scope.savelable = "Save";
               $scope.savebtn = false;
               $rootScope.fulldata = response.data;
               if ($rootScope.fulldata.LEADM.length > 1) {
                   //alert("more");
                   $("#myModal").modal()
               }
               else {
                   //alert("one");
                   $rootScope.findone(0);
               }
               //$scope.reset();
               //alert(response.data.VALIDATE[0].Msg);
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


    //LeadPreviousRecord
    $scope.LeadPreviousRecord = function () {
        //$scope.savelable = "Loading..";
        $scope.code = true;
        $scope.savebtn = true;
        //$scope.input = { "LEADM": [{ "Code": "001", "Name": "", "U_TelNo": "", "U_FaxNo": "", "U_MNo": "", "U_Email": ""}] }


        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var parms = JSON.stringify({ "LEADM": [{ "Code": $scope.data.LEADM[0].Code}] });
        $http.post(US.url + "LeadPreviousRecord", "value=" + parms, config)
   .then(
       function (response) {
           // success callback
           console.log(response.data);
           if (response.data.VALIDATE === undefined) {
               $scope.savelable = "Save";
               $scope.savebtn = false;
               $rootScope.fulldata = response.data;
               if ($rootScope.fulldata.LEADM.length > 1) {
                   //alert("more");
                   $("#myModal").modal()
               }
               else {
                   //alert("one");
                   $rootScope.findone(0);
               }
               //$scope.reset();
               //alert(response.data.VALIDATE[0].Msg);
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



    //updaterecord
    $scope.updaterecord = function () {
        $scope.savelable = "Loading..";
        $scope.savebtn = true;
        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var parms = JSON.stringify($scope.data);
        $http.post(US.url + "LeadUpdate", "value=" + parms, config)
   .then(
       function (response) {
           // success callback
           console.log(response.data);
           if (response.data.VALIDATE[0].Status == "True") {
               $scope.savelable = "Save";
               $scope.savebtn = false;
               //$scope.reset();
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


} ]);
