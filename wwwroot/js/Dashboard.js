function getRandomColor() {
    let letters = '0123456789ABCDEF';
    let color = '#';
    for (i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
};

// Counter Number
$('.Money').each(function () {
    $(this).prop('Counter', 0).animate({
        Counter: $(this).text()
    }, {
        duration: 3000,
        easing: 'swing',
        step: function (now) {
            $(this).text(Math.ceil(now));
        }
    });
});

$('.CompeleteRate').countMe(80, 30);

let elem = document.querySelector('.progress-bar');
let width = 1;
let id = setInterval(frame, ResCompelete);
function frame() {
    if (width >= ResCompelete) {
        clearInterval(id);
    } else {
        width += 5;
        elem.style.transitionDuration = "1.3s";
        elem.style.width = width + '%';
    }
}

$.get('/Home/GetSlotData', function (data) {
    data.forEach(function (item) {
        let randomColor = getRandomColor();
        //動態創建進度條，並顯示出租率
        $('#progress-bars').append(`
            <div>
                <span>${item.lotName} (${item.percentage.toFixed(2)}%)</span>
                <div class="progress">
                    <div class="progress-bar" role="progressbar" style="width: ${item.percentage}%;background-color: ${randomColor};" aria-valuenow="${item.percentage}" aria-valuemin="0" aria-valuemax="100"></div>
                </div>
            </div>
        `);
    });
}, 'Json');
//$.get('/Home/GetSlotData', function (data) {
//    console.log(data);  // 檢查 data 是不是一個陣列

//    // 確認是陣列後再進行操作
//    if (Array.isArray(data)) {
//        data.forEach(function (item) {
//            let randomColor = getRandomColor();
//            $('#progress-bars').append(`
//                <div>
//                    <span>${item.lotName} (${item.percentage.toFixed(2)}%)</span>
//                    <div class="progress">
//                        <div class="progress-bar" role="progressbar" style="width: ${item.percentage}%;background-color: ${randomColor};" aria-valuenow="${item.percentage}" aria-valuemin="0" aria-valuemax="100"></div>
//                    </div>
//                </div>
//            `);
//        });
//    } else {
//        console.error('Returned data is not an array:', data);
//    }
//}, 'Json');

