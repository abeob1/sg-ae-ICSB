var App = angular.module('myApp', ['ngCookies', 'cfp.hotkeys']);


App.directive('loading', ['$http', function ($http) {
      return {
          restrict: 'A',
          link: function (scope, element, attrs) {
              scope.isLoading = function () {
                  return $http.pendingRequests.length > 0;
              };
              scope.$watch(scope.isLoading, function (value) {
                  if (value) {
                      $("#loading").animate({top: '0px'},800);
                  } else {
                      $("#loading").animate({ top: '-100px' },800);
                  }
              });
          }
      };
  } ]);

  App.directive('datetimez', function() {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function(scope, element, attrs, ctrl) {
            $(element).datepicker({
                format: 'dd/mm/yyyy',
                autoclose: true,
                onSelect: function(date) {
                    ctrl.$setViewValue(date);
                    ctrl.$render();
                    scope.$apply();
                }
            });
        }
    };
});

  App.directive('stringToNumber', function () {
      return {
          require: 'ngModel',
          link: function (scope, element, attrs, ngModel) {
              ngModel.$parsers.push(function (value) {
                  return '' + value;
              });
              ngModel.$formatters.push(function (value) {
                  return parseFloat(value, 10);
              });
          }
      }
  })


  App.directive('ngEnter', function () {
      return function (scope, element, attrs) {
          element.bind("keydown keypress", function (event) {
              if (event.which === 13) {
                  scope.$apply(function () {
                      scope.$eval(attrs.ngEnter, { 'event': event });
                  });

                  event.preventDefault();
              }
          });
      };
  });

 
  App.directive('capitalize', function () {
      return {
          require: 'ngModel',
          link: function (scope, element, attrs, modelCtrl) {
              var capitalize = function (inputValue) {
                  if (inputValue == undefined) inputValue = '';
                  var capitalized = inputValue.toUpperCase();
                  if (capitalized !== inputValue) {
                      modelCtrl.$setViewValue(capitalized);
                      modelCtrl.$render();
                  }
                  return capitalized;
              }
              modelCtrl.$parsers.push(capitalize);
              capitalize(scope[attrs.ngModel]); // capitalize initial value
          }
      };
  });



  App.directive('fileModel', ['$parse', function ($parse) {
      return {
          restrict: 'A',
          link: function (scope, element, attrs) {
              var model = $parse(attrs.fileModel);
              var modelSetter = model.assign;

              element.bind('change', function () {
                  scope.$apply(function () {
                      modelSetter(scope, element[0].files[0]);
                  });
              });
          }
      };
  } ]);
