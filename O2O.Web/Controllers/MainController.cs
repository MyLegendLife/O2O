using O2O.Common;
using O2O.IService;
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

            Session["CON_STR"] = ToolsCommon.FromBase64(user.ConnString);
            
            return View(user);
        }
    }
}