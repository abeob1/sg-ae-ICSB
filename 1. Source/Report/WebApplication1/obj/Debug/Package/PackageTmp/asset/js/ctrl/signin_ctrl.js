App.controller('signin', ['$scope', '$rootScope', '$http', '$window',

function ($scope, $rootScope, $http, $window) {
    // $scope.items = Data;
    $scope.userId = "sss";
    $scope.password = "sdddd";

    $scope.checklogin = function () {

        var data = {
            "WUSER": [
                        {
                            "uid": "W001",
                            "pwd": "abcd"
                        }
                      ]
        }

        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }


        $http.post("http://119.73.138.58:82/ICSB.asmx/Login", data, config)
   .then(
       function (response) {
           // success callback
           console.log(response);
       },
       function (response) {
           // failure callback
       }
    );

    }



} ]);
