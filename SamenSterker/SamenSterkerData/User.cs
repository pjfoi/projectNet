using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamenSterkerData
{
    public class User : BaseModel
    {
        private int id;
        private string username;
        private int companyId;
        private Company company;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public int CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }

        public Company Company
        {
            get { return company;  }
            set { company = value;  }
        }
    }
}
