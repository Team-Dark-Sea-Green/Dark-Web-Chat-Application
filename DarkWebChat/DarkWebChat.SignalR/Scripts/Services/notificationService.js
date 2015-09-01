app.factory('notificationService', function () {

    function showInfoMessage(msg) {
            noty({
                    text: msg,
                    type: 'info',
                    layout: 'topCenter',
                    timeout: 1000}
            );
        }

    function showErrorMessage(msg) {
            noty({
                    text: msg,
                    type: 'error',
                    layout: 'topCenter',
                    timeout: 5000
                }
            );
        }

        return  {
            showInfoMessage: showInfoMessage,
            showErrorMessage: showErrorMessage
        }
});