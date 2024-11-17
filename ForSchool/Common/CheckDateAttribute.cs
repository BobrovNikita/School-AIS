using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForSchool.Common
{
    public class CheckDateAttribute : ValidationAttribute
    {
        public CheckDateAttribute()
        {

        }
        public override bool IsValid(object? value)
        {
            if (value != null)
            {
                var dt = (DateTime)value;

                if (dt.Day >= DateTime.Now.Day-1 && dt.Month >= DateTime.Now.Month && dt.Year <= DateTime.Now.Year + 10)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
