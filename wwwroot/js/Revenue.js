// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.font.family = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.font.color = '#858796';

var ctxPie = document.getElementById("RevenuePieChart").getContext('2d');

$.ajax({
    url: revenueUrl, // 請求伺服器端的 JSON 資料
    type: "get",
    dataType: "json" // 設定返回的資料類型為 JSON
}).done(function (data) {
    // 解析伺服器回傳的資料
    var entryExitAmounts = data.entryExitAmouts;
    var reservationAmounts = data.reservationAmounts;
    var monthlyRentalAmounts = data.monthlyRentalAmounts;

    // 用 Chart.js 繪製圓餅圖
    var myPieChart = new Chart(ctxPie, {
        type: 'pie', // 設定圖表類型為圓餅圖
        data: {
            labels: ['停車收益', '預定違約金', '月租收益'], // 標籤
            datasets: [{
                label: '收益貢獻', // 數據標籤
                data: [entryExitAmounts, reservationAmounts, monthlyRentalAmounts], // 將伺服器回傳的值放入數據
                backgroundColor: [
                    'rgb(255, 99, 132)',
                    'rgb(54, 162, 235)',
                    'rgb(255, 205, 86)'
                ], // 每個區域的顏色
                borderWidth: 1, // 邊框寬度
                hoverBorderColor: "rgba(234, 236, 244, 1)",
            }]
        },
        options: {
            maintainAspectRatio: false,
            tooltips: {
                backgroundColor: "rgb(255,255,255)",
                bodyFontColor: "#858796",
                borderColor: '#dddfeb',
                borderWidth: 1,
                xPadding: 15,
                yPadding: 15,
                displayColors: false,
                caretPadding: 10,
            },
            legend: {
                display: false
            },
            cutoutPercentage: 80,
        }
    });
}).fail(function (err) {
    alert("Error: " + err.statusText); // 處理請求錯誤
});

/*
var ctxline = document.getElementById("lineChartE").getContext('2d');

$.ajax({
    url: revenueEntryExitUrl,
    type: "get",
    dataType: "json"
}).done(function (data) {
    var labels = []; var dataresults = [];
    $.each(data, function (index, item) {
        var formattedDate = new Date(item.daytime).toISOString().split('T')[0];
        labels.push(formattedDate); dataresults.push(item.amount);
    })
    var mylineChart = new Chart(ctxline, {
        type:'line',
        data: {
            labels: labels,
            datasets: [{
                label: '停車費用',
                data: dataresults,
                borderWidth: 1,
                fill: false,
                borderColor: 'rgb(75, 192, 192)',
                tension: 0.1,
            }]
        },
    });
}).fail(function (err) { alert(err.statusText); });

var ctxline2 = document.getElementById("lineChartM").getContext('2d');

$.ajax({
    url: revenueMonthlyUrl,
    type: "get",
    dataType: "json"
}).done(function (data) {
    var labels = []; var dataresults = [];
    $.each(data, function (index, item) {
        var formattedDate = new Date(item.daytime).toISOString().split('T')[0];
        labels.push(formattedDate); dataresults.push(item.amount);
    })
    var mylineChart = new Chart(ctxline2, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: '月租費用',
                data: dataresults,
                borderWidth: 1,
                fill: false,
                borderColor: 'rgb(75, 192, 192)',
                tension: 0.1,
            }]
        },
    });
}).fail(function (err) { alert(err.statusText); });
*/

// 新的方法 盡量用減少Code的使用

