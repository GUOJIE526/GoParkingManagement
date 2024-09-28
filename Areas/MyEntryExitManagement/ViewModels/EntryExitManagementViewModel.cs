using MyGoParking.Models;
using System.ComponentModel.DataAnnotations;


namespace MyGoParking.Areas.MyEntryExitManagement.ViewModel
{
    public class EntryExitManagementViewModel : IValidatableObject
    {
        public int EntryexitId { get; set; }

        [Display(Name="停車場編號")]
        public int? LotId { get; set; }

        [Display(Name = "車輛編號")]
        public int? CarId { get; set; }

        [Display(Name = "預定類型")]
        public string? Parktype { get; set; }

        [Display(Name = "車牌號碼")]
        public string? LicensePlatePhoto { get; set; }

        [Display(Name = "入場時間")]
        public DateTime? EntryTime { get; set; }

        [Display(Name = "繳費時間")]
        public DateTime? LicensePlateKeyinTime { get; set; }

        [Display(Name = "停車費用")]
        public int? Amount { get; set; }

        [Display(Name = "出場時間")]
        public DateTime? ExitTime { get; set; }

        [Display(Name = "繳費狀態")]
        public bool PaymentStatus { get; set; }

        [Display(Name = "付款時間")]
        public DateTime? PaymentTime { get; set; }

        [Display(Name = "逾時時間")]
        public DateTime? ValidTime { get; set; }

        [Display(Name = "完成狀態")]
        public bool IsFinish { get; set; }

        
        public virtual Car? Car { get; set; }

        public virtual ParkingLot? Lot { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
