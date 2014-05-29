﻿
namespace SamenSterkerData
{
    /// <summary>
    /// A User Model.
    /// </summary>
    public class User : BaseModel
    {
        private int id;
        private string username;
        private int companyId;
        private Company company;

        /// <summary>
        /// The id of the user.
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// The username of the user.
        /// </summary>
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        /// <summary>
        /// The id of the company of the user.
        /// </summary>
        public int CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }

        /// <summary>
        /// The company of the user.
        /// </summary>
        public Company Company
        {
            get { return company;  }
            set { company = value;  }
        }
    }
}