// Call the function to generate options when the page loads
$(function () {

    function generateMonthOptions(ID) {
        var select = document.getElementById(ID);
        var today = new Date();
        var year = today.getFullYear();
        var currentMonth = today.getMonth() + 1; // getMonth() returns 0-based month, so add 1

        for (var month = 1; month <= currentMonth; month++) {
            var monthStr = month.toString().padStart(2, '0'); // Format month to be two digits (e.g., "01")
            var option = document.createElement('option');
            option.value = year + '-' + monthStr;
            option.textContent = year + '年 ' + monthStr + '月';
            select.appendChild(option);
        }
        select.selectedIndex = select.options.length - 1; //預設先使用selectlist最後一個
    }

    //存全部chart
    var chartInstances = {};

    function fetchAndUpdateChart(month, ChartID, creatChart, typeurl) {

        //找id後destroy chartInstances中的圖表
        var chartID = ChartID.id;
        if (chartInstances[chartID]) {
            chartInstances[chartID].destroy();
        };

        var canvas = $('#' + chartID)[0];


        $.ajax({
            url: typeurl,
            type: "get",
            data: { month: month }, // 傳遞選擇的月份
            dataType: "json"
        }).done(function (data) {
            var labels = [];
            var dataresults = [];
            $.each(data, function (index, item) {
                var formattedDate = new Date(item.daytime).toISOString().split('T')[0];
                labels.push(formattedDate);
                dataresults.push(item.amount);
            });
            const ctx = document.getElementById(chartID).getContext('2d');

            const totalDuration = 2500; // 總持續時間
            const delayBetweenPoints = totalDuration / data.length; // 每個點的延遲

            // 獲取前一個 Y 值
            const previousY = (ctx) => {
                if (ctx.index === 0) return 0; // 第一個點的初始值
                return ctx.chart.getDatasetMeta(ctx.datasetIndex).data[ctx.index - 1].getProps(['y'], true).y;
            };

            const animation = {
                x: {
                    type: 'number',
                    easing: 'linear',
                    duration: delayBetweenPoints,
                    from: NaN,
                    delay(ctx) {
                        if (ctx.type !== 'data' || ctx.xStarted) {
                            return 0;
                        }
                        ctx.xStarted = true;
                        return ctx.index * delayBetweenPoints;
                    }
                },
                y: {
                    type: 'number',
                    easing: 'linear',
                    duration: delayBetweenPoints,
                    from: previousY,
                    delay(ctx) {
                        if (ctx.type !== 'data' || ctx.yStarted) {
                            return 0;
                        }
                        ctx.yStarted = true;
                        return ctx.index * delayBetweenPoints;
                    }
                }
            };
            chartInstances[chartID] = new Chart(ChartID, {
                type: 'line',
                data: {
                    labels: labels,
                    datasets: [{
                        label: '收益',
                        data: dataresults,
                        borderWidth: 1,
                        borderColor: 'rgba(75, 192, 192, 1)',
                        backgroundColor: 'rgba(75, 192, 192, 0.2)',
                        fill:true
                    }]
                },
                options: {
                    //animation,
                    scales: {
                        x: {
                            type: 'category', // 如果 datetime 是標籤類型
                            title: {
                                display: true,
                                text: '日期'
                            },
                            ticks: {
                                maxRotation: 90,
                                minRotation: 45
                            }
                        },                        
                        y: {
                            beginAtZero: true,
                            title: {
                                display: true,
                                text: '金額'
                            }
                        }
                        
                    }                    
                }
            });
        }).fail(function (err) {
            alert(err.statusText);
        });
    };


    //生成select + chart的方法
    function createChart(selectlist, chartID, createchart, typeselectlist) {
        generateMonthOptions(selectlist.id);// 調用上面的方法 新增一個selectlist
        var typeurl;
        //先偵測要生成的type
        //初始
        //GPT說寫這樣比較好
        var typeurl = typeselectlist.value == "MonthlyRental" ? revenueGetMonthlyRentalMonthlyUrl : revenueGetEntryExitMonthlyUrl;

        /* 他說不要用這個 一直跑判斷
        if (typeurl == null) {
            //console.log("我在初始狀態喔" + typeselectlist.value)
            //console.log(typeselectlist.id)
            if (typeselectlist.value == "MonthlyRental") {
                typeurl = revenueGetMonthlyRentalMonthlyUrl;
            }
            else {
                typeurl = revenueGetEntryExitMonthlyUrl;
            }
        }
        */

        //監聽變化
        $('#' + typeselectlist.id).on('change', function () {
            //console.log("我有改變喔" + this.value)
            if (this.value == "MonthlyRental") {
                //console.log("我要生成月租的")
                typeurl = revenueGetMonthlyRentalMonthlyUrl;
                fetchAndUpdateChart(selectlist.value, chartID, createchart, typeurl);
            }
            else {
                //console.log("我要生成出入的")
                typeurl = revenueGetEntryExitMonthlyUrl;
                fetchAndUpdateChart(selectlist.value, chartID, createchart, typeurl);
            }
        });

        // 初始化圖表
        fetchAndUpdateChart(selectlist.value, chartID, createchart, typeurl);

        // 監聽選擇變化
        $('#' + selectlist.id).on('change', function () {
            var selectedMonth = $(this).val();
            fetchAndUpdateChart(selectedMonth, chartID, createchart, typeurl);
        });

    }



    var ctxlineES = document.getElementById("lineChartES"); //要生成chart的canvas ID
    var mylineChartES; //生成的chart 這是保險用的 怕之後要對它做變動才弄一個參數做保留
    var selectlistEntryExit = document.getElementById("monthFilterES"); //要操控表格的selectlist ID
    var TypeselectlistEntryExit = document.getElementById("typeFilterES"); //要操控表格的Typeselectlist ID
    createChart(selectlistEntryExit, ctxlineES, mylineChartES, TypeselectlistEntryExit);


    var ctxlineMS = document.getElementById("lineChartMS");
    var mylineChartMS;
    var selectlistMonthlyRental = document.getElementById("monthFilterMS");
    var TypeselectlistMonthlyRental = document.getElementById("typeFilterMS");
    createChart(selectlistMonthlyRental, ctxlineMS, mylineChartMS, TypeselectlistMonthlyRental);
});


