app.controller("ChatController",
    function ($scope, $routeParams, chatHub, channelMessagesService, userMessagesService, notificationService, credentialsService) {

        // SignalR functions
        chatHub.client.onConnected = function (channelOnlineUsers) {
            $scope.channelOnlineUsers = channelOnlineUsers;
            $scope.$apply();
        }
        
        chatHub.client.onNewUserConnected = function (id, username) {
            $scope.channelOnlineUsers.push({ "ConnectionId": id, "UserName": username });
            sortAlphabeticaly($scope.channelOnlineUsers);
            $scope.$apply();
            notificationService.showInfoMessage(username + " connected");
        }

        chatHub.client.onUserDisconnected = function (username) {
            var set =
                $scope.channelOnlineUsers
                    .filter(function(el) {
                        return el.UserName !== username;
                    });

            $scope.channelOnlineUsers = sortAlphabeticaly(set);
            $scope.$apply();
        }

        chatHub.client.messageReceived = function (message) {
            $scope.channelMessages.push(message);
            $scope.$apply();
        }

        chatHub.client.onPrivateMessageRecieved = function (fromUserConnetionId, message) {
            console.log(message);
        }

        $.connection.hub.stop();
        $.connection.hub.start().done(function() {
            chatHub.server.joinChannel(credentialsService.getUsername(), $routeParams.channelName);
        });

        // Auto-call-functions
        GetChannelMessages($routeParams.channelName);
        function GetChannelMessages(channelName) {
            channelMessagesService.GetChannelMessages(channelName, { Authorization: credentialsService.getSessionToken() },
                function (serverData) {
                    $scope.channelMessages = serverData;
                },
                function (serverError) {
                    notificationService.showErrorMessage(JSON.stringify(serverError));
                });
        }

        // Event-handlers
        $scope.postChannelMessage = function (channelMessageData) {
            var hasFile = false;

            if (channelMessageData.FileContent !== undefined &&
                    channelMessageData.FileContent !== null && channelMessageData.FileContent.trim() !== "") {
                hasFile = true;
            }

            if (channelMessageData.Text === undefined || channelMessageData.Text === null ||
                    channelMessageData.Text.trim() === "" && hasFile === true) {
                channelMessageData.Text = "File only";
            }
            channelMessagesService.PostChannelMessage($routeParams.channelName, channelMessageData,
                { Authorization: credentialsService.getSessionToken() },
                function (serverData) {
                    serverData.hasFile = hasFile;
                    chatHub.server.sendMessageToGroup(JSON.stringify(serverData), $routeParams.channelName);
                },
                function(serverError) {
                    notificationService.showErrorMessage(JSON.stringify(serverError));
                });
        }

        $scope.postPrivateMessage = function (username, userConnectionId, userMessageData) {
            var hasFile = false;

            if (userMessageData.FileContent !== undefined &&
                    userMessageData.FileContent !== null && userMessageData.FileContent.trim() !== "") {
                hasFile = true;
            }

            if (userMessageData.Text === undefined || userMessageData.Text === null ||
                    userMessageData.Text.trim() === "" && hasFile === true) {
                userMessageData.Text = "File only";
            }

            userMessagesService.PostUserMessage(username, userMessageData,
                { Authorization: credentialsService.getSessionToken() },
                function (serverData) {
                    serverData.hasFile = hasFile;
                        chatHub.server.sendPrivateMessage(userConnectionId, JSON.stringify(serverData), $routeParams.channelName);
                },
                function(serverError) {
                    notificationService.showErrorMessage(JSON.stringify(serverError));
                });
        }

        // Functions
        function sortAlphabeticaly(array) {

            var sortedArray = array.sort(function (a, b) {
                var userA = a.UserName.toUpperCase();
                var userB = b.UserName.toUpperCase();
                return (userA < userB) ? -1 : (userA > userB) ? 1 : 0;
            });

            return sortedArray;
        }
});