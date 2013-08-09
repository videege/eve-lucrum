using System;
using System.Collections.Generic;
using System.Linq;
using EVE.Net.Account;

namespace EveLucrum.Infrastructure.API
{
    public class AccountReader : APIHelper
    {
        public AccountReader(string keyID, string vCode) : base(keyID, vCode)
        {
        }

        public IEnumerable<CharacterDTO> GetCharacters()
        {
            var api = new Characters(keyID, vCode);
            if (!api.Query())
                throw new InvalidOperationException("Error querying account information.");

            return api.characters.Select(c => new CharacterDTO()
                {
                    CharacterID = c.characterID,
                    Name = c.name,
                    CorporationName = c.corporationName
                });
        }
    }

    public class CharacterDTO
    {
        public long CharacterID { get; set; }

        public string Name { get; set; }

        public string CorporationName { get; set; }
    }
}
