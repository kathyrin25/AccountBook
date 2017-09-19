using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AccountBook.ValidateAttribute
{
    public sealed class CheckDateAttribute : ValidationAttribute, IClientValidatable
    {
        public string Input { get; set; }

        public CheckDateAttribute(string input)
        {
            if (string.IsNullOrWhiteSpace(input))//參數空白則帶今天的日期
                input = DateTime.Now.ToString("yyyy/MM/dd");

            this.Input = input;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule
            {
                //ValidationType 的值一定要是小寫！
                ValidationType = "checkdate",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };
            //ValidationParameters 一定要是小寫！
            rule.ValidationParameters["input"] = Input;
            yield return rule;
        }

        public override bool IsValid(object value)
        {
            //權責分清楚，沒有輸入不算錯
            if (value == null)
            {
                return true;
            }

            if (value is DateTime)
            {
                var CheckDate = DateTime.Parse(this.Input);
                var InputDate = (DateTime)value;
                //日期不可大於檢查日      
                return DateTime.Compare(InputDate, CheckDate) <= 0;
            }
            return true;
        }
    }
}