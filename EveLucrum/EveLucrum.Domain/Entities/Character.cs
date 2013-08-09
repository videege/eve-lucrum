using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveLucrum.Domain.Entities
{
    public class Character
    {
        public int CharacterID { get; set; }

        public virtual Account Account { get; set; }

        public long ActorID { get; set; }

        public string Name { get; set; }

        public string CorporationName { get; set; }
    }
}
