using Newtonsoft.Json;
using O2O.Common;
using O2O.IService;
using O2O.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace O2O.Web.Controllers
{
    public class MainController : Controller
    {
        public IUserService _service { get; set; }

        // GET: Main
        public ActionResult Index()
        {
            string id = Session["UserId"].ToString();
            var user = _service.Get(id);

            Global.USER_ID = user.Id;
            Global.USER_NO = user.UserNo;
            Global.CON_STR = ToolsCommon.FromBase64(user.ConnString);
            
            return View(user);
        }
    }
}