
App.factory('Data', function () {

    var items = [{ "itemId": 1, "itemName": "Mobile%20Phone", "itemCategory": 2, "uom": "PCS", "qty": 15, "itemSpec": [{ "attId": 1, "attName": "Brand", "attvalue": "red%20me", "$$hashKey": "015" }, { "attId": 2, "attName": "Color", "attvalue": "red", "$$hashKey": "016" }, { "attId": 4, "attName": "Variety", "attvalue": "", "$$hashKey": "017" }], "remark": "", "status": "Purchase", "$$hashKey": "01B" }, { "index": 1, "itemId": 1, "itemName": "Mobile%20Phone", "itemCategory": 2, "uom": "PCS", "qty": 14, "itemSpec": [{ "attId": 1, "attName": "Brand", "attvalue": "th", "$$hashKey": "013" }, { "attId": 2, "attName": "Color", "attvalue": "", "$$hashKey": "014" }, { "attId": 4, "attName": "Variety", "attvalue": "", "$$hashKey": "015" }], "remark": "", "status": "Purchase", "$$hashKey": "019" }, { "index": 1, "itemId": 4, "itemName": "Apple", "itemCategory": 3, "uom": "PCS", "qty": 78, "itemSpec": [{ "attId": 3, "attName": "Size", "attvalue": "", "$$hashKey": "01T" }, { "attId": 4, "attName": "Variety", "attvalue": "", "$$hashKey": "01U" }, { "attId": 5, "attName": "Weight", "attvalue": "", "$$hashKey": "01V" }], "remark": "", "status": "Purchase", "$$hashKey": "01Z" }, { "index": 1, "itemId": 6, "itemName": "Banana", "itemCategory": 4, "uom": "PCS", "qty": 47, "itemSpec": [{ "attId": 3, "attName": "Size", "attvalue": "", "$$hashKey": "02G" }, { "attId": 4, "attName": "Variety", "attvalue": "", "$$hashKey": "02H" }], "remark": "", "status": "Purchase", "$$hashKey": "02K" }, { "index": 1, "itemId": 12, "itemName": "Dates", "itemCategory": 3, "uom": "KGS", "qty": 43, "itemSpec": [{ "attId": 3, "attName": "Size", "attvalue": "", "$$hashKey": "030" }, { "attId": 4, "attName": "Variety", "attvalue": "", "$$hashKey": "031" }, { "attId": 5, "attName": "Weight", "attvalue": "", "$$hashKey": "032" }], "remark": "", "status": "Purchase", "$$hashKey": "036" }, { "index": 1, "itemId": 12, "itemName": "Dates", "itemCategory": 3, "uom": "KGS", "qty": 23, "itemSpec": [{ "attId": 3, "attName": "Size", "attvalue": "", "$$hashKey": "03E" }, { "attId": 4, "attName": "Variety", "attvalue": "", "$$hashKey": "03F" }, { "attId": 5, "attName": "Weight", "attvalue": "", "$$hashKey": "03G" }], "remark": "", "status": "Purchase", "$$hashKey": "03K" }, { "index": 1, "itemId": 6, "itemName": "Banana", "itemCategory": 4, "uom": "PCS", "qty": 78, "itemSpec": [{ "attId": 3, "attName": "Size", "attvalue": "23", "$$hashKey": "041" }, { "attId": 4, "attName": "Variety", "attvalue": "", "$$hashKey": "042" }], "remark": "", "status": "Purchase", "$$hashKey": "045" }, { "index": 1, "itemId": 9, "itemName": "Cabbage", "itemCategory": 4, "uom": "KGS", "qty": 45, "itemSpec": [{ "attId": 3, "attName": "Size", "attvalue": "34", "$$hashKey": "04I" }, { "attId": 4, "attName": "Variety", "attvalue": "", "$$hashKey": "04J" }], "remark": "", "status": "Purchase", "$$hashKey": "04M" }, { "index": 1, "itemId": 1, "itemName": "Mobile%20Phone", "itemCategory": 2, "uom": "PCS", "qty": 78, "itemSpec": [{ "attId": 1, "attName": "Brand", "attvalue": "sony", "$$hashKey": "04Z" }, { "attId": 2, "attName": "Color", "attvalue": "", "$$hashKey": "050" }, { "attId": 4, "attName": "Variety", "attvalue": "", "$$hashKey": "051" }], "remark": "", "status": "Purchase", "$$hashKey": "055" }, { "index": 1, "itemId": 1, "itemName": "Mobile%20Phone", "itemCategory": 2, "uom": "PCS", "qty": 74, "itemSpec": [{ "attId": 1, "attName": "Brand", "attvalue": "eee", "$$hashKey": "05P" }, { "attId": 2, "attName": "Color", "attvalue": "", "$$hashKey": "05Q" }, { "attId": 4, "attName": "Variety", "attvalue": "", "$$hashKey": "05R" }], "remark": "", "status": "Purchase", "$$hashKey": "05V" }];
    return items;
});

