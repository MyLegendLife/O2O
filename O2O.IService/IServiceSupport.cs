using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.IService
{
    /// <summary>
    /// 继承IServiceSupport的接口才会写入容器(Autofac)
    /// </summary>
    public interface IServiceSupport
    {
    }
}
