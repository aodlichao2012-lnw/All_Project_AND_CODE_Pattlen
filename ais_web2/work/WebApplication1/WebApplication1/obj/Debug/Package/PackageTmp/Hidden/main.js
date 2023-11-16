
function getCookie(cookieName) {
    var name = cookieName + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var cookieArray = decodedCookie.split(';');
    for (var i = 0; i < cookieArray.length; i++) {
        var cookie = cookieArray[i];
        while (cookie.charAt(0) == ' ') {
            cookie = cookie.substring(1);
        }
        if (cookie.indexOf(name) == 0) {
            return cookie.substring(name.length, cookie.length).replace(/\+/g, ' ');
        }
    }
    return "";
}
function getCookie1(cookieName) {
    var name = cookieName + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var cookieArray = decodedCookie.split(';');
    for (var i = 0; i < cookieArray.length; i++) {
        var cookie = cookieArray[i];
        while (cookie.charAt(0) == ' ') {
            cookie = cookie.substring(1);
        }
        if (cookie.indexOf(name) == 0) {
            return cookie.substring(name.length, cookie.length).replace(/\+/g, ' ');
        }
    }
    return "";
}

sessionStorage.setItem("user_name", getCookie("id"))
    sessionStorage.setItem("strDB", getCookie("id"))
    sessionStorage.setItem("type_title", getCookie("id"))
    sessionStorage.setItem("type_db", getCookie("id"))
    sessionStorage.setItem("Agen", getCookie("Agen"))
    sessionStorage.setItem("EXTENSION", getCookie("id"))
    sessionStorage.setItem("Agent_Ip", getCookie("id"))
    sessionStorage.setItem("id", getCookie("id"))
