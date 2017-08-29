App.controller('main', ['$scope', '$rootScope', '$http', '$window', '$cookies', 'util_SERVICE',

function ($scope, $rootScope, $http, $window, $cookies,US) {
    // $scope.items = Data;
    $scope.userId = "";
    $scope.password = "";

    if ($cookies.get('Islogin') == "true") {
        // alert("ss");
    }
    else {
        window.location = "login.aspx";
    }
    $scope.MenuInfo = angular.fromJson($cookies.get('MenuInfo'));
    $scope.UserData = angular.fromJson($cookies.get('UserData'));


    $scope.username = "Admin";
    $scope.user_role = "Administrator";

    $scope.isMenuActive = function (MID) {
        var rval = false;
        for (var i = 0; i < $scope.MenuInfo.length; i++) {
            
            if ($scope.MenuInfo[i].U_MenuID == MID)
                rval = true;

        }
        return rval;

    }


} ]);



function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}