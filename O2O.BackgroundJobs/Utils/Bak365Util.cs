using System.Collections.Generic;
using System.Data;
using O2O.BackgroundJobs.Tools;
using O2O.Common;

namespace O2O.BackgroundJobs.Utils
{
    public class Bak365Util
    {
        public SqlUtil _sqlUtil;

        public Bak365Util(string connString)
        {
            _sqlUtil = new SqlUtil(ToolsCommon.FromBase64(connString));
        }

        public List<BakStock> GetCurrentStock(string shopNo, IEnumerable<string> prodNos)
        {
            var strProdNos = "'" + string.Join("','", prodNos) + "'";

            var table = _sqlUtil.ExecuteDataTable($"SELECT ShopNo,ProdNo,SUM(StoreQty) AS Stock FROM MD_StoreInfo " +
                                                  $"WHERE ShopNo IN ('{shopNo}') AND ProdNo IN ({strProdNos}) " +
                                                  $"GROUP BY ShopNo,ProdNo");

            var list = new List<BakStock>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(new BakStock()
                {
                    ShopNo = row["ShopNo"].ToString(),
                    ProdNo = row["ProdNo"].ToString(),
                    Stock = double.Parse(row["Stock"].ToString())
                });
            }

            return list;
        }
    }

    public class BakStock
    {
        public string ShopNo { get; set; }

        public string ProdNo { get; set; }

        public double Stock { get; set; }
    }
}
