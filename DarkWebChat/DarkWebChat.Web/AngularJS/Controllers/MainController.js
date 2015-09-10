app.controller("MainController", function ($scope, $location, chatHub,
    channelService, userMessagesService, notificationService, credentialsService) {

    if (!credentialsService.isLogged()) {
        $location.path('/');
        return 0;
    }

    chatHub.client.onPrivateMessageRecieved = function (fromUserConnetionId, message) {
        var message = JSON.parse(message);
        if(isNaN($scope.sender)){
            $scope.sender = 1;
            $('a#' + message.sender).after('<button class="btn btn-warning btn-xs" style="margin-left: 5px; padding: 0px 5px; line-height: 1.4">'+ $scope.sender +'</button>');
        }
        else{
            $scope.sender = ++$scope.sender;
            $('a#' + message.sender).parent().find('button').text($scope.sender);
        }
        notificationService.showInfoMessage
            (message.sender + " wrote you: " + message.text.substring(0, 10) + "...");
    }

    $.connection.hub.stop();
    $.connection.hub.start().done(function () {
        chatHub.server.connectUser(credentialsService.getUsername());
    });

    //Auto-call-functions
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

    GetUserMessagesUsers();
    function GetUserMessagesUsers() {
        userMessagesService.GetUserMessagesUsers({ Authorization: credentialsService.getSessionToken() },
            function (serverData) {
                $scope.userMessagesUsers = serverData;
                console.log(serverData);
            },
            function (serverError) {
                notificationService.showErrorMessage(JSON.stringify(serverError));
            });
    }

    // Event-handlers
    $scope.addNewChannel = function(channelData) {
        channelService.AddChannel(channelData,
               { Authorization: credentialsService.getSessionToken() },
               function (serverData) {
                   $scope.channels.push(serverData);
               },
               function (serverError) {
                   notificationService.showErrorMessage(JSON.stringify(serverError));
               });
    };
});