sessionStorage.setItem("ishastel", "")


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
            $("#modal1").css("display", "none");
            // ทำอย่างอื่นต่อที่นี่
        }
    });


}
function checkSweetAlertStatus() {
    if (isSweetAlertOpen) {
        // SweetAlert ถูกเปิดอยู่
        $("#modal1").css("display", "block");
    } else {
        // SweetAlert ไม่ถูกเปิด
    }
}
$(function () {

    jQuery(function ($) {
        var currentYear = (new Date()).getFullYear();
        ////Loop and add the Year values to DropDownList.
        let year_arr = [];
        for (var i = 2500; i <= currentYear + 543; i++) {

            year_arr.push("" + i + "");
        }
        $("#year_thai").autocomplete({
            source: function (request, response) {
                var searchTerm = request.term.toLowerCase(); // รับคำที่พิมพ์
                var filteredYears = year_arr.filter(function (year) {
                    return year.toString().includes(searchTerm);
                });
                response(filteredYears);
            }
        });
    })
    $("#button_clear").on('click', function (e) {
        showreportToday()
        $("#Service_select").hide()
        $("#select_st").val(``)
        $("#select_rs").text(`-- กรุณาเลือก --`)
        $("#select_rs").val(``)
        $("#cname").val(``)
        $("#cbocity").val(`0100`)
        $("#cboDeny").val(``)
        $("#csname").val(``)
        $("#txt_tel").val(``)
        $("#date_num").val(``)
        $("#date_thai").val(``)
        $("#year_thai").val(``)
        $("#mouth_thai").val(``)
        $("#button_save").prop('disabled', false)
        $("#button_save2").prop('disabled', false)
        let ajax_ = $.ajax({
            url: '/FrmDetail/Clear_edit?id=' + sessionStorage.getItem("id"),
            type: 'GET',
            data: null,
            success: function (e) {
                cbostatus2 = null;
                fuc_select_status();
                fuc_select_status_2();
                fuc_edit_Service("#Service")
                fucshowtel3(true);
                sessionStorage.setItem("ishastel", "")
            }
        })

    })
    $("#mouth_thai").on('change', function (e) {
        let mounth_ = $("#mouth_thai option:selected").text()
        if (mounth_.endsWith("ยน")) {
            $("#date_num").html(`<select style="width: 200px; height: 25px; position: absolute;" id="date_num" name="">
                                    <option value="" selected>-- กรุณาเลือก --</option>
                                    <option value="1">1</option>
                                    <option value="2">2</option>
                                    <option value="3">3</option>
                                    <option value="4">4</option>
                                    <option value="5">5</option>
                                    <option value="6">6</option>
                                    <option value="7">7</option>
                                    <option value="8">8</option>
                                    <option value="9">9</option>
                                    <option value="10">10</option>
                                    <option value="11">11</option>
                                    <option value="12">12</option>
                                    <option value="13">13</option>
                                    <option value="14">14</option>
                                    <option value="15">15</option>
                                    <option value="16">16</option>
                                    <option value="17">17</option>
                                    <option value="18">18</option>
                                    <option value="19">19</option>
                                    <option value="20">20</option>
                                    <option value="21">21</option>
                                    <option value="22">22</option>
                                    <option value="23">23</option>
                                    <option value="24">24</option>
                                    <option value="25">25</option>
                                    <option value="26">26</option>
                                    <option value="27">27</option>
                                    <option value="28">28</option>
                                    <option value="29">29</option>
                                    <option value="30">30</option>
                                </select>`)
        }
        else if (mounth_.endsWith("พันธ์")) {
            $("#date_num").html(`<select style="width: 200px; height: 25px; position: absolute;" id="date_num" name="">
                                    <option value="" selected>-- กรุณาเลือก --</option>
                                    <option value="1">1</option>
                                    <option value="2">2</option>
                                    <option value="3">3</option>
                                    <option value="4">4</option>
                                    <option value="5">5</option>
                                    <option value="6">6</option>
                                    <option value="7">7</option>
                                    <option value="8">8</option>
                                    <option value="9">9</option>
                                    <option value="10">10</option>
                                    <option value="11">11</option>
                                    <option value="12">12</option>
                                    <option value="13">13</option>
                                    <option value="14">14</option>
                                    <option value="15">15</option>
                                    <option value="16">16</option>
                                    <option value="17">17</option>
                                    <option value="18">18</option>
                                    <option value="19">19</option>
                                    <option value="20">20</option>
                                    <option value="21">21</option>
                                    <option value="22">22</option>
                                    <option value="23">23</option>
                                    <option value="24">24</option>
                                    <option value="25">25</option>
                                    <option value="26">26</option>
                                    <option value="27">27</option>
                                    <option value="28">28</option>
                                    <option value="29">29</option>
                                </select>`)
        }
        else if (mounth_.endsWith("คม")) {
            $("#date_num").html(`<select style="width: 200px; height: 25px; position: absolute;" id="date_num" name="">
                                    <option value="" selected>-- กรุณาเลือก --</option>
                                    <option value="1">1</option>
                                    <option value="2">2</option>
                                    <option value="3">3</option>
                                    <option value="4">4</option>
                                    <option value="5">5</option>
                                    <option value="6">6</option>
                                    <option value="7">7</option>
                                    <option value="8">8</option>
                                    <option value="9">9</option>
                                    <option value="10">10</option>
                                    <option value="11">11</option>
                                    <option value="12">12</option>
                                    <option value="13">13</option>
                                    <option value="14">14</option>
                                    <option value="15">15</option>
                                    <option value="16">16</option>
                                    <option value="17">17</option>
                                    <option value="18">18</option>
                                    <option value="19">19</option>
                                    <option value="20">20</option>
                                    <option value="21">21</option>
                                    <option value="22">22</option>
                                    <option value="23">23</option>
                                    <option value="24">24</option>
                                    <option value="25">25</option>
                                    <option value="26">26</option>
                                    <option value="27">27</option>
                                    <option value="28">28</option>
                                    <option value="29">29</option>
                                    <option value="30">30</option>
                                    <option value="31">31</option>
                                </select>`)
        }
    })
});

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
function fuclogout() {

    let ajax_ = $.ajax({
        url: "/FrmDetail/SingOut?id=" + sessionStorage.getItem("id"),
        type: 'GET',
        success: function (e) {
            if (e === "server มี ปัญหา") {
                clearAllCookies();

                window.location.href = '/FrmLogin/Index'

            }
            clearAllCookies();

            window.location.href = '/FrmLogin/Index'
        }
    })

}
function formatDate(date) {
    return (
        [
            date.getFullYear(),
            padTo2Digits(date.getMonth() + 1),
            padTo2Digits(date.getDate()),
        ].join('-') +
        ' ' +
        [
            padTo2Digits(date.getHours()),
            padTo2Digits(date.getMinutes()),
            // padTo2Digits(date.getSeconds()),  // 👈️ can also add seconds
        ].join(':')
    );
}
function loadser_Service() {
    var cookies = document.cookie.split(";");
    for (var i = 0; i < cookies.length; i++) {
        var cookieParts = cookies[i].split("=");
        var cookieName = cookieParts[0];
        var cookie_values = cookieParts[1];
        $("#button_add_ser").css("display", "block")
    }
}
let column_name_ser = "";
let column_name_id_ser = "";
function fuc_edit_Service(Service) {

    $.ajax({
        url: '/FrmDetail/list_Service2?id=' + sessionStorage.getItem("id"),
        type: 'GET'
        , success: function (e) {
            if (e !== null) {
                let values = JSON.parse(e)
                let htmls = ``
                for (i = 0; i < values.length; i++) {
                    if (i <= 10) {

                        htmls += `<div style="" >`

                        column_name_id_ser += `SERVICE_` + values[i].SER_ID + `,`
                        column_name_ser += values[i].SER_NAME + ","
                        htmls += `<span style=""><input type="checkbox"  class="checkbox_service" id="SERVICE_` + values[i].SER_ID + `" placeholder="" class="input" />  <span class="label_service" id="lbserSERVICE_` + values[i].SER_ID + `" > บริการ ` + values[i].SER_NAME + ` </span> </span>  `


                        htmls += `</div>`

                    }
                    else if (i <= 20) {

                        htmls += `<div style="" >`

                        column_name_id_ser += `SERVICE_` + values[i].SER_ID + `,`
                        column_name_ser += values[i].SER_NAME + ","
                        htmls += ` <span class="label_service" id="lbserSERVICE_` + values[i].SER_ID + `" > บริการ ` + values[i].SER_NAME + ` </span>  <input type="checkbox" class="checkbox_service"  id="SERVICE_` + values[i].SER_ID + `" placeholder="" class="input" />  `

                        htmls += `</div>`

                    } else if (i <= 30) {

                        htmls += `<div style="" >`

                        column_name_id_ser += `SERVICE_` + values[i].SER_ID + `,`
                        column_name_ser += values[i].SER_NAME + ","
                        htmls += ` <span class="label_service" id="lbserSERVICE_` + values[i].SER_ID + `" > บริการ ` + values[i].SER_NAME + ` </span>  <input type="checkbox"  class="checkbox_service" id="SERVICE_` + values[i].SER_ID + `" placeholder="" class="input" />  `

                        htmls += `</div>`

                    }
                    else if (i <= 39) {

                        htmls += `<div style="" >`

                        column_name_id_ser += `SERVICE_` + values[i].SER_ID + `,`
                        column_name_ser += values[i].SER_NAME + ","
                        htmls += ` <span class="label_service" id="lbserSERVICE_` + values[i].SER_ID + `" > บริการ ` + values[i].SER_NAME + ` </span>  <input type="checkbox"  class="checkbox_service"  id="SERVICE_` + values[i].SER_ID + `" placeholder="" class="input" />  `

                        htmls += `</div>`

                    }

                }
                $(Service).html(htmls)
            }

        }

    })
}
function fuc_select_status_2() {
    let ajax_ = $.ajax({
        url: "/FrmDetail/showCity?id=" + sessionStorage.getItem("id")
        , type: "GET",
        success: function (e) {
            if (e === "") {
                Cache_item()
            }
            if (e === "server มี ปัญหา") {
                alert2("server มี ปัญหา กำลัง reload ในอีกสักครู่");
                fuc_select_status_2();
            }
            let values = JSON.parse(e)
            if (values != null || values != "" || e != "[]") {
                let htmls = ` <select style="width:200px;height:25px;"  id="cbocity" > `
                htmls += `  <option  value="-" >  -- กรุณาเลือก -- </option>`
                htmls += `  <option  value="-" > - </option>`
                for (i = 0; i < values.length; i++) {
                    htmls += `  <option  value="` + values[i].CITY_CODE + `" >` + values[i].CITY_NAME_T + `</option>`
                }
                htmls += ` </select>`
                $("#cbocity").html(htmls)
            } else {
                fuc_select_status_2()
            }

        }
    })

}

