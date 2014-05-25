using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamenSterkerData
{
    public class Reservation : BaseModel
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
            set 
            {
                startDate = value;
                ValidateEndDate(EndDate);
            }
        }

        private DateTime endDate;
        public DateTime EndDate
        {
            get { return endDate; }
            set 
            { 
                endDate = value;
                ValidateEndDate(endDate);
            }
        }

        private int locationId;
        public int LocationId
        {
            get { return locationId; }
            set { locationId = value; }
        }

        private Location location;

        public Location Location
        {
            get { return location; }
            set { location = value; }
        }

        private int companyId;
        public int CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }

        private Company company;
        public Company Company
        {
            get { return company; }
            set { company = value; }
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

        private void ValidateEndDate(DateTime endDate)
        {
            ICollection<string> validationErrors = new List<string>();

            if (StartDate != null && !(endDate > StartDate))
            {
                validationErrors.Add("Het einde van de reservatie moet na het begin liggen.");
            }

            UpdateValidationErrors("EndDate", validationErrors);
        }
    }
}
