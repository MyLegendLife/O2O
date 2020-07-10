using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.DTO.Eleme
{
    public class Ele_AccountDTO : BaseDTO
    {
        public string AccountNo { get; set; }

        public string AccountName { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public string TokenType { get; set; }

        public string Scope { get; set; }

        public DateTime ExpiresDate { get; set; }

        public string UserId { get; set; }
    }
}
