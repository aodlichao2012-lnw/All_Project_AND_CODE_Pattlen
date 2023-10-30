let intervalId = null;
let telephone_update = '';
let telephone_isSave = '';
let status_busy = '';
$(window).on('load', function (e) {
    $("#txt_tel").attr('disabled', true)
})
/*$(document).on('load', fucshowtel());*/
$(document).on('load', getstatus());

const regex = /^[0-9]+$/;

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
let set_intravel;
let set_intravel2;
//function Load_Telnumber() {
//    set_intravel2 = setInterval(function (e) {
//        if ($("#txt_tel").val() == "" || $("#txt_tel").val() == null || $("#txt_tel").val() == "กำลังค้นหาหมายเลขโทรศัพท์ ....") {
//            fucshowtel3("1");
//            console.log("load = " + $("#txt_tel").val())
//        } else if ($("#txt_tel").val() != "" || $("#txt_tel").val() != null || $("#txt_tel").val() != "กำลังค้นหาหมายเลขโทรศัพท์ ....") {

//            fucshowtel3("0");
//        }
//    }, 1000)

//}
//$(window).on('load', function (e) {
//    Load_Telnumber();
//})
//$("#txt_tel").on('input', function (e) {
//    console.log("input = " + $("#txt_tel").val())
//    if ($("#txt_tel").val() != "" || $("#txt_tel").val() != null) {
//        Load_Telnumber() 
//    } else if ($("#txt_tel").val() == "" || $("#txt_tel").val() == null) {
//        Load_Telnumber() 
//    } else {
//        Load_Telnumber() 
//    }

//    })
$("#button_save").on('click', function (e) { fucsave(); showreportToday(); })
$("#button_save2").on('click', function (e) { fucsave(); showreportToday(); })
function getfuc() {
       let ajax_ =  $.ajax({
            url: '/FrmStatus/FrmStatus_Load',
            type: "GET",
            data: null,
            success: function (e) {
             
                if (e === "Busy") {
                    status_busy = 'is_Busy'
                    $("#status").text("Busy").css("color", "red")
                    if (telephone_isSave == 'isSave') {
                        $("#txt_tel").val(``)
                        telephone_isSave = ''
                        fucshowtel3(true)
                        showreportToday();
                    }
                    fucshowtel3(true)
                } else if (e === "Not Login") {
                    $("#txt_tel").val(``)
                    $("#status").text("Not Login").css("color", "gray")
                } else if (e === "Available") {
                    fucshowtel3(false)
                    $("#txt_tel").val(``)
                    $("#status").text("Available").css("color", "green")

                  
                } else if (e === "Aux"){
                    $("#status").text("Aux").css("color", "black")
/*                    fucshowtel3(false)*/
                } else if (e === "Standby") {
                    fucshowtel3(false)
                    $("#txt_tel").val(``)
                    $("#status").text("Standby").css("color", "green")
                    showreportToday();

                }
             
            }
       })
   
}
function getstatus() {
    intervalId = setInterval(getfuc, 2000)

}
//function fucshowtel() {
//           let ajax_ =  $.ajax({
//                url: "/FrmDetail/GetPhone"
//                , type: "GET",
//                data: null,
//                success: function (e) {
//                    if (e === "") {

//                    } else {
//                        $("#txt_tel").val(e)

//                    }

//                }
//            })

//}

