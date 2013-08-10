using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EveLucrum.Domain.Entities;

namespace EveLucrum.Domain
{
    public interface IRepository
    {
        IQueryable<Account> Accounts { get; }
        IQueryable<Character> Characters { get; }
        IQueryable<ItemType> ItemTypes { get; }
        IQueryable<ItemPrice> ItemPrices { get; }
    }
}
