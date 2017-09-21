using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace AccountBook.Helper
{
    public static class UserIdentityExtensions
    {
        public static string GetNickName(this IIdentity identity)
        {            
            if (identity == null)
            {
                return String.Empty;
            }              
            
            var NickName = ((ClaimsIdentity)identity).FindFirstValue("NickName");

            return (NickName != null) ? NickName : String.Empty;
        }
    }
}