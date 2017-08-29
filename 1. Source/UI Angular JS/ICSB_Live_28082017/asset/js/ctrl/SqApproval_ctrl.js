App.controller('SqApproval_ctrl', ['$scope', '$rootScope', '$http', '$window', '$cookies', 'util_SERVICE', 'SQA_SERVICE', 'hotkeys',

function ($scope, $rootScope, $http, $window, $cookies, US, SQAS, hotkeys) {
    $scope.savelable = "Save";
    $scope.savebtn = true;
    $scope.findbtn = false;
    $scope.updatebtn = false;
    $scope.loading = false;
    $scope.whilefind = false;
    $scope.btndisable = true;
    $scope.FromCode = "";
    $scope.ToCode = "";
    $scope.CdateFrom = "";
    $scope.CdateTo = "";
    $scope.ApprovalSt = "";
    $scope.CreateBy = "";




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


    $scope.addressarray = [];
    US.geteApproval_Status().then(function (response) { $scope.Astatus = response.data.APPROVAL; console.log($scope.Astatus); });
    US.geteCreated_By().then(function (response) { $scope.CreatedBy = response.data.CREATEDBY; console.log($scope.CreatedBy); });


    $scope.getcustomer = function () {

        SQAS.getcustomer($scope.FromCode, "").then(function (response) {
            $rootScope.customer = response.data; console.log($rootScope.customer); $("#myModal").modal();

        });
    }


    $scope.getcustomerTo = function () {

        SQAS.getcustomer($scope.ToCode, "").then(function (response) {
            $rootScope.customerTO = response.data; console.log($rootScope.customerTO); $("#myModalTo").modal();

        });
    }


    $rootScope.setc1 = function (c) {

        $scope.FromCode = c;
    }

    $rootScope.setc2 = function (c) {

        $scope.ToCode = c;
    }


    //search and get data
    $scope.search = function () {

        SQAS.getdata($scope).then(function (response) {
            $scope.data = response.data; console.log($scope.data);

        });

    }





} ]);
