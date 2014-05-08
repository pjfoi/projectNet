using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamenSterkerData
{
    class Company
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string street;

        public string Street
        {
            get { return street; }
            set { street = value; }
        }
        private int zipcode;

        public int Zipcode
        {
            get { return zipcode; }
            set { zipcode = value; }
        }
        private string city;

        public string City
        {
            get { return city; }
            set { city = value; }
        }
        private string country;

        public string Country
        {
            get { return country; }
            set { country = value; }
        }
        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        private string phone;

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        private int employees;

        public int Employees
        {
            get { return employees; }
            set { employees = value; }
        }

        public Company()
        {
        }
    }
}
