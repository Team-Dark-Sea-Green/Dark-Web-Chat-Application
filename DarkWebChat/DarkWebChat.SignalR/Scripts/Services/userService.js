app.factory('userService', function ($http, baseUrl) {
    var serviceUrl = baseUrl + 'Account';

    function Register(registerData, success, error) {
        return $http.post(serviceUrl + '/Register', registerData)
            .success(function (data, status, headers, config) {
                success(data);
            }).error(error);
    }

    function Login(loginData, success, error) {
        return $http.post(serviceUrl + '/Login', loginData)
            .success(function (data, status, headers, config) {
                success(data);
            }).error(error);
    }

    return {
        Register : Register,
        Login : Login
    }
});