using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountBook.ValidateAttribute
{
    public sealed class PositiveIntegerAttribute : ValidationAttribute, IClientValidatable
    {
        private int Input;

        public PositiveIntegerAttribute(int input)
        {
            this.Input = input;
        }

        public override bool IsValid(object value)
        {
            //權責分清楚，沒有輸入不算錯
            if (value == null)
            {
                return true;
            }

            if (value is int)
            {
                int amount = (int)value;
                return amount > this.Input;
            }

            return true;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule
            {
                //ValidationType 的值一定要是小寫！
                ValidationType = "positiveinteger",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };
            //ValidationParameters 一定要是小寫！
            rule.ValidationParameters["input"] = this.Input;
            yield return rule;
        }
    }
}