$('#reservationTable').dataTable({
    ajax: {
        url: '/MyReservation/Reservation/IndexJson',
        type: 'GET',
        dataSrc: "",  // 如果你的 JSON 根是數組，否則用 dataSrc: "data"
        datatype: 'Json'
    },
    pagingType: "full_numbers",
    fixedHeader: {
        header: true
    },
    scrollY: 500,
    language: {
        url: '//cdn.datatables.net/plug-ins/2.1.5/i18n/zh-HANT.json',
    },
    columns: [
        { data: 'reservationTime' },
        { data: 'validUntil' },
        { data: 'depositStatus' },
        { data: 'isOverdue' },
        { data: 'isCanceled' },
        { data: 'notificationStatus' },
        { data: 'amount' },
        { data: 'isRefoundDeposit' },
        { data: 'isFinish' },
        { data: 'carId' },
        { data: 'lotId' },
        {
            data: 'resId',  
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
    ]
});

//動態加載Create內容
$('.CreateBtn').on('click', async function () {
    let response = await fetch('/MyReservation/Reservation/CreatePartial');
    let partialView = await response.text();  // 讀取返回的 HTML 內容
    $('.CreateRes').html(partialView);  // 動態插入到指定區域

    // 綁定表單提交事件
    $(document).off('submit', '#CreateForm').on('submit', '#CreateForm', async function (e) {
        e.preventDefault(); // 防止默認提交行為

        // 將表單序列化
        let formData = new FormData(this);  // 使用 FormData 獲取表單數據

        // 使用 fetch 發送 POST 請求
        let fetchResponse = await fetch($(this).attr('action'), {
            method: 'POST',
            body: formData,
        });

        if (fetchResponse.ok) {
            let result = await fetchResponse.json(); // 解析為 JSON

            if (result.success) {
                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "新增項目成功!",
                    showConfirmButton: false,
                    timer: 1500
                });
                $('#ResModal').modal('hide');  // 隱藏模態框
                $('#reservationTable').DataTable().ajax.reload(null, false);  // 刷新表格資料
            } else {
                // 處理失敗，顯示錯誤
                Swal.fire({
                    icon: "error",
                    title: "Oops...",
                    text: result.errors ? result.errors.join(", ") : result.message,
                });
            }
        } else {
            // 處理 AJAX 請求錯誤
            Swal.fire({
                icon: "error",
                title: "Oops...",
                text: "請求發生錯誤，請稍後再試",
            });
        }
    });

    // 顯示模態框
    $('#ResModal').modal('show');
});

//動態加載Edit內容
$(document).on('click', '.EditBtn',  async function () {
    var id = $(this).data('id'); // 獲取該筆資料的 ID
    let response = await fetch(`/MyReservation/Reservation/EditPartial/${id}`);
    let partialView = await response.text();  // 讀取返回的 HTML 內容
    $('.EditRes').html(partialView);  // 動態插入到指定區域
    console.log(id);
    // 綁定表單提交事件
    $(document).off('submit', '#EditForm').on('submit', '#EditForm', async function (e) {
        e.preventDefault(); // 防止默認提交行為

        // 將表單序列化
        let formData = new FormData(this);  // 使用 FormData 獲取表單數據

        // 使用 fetch 發送 POST 請求
        console.log($(this).attr('action'));

        let fetchResponse = await fetch($(this).attr('action'), {
            method: 'POST',
            body: formData,
            headers: {
                'X-Requested-With': 'XMLHttpRequest',  // 用來標示這是一個 AJAX 請求
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()  // 防護 token
            }
        });

        if (fetchResponse.ok) {
            let result = await fetchResponse.json(); // 解析為 JSON

            if (result.success) {
                Swal.fire({
                    icon: "success",
                    title: "編輯成功!",
                    showConfirmButton: false,
                    timer: 1500
                });
                $('#EditModal').modal('hide');  // 隱藏模態框
                $('#reservationTable').DataTable().ajax.reload(null, false);  // 刷新表格資料
            } else {
                // 處理失敗，顯示錯誤
                Swal.fire({
                    icon: "error",
                    title: "Oops...",
                    text: result.errors ? result.errors.join(", ") : result.message,
                });
            }
        } else {
            // 處理 AJAX 請求錯誤
            Swal.fire({
                icon: "error",
                title: "Oops...",
                text: "請求發生錯誤，請稍後再試",
            });
        }
    });

    // 顯示模態框
    $('#EditModal').modal('show');
});


//$(document).on('click', '.EditBtn', function () {
//    var id = $(this).data('id'); // 獲取該筆資料的 ID
//    $('.EditRes').load(`/MyReservation/Reservation/Edit/${id} #EditForm`, function () {
//        function selfsubmitEvent() {
//            $(document).off('submit', '#EditForm').one('submit', '#EditForm', function (e) {
//                e.preventDefault(); // 防止默認提交行為
//                $.ajax({
//                    url: $(this).attr('action'), // 表單的 action URL
//                    type: 'POST',
//                    data: $(this).serialize(), // 將表單數據序列化
//                    dataType: 'Json',
//                    success: function (response) {
//                        if (response.success) {
//                            // 如果成功，關閉模態視窗，並刷新頁面或局部刷新
//                            $('#EditModal').modal('hide');
//                            $('#reservationTable').DataTable().ajax.reload(null, false); // 刷新特定部分
//                        } else {
//                            // 如果失敗，動態替換SweetAlert，顯示錯誤
//                            Swal.fire({
//                                icon: "error",
//                                title: "Oops...",
//                                text: "請填入必填欄位!",
//                            });
//                            selfsubmitEvent();
//                        }
//                    }
//                });
//            });
//        }
//        selfsubmitEvent();
//        $('#EditModal').modal('show');
//    });
//});

//function Delete(id) {
//    Swal.fire({
//        title: "確認刪除？",
//        text: "此操作無法恢復！",
//        icon: "warning",
//        showCancelButton: true,
//        confirmButtonColor: "#3085d6",
//        cancelButtonColor: "#d33",
//        confirmButtonText: "是的，刪除它！"
//    }).then((result) => {
//        if (result.isConfirmed) {
//            $.ajax({
//                url: `/MyReservation/Reservation/Delete/${id}`,  // 刪除請求的 URL
//                type: 'POST',
//                data: {
//                    __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
//                },
//                success: function (response) {
//                    if (response.success) {
//                        Swal.fire({
//                            icon: "success",
//                            title: "刪除成功",
//                            showConfirmButton: false,
//                            timer: 1500
//                        });
//                        // 局部刷新 DataTable
//                        $('#reservationTable').DataTable().ajax.reload(null, false);
//                    } else {
//                        Swal.fire({
//                            icon: "error",
//                            title: "Oops...",
//                            text: response.message
//                        });
//                    }
//                },
//                error: function (xhr, status, error) {
//                    Swal.fire({
//                        icon: "error",
//                        title: "刪除失敗",
//                        text: "請檢查您的網絡連接或重新嘗試。"
//                    });
//                }
//            });
//        }
//    });
//}
