using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccountBook.ValidateAttribute
{
    public class BanWordsAttribute : ValidationAttribute
    {
        public string[] Input { get; set; }

        public BanWordsAttribute(string input)
        {
            if (input.IndexOf(",") > -1)
                this.Input = input.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            else
                this.Input = new string[] { input };

        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                if (value is string)
                {
                    foreach (string keyword in this.Input)
                    {
                        if (value.ToString().IndexOf(keyword) >= 0)
                            return false;
                    }
                }
            }

            return true;
        }
    }
}