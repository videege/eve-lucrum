using System;

namespace EveLucrum.Domain.Entities
{
    public class ItemPrice
    {
        public int ItemPriceID { get; set; }
        public virtual ItemType ItemType { get; set; }

        public DateTime PriceDate { get; set; }
        public int SystemID { get; set; }

        public decimal MinSell { get; set; }
        public decimal MaxBuy { get; set; }

        public long BuyVolume { get; set; }
        public long SellVolume { get; set; }
        public long TotalVolume { get; set; }

        public decimal Margin { get { return MinSell - MaxBuy; } }
    }
}