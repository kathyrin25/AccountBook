using AccountBook.Models.Enums;
using AccountBook.ValidateAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccountBook.Models.ViewModels
{
    public class EditRecordViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "日期")]
        [Required]
        [CheckDate("", ErrorMessage = "日期不可大於今天")]
        [UIHint("DateTimeInput")]
        public DateTime Date { get; set; }

        [Display(Name = "類別")]
        [Required]
        public BookType Category { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "金額")]
        [Required]
        [PositiveInteger(0, ErrorMessage = "金額請輸入大於0的正整數")]
        public int Amount { get; set; }

        [Required]
        [Display(Name = "備註")]
        [StringLength(100, ErrorMessage = "輸入字串長度不可超過{1}個字元")]
        public string Remark { get; set; }
    }
}