let tables;
$('#tb_2 tbody').on('click', 'td', function () {
    let cell = $(this);
    var row = cell.closest('tr');
    var button = row.find('button');
    var status = button.data('status');
    var anumber = button.data('anumber');
    console.log(status)
    console.log(anumber)
    let datas = new FormData();
    datas.append('status', status)
    datas.append('anumber', anumber)
   let ajax_ =  $.ajax({
        url: '/FrmDetail/FrmDetail_Load',
        type: 'POST',
        contentType: false,
        data: datas,
        processData: false,
        success: function (e) {
            if (e === "server มี ปัญหา") {
                alert2("server มี ปัญหา กรุณาติดต่อ admin");
            }

            window.location.href = '/FrmDetail/Index'
         
        }
    })
   
})

$("#button_Serach_from_SearchNumber").on('click', function (e) {
    gettable();
})


    $(document).on('load', gettable())

    function gettable() {

   $.ajax({
            url: '/FrmSearchNumber/FrmSearchNumber_Load?textbox_search_number=' + $("#textbox_search_number").val(),
            type: 'GET',
            data: null,
            success: function (e) {
                console.log(e)
                let data_json = JSON.parse(e)
                console.log(data_json)
                    tables =   $("#tb_2").DataTable({
                    searching: false,
                        lengthChange: false,
                        destroy: true, 
                    columns: [
                        {
                            data: 'anumber', render: function (data, type, row) {

                                return `<button id="button_edit" data-anumber="` + row["anumber"] + `" data-status="` + row["status"] + `">แก้ไข</button>`
                            }
                        },
                        {
                            data: 'anumber', Title: 'หมายเลข        ', render: function (data, type, row) { /*return `<span>` + data.substring(0, 3) + `-XXX-XXXX<span>`*/
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
                        { data: 'status', Title: 'สถานะ        ' },
                        { data: 'cust_name', Title: 'ชื่อ            ' },
                        { data: 'cust_sname', Title: 'นามสกุล        ' },
                        { data: 'cust_sex', Title: 'เพศ' },
                        { data: 'lead_call_date', Title: 'lead_call_date        ' }

                    ],
                    data: data_json
                });
            }
       })
    }


