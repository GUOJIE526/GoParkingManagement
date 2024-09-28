using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using MyGoParking.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyGoParking.Areas.MyReservation.Controllers
{
    [Authorize]
    [Area("MyReservation")]
    public class ReservationController : Controller
    {
        private readonly MyGoParkingContext _context;

        public ReservationController(MyGoParkingContext context)
        {
            _context = context;
        }

        // GET: MyReservation/Reservation
        public async Task<IActionResult> Index()
        {
            var myGoParkingContext = _context.Reservation.Include(r => r.Car).Include(r => r.Lot);
            return View(await myGoParkingContext.ToListAsync());
        }

        public IActionResult IndexJson()
        {
            var myGoParkingContext = _context.Reservation.Include(r => r.Car).Include(r => r.Lot)
                .Select(r => new
                {
                    resId = r.ResId,
                    lotId = r.Lot.LotName,
                    carId = r.Car.LicensePlate,
                    reservationTime = r.ReservationTime.HasValue? r.ReservationTime.Value.ToString("yyyy/M/d tt hh:mm:ss"):null,
                    validUntil = r.ValidUntil.HasValue? r.ValidUntil.Value.ToString("yyyy/M/d tt hh:mm:ss"):null,
                    depositStatus = r.DepositStatus.Value ? "繳款" : "未繳款",
                    isOverdue = r.IsOverdue.Value ? "逾時" : "未逾時",
                    isCanceled = r.IsCanceled.Value ? "取消" : "未取消",
                    notificationStatus = r.NotificationStatus.Value ? "通知" : "未通知",
                    amount = r.Amount.HasValue? r.Amount.Value.ToString("C0", CultureInfo.CurrentCulture):null,
                    isRefoundDeposit = r.IsRefoundDeposit.Value ? "退款" : "未退款",
                    isFinish = r.IsFinish.Value ? "完成" : "未完成"

                });
            return Json(myGoParkingContext);
        }

        // GET: MyReservation/Reservation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.Car)
                .Include(r => r.Lot)
                .FirstOrDefaultAsync(m => m.ResId == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        public IActionResult CreatePartial()
        {
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "LicensePlate");
            ViewData["LotId"] = new SelectList(_context.ParkingLot, "LotId", "LotName");
            ViewData["DepositStatus"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "繳款", Value = "True"},
                new SelectListItem {Text = "未繳款", Value="False"},
            }, "Value", "Text");
            ViewData["IsOverdue"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "逾時", Value = "True"},
                new SelectListItem {Text = "未逾時", Value="False"},
            }, "Value", "Text");
            ViewData["IsCanceled"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "取消", Value = "True"},
                new SelectListItem {Text = "未取消", Value="False"},
            }, "Value", "Text");
            ViewData["NotificationStatus"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "通知", Value = "True"},
                new SelectListItem {Text = "未通知", Value="False"},
            }, "Value", "Text");
            ViewData["IsRefoundDeposit"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "退款", Value = "True"},
                new SelectListItem {Text = "未退款", Value="False"},
            }, "Value", "Text");
            ViewData["IsFinish"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "完成", Value = "True"},
                new SelectListItem {Text = "未完成", Value="False"},
            }, "Value", "Text");

            return PartialView("_CreatePartial");
        }

        // GET: MyReservation/Reservation/Create
        public IActionResult Create()
        {
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "LicensePlate");
            ViewData["LotId"] = new SelectList(_context.ParkingLot, "LotId", "LotName");
            ViewData["DepositStatus"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "繳款", Value = "True"},
                new SelectListItem {Text = "未繳款", Value="False"},
            }, "Value", "Text");
            ViewData["IsOverdue"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "逾時", Value = "True"},
                new SelectListItem {Text = "未逾時", Value="False"},
            }, "Value", "Text");
            ViewData["IsCanceled"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "取消", Value = "True"},
                new SelectListItem {Text = "未取消", Value="False"},
            }, "Value", "Text");
            ViewData["NotificationStatus"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "通知", Value = "True"},
                new SelectListItem {Text = "未通知", Value="False"},
            }, "Value", "Text");
            ViewData["IsRefoundDeposit"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "退款", Value = "True"},
                new SelectListItem {Text = "未退款", Value="False"},
            }, "Value", "Text");
            ViewData["IsFinish"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "完成", Value = "True"},
                new SelectListItem {Text = "未完成", Value="False"},
            }, "Value", "Text");


            return View();
        }

        // POST: MyReservation/Reservation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string LicensePlate, [Bind("ResId,LotId,CarId,ReservationTime,ValidUntil,DepositStatus,IsOverdue,IsCanceled,NotificationStatus,Amount,IsRefoundDeposit,IsFinish")] Reservation reservation)
        {
            if (string.IsNullOrEmpty(LicensePlate))
            {
                return Json(new { success = false, errors = new[] { "請輸入車牌號碼!"} });
            }
            var car = await _context.Car.FirstOrDefaultAsync(c => c.LicensePlate == LicensePlate);
            if (car == null)
            {
                return Json(new { success = false, errors = new[] { "車牌尚未註冊" } });
            }
            reservation.CarId = car.CarId;

            // 檢查 ModelState 是否有效
            if (ModelState.IsValid)
            {
                // 自動設置有效時間
                if (reservation.ReservationTime.HasValue)
                {
                    reservation.ValidUntil = reservation.ReservationTime.Value.AddHours(1);
                }

                // 保存預訂資料
                _context.Add(reservation);
                await _context.SaveChangesAsync();

                // 處理超時邏輯
                if (reservation.IsOverdue == true)
                {
                    var carEntity = await _context.Car.FindAsync(reservation.CarId);
                    if (carEntity != null)
                    {
                        var user = await _context.Customer.FindAsync(carEntity.UserId);
                        if (user != null)
                        {
                            user.BlackCount++;

                            var overdueCount = await _context.Reservation
                                .Where(r => r.CarId == reservation.CarId && r.IsOverdue == true)
                                .CountAsync();

                            if (overdueCount >= 3 || user.BlackCount >= 3)
                            {
                                user.IsBlack = true;
                            }
                            else
                            {
                                user.IsBlack = false;
                            }

                            _context.Update(user);
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                return Json(new { success = true });
            }
            else
            {
                // 如果 ModelState 驗證失敗，返回錯誤訊息
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors });
            }
        }

        public async Task<ActionResult> EditPartial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "LicensePlate", reservation.CarId);
            ViewData["LotId"] = new SelectList(_context.ParkingLot, "LotId", "LotName", reservation.LotId);
            ViewData["DepositStatus"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "繳款", Value = "True"},
                new SelectListItem {Text = "未繳款", Value="False"},
            }, "Value", "Text");
            ViewData["IsOverdue"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "逾時", Value = "True"},
                new SelectListItem {Text = "未逾時", Value="False"},
            }, "Value", "Text");
            ViewData["IsCanceled"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "取消", Value = "True"},
                new SelectListItem {Text = "未取消", Value="False"},
            }, "Value", "Text");
            ViewData["NotificationStatus"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "通知", Value = "True"},
                new SelectListItem {Text = "未通知", Value="False"},
            }, "Value", "Text");
            ViewData["IsRefoundDeposit"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "退款", Value = "True"},
                new SelectListItem {Text = "未退款", Value="False"},
            }, "Value", "Text");
            ViewData["IsFinish"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Text = "完成", Value = "True"},
                new SelectListItem {Text = "未完成", Value="False"},
            }, "Value", "Text");

            return PartialView("_EditPartial", reservation);
        }

        // GET: MyReservation/Reservation/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var reservation = await _context.Reservation.FindAsync(id);
        //    if (reservation == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CarId"] = new SelectList(_context.Car, "CarId", "LicensePlate", reservation.CarId);
        //    ViewData["LotId"] = new SelectList(_context.ParkingLot, "LotId", "LotName", reservation.LotId);
        //    ViewData["DepositStatus"] = new SelectList(new List<SelectListItem>
        //    {
        //        new SelectListItem {Text = "繳款", Value = "True"},
        //        new SelectListItem {Text = "未繳款", Value="False"},
        //    }, "Value", "Text");
        //    ViewData["IsOverdue"] = new SelectList(new List<SelectListItem>
        //    {
        //        new SelectListItem {Text = "逾時", Value = "True"},
        //        new SelectListItem {Text = "未逾時", Value="False"},
        //    }, "Value", "Text");
        //    ViewData["IsCanceled"] = new SelectList(new List<SelectListItem>
        //    {
        //        new SelectListItem {Text = "取消", Value = "True"},
        //        new SelectListItem {Text = "未取消", Value="False"},
        //    }, "Value", "Text");
        //    ViewData["NotificationStatus"] = new SelectList(new List<SelectListItem>
        //    {
        //        new SelectListItem {Text = "通知", Value = "True"},
        //        new SelectListItem {Text = "未通知", Value="False"},
        //    }, "Value", "Text");
        //    ViewData["IsRefoundDeposit"] = new SelectList(new List<SelectListItem>
        //    {
        //        new SelectListItem {Text = "退款", Value = "True"},
        //        new SelectListItem {Text = "未退款", Value="False"},
        //    }, "Value", "Text");
        //    ViewData["IsFinish"] = new SelectList(new List<SelectListItem>
        //    {
        //        new SelectListItem {Text = "完成", Value = "True"},
        //        new SelectListItem {Text = "未完成", Value="False"},
        //    }, "Value", "Text");

        //    return View(reservation);
        //}

        // POST: MyReservation/Reservation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ResId,LotId,CarId,ReservationTime,ValidUntil,DepositStatus,IsOverdue,IsCanceled,NotificationStatus,Amount,IsRefoundDeposit,IsFinish")] Reservation reservation)
        {
            if (id != reservation.ResId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 處理超時邏輯
                    if (reservation.IsOverdue == true)
                    {
                        var carEntity = await _context.Car.FindAsync(reservation.CarId);
                        if (carEntity != null)
                        {
                            var user = await _context.Customer.FindAsync(carEntity.UserId);
                            if (user != null)
                            {
                                user.BlackCount++;

                                var overdueCount = await _context.Reservation
                                    .Where(r => r.CarId == reservation.CarId && r.IsOverdue == true)
                                    .CountAsync();

                                if (overdueCount >= 3 || user.BlackCount >= 3)
                                {
                                    user.IsBlack = true;
                                }
                                else
                                {
                                    user.IsBlack = false;
                                }
                            }
                        }
                    }
                    else if(reservation.IsOverdue == false) 
                    {
                        var carEntity = await _context.Car.FindAsync(reservation.CarId);
                        if (carEntity != null)
                        {
                            var user = await _context.Customer.FindAsync(carEntity.UserId);
                            if (user != null)
                            {
                                user.BlackCount--;

                                var overdueCount = await _context.Reservation
                                    .Where(r => r.CarId == reservation.CarId && r.IsOverdue == true)
                                    .CountAsync();

                                if (overdueCount >= 3 || user.BlackCount >= 3)
                                {
                                    user.IsBlack = true;
                                }
                                else
                                {
                                    user.IsBlack = false;
                                }
                            }
                        }
                    }
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.ResId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Json(new { success = true });
                //return RedirectToAction(nameof(Index));
            }
            else
            {
                return Json(new { success = false });
            }
        }

        // GET: MyReservation/Reservation/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var reservation = await _context.Reservation
        //        .Include(r => r.Car)
        //        .Include(r => r.Lot)
        //        .FirstOrDefaultAsync(m => m.ResId == id);
        //    if (reservation == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(reservation);
        //}

        // POST: MyReservation/Reservation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation != null)
            {
                try
                {
                    _context.Reservation.Remove(reservation);
                    if (reservation.IsOverdue == true)
                    {
                        var car = await _context.Car.FindAsync(reservation.CarId);
                        if (car != null)
                        {
                            var user = await _context.Customer.FindAsync(car.UserId);
                            if (user != null)
                            {
                                // 減少 BlackCount
                                user.BlackCount--;

                                // 檢查同一個 car_id 有多少超時的預訂
                                var overdueCount = await _context.Reservation.Where(r => r.CarId == reservation.CarId && r.IsOverdue == true).CountAsync();

                                // 如果超時超過或等於 3 筆，將 IsBlack 設為 true
                                if (overdueCount < 3 || user.BlackCount < 3)
                                {
                                    if (user.BlackCount <= 0)
                                    {
                                        user.BlackCount = 0;
                                    }
                                    user.IsBlack = false;
                                }
                                else
                                {
                                    user.IsBlack = true;
                                }
                                // 保存更改
                                _context.Update(user);
                            }
                        }
                    }
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                } 
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "找不到此預定記錄" });
            //await _context.SaveChangesAsync();
            //return Json(new { success = true });
            //return RedirectToAction(nameof(Index));
        }


        private bool ReservationExists(int id)
        {
            return _context.Reservation.Any(e => e.ResId == id);
        }
    }
}
