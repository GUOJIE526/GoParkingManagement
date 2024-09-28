$(document).ready(function () {


    //自動載入預設停車場車位
    var defaultlotName = $('#selectLotName').val();
    $.ajax({
        url: '@Url.Action("GetSlotsByLotName", "MonthlyRentals", new { area = "MyMonthlyRental" })',


        // 使用對應的 action 路徑
        type: 'GET',
        data: { lotName: defaultlotName },
        success: function (data) {
            // 更新車位下拉選單
            var selectSlotNumber = $('#SelectSlotNumber');

            //將所有車位號碼加到下拉選項
            $.each(data, function (index, item) {
                console.log(data);
                selectSlotNumber.append($('<option>', {
                    value: item.value,
                    text: item.text
                }));
            });
        }

    });


    $('#selectLotName').change(function () {
        var lotName = $(this).val();

        // 發送AJAX請求獲取相應的車位號碼列表
        $.ajax({
            url: '@Url.Action("GetSlotsByLotName", "MonthlyRentals", new { area = "MyMonthlyRental" })',


            // 使用對應的 action 路徑
            type: 'GET',
            data: { lotName: lotName },
            success: function (data) {
                // 更新車位下拉選單
                var selectSlotNumber = $('#SelectSlotNumber');
                selectSlotNumber.empty(); // 清空現有選項

                //將所有車位號碼加到下拉選項
                $.each(data, function (index, item) {
                    console.log(data);
                    selectSlotNumber.append($('<option>', {
                        value: item.value,
                        text: item.text

                    }));
                });
            }

        });
    });
});