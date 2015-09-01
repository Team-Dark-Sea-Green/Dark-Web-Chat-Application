app.factory('credentialsService', function ($http, baseUrl) {

    function getSessionToken() {
        return sessionStorage.getItem('sessionToken');
    }

    function setSessionToken(sessionToken, type) {
        sessionStorage.setItem('sessionToken', type + ' ' + sessionToken);
    }

    function getUsername() {
        return sessionStorage.getItem('username');
    }

    function setUsername(username) {
        sessionStorage.setItem('username', username);
    }

    function getEmail() {
        return sessionStorage.getItem('email');
    }

    function setEmail(email) {
        sessionStorage.setItem('email', email);
    }

    function isLogged () {
        if (sessionStorage['sessionToken']) {
            return true;
        }
        return false ;
    }

    function clearCredentials() {
        sessionStorage.clear();
    }

    return {
        getSessionToken: getSessionToken,
        setSessionToken: setSessionToken,
        getUsername: getUsername,
        setUsername: setUsername,
        getEmail: getEmail,
        setEmail: setEmail,
        isLogged : isLogged,
        clearCredentials: clearCredentials
    }
});