function button_ser_save(value, id) {
    let ajax_ = $.ajax({
        url: '/FrmDetail/Save_service?id=' + id + "&values=" + value,
        type: 'GET',
        data: null,
        success: function (e) {
            alert2(e)
            fuc_edit_Service("#Service")
            $("#button_ser_set_active").prop('disabled', true);
        }
    })

}
function cbostatus() {
    let ajax_ = $.ajax({
        url: '/FrmReportTel/FrmReportTel_Load?id=' + sessionStorage.getItem("id"),
        type: 'GET',
        data: null,
        success: function (e) {
            if (e === "server มี ปัญหา") {
                alert2("server มีปัญหา กำลัง reload ในอักสักครู่")
                cbostatus()
            }
            else {
                table = JSON.parse(e)
                table_sub = JSON.parse(table[1])
                htmls += '   <select name="" id="select_reson" style="width:100%;" >'
                for (i = 0; i < table_sub.length; i++) {
                    if (table_sub[i] != "undefined") {
                        htmls += '<option '
                        htmls += 'value="' + table_sub[i].RES_CODE + '" >'
                        htmls += table_sub[i].RES_NAME
                        htmls += '</option >'
                    } else {
                        alert2("ไม่มี object ")
                    }
                }
                htmls += '</select>'
                $("#select_reson").html(htmls);
            }

        }
    })

}
function formatNumber(number, decimalPlaces) {
    let numbers = number.torelative(decimalPlaces);
    return numbers.replace('0', 'X').replace('.', '').replace('1', 'X').replace('2', 'X').replace('4', 'X').replace('5', 'X').replace('7', 'X').replace('8', 'X').replace('9', 'X')
}
let set_interval;
let set_intravel;
let set_intravel2;
function fucshowtel3(is_time) {

    if (is_time === 1) {
        /*            console.log("1")*/
        $.ajax({
            url: "/FrmDetail/GetPhone?id=" + sessionStorage.getItem("id") + "&Agen=" + sessionStorage.getItem("Agen")
            , type: "GET",
            cache: false,
            success: function (e) {
                console.log("Tel from ajax is = " + e)
                if (e === "") {
                    $("#txt_tel").attr('disabled', true)
                    $("#txt_tel").val("กำลังค้นหาหมายเลขโทรศัพท์ ....")
                }
                else if (e === "0") {
                    $("#txt_tel").val("")
                }
                else {
                    $("#txt_tel").attr('disabled', true)
                    $("#txt_tel").val(e)
                    is_time = 0;
                }
            }
        })


    }
    else if (is_time === 0) {
    }
    else
        if (is_time === 2) {

            $("#txt_tel").val(``)
            /*  console.log("2")*/
            /*        }*/
        }



}
var chatHub;

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

                                }
                            } else {

                                Save_function()


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
        datas.append("id", sessionStorage.getItem("id"))

        /*    datas.append("cboDeny", current_date)*/

        let ajax_ = $.ajax({
            url: '/FrmDetail/btnSave_Click',
            contentType: false,
            processData: false,
            type: 'POST',
            data: datas
            , success: function (e) {

                showreportToday();
                if (e === "บันทึกข้อมูลเรียบร้อย") {

                    sessionStorage.setItem("ishastel", "Standby")
                    $("#status").text("Standby").css("color", "green")
                    $("#txt_tel").text("")
                    $("#txt_tel").val(``)
                    $("#button_save").prop('disabled', true)

                    showreportToday();
                    Issave = true
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
                    alert2(e)
                } else
                {

                    alert2(e)
                    sessionStorage.setItem("ishastel", "Standby")
                }

            }
        })


    }

    function showreportToday() {

        $.ajax({
            url: '/FrmDetail/showreportToday?id=' + sessionStorage.getItem("id"),
            cache: false,
            type: 'GET',
            data: null,
            success: function (e) {

                table = JSON.parse(e)
                table_sub = JSON.parse(table[0])
                table_sub2 = JSON.parse(table[1])
                table_sub3 = JSON.parse(table[2])
                sum2(table_sub, table_sub2, table_sub3)

            }
        })

    }
    function sum2(table, table2, table_sub3) {

        if (table[0].SUM == null) {
            $("#Label8_today").text("0")
            $("#labelTel").text("0")
            for (i = 0; i < table_sub3.length; i++) {

                if (table_sub3[0].IS_ACTIVE == 1) {
                    $("#label_ser1_today").text(table_sub3[0].SER_NAME)
                    $("#Label9_today").text("0 บริการ")
                }
                else {
                    $("div[data-show='1']").remove();
                }

                if (table_sub3[1].IS_ACTIVE == 1) {
                    $("#label_ser2_today").text(table_sub3[1].SER_NAME)
                    $("#Label10_today").text("0 บริการ")
                }
                else {
                    $("div[data-show='2']").remove();
                }

                if (table_sub3[2].IS_ACTIVE == 1) {
                    $("#label_ser3_today").text(table_sub3[2].SER_NAME)
                    $("#Label11_today").text("0 บริการ")
                }
                else {
                    $("div[data-show='3']").remove();
                }

                if (table_sub3[3].IS_ACTIVE == 1) {
                    $("#label_ser4_today").text(table_sub3[3].SER_NAME)
                    $("#Label12_today").text("0 บริการ")
                }
                else {
                    $("div[data-show='4']").remove();
                }
                $("#Label8_today").text("0")
            }
            $("#labelTel").text("0")
        } else {

            $("#Label8_today").text(table[0].SUM)
            $("#labelTel").text(table2.length)
            if (table_sub3[0].SER_ID == '11' && table_sub3[0].IS_ACTIVE == "1") {
                $("#label_ser1_today").text(table_sub3[0].SER_NAME)
                $("#Label9_today").text(table[0].SER11 + " บริการ")

            }
            else {
                $("div[data-show='1']").remove();
            }

            if (table_sub3[1].SER_ID == '12' && table_sub3[1].IS_ACTIVE == "1") {
                $("#label_ser2_today").text(table_sub3[1].SER_NAME)
                $("#Label10_today").text(table[0].SER12 + " บริการ")
            }
            else {
                $("div[data-show='2']").remove();
            }

            if (table_sub3[2].SER_ID == '13' && table_sub3[2].IS_ACTIVE == "1") {
                $("#label_ser3_today").text(table_sub3[2].SER_NAME)
                $("#Label11_today").text(table[0].SER13 + " บริการ")

            }
            else {
                $("div[data-show='3']").remove();
            }

            if (table[0].SER21 > 0 && table_sub3[3].SER_ID == '21' && table_sub3[3].IS_ACTIVE == "1") {
                $("#label_ser4_today").text(table_sub3[3].SER_NAME)
                $("#Label12_today").text(table[0].SER21 + " บริการ")

            }
            else {
                $("div[data-show='4']").remove();
            }




        }


    }
    function fuc_select_status() {
        let ajax_ = $.ajax({
            url: "/FrmDetail/setcboStatus?id=" + sessionStorage.getItem("id")
            , type: "GET",

            success: function (e) {
                if (e === null) {
                    Cache_item()
                }
                if (e === "server มี ปัญหา" || e === "<empty string>") {
                    alert2("server มี ปัญหา กำลัง reload ในอีกสักครู่")
                    fuc_select_status();
                }
                if (cbostatus2 === null) {
                    let values = JSON.parse(e)
                    if (values != "" || values != null || e != "[]") {
                        let htmls = ` <select  id="select_st"  style="width:200px;height:25px;" >   <option value="" selected>
                                        -- กรุณาเลือก --
                                    </option>`

                        for (i = 0; i < values.length; i++) {
                            if (values[i].RES_CODE === "01") {
                                htmls += `  <option  value="` + values[i].RES_CODE + `"  >` + values[i].RES_NAME + `</option>`
                            }
                            else {
                                htmls += `  <option  value="` + values[i].RES_CODE + `"  >` + values[i].RES_NAME + `</option>`
                            }
                        }
                        htmls += ` </select >`
                        $("#select_st").html(htmls)

                        clearInterval()
                    }
                    else {
                        fuc_select_status()
                    }

                }

            }
        })

    }
    $(document).on('load', getstatus());

    //function getstatus() {
    //    setInterval(getfuc, 600);
    //}

    //$(document).on('load', getstatus());

    //function getstatus() {
    //    setInterval(getfuc, 600);
    //}


    //function fucshowtel3(is_time) {

    //    if (is_time === 1) {
    //        /*            console.log("1")*/

    //        $.ajax({
    //            url: "/FrmDetail/GetPhone?id=" + sessionStorage.getItem("id")
    //            , type: "GET",
    //            cache: false,
    //            success: function (e) {
    //                console.log("Tel from ajax is = " + e)
    //                if (e === "") {
    //                    $("#txt_tel").attr('disabled', true)
    //                    $("#txt_tel").val("กำลังค้นหาหมายเลขโทรศัพท์ ....")
    //                }
    //                else if (e === "0") {
    //                    $("#txt_tel").val("")
    //                }
    //                else {
    //                    $("#txt_tel").attr('disabled', true)
    //                    $("#txt_tel").val(e)
    //                    is_time = 0;
    //                }
    //            }
    //        })


    //    }
    //    else if (is_time === 0) {
    //    }
    //    else
    //        if (is_time === 2) {

    //            $("#txt_tel").val(``)
    //            /*  console.log("2")*/
    //            /*        }*/
    //        }



    //}

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


                                    }
                                } else {

                                    Save_function()


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


        //function Save_function() {
        //    let SERVICE2_ = "#SERVICE_";
        //    let service = $("[id]").filter(function (e) {
        //        return this.id === SERVICE2_
        //    })
        //    let datas = new FormData();

        //    for (i = 0; i < service.prevObject.length; i++) {

        //        if (service.prevObject[i].id.includes("SERVICE_") === true) {

        //            datas.append(service.prevObject[i].id, service.prevObject[i].checked)
        //        }

        //    }

        //    let reson = $("#select_st").val()
        //    let reson_2 = $("#select_rs").find(":selected").text()
        //    let reson_code = $("#select_rs").val()
        //    let current_date = $("#current_date").val()
        //    let cname = $("#cname").val()
        //    let cbocity = $("#cbocity").val()
        //    let cbocity_name = $("#cbocity").find(":selected").text()
        //    let cboDeny = $("#cboDeny").val()
        //    let sex2 = $("#sex2").val()
        //    let csname = $("#csname").val()
        //    let txt_tel = $("#txt_tel").val()
        //    let date_num = $("#date_num").val()
        //    let date_thai = $("#date_thai").val()
        //    let year = $("#year_thai").val()
        //    let mouth = $("#mouth_thai").val()
        //    let date_tel = $("#date_tel").val()
        //    datas.append("txtYear", year)
        //    datas.append("cboMouth", mouth)
        //    datas.append("cboStatus", reson)
        //    datas.append("cboDate", date_num)
        //    datas.append("txtName", cname)
        //    datas.append("txtSName", csname)
        //    datas.append("txtTel_No", txt_tel)
        //    datas.append("Date_thai", date_thai)
        //    datas.append("cbocity", cbocity)
        //    datas.append("cbocity_name", cbocity_name)
        //    datas.append("cboDeny", cboDeny)
        //    datas.append("strDeny", reson_2)
        //    datas.append("cboSex", sex2)
        //    datas.append("strDenycode", reson_code)
        //    datas.append("txtDate_Tel", date_tel)
        //    datas.append("id", sessionStorage.getItem("id"))

        //    /*    datas.append("cboDeny", current_date)*/

        //    let ajax_ = $.ajax({
        //        url: '/FrmDetail/btnSave_Click',
        //        contentType: false,
        //        processData: false,
        //        type: 'POST',
        //        data: datas
        //        , success: function (e) {

        //            showreportToday();
        //            if (e === "บันทึกข้อมูลเรียบร้อย") {
        //                alert2(e)

        //                $("#status").text("Standby").css("color", "green")
        //                $("#txt_tel").text("")
        //                $("#txt_tel").val(``)
        //                $("#button_save").prop('disabled', true)

        //                showreportToday();
        //                Issave = true
        //                $("#button_save").prop('disabled', false)
        //                $("#button_save2").prop('disabled', false)
        //                $("#Service_select").hide()
        //                $("#select_st").val(``)
        //                $("#select_rs").text(`-- กรุณาเลือก--`)
        //                /*                    $("#current_date").val(``)*/
        //                $("#cname").val(``)
        //                $("#cbocity").val(`0100`)
        //                $("#cboDeny").val(``)
        //                /*                    $("#date_tel").val(``)*/
        //                $("#csname").val(``)
        //                $("#txt_tel").val(``)
        //                $("#date_num").val(``)
        //                $("#date_thai").val(``)
        //                $("#year_thai").val(``)
        //                $("#mouth_thai").val(``)
        //                $("#button_save").show()
        //                $("#button_save2").hide()
        //                fuc_edit_Service("#Service")
        //                $("#valid1").hide()
        //                $("#valid2").hide()
        //                $("#valid3").hide()
        //                $("#valid4").hide()
        //                $("#valid5").hide()
        //                showreportToday();
        //            } else {
        //                alert2(e)
        //            }

        //        }
        //    })


        //}
    }
