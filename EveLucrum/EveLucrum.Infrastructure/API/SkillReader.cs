using System;
using System.Linq;
using EVE.Net.Character;

namespace EveLucrum.Infrastructure.API
{
    public class SkillReader : APIHelper
    {
        public SkillReader(string keyID, string vCode) : base(keyID, vCode)
        {
        }

        /// <summary>
        /// Gets the accounting skill level and broker skill level of a character.
        /// </summary>
        /// <param name="actorID"></param>
        /// <returns>Tuple<Accounting,Broker></returns>
        public Tuple<int, int> GetTradingSkillLevels(long actorID)
        {
            var api = new CharacterSheet(keyID, vCode, actorID.ToString());
            if (!api.Query())
                throw new InvalidOperationException("Error querying character information.");

            var accounting = api.skills.FirstOrDefault(s => s.typeID == (decimal) SkillTypes.Accounting);
            var accountingLevel = accounting != null ? accounting.level : 0;

            var brokerRelations = api.skills.FirstOrDefault(s => s.typeID == (decimal)SkillTypes.BrokerRelations);
            var brokerRelationsLevel = brokerRelations != null ? brokerRelations.level : 0; 

            return new Tuple<int, int>(accountingLevel, brokerRelationsLevel);
        }
    }

    
}

