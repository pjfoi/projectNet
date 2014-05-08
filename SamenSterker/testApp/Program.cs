using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testApp
{
    class Program
    {
        static void Main(string[] args)
        {
            SamenSterkerData.Company c1 = SamenSterkerData.CompanyDB.GetById(1);
            Console.WriteLine(c1.Name);
            Console.Read();
        }
    }
}
