using O2O.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace O2O.Admin.Web.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index(UserDTO dto)
        {
            dto.UserNo = "abcv";
            dto.UserName = dto.UserNo + "123";
            return View(dto);
        }
    }
}