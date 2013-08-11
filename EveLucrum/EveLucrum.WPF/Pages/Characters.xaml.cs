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
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FragmentNavigationEventArgs = FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs;
using NavigatingCancelEventArgs = FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs;
using NavigationEventArgs = FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs;

namespace EveLucrum.WPF.Pages
{
    /// <summary>
    /// Interaction logic for Characters.xaml
    /// </summary>
    public partial class Characters : UserControl, IContent
    {
        private readonly IAPIService apiService;
        public Characters()
        {
            InitializeComponent();
            apiService = DependencyResolver.Get<IAPIService>();

            UpdateGrid();
        }

        private void UpdateGrid()
        {
            ObservableCollection<CharacterViewModel> charData = GetData();
            CharactersDataGrid.DataContext = charData;
        }

        private ObservableCollection<CharacterViewModel> GetData()
        {
            var characters =
                apiService.Repository.Characters.OrderBy(c => c.Account.AccountID).ThenBy(a => a.Name).ToList();

            var viewModels = new ObservableCollection<CharacterViewModel>(characters.Select(a => new CharacterViewModel()
                {
                    CharacterID = a.CharacterID,
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
            var viewModel = ((FrameworkElement)sender).DataContext as CharacterViewModel;

            if (ModernDialog.ShowMessage("Are you sure you wish to delete this character?", "Delete " + viewModel.Name,
                                         MessageBoxButton.YesNo)
                == MessageBoxResult.Yes)
            {
                if (UserSelections.CharacterID == viewModel.CharacterID)
                    UserSelections.CharacterID = 0;
                apiService.DeleteCharacter(viewModel.CharacterID);
                UpdateGrid();
            }
        }


        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            return;
        }

        public void OnNavigatedFrom(NavigationEventArgs e)
        {
            return;
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            UpdateGrid();
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            return;
        }

        private void SelectIconClick(object sender, RoutedEventArgs e)
        {
            var viewModel = ((FrameworkElement)sender).DataContext as CharacterViewModel;

            UserSelections.CharacterID = viewModel.CharacterID;
            ModernDialog.ShowMessage(viewModel.Name + " is now the current character.", "Character Selected",
                                     MessageBoxButton.OK);
        }
    }

    public class CharacterViewModel
    {
        public int CharacterID { get; set; }
        public string Name { get; set; }
        public string Corporation { get; set; }
        public int Accounting { get; set; }
        public int BrokerRelations { get; set; }
        public string APIID { get; set; }
        public string APIVCODE { get; set; }
    }

}
