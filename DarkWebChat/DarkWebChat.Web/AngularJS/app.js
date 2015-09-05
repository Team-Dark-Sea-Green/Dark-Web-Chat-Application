var app = angular.module('DarkWebChat', ['ngRoute']);

var connection = $.connection.DarkWebChatHub;
app.value('chatHub', connection);

app.run(function ($rootScope, $route, $location) {
    //Bind the `$locationChangeSuccess` event on the rootScope, so that we dont need to 
    //bind in induvidual controllers.

    $rootScope.$on('$locationChangeSuccess', function () {
        $rootScope.actualLocation = $location.path();
    });

    $rootScope.$watch(function () { return $location.path() }, function (newLocation, oldLocation) {
        if ($rootScope.actualLocation === newLocation) {
            var routeParams = oldLocation.split('/');
            if (routeParams[1] === 'channel') {
                connection.server.leaveChannel(routeParams[2]);
            }
        }
    });
});

app.constant('baseUrl',
    'http://localhost:61714/api/'
);

app.config(['$routeProvider', function (routeProvider) {
    routeProvider
        .when('/', {
            templateUrl: 'Partials/authentication.html',
            controller: 'AuthenticationController'
        })
        .when('/chat-main', {
            templateUrl: 'Partials/chat-main-window.html',
            controller: 'MainController'
        })
        .when('/channel/:channelName', {
            templateUrl: 'Partials/chat-channel-window.html',
            controller: 'ChatController'
        })
        .otherwise({
            redirectTo: '/'
        });
}]);
