App.controller('signin', ['$scope', '$rootScope', '$http', '$window', '$cookies', 'util_SERVICE',

function ($scope, $rootScope, $http, $window, $cookies,US) {
    // $scope.items = Data;
    $scope.userId = "";
    $scope.password = "";
    $cookies.put('MenuInfo', "");
    $cookies.put('UserData', "");
    $cookies.put('Islogin', "false");

    $scope.checklogin = function () {

        var data = {
            "WUSER": [
                        {
                            "uid": $scope.userId,
                            "pwd": $scope.password
                        }
                      ]
        }

        var config = {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        var parms = JSON.stringify(data);
        $http.post(US.url+"Login", "value=" + parms, config)
   .then(
       function (response) {
           // success callback
           console.log(response.data);
           if (response.data.VALIDATE[0].Status === "True") {
               $cookies.put('MenuInfo', JSON.stringify(response.data.MenuInfo));
               $cookies.put('UserData', JSON.stringify(response.data.UserData));
               $cookies.put('Islogin', "true");
               if( response.data.UserData[0].Code!='I001' && response.data.UserData[0].Code!='I002')
                   window.location = "Default.aspx";
                        else
                            window.location = "Default.aspx";
           }
            else
               alert(response.data.VALIDATE[0].Msg);
       },
       function (response) {
           // failure callback

       }
    );

    }



} ]);
