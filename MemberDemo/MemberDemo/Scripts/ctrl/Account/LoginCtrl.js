angular.module('app', ['ngWW'])
.controller('LoginCtrl', ['$scope', '$http', '$window', function ($scope, $http, $window) {
    $scope.m_signin = {};
    $scope.m_signup = {};
    //Panel切換
    $scope.doSwitch = function (s) { $scope.panel = s; };
    
    function signin() {
        var data = $scope.m_signin;
        var token = $(':input:hidden[name*="RequestVerificationToken"]');
        data[token.attr('name')] = token.val();

        return $http({
            url: myPage.SigninUrl,
            method: 'POST',
            data: $.param(data),
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            }
        });
    }

    $scope.signin = function () {
        signin()
        .then(function (d) {
            console.log(d);
        }, function (d) {
            console.log(d);
            alert(d.message);
        });
        return false;
    };
    $scope.signup = function (m) {
        if ($scope.m_signup.$valid) {
            var data = m;
            var token = $(':input:hidden[name*="RequestVerificationToken"]');
            data[token.attr('name')] = token.val();

            $scope.errmsg = null;
            $http({
                url: myPage.SignupUrl,
                method: 'POST',
                data: $.param(data),
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                }
            }).then(function (d) {
                var rep = d.data;
                if (d.status == 200 && rep.err == 0) {
                    //帳號建立成功，直接登入
                    $scope.m_signin = {
                        UserName: $scope.m_signup.UserName,
                        Password: $scope.m_signup.Password
                    };
                    signin().then(function (d) {
                        $window.location = rep.data;
                    }, function (d) {
                        $scope.errmsg = ['登入失敗'];
                    });
                } else {
                    alert(rep.msg);
                }
            }, function (d) {
                $scope.errmsg = [d.message];
            });
        }
        return false;
    };

}]);