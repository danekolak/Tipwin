using System;
using System.ComponentModel.DataAnnotations;

namespace Tipwin.Models
{
    public class DateRangeAttribute : RangeAttribute
    {
        public DateRangeAttribute(string min) : base(typeof(DateTime), min,
            DateTime.Now.ToShortDateString())
        { }
    }
    public class CurrentDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dateTime = Convert.ToDateTime(value);

            return dateTime <= DateTime.Now;
        }
    }
}
