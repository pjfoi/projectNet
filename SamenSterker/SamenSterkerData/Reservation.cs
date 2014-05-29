using System;
using System.Collections.Generic;

namespace SamenSterkerData
{
    /// <summary>
    /// A Reservation Model.
    /// </summary>
    public class Reservation : BaseModel
    {
        private int id;
        private int number;
        private DateTime startDate;
        private DateTime endDate;
        private int locationId;
        private Location location;
        private int companyId;
        private Company company;

        /// <summary>
        /// The id of the reservation.
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// The number of the reservation.
        /// </summary>
        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        /// <summary>
        /// The start date and time of the reservation.
        /// </summary>
        public DateTime StartDate
        {
            get { return startDate; }
            set 
            {
                startDate = value;
                ValidateEndDate(EndDate);
            }
        }

        /// <summary>
        /// The end date and time of the reservation.
        /// </summary>
        public DateTime EndDate
        {
            get { return endDate; }
            set 
            { 
                endDate = value;
                ValidateEndDate(endDate);
            }
        }

        /// <summary>
        /// The id of the location of the reservation.
        /// </summary>
        public int LocationId
        {
            get { return locationId; }
            set { locationId = value; }
        }

        /// <summary>
        /// The location of the reservation.
        /// </summary>
        public Location Location
        {
            get { return location; }
            set { location = value; }
        }

        /// <summary>
        /// The id of the company of the reservation.
        /// </summary>
        public int CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }

        /// <summary>
        /// The company of the reservation.
        /// </summary>
        public Company Company
        {
            get { return company; }
            set { company = value; }
        }

        /// <summary>
        /// The creation date and time of the reservation.
        /// </summary>
        private DateTime createDate;
        public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
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
