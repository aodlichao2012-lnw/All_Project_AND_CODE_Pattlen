$("#btnlogin").on('click', function (e) {
    $("#loadingLogin").show()
    $.ajax({
        url: '/FrmLogin/btnLogin_Click?txtUsername=' + $("#txtUsername").val() + '&txtPassword=' + $("#txtPassword").val() ,
        type: 'POST',
        success: function (e) {
            $("#loadingLogin").hide()
            if (e === "2") {
                alert("กรุณากรอก Username ค่ะ");
            } else if (e === "3") {
                alert("กรุณากรอก Password ค่ะ")
            } else if (e === "04") {
                alert(" ไม่สามารถ เชื่อมต่อ ฐานข้อมูลได้ เนื่องจาก คุณอาจจะคุณป้อน Username หรือ Password ไม่ถูกต้อง หรือ หมดเวลาเชื่อมต่อฐานข้อมูลค่ะ")
            } else if (e === "05") {
                alert("ไม่สามารถเชื่อมต่อ ฐานข้อมูลได้ กรุณาติดต่อผู้ดูแลระบบ")
            } else {
                var msg = e;
                document.cookie = msg
                window.location.href = '/FrmDetail/Index'
            }
           
        }

    })

})

$("#Cbo_Database").on('change', function () {
    var cbocheck = $("#Cbo_Database")
    console.log(cbocheck[0].checked)
    var table = new FormData();
    if (cbocheck[0].checked) {
        table.append("Chk_DB_Backup", true)
        table.append("Cbo_Database", "Backup")
    } else {
        table.append("Chk_DB_Backup", true)
        table.append("Cbo_Database", "Production")
    }

    $.ajax({
        url: '/FrmLogin/Select_database',
        type: 'POST',
        processData: false,
        contentType: false,
        data: table,
        success: function (e) {
            var msg = JSON.parse(e)
            console.log(msg)
            alert(msg)
            $("#status").text(msg[0])
            $("#strDB").text(msg[1])
        }
    })
})
//$(document).on('load',
//    clearAllCookies()
//);

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