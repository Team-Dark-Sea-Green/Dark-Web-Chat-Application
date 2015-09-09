app.factory('channelMessagesService', function ($http, baseUrl) {
    var serviceUrl = baseUrl + 'channel-messages';

    function GetChannelMessages(channelName, limit, headers, success, error) {
        return $http.get(serviceUrl + '/' + channelName + '?limit=' + limit, { headers: headers })
            .success(function (data, status, headers, config) {
                success(data);
            }).error(error);
    }

    function GetChannelMessageById(id, headers, success, error) {
        return $http.get(serviceUrl + '/message/' + id, { headers: headers })
            .success(function (data, status, headers, config) {
                success(data);
            }).error(error);
    }

    function PostChannelMessage(channelName, data, headers, success, error) {
        return $http.post(serviceUrl + '/' + channelName, data, { headers: headers })
            .success(function (data, status, headers, config) {
                success(data);
            }).error(error);
    }


    return {
        GetChannelMessages: GetChannelMessages,
        GetChannelMessageById: GetChannelMessageById,
        PostChannelMessage: PostChannelMessage
    }
});