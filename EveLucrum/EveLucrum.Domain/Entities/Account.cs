using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveLucrum.Domain.Entities
{
    public class Account
    {
        public int AccountID { get; set; }

        public string KeyID { get; set; }

        public string VerificationCode { get; set; }

        public virtual ICollection<Character> Characters { get; set; }

        public void RefreshCharacters()
        {
            
        }
    }
}
