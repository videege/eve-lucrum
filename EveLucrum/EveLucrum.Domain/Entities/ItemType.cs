using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveLucrum.Domain.Entities
{
    public class ItemType
    {
        public int ItemTypeID { get; set; }
        public int TypeID { get; set; }
        public int GroupID { get; set; }
        public int MarketGroupID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ItemPrice> ItemPrices { get; set; }

        public ItemPrice LatestItemPrice
        {
            get { return ItemPrices.OrderByDescending(d => d.PriceDate).FirstOrDefault(); }
        }
    }
}
