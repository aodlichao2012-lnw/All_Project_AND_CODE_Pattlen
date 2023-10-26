/*    $("#right_bar").toggle(); */
$("#Hamberger").on('click', function (e) {

    if ($("#right_bar").css("display") === "none") {
        $("#right_bar").toggle();

        $(".grap_from_detail").css("transform", "translateX(200px)");
        $(".fiexed_report").css("transform", "translateX(200px)");
        $(".fiexed_report2").css("transform", "translateX(200px)");
        /*        $(".vector3").css("transform", "translateX(200px)");*/
        $(".home").css("transform", "translateX(200px)");
        /*            $(".label_search_report2").css("transform", "translateX(200px)");*/
        $(".label_search_report2").css("width", "100%");
    } else {
        $("#right_bar").toggle();

        $(".grap_from_detail").css("transform", "translateX(0px)");
        $(".fiexed_report").css("transform", "translateX(0px)");
        $(".fiexed_report2").css("transform", "translateX(0px)");
        /*           $(".vector3").css("transform", "translateX(0px)");*/
        $(".home").css("transform", "translateX(0px)");
        /*          $(".label_search_report2").css("transform", "translateX(0px)");*/
        $(".label_search_report2").css("width", "100%");
    }
    // ใช้ .toggle() เพื่อสลับการแสดงและการซ่อน

});
