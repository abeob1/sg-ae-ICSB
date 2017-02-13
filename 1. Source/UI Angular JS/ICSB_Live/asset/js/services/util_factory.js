App.service('util_SERVICE', ['$http', '$window', '$cookieStore', '$rootScope', function ($http, $window, $cookie, $rootScope) {
    var urlsd = window.location.href.split("/");
    this.url = "http://119.73.138.58:82/ICSB.asmx/";
    this.BURL = "http://119.73.138.58:84/TempFiles/";
    this.Host = "http://119.73.138.58:84/";
    this.config = {
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
        }
    }

    this.systemId = "4";
    this.authKey = "adfs3sdaczxcsdfw34";
    this.userId = "4";
    this.roleName = "4";
    this.grequestId = function () {
        return "1111";
    }

    this.msg;
    this.alerts = [];
    this.catstatus = function () {
        return $cookie.get('catstatus');
    }

    this.setmsg = function (d) {
        this.msg = d;
    }



    this.addAlert = function (type, msgs) {
        var length = this.alerts.push({ "type": type, "msg": msgs });
        //$rootScope.fadAlert(length-1);

        //console.log(this.alerts);
        //$rootScope.$broadcast("hi");

    };

    //set or change Category list
    this.setCategorylist = function (data) {
        console.log(data);

        $cookie.put('categoryList', data);
        return true;

    };

    //set or change Category
    this.setCategory = function (d) {
        $cookie.put('categoryID', d);
        $cookie.put('catstatus', true);
    };

    ////set or change Category
    //this.setItemCategoryIDs = function (d) {
    //    $cookie.put('itemCategoryIDs', d);
    //};

    ////to get purchace category
    //this.getItemCategoryIDs = function () {
    //    return $cookie.get('itemCategoryIDs');
    //};

    //to get purchace category
    this.getCategory = function () {
        return $cookie.get('categoryID');
    };

    //to get purchace category
    this.getEmail = function () {
        return $cookie.get('email');
    };

    this.setDefaultPurCategory = function (cid) {
        var data = this.gcList();
        $cookie.put('catstatus', false);
        for (var i = 0; i < data.length; i++) {
            if (data[i].categoryId == cid) {
                $cookie.put('defaultPurCategory', data[i].itemCategoryId);
                this.setCategory(data[i].itemCategoryId);
            }
        }
        console.log($cookie.get('defaultPurCategory'));
        return true;
    }

    //get default purcat
    this.gdefaultPurCategory = function () {
        return $cookie.get('defaultPurCategory');
    }


    //get all GST
    this.getGST = function () {

        var parms = "";
        var promise = $http.post(this.url + "GSTMaster_OutPut", "value=" + parms, this.config)
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


    //get all Country
    this.getUOM = function () {

        var parms = "";
        var promise = $http.post(this.url + "UOMMaster", "value=" + parms, this.config)
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

    //get all country
    this.getCountryMaster = function () {

        var parms = "";
        var promise = $http.post(this.url + "CountryMaster_full", "value=" + parms, this.config)
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

    //get all IMG Master
    this.getIMGMaster = function () {

        var parms = "";
        var promise = $http.post(this.url + "IMO_Master", "value=" + parms, this.config)
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


    //get all City
    this.getCityMaster = function () {

        var parms = "";
        var promise = $http.post(this.url + "CityMaster_full", "value=" + parms, this.config)
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

    //get all Location
    this.getLocationMaster = function () {

        var parms = "";
        var promise = $http.post(this.url + "LocationMaster_full", "value=" + parms, this.config)
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

    //get all SurveyTypeMaster
    this.getstype = function () {

        var parms = "";
        var promise = $http.post(this.url + "SurveyTypeMaster", "value=" + parms, this.config)
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


    //get all curency
    this.getcurency = function () {

        var parms = "";
        var promise = $http.post(this.url + "CurrencyMaster", "value=" + parms, this.config)
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


    //get all egroup
    this.getegroup = function () {

        var parms = "";
        var promise = $http.post(this.url + "EquipmentGroupMaster", "value=" + parms, this.config)
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


    //get all Charge Type
    this.getChargeType = function () {

        var parms = "";
        var promise = $http.post(this.url + "ChargeTypeMaster", "value=" + parms, this.config)
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

    //get city
    this.getcity = function (con) {
        console.log(con);
        var data = {
            "COUNTRY": [{
                "U_Country": con
            }]
        };
        var parms = encodeURIComponent(JSON.stringify(data));
        var promise = $http.post(this.url + "CityMaster", "value=" + parms, this.config)
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

    //get location
    this.getlocation = function (con) {
        console.log(con);
        var data = {
            "CITY": [{
                "U_City": con
            }]
        };
        var parms = encodeURIComponent(JSON.stringify(data));
        var promise = $http.post(this.url + "LocationMaster", "value=" + parms, this.config)
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


    //get Contry
    this.getcountry = function (conti) {
        console.log(conti);
        var data = {
            "CONT": [{
                "U_Conti": conti
            }]
        };
        var parms = encodeURIComponent(JSON.stringify(data));
        var promise = $http.post(this.url + "CountryMaster", "value=" + parms, this.config)
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

    //get all avilable Continent
    this.getContinent = function () {
        var rdata = [];
        var data = {
            "requestType": "authorisation",
            "subRequestType": "getCategorydata",
            "systemId": this.systemId,
            "authKey": "adfs3sdaczxcsdfw34",
            "sessionId": this.gsid()
        };
        //var parms = encodeURIComponent(JSON.stringify(data));
        var parms = "";
        var promise = $http.post(this.url + "ContinentMaster", "value=" + parms, this.config)
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



    //get all CREATEDBY
    this.geteCreated_By = function () {

        var parms = "";
        var promise = $http.post(this.url + "Created_By", "value=" + parms, this.config)
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


    //get all Approval_Status_Master
    this.geteApproval_Status = function () {

        var parms = "";
        var promise = $http.post(this.url + "Approval_Status_Master", "value=" + parms, this.config)
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





    this.errorsomething = function (d) {
        //switch (d.error.code) {
        //    case 16: document.write("Not Saturday"); break;
        //    case 20: document.write("Not Sunday"); break;
        //    default: this.addAlert("danger", d.error.Message); break;
        //}
        if (d.error.code > 0)
        // alertify.alert(d.error.detatiledMessage);
            this.addAlert("danger", d.error.Message);

    }

    //eh = error handling
    this.eh = function (d) {
        /*console.log(d);
        if (d.returnStatus == 1 || d.error == null || d.error.code == 0) {
        return true;
        }
        else
        this.errorsomething(d);*/

        if (d.code > 0)
            this.addAlert("danger", d.message);
    }
    this.getserverURL = function () {
        return url;
    };

    this.gsid = function () {
        return $cookie.get('sessionId');
    };

    this.checkLogin_old = function () {
        //debugger;

        var urlsd = window.location.href.split("/");
        if ($cookie.get('usermenu') == "" || $cookie.get('usermenu') == undefined || $cookie.get('sessionId') == undefined || $cookie.get('sessionId') == "")
            if (urlsd[4] == "purchase_dev")
                window.location = "http://ishademo.ddns.net/po/purchase_dev/index.html"
            else
                window.location = "http://ishademo.ddns.net/purchase/index.html"

        };

        this.checkmenu = function (id) {
            if ($cookie.get('usermenu') == "" || $cookie.get('usermenu') == undefined)
                return false;

            //console.log($cookie.get('usermenu'));

            var key = JSON.parse($cookie.get('usermenu'));
            var i = null;
            var ret = false;
            for (i = 0; key.length > i; i += 1) {
                if (key[i].menuId == id) {
                    ret = true;
                }
            };
            return ret;
        };

        this.checkLogin = function (email, id) {
            //debugger;

            var rdata = [];
            var data = {
                "requestType": "authorisation",
                "subRequestType": "getUserInfo",
                "systemId": this.systemId,
                "authKey": "adfs3sdaczxcsdfw34",
                // "userId": this.getEmail(),
                "userId": email,
                "parameter": {
                    "clientCode": id
                }
            };
            console.log(data);
            var promise = $http.get(url + JSON.stringify(data)).success(function (response) {
                if (response.returnStatus == 1) {
                    return response;
                } else {
                    //alert('Not Connecting to server');
                    return false;
                }
            });
            return promise;
        };


        this.getUOMMaster = function () {

            var rdata = [{}];
            var data = {
                "requestType": "indent",
                "sessionId": this.gsid(),
                "subRequestType": "GetUOM",
                "systemId": this.systemId,
                "authKey": "adfs3sdaczxcsdfw34",
                "userId": "senthil@gmail.com"
            };
            var promise = $http.get(url + JSON.stringify(data)).success(function (response) {
                if (response.returnStatus == '1') {
                    rdata.push(response);
                } else {
                    alert('Not Connecting to server');
                    return false;
                }
            });
            return promise;
        };

        this.isUndefinedOrNull = function (val) {
            return angular.isUndefined(val) || val === null
        }






    } ]);


App.factory('focus', function ($timeout, $window) {
    return function (id) {
        // timeout makes sure that it is invoked after any other event has been triggered.
        // e.g. click events that need to run before the focus or
        // inputs elements that are in a disabled state but are enabled when those events
        // are triggered.
        $timeout(function () {
            var element = $window.document.getElementById(id);
            if (element)
                element.focus();
        });
    };
});

App.controller('MenuCtrl', ['$scope', '$rootScope', '$window', 'util_SERVICE', function ($scope, $rootScope, $window, US) {
    $scope.redirectUrl = "#";
    $("#apDiv3").draggable();

    //$scope.prfHasAnyChangesMade = false
    $scope.redirectUrls = {
        prf: 'prf.html',
        ia: 'IndentApproval.html',
        qEntry: 'quoteEntry.html',
        qApproval: 'quoteApproval.html',
        poGeneration: 'PoGeneration.html',
        poList: 'PoList.html',
        mrn: 'mrn.html',
        dn: 'dn.html',
        returnNote: 'return.html',
        returnStatus: 'return_status.html',
        userManagement: 'user_management.html',
        itemCategory: 'item_category.html',
        mainCategory: 'main_category.html',
        itemMaster: 'item_master.html',
        arrtributeMaster: 'attribute_master.html',
        attributeValue: 'attribute_value.html',
        attributeMap: 'attribute_map.html',
        cashList: 'cashindent_list.html',
        supplierMaster: 'supplier_master.html',
        companyMaster: 'company_master.html',
        departmentMaster: 'department_master.html',
        uomMaster: 'uom_master.html',
        indentList: 'IndentList.html',
        newItemMaster: 'New_item_master1.html'
    };

    US.checkLogin();

    $scope.checkmenu = function (data) {
        return US.checkmenu(data);
    }

    $scope.redirectPage = function (page, url, redirectPage) {
        if (page == 'indent') {
            $scope.pRFHasAnyChanges();
            var prfHasAnyChanges = $rootScope.prfHasAnyChangesMade;

            if (prfHasAnyChanges) {
                $scope.changeUrls(redirectPage, '#');
                $scope.confirmPopup = true;

                //if (confirm('Some changes made. Do you want to discard your changes ?')) {
                //    changeUrls(redirectPage, '#');
                //}

                //else {
                //    $scope.changeUrls(redirectPage, '#');
                //    $window.open(url, '_blank');
                //}

                $scope.redirectUrl = url;
                //$scope.openn();
            }
        }

        setTimeout($scope.changeUrls, 1000, redirectPage, url)
    };

    $scope.changeUrls = function (page, url) {
        switch (page) {
            case "IndentApproval":
                $scope.redirectUrls.ia = url;
                break;
            case "quoteEntry":
                $scope.redirectUrls.qEntry = url;
                break;
            case "quoteApproval":
                $scope.redirectUrls.qApproval = url;
                break;
            case "POGeneration":
                $scope.redirectUrls.poGeneration = url;
                break;
            case "POList":
                $scope.redirectUrls.poList = url;
                break;
            case "MRN":
                $scope.redirectUrls.mrn = url;
                break;
            case "DN":
                $scope.redirectUrls.dn = url;
                break;
            case "return":
                $scope.redirectUrls.returnNote = url;
                break;
            case "return_status":
                $scope.redirectUrls.returnStatus = url;
                break;
            case "user_management":
                $scope.redirectUrls.userManagement = url;
                break;
            case "item_category":
                $scope.redirectUrls.itemCategory = url;
                break;
            case "main_category":
                $scope.redirectUrls.mainCategory = url;
                break;
            case "item_master":
                $scope.redirectUrls.itemMaster = url;
                break;
            case "attribute_master":
                $scope.redirectUrls.arrtributeMaster = url;
                break;
            case "attribute_value":
                $scope.redirectUrls.attributeValue = url;
                break;
            case "attribute_map":
                $scope.redirectUrls.attributeMap = url;
                break;
            case "":
                $scope.redirectUrls.cashList = url;
                break;
            case "supplier_master":
                $scope.redirectUrls.supplierMaster = url;
                break;
            case "company_master":
                $scope.redirectUrls.companyMaster = url;
                break;
            case "department_master":
                $scope.redirectUrls.departmentMaster = url;
                break;
            case "uom_master":
                $scope.redirectUrls.uomMaster = url;
                break;
            case "IndentList":
                $scope.redirectUrls.indentList = url;
                break;
            case "new_item_master":
                $scope.redirectUrls.newItemMaster = url;
                break;
            default:
                break;
        }
    };

    $scope.pRFHasAnyChanges = function () {
        $rootScope.$emit("pRFHasAnyChanges", {});
    }

    $scope.openn = function () {
        $rootScope.$emit("openn", {});
    }

    $scope.ok = function () {
        $scope.confirmPopup = false;
        $window.location.href = $scope.redirectUrl;
    };

    $scope.openInNewTab = function () {
        $scope.confirmPopup = false;
        $window.open($scope.redirectUrl, '_blank');
    };

    $scope.cancel = function () {
        $scope.confirmPopup = false;
    };
}]);

App.controller('toprightCtrl', ["$rootScope", "$scope", "util_SERVICE", '$cookieStore', function ($rs, $scope, US, $cookie) {




    $scope.getcategoryName;
    $scope.email = $cookie.get('email');
    $scope.name = $cookie.get('name');
    $scope.allCategory = US.gcList();
    console.log('all Category ', $scope.allCategory);

    if ($cookie.get('Imgurl') == 'default')
        $scope.profileimg = "../images/27.jpg";
    else
        $scope.profileimg = $cookie.get('Imgurl');
    //$scope.categoryName = US.getcategoryName($scope.CategoryId);
    //console.log(US.gdefaultPurCategory());

    if (US.catstatus()) {
        $scope.CategoryId = US.getCategory();
    }
    else if (US.gdefaultPurCategory()) {
        $scope.CategoryId = US.gdefaultPurCategory();
    }
    else
        $scope.CategoryId = "0";

    $scope.previousCategory = "0";

    $scope.setCategory = function (cid) {
        if (confirm('Are you sure you want to Change Category ?')) {
            US.setCategory(cid);
            location.reload();
        }

        else {
            $scope.CategoryId = US.getCategory();
            return false;
        }
    }
}]);

App.controller('AlertDemoCtrl', ["$rootScope", "util_SERVICE", function ($rootScope, US) {

    $rootScope.alerts = US.alerts;
    $rootScope.profileimg = US.dp;

    $rootScope.addAlert = function (type, msgs) {

        $rootScope.alerts.push({ "type": type, "msg": msgs });


    };

    $rootScope.closeAlert = function (index) {

        $rootScope.alerts.splice(index, 1);
    };
    $rootScope.fadAlert = function (i) {

        setTimeout(function () {

            $rootScope.alerts.splice(i, 1);
        }, 400);


    };



    $rootScope.alertchange = function () {
        console.log(US.$rootScope.alerts);
    }

    //$rootScope.$on("hi", $rootScope.alertchange())
}]);
