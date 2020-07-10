using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.DTO.Eleme
{
    public class Ele_ShopDTO : BaseDTO
    {
        public long ShopId { get; set; }

        public string ShopNo { get; set; }

        public Guid AccountId { get; set; }

        public string AccessToken { get; set; }

        public string UserId { get; set; }
    }
}
