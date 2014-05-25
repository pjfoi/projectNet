using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamenSterkerData
{
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

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        

        public string Street
        {
            get { return street; }
            set { street = value; }
        }
        
        public int Zipcode
        {
            get { return zipcode; }
            set { zipcode = value; }
        }
        
        public string City
        {
            get { return city; }
            set { city = value; }
        }
        
        public string Country
        {
            get { return country; }
            set { country = value; }
        }
        
        public string Email
        {
            get { return email; }
            set
            { 
                email = value;
                ValidateEmailAddress(email);
            }
        }
        
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public int Employees
        {
            get { return employees; }
            set { employees = value; }
        }

        public Company()
        {
        }

        public override String ToString() 
        {
            return String.Format("{0} ({1})", Name, Id);
        }

        public void ValidateEmailAddress(string email)
        {
            System.Diagnostics.Debug.WriteLine("Validate email " + email, "Company");

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
