﻿app.controller("ChatController",
    function ($scope, $routeParams, $window, $location, chatHub, channelMessagesService,
        userMessagesService, notificationService, credentialsService) {

        if (!credentialsService.isLogged()) {
            $location.path('/');
            return 0;
        }

        var defaultMessagesLimit = 5;

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

        chatHub.client.onChannelMessageReceived = function (message) {
            var messagesOnPage = $scope.channelMessages.length;
            if (messagesOnPage >= 5) {
                $scope.channelMessages.shift();
            }

            $scope.channelMessages.push(JSON.parse(message));
            $scope.$apply();
        }

        chatHub.client.onPrivateMessageRecieved = function (fromUserConnetionId, message) {
            var message = JSON.parse(message);
            notificationService.showInfoMessage
                (message.sender + " wrote you: " + message.text.substring(0, 10) + "...");
        }

        $.connection.hub.stop();
        $.connection.hub.start().done(function () {
            chatHub.server.connectUser(credentialsService.getUsername());
            chatHub.server.joinChannel(credentialsService.getUsername(), $routeParams.channelName);
        });

        // Auto-call-functions
        GetChannelMessages($routeParams.channelName);
        function GetChannelMessages(channelName) {
            channelMessagesService.GetChannelMessages(channelName, defaultMessagesLimit,
                { Authorization: credentialsService.getSessionToken() },
                function (serverData) {
                    $scope.currentChannel = channelName;
                    $scope.channelMessages = serverData;
                },
                function (serverError) {
                    notificationService.showErrorMessage(JSON.stringify(serverError));
                });
        }

        // Event-handlers
        $scope.getChannelMessageById = function(id) {
            channelMessagesService.GetChannelMessageById(id, { Authorization: credentialsService.getSessionToken() },
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

        $scope.postChannelMessage = function(channelMessageData) {
            var hasFile = false;
            var isMoreThanOneMb;

            if (channelMessageData.FileContent !== undefined && channelMessageData.FileContent !== null) {
                hasFile = true;
                isMoreThanOneMb = channelMessageData.FileContent.size > 1024 * 1024;
                channelMessageData.FileContent = channelMessageData.FileContent.src;
            }

            if (hasFile === true && isMoreThanOneMb === true) {
                return notificationService.showErrorMessage("File size must be up to 1MB.");
            }

            if (channelMessageData.Text === undefined || channelMessageData.Text === null ||
                channelMessageData.Text.trim() === "" && hasFile === true) {
                channelMessageData.Text = "File only";
            }

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
        function sortAlphabeticaly(array) {

            var sortedArray = array.sort(function (a, b) {
                var userA = a.UserName.toUpperCase();
                var userB = b.UserName.toUpperCase();
                return (userA < userB) ? -1 : (userA > userB) ? 1 : 0;
            });

            return sortedArray;
        }

        function b64ToBlob(b64Data, contentType, sliceSize) {
            contentType = contentType || '';
            sliceSize = sliceSize || 512;

            var byteCharacters = atob(b64Data);
            var byteArrays = [];

            for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
                var slice = byteCharacters.slice(offset, offset + sliceSize);

                var byteNumbers = new Array(slice.length);
                for (var i = 0; i < slice.length; i++) {
                    byteNumbers[i] = slice.charCodeAt(i);
                }

                var byteArray = new Uint8Array(byteNumbers);

                byteArrays.push(byteArray);
            }

            var blob = new Blob(byteArrays, { type: contentType });
            return blob;
        }
});