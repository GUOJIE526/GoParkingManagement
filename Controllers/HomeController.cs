using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyGoParking.Models;
using System.Diagnostics;

namespace MyGoParking.Controllers
{
    [Authorize]
    public class HomeController : Controller

    {

        private readonly MyGoParkingContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, MyGoParkingContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var monthlySum = _context.MonthlyRental
                .Where(s => s.PaymentStatus == true)
                .Select(s => s.Amount)
                .Sum();
            var EntryExitSum = _context.EntryExitManagement
               .Where(s => s.PaymentStatus == true)
               .Select(s => s.Amount)
               .Sum();
            var MonthlyApply = _context.MonApplyList
                .Where(s => s.ApplyStatus == "pending")
                .Count();
            var ParkingLot = await _context.ParkingLot
                .Select(p => new ParkingLot
                {
                    LotName = p.LotName,
                    Qty = p.Qty,
                }).OrderByDescending(p => p.Qty).Take(5).ToListAsync();

            var overtimecount = _context.Reservation.Where(s => s.IsOverdue == false).Count();
            var AllReservationCount = _context.Reservation.Count();
            decimal CompeleteRate = 0;
            if (AllReservationCount > 0)
            {
                CompeleteRate += (decimal)overtimecount / AllReservationCount;
                ViewData["CompeleteRate"] = Math.Round(decimal.Round(CompeleteRate, 2) * 100);
            }
            else
            {
                ViewData["CompeleteRate"] = '0';
            }
            ViewData["monthlySum"] = monthlySum;
            ViewData["EntryExitSum"] = EntryExitSum;
            ViewData["MonthlyApply"] = MonthlyApply;

            return View(ParkingLot);
        }
       

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> GetSlotData()
        {
            // 按照停車場分組，計算每個停車場的出租率
            var data = await _context.ParkingSlot
                .GroupBy(m => m.Lot.LotName)
                .Select(g => new
                {
                    LotName = g.Key,
                    RentedCount = g.Count(p => p.IsRented == true),
                    TotalCount = g.Count(),
                    Percentage = g.Count(p => p.IsRented == true) * 100.0 / g.Count()
                })
                .OrderByDescending(x=>x.Percentage)
                .Take(5)
                .ToListAsync();
            return Json(data); // 返回JSON數據
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
