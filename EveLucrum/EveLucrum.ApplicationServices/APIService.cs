using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EveLucrum.Domain;
using EveLucrum.Domain.Entities;
using EveLucrum.Infrastructure.API;

namespace EveLucrum.ApplicationServices
{
    public class APIService : IAPIService
    {
        private readonly ILucrumContext context;

        public IRepository Repository { get { return context; } }



        public APIService(ILucrumContext context)
        {
            this.context = context;
        }

        public void DeleteCharacter(int characterID)
        {
            var character = context.Characters.FirstOrDefault(c => c.CharacterID == characterID);

            if (character == null)
                throw new ArgumentException("Could not locate character.");

            context.Delete(character);

            context.SaveChanges();
        }

        public Account AddOrUpdateAccount(string keyID, string vCode)
        {
            var account = context.Accounts.FirstOrDefault(a => a.KeyID == keyID && a.VerificationCode == vCode);

            if (account == null)
            {
                try
                {
                    new AccountReader(keyID, vCode).GetCharacters();
                }
                catch (Exception)
                {
                    throw new ArgumentException("Invalid API key");
                }

                account = new Account() { KeyID = keyID, VerificationCode = vCode };
                context.Add(account);
                context.SaveChanges();
            }

            return account;
        }

        public void UpdateCharacterList(int accountID)
        {
            var account = context.Accounts.FirstOrDefault(a => a.AccountID == accountID);

            if (account == null)
                throw new ArgumentException("Could not locate account in database.");

            var existingCharacters = account.Characters.ToList();

            var apiAccountReader = new AccountReader(account.KeyID, account.VerificationCode);
            var apiCharacterReader = new SkillReader(account.KeyID, account.VerificationCode);

            var characterDTOs = apiAccountReader.GetCharacters().ToList();

            foreach (var existingCharacter in existingCharacters)
            {
                var dto = characterDTOs.FirstOrDefault(c => c.CharacterID == existingCharacter.ActorID);

                if (dto != null)
                {
                    existingCharacter.Name = dto.Name;
                    existingCharacter.CorporationName = dto.CorporationName;
                }
                else
                {
                    //no longer exists, delete character?
                }
            }

            foreach (var characterDTO in characterDTOs)
            {
                var character = existingCharacters.FirstOrDefault(c => c.ActorID == characterDTO.CharacterID);
                if (character == null)
                {
                    //new character
                    var tradingSkills = apiCharacterReader.GetTradingSkillLevels(characterDTO.CharacterID);

                    character = new Character()
                        {
                            Account = account,
                            ActorID = characterDTO.CharacterID,
                            CorporationName = characterDTO.CorporationName,
                            Name = characterDTO.Name,
                            AccountingSkill = tradingSkills.Item1,
                            BrokerRelationsSkill = tradingSkills.Item2
                        };
                    context.Add(character);
                }
            }

            context.SaveChanges();
        }
    }
}
