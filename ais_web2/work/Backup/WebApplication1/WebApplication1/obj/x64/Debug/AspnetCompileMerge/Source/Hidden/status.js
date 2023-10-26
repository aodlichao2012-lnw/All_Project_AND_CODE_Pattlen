let intervalId = null;
$(document).on('load', fucshowtel());
$(document).on('load', getstatus());
$("#btn_save").on('click', function (e) { fucsave() })
function getfuc() {
        $.ajax({
            url: '/FrmStatus/FrmStatus_Load',
            type: "GET",
            data: null,
            success: function (e) {
             
                if (e === "Busy") {
                   fucshowtel()
                    $("#status").text(e).css("color", "red")
                    return e
                } else if (e === "Not Login") {
                    $("#status").text(e).css("color", "gray")
                    return e
                } else {
                    $("#status").text(e).css("color", "green")
                    return e
                }
             
            }
        })
}
function getstatus() {
    intervalId = setInterval(getfuc, 1000)

}
function fucshowtel() {
        $.ajax({
            url: "/FrmDetail/GetPhone"
            , type: "GET",
            data: null,
            success: function (e) {
                if (e === "") {

                } else {
                    $("#txt_tel").val(e)
                }


            }
        })
}
function fucsave() {

        let SERVICE2_ = "#lbSERVICE_";
        let service = $("[id]").filter(function (e) {
            return this.id === SERVICE2_
        })
        let datas = new FormData();
        for (i = 0; i < service.prevObject.length; i++) {

            if (service.prevObject[i].id.includes("lbSERVICE_") === true) {
                console.log(service.prevObject[i].id.replace('lb', ''))
                datas.append(service.prevObject[i].id.replace('lb', ''), service.prevObject[i].checked)
            }

        }

        let reson = $("#select_st").val()
    let reson_2 = $("#select_rs").find(":selected").text()
    let reson_code = $("#select_rs").val()
        let current_date = $("#current_date").val()
        let cname = $("#cname").val()
        let cbocity = $("#cbocity").val()
        let cboDeny = $("#cboDeny").val()
        let csname = $("#csname").val()
        let txt_tel = $("#txt_tel").val()
        let date1 = $("#date1").val().split('/')
        let year = date1[2]
        let mouth = date1[1]
        let day = date1[0]

        datas.append("txtYear", year)
        datas.append("cboMouth", mouth)
        datas.append("cboStatus", reson)
        datas.append("cboDate", day)
        datas.append("txtName", cname)
        datas.append("txtSName", csname)
        datas.append("txtTel_No", txt_tel)
        datas.append("cbocity", cbocity)
        datas.append("cboDeny", cboDeny)
        datas.append("strDeny", reson_2)
    datas.append("strDenycode", reson_code)
        /*    datas.append("cboDeny", current_date)*/

        $.ajax({
            url: '/FrmDetail/btnSave_Click',
            contentType: false,
            processData: false,
            type: 'POST',
            data: datas
            , success: function (e) {
                getstatus();
                alert(e)
                $("#sms_select").hide()
            }
        })
    }



