using AccountBook.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccountBook.Areas.backend.Models
{
    public class UserModel
    {
        [Key]
        public string Id { get; set; }


        [Display(Name = "電子郵件")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "名稱")]
        public string UserName { get; set; }

        [Display(Name = "暱稱")]
        public string NickName { get; set; }

        [Required(ErrorMessage = "請至少選一個角色")]
        [Display(Name = "角色")]
        public string Role { get; set; }

        [Required]
        [Display(Name = "狀態")]
        public UserStatus status { get; set; }

        
    }
}