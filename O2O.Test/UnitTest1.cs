using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using O2O.Service;

namespace O2O.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //程序启动然后启动Log4NET日志框架
            log4net.Config.XmlConfigurator.Configure();

            using (O2OContext c = new O2OContext())
            {
                var user = new UserEntity() {
                UserNo = "01",
                UserName = "爱达乐",
                ConnString = "123123abc",
                Ket = "123456"
                };

                user.MtConfig = new MtConfigEntity();

                c.User.Add(user);
                c.SaveChanges();
            }
        }
    }
}
