using System;
using System.Collections.Generic;

namespace SamenSterkerData
{
    /// <summary>
    /// A company model 
    /// </summary>
    public class Company : BaseModel
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
            set { id = value; }
        }
        
        /// <summary>
        /// The name of the company.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        
        /// <summary>
        /// The street where the company is located.
        /// </summary>
        public string Street
        {
            get { return street; }
            set { street = value; }
        }
        
        /// <summary>
        /// The zipcode of the city where the company is
        /// located.
        /// </summary>
        public int Zipcode
        {
            get { return zipcode; }
            set { zipcode = value; }
        }
        
        /// <summary>
        /// The city where the company is located.
        /// </summary>
        public string City
        {
            get { return city; }
            set { city = value; }
        }
        
        /// <summary>
        /// The country where the company is located.
        /// </summary>
        public string Country
        {
            get { return country; }
            set { country = value; }
        }
        
        /// <summary>
        /// The main email address of the company.
        /// </summary>
        public string Email
        {
            get { return email; }
            set
            { 
                email = value;
                ValidateEmailAddress(email);
            }
        }
        
        /// <summary>
        /// The telephone number of the company.
        /// </summary>
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        /// <summary>
        /// The number of employees of the company.
        /// </summary>
        public int Employees
        {
            get { return employees; }
            set { employees = value; }
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
        /// Checks if the specified email address is valid. 
        /// If not a validation error is stored.
        /// </summary>
        /// <param name="email">An email address.</param>
        public void ValidateEmailAddress(string email)
        {
            // source regex : http://stackoverflow.com/a/6893571
            string regex = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                           + "@"
                           + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

            ICollection<string> validationErrors = new List<string>();

            if (! System.Text.RegularExpressions.Regex.IsMatch(email, regex))
            {
                validationErrors.Add("Gelieve een geldig emailadres in te vullen.");
            }

            UpdateValidationErrors("Email", validationErrors);
        }
    }
}
