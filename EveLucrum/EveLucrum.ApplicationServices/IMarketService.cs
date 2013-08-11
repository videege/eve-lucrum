using System.Collections.Generic;
using EveLucrum.Domain;
using EveLucrum.Domain.Entities;

namespace EveLucrum.ApplicationServices
{
    public interface IMarketService
    {
        IRepository Repository { get; }
        int UpdatePricesForAllItems(int systemID);
        IEnumerable<ItemPrice> GetItemPrices(int systemID);
    }
}