app.factory('utilitiesService', function () {

    function sortArrayAlphabeticaly(array) {

        var sortedArray = array.sort(function (a, b) {
            var userA = a.UserName.toUpperCase();
            var userB = b.UserName.toUpperCase();
            return (userA < userB) ? -1 : (userA > userB) ? 1 : 0;
        });

        return sortedArray;
    }

    function convertB64ToBlob(b64Data, contentType, sliceSize) {
        contentType = contentType || '';
        sliceSize = sliceSize || 512;

        var byteCharacters = atob(b64Data);
        var byteArrays = [];

        for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
            var slice = byteCharacters.slice(offset, offset + sliceSize);

            var byteNumbers = new Array(slice.length);
            for (var i = 0; i < slice.length; i++) {
                byteNumbers[i] = slice.charCodeAt(i);
            }

            var byteArray = new Uint8Array(byteNumbers);

            byteArrays.push(byteArray);
        }

        var blob = new Blob(byteArrays, { type: contentType });
        return blob;
    }

    return {
        sortArrayAlphabeticaly: sortArrayAlphabeticaly,
        convertB64ToBlob: convertB64ToBlob
    }
});