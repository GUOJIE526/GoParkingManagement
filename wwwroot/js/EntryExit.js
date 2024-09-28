$(document).ready(function () {
    $('#mymodal').on('show.bs.modal', function (event) {
        //console.log("我有被按到");                
        var button = $(event.relatedTarget); // 觸發modal的按鈕
        var id = button.data('id'); // 從按鈕中獲取資料ID
        var action = button.data('action'); // 從按鈕中獲取動作 (details/edit/create等)
        var title = button.data('title')

        // 清空 modal 的內容
        var modal = $(this);
        modal.find('.modal-body').empty(); // 清除現有內容

        if (action == "ShowPicturePartial") {
            $('#modalid').attr('class', 'modal-dialog modal-lg');
        }
        else {
            $('#modalid').attr('class', 'modal-dialog modal-xl');// 改為大尺寸
        }
        $('#exampleModalLabel').text(title)
        var modal = $(this);
        // 根據不同的動作，載入對應的 Partial View
        var url = 'EntryExitManagement/' + action + '/' + id;
        modal.find('.modal-body').load(url); // 動態加載Partial View
    });


    $(document).on("change", "#Picture", function () {
        previewImage(this);
    });

    $('#myDataTableId').DataTable({
        ajax: {
            url: 'EntryExitManagement/GetData',
            type: 'GET',
            dataSrc: '',
            datatype: 'json',
        },
        scrollY: 500,
        language: {
            url: '//cdn.datatables.net/plug-ins/2.1.6/i18n/zh-HANT.json',
        },
        columns: [
            { data: 'parktype' },
            {
                data: 'licensePlatePhoto',
                render: function (data, type, row) {
                    return '<div class="text-center">' +
                        '<button class="btn btn-success preview" data-bs-toggle="modal" data-bs-target="#mymodal" data-id="' + row.entryexitId + '" data-action="ShowPicturePartial" data-title="預覽圖片">預覽</button>' +
                        '</div>';
                }
            },
            {
                data: 'carId', // 因為我們將要自定義內容，所以設為 null
                render: function (data, type, row) {
                    return row.carId + '<br>車主: ' + row.userName; // 同時顯示 carId 和 userName
                }
            },
            { data: 'entryTime' },
            { data: 'licensePlateKeyinTime' },
            { data: 'amount' },
            { data: 'exitTime' },
            { data: 'paymentStatus' },
            { data: 'paymentTime' },
            { data: 'validTime' },
            { data: 'lotName' }, // 修改 LotId 為 lotName
            { data: 'isFinish' },
            {
                data: 'entryexitId',
                render: function (data) {
                    return '<div class="text-center">' + '<a class="EditBtn btn btn-warning preview" data-bs-toggle="modal" data-bs-target="#mymodal" data-id="' + data + '" data-action="EditPartial" data-title="編輯資料">編輯</a>' + '</div>';
                }
            }
        ]
    });

    $(document).on('submit', '#editForm', function (event) {
        event.preventDefault(); // 防止表單的默認提交

        var form = $(this);
        var actionUrl = form.attr('action'); // 獲取表單的提交 URL
        var formElement = form[0];
        var formData = new FormData(formElement);

        $.ajax({
            url: form.attr('action'), // 取得表單 action 的 URL
            type: 'POST',
            data: formData,
            datatype: 'Json',
            processData: false, // 不序列化數據
            contentType: false, // 設定為 false 來讓 jQuery 設定 content-type
        }).done(response => {
            if (response.success) {
                Swal.fire({
                    title: "成功!",
                    text: "資料已成功更新!",
                    icon: "success",
                    confirmButtonText: 'OK'
                })
                $('#mymodal').modal('hide');
                $('#myDataTableId').DataTable().ajax.reload();
            } else {
                Swal.fire({
                    title: '失敗!',
                    text: response.message,
                    icon: 'error',
                    confirmButtonText: 'OK'
                })
            }
        }).fail(error => {
            Swal.fire('失敗', '資料更新失敗', 'error');
        });
    });

    $(document).on('submit', '#createForm', function (event) {
        event.preventDefault(); // 防止表單的默認提交

        var form = $(this);
        var actionUrl = form.attr('action'); // 獲取表單的提交 URL
        var formElement = form[0];
        var formData = new FormData(formElement);

        $.ajax({
            url: form.attr('action'), // 取得表單 action 的 URL
            type: 'POST',
            data: formData,
            datatype: 'Json',
            processData: false, // 不序列化數據
            contentType: false, // 設定為 false 來讓 jQuery 設定 content-type
        }).done(response => {
            if (response.success) {
                Swal.fire({
                    title: '成功!',
                    text: '資料已成功更新!',
                    icon: 'success',
                    confirmButtonText: 'OK'
                })
                $('#mymodal').modal('hide');
                $('#myDataTableId').DataTable().ajax.reload();
            } else {
                Swal.fire({
                    title: '失敗!',
                    text: response.message,
                    icon: 'error',
                    confirmButtonText: 'OK'
                })
            }
        }).fail(error => {
            Swal.fire('失敗', '資料更新失敗', 'error');
        });
    });
});
