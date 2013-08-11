using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveLucrum.Infrastructure.Market
{
    public class ProfitCalculator
    {
        private int accounting;
        private int brokerRelations;
        private decimal marketTax;
        private decimal brokerFee;
        /*elling price - Cost - Taxes - Broker Fees = Profit
        */

        public ProfitCalculator(int accounting, int brokerRelations)
        {
            this.accounting = accounting;
            this.brokerRelations = brokerRelations;
            //MarketTax % = 1.00 % - 0.10 % * AccountingSkillLevel
            //BrokerFee % = (1.00 % – 0.05 % × BrokerRelationsSkillLevel) / e ^ (0.10 × FactionStanding + 0.04 × CorporationStanding)
            marketTax = .01m - (.001m * new decimal(accounting));
            brokerFee = .01m - (.0005m * new decimal(brokerRelations));
        }


        public decimal CalculateMargin(decimal maxBuy, decimal minSell)
        {
            var buyCost = maxBuy + (maxBuy * brokerFee);
            var sellTotal = minSell - (minSell * brokerFee) - (minSell * marketTax);

            return sellTotal - buyCost;
        }


        public decimal CalculateAdjustedMargin(decimal maxBuy, decimal minSell, long buyVolume, long sellVolume)
        {
            var baseMargin = CalculateMargin(maxBuy, minSell);
            var b = new decimal(buyVolume);
            var s = new decimal(sellVolume);
            var v = b + s;

            //Adjusted Margin = Margin * log(Volume) * [(min BuyVolume, SellVolume) / (max BuyVolume, SellVolume)]
            return baseMargin * new decimal(Math.Log10((double)v)) * (Math.Min(b, s) / Math.Max(b, s));
        }
    }
}
