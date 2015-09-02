app.factory('userMessagesService', function ($http, baseUrl) {
    var serviceUrl = baseUrl + 'user-messages';

    function GetUserMessages(userName, headers, success, error) {
        return $http.get(serviceUrl + '/' + userName, { headers: headers })
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
        PostUserMessage: PostUserMessage
    }
});