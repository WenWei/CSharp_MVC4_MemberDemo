angular.module('app', ['ngWW'])
.controller('ctrl', ['$scope', '$http', function ($scope, $http) {
    $scope.members = [];
    //換頁，取得帳號資料
    $scope.getPage = function (page) {
        var data = {pi:page-1};
        $http({
            url: myPage.GetMembersUrl,
            method: 'GET',
            params: data,
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(function (d) {
            for (var i = 0; i < d.data.length; i++) {
                d.data[i].CreatedOn = moment(d.data[i]).format('YYYY-MM-DD HH:MM:SS');
            }
            $scope.members = d.data;
        });

    };
    //刪除帳號
    $scope.delete = function (m,$event) {
        if (!confirm('是否刪除帳號「'+m.UserName+'」?')) {
            return false;
        }

        var data = {'id':m.UserName};
        var token = $(':input:hidden[name*="RequestVerificationToken"]');
        data[token.attr('name')] = token.val();

        $http({
            url: myPage.DeleteMemberUrl,
            method: 'POST',
            data: $.param(data),
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            }
        }).then(function (d) {
            $($event.target).closest('tr').remove();
        },
        function () {
            alert('刪除失敗!');
        });

        return false;
    };


    function init() {
        $scope.getPage(1);
    }

    init();
}]);