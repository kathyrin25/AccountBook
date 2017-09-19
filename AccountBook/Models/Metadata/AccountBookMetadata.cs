using AccountBook.ValidateAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccountBook.Models
{
    [MetadataType(typeof(AccountBookMetadata))]
    public partial class AccountBook
    {
        public class AccountBookMetadata
        {
            public Guid Id { get; set; }

            [Display(Name = "類別")]
            [Required]
            public int Category { get; set; }

            [DisplayFormat(DataFormatString = "{0:N0}")]
            [Display(Name = "金額")]
            [Required]
            [PositiveInteger(0, ErrorMessage = "金額請輸入大於0的正整數")]
            public int Amount { get; set; }

            [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
            [Display(Name = "日期")]
            [Required]
            public DateTime Date { get; set; }

            [Display(Name = "備註")]
            [StringLength(500)]
            [Required]
            public string Remark { get; set; }

            [Display(Name = "填寫人")]
            [StringLength(100)]
            public string Creator { get; set; }

            [Display(Name = "登錄日")]
            public Nullable<DateTime> CreateDT { get; set; }
        }
    }
}