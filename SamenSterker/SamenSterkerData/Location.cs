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


        /// <summary>
        /// Is the specified object equal to the location.
        /// </summary>
        /// <param name="obj">Object to check for equality</param>
        /// <returns>Equal or not</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Location))
                return false;

            return ((Location)obj).Id == this.Id;
        }

        /// <summary>
        /// Get a hashcode for the location.
        /// </summary>
        /// <returns>An hashcode</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
