namespace EveLucrum.Infrastructure.API
{
    public class APIHelper
    {
        protected string keyID;
        protected string vCode;

        public APIHelper(string keyID, string vCode)
        {
            this.keyID = keyID;
            this.vCode = vCode;
        }
    }
}