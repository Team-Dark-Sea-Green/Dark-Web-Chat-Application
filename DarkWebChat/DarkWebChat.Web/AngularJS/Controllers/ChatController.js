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
            if (channelMessageData.Text === undefined && channelMessageData.FileContent !== undefined) {
                channelMessageData.Text = "File";
            } channelMessagesService.PostChannelMessage($routeParams.channelName, channelMessageData,
                { Authorization: credentialsService.getSessionToken() },
                function(serverData) {
                    chatHub.server.sendMessageToGroup(JSON.stringify(serverData), $routeParams.channelName);
                },
                function(serverError) {
                    notificationService.showErrorMessage(JSON.stringify(serverError));
                });
        }

        $scope.postPrivateMessage = function (username, userConnectionId, userMessageData) {
            if (userMessageData.Text === undefined && userMessageData.FileContent !== undefined) {
                userMessageData.Text = "File";
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