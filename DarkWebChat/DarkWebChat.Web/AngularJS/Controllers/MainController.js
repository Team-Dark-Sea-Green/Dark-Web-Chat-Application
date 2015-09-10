app.controller("MainController", function ($scope, $location, chatHub, channelService, notificationService, credentialsService) {

    if (!credentialsService.isLogged()) {
        $location.path('/');
        return 0;
    }

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