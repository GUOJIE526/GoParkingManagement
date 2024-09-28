using MyGoParking.Models;
using System.ComponentModel.DataAnnotations;

namespace MyGoParking.Metadata
{
    internal class CustomerMetadata
    {
        public int UserId { get; set; }

        [Display(Name = "用戶")]
        [Required(ErrorMessage = "用戶必填寫")]
        public string Username { get; set; }

        [Display(Name = "密碼")]
        public string? Password { get; set; }

        [Display(Name = "信箱")]
        public string? Email { get; set; }

        [Display(Name = "電話")]
        public string? Phone { get; set; }

        [Display(Name = "逾時次數")]
        public int BlackCount { get; set; }

        [Display(Name = "黑名單狀態")]
        public bool IsBlack { get; set; } = false;

        public virtual ICollection<Car> Car { get; set; } = new List<Car>();

        public virtual ICollection<Ewallet> Ewallet { get; set; } = new List<Ewallet>();

    }
}