using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveLucrum.Infrastructure
{
    public interface ICharacterValuesCalculator
    {
        decimal GetMarketTax();

        decimal GetBrokerFee();
    }
}
