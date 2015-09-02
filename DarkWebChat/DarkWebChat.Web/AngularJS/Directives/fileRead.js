app.directive("fileread", ['notificationService',function (notificationService) {
    return {
        scope: {
            fileread: "="
        },
        link: function (scope, element, attributes) {
            element.bind("change", function (changeEvent) {
                var file = changeEvent.target.files[0];

                if (file.type.match(/image\/.*/)) {
                    var reader = new FileReader();
                    reader.onload = function (loadEvent) {
                        scope.$apply(function () {
                            scope.fileread = {
                                src: loadEvent.target.result,
                                size: file.size
                            }
                        });
                    };
                    reader.readAsDataURL(file);
                } else {
                    notificationService.showErrorMessage('File error, only images allowed.');
                }
            });
        }
    }
}]);