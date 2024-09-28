$('#CustTable').dataTable({
    ajax: {
        url: "/MyCustomer/Customer/IndexJson",
        type: 'GET',
        dataSrc: "",  // 如果你的 JSON 根是數組，否則用 dataSrc: "data"
        datatype: 'Json'
    },
    pagingType: "full_numbers",
    fixedHeader: {
        header: true
    },
    language: {
        url: '//cdn.datatables.net/plug-ins/2.1.5/i18n/zh-HANT.json',
    },
    columns: [
        { data: 'username', width: '10%' },
        { data: 'password', width: '10%' },
        { data: 'email', width: '10%' },
        { data: 'phone', width: '10%' },
        { data: 'blackCount', width: '10%' },
        { data: 'isBlack', width: '10%' },
        {
            data: 'userId', width: '10%', render: function (data) {
                return `
                    <div>
                        <a data-id="${data}" class="EditBtn btn btn-info" data-bs-toggle="modal" data-bs-target="#EditCust">
                            <i class="fa-solid fa-pen-to-square fa-beat"></i>
                        </a>
                    </div>
                `;
            }
        }
    ]
});



//動態加載Edit內容
$(document).on('click', '.EditBtn', async function () {
    var id = $(this).data('id'); // 獲取該筆資料的 ID
    let response = await fetch(`/MyCustomer/Customer/EditPartial/${id}`);
    let partialview = await response.text();
    $('.Editbody').html(partialview);
    console.log(id);

    $(document).off('submit', '#EditForm').on('submit', '#EditForm', async function (e) {
        e.preventDefault();
        let form = new FormData(this);
        let fetchresonse = await fetch($(this).attr('action'), {
            method: "POST",
            body: form,
        });
        if (fetchresonse.ok) {
            let result = await fetchresonse.json();
            if (result.success) {
                Swal.fire({
                    icon: "success",
                    title: "編輯成功!",
                    showConfirmButton: false,
                    timer: 1500
                });
                $('#EditCust').modal('hide');
                $('#CustTable').DataTable().ajax.reload(null, false);
            } else {
                Swal.fire({
                    icon: "error",
                    title: "Oops...",
                    text: result.errors ? result.errors.join(", ") : result.message,
                });
            }
        } else {
            Swal.fire({
                icon: "error",
                title: "Oops...",
                text: "請求發生錯誤，請稍後再試",
            });
        }
    });
    $('#EditCust').modal('show');
});

//$('.EditBtn').on('click', function () {
//    var id = $(this).data('id');
//    $('#Editbody').load(`/MyCustomer/Customer/Edit/${id} #customerEditForm`, function () {
//        $(document).on('submit', '#customerEditForm', function (e) {
//            e.preventDefault(); // 防止默認提交行為
//            $.ajax({
//                url: $(this).attr('action'), // 表單的 action URL
//                type: 'POST',
//                data: $(this).serialize(), // 將表單數據序列化
//                dataType: 'Json',
//                success: function (response) {
//                    if (response.success) {
//                        // 如果成功，關閉模態視窗，並刷新頁面或局部刷新
//                        $('#customerEditModal').modal('hide');
//                        location.reload(); // 刷新特定部分
//                    } else {
//                        // 如果失敗，動態替換SweetAlert，顯示錯誤
//                        Swal.fire({
//                            icon: "error",
//                            title: "Oops...",
//                            text: "請填入必填欄位!",
//                        });
//                    }
//                }
//            });
//        });
//        $('#customerEditModal').modal('show');
//    });
//});

//動態加載Create內容
$('#CreateBtn').on('click', async function () {
    let response = await fetch('/MyCustomer/Customer/CreatePartial');
    let partialview = await response.text();
    $('.Createbody').html(partialview);

    $(document).off('submit', '#CreateForm').on('submit', '#CreateForm', async function (e) {
        e.preventDefault();
        let form = new FormData(this);
        let fetchresponse = await fetch($(this).attr('action'), {
            method: "POST",
            body: form
        });
        if (fetchresponse.ok) {
            let result = await fetchresponse.json();
            if (result.success) {
                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "新增項目成功!",
                    showConfirmButton: false,
                    timer: 1500
                });
                $('#CreateCust').modal('hide');
                $('#CustTable').DataTable().ajax.reload(null, false);
            } else {
                Swal.fire({
                    icon: "error",
                    title: "Oops...",
                    text: result.errors ? result.errors.join(", ") : result.message,
                });
            }
        } else {
            Swal.fire({
                icon: "error",
                title: "Oops...",
                text: "請求發生錯誤，請稍後再試",
            });
        }
    });
    $('#CreateCust').modal('show');
});