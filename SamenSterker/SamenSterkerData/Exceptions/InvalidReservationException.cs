using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamenSterkerData.Exceptions
{
    public class InvalidReservationException : Exception
    {
        public InvalidReservationException() : base() { }
        public InvalidReservationException(string message) : base(message) { }
        public InvalidReservationException(string message, Exception e) : base(message, e) { }
    }
}
