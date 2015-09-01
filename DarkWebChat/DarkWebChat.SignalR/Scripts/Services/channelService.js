app.factory('channelService', function ($http, baseUrl) {
    var serviceUrl = baseUrl + 'Channels';

    function GetChannels(headers, success, error) {
        return $http.get(serviceUrl, {headers: headers})
            .success(function (data, status, headers, config) {
                success(data);
            }).error(error);
    }


    return {
        GetChannels: GetChannels
    }
});