using System.Web.Mvc;
using O2O.Service.Eleme;

namespace O2O.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //EleOrderApiService dd = new EleOrderApiService();

            //var dfd = dd.GetAllOrders("3c9f84d064e8fc22f95b61b9acace5b0", 305172254,1,10,"2021-01-10");

            return View();
        }
    }
}