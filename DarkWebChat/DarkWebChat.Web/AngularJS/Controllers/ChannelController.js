app.controller("ChannelController",
    function ($scope, $routeParams, chatHub, channelMessagesService, notificationService, credentialsService) {

        //SignalR functions
        chatHub.client.messageReceived = function(message) {
            $scope.channelMessages.push(message);
            $scope.$apply();
        }

        chatHub.client.onConnected = function (channelOnlineUsers) {
            $scope.channelOnlineUsers = SortAlphabeticaly(channelOnlineUsers);
            $scope.$apply();
            console.log(channelOnlineUsers);
        }
        
        chatHub.client.onNewUserConnected = function (id, username) {
            $scope.channelOnlineUsers.push({ "ConnectionId": id, "UserName": username });
            SortAlphabeticaly($scope.channelOnlineUsers);
            $scope.$apply();
            notificationService.showInfoMessage(username + " connected");
        }

        chatHub.client.onUserDisconnected = function (username) {
            var set =
                $scope.channelOnlineUsers
                    .filter(function(el) {
                        return el.UserName !== username;
                    });

            $scope.channelOnlineUsers = SortAlphabeticaly(set);
            $scope.$apply();
        }

        $.connection.hub.stop();
        $.connection.hub.start().done(function() {
            chatHub.server.joinChannel(credentialsService.getUsername(), $routeParams.channelName);
        });

        //Auto-call-functions
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
            channelMessageData.isFile = 0;
            channelMessagesService.PostChannelMessage($routeParams.channelName, channelMessageData,
                { Authorization: credentialsService.getSessionToken() },
                function(serverData) {
                    chatHub.server.sendMessageToGroup(JSON.stringify(serverData), $routeParams.channelName);
                },
                function(serverError) {
                    notificationService.showErrorMessage(JSON.stringify(serverError));
                });
        }

        // Functions
        function SortAlphabeticaly(array) {

            var sortedArray = array.sort(function (a, b) {
                var userA = a.UserName.toUpperCase();
                var userB = b.UserName.toUpperCase();
                return (userA < userB) ? -1 : (userA > userB) ? 1 : 0;
            });

            return sortedArray;
        }
});