using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MyGoParking.Models;


namespace MyGoParking.Areas.MyParkingLot.ViewModels
{
    public class ParkingLotViewModel
    {
        [Required(ErrorMessage = "請選擇停車場編號")]
        [Display(Name ="停車場編號")]
        public int LotId { get; set; }

        [Required(ErrorMessage = "請選擇停車場名稱")]
        [Display(Name = "停車場名稱")]
        public string? LotName { get; set; }

        [Required(ErrorMessage = "請選擇停車場地址")]
        [Display(Name = "停車場地址")]
        public string? LotAddress { get; set; }

        [Required(ErrorMessage = "請選擇停車場數量")]
        [Display(Name = "停車場數量")]
        public int? Qty { get; set; }

        [Required(ErrorMessage = "請選擇停車場電動車位數量")]
        [Display(Name = "停車場電動車位數量")]
        public int? Etcqty { get; set; }

        [Required(ErrorMessage = "請選擇可月租車位數量")]
        [Display(Name = "可月租車位數量")]
        public int? Monqty { get; set; }

        [Required(ErrorMessage = "請選擇狀態")]
        [Display(Name = "是否可預訂")]
        public bool IsResStatus { get; set; }

        [Required(ErrorMessage = "請選擇狀態")]
        [Display(Name = "是否可月租")]
        public bool IsMonStatus { get; set; }

        
        public string? Contract { get; set; }

        public virtual ICollection<EntryExitManagement> EntryExitManagement { get; set; } = new List<EntryExitManagement>();

        public virtual ICollection<MonApplyList> MonApplyList { get; set; } = new List<MonApplyList>();

        public virtual ICollection<MonthlyRental> MonthlyRental { get; set; } = new List<MonthlyRental>();

        public virtual ICollection<ParkingSlot> ParkingSlot { get; set; } = new List<ParkingSlot>();

        public virtual ICollection<Reservation> Reservation { get; set; } = new List<Reservation>();
    }
}
