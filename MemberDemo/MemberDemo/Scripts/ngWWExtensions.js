var ngWW = angular.module('ngWW', []);

// for bootstrap 3 pagination
//<div ng-init="currentPage=3; numPages=5; selectCount=0">
//    <pagination num-pages="numPages" current-page="currentPage" on-select-page="selectCount=selectCount+1"></pagination>
//</div>
//var ngPagination = angular.module('ngPagination', [])
ngWW.directive('ngPagination', function () {
    return {
        restrict: 'EA',
        scope: {
            numPages: '=',
            currentPage: '=',
            onSelectPage: '&'
        },
        template:
          '<div class="PageBar"><ul class="pagination">' +
            '<li ng-class="{disabled: noPrevious()}" class="Prev"><a ng-click="selectPrevious()">&#60;</a></li>' +
            '<li ng-repeat="page in pages" ng-class="{active: isActive(page)}"><a ng-click="selectPage(page)">{{page}}</a></li>' +
            '<li ng-class="{disabled: noNext()}" class="Next"><a ng-click="selectNext()">&#62;</a></li>' +
            '</ul>' +
          '</div>',
        replace: true,
        link: function (scope) {
            scope.$watch('numPages', function (value) {
                scope.pages = [];
                for (var i = 1; i <= value; i++) {
                    scope.pages.push(i);
                }
                if (scope.currentPage > value) {
                    scope.selectPage(value);
                }
            });
            scope.$watch('currentPage', function () {
                var min = 1;
                var max = scope.numPages;
                var curMin = scope.currentPage - 4;
                var curMax = scope.currentPage + 5;
                if (curMin < min) {
                    curMin = min;
                    curMax = max < 10?max:10;
                }
                else {
                    if (curMax > max) {
                        curMax = max;
                        if ((curMax - 9) > min) {
                            curMin = (curMax - 10);
                        }
                    }
                }
                scope.pages = [];
                for (var i = curMin; i <= curMax; i++) {
                    scope.pages.push(i);
                }
            });
            scope.noPrevious = function () {
                return scope.currentPage === 1;
            };
            scope.noNext = function () {
                return scope.currentPage === scope.numPages;
            };
            scope.isActive = function (page) {
                return scope.currentPage === page;
            };

            scope.selectPage = function (page) {
                if (!scope.isActive(page)) {
                    scope.currentPage = page;
                    scope.onSelectPage({ "page": page });
                }
            };

            scope.selectPrevious = function () {
                if (!scope.noPrevious()) {
                    scope.selectPage(scope.currentPage - 1);
                }
            };
            scope.selectNext = function () {
                if (!scope.noNext()) {
                    scope.selectPage(scope.currentPage + 1);
                }
            };
        }
    };
});


// http://ericpanorel.net/2013/10/05/angularjs-password-match-form-validation/
// <div class="form-group" data-ng-class="{'has-error':!myForm.password.$valid}"><label for="password">Password</label>
// <input class="form-control" type="password" name="password" required="" data-ng-model="Password" />
// <span class="help-block" data-ng-show="myForm.password.$error.required">This is required.</span></div>
//
// <div class="form-group" data-ng-class="{'has-error':!myForm.passwordCompare.$valid}"><label for="passwordCompare">Confirm Password</label>
//  <input class="form-control" type="password" name="passwordCompare" required="" data-ng-model="PasswordCompare" data-ng-match="Password" />
//  <span class="help-block" data-ng-show="myForm.passwordCompare.$error.required">This is required.</span>
//  <span class="help-block" data-ng-show="myForm.passwordCompare.$error.match">Passwords do not match.</span></div>

var directiveId = 'ngMatch';
ngWW.directive(directiveId, ['$parse','$timeout', function ($parse,$timeout) {

    var directive = {
        link: link,
        restrict: 'A',
        require: '?ngModel'
    };
    return directive;

    function link(scope, elem, attrs, ctrl) {
        // if ngModel is not defined, we don't need to do anything
        if (!ctrl) return;
        if (!attrs[directiveId]) return;

        var firstPassword = $parse(attrs[directiveId]),
            timeout;

        var validator = function (value) {
            var temp = firstPassword(scope),
            v = value === temp;
            ctrl.$setValidity('match', true);
            if (timeout) $timeout.cancel(timeout);
            timeout = $timeout(function () {
                ctrl.$setValidity('match', v);
            }, 500);
            return value;
        }
        ctrl.$parsers.unshift(validator);
        ctrl.$formatters.push(validator);
        scope.$watch(attrs[directiveId], function () {
            validator(ctrl.$viewValue);
        });
    }
}]);