app.filter('dateTimeFilter', function () {
    return function(input) {
        var now = new Date();
        var postDate = new Date(input);
        var timeDifference = now.getTime() - postDate.getTime();
        timeDifference /= 1000;
        var currentDay = now.getDate();
        var currentMonth = now.getMonth() + 1;
        var currentYear = now.getFullYear();
        var postDay = postDate.getDate();
        var postMonth = postDate.getMonth() + 1;
        var postYear = postDate.getFullYear();

        if (postDay === currentDay && postMonth === currentMonth && postYear === currentYear) {
            if (timeDifference < 60) {
                return 'a few moments ago';
            } else if (timeDifference < 60 * 60) {
                var minutes = Math.floor(timeDifference / 60);
                if (minutes === 1) {
                    return 'a minute ago';
                } else {
                    return minutes + ' minutes ago';
                }
            } else if (timeDifference < 24 * 60 * 60) {
                var hours = Math.floor(timeDifference / 3600);
                if (hours === 1) {
                    return 'an hour ago';
                } else {
                    return hours + ' hours ago';
                }
            }
        } else if (postDay + 1 === currentDay && postMonth === currentMonth && postYear === currentYear) {
            return 'yesterday';
        } else {
            return postDate.toUTCString().split(' ').slice(0, 4).join(' ');
        }
    };
});