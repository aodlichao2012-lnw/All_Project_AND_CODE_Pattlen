


function clearCookie(cookieName) {
    document.cookie = cookieName + "=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
}
function clearAllCookies() {
    var cookies = document.cookie.split(";");
    for (var i = 0; i < cookies.length; i++) {
        var cookieParts = cookies[i].split("=");
        var cookieName = cookieParts[0];
        clearCookie(cookieName);
    }
}

$(document).ready(function () {
    $(window).on('load', function () {
        clearAllCookies();
    });
})