function getfuc() {
  
    if (sessionStorage.getItem("ishastel") !== "Busy")
        $.ajax({
            url: '/FrmStatus/FrmStatus_Load?id=' + sessionStorage.getItem("id") + "&Agen=" + sessionStorage.getItem("Agen"),
            type: "GET",
            data: null,
            cache: false,
            success: function (e) {
                console.log(e)

                if (e === "Busy") {

                    if (getCookie("Tel" + getCookie("id")) == "" || getCookie("Tel" + getCookie("id")) == null) {
                        console.log("ค้นหาเบอร์ใหม่")

                        fucshowtel3(1)
                    }
                    else {
                        $("#button_save").prop('disabled', false)
                        console.log("มีเบอร์แล้ว")
                        console.log(getCookie("Tel" + sessionStorage.getItem("id")))
                        $("#txt_tel").text(getCookie("Tel" + getCookie("id")))
                        sessionStorage.setItem("ishastel", "Busy")
                        fucshowtel3(0)
                    }
                    $("#status").text("Busy").css("color", "red")
                    showreportToday()

                } else if (e === "Not Login") {
                    $("#txt_tel").val(``)
                    $("#txt_tel").text("")
                    $("#status").text("Not Login").css("color", "gray")
                    showreportToday()

                } else if (e === "Available") {
                    /*       fucshowtel3(1)*/
                    $("#txt_tel").val(``)
                    $("#txt_tel").text("")
                    $("#status").text("Available").css("color", "green")
                    sessionStorage.setItem("ishastel", "Available")
                    showreportToday()

                } else if (e === "Aux") {
                    $("#status").text("Aux").css("color", "black")
                    showreportToday()
                    fucshowtel3(0)
                    sessionStorage.setItem("ishastel", "Aux")

                    /*                    fucshowtel3(2)*/
                } else if (e === "Standby") {
                    $("#txt_tel").val(``)
                    $("#txt_tel").text("")
                    $("#status").text("Standby").css("color", "green")
                    fucshowtel3(2)
                    sessionStorage.setItem("ishastel", "Standby")
                    showreportToday();
                }
            }
        })
    }
    function getstatus() {
        setInterval(getfuc, 600);
    }

    let cbostatus2 = null;
    let fucshowtel2 = null;
    let id_serivce;
    let name_serivce;
    let table2;
    $(document).on('load', $("#modal1").css("display", "none"));

    $(".sweet-confirm").on('click', function (e) {

        $("#modal1").css("display", "none")
    })
    $("#total_l").hide()
    $("#right_bar").hide()
    $("#Service_select").hide()
    $("#button_add_ser").hide()
    $("#button_ser_set_active").prop('disabled', true);
    $("#Service").on('load', fuc_edit_Service("#Service"));
    $("#button_search").on('click', function (e) {

        window.location.href = '/FrmSearchNumber/Index?id=' + sessionStorage.getItem("id")

    })
    $("#button_Main").on('click', function (e) {

        window.location.href = '/FrmDetail/Index?id=' + sessionStorage.getItem("id")

    })
    $("#button_Report").on('click', function (e) {

        window.location.href = '/FrmReportTel/Index?id=' + sessionStorage.getItem("id")

    })

    $('#current_date').datetimepicker({
        format: 'd/m/Y',
        formatDate: 'd/m/Y',
        timepicker: false, // ไม่ต้องแสดงเวลา
        yearOffset: 543, // ปรับปีให้เป็น พ.ศ.

    });
    $("#date_reson").datetimepicker({
        format: 'd/m/Y',
        formatDate: 'd/m/Y',
        timepicker: false, // ไม่ต้องแสดงเวลา
        yearOffset: 543, // ปรับปีให้เป็น พ.ศ.

    });
    $('#date_tel').datetimepicker({
        format: 'd/m/Y',
        formatDate: 'd/m/Y',
        timepicker: false, // ไม่ต้องแสดงเวลา
        yearOffset: 543, // ปรับปีให้เป็น พ.ศ.
    });
    $(document).on('load', showreportToday());
    $("#valid1").hide()
    $("#valid2").hide()
    $("#valid3").hide()
    $("#valid4").hide()
    $("#valid5").hide()
    const currentDate = new Date();
    const year = parseInt(currentDate.getFullYear()) + 543;
    // Get the current month (0-11, where 0 is January and 11 is December)
    const month = (currentDate.getMonth() + 1).toString().padStart(2, '0'); // Adding 1 because months are zero-based
    // Get the current day of the month
    const day = (currentDate.getDate()).toString().padStart(2, '0');
    $("#current_date").val(`` + day + `/` + month + `/` + year + ``)
    $("#date_tel").val(`` + day + `/` + month + `/` + year + ``)
    $("#date_reson").val(`` + day + `/` + month + `/` + year + ``)
    $("#button_save2").hide()
    $("#svg1").on('click', function (e) {

        window.location.href = '/FrmDetail/Index?id=' + sessionStorage.getItem("id")


    })
    $("#svg2").on('click', function (e) {

        window.location.href = '/FrmDetail/Index?id=' + sessionStorage.getItem("id")


    })
    $("#Service_select tbody").on('click', 'input[type="checkbox"]', function () {
        let isChecked = $(this).prop('checked');
        let $row = $(this).closest('tr');
        let $span = $row.find('span');
        let spanText
        $span.each(function () {
            spanText = $(this).text();
            if (isChecked) {
                $("#button_ser_set_active").prop('disabled', false);
            }

            else {
                $("#button_ser_set_active").prop('disabled', true);
            }
        });

    })
    $("#Service_select tbody").on('click', '[id^="button_ser_save"]', function () {
        let $closestTd = $(this).closest('td');
        let $textInput = $closestTd.find('input[type="text"]');
        let textInputId = $textInput.attr('id');
        let textInputname = $textInput.val();
        $textInput.prop('disabled', true);
        button_ser_save(textInputname, textInputId)
        table2.draw();

    })
    $("#Service_select tbody").on('click', 'td', function (e) {
        var inputField = $(this).find('input[type="text"][disabled]');
        if (inputField.length > 0) {
            inputField.attr('disabled', false);
        }
    })
    $("#Service_select tbody").on('input', 'td', function (e) {

        if ($(this).find('input').length > 0) {
            var value = $(this).find('input');
            id_serivce = value.attr('id');
            name_serivce = value.val();
        }
    })
    $("#button_logout").on('click', function (e) { fuclogout() })
    $("#button_reload").on('click', function (e) {
        fuc_edit_Service("#Service")
    })
    $("#button_add_ser").on('click', function (e) { fuc_edit_Service("#Service") })
    $("#button_ser_add").on('click', function (e) { fuc_insert_ser() })
    $("#button_ser_set_active").on('click', function (e) { fuc_insert_ser_confirm() })
    $("#button_ser_remove").on('click', function (e) {

        fuc_ser_remove_confirm();
    })
    $("#button_ser_remove_save").on('click',
        function (e) {
            let cons = confirm("คุณต้องการลบใช่หรือไม่ ?");
            if (con) {
                fuc_ser_remove_confirm()
            } else {

            }
        }
    )
    let table;
    let htmls;
    let table_sub;
    let table_sub2;
    $(document).on('load', cbostatus())
    $("#button_logout").on('click', function (e) { fuclogout() })
    $("#button_back").on('click', function (e) { fucback() })
    $("#year_thai").on('change', function (e) {
        let year = parseInt($("#year_thai").val());
        let Now_years = new Date().getFullYear()
        Now_years = parseInt(Now_years.toString()) + 543
        year = Now_years - year
        if (year > 55) {
            alert2("ลูกค้าอายุมากกว่า 55 ปี ");
            //$("#button_save").prop('disabled', true)
            //$("#button_save2").prop('disabled', true)
            $("#button_save").prop('disabled', false)
            $("#button_save2").prop('disabled', false)
        }
        else if (year < 15) {
            alert2("ลูกค้าอายุน้อยกว่า 15 ปี ")
            //$("#button_save").prop('disabled', true)
            //$("#button_save2").prop('disabled', true)
            $("#button_save").prop('disabled', false)
            $("#button_save2").prop('disabled', false)
        } else {
            $("#button_save").prop('disabled', false)
            $("#button_save2").prop('disabled', false)
        }
    })
    let intervalId = null;
    let telephone_update = '';
    let telephone_isSave = '';
    let status_busy = '';
    $(window).on('load', function (e) {
        $("#txt_tel").attr('disabled', true)
    })
    const regex = /^[0-9]+$/;
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
    $("#button_save").on('click', function (e) { fucsave(); showreportToday(); })
    $("#button_save2").on('click', function (e) { fucsave(); showreportToday(); })

    $("#select_st").on('load',
        fuc_select_status()
    )
    $("#cbocity").on('load',
        fuc_select_status_2()
    )
    $("#button_logout").on('click', fuclogout)
    function fuc_select_change_reson() {
        let variable = "data-val"
        let reson_select = $("#select_st").val()
        /*  let select = $('#browsers [value="' + reson_select + '"]').data('val')*/
        let ajax_ = $.ajax({
            url: "/FrmDetail/cboStatus_SelectedIndexChanged?cboStatus=" + reson_select + "&res_code=" + reson_select + "&id=" + sessionStorage.getItem("id")
            , type: "GET",
            success: function (e) {

                let values = JSON.parse(e)
                if (e === "server มี ปัญหา") {
                    alert2("server มี ปัญหา กรุณาติดต่อ admin");
                }
                if (values[0] === "เครือข่าย") {
                    let htmls = `  <select  id="cboDeny"  style="width:200px;height:25px;" >`
                    for (i = 0; i < values.length; i++) {
                        htmls += `  <option value = "` + values[i] + `" >` + values[i] + `</option>`
                    }
                    htmls += ` </select>`
                    $("#cboDeny").html(htmls)
                }
                else {
                    let htmls = `  <select  id="select_rs" style="width:200px;height:25px;" >   <option value="" >
                                        -- กรุณาเลือก --
                                    </option>`
                    for (i = 0; i < values.length; i++) {
                        htmls += `  <option value="` + values[i].DENY_CODE + `" data-val="` + values[i].DENY_CODE + `" >` + values[i].DENY_NAME + `</option>`
                    }
                    htmls += ` </select>`
                    $("#select_rs").html(htmls)
                    if (reson_select === "01") {
                        $("#button_add_ser").show()
                    } else {
                        $("#button_add_ser").hide()
                    }
                }
            }
        })

    }
    $("#select_st").on('change', function (e) {
        fuc_select_change_reson()
    })
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


