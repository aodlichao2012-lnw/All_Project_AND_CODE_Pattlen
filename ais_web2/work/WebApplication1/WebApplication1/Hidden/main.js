let cbostatus2 = null;
let fucshowtel2 = null;
let id_serivce;
let name_serivce;
let table2;
$("#total_l").hide()
$("#right_bar").hide()

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

$("#occ_tel").text(" แบบฟอร์มบันทึกโทร ระบบ " + getCookie("type_db"))
$("#label_username").text(getCookie("user_name"));


function Cache_item() {
    let ajax_ = $.ajax({
        url: '/FrmDetail/Get_sessionStorage_detail',
        type: 'GET'
        , success: function (e) {
            let cbocity_j;
            let select_st_j;


            if (sessionStorage.getItem("cbocity") != null) {
                cbocity_j = sessionStorage.getItem("cbocity");
            }
            if (sessionStorage.getItem("select_st") != null) {
                select_st_j = sessionStorage.getItem("select_st");
            }
            else {
                sessionStorage.setItem("cbocity", e.split(';')[0]);
                cbocity_j = sessionStorage.getItem("cbocity");
                sessionStorage.setItem("select_st", e.split(';')[1]);
                select_st_j = sessionStorage.getItem("select_st");
            }
            let values = JSON.parse(cbocity_j)
            if (values != null || values != "" || e != "[]") {
                let htmls = ` <select style="width:200px;height:25px;"  id="cbocity" > `
                for (i = 0; i < values.length; i++) {
                    htmls += `  <option  value="` + values[i].CITY_CODE + `" >` + values[i].CITY_NAME_T + `</option>`
                }
                htmls += ` </select>`
                $("#cbocity").html(htmls)
            }

            let values2 = JSON.parse(select_st_j)
            if (values2 != "" || values2 != null || e != "[]") {
                let htmls = ` <select  id="select_st"  style="width:200px;height:25px;" >   <option value="" selected>
                                    -- กรุณาเลือก--
                                </option>`

                for (i = 0; i < values2.length; i++) {
                    if (values2[i].RES_CODE === "01") {
                        htmls += `  <option  value="` + values2[i].RES_CODE + `"  >` + values2[i].RES_NAME + `</option>`
                    }
                    else {
                        htmls += `  <option  value="` + values2[i].RES_CODE + `"  >` + values2[i].RES_NAME + `</option>`
                    }
                }
                htmls += ` </select >`
                $("#select_st").html(htmls)

            }

        }
    })

}



$("#valid1").hide()
$("#valid2").hide()
$("#valid3").hide()
$("#valid4").hide()
$("#valid5").hide()

const currentDate = new Date();
const year = parseInt( currentDate.getFullYear()) + 543;
// Get the current month (0-11, where 0 is January and 11 is December)
const month = (currentDate.getMonth() + 1).toString().padStart(2, '0'); // Adding 1 because months are zero-based
// Get the current day of the month
const day = (currentDate.getDate()).toString().padStart(2, '0');

$("#current_date").val(`` + day + `/` + month + `/` + year + ``)
$("#date_tel").val(`` + day + `/` + month + `/` + year + ``)
$("#date_reson").val(`` + day + `/` + month + `/` + year + ``)

