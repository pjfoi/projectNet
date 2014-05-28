using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamenSterkerData.Exceptions
{
    public class LocationOccupiedException : InvalidReservationException
    {
        public LocationOccupiedException() : base() { }
        public LocationOccupiedException(string message) : base(message) { }
        public LocationOccupiedException(string message, Exception e) : base(message, e) { }
    }
}
