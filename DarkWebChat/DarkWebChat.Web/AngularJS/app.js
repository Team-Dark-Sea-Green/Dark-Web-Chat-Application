var app = angular.module('DarkWebChat', ['ngRoute']);

app.constant('baseUrl',
    'http://localhost:61714/api/'
);

app.value('chatHub', $.connection.DarkWebChatHub);

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
            controller: 'ChannelController'
        })
        .otherwise({
            redirectTo: '/'
        });
}]);
