using EveLucrum.Domain;

namespace EveLucrum.ApplicationServices
{
    public interface IMarketService
    {
        IRepository Repository { get; }
        void GetLatestPricesForAllItems();
    }
}