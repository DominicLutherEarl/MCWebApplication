using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Mc.TD.Upload.Domain.DataMatch
{
    [AttributeUsage(AttributeTargets.Property |  AttributeTargets.Field, AllowMultiple = true)]
    public sealed class MCAPIValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool result = true;
            if (1==1) { }
            if (ErrorMessageResourceName == "orderid")
            {

            }
            // Add validation logic here.
            return result;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name);
        }
    }
}