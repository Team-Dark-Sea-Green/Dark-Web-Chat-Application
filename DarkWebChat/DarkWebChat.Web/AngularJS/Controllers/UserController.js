app.controller("UserController",
    function ($scope, $routeParams, $window, $location, chatHub, userMessagesService,
        channelService, notificationService, credentialsService, utilitiesService) {

        if (!credentialsService.isLogged()) {
            $location.path('/');
            return 0;
        }

        var defaultMessagesLimit = 5;

        chatHub.client.onPrivateMessageRecieved = function (fromUserConnetionId, message) {
            var messagesOnPage = $scope.userMessages.length;
            if (messagesOnPage >= 5) {
                $scope.userMessages.shift();
            }

            $scope.userMessages.push(JSON.parse(message));
            $scope.$apply();
        }

        chatHub.client.onSentPrivateMessage = function (toUserConnetionId, message) {
            var messagesOnPage = $scope.userMessages.length;
            if (messagesOnPage >= 5) {
                $scope.userMessages.shift();
            }

            $scope.userMessages.push(JSON.parse(message));
            $scope.$apply();
        }

        $.connection.hub.stop();
        $.connection.hub.start().done(function () {
            chatHub.server.connectUser(credentialsService.getUsername());
        });

        // Auto-call-functions
        GetUserMessages($routeParams.userName);
        function GetUserMessages(userName) {
            userMessagesService.GetUserMessages(userName, defaultMessagesLimit,
                { Authorization: credentialsService.getSessionToken() },
                function (serverData) {
                    $scope.currentUserChat = userName;
                    $scope.userMessages = serverData;
                },
                function (serverError) {
                    notificationService.showErrorMessage(JSON.stringify(serverError));
                });
        }

        GetChannels();
        function GetChannels() {
            channelService.GetChannels({ Authorization: credentialsService.getSessionToken() },
                function (serverData) {
                    $scope.channels = serverData;
                },
                function (serverError) {
                    notificationService.showErrorMessage(JSON.stringify(serverError));
                });
        }

        $scope.getUserMessageById = function(id) {
            userMessagesService.GetUserMessageById(id, { Authorization: credentialsService.getSessionToken() },
                function (serverData) {
                    var base64String = serverData.fileContent.match(',(.+)')[1];
                    var fileType = serverData.fileContent.match(':(.+);');
                    if (fileType != null) {
                        fileType = fileType[1];
                    } else {
                        fileType = 'application/zip';
                    }
                    var blob = utilitiesService.convertB64ToBlob(base64String, fileType);
                    var blobUrl = URL.createObjectURL(blob);

                    $window.open(blobUrl);
                },
                function (serverError) {
                    notificationService.showErrorMessage(JSON.stringify(serverError));
                });
        }

        $scope.postPrivateMessage = function (username, userMessageData) {
            var hasFile = false;
            var isMoreThanOneMb;

            if (userMessageData.FileContent !== undefined && userMessageData.FileContent !== null) {
                hasFile = true;
                isMoreThanOneMb = userMessageData.FileContent.size > 1024 * 1024;
                userMessageData.FileContent = userMessageData.FileContent.src;
            }

            if (hasFile === true && isMoreThanOneMb === true) {
                return notificationService.showErrorMessage("File size must be up to 1MB.");
            }

            if (userMessageData.Text === undefined || userMessageData.Text === null ||
                userMessageData.Text.trim() === "" && hasFile === true) {
                userMessageData.Text = "File only";
            }

            userMessagesService.PostUserMessage(username, userMessageData,
                { Authorization: credentialsService.getSessionToken() },
                function (serverData) {
                    chatHub.server.sendPrivateMessage(username, JSON.stringify(serverData));
                },
                function(serverError) {
                    notificationService.showErrorMessage(JSON.stringify(serverError));
            });
        }
});