using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EveLucrum.Domain;
using EveLucrum.Domain.Entities;
using EveLucrum.Infrastructure.API;
using EveLucrum.Infrastructure.Market;

namespace EveLucrum.ApplicationServices
{
    public class MarketService : IMarketService
    {
        private readonly ILucrumContext context;
        private readonly IMarketReader marketReader;

        public MarketService(ILucrumContext context, IMarketReader marketReader)
        {
            this.context = context;
            this.marketReader = marketReader;
        }

        public IRepository Repository { get { return context; } }

        public void GetLatestPricesForAllItems()
        {
            var itemIDs = context.ItemTypes.Select(i => i.TypeID).ToList();
            var items = context.ItemTypes.ToDictionary(k => k.TypeID, v => v);
            var priceDTOs = marketReader.GetPricesForItems(itemIDs, (int) SystemTypes.Jita);

            foreach (var priceDTO in priceDTOs)
            {
                var price = new ItemPrice()
                    {
                        ItemType = items[priceDTO.TypeID],
                        MaxBuy = priceDTO.MaxBuy,
                        MinSell = priceDTO.MinSell,
                        BuyVolume = priceDTO.BuyVolume,
                        SellVolume = priceDTO.SellVolume,
                        TotalVolume = priceDTO.TotalVolume,
                        PriceDate = priceDTO.PriceDate,
                        SystemID = (int) SystemTypes.Jita
                    };
                context.Add(price);
            }

            context.SaveChanges();
        }
    }
}
