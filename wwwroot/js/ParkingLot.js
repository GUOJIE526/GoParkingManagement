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
async function CreatePopup() {
    let response = await fetch('/MyParkingLot/ParkingLot/CreatePartial');
    let partialview = await response.text();
    $('.ParkCreate').html(partialview);
    $(document).off('submit', '#CreateForm').on('submit', '#CreateForm', async function (e) {
        e.preventDefault();
        let form = new FormData(this);
        let fetchresponse = await fetch($(this).attr('action'), {
            method: 'POST',
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
                $('#CreateModal').modal('hide');
                $('#ParkLotTable').DataTable().ajax.reload(null, false);
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
    $('#CreateModal').modal('show');
}
//動態加載Edit內容
async function EditPark(id) {
    console.log(id);
    let response = await fetch(`/MyParkingLot/ParkingLot/EditPartial/${id}`);
    let partialview = await response.text();
    $('.ParkEdit').html(partialview);

    $(document).off('submit', '#EditForm').on('submit', '#EditForm', async function (e) {
        e.preventDefault();
        let form = new FormData(this);
        let fetchresponse = await fetch($(this).attr('action'), {
            method: 'POST',
            body: form
        });
        if (fetchresponse.ok) {
            let result = await fetchresponse.json();
            if (result.success) {
                Swal.fire({
                    icon: "success",
                    title: "編輯成功!",
                    showConfirmButton: false,
                    timer: 1500
                });
                $('#EditModal').modal('hide');
                $('#ParkLotTable').DataTable().ajax.reload(null, false);
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
    $('#EditModal').modal('show');
}
$(document).on('click', '.EditBtn', function () {
    var id = $(this).data('id');
    EditPark(id);
})