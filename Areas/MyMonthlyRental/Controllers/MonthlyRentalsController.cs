using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyGoParking.Areas.MyMonthlyRental.ViewModels;
using MyGoParking.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyGoParking.Areas.MyMonthlyRental.Controllers
{
    [Authorize]
    [Area("MyMonthlyRental")]
    public class MonthlyRentalsController : Controller
    {
        private readonly MyGoParkingContext _context;

        public MonthlyRentalsController(MyGoParkingContext context)
        {
            _context = context;
        }

        // GET: MyMonthlyRental/MonthlyRentals
        public async Task<IActionResult> Index()
        {
            var myGoParkingContext = _context.MonthlyRental.Include(m => m.Car).Include(m => m.Lot).Include(m => m.Slot);
            return View(await myGoParkingContext.ToListAsync());
        }

        //Ajax請求:用於產生月租訂單表格
        public async Task<IActionResult> GetRentalData()
        {
            var data = await _context.MonthlyRental
                .Select(m => new
                {
                    Id = m.MonId, // 確保這裡提供 ID
                    lotName = m.Lot.LotName,
                    slotNumber = m.Slot.SlotNumber,
                    plate = m.Car.LicensePlate,
                    startDate = m.StartDate,
                    endDate = m.EndDate,
                    amount = m.Amount,
                    paymentTime = m.PaymentTime,
                    paymentStatus = m.PaymentStatus,
                    notificationStatus = m.NotificationStatus

                })
                .ToListAsync();

            return Json(data);
        }


        // GET: MyMonthlyRental/MonthlyRentals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monthlyRental = await _context.MonthlyRental
                .Include(m => m.Car)
                .Include(m => m.Lot)
                .Include(m => m.Slot)
                .FirstOrDefaultAsync(m => m.MonId == id);
            if (monthlyRental == null)
            {
                return NotFound();
            }

            return View(monthlyRental);
        }

        //20240918
        // GET: MyMonthlyRental/MonthlyRentals/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new MonthlyRentalViewModel
            {
                StartDate = DateTime.Now.Date,  // 設置開始日期為當前日期
                EndDate = DateTime.Now.Date.AddMonths(1)  // 預設結束日期為開始日期加一個月
            };
            ViewData["LotName"] = new SelectList(_context.ParkingLot, "LotName", "LotName");
            ViewData["SlotNumber"] = new SelectList(new List<SelectListItem>());  // 建立空白下拉式選單(車位號碼)
            return View(viewModel);

        }


        //根據所選停車場，將可選車位帶入下拉式選單
        public JsonResult GetSlotsByLotName(string lotName)
        {
            var slots = _context.ParkingSlot
                .Where(slot => slot.Lot.LotName == lotName && slot.IsRented == false)
                .Select(slot => new
                {
                    Value = slot.SlotNumber,
                    Text = slot.SlotNumber
                })
                .ToList();

            return Json(slots);
        }

        public JsonResult LoadSlotsByLotName(string lotName, string SlotNumber)
        {
            var slots = _context.ParkingSlot
                .Where(slot => slot.Lot.LotName == lotName && (slot.IsRented == false || slot.SlotNumber == SlotNumber))
                .Select(slot => new
                {
                    Value = slot.SlotNumber,
                    Text = slot.SlotNumber
                })
                .ToList();

            return Json(slots);
        }




        //20240918
        // POST: MyMonthlyRental/MonthlyRentals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MonthlyRentalViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                
                var lotId = _context.ParkingLot
                    .Where(n => n.LotName == viewModel.LotName)
                    .Select(n => n.LotId)
                    .FirstOrDefault();

                var SlotId = _context.ParkingSlot
                    .Where(n => n.LotId == lotId && n.SlotNumber == viewModel.SlotNumber)
                    .Select(n => n.SlotId)
                    .FirstOrDefault();

                var monthlyRental = new MonthlyRental
                {
                    LotId = lotId,

                    SlotId = SlotId,

                    CarId = _context.Car
                    .Where(n => n.LicensePlate == viewModel.LicensePlate)
                    .Select(n => n.CarId)
                    .FirstOrDefault(),

                    StartDate = viewModel.StartDate,
                    EndDate = viewModel.EndDate,
                    //欲設值
                    Amount = 3000
                    //PaymentStatus = false,(不需要了，資料表已有欲設值)                    
                };

                // 查詢車位資料(要更改車位狀態)
                var updateSlot = _context.ParkingSlot
                    .FirstOrDefault(slot => slot.SlotId == SlotId); // 使用 slotId 查詢車位
                //更新該車位狀態為被預訂
                updateSlot.IsRented = true;

                await _context.SaveChangesAsync();  //更新車位表(is_rented屬性)

                _context.Add(monthlyRental);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return Json(new { success = true });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors });
            }

            // 如果模型無效，重新加載下拉選單並返回視圖
            //ViewData["LotName"] = new SelectList(_context.ParkingLot, "LotName", "LotName");
            //return View(viewModel);

        }


        // GET: MyMonthlyRental/MonthlyRentals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            

            var monthlyRental = await _context.MonthlyRental.FindAsync(id);
            if (monthlyRental == null)
            {
                return NotFound();
            }

            string lotName = await _context.ParkingLot.Where(lot => lot.LotId == monthlyRental.LotId).Select(lot => lot.LotName).FirstOrDefaultAsync();
            string slotNumber = await _context.ParkingSlot.Where(slot  => slot.SlotId == monthlyRental.SlotId).Select (slot => slot.SlotNumber).FirstOrDefaultAsync();
            string license = await _context.Car.Where(car => car.CarId == monthlyRental.CarId).Select(car => car.LicensePlate).FirstOrDefaultAsync();
            var viewModel = new MonthlyRentalViewModel
            {
                MonId = monthlyRental.MonId,
                LotName = lotName,
                SlotNumber = slotNumber,                
                LicensePlate = license,
                StartDate = (DateTime)monthlyRental.StartDate,
                EndDate = (DateTime)monthlyRental.EndDate,
                Amount = (int)monthlyRental.Amount,
                PaymentTime = monthlyRental.PaymentTime,
                PaymentStatus = monthlyRental.PaymentStatus,
                NotificationStatus = monthlyRental.NotificationStatus,                

            };
            //新增的
            ViewData["LotName"] = new SelectList(_context.ParkingLot, "LotName", "LotName");
            ViewData["SlotNumber"] = new SelectList(new List<SelectListItem>());  // 建立空白下拉式選單(車位號碼)

            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MonthlyRentalViewModel viewModel)
        {
            if (id != viewModel.MonId)
            {
                return NotFound();
            }

            var originalSlotId = await _context.MonthlyRental
                    .Where(n => n.MonId == id)
                    .Select(n => n.SlotId)
                    .FirstOrDefaultAsync();


            var lotId = _context.ParkingLot
                    .Where(n => n.LotName == viewModel.LotName)
                    .Select(n => n.LotId)
                    .FirstOrDefault();

            var SlotId = _context.ParkingSlot
                    .Where(n => n.LotId == lotId && n.SlotNumber == viewModel.SlotNumber)
                    .Select(n => n.SlotId)
                    .FirstOrDefault();

            var carId = _context.Car
                    .Where(n => n.LicensePlate == viewModel.LicensePlate)
                    .Select(n => n.CarId)
                    .FirstOrDefault();

            if (ModelState.IsValid)
            {
                try
                {
                    // 從 ViewModel 轉換為 MonthlyRental
                    var monthlyRental = new MonthlyRental
                    {
                        MonId = viewModel.MonId,
                        LotId = lotId,
                        CarId = carId,
                        SlotId = SlotId,
                        StartDate = viewModel.StartDate,
                        EndDate = viewModel.EndDate,
                        RenewalStatus = false,
                        NotificationStatus = viewModel.NotificationStatus,
                        Amount = viewModel.Amount,
                        PaymentTime = viewModel.PaymentTime,
                        PaymentStatus = viewModel.PaymentStatus
                    };

                    // 查詢車位資料(要更改車位狀態)
                    var updateSlot = _context.ParkingSlot
                        .FirstOrDefault(slot => slot.SlotId == SlotId); // 使用 slotId 查詢車位
                                                                        //更新該車位狀態為被預訂
                    updateSlot.IsRented = true;
                    var originSlot = _context.ParkingSlot
                        .FirstOrDefault(slot => slot.SlotId == originalSlotId); //原本的車位
                    originSlot.IsRented = false;

                    
                    _context.Update(monthlyRental);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MonthlyRentalExists(viewModel.MonId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Json(new { success = true });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors });
            }

            // 如果資料驗證失敗，重新生成 ViewData 選項列表並回傳 ViewModel
            //ViewData["LotName"] = new SelectList(_context.ParkingLot, "LotName", "LotName");
            //ViewData["SlotNumber"] = new SelectList(new List<SelectListItem>());  // 建立空白下拉式選單(車位號碼)
            
        }

       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monthlyRental = await _context.MonthlyRental
                .Include(m => m.Car)
                .Include(m => m.Lot)
                .Include(m => m.Slot)
                .FirstOrDefaultAsync(m => m.MonId == id);
            if (monthlyRental == null)
            {
                return NotFound();
            }

            return View(monthlyRental);
        }

        // POST: MyMonthlyRental/MonthlyRentals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var monthlyRental = await _context.MonthlyRental.FindAsync(id);
            if (monthlyRental != null)
            {
                _context.MonthlyRental.Remove(monthlyRental);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MonthlyRentalExists(int id)
        {
            return _context.MonthlyRental.Any(e => e.MonId == id);
        }
    }
}
