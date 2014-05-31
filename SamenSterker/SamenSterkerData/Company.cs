using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SamenSterkerData
{
    /// <summary>
    /// A company model 
    /// </summary>
    public class Company : ModelValidation
    {
        private int id;
        private string name;
        private string street;
        private int zipcode;
        private string city;
        private string country;
        private string email;
        private string phone;
        private int employees;

        /// <summary>
        /// The id of the company.
        /// </summary>
        public int Id
        {
            get { return id; }
            set 
            {
                id = value;
                ValidateProperty(value);
            }
        }
        
        /// <summary>
        /// The name of the company.
        /// </sum-mary>
        [Required]
        public string Name
        {
            get { return name; }
            set 
            { 
                name = value;
                ValidateProperty(value);
            }
        }
        
        /// <summary>
        /// The street where the company is located.
        /// </summary>
        [Required]
        public string Street
        {
            get { return street; }
            set 
            { 
                street = value;
                ValidateProperty(value);
            }
        }
        
        /// <summary>
        /// The zipcode of the city where the company is
        /// located.
        /// </summary>
        [Required]
        public int Zipcode
        {
            get { return zipcode; }
            set 
            { 
                zipcode = value;
                ValidateProperty(value);
            }
        }
        
        /// <summary>
        /// The city where the company is located.
        /// </summary>
        [Required]
        public string City
        {
            get { return city; }
            set 
            { 
                city = value;
                ValidateProperty(value);
            }
        }
        
        /// <summary>
        /// The country where the company is located.
        /// </summary>
        [Required]
        public string Country
        {
            get { return country; }
            set { country = value; }
        }
        
        /// <summary>
        /// The main email address of the company.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email
        {
            get { return email; }
            set
            { 
                email = value;
                ValidateProperty(value);
            }
        }
        
        /// <summary>
        /// The telephone number of the company.
        /// </summary>
        [Required]
        public string Phone
        {
            get { return phone; }
            set 
            { 
                phone = value;
                ValidateProperty(value);
            }
        }

        /// <summary>
        /// The number of employees of the company.
        /// </summary>
        [Required]
        public int Employees
        {
            get { return employees; }
            set 
            { 
                employees = value;
                ValidateProperty(value);
            }
        }

        /// <summary>
        /// Get a textual respresentation of the company.
        /// </summary>
        /// <returns></returns>
        public override String ToString() 
        {
            return String.Format("{0} ({1})", Name, Id);
        }


        /// <summary>
        /// Is the specified object equal to the company.
        /// </summary>
        /// <param name="obj">Object to check for equality</param>
        /// <returns>Equal or not</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Company))
                return false;

            return ((Company)obj).Id == this.Id;
        }

        /// <summary>
        /// Get a hashcode for the company.
        /// </summary>
        /// <returns>An hashcode</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}
