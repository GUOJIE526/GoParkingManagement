function previewImage(inputFile) {
    if (inputFile.files[0]) {
        //user有上傳我才要處理
        var allowTypes = "image.*"; //限制副檔名
        var file = inputFile.files[0];
        if (file.type.match(allowTypes)) {
            //預覽
            var reader = new FileReader();
            reader.onload = function (e) {
                //JS沒有prev可以用
                $(".Picture").prev().find("img").attr("src", e.target.result);
                $(".Picture").prev().find("img").attr("title", file.name);

            };
            reader.readAsDataURL(file);
            $(".btn").prop("disabled", false);

        }
        else {
            alert("不允許的檔案上傳類型!");
            $(".btn").prop("disabled", true);
            inputFile.value = "";
            // $("#Picture").prev().attr("src", "@Url.Content("~/images/NoImage.jpg")");
            // $("#Picture").prev().attr("title", "尚無圖片");
        }
    }
}

$("#Picture").on("change", function () {
    alert("Change");
    previewImage(this);
});

