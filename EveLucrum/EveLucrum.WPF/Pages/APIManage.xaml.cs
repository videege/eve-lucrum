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
using FirstFloor.ModernUI.Windows.Controls;

namespace EveLucrum.WPF.Pages
{
    /// <summary>
    /// Interaction logic for APIManage.xaml
    /// </summary>
    public partial class APIManage : UserControl
    {
        private readonly IAPIService apiService;
        public APIManage()
        {
            InitializeComponent();
            apiService = DependencyResolver.Get<IAPIService>();
        }

        private void ImportCharactersButton_OnClick(object sender, RoutedEventArgs e)
        {
            var keyID = KeyIDTextBox.Text;
            var vCode = VCodeTextBox.Text;

            if (string.IsNullOrWhiteSpace(keyID))
            {
                ModernDialog.ShowMessage("You must enter a key ID.", "Error", MessageBoxButton.OK);
                return;
            }

            if (string.IsNullOrWhiteSpace(vCode))
            {
                ModernDialog.ShowMessage("You must enter a vCode.", "Error", MessageBoxButton.OK);
                return;
            }

            try
            {
                var account = apiService.AddOrUpdateAccount(keyID, vCode);
                apiService.UpdateCharacterList(account.AccountID);

                var characters = string.Join(", ", account.Characters.Select(a => a.Name));

                ModernDialog.ShowMessage("Imported or updated information for characters: " + characters,
                                         "Import successful", MessageBoxButton.OK);
            }
            catch (Exception)
            {
                ModernDialog.ShowMessage("Error accessing API - are you sure you entered correct key information?",
                                         "Error", MessageBoxButton.OK);
            }
            
        }
    }
}
