using AccountBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountBook.Helper
{
    public static class CustomHtmlExtensions
    {
        public static string ShowClass(this HtmlHelper helper, int _booktype)
        {
            return (_booktype == (int)BookType.支出) ? "text-danger" : "text-primary";
        }

        public static string ShowBookType(this HtmlHelper helper, int _booktype)
        {
            return (_booktype == (int)BookType.支出) ? BookType.支出.ToString() : BookType.收入.ToString();
        }
    }
}