using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EveLucrum.Infrastructure.Market
{
    public class EveCentralReader : IMarketReader
    {
        private static string baseAPIUrl = "http://api.eve-central.com/api/marketstat";
        public IEnumerable<PriceDTO> GetPricesForItems(IEnumerable<int> itemTypeIDs, int systemID)
        {
            //partition the types into 100 item slices
            var partitions = itemTypeIDs.Partition(100).ToList();

            var results = new List<PriceDTO>();

            Parallel.ForEach(partitions, (partition) =>
                {
                    var dtos = processPartition(partition, systemID);

                    lock (this)
                    {
                        results.AddRange(dtos);
                    }
                });

            return results;
        }

        private IEnumerable<PriceDTO> processPartition(IEnumerable<int> partition, int systemID)
        {
            var url = getAPIUrl(partition, systemID);
            
            var document = XDocument.Load(url);

            var items = document.Descendants("type");
            
            return items.Select(i => new PriceDTO()
                {
                    TypeID = int.Parse(i.Attribute("id").Value),
                    MaxBuy = decimal.Parse(i.Element("buy").Element("max").Value),
                    BuyVolume = long.Parse(i.Element("buy").Element("volume").Value),
                    MinSell = decimal.Parse(i.Element("sell").Element("min").Value),
                    SellVolume = long.Parse(i.Element("sell").Element("volume").Value),
                    TotalVolume = long.Parse(i.Element("all").Element("volume").Value)
                }).ToList();
        }

        private string getAPIUrl(IEnumerable<int> itemTypeIDs, int systemID)
        {
            var itemString = string.Join("&", itemTypeIDs.Select(s => "typeid=" + s.ToString(CultureInfo.InvariantCulture)));
            var systemString = "&usesystem=" + systemID.ToString(CultureInfo.InvariantCulture);
            return baseAPIUrl + "?" + itemString + systemString;
        }
    }

    public class PriceDTO
    {
        public PriceDTO()
        {
            PriceDate = DateTime.Now;
        }

        public int TypeID { get; set; }
        public DateTime PriceDate { get; set; }

        public decimal MinSell { get; set; }
        public decimal MaxBuy { get; set; }

        public long BuyVolume { get; set; }
        public long SellVolume { get; set; }
        public long TotalVolume { get; set; }
    }
}
