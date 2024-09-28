using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyGoParking.Models;

namespace MyGoParking.Controllers
{
    public class Revenue : Controller
    {
        private readonly MyGoParkingContext _context;

        public Revenue(MyGoParkingContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var revenue = _context;
            return View(revenue);
        }

        public async Task<JsonResult> GetCustomersCount()
        {
            var query = _context.Customer.Count();

            return Json(query);
        }

        public async Task<JsonResult> GetReservationRevenue()
        {
            var query = _context.Reservation.Where(s => s.IsFinish == true && s.IsOverdue == true);

            return Json(query);
        }

        public async Task<JsonResult> GetMonthlyRentalRevenue()
        {
            var query = _context.MonthlyRental
                .Where(s => s.PaymentStatus == true)
                .GroupBy(s=>s.PaymentTime.Value.Date)
                .Select(s=>new
                {
                    Daytime = s.Key,
                    Amount = s.Sum(s => s.Amount)
                });
                
                

            return Json(query);
        }

        public async Task<JsonResult> GetEntryExitRevenue()
        {
            var query = _context.EntryExitManagement
                .Where(s => s.PaymentStatus == true)
                .GroupBy(s => s.PaymentTime.Value.Date)
                .Select(s => new 
                {
                    Daytime = s.Key,
                    Amount = s.Sum(s => s.Amount)
                });


            return Json(query);
        }

        public async Task<JsonResult> GetAllRevenue()
        {
            // 查詢 EntryExitManagement 的Amount
            var entryExitAmounts = _context.EntryExitManagement.Where(s => s.PaymentStatus == true).Select(s=>s.Amount).Sum();
            // 查詢 Reservation 的 Amount
            var reservationAmounts = _context.Reservation.Where(s => s.IsFinish == true && s.IsOverdue == true).Select(s => s.Amount).Sum();
            // 查詢 MonthlyRental 的 Amount
            var monthlyRentalAmounts = _context.MonthlyRental.Where(s => s.PaymentStatus == true).Select(s => s.Amount).Sum();
            // 將 Reservation 和 MonthlyRental 的資料整合到一個匿名物件中
            var result = new
            {
                EntryExitAmouts = entryExitAmounts,
                ReservationAmounts = reservationAmounts,
                MonthlyRentalAmounts = monthlyRentalAmounts
            };
            return Json(result);
        }

        public async Task<JsonResult> GetMonthlyRentalMonthlyData(string month)
        {
            // 解析月份參數
            if (string.IsNullOrEmpty(month) || !DateTime.TryParseExact(month + "-01", "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime startDate))
            {
                return Json(new { error = "Invalid month format" });
            }

            // 計算開始日期和結束日期
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            // 查詢資料
            var data = await _context.MonthlyRental
                .Where(p => p.PaymentStatus == true && p.PaymentTime.Value.Date >= startDate && p.PaymentTime.Value.Date <= endDate)
                .GroupBy(p => p.PaymentTime.Value.Date)
                .Select(g => new
                {
                    daytime = g.Key,
                    amount = g.Sum(p => p.Amount)
                })
                .ToListAsync();

            return Json(data);
        }


        public async Task<JsonResult> GetEntryExitMonthlyData(string month)
        {
            // 解析月份參數
            if (string.IsNullOrEmpty(month) || !DateTime.TryParseExact(month + "-01", "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime startDate))
            {
                return Json(new { error = "Invalid month format" });
            }

            // 計算開始日期和結束日期
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            // 查詢資料
            var data = await _context.EntryExitManagement
                .Where(p => p.PaymentStatus == true && p.PaymentTime.Value.Date >= startDate && p.PaymentTime.Value.Date <= endDate)
                .GroupBy(p => p.PaymentTime.Value.Date)
                .Select(g => new
                {
                    daytime = g.Key,
                    amount = g.Sum(p => p.Amount)
                })
                .ToListAsync();

            return Json(data);
        }


    }
}
