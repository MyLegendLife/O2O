using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace O2O.Common
{
    public class ToolsCommon
    {

        //获取盐（用户登录密码）+生成验证码文字
        public static string CreateVerifyCode(int len)
        {
            char[] data = { 'a','c','d','e','f','h','k','m',
                'n','r','s','t','w','x','y'};
            StringBuilder sb = new StringBuilder();
            Random rand = new Random();
            for (int i = 0; i < len; i++)
            {
                int index = rand.Next(data.Length);//[0,data.length)
                char ch = data[index];
                sb.Append(ch);
            }
            //勤测！
            return sb.ToString();
        }

        /// <summary>
        /// 实体赋值到另外一个实体
        /// </summary>
        /// <param name="sourceEntity"></param>
        /// <param name="targetEntity"></param>
        /// <returns></returns>
        public static object EntityToEntity(object sourceEntity, object targetEntity, params string[] excudeFields)
        {
            if (sourceEntity == null || targetEntity == null) return null;
            var sourceType = sourceEntity.GetType();
            var targetType = targetEntity.GetType();

            foreach (var item in targetType.GetProperties())
            {
                if (excudeFields.Any(a => a == item.Name))
                    continue;
                if (sourceType.GetProperty(item.Name) == null) 
                    continue;
                item.SetValue(targetEntity, sourceType.GetProperty(item.Name).GetValue(sourceEntity, null), null);
            }

            return targetEntity;
        }

        /// <summary>
        /// 动态赋值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dic"></param>
        public static void SetValue(object entity, Dictionary<string, object> dic)
        {
            if (entity is null) return;

            PropertyInfo[] properties = entity.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var item in dic)
            {
                //if (item.Value is null) continue;

                var property = properties.Where(a => a.Name == item.Key).FirstOrDefault();

                if (property is null) continue;

                property.SetValue(entity,item.Value);
            }
        }

        /// <summary>
        /// 将DataTable转为实体对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="sourceDT">原DT</param>
        /// <returns>转换后的实体列表</returns>
        public static List<T> DataTableToEntity<T>(DataTable table) where T : class
        {
            List<T> list = new List<T>();
            // 获取需要转换的目标类型
            Type type = typeof(T);
            foreach (DataRow dRow in table.Rows)
            {
                // 实体化目标类型对象
                object obj = Activator.CreateInstance(type);
                foreach (var prop in type.GetProperties())
                {
                    // 给目标类型对象的各个属性值赋值
                    prop.SetValue(obj, dRow[prop.Name], null);
                }
                list.Add(obj as T);
            }
            return list;
        }

        public static long GetTimestamp()
        {
            return (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static string GetTimestampInt32()
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return unixTimestamp.ToString();
        }

        /// <summary>
        /// 判断一个字符串是否是正数型的字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>是则返回true，否则返回false</returns>
        public static bool IsPositiveNumber(string str)
        {
            return Regex.IsMatch(str, @"^[+]{0,1}(\d+)$|^[+]{0,1}(\d+\.\d+)$");
        }

        /// <summary>
        /// 分解字符串   0001,2;0002,1;
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Dictionary<string, double> CustomFormat(string str)
        {
            Dictionary<string, double> dic = new Dictionary<string, double>();

            string[] strArr = str.Split(new char[] {';' },StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in strArr)
            {
                string[] itemArr = item.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                double result = 0;
                double.TryParse(itemArr[1], out result);
                dic.Add(itemArr[0], result);
            }

            return dic;
        }

        /// <summary>
        /// 用MD5加密字符串
        /// </summary>
        /// <param name="assemble">待加密的字符串</param>
        /// <returns></returns>
        public static string MD5Encrypt(string str)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.UTF8.GetBytes(str);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        ///// <summary>
        ///// 用MD5加密字符串
        ///// </summary>
        ///// <param name="assemble">待加密的字符串</param>
        ///// <returns></returns>
        //public static string MD5Encrypt(string assemble)
        //{
        //    string cl = assemble;
        //    string pwd = "";
        //    MD5 md5 = MD5.Create();//实例化一个md5对像
        //                           // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
        //    byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
        //    // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
        //    for (int i = 0; i < s.Length; i++)
        //    {
        //        // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
        //        pwd = pwd + s[i].ToString("x");
        //    }
        //    return pwd;
        //}

        /// <summary>
        /// 取MD5
        /// </summary>
        /// <param name="sDataIn"></param>
        /// <returns></returns>
        public static string GetMD5(string sDataIn)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytValue, bytHash;
            bytValue = System.Text.Encoding.UTF8.GetBytes(sDataIn);
            bytHash = md5.ComputeHash(bytValue);
            md5.Clear();
            string sTemp = "";
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
            }
            return sTemp.ToLower();
        }

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="assemble"></param>
        /// <returns></returns>
        public static string SHA1Encrypt(string assemble)
        {
            SHA1 sha;
            //ASCIIEncoding enc;
            string signature = "";
            try
            {
                sha = new SHA1CryptoServiceProvider();
                //enc = new ASCIIEncoding();
                //byte[] dataToHash = enc.GetBytes(assemble);
                byte[] dataToHash = System.Text.Encoding.UTF8.GetBytes(assemble);
                byte[] dataHashed = sha.ComputeHash(dataToHash);
                signature = BitConverter.ToString(dataHashed).Replace("-", "");
                signature = signature.ToLower();
            }
            catch (Exception)
            {
            }

            return signature;
        }

        /// <summary>
        /// Base64编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToBase64(string str)
        {
            Encoding encode = System.Text.Encoding.ASCII;
            byte[] bytedata = encode.GetBytes(str);
            string strPath = Convert.ToBase64String(bytedata, 0, bytedata.Length);
            return strPath;
        }

        /// <summary>
        /// Base64解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FromBase64(string str)
        {
            byte[] c = Convert.FromBase64String(str);
            return Encoding.Default.GetString(c);
        }

        /// <summary>
        /// UTF8转换成GB2312
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string UTF8ToGB2312(string text)
        {
            //声明字符集   
            Encoding utf8, gb2312;
            //utf8   
            utf8 = Encoding.GetEncoding("utf-8");
            //gb2312   
            gb2312 = Encoding.GetEncoding("gb2312");
            byte[] utf;
            utf = utf8.GetBytes(text);
            utf = Encoding.Convert(utf8, gb2312, utf);
            //返回转换后的字符   
            return gb2312.GetString(utf);
        }

        /// <summary>
        /// 获取枚举描述集合
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<object> GetEnumListKey(Type type)
        {
            return Enum.GetValues(type).OfType<Enum>()
                .Select(a => new
                {
                    Key = a.ToString(),
                    Value = a.GetDescription()
                });
        }

        /// <summary>
        /// 获取枚举描述集合
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<object> GetEnumListValue(Type type)
        {
            return Enum.GetValues(type).OfType<Enum>()
                .Select(a => new
                {
                    Key = Convert.ToInt32(a),
                    Value = a.GetDescription()
                });
        }
    }
}
