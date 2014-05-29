using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamenSterkerData
{

    /// <summary>
    /// A Location Model.
    /// </summary>
    public class Location
    {
        private int id;
        private string name;

        /// <summary>
        /// The id of the loation.
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// The name of the location.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
