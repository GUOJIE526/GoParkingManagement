//JQuery dataTable
$('#ParkLotTable').dataTable({
    ajax: {
        url: "/MyParkingLot/ParkingLot/IndexJson",
        type: 'GET',
        dataSrc: "",  // 如果你的 JSON 根是數組，否則用 dataSrc: "data"
        datatype: 'Json'
    },
    columns: [
        { data: 'lotName' },
        { data: 'lotAddress' },
        { data: 'qty' },
        { data: 'etcqty' },
        { data: 'monqty' },
        { data: 'isResStatus' },
        { data: 'isMonStatus' },
        { data: 'contract' },
        {
            data: 'lotId',
            render: function (data) {
                return `
                    <div class = "text-nowrap">
                        <a data-id="${data}" class="EditBtn btn btn-info" data-bs-toggle="modal" data-bs-target="#EditModal">
                            <i class="fa-solid fa-pen-to-square fa-beat"></i>
                        </a>
                    </div>
                `;
            }
        }
    ],
    pagingType: "full_numbers",
    fixedHeader: {
        header: true
    },
    scrollY: 550,
    language: {
        url: '//cdn.datatables.net/plug-ins/2.1.5/i18n/zh-HANT.json',
    },
});


//動態加載Create內容
$('#CreateBtn').on('click', function () {
    $('.ParkCreate').load('/MyParkingLot/ParkingLot/Create #CreateForm', function () {
        function submiEvent() {
            $(document).one('submit', '#CreateForm', function (e) {
                e.preventDefault(); // 防止默認提交行為
                var formData = new FormData(this);
                $.ajax({
                    url: $(this).attr('action'), // 表單的 action URL
                    type: 'POST',
                    data: formData, // 將表單數據序列化
                    contentType: false,
                    processData: false,
                    dataType: 'Json',
                    success: function (response) {
                        console.log(response);
                        if (response.success) {
                            // 如果成功，關閉模態視窗，並刷新頁面或局部刷新
                            console.log(response);
                            $('#CreateModal').modal('hide');
                            location.reload(); // 刷新特定部分
                        } else {
                            // 如果失敗，動態替換SweetAlert，顯示錯誤
                            Swal.fire({
                                icon: "error",
                                title: "Oops...",
                                text: "請填入必填欄位!",
                            });
                            submiEvent();
                        }
                    }
                });
            });
        }
        submiEvent();
        $('#CreateModal').modal('show');
    });
});
//動態加載Edit內容
$(document).on('click', '#EditBtn', function () {
    var id = $(this).data('id'); // 獲取該筆資料的 ID
    $('.modal-Editbody').load(`/MyParkingLot/ParkingLot/Edit/${id} #EditForm`, function () {
        function EditSubmit() {
            $(document).one('submit', '#EditForm', function (e) {
                e.preventDefault(); // 防止默認提交行為
                var formData = new FormData(this);
                $.ajax({
                    url: $(this).attr('action'), // 表單的 action URL
                    type: 'POST',
                    data: formData, // 將表單數據序列化
                    contentType: false,
                    processData: false,
                    dataType: 'Json',
                    success: function (response) {
                        if (response.success) {
                            // 如果成功，關閉模態視窗，並刷新頁面或局部刷新
                            $('#EditModal').modal('hide');
                            location.reload(); // 刷新特定部分
                        } else {
                            // 如果失敗，動態替換SweetAlert，顯示錯誤
                            Swal.fire({
                                icon: "error",
                                title: "Oops...",
                                text: "請填入必填欄位!",
                            });
                            EditSubmit();
                        }
                    }
                });
            });
        }
        EditSubmit();
        $('#EditModal').modal('show');
    });
});
