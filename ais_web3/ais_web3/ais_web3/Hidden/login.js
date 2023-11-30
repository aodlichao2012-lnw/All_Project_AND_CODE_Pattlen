$(document).ready(function (e) {

    var isSweetAlertOpen = false;
    function alert2(txt) {
        isSweetAlertOpen = true;
        checkSweetAlertStatus()
        swal({
            title: 'แจ้งเตือน!',
            text: txt,
            confirmButtonText: 'ปิด'
        }).then((value) => {
            // ให้ทำอย่างอื่นต่อที่นี่หลังจากผู้ใช้คลิกปุ่ม OK
            if (value) {
                // ตัวอย่าง: ทำสิ่งที่คุณต้องการเมื่อคลิก OK
                $("#modal2").css("display", "none");
                // ทำอย่างอื่นต่อที่นี่
            }
        });


    }
    function checkSweetAlertStatus() {
        if (isSweetAlertOpen) {
            // SweetAlert ถูกเปิดอยู่
            $("#modal2").css("display", "block");
        } else {
            // SweetAlert ไม่ถูกเปิด
        }
    }
    $("#modal1").css("display", "none");
    function alert2(txt) {
        swal({
            title: 'แจ้งเตือน!',
            text: txt,
            confirmButtonText: 'ปิด',
            //showCancelButton: true,
            //cancelButtonText: 'Cancel',
            /*        reverseButtons: true,*/
            customClass: {
                confirmButton: 'my-custom-button' // ใช้คลาส CSS ที่คุณสร้าง
            }
        });
    }
    $("#btnlogin").on('click', function (e) {
        $("#modal1").css("display", "block");
        /*    $("#loadingLogin").show()*/
        if ($("#dd_type_gov").val() != "") {
            sessionStorage.setItem("type_db", $("#dd_type_gov").val())
            sessionStorage.setItem("strDB", $("#Cbo_Database").val())
        }
        let ajax2 = $.ajax({
            url: "/All/Before_CheckLogin?type_db=" + sessionStorage.getItem("type_db") + "&strDB=" + sessionStorage.getItem("strDB"), type: 'GET', success: function (e) {
                let ajax_ = $.ajax({
                    url: '/All/CheckLogin?txtUsername=' + $("#txtUsername").val() + '&txtPassword=' + $("#txtPassword").val() + "&strConn=" + e.split(':')[0] + "&type_db=" + e.split(':')[1],
                    type: 'POST',
                    success: function (e) {
                        let Split1 = e.split(";")[0]
                        let Split2 = e.split(";")[1].replace("'", "")
                        let e1 = e.split(";")[2]
                        let strcon = e.split('|')[1].split(':')[0]
                        let type_db = e.split(":")[1]

                        if (e1 === "2") {
                            alert2("กรุณากรอก Username ค่ะ");
                            $("#modal1").css("display", "none");
                        } else if (e1 === "3") {
                            alert2("กรุณากรอก Password ค่ะ")
                            $("#modal1").css("display", "none");
                        } else if (e1 === "04") {
                            alert2("กรุณากรอกตัวอักขระ เป็นตัวเลข หรือภาษาอังกฤษค่ะ")
                            $("#modal1").css("display", "none");
                        } else if (e1 === "05") {
                            alert2("ไม่สามารถเชื่อมต่อ ฐานข้อมูลได้ กรุณาติดต่อผู้ดูแลระบบค่ะ")
                            $("#modal1").css("display", "none");
                        } else if (e1 === "06") {
                            alert2("Username หรือ Password ไม่ถูกต้องค่ะ")
                            $("#modal1").css("display", "none");
                        }
                        else if (e1 === "1") {

                            sessionStorage.setItem("strcon", strcon)
                            sessionStorage.setItem("Agen", Split1)
                            sessionStorage.setItem("user_name", Split2.replace("'",""))
                            sessionStorage.setItem("strcon", strcon)
                            sessionStorage.setItem("type_db", type_db)
                            window.location.href = '/All/Detail?usernane='+""

                        

                            $("#modal1").css("display", "block");

                        } else {
                            alert2(e1)
                        }
                    }
                })

            }
        })

    })
    $("#Cbo_Database").on('change', function () {
        var cbocheck = $("#Cbo_Database").val()
        alert2(cbocheck)
        var table = new FormData();
        if (cbocheck === "Backup") {
            sessionStorage.setItem("checkbox_DB_Backup", true)
            sessionStorage.setItem("strDB", "Backup")
        } else {
            sessionStorage.setItem("checkbox_DB_Backup", false)
            sessionStorage.setItem("strDB", "Production")
        }
    })
})