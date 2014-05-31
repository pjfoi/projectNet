using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SamenSterkerData
{
    /// <summary>
    /// A Reservation Model.
    /// </summary>
    public class Reservation : ModelValidation
    {
        private int id;
        private int number;
        private DateTime startDate;
        private DateTime endDate;
        private DateTime createDate;
        private Location location;
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
        [Required]
        public int Number
        {
            get { return number; }
            set
            {
                number = value;
                ValidateProperty(value);
            }
        }

        /// <summary>
        /// The start date and time of the reservation.
        /// </summary>
        [Required]
        public DateTime StartDate
        {
            get { return startDate; }
            set 
            {
                startDate = value;
                ValidateProperty(value);
            }
        }

        /// <summary>
        /// The end date and time of the reservation.
        /// </summary>
        [Required]
        [Foolproof.GreaterThan("StartDate")]
        public DateTime EndDate
        {
            get { return endDate; }
            set 
            {
                endDate = value;
                ValidateProperty(value);
            }
        }

        /// <summary>
        /// The creation date and time of the reservation.
        /// </summary>
        public DateTime CreateDate
        {
            get { return createDate; }
            set
            {
                createDate = value;
                ValidateProperty(value);
            }
        }

        /// <summary>
        /// The location of the reservation.
        /// </summary>
        [Required]
        public Location Location
        {
            get { return location; }
            set
            {
                location = value;
                ValidateProperty(value);
            }
        }

        /// <summary>
        /// The id of the location of the reservation.
        /// </summary>
        public int LocationId
        {
            get { return (Location == null) ? 0 : Location.Id; }
        }

        /// <summary>
        /// The company of the reservation.
        /// </summary>
        [Required]
        public Company Company
        {
            get { return company; }
            set
            {
                company = value;
                ValidateProperty(value);
            }
        }

        /// <summary>
        /// The id of the company of the reservation.
        /// </summary>
        public int CompanyId
        {
            get { return (Company == null) ? 0 : Company.Id; }
        }

        /// <summary>
        /// Is the specified object equal to the reservation.
        /// </summary>
        /// <param name="obj">Object to check for equality</param>
        /// <returns>Equal or not</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Reservation))
                return false;

            return ((Reservation)obj).Id == this.Id;
        }

        /// <summary>
        /// Get a hashcode for the reservation.
        /// </summary>
        /// <returns>An hashcode</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}
