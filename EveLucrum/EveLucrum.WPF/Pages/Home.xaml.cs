using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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
using EveLucrum.Infrastructure;
using EveLucrum.Infrastructure.API;
using EveLucrum.Infrastructure.Market;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FragmentNavigationEventArgs = FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs;
using NavigatingCancelEventArgs = FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs;
using NavigationEventArgs = FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs;

namespace EveLucrum.WPF.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl, IContent
    {
        private readonly IMarketService marketService;
        private int minimumVolumeFilter = 0;

        private static readonly Dictionary<int, int> volumeSliderTranslation = new Dictionary<int, int>() {
            {0, 0}, {1, 10}, {2, 100}, {3, 250}, {4, 500}, {5, 1000}, {6, 5000}, {7, 10000}, {8, 25000}, {9, 100000}, {10, 250000}
        };
        public Home()
        {
            InitializeComponent();
            marketService = DependencyResolver.Get<IMarketService>();
        }

        private void UpdateSelectedCharacter()
        {
            var character =
                marketService.Repository.Characters.FirstOrDefault(c => c.CharacterID == UserSelections.CharacterID);

            SelectedCharacterLabel.Content = character == null ? "Level 5 Character" : character.Name;
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
            UpdateSelectedCharacter();
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            return;
        }

        private void SelectCharacterClick(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("/Pages/Characters.xaml", null);
        }

        private void ItemIconClick(object sender, RoutedEventArgs e)
        {
            return;
        }

        private void UpdateItems(int systemID)
        {
            var itemPrices = marketService.GetItemPrices(systemID);
            int accounting = 5;
            int brokerRelations = 5;
            var selectedCharacter =
                marketService.Repository.Characters.FirstOrDefault(c => c.CharacterID == UserSelections.CharacterID);
            if (selectedCharacter != null)
            {
                accounting = selectedCharacter.AccountingSkill;
                brokerRelations = selectedCharacter.BrokerRelationsSkill;
            }

            var profitCalculator = new ProfitCalculator(accounting, brokerRelations);

            var dataSource = new ObservableCollection<ItemViewModel>(itemPrices.Select(i => new ItemViewModel()
                {
                    ItemTypeID = i.ItemType.ItemTypeID,
                    Name = i.ItemType.Name,
                    Description = i.ItemType.Description,
                    Buy = i.MaxBuy,
                    Sell = i.MinSell,
                    BuyVolume = i.BuyVolume,
                    SellVolume = i.SellVolume,
                    Margin = profitCalculator.CalculateMargin(i.MaxBuy, i.MinSell),
                    AdjustedMargin = profitCalculator.CalculateAdjustedMargin(i.MaxBuy, i.MinSell, i.BuyVolume, i.SellVolume)
                }).ToList());

            Dispatcher.Invoke(() =>
                {
                    ItemsDataGrid.DataContext = dataSource;
                    var dataView = CollectionViewSource.GetDefaultView(dataSource);
                    dataView.SortDescriptions.Clear();
                    dataView.SortDescriptions.Add(new SortDescription("AdjustedMargin", ListSortDirection.Descending));
                    dataView.Refresh();
                });
        }

        private void CalculateButton_OnClick(object sender, RoutedEventArgs e)
        {
            CalculateProgressBar.Visibility = Visibility.Visible;
            CalculateProgressBar.IsIndeterminate = true;
            CalculateProgressBar.IsEnabled = false;
            int systemID = getSelectedSystemID();
            var updateTask = Task.Factory.StartNew(() => UpdateItems(systemID));
            updateTask.ContinueWith(delegate
            {
                Dispatcher.Invoke(() =>
                {
                    CalculateProgressBar.Visibility = Visibility.Hidden;
                    CalculateProgressBar.IsIndeterminate = false;
                    CalculateProgressBar.IsEnabled = true;
                });
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        private int getSelectedSystemID()
        {
            var x = SystemComboBox.SelectedValue;

            return (int)SystemTypes.Jita;
        }

        private void VolumeSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VolumeLabel.Content = volumeSliderTranslation[(int) VolumeSlider.Value].ToString("#,##0");
        }


    }

    public class ItemViewModel
    {
        public int ItemTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Buy { get; set; }
        public decimal Sell { get; set; }
        public long BuyVolume { get; set; }
        public long SellVolume { get; set; }
        public decimal Margin { get; set; }
        public decimal AdjustedMargin { get; set; }
        public decimal Ratio
        {
            get
            {
                if (BuyVolume == 0)
                    return decimal.MaxValue;
                var b = new decimal(BuyVolume);
                var s = new decimal(SellVolume);
                return Math.Min(s, b) / Math.Max(s, b);
            }
        }

        public string BuyVolumeDisplay { get { return BuyVolume.AbbreviatedFormat(); } }
        public string SellVolumeDisplay { get { return SellVolume.AbbreviatedFormat(); } }
        public string RatioDisplay { get { return BuyVolume != 0 ? (new decimal(SellVolume) / new decimal(BuyVolume)).ToString() : "0"; } }
    }
}
