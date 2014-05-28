using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamenSterkerData.Exceptions
{
    public class InvalidContractException : Exception
    {
        public InvalidContractException() : base() { }
        public InvalidContractException(string message) : base(message) { }
        public InvalidContractException(string message, Exception e) : base(message, e) { }
    }
}
