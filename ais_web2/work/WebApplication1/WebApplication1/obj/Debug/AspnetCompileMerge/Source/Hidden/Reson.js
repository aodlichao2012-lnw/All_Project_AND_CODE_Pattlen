

function fuc_select_change_reson() {
    let variable = "data-val"
    let reson_select = $('#select_st').val()
    /*  let select = $('#browsers [value="' + reson_select + '"]').data('val')*/
    $.ajax({
        url: "/FrmDetail/cboStatus_SelectedIndexChanged?cboStatus=" + reson_select + "&res_code=" + reson_select
        , type: "GET",
        success: function (e) {
            let values = JSON.parse(e)
            console.log(values)
            if (e === "server มี ปัญหา") {
                alert2("server มี ปัญหา กรุณาติดต่อ admin");
            }
            if (values[0] === "เครือข่าย") {
                let htmls = `  <select  id="cboDeny"  style="width:200px;height:25px;" >`
                for (i = 0; i < values.length; i++) {
                    htmls += `  <option value = "` + values[i] + `" >` + values[i] + `</option>`
                }
                htmls += ` </select>`
                console.log(htmls)
                $("#cboDeny").html(htmls)
            }
            else {
                let htmls = `  <select  id="select_rs" style="width:200px;height:25px;" >   <option value="" >
                                    -- โปรดเลือกเหตุผล --
                                </option>`
                for (i = 0; i < values.length; i++) {
                    htmls += `  <option value="` + values[i].DENY_CODE + `" data-val="` + values[i].DENY_CODE + `" >` + values[i].DENY_NAME + `</option>`
                }
                htmls += ` </select>`
                console.log(htmls)
                $("#select_rs").html(htmls)
                if (reson_select === "01") {
                    $("#btn_add_ser").show()
                } else {
                    $("#btn_add_ser").hide()
                }
            }
        }
    })
}

$("#select_st").on('change', function (e) {
    fuc_select_change_reson()
})