$("#button_save2").hide()
function alert2(txt) {
   swal({
        title: 'แจ้งเตือน!',
        text: txt,
        confirmButtonText: 'ปิด',
        //showCancelButton: true,
        //cancelButtonText: 'Cancel',
    /*    reverseButtons: true,*/
        customClass: {
            confirmButton: '    my-custom-button' // ใช้คลาส CSS ที่คุณสร้าง
        }
    });
}
$("#svg1").on('click', function (e) {

    window.location.href = '/FrmDetail/Index'

})
$("#svg2").on('click', function (e) {

    window.location.href = '/FrmDetail/Index'

})
$(function () {
    //Reference the DropDownList.
    var ddlYears = $("#year_thai");

    //Determine the Current Year.
    var currentYear = (new Date()).getFullYear();



    //Loop and add the Year values to DropDownList.
    for (var i = 2500; i <= currentYear + 543; i++) {
        var option = $("<option />");
        option.html(i);
        option.val(i);
        ddlYears.append(option);
    }

    $("#button_clear").on('click', function (e) {
        showreportToday()
        $("#Service_select").hide()
        $("#select_st").val(``)
        $("#select_rs").text(`-- กรุณาเลือก --`)
        $("#select_rs").val(``)
 /*       $("#current_date").val(``)*/
        $("#cname").val(``)
        $("#cbocity").val(`0100`)
        $("#cboDeny").val(``)
        $("#csname").val(``)
        $("#txt_tel").val(``)
        $("#date_num").val(``)
 /*       $("#date_tel").val(``)*/
        $("#date_thai").val(``)
        $("#year_thai").val(``)
        $("#mouth_thai").val(``)
        $("#button_save").prop('disabled', false)
        $("#button_save2").prop('disabled', false)
        let ajax_ = $.ajax({
            url: '/FrmDetail/Clear_edit',
            type: 'GET',
            data: null,
            success: function (e) {
                cbostatus2 = null;
                fuc_select_status();
                fuc_select_status_2();
                fuc_edit_Service("#Service")
                fucshowtel3(true);
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
pdfMake.fonts = {
    THSarabun: {
        normal: 'THSarabun.ttf',
        bold: 'THSarabun-Bold.ttf',
        italics: 'THSarabun-Italic.ttf',
        bolditalics: 'THSarabun-BoldItalic.ttf'
    }
}
/*$(function () { getfucLoad() })*/

$("#Service_select").hide()
$("#button_add_ser").hide()
$("#button_ser_set_active").prop('disabled', true);


$("#Service").on('load', fuc_edit_Service("#Service"));
$("#button_search").on('click', function (e) {

    window.location.href = '/FrmSearchNumber/Index'

})
$("#button_Main").on('click', function (e) {

    window.location.href = '/FrmDetail/Index'

})
$("#button_Report").on('click', function (e) {

    window.location.href = '/FrmReportTel/Index'

})

$('#current_date').datetimepicker({
    format: 'd/m/Y',
    formatDate: 'd/m/Y',

});
$("#date_reson").datetimepicker({
    format: 'd/m/Y',
    formatDate: 'd/m/Y',

});


$('#date_tel').datetimepicker({
    format: 'd/m/Y',
    formatDate: 'd/m/Y',
});


$("#select_st").on('load',
    fuc_select_status()
)
$("#cbocity").on('load',
    fuc_select_status_2()
)

function fuc_select_status() {
    let ajax_ = $.ajax({
        url: "/FrmDetail/setcboStatus"
        , type: "GET",
        data: null,
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

function fuc_select_status_2() {
    let ajax_ = $.ajax({
        url: "/FrmDetail/showCity"
        , type: "GET",
        data: null,
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
        url: "/FrmDetail/SingOut",
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
function getfucLoad() {
    let ajax_ = $.ajax({
        url: '/FrmDetail/LoadEdit',
        type: "GET",
        data: null,
        contentType: 'text/plain',
        success: function (e) {
            if (e !== null) {
                let valutf8 = decodeURIComponent(e);
                let values = JSON.parse(valutf8)
                if (values != null) {
                    if (values.SERVICE_1 !== null && values.SERVICE_1 == true) {
                        $("#SERVICE_01").attr("checked", true)
                    }
                    if (values.SERVICE_2 !== null && values.SERVICE_2 == true) {
                        $("#SERVICE_02").attr("checked", true)
                    }
                    if (values.SERVICE_3 !== null && values.SERVICE_3 == true) {
                        $("#SERVICE_03").prop("checked", true)

                    } if (values.SERVICE_4 !== null && values.SERVICE_4 == true) {
                        $("#SERVICE_04").prop("checked", true)

                    } if (values.SERVICE_5 !== null && values.SERVICE_5 == true) {
                        $("#SERVICE_05").prop("checked", true)

                    } if (values.SERVICE_6 !== null && values.SERVICE_6 == true) {
                        $("#SERVICE_06").prop("checked", true)

                    } if (values.SERVICE_7 !== null && values.SERVICE_7 == true) {
                        $("#SERVICE_07").prop("checked", true)

                    } if (values.SERVICE_8 !== null && values.SERVICE_8 == true) {
                        $("#SERVICE_08").prop("checked", true)

                    } if (values.SERVICE_9 !== null && values.SERVICE_9 == true) {
                        $("#SERVICE_09").prop("checked", true)

                    } if (values.SERVICE_10 !== null && values.SERVICE_10 == true) {
                        $("#SERVICE_10").prop("checked", true)

                    } if (values.SERVICE_11 !== null && values.SERVICE_11 == true) {
                        $("#SERVICE_11").prop("checked", true)

                    } if (values.SERVICE_12 !== null && values.SERVICE_12 == true) {
                        $("#SERVICE_12").prop("checked", true)

                    } if (values.SERVICE_13 !== null && values.SERVICE_13 == true) {
                        $("#SERVICE_13").prop("checked", true)

                    } if (values.SERVICE_14 !== null && values.SERVICE_14 == true) {
                        $("#SERVICE_14").prop("checked", true)

                    } if (values.SERVICE_15 !== null && values.SERVICE_15 == true) {
                        $("#SERVICE_15").prop("checked", true)

                    } if (values.SERVICE_16 !== null && values.SERVICE_16 == true) {
                        $("#SERVICE_16").prop("checked", true)

                    } if (values.SERVICE_17 !== null && values.SERVICE_17 == true) {
                        $("#SERVICE_17").prop("checked", true)

                    } if (values.SERVICE_18 !== null && values.SERVICE_18 == true) {
                        $("#SERVICE_18").prop("checked", true)

                    } if (values.SERVICE_19 !== null && values.SERVICE_19 == true) {
                        $("#SERVICE_19").prop("checked", true)

                    } if (values.SERVICE_20 !== null && values.SERVICE_20 == true) {
                        $("#SERVICE_20").prop("checked", true)

                    } if (values.SERVICE_21 !== null && values.SERVICE_21 == true) {
                        $("#SERVICE_21").prop("checked", true)

                    } if (values.SERVICE_22 !== null && values.SERVICE_22 == true) {
                        $("#SERVICE_22").prop("checked", true)

                    } if (values.SERVICE_23 !== null && values.SERVICE_23 == true) {
                        $("#SERVICE_23").prop("checked", true)

                    } if (values.SERVICE_24 !== null && values.SERVICE_24 == true) {
                        $("#SERVICE_24").prop("checked", true)

                    } if (values.SERVICE_25 !== null && values.SERVICE_25 == true) {
                        $("#SERVICE_25").prop("checked", true)

                    } if (values.SERVICE_26 !== null && values.SERVICE_26 == true) {
                        $("#SERVICE_26").prop("checked", true)

                    } if (values.SERVICE_27 !== null && values.SERVICE_27 == true) {
                        $("#SERVICE_27").prop("checked", true)

                    } if (values.SERVICE_28 !== null && values.SERVICE_28 == true) {
                        $("#SERVICE_28").prop("checked", true)

                    } if (values.SERVICE_29 !== null && values.SERVICE_29 == true) {
                        $("#SERVICE_29").prop("checked", true)

                    }
                    //$("#select_st").html(` <select  id="select_st"  style="width:200px;height:25px;" >
                    //                <option>

                    //                </option>
                    //            </select>`)

                    $("#select_st").append(`<option value="` + values.res_code + `" selected> ` + values.cboStatus + `</option>`)
                    /*  $("#select_rs").val(values.strDenycode)*/
                    $("#select_rs").append(`<option value="` + values.strDenycode + `" selected> ` + values.strDeny + `</option>`)
                    $("#txt_tel").val(values.txtTel_No)
                    $("#cname").val(values.txtName)
                    $("#csname").val(values.txtSName)
                    if (values.cboSex === "F") {
                        $("#sex2").append(`<option value="F" selected>หญิง</option>`)
                    }
                    else if (values.cboSex === "N") {
                        $("#sex2").append(`<option value="N" selected>ไม่ระบุ</option>`)
                    }
                    else {
                        $("#sex2").append(`<option value="M" selected>ชาย</option>`)
                    }
                    $("#date_tel").val(values.txtDate_Tel)
                    $("#date_thai").val(values.Date_thai)
                    $("#date_num").val(values.cboDate_No)
                    $("#mouth_thai").val(values.cboMouth)
                    $("#year_thai").val(values.txtYear)

                    //$("#cbocity").text(values.cbocity_name)
                    //$("#cbocity").val(values.cbocity)
                    $("#cbocity").append(`<option value="` + values.cbocity + `" selected>` + values.cbocity_name + `</option>`)
                    /*   $("#status").text(e)*/
                    cbostatus2 = values.cboStatus;
                    if (values.cboStatus === "สมัคร") {

                        $("#button_add_ser").show()
                    } else {
                        $("#button_add_ser").hide()
                    }

                    $("#button_save").hide()
                    $("#button_save2").show()
                }
            }
            else {
                getfucLoad()
            }


        }
    })
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
    let datas = null;
    if ($("#txt_tel").val() == null) {
        datas = null
    } else {
        datas = $("#txt_tel").val()
    }
    let ajax_ = $.ajax({
        url: '/FrmDetail/list_Service?txt_tel=' + datas,
        type: 'GET',
        data: datas
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
                getfucLoad();
            }

        }

    })
}
function fuc_insert_ser() {
    let datas = null;
    if ($("#txt_tel").val() == null) {
        datas = null
    } else {
        datas = $("#txt_tel").val()
    }
    let ajax_ = $.ajax({
        url: '/FrmDetail/SetVisible_remove?txt_tel=' + datas,
        type: 'GET'
        , data: datas
        , success: function (e) {
            let htmls2 = ``
            let values = JSON.parse(e)

            table2 = $("#Service_select").dataTable({
                destroy: true,
  
                lengthChange: false,
                columns: [
                    { data: 'SER_ID', render: function (data, type, row) { return '<input type="checkbox" id="addSERVICE_' + data + '" value="' + data + '" data-active="' + row["IS_ACTIVE"] + '" class="editor-active">' } },
                    { data: 'SER_ID', title: 'หมายเลขบริการ' },
                    { data: 'IS_ACTIVE', render: function (data, type, row) { return `<span id="ch` + row["SER_ID"] + `">` + data + `</span>` } },
                    {
                        data: 'SER_NAME', render: function (data, type, row) {
                            return '<input type="text" id="editSERVICE_' + row["SER_ID"] + '" value="' + data + '" class="editor-active2 m-3 " disabled style="width:100%;"><button id="button_ser_save' + row["SER_ID"] + '" class="btn btn-primary " >บันทึก</button>'
                        }
                    },
                    { data: 'SER_NAME', render: function (data, type, row) { return '<span style="display: none;">' + data + '</span>' } }



                ],
                data: values,
                "createdRow": function (row, data, dataIndex) {
                    // ตรวจสอบเงื่อนไขของข้อมูลในแต่ละคอลัมน์
                    // ในตัวอย่างนี้เราจะตรวจสอบคอลัมน์ที่ 2 (index 1)
                    var columnValue = data.IS_ACTIVE;
                    if (columnValue == 'เปิดให้ใช้บริการ') {
                        // กำหนดพื้นหลัง (background color) สำหรับแถวนี้
                        $(row).css('background-color', 'lightgreen');
                    } else if (columnValue == 'ปิดการใช้บริการ') {
                        $(row).css('background-color', 'lightcoral');
                    }
                }
            })
            /*         table2.addClass("table-responsive-xl")*/
            table2.removeAttr('style')
            $("#Service_select").show()
        }

    })

}

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
function fuc_insert_ser_confirm() {

    let checkedvalues = $('#Service_select tbody td');
    let active;
    let bool_active = "";
    let data_active = "";
    let span_active_name = "";
    let span_active_name_val = "";
    let is_checked;

    checkedvalues.each(function (e) {
        active = $(this).find("input[type='checkbox']:checked")
        is_checked = active.prop("checked")
        data_active = active.attr("data-active")
        span_active_name_val = active.val()
        if (is_checked) {
            bool_active += data_active + ","
            span_active_name += span_active_name_val + ","
        }


    })
    let data1 = new FormData();
    data1.append("IsActive", bool_active)
    data1.append("Service_id_name", span_active_name)
    let ajax_ = $.ajax({
        url: '/FrmDetail/SetVisible_Unvisible_Enable',
        type: 'POST',
        processData: false,
        contentType: false,
        data: data1,
        success: function (e) {
            alert2(e);
            /*            fuc_insert_ser();*/
        }
    })

}
function fuc_Service_ser_show(Service) {
    //$("#Service2").remove()
    $("#Service2").remove()
    $("#Service_modal").html(`<div id="Service2" style=" width: 1000px; right: 200px; flex-wrap: wrap;"></div>`)
    let htmls2 = ``;
    let nameid = column_name_id_ser.split(',')
    let name = column_name_ser.split(',')

    htmls2 += `<div style="" >`
    for (i = 0; i < name.length - 1; i++) {
        if ($("#" + nameid[i] + "").is(':checked')) {
            htmls2 += `  <input type="checkbox" id="lb` + nameid[i] + `" placeholder="" class="input" /> <span style=""> <span id="lbsersh` + nameid[i] + `" > บริการ ` + name[i] + `</span>`
        }

    }
    htmls2 += `</div style="" >`
    column_name_id_ser = ``;
    column_name_ser = ``;
    $(Service).html(htmls2)
}
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

function cbostatus() {
    let ajax_ = $.ajax({
        url: '/FrmReportTel/FrmReportTel_Load',
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
function sum(table, table2, table_sub3) {
 /*   if (table[0].SUM != "") {*/
        $("#Label8").text(table[0].SUM)
    $("#labelTel2").text(table2.length)


    $("#Labelstatus").text(table2[0].RES_NAME)

    for (i = 0; i < table_sub3.length; i++) {

        if (table[i].SER13 != null && table_sub3[0].IS_ACTIVE == 1) {
            $("#label_ser1").text("Service " + table_sub3[0].SER_NAME)
            $("#Label9").text(""+table[i].SER13 + " บริการ")
        }
        if (table[i].SER11 != null && table_sub3[1].IS_ACTIVE == 1) {
            $("#label_ser2").text("Service " + table_sub3[1].SER_NAME)
            $("#Label10" ).text(""+table[i].SER11 + " บริการ")
        }
        if (table[i].SER21 != null  && table_sub3[2].IS_ACTIVE == 1) {
            $("#label_ser3").text("Service " + table_sub3[2].SER_NAME)
            $("#Label11" ).text(""+table[i].SER21 + " บริการ")
        }
        if (table[i].SER12 != null  && table_sub3[3].IS_ACTIVE == 1) {
            $("#label_ser4").text("Service " + table_sub3[3].SER_NAME)
            $("#Label12" ).text(""+table[i].SER12 + " บริการ")
        }
 
    
    }
 





}
function sum2(table, table2, table_sub3) {

    $("#Label8_today").text(table[0].SUM)
    $("#labelTel").text(table2.length)

    if (table[0].SUM == null) {

        for (i = 0; i < table_sub3.length; i++) {
            if ( table_sub3[0].IS_ACTIVE == 1) {
                $("#label_ser1_today").text(table_sub3[0].SER_NAME)
                $("#Label9_today").text("0 บริการ")
            }
            else {
                $("div[data-show='1']").remove();
            }
            if ( table_sub3[1].IS_ACTIVE == 1) {
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

        for (i = 0; i < table_sub3.length; i++) {

            if (table[i].SER13 != null && table_sub3[0].IS_ACTIVE == 1) {
                $("#label_ser1_today").text(table_sub3[0].SER_NAME)
                $("#Label9_today").text(table[0].SER13 + " บริการ")
            }
            else {
                $("div[data-show='1']").remove();
            }
            if (table[i].SER11 != null && table_sub3[1].IS_ACTIVE == 1) {
                $("#label_ser2_today").text(table_sub3[1].SER_NAME)
                $("#Label10_today").text(table[0].SER11 + " บริการ")
            }
            else {
                $("div[data-show='2']").remove();
            }
            if (table[i].SER21 != null && table_sub3[2].IS_ACTIVE == 1) {
                $("#label_ser3_today").text(table_sub3[2].SER_NAME)
                $("#Label11_today").text(table[0].SER21 +" บริการ")
            }
            else {
                $("div[data-show='3']").remove();
            }
            if (table[i].SER12 != null && table_sub3[3].IS_ACTIVE == 1) {
                $("#label_ser4_today").text(table_sub3[3].SER_NAME)
                $("#Label12_today").text(table[0].SER12 + " บริการ")
            }
            else {
                $("div[data-show='4']").remove();
            }


        }

    
    }
  

}
function formatNumber(number, decimalPlaces) {
    let numbers = number.torelative(decimalPlaces);
    return numbers.replace('0', 'X').replace('.', '').replace('1', 'X').replace('2', 'X').replace('4', 'X').replace('5', 'X').replace('7', 'X').replace('8', 'X').replace('9', 'X')
}
function tableload(tables, table_sub3) {
    var columns = [];
 

    $("#Label4").text(tables.length)
    if (tables[0].RES_NAME === "ไม่สนใจ") {




           
            $('#tb_1').DataTable().destroy();
            $('#tb_1').hide()
            $('#tb_11').show()
            $('#tb_11').DataTable().destroy();

            columns.push(

                {
                    data: 'ANUMBER', title: 'เบอร์โทรศัพท์', render: function (data, type, row) {
                        if (type === 'excel') {
                            return data; // ในโหมดการแสดงหรือส่งออก Excel ให้คืนค่าเบอร์โทรศัพท์แบบปกติ
                        }

                        else if (type === 'display') {
                            // ปิดเลขโทรศัพท์เพื่อแสดงเป็น ****
                            return `<span>` + '***-***-' + data.substr(data.length - 4); + `<span>`
                        }
                        return data;
                    }
                },
                { data: 'CUST_NAME', title: 'ชื่อ', render: function (data, type, row) { return `<span style='padding-right: 60px;text-align: left;display: flex;'>` + data + `<span>` } },
                { data: 'CUST_SNAME', title: 'นามสกุล', render: function (data, type, row) { return `<span style='padding-right: 60px;text-align: left;display: flex;'>` + data + `<span>` } },
                );
        for (var i = 0; i < table_sub3.length; i++) {

            if (table_sub3[i].IS_ACTIVE == 1) {
                console.log(table_sub3[i].SER_ID)
                console.log(table_sub3[i].SER_NAME)
                columns.push(

                    { data: 'SERVICE_' + table_sub3[i].SER_ID, title: "" + table_sub3[i].SER_NAME + "" },

                );
            } else {
                $("th[data-table='" + i + "']").remove()
            }
        }

            columns.push(
                { data: 'DENY_NAME', title: 'เหตุผล', render: function (data, type, row) { return `<span style='padding-right: 30px;text-align: left;display: flex;'>` + data + `<span>` } }
            );

            $('#tb_11').DataTable({

                searching: false,
                lengthChange: false,
                dom: 'Bfrtip',
                buttons: [
                    //{
                    //    extend: 'csv',
                    //    text: 'CSV',
                    //    exportOptions: {
                    //        encoding: 'utf8'
                    //    }
                    //},
                    //{
                    //    extend: 'excel',
                    //    text: 'Excel',
                    //    exportOptions: {
                    //        encoding: 'utf8'
                    //    }
                    //},
                ],
                columns:
                    columns
                ,
                data: tables
            }).draw()

        
        $("#total_l").show()
    }else {


            $('#tb_11').hide()
            $('#tb_1').show()
            $('#tb_1').DataTable().destroy();

            columns.push(

                {
                    data: 'ANUMBER', title: 'เบอร์โทรศัพท์', render: function (data, type, row) {
                        if (type === 'excel') {
                            return data; // ในโหมดการแสดงหรือส่งออก Excel ให้คืนค่าเบอร์โทรศัพท์แบบปกติ
                        }

                        else if (type === 'display') {
                            // ปิดเลขโทรศัพท์เพื่อแสดงเป็น ****
                            return `<span>` + '***-***-' + data.substr(data.length - 4); + `<span>`
                        }
                        return data;
                    }
                },
                { data: 'CUST_NAME', title: 'ชื่อ', render: function (data, type, row) { return `<span style='padding-right: 60px;text-align: left;display: flex;'>` + data + `<span>` } },
                { data: 'CUST_SNAME', title: 'นามสกุล', render: function (data, type, row) { return `<span style='padding-right: 60px;text-align: left;display: flex;'>` + data + `<span>` } },
                );
        for (var i = 0; i < table_sub3.length; i++) {
            if (table_sub3[i].IS_ACTIVE == 1) {
                columns.push(

                    { data: 'SERVICE_' + table_sub3[i].SER_ID, title: "" + table_sub3[i].SER_NAME + "" },

                );
            } else {
                $("th[data-table='" + i + "']").remove()
            }
            }
            $('#tb_1').DataTable({
                searching: false,
                lengthChange: false,
                dom: 'Bfrtip',
                buttons: [
                ],
                columns: 
                    columns
                ,
                data: tables
            }).draw()
            $("#total_l").show()
        }

    }


    let set_interval;
    function showreportToday() {
        let reson = $("#select_reson").val()
        let date = $("#date_reson").val()
        let datas = new FormData();
        if (reson === "" || reson == null) {
            reson = "01"
        }
        datas.append("res_code", reson)
        let ajax_ = $.ajax({
            url: '/FrmReportTel/showreportToday',
            processData: false,
            contentType: false,
            type: 'POST',
            data: datas,
            success: function (e) {

                table = JSON.parse(e)

                table_sub = JSON.parse(table[0])
                table_sub2 = JSON.parse(table[1])
                table_sub3 = JSON.parse(table[2])
                /*    sum(table_sub, table_sub2 , table_sub3)*/
                sum2(table_sub, table_sub2, table_sub3)

            }
        })

    }
    function btnreport_click() {
        let reson = $("#select_reson").val()
        let date =  $("#date_reson").val() 
        let datas = new FormData();
        if (reson === "" || reson == null) {
            reson = "01"
        }
        datas.append("res_code", reson)

        datas.append("Day", date)

        let ajax_ = $.ajax({
            url: '/FrmReportTel/btnReport_Click',
            processData: false,
            contentType: false,
            type: 'POST',
            data: datas,
            success: function (e) {
                if (e === null || e === "") {
                    btnreport_click();
                }
                if (e === "ไม่มีข้อมูลที่คุณค้นหา") {
                    alert2("ไม่มีข้อมูลที่คุณค้นหา")
                }
                else if (e === "ไม่สามารถแสดงข้อมูลได้ เนื่องจากมีข้อผิดพลาด") {
                }
                else {
                    table = JSON.parse(e)

                    table_sub = JSON.parse(table[0])
                    table_sub2 = JSON.parse(table[1])
                    table_sub3 = JSON.parse(table[2])

                    tableload(table_sub2, table_sub3)
                    sum(table_sub, table_sub2, table_sub3)
              /*      sum2(table_sub, table_sub2, table_sub3)*/
                }
            }
        })


    }


    function fucback() {


        window.location.href = '/FrmDetail/Index'

    }


    $(document).on('load', cbostatus())
    $("#button_report").on('click', function (e) {
        btnreport_click()
    })
    $(document).on('load', showreportToday());
    $("#button_logout").on('click', function (e) { fuclogout() })
    $("#button_back").on('click', function (e) { fucback() })

    $("#year_thai").on('change', function (e) {
        let year = parseInt($("#year_thai").val());
        let Now_years = new Date().getFullYear()
        Now_years = parseInt(Now_years.toString()) + 543
        year = Now_years - year
        if (year > 55) {
            alert2("ลูกค้าอายุมากกว่า 55 ปี ไม่สามารถรับบริการได้ค่ะ");
            //$("#button_save").prop('disabled', true)
            //$("#button_save2").prop('disabled', true)
            $("#button_save").prop('disabled', false)
            $("#button_save2").prop('disabled', false)
        }
        else if (year < 15) {
            alert2("ลูกค้าอายุน้อยกว่า 15 ปี ไม่สามารถรับบริการได้ค่ะ")
            //$("#button_save").prop('disabled', true)
            //$("#button_save2").prop('disabled', true)
            $("#button_save").prop('disabled', false)
            $("#button_save2").prop('disabled', false)
        } else {
            $("#button_save").prop('disabled', false)
            $("#button_save2").prop('disabled', false)
        }
    })






$("#button_logout").on('click', fuclogout)


