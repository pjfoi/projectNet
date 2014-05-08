using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamenSterkerData
{
    class Reservation
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private int number;

        public int Number
        {
            get { return number; }
            set { number = value; }
        }
        private DateTime startDate;

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }
        private DateTime endDate;

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }
        private int locationId;

        public int LocationId
        {
            get { return locationId; }
            set { locationId = value; }
        }
        private DateTime createDate;

        public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }

        public Reservation()
        {
        }
    }
}
