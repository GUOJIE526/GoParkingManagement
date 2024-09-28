using MyGoParking.Models;
using System.ComponentModel.DataAnnotations;

namespace MyGoParking.Metadata
{
    internal class CarMetadata
    {
        [Display(Name = "車輛編號")]
        public int CarId { get; set; }
        [Display(Name = "用戶編號")]
        public int? UserId { get; set; }
        [Required(ErrorMessage = "車牌號碼不能為空")]
        [Display(Name = "車牌號碼")]
        public string? LicensePlate { get; set; }

        public virtual ICollection<EntryExitManagement> EntryExitManagement { get; set; } = new List<EntryExitManagement>();

        public virtual ICollection<MonApplyList> MonApplyList { get; set; } = new List<MonApplyList>();

        public virtual ICollection<MonthlyRental> MonthlyRental { get; set; } = new List<MonthlyRental>();

        public virtual ICollection<Reservation> Reservation { get; set; } = new List<Reservation>();
        [Display(Name = "用戶")]
        public virtual Customer? User { get; set; }

    }
}