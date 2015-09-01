var app = angular.module('DarkWebChat', ['ngRoute']);

app.constant('baseUrl',
    'http://localhost:64919/api/'
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
        .otherwise({
            redirectTo: '/'
        });
}]);
