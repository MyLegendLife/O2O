using Newtonsoft.Json.Linq;
using O2O.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.Service
{
    public class Bak365Service
    {
        public static Dictionary<string, string> GetShopList()
        {
            var table = SqlHelper.ExecuteDataTable("SELECT ShopNo,ShopName FROM Dict_ShopInfo");

            var dic = new Dictionary<string, string>();
            foreach (DataRow row in table.Rows)
            {
                dic.Add(row["ShopNo"].ToString(), row["ShopName"].ToString());
            }

            return dic;
        }

        public static List<ProdInfo> GetProdList()
        {
            var table = SqlHelper.ExecuteDataTable("SELECT ProdNo,ProdName,ProdUnit,CONVERT(FLOAT,Price) AS Price FROM Dict_ProdInfo WHERE State != 1");

            List<ProdInfo> list = new List<ProdInfo>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(new ProdInfo()
                {
                    ProdNo = row["ProdNo"].ToString(),
                    ProdName = row["ProdName"].ToString(),
                    ProdUnit = row["ProdUnit"].ToString(),
                    Price = row["Price"].ToString()
                });
            }

            return list;
        }

        public static object GetShopInfo(string shopNo)
        {
            var table = SqlHelper.ExecuteDataTable("SELECT ShopNo,ShopName FROM Dict_ShopInfo WHERE ShopNo = '" + shopNo + "'");

            if (table.Rows.Count < 1)
                return null;

            var res = new
            {
                ShopNo = table.Rows[0]["ShopNo"].ToString(),
                ShopName = table.Rows[0]["ShopName"].ToString(),
            };

            return res;
        }
    }

    public class ProdInfo
    {
        public string ProdNo { get; set; }
        public string ProdName { get; set; }
        public string ProdUnit { get; set; }
        public string Price { get; set; }
    }
}
