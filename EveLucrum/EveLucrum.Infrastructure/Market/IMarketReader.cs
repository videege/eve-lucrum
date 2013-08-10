using System.Collections.Generic;

namespace EveLucrum.Infrastructure.Market
{
    public interface IMarketReader
    {
        IEnumerable<PriceDTO> GetPricesForItems(IEnumerable<int> itemTypeIDs, int systemID);
    }
}