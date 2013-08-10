using EveLucrum.Domain;

namespace EveLucrum.ApplicationServices
{
    public interface IAPIService
    {
        void UpdateCharacterList(int accountID);

        IRepository Repository { get; }
    }
}