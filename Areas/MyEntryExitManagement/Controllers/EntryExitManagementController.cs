using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyGoParking.Models;
using MyGoParking.Areas.MyEntryExitManagement.ViewModel;
using System.Drawing;
using System.IO;
using System.IO.Pipelines;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using System.Diagnostics.Eventing.Reader;

namespace MyGoParking.Areas.MyEntryExitManagement.Controllers
{
    [Authorize]
    [Area("MyEntryExitManagement")]
    public class EntryExitManagementController : Controller
    {
        private readonly MyGoParkingContext _context;

        public EntryExitManagementController(MyGoParkingContext context)
        {
            _context = context;
        }

        // GET: MyEntryExitManagement/EntryExitManagement
        public async Task<IActionResult> Index()
        {
            var myGoParkingContext = _context.EntryExitManagement.Include(e => e.Car).ThenInclude(e => e.User).Include(e => e.Lot);
            #region 用ViewModel廢用,但先留著而已
            //var entryExitManagements = await _context.EntryExitManagement.ToListAsync();

            //var viewModel = entryExitManagements.Select(e => new EntryExitManagementViewModel
            //{
            //    EntryexitId = e.EntryexitId,
            //    LotId = e.LotId,
            //    CarId = e.CarId,
            //    Parktype = e.Parktype,
            //    LicensePlatePhoto = e.LicensePlatePhoto,
            //    EntryTime = e.EntryTime,
            //    LicensePlateKeyinTime = e.LicensePlateKeyinTime,
            //    Amount = e.Amount,
            //    ExitTime = e.ExitTime,
            //    PaymentStatus = e.PaymentStatus,
            //    PaymentTime = e.PaymentTime,
            //    ValidTime = e.ValidTime,
            //    IsFinish = e.IsFinish,
            //});

            //_viewmodel = viewModel;


            //return View(viewModel);
            #endregion
            return View(myGoParkingContext);
        }


        #region 圖片預覽
        public async Task<FileResult> GetPicture(int? id)
        {
            EntryExitManagement? c = await _context.EntryExitManagement.FindAsync(id);
            string? pictureName = c.LicensePlatePhoto;
            var filePath = $"wwwroot\\images\\{pictureName}.png";
            var No_filePath = "wwwroot\\images\\not_found.png";

            if (!System.IO.File.Exists(filePath))
            {
                var No_fileStream = new FileStream(No_filePath, FileMode.Open, FileAccess.Read);
                return File(No_fileStream, "image/png");
            }
            else
            {
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                return File(fileStream, "image/png");
            }            
        }

        private async void SaveUploadImage(EntryExitManagement entryExitManagement, IFormFile Picture)
        {
            if (Picture != null && Picture.Length > 0)
            {
                /*
                using (var memoryStream = new MemoryStream())
                {
                    string Name = Picture.Name;
                    string FileName = Picture.FileName;
                    await Picture.CopyToAsync(memoryStream);
                    entryExitManagement.LicensePlatePhoto = Picture.FileName;
                    Console.WriteLine(Name + FileName );
                    var filename = entryExitManagement.LicensePlatePhoto;
                    var filePath = $"wwwroot\\images\\{filename}.png";

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Picture.CopyToAsync(fileStream);
                    }
                }
                */


                // 將圖片文件名儲存到資料庫字段
                entryExitManagement.LicensePlatePhoto = Picture.FileName;
                var filename = entryExitManagement.LicensePlatePhoto;
                var filePath = Path.Combine("wwwroot", "images", $"{filename}.png");

                // 直接將圖片複製到指定路徑，而不是使用 MemoryStream
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Picture.CopyToAsync(fileStream);
                }
            }
        }

        //0921新增的顯示partialview的aciton
        public async Task<IActionResult> ShowPicturePartial(int id)
        {
            var model = _context.EntryExitManagement.Find(id); // 根據ID查詢資料

            if (model == null)
            {
                return NotFound();
            }

            return PartialView("_ShowPicturePartial", model);

        }

        #endregion

        #region 測試看看如果都改成partial的話
        public async Task<IActionResult> DetailsPartial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {

            }

            var entryExitManagement = await _context.EntryExitManagement
                .Include(e => e.Car)
                    .ThenInclude(e => e.User)
                .Include(e => e.Lot)
                .FirstOrDefaultAsync(m => m.EntryexitId == id);

            if (entryExitManagement == null)
            {
                return NotFound();
            }

