using EveLucrum.Domain;
using EveLucrum.Domain.Entities;

namespace EveLucrum.ApplicationServices
{
    public interface IAPIService
    {
        void UpdateCharacterList(int accountID);

        IRepository Repository { get; }
        void DeleteCharacter(int characterID);
        Account AddOrUpdateAccount(string keyID, string vCode);
    }
}