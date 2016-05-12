using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passbook.Generator.Exceptions
{
    [Serializable]
    public class MissingStandardKeyException : Exception
    {
        public MissingStandardKeyException(string key) :
            base("Missing value for standard key [key]: '" + key + "']. Every standard key must have a value specified in the template or the individual pass.")
        { }
    }
}