App.directive('datepickerPopup', function () {
    return {
        restrict: 'EAC',
        require: 'ngModel',
        link: function (scope, element, attr, controller) {
            //remove the default formatter from the input directive to prevent conflict
            controller.$formatters.shift();
        }
    }
});

//App.directive("customSort", function () {
//    return {
//        restrict: 'A',
//        transclude: true,
//        scope: {
//            order: '=',
//            sort: '='
//        },
//        template:
//          ' <a ng-click="sort_by(order)" style="color: #555555;">' +
//          '    <span ng-transclude></span>' +
//          '    <i ng-class="selectedCls(order)"></i>' +
//          '</a>',
//        link: function (scope) {

//            // change sorting order
//            scope.sort_by = function (newSortingOrder) {
//                var sort = scope.sort;

//                if (sort.sortingOrder == newSortingOrder) {
//                    sort.reverse = !sort.reverse;
//                }

//                sort.sortingOrder = newSortingOrder;
//            };

//            scope.selectedCls = function (column) {
//                if (column == scope.sort.sortingOrder) {
//                    return ('icon-chevron-' + ((scope.sort.reverse) ? 'down' : 'up'));
//                }
//                else {
//                    return 'icon-sort'
//                }
//            };
//        }// end link
//    }
//});

App.service('PRF_SERVICE', ['$http', '$window', 'util_SERVICE', function ($http, $window, US) {
    var baseRESTUrl = US.baseRESTAPI;

    var url = US.url;
    var Puturl = US.Puturl;

    this.testput = function () {
        var headersss = {
            'Access-Control-Allow-Methods': 'POST, GET, OPTIONS, PUT',
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        };


        var datas = { "name": "John", "surname": "Doe" }
        var action = $http({
            method: 'PUT',
            url: Puturl,
            headers: headersss,
            withCredentials: true,
            data: datas
        }).success(function (response) { alert("Thomas"); });
    }

    this.getCompany = function () {
        var email = US.getEmail()
        console.log(email);

        //var data = {
        //    "requestType": "indent",
        //    "sessionId": US.gsid(),
        //    "subRequestType": "GetCompanyInfo",
        //    "systemId": US.systemId,
        //    "authKey": "adfs3sdaczxcsdfw34",
        //    "userId": email
        //};

        // + JSON.stringify(data)

        var promise = $http.get(baseRESTUrl + '/indent/companyInfo').success(function (response) {
            if (US.eh(response))
                return response;
            else
                return false;
        });

        return promise;
    };

    this.getDepartment = function (activeStatus) {
        //var email = US.getEmail();

        //var data = {
        //    "requestType": "indent",
        //    "sessionId": US.gsid(),
        //    "subRequestType": "GetDepartmentInfo",
        //    "systemId": US.systemId,
        //    "authKey": "adfs3sdaczxcsdfw34",
        //    "userId": email,
        //    "roleName": US.roleName,
        //    "requestId": US.grequestId(),
        //    "parameter":
        //        {
        //            "activeRecords": activeStatus
        //        }
        //};

        //baseRESTUrl
        //+ JSON.stringify(data)

        var promise = $http.get(baseRESTUrl + '/indent/departmentInfo/' + activeStatus).success(function (response) {
            //if (US.eh(response)) {
            //    return response;
            //} else {
            //    return false;
            //}

            return response;
        });

        return promise;
    };

    //get all IndentStatus
    this.getIndentStatus = function () {
        //var data = {
        //    "requestType": "indent",
        //    "sessionId": US.gsid(),
        //    "subRequestType": "getIndentStatus",
        //    "systemId": 4,
        //    "authKey": "adfs3sdaczxcsdfw34",
        //    "userId": US.getEmail()
        //};

        //JSON.stringify(data)
        var promise = $http.get(baseRESTUrl + '/indent/indentStatus').success(function (response) {
            return response;
            //if (US.eh(response)) {
            //    return response;
            //} else {
            //    return false;
            //}
        });

        return promise;
    };

    //save quick
    this.savequick = function (s) {
        var rdata = [];
        var data = {
            "requestType": "quickAdd",
            "sessionId": US.gsid(),
            "subRequestType": "saveItemandSpec",
            "systemId": US.systemId,
            "authKey": "adfs3sdaczxcsdfw34",
            "userId": US.userId,
            "itemDetails": {
                "itemId": s.itemId,
                "itemName": s.newItemName,
                "itemShortName": s.itemShortName,
                "itemCategoryId": s.itemCategoryId,
                "defaultUOM": s.DUOM,
                "alternateUOM": s.AlternateuomIds,
                "status": s.inactive,
                "specDetails": this.getSpecCombo(s.displayCombination.attributeList)
            }
        };
        var parameter = {
            method: 'PUT',
            url: Puturl,
            data: data,
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        }

        var promise = $http(parameter).success(function (response, status, headers, config) {

            console.log('Saved Item successfully');

            if (US.eh(response)) {
                return response;
            } else {
                return false;
            }
        }).
         error(function (response, status, headers, config) {
             // called asynchronously if an error occurs
             // or server returns response with an error status.
         });


        //var promise = $http.get(url + JSON.stringify(data)).success(function (response) {
        //    if (US.eh(response)) {
        //        return response;
        //    } else {
        //        //alert('Not Connecting to server');
        //        return false;
        //    }
        //});


        return promise;
    };

    this.getSpecCombo = function (attrconmbination) {
        var rdata = [];
console.log('attributevalue',attrconmbination);
        for (var i = 0; i < attrconmbination.length; i++)
        {
            var attributeSelectedValue = attrconmbination[i].attributeValueId;
            var attributeSelectedText = attrconmbination[i].attributeValue;

            var attributeId = attrconmbination[i].attributeId;
            var attributeName = attrconmbination[i].attributeName;

            if (attributeSelectedValue > 0) {
                rdata.push(
                {
                    "attributeId": attributeId,
                    "attributeName": attributeName,
                    "attributeOrder": 0,
                    "parentAttribute": 0,
                    "attributeValueId": attributeSelectedValue,
                    "attributeValue": attributeSelectedText
                });
            }

            else if (attributeSelectedText != "" && attributeSelectedText != undefined) {
                rdata.push(
                 {
                     "attributeId": attributeId,
                     "attributeName": attributeName,
                     "attributeOrder": 0,
                     "parentAttribute": 0,
                     "attributeValueId": 0,
                     "attributeValue": attributeSelectedText
                 });
            }
        }

        return rdata;
    };

  this.getSpecCombination = function (specCombo) {
        var rdata = [];


    for (var i = 0; i < specCombo.length; i++) {
            if (specCombo[i].selectedAttributeValue.attributeValueId > 0) {
                rdata.push(
             {
                 "attributeId": specCombo[i].attributeId,
                 "attributeName": specCombo[i].attributeName,
                 "attributeOrder": 0,
                 "parentAttribute": 0,
                 "attributeValueId": specCombo[i].selectedAttributeValue.attributeValueId,
                 "attributeValue": specCombo[i].selectedAttributeValue.attributeValue
             });
            }
        }

        return rdata;
    }





    //get all UOM
    this.getUOM = function (itemCat) {
        var promise = $http.get(baseRESTUrl + '/indent/uomInfo?itemCategoryId=' + itemCat + '&categoryId=0').success(function (response) {
            if (US.eh(response)) {
                return response;
            } else {
                return false;
            }
        });

        return promise;
    };

    //get all ItemCategory
    this.getItemCategory = function () {
     var promise =$http.get(baseRESTUrl +'/indent/getItemCategory?categoryId=0' ).success(function (response) {
            if (US.eh(response)) {
               return response;
            } else {
                return false;
            }
        });

        return promise;
    };

    this.getStatusList = function () {
        //var rdata = [];
        /*var data = {
            "requestType": "indent",
            "subRequestType": "yesOrNo",
            "systemId": US.systemId,
            "sessionId": US.gsid(),
            "userId": "senthil@gmail.com"
        }; */
        var promise = $http.get(baseRESTUrl +'/indent/getYesOrNo').success(function (response) {
            if (US.eh(response)) {
                return response;
            } else {
                return false;
            }
        });

        return promise;

    };



    //get item dropdown list
    this.getItem = function () {

        var rdata = [{}];
        var data = {
            "requestType": "indent",
            "sessionId": US.gsid(),
            "subRequestType": "GetIndentItem",
            "systemId": US.systemId,
            "authKey": "adfs3sdaczxcsdfw34",
            "userId": US.userId, "roleName": US.roleName, "requestId": US.grequestId(),
            "parameter": {
                "itemName": "mo"
            }
        };
        //"itemCategoryId": US.getCategory()
        $http.get(url + JSON.stringify(data)).success(function (response) {
            if (US.eh(response)) {
                rdata.push(response);
                //console.log(response);
            } else {
                return false;
            }
        });

        return rdata;

    };
    //get UOM dropdown list
    this.GetUOM = function () {

        var rdata = [{}];
        var data = {
            "requestType": "indent",
            "sessionId": US.gsid(),
            "subRequestType": "GetUOM",
            "systemId": US.systemId,
            "authKey": "adfs3sdaczxcsdfw34",
            "userId": "senthil@gmail.com"
        };
        $http.get(url + JSON.stringify(data)).success(function (response) {
            if (US.eh(response)) {
                rdata.push(response);
                //console.log(response);
            } else {
                //alert('Not Connesssscting to server');
                return false;
            }
        });
        return rdata;

    };

    //Save Indent
    this.createIndent = function (scope) {
        var rdata;
        var INCOM = 0;
        if (scope.IntercomNo == "" || scope.IntercomNo <= 0)
            INCOM = 0;
        else
            INCOM = scope.IntercomNo;

        //"requestType": "indent",
        //  "sessionId": US.gsid(),
        //  "subRequestType": "createIndent",
        //  "systemId": US.systemId,
        //  "authKey": US.authKey,
        //  "userId": US.userId, "roleName": US.roleName, "requestId": US.grequestId(),

        var customItems = this.customizeItems(scope.items);

        var data = {
            //  "indentInfo":
            //{
            //"intendNo": scope.indentNo,
            "prDate": scope.date,
            "name": scope.Name,
            "mobile": scope.MobileNo,
            "interComm": INCOM,
            "email": scope.EmailID,
            "companyId": scope.companyId,
            "departmentId": scope.Department,
            "itemList": customItems //scope.items
            //}
        };

        var isNewIndent = (scope.indentNo == "" || scope.indentNo == "0" || scope.indentNo == null);

        //var parameter =

        var promise = $http({
            method: isNewIndent ? 'POST' : 'PUT',
            url: isNewIndent ? baseRESTUrl + '/indent/' : baseRESTUrl + '/indent/' + scope.indentNo,
            data: data,
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        }).success(function (response, status, headers, config) {
            // this callback will be called asynchronously
            // when the response is available

            console.log('Saved successfully');

            if (US.eh(response)) {
                rdata.push(response);
            } else {
                return false;
            }
        }).
          error(function (response, status, headers, config) {
              US.eh(response);
              //console.log(response);

              // called asynchronously if an error occurs
              // or server returns response with an error status.
          });

        return promise;
    };

    this.customizeItems = function (items) {
        var arr = [];

        for (var i = 0; i < items.length; i++) {
            var obj = new Object();
            obj.itemId = items[i].itemId;
            obj.prDetailId = items[i].prDetailId;
            obj.itemName = items[i].itemName;
            obj.itemCategory = items[i].itemCategory;
            obj.uom = items[i].uom;
            obj.qty = items[i].qty;
            obj.itemSpec = items[i].itemSpec;
            obj.prDetailAnnotation = items[i].prDetailAnnotation;
            obj.status = items[i].status;
            arr.push(obj);

            //"": data.itemId,
            //      "prDetailId": 0,
            //      "itemName": data.itemName,
            //      "itemCategory": data.itemCategory,
            //      "uom": $scope.unity,
            //      "qty": $scope.qty,
            //      "itemSpec": $scope.itemSpec,
            //      "prDetailAnnotation": $scope.remark,
            //      "status": $scope.status,
            //      "budgetName": $scope.budgetName,
            //      "purpose": $scope.purpose,
            //      "legderHead": $scope.legderHead
        }

        return arr;
    };

    this.posttest = function () {
        var rdata = [];
        var data = {
            "requestType": "indent",
            "sessionId": US.gsid(),
            "subRequestType": "GetCompanyInfo",
            "systemId": US.systemId,
            "authKey": "adfs3sdaczxcsdfw34",
            "userId": "senthil@gmail.com"
        };

        var parameter = {
            method: 'PUT',
            url: 'http://192.168.99.122:9090/IshaPURServiceDev/rest/services/putData',
            data: data,
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        }
        $http(parameter).success(function (data, status, headers, config) {
            // this callback will be called asynchronously
            // when the response is available
            console.log(data);
            console.log(headers);
        }).
          error(function (data, status, headers, config) {
              // called asynchronously if an error occurs
              // or server returns response with an error status.
          });


        /*$http.post(url+JSON.stringify(data)).success(function(response){
                if (US.eh(response))
                    rdata.push(response);
                else
                    return false;
            });
        return rdata;*/
    };
}]);


