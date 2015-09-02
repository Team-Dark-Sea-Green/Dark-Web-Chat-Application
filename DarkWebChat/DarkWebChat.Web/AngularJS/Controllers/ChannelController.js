app.controller("ChannelController",
    function ($scope, $routeParams, channelMessagesService, notificationService, credentialsService) {

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
                notificationService.showInfoMessage(JSON.stringify(serverData))
            },
            function(serverError) {
                notificationService.showErrorMessage(JSON.stringify(serverError));
            });
    }
});