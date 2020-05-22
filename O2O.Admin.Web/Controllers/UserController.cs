using O2O.IService;
using O2O.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace O2O.Admin.Web.Controllers
{
    public class UserController : Controller
    {
        private IUserService _service { get; set; }

        // GET: User
        public ActionResult Index()
        {
            //_service = new UserService();

            UserEntity e = new UserEntity() { 
                UserName = "a",
                UserNo="b",
                ConnString="c",
                Ket="e"
            };
            
            
            var list = _service.GetAll().ToList();

            return View();
        }
    }
}