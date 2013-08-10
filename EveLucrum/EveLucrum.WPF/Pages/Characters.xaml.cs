using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using EveLucrum.Domain;
using FirstFloor.ModernUI.Windows.Controls;

namespace EveLucrum.WPF.Pages
{
    /// <summary>
    /// Interaction logic for Characters.xaml
    /// </summary>
    public partial class Characters : UserControl
    {
        private readonly IAPIService apiService;
        public Characters()
        {
            InitializeComponent();
            apiService = DependencyResolver.Get<IAPIService>();

            ObservableCollection<CharacterViewModel> charData = GetData();
            CharactersDataGrid.DataContext = charData;
        }

        private ObservableCollection<CharacterViewModel> GetData()
        {
            var characters =
                apiService.Repository.Characters.OrderBy(c => c.Account.AccountID).ThenBy(a => a.Name).ToList();

            var viewModels = new ObservableCollection<CharacterViewModel>(characters.Select(a => new CharacterViewModel()
                {
                    Name = a.Name,
                    Corporation = a.CorporationName,
                    Accounting = a.AccountingSkill,
                    BrokerRelations = a.BrokerRelationsSkill,
                    APIID = a.Account.KeyID,
                    APIVCODE = a.Account.VerificationCode
                }).ToList());
            return viewModels;
        }

        private void InfoIconClick(object sender, RoutedEventArgs e)
        {
            var viewModel = ((FrameworkElement)sender).DataContext as CharacterViewModel;

            ModernDialog.ShowMessage("ID: " + viewModel.APIID + "\nvCode: " + viewModel.APIVCODE,
                                     "API Information for " + viewModel.Name, MessageBoxButton.OK);
        }

        private void DeleteIconClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }

    public class CharacterViewModel
    {
        public string Name { get; set; }
        public string Corporation { get; set; }
        public int Accounting { get; set; }
        public int BrokerRelations { get; set; }
        public string APIID { get; set; }
        public string APIVCODE { get; set; }
    }

}
