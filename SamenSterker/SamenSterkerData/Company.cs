﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamenSterkerData
{
    public class Company
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
            set { email = value; }
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
    }
}
