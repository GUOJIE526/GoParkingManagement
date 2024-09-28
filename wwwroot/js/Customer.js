$(document).ready(function () {
    $('#CustTable').dataTable({
        "pagingType": "full_numbers",
        fixedHeader: {
            header: true
        },
        scrollY: 650,
        language: {
            url: '//cdn.datatables.net/plug-ins/2.1.5/i18n/zh-HANT.json',
        },
    });
});



//動態加載Edit內容
$('.customerEditBtn').on('click', function () {
    var id = $(this).data('id');
    console.log(id);
    $('#Editbody').load(`/MyCustomer/Customer/Edit/${id} #customerEditForm`, function () {
        $(document).on('submit', '#customerEditForm', function (e) {
            e.preventDefault(); // 防止默認提交行為
            $.ajax({
                url: $(this).attr('action'), // 表單的 action URL
                type: 'POST',
                data: $(this).serialize(), // 將表單數據序列化
                dataType: 'Json',
                success: function (response) {
                    if (response.success) {
                        // 如果成功，關閉模態視窗，並刷新頁面或局部刷新
                        $('#customerEditModal').modal('hide');
                        location.reload(); // 刷新特定部分
                    } else {
                        // 如果失敗，動態替換SweetAlert，顯示錯誤
                        Swal.fire({
                            icon: "error",
                            title: "Oops...",
                            text: "請填入必填欄位!",
                        });
                    }
                }
            });
        });
        $('#customerEditModal').modal('show');
    });
});

//動態加載Create內容
$('#customerCreateBtn').on('click', function () {
    $('#Createbody').load('/MyCustomer/Customer/Create #customerCreateForm', function () {
        $(document).on('submit', '#customerCreateForm', function (e) {
            e.preventDefault(); // 防止默認提交行為
            $.ajax({
                url: $(this).attr('action'), // 表單的 action URL
                type: 'POST',
                data: $(this).serialize(), // 將表單數據序列化
                dataType: 'Json',
                success: function (response) {
                    if (response.success) {
                        // 如果成功，關閉模態視窗，並刷新頁面或局部刷新
                        $('#customerCreateModal').modal('hide');
                        location.reload(); // 刷新特定部分
                    } else {
                        // 如果失敗，動態替換SweetAlert，顯示錯誤
                        Swal.fire({
                            icon: "error",
                            title: "Oops...",
                            text: "請填入必填欄位!",
                        });
                    }
                }
            });
        });
        $('#customerCreateModal').modal('show');
    });
});