            return PartialView("_DetailsPartial",entryExitManagement);
        }

        public IActionResult CreatePartial()
        {
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "LicensePlate");
            ViewData["LotId"] = new SelectList(_context.ParkingLot, "LotId", "LotName");
            ViewData["Parktype"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "Reservation", Value = "Reservation" },
                new SelectListItem { Text = "MonthlyRental", Value = "MonthlyRental" }
            }, "Value", "Text");

            ViewData["IsPayment"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "完成", Value = "True" },
                new SelectListItem { Text = "未完成", Value = "False" }
            }, "Value", "Text");

            ViewData["IsFinished"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "完成", Value = "True" },
                new SelectListItem { Text = "未完成", Value = "False" }
            }, "Value", "Text");
            return PartialView("_CreatePartial");
        }

        public async Task<IActionResult> EditPartial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entryExitManagement = await _context.EntryExitManagement.FindAsync(id);
            if (entryExitManagement == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "LicensePlate");
            ViewData["LotId"] = new SelectList(_context.ParkingLot, "LotId", "LotName");

            ViewData["Parktype"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "Reservation", Value = "Reservation" },
                new SelectListItem { Text = "MonthlyRental", Value = "MonthlyRental" }
            }, "Value", "Text");

            ViewData["IsPayment"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "完成", Value = "True" },
                new SelectListItem { Text = "未完成", Value = "False" }
            }, "Value", "Text", entryExitManagement.PaymentStatus);
            ViewData["IsFinished"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "完成", Value = "True" },
                new SelectListItem { Text = "未完成", Value = "False" }
            }, "Value", "Text", entryExitManagement.IsFinish);

            return PartialView("_EditPartial",entryExitManagement);
        }

        public async Task<IActionResult> DeletePartial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entryExitManagement = await _context.EntryExitManagement
                .Include(e => e.Car)
                    .ThenInclude(e => e.User)
                .Include(e => e.Lot)
                .FirstOrDefaultAsync(m => m.EntryexitId == id);

            if (entryExitManagement == null)
            {
                return NotFound();
            }

            return PartialView("_DeletePartial",entryExitManagement);
        }


        #endregion


        #region 測試看看把datatable用ajax的方式生成
        public IActionResult GetData()
        {

            /*
            var data = _context.EntryExitManagement
                .Include(e => e.Car) // 加載 Car 資料
                    .ThenInclude(c => c.User) // 加載 User 資料
                .Include(e => e.Lot) // 加載 Lot 資料
                .Select(e => new
                {
                    e.EntryexitId,
                    e.Parktype,
                    e.LicensePlatePhoto,
                    CarId = e.Car.LicensePlate, // 假設 LicensePlate 是 Car 的屬性
                    UserName = e.Car.User.Username, // 來自 User 的名稱
                    e.EntryTime,
                    e.LicensePlateKeyinTime,
                    amount = e.Amount.HasValue ? e.Amount.Value.ToString("C0", CultureInfo.CurrentCulture) : null,
                    e.ExitTime,
                    e.PaymentStatus,
                    e.PaymentTime,
                    e.ValidTime,
                    LotName = e.Lot.LotName, // 來自 Lot 的名稱
                    e.IsFinish
                });
            */

            var data = _context.EntryExitManagement
                    .Include(e => e.Car).ThenInclude(e => e.User)
                    .Include(e => e.Lot)
                    .Select(e => new
                    {
                        entryexitId = e.EntryexitId,
                        parktype = e.Parktype,
                        licensePlatePhoto = e.LicensePlatePhoto,
                        carId = e.Car.LicensePlate,
                        userName = e.Car.User.Username,
                        entryTime = e.EntryTime.HasValue ? e.EntryTime.Value.ToString("yyyy/M/d tt hh:mm:ss") : null, // 格式化
                        licensePlateKeyinTime = e.LicensePlateKeyinTime.HasValue ? e.LicensePlateKeyinTime.Value.ToString("yyyy/M/d tt hh:mm:ss") : null, // 格式化
                        amount = e.Amount.HasValue ? e.Amount.Value.ToString("C0", CultureInfo.CurrentCulture) : null,
                        exitTime = e.ExitTime.HasValue ? e.ExitTime.Value.ToString("yyyy/M/d tt hh:mm:ss") : null, // 格式化
                        paymentStatus = e.PaymentStatus ? "完成" : "未完成",
                        paymentTime = e.PaymentTime.HasValue ? e.PaymentTime.Value.ToString("yyyy/M/d tt hh:mm:ss") : null, // 格式化
                        validTime = e.ValidTime.HasValue ? e.ValidTime.Value.ToString("yyyy/M/d tt hh:mm:ss") : null, // 格式化
                        lotName = e.Lot.LotName,
                        isFinish = e.IsFinish ? "完成":"未完成"
                    });
            

            return Json(data); // 返回 JSON 格式的數據
        }
        #endregion




        // GET: MyEntryExitManagement/EntryExitManagement/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                
            }

            var entryExitManagement = await _context.EntryExitManagement
                .Include(e => e.Car)
                    .ThenInclude(e => e.User)
                .Include(e => e.Lot)
                .FirstOrDefaultAsync(m => m.EntryexitId == id);

            if (entryExitManagement == null)
            {
                return NotFound();
            }

            return View(entryExitManagement);
        }

        // GET: MyEntryExitManagement/EntryExitManagement/Create
        public IActionResult Create()
        {
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "LicensePlate");
            ViewData["LotId"] = new SelectList(_context.ParkingLot, "LotId", "LotName");

            ViewData["Parktype"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "Reservation", Value = "Reservation" },
                new SelectListItem { Text = "MonthlyRental", Value = "MonthlyRental" }
            }, "Value", "Text");

            ViewData["IsPayment"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "完成", Value = "True" },
                new SelectListItem { Text = "未完成", Value = "False" }
            }, "Value", "Text");

            ViewData["IsFinished"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "完成", Value = "True" },
                new SelectListItem { Text = "未完成", Value = "False" }
            }, "Value", "Text");
            return View();
        }

        // POST: MyEntryExitManagement/EntryExitManagement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EntryexitId,LotId,CarId,Parktype,LicensePlatePhoto,EntryTime,LicensePlateKeyinTime,Amount,ExitTime,PaymentStatus,PaymentTime,ValidTime,IsFinish")] EntryExitManagement entryExitManagement, IFormFile? Picture)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Picture != null)
                    {
                        SaveUploadImage(entryExitManagement, Picture);
                    }
                    _context.Add(entryExitManagement);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            else
            {
                return Json(new { success = false, message = "必填欄位沒填" });
            }

        }

        // GET: MyEntryExitManagement/EntryExitManagement/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entryExitManagement = await _context.EntryExitManagement.FindAsync(id);
            if (entryExitManagement == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Car, "CarId", "LicensePlate");
            ViewData["LotId"] = new SelectList(_context.ParkingLot, "LotId", "LotName");
            ViewData["IsPayment"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "完成", Value = "True" },
                new SelectListItem { Text = "未完成", Value = "False" }
            }, "Value", "Text", entryExitManagement.PaymentStatus);
            ViewData["IsFinished"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "完成", Value = "True" },
                new SelectListItem { Text = "未完成", Value = "False" }
            }, "Value", "Text", entryExitManagement.IsFinish);
            return View(entryExitManagement);
        }

        // POST: MyEntryExitManagement/EntryExitManagement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestFormLimits(MultipartBodyLengthLimit = 2048000)]
        [RequestSizeLimit(2048000)]
        public async Task<IActionResult> Edit(int id, [Bind("EntryexitId,LotId,CarId,Parktype,LicensePlatePhoto,EntryTime,LicensePlateKeyinTime,Amount,ExitTime,PaymentStatus,PaymentTime,ValidTime,IsFinish")] EntryExitManagement entryExitManagement, IFormFile? Picture)
        {
            if (id != entryExitManagement.EntryexitId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Picture != null)
                    {
                        SaveUploadImage(entryExitManagement, Picture);
                    }
                    else
                    {
                        entryExitManagement.LicensePlatePhoto = _context.EntryExitManagement.Where(s => s.EntryexitId == id).Select(s => s.LicensePlatePhoto).FirstOrDefault();
                    }

                    _context.Update(entryExitManagement);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    // 處理錯誤
                    return Json(new { success = false, message = ex.Message });
                }
            }
            else
            {
                return Json(new { success = false, message = "必填欄位是空的" });
            }

        }


        // GET: MyEntryExitManagement/EntryExitManagement/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entryExitManagement = await _context.EntryExitManagement
                .Include(e => e.Car)
                    .ThenInclude(e => e.User)
                .Include(e => e.Lot)
                .FirstOrDefaultAsync(m => m.EntryexitId == id);

            if (entryExitManagement == null)
            {
                return NotFound();
            }

            return View(entryExitManagement);
        }

        /*原本的delete
        // POST: MyEntryExitManagement/EntryExitManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entryExitManagement = await _context.EntryExitManagement.FindAsync(id);
            if (entryExitManagement != null)
            {
                _context.EntryExitManagement.Remove(entryExitManagement);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        */

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entryExitManagement = await _context.EntryExitManagement.FindAsync(id);
            try
            {
                if (entryExitManagement != null)
                {
                    _context.EntryExitManagement.Remove(entryExitManagement);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "發生未知錯誤" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }



        private bool EntryExitManagementExists(int id)
        {
            return _context.EntryExitManagement.Any(e => e.EntryexitId == id);
        }


        #region 自己做的方法 廢用
        Task<IActionResult> myselectlist(int? id)
        {
            var boolSelectList = new List<SelectListItem>
            {
                new SelectListItem { Text = "已完成", Value = "true" },
                new SelectListItem { Text = "未完成", Value = "false" }
            };

            ViewBag.IsFinishSelectList = new SelectList(boolSelectList, "Value", "Text", _context.EntryExitManagement.SingleOrDefault(s=>s.EntryexitId==id).IsFinish.ToString());

            return ViewBag.IsFinishSelectList;
        }



        public async Task<IActionResult> Upload(int? id)
        {
            return View();
        }


        
        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            var file = Request.Form.Files["file"];
            if (file != null && file.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    var fileBytes = stream.ToArray();
                    // 進行進一步處理，例如儲存檔案
                }
            }
            return RedirectToAction("Index");
        }

        /*
        [HttpPost]
        public IActionResult Upload(string test)
        {
            Console.WriteLine($"Test Value: {test}");
            return RedirectToAction("Index");
        }
        */

        #endregion
    }
}
