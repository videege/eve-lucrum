namespace EveLucrum.Infrastructure
{
    public class MaxedCharacterValuesCalculator : ICharacterValuesCalculator
    {
        public decimal GetMarketTax()
        {
            return new decimal(0.005d);
        }

        public decimal GetBrokerFee()
        {
            return new decimal(.0075d);
        }
    }
}