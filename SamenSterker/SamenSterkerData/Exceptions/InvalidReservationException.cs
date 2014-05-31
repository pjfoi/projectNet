using System;

namespace SamenSterkerData.Exceptions
{
    public class InvalidReservationException : Exception
    {
        public InvalidReservationException() : base() { }
        public InvalidReservationException(string message) : base(message) { }
        public InvalidReservationException(string message, Exception e) : base(message, e) { }
    }
}
