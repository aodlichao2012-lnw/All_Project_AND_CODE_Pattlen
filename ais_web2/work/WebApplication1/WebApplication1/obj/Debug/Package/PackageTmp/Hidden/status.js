

let telephone_isSave = '';
let status_busy = '';
$(window).on('load', function (e) {
    $("#txt_tel").attr('disabled', true)
})


const regex = /^[0-9]+$/;


$("#button_save").on('click', function (e) { fucsave(); showreportToday(); })
$("#button_save2").on('click', function (e) { fucsave(); showreportToday(); })

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
