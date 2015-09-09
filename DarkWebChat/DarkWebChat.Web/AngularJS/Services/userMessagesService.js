app.factory('userMessagesService', function ($http, baseUrl) {
    var serviceUrl = baseUrl + 'user-messages';

    function GetUserMessages(userName, limit, headers, success, error) {
        return $http.get(serviceUrl + '/' + userName + '?limit=' + limit, { headers: headers })
            .success(function (data, status, headers, config) {
                success(data);
            }).error(error);
    }

    function GetUserMessageById(id, headers, success, error) {
        return $http.get(serviceUrl + '/message/' + id, { headers: headers })
            .success(function (data, status, headers, config) {
                success(data);
            }).error(error);
    }

    function PostUserMessage(userName, data, headers, success, error) {
        return $http.post(serviceUrl + '/' + userName, data, { headers: headers })
            .success(function (data, status, headers, config) {
                success(data);
            }).error(error);
    }

    return {
        GetUserMessages: GetUserMessages,
        GetUserMessageById: GetUserMessageById,
        PostUserMessage: PostUserMessage
    }
});