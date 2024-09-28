using MyGoParking.Models;
using System.ComponentModel.DataAnnotations;

namespace MyGoParking.Metadata
{
    internal class EwalletMetadata
    {
        public int WalletId { get; set; }

        [Display(Name = "用戶")]
        public int? UserId { get; set; }

        [Required(ErrorMessage = "金額不能為空")]
        [Display(Name = "錢包餘額")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public int Balance { get; set; }

        [Required(ErrorMessage = "時間不能為空")]
        [Display(Name = "更新時間")]
        public DateTime? UpdatedTime { get; set; }

        [Display(Name = "用戶")]
        public virtual Customer? User { get; set; }
    }
}
