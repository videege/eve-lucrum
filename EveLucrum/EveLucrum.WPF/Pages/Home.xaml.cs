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
using EveLucrum.ApplicationServices;
using EveLucrum.Data;
using EveLucrum.Domain;
using EveLucrum.Domain.Entities;
using EveLucrum.Infrastructure.API;
using EveLucrum.Infrastructure.Market;
using FirstFloor.ModernUI.Windows.Controls;

namespace EveLucrum.WPF.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();

            //var service = DependencyResolver.Get<IAPIService>();
            //var context = DependencyResolver.Get<ILucrumContext>();
            //var marketService = DependencyResolver.Get<IMarketService>();

            //var a = context.Accounts.First();
            //service.UpdateCharacterList(a.AccountID);
            //var x = a.Characters.ToList();

            //marketService.GetLatestPricesForAllItems();

            //var fusionS = marketService.Repository.ItemTypes.First(i => i.TypeID == 183);

            //ModernDialog.ShowMessage(a.VerificationCode, "vcode", MessageBoxButton.OK);
        }

    }
}
