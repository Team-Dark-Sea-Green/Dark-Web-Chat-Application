app.controller("UserController",
    function ($scope, $routeParams, $window, $location, chatHub, userMessagesService, notificationService, credentialsService) {

        var defaultMessagesLimit = 5;

        chatHub.client.onUserMessageReceived = function (message) {
            var messagesOnPage = $scope.userMessages.length;
            if (messagesOnPage >= 5) {
                $scope.userMessages.shift();
            }

            $scope.userMessages.push(JSON.parse(message));
            $scope.$apply();
        }

        chatHub.client.onPrivateMessageRecieved = function (fromUserConnetionId, message) {
            console.log(JSON.parse(message));
        }

        $.connection.hub.stop();
        $.connection.hub.start().done(function() {
            chatHub.server.joinChannel(credentialsService.getUsername(), $routeParams.channelName);
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
                    var blob = b64ToBlob(base64String, fileType);
                    var blobUrl = URL.createObjectURL(blob);

                    $window.open(blobUrl);
                },
                function (serverError) {
                    notificationService.showErrorMessage(JSON.stringify(serverError));
                });
        }

        $scope.postPrivateMessage = function (username, userConnectionId, userMessageData) {
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
                    chatHub.server.sendPrivateMessage(userConnectionId, JSON.stringify(serverData), $routeParams.channelName);
                },
                function(serverError) {
                    notificationService.showErrorMessage(JSON.stringify(serverError));
            });
        }

});