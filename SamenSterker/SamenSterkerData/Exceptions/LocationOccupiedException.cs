using System;

namespace SamenSterkerData.Exceptions
{
    public class LocationOccupiedException : InvalidReservationException
    {
        public LocationOccupiedException() : base() { }
        public LocationOccupiedException(string message) : base(message) { }
        public LocationOccupiedException(string message, Exception e) : base(message, e) { }
    }
}
