using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passbook.Generator.Exceptions
{
    public class RequiredIconImageMissingException : Exception
    {
        public RequiredIconImageMissingException() :
            base("Missing icon.png/icon@2x.png image. This value is required for every pass to work on iOS.")
        { }
    }
}