App.service('util_SERVICE', ['$http', '$window', '$cookieStore', '$rootScope', function ($http, $window, $cookie, $rootScope) {
    var urlsd = window.location.href.split("/");
    var baseRESTAPI = "http://192.168.99.122:9090/IshaPMSServiceDev/api";

    if (urlsd[4] == "purchase_dev" || urlsd[4] == "pur_dev" || urlsd[4] == "new_pur_git" || urlsd[4] == "pur_santhosh") {
        var url = "http://192.168.99.122:9090/IshaPURServiceDev/rest/services/getData?requestParam=";
		//var url = "http://192.168.99.122:9090/IshaPMSServiceDev/api/"
        var Puturl = "http://192.168.99.122:9090/IshaPURServiceDev/rest/services/putData";
    }
    else {
        var url = "http://192.168.99.122:9090/IshaPURService/rest/services/getData?requestParam=";
        var Puturl = "http://192.168.99.122:9090/IshaPURService/rest/services/putData";

    }

    this.baseRESTAPI = baseRESTAPI;
    this.url = url;
    this.Puturl = Puturl;
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
    //get category list
    this.gcList = function () {
        console.log('categoryList ', $cookie.get('categoryList'));
        return $cookie.get('categoryList');
        // return "2";
    };

    //get all avilable category
    this.getCategorydata = function () {
        var rdata = [];
        var data = {
            "requestType": "authorisation",
            "subRequestType": "getCategorydata",
            "systemId": this.systemId,
            "authKey": "adfs3sdaczxcsdfw34",
            "sessionId": this.gsid()
        };
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






}]);


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
