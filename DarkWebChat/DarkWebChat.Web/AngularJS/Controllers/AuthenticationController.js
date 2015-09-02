﻿app.controller('AuthenticationController', function(
    $scope, $location, userService, notificationService, credentialsService) {

    $scope.register = function (registerData) {

        if (!registerData.username) {
            return notificationService.showErrorMessage('Missing username..');
        }
        if (!registerData.password) {
            return notificationService.showErrorMessage('Missing password..');
        }
        if (registerData.password.length < 6 || registerData.password.length > 100) {
            return notificationService.showErrorMessage('Password should be between 6 and 100 characters long');
        }
        if (!registerData.confirmPassword) {
            return notificationService.showErrorMessage('Missing password repeat..');
        }
        if (registerData.confirmPassword != registerData.password) {
            return notificationService.showErrorMessage('Passwords doesn`t match..');
        }
        if (!registerData.email) {
            return notificationService.showErrorMessage('Missing email..');
        }

        userService.Register(registerData,
            function(serverData) {
                notificationService.showInfoMessage('Registration Successful.');
                credentialsService.setSessionToken(serverData['access_token'], serverData['token_type']);
                credentialsService.setUsername(serverData['userName']);
                $location.path('/chat-main');
            },
            function(serverError) {
                notificationService.showErrorMessage(JSON.stringify(serverError));
            });
    };

    $scope.login = function (loginData) {
        if (!loginData.userName || !loginData.Password) {
            return notificationService.showErrorMessage('Missing login name or password..');
        }
        
        var loginString = 'grant_type=password&username='+loginData.userName+'&password='+loginData.Password;
        userService.Login(loginString,
            function (serverData) {
                notificationService.showInfoMessage('Login Successful.');
                credentialsService.setSessionToken(serverData['access_token'], serverData['token_type']);
                credentialsService.setUsername(serverData['userName']);
                $location.path('/chat-main');
            },
            function (serverError) {
                notificationService.showErrorMessage(JSON.stringify(serverError));
            });
    };
})