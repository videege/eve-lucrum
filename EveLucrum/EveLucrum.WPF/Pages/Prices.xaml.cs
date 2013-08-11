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
using EveLucrum.Infrastructure.API;
using FirstFloor.ModernUI.Windows.Controls;

namespace EveLucrum.WPF.Pages
{
    /// <summary>
    /// Interaction logic for Prices.xaml
    /// </summary>
    public partial class Prices : UserControl
    {
        private IMarketService marketService;

        public Prices()
        {
            InitializeComponent();
            marketService = DependencyResolver.Get<IMarketService>();
        }

        private void ImportPricesButton_OnClick(object sender, RoutedEventArgs e)
        {
            ImportProgressBar.Visibility = Visibility.Visible;
            ImportProgressBar.IsIndeterminate = true;
            ImportPricesButton.IsEnabled = false;
            int itemsUpdated = 0;
            int systemID = getCheckedSystemID();
            var updateTask = Task.Factory.StartNew(() => itemsUpdated = marketService.UpdatePricesForAllItems(systemID));
            updateTask.ContinueWith(delegate
                {
                    Dispatcher.Invoke(() =>
                        {
                            ImportProgressBar.Visibility = Visibility.Hidden;
                            ImportProgressBar.IsIndeterminate = false;
                            ModernDialog.ShowMessage("Prices successfully refreshed.  Updated " + itemsUpdated.ToString() + " items.", "Import Successful", MessageBoxButton.OK);
                            ImportPricesButton.IsEnabled = true;
                        });
                }, TaskContinuationOptions.OnlyOnRanToCompletion);

        }

        private int getCheckedSystemID()
        {
            if (JitaButton.IsChecked.GetValueOrDefault(false))
                return (int) SystemTypes.Jita;

            if (AmarrButton.IsChecked.GetValueOrDefault(false))
                return (int) SystemTypes.Jita;  //todo change

            return (int) SystemTypes.Jita;
        }
    }
}