function fucshowtel3(is_time) {

    if (is_time == false) {

        $("#txt_tel").val(``)

        /*        }*/
    } else if (is_time == true) {
/*        if (telephone_update != $("#txt_tel").val() ||  $("#txt_tel").val() == "" || $("#txt_tel").val() == null || $("#txt_tel").val() == "กำลังค้นหาหมายเลขโทรศัพท์ ....") {*/
           let ajax_ =  $.ajax({
                url: "/FrmDetail/GetPhone"
                , type: "GET",
                data: null,
                success: function (e) {
                    if (e === "") {
                        $("#txt_tel").attr('disabled', true)
                        $("#txt_tel").val("กำลังค้นหาหมายเลขโทรศัพท์ ....")
                        /*  $("#button_request_phone").text("กำลังค้นหา...")*/
                    } else if (telephone_update != e) {

                        $("#txt_tel").val(e)
                        telephone_update = e

                    } else if (status_busy == 'is_Busy') {
                        $("#txt_tel").val(e)
                    }

                }
           })
           

   
  
    }
  
}
$("#cbocity").on('click', function (e) {
    $("#valid1").hide()
})
$("#date_tel").on('click', function (e) {
    $("#valid2").hide()
})
$("#select_st").on('click', function (e) {
    $("#valid3").hide()
})
$("#date_thai").on('click', function (e) {
    $("#valid5").hide()
})
$("#year_thai").on('click', function (e) {
    $("#valid4").hide()
})
function fucsave() {



    if ($("#txt_tel").val() == "" || $("#txt_tel").val() == null || $("#txt_tel").val() === "กำลังค้นหาหมายเลขโทรศัพท์ ....") {
        alert2("ไม่สามารถบันทึกได้ เนื่องจากไม่มี เบอร์โทรศัพท์ กรุณากรอกหมายเลขโทรศัพท์")
    } else {
        if (regex.test($("#txt_tel").val())) {

            if ($("#cbocity").val() == "" || $("#cbocity").val() == null) {
                $("#valid1").show()
            } else {
                if ($("#date_tel").val() == "" || $("#date_tel").val() == null) {
                    $("#valid2").show()
                } else {
                    if ($("#txt_tel").val().length < 10) {
                        alert2("กรุณากรอก หมายเลขโทรศัพท์ให้ครบ 10 หลัก")
                    } else {
                        if ($("#select_st").val() === "") {
                            $("#valid3").show()
                        } else {
                            if ($("#select_st").val() === "01") {
                                if ($("#date_thai").val() == "" || $("#year_thai").val() == "" || $("#year_thai").val() == null || $("#mouth_thai").val() == null) {
                                    alert2("กรุณากรอก วัน  ปี เกิด")
                                    $("#valid4").show()
                                    $("#valid5").show()
                                }
                                else {

                                    Save_function()
                                    fucshowtel3(true)
                                }
                            } else {

                                Save_function()
                                fucshowtel3(true)
                            }
                        }
                    }
                }
            }
        }

        else {


            alert2("กรุณาพิมพ์หมายเลขโทรศัพท์ให้ถูกต้อง ห้ามมีอักขระ หรือตัวอักษร")
        }
        showreportToday()

    }



    function Save_function() {
        let SERVICE2_ = "#SERVICE_";
        let service = $("[id]").filter(function (e) {
            return this.id === SERVICE2_
        })
        let datas = new FormData();

        for (i = 0; i < service.prevObject.length; i++) {

            if (service.prevObject[i].id.includes("SERVICE_") === true) {

                datas.append(service.prevObject[i].id, service.prevObject[i].checked)
            }

        }

        let reson = $("#select_st").val()
        let reson_2 = $("#select_rs").find(":selected").text()
        let reson_code = $("#select_rs").val()
        let current_date = $("#current_date").val()
        let cname = $("#cname").val()
        let cbocity = $("#cbocity").val()
        let cbocity_name = $("#cbocity").find(":selected").text()
        let cboDeny = $("#cboDeny").val()
        let sex2 = $("#sex2").val()
        let csname = $("#csname").val()
        let txt_tel = $("#txt_tel").val()
        let date_num = $("#date_num").val()
        let date_thai = $("#date_thai").val()
        let year = $("#year_thai").val()
        let mouth = $("#mouth_thai").val()
        let date_tel = $("#date_tel").val()
        datas.append("txtYear", year)
        datas.append("cboMouth", mouth)
        datas.append("cboStatus", reson)
        datas.append("cboDate", date_num)
        datas.append("txtName", cname)
        datas.append("txtSName", csname)
        datas.append("txtTel_No", txt_tel)
        datas.append("Date_thai", date_thai)
        datas.append("cbocity", cbocity)
        datas.append("cbocity_name", cbocity_name)
        datas.append("cboDeny", cboDeny)
        datas.append("strDeny", reson_2)
        datas.append("cboSex", sex2)
        datas.append("strDenycode", reson_code)
        datas.append("txtDate_Tel", date_tel)

        /*    datas.append("cboDeny", current_date)*/

       let ajax_ =  $.ajax({
            url: '/FrmDetail/btnSave_Click',
            contentType: false,
            processData: false,
            type: 'POST',
            data: datas
            , success: function (e) {
                alert2(e)
                showreportToday();
                if (e === "บันทึกข้อมูลเรียบร้อย") {
                    showreportToday();
                    telephone_isSave = 'isSave'
                    telephone_update = ''
                    $("#button_save").prop('disabled', false)
                    $("#button_save2").prop('disabled', false)
                    $("#Service_select").hide()
                    $("#select_st").val(``)
                    $("#select_rs").text(`-- กรุณาเลือก --`)
/*                    $("#current_date").val(``)*/
                    $("#cname").val(``)
                    $("#cbocity").val(`0100`)
                    $("#cboDeny").val(``)
/*                    $("#date_tel").val(``)*/
                    $("#csname").val(``)
                    $("#txt_tel").val(``)
                    $("#date_num").val(``)
                    $("#date_thai").val(``)
                    $("#year_thai").val(``)
                    $("#mouth_thai").val(``)
                    $("#button_save").show()
                    $("#button_save2").hide()
                    fuc_edit_Service("#Service")
                    $("#valid1").hide()
                    $("#valid2").hide()
                    $("#valid3").hide()
                    $("#valid4").hide()
                    $("#valid5").hide()
                    showreportToday();
                }

            }
       })

       
    }
}



