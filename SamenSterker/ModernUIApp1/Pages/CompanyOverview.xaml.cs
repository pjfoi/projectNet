using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UserInteface.Pages
{
    /// <summary>
    /// Interaction logic for CompanyOverview.xaml
    /// </summary>
    public partial class CompanyOverview : UserControl
    {
        public CompanyOverview()
        {
            InitializeComponent();

            // create and assign the view model
            this.DataContext = new CompanyOverviewViewModel();
        }
    }
}
