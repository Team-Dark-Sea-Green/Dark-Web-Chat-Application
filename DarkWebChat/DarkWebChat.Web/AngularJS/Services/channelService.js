app.factory('channelService', function ($http, baseUrl) {
    var serviceUrl = baseUrl + 'Channels';

    function GetChannels(headers, success, error) {
        return $http.get(serviceUrl, { headers: headers })
            .success(function (data, status, headers, config) {
                success(data);
            }).error(error);
    }

    function AddChannel(data, headers, success, error) {
        return $http.post(serviceUrl, data, { headers: headers })
            .success(function (data, status, headers, config) {
                success(data);
            }).error(error);
    }

    return {
        GetChannels: GetChannels,
        AddChannel: AddChannel
    }
});