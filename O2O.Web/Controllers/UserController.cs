using O2O.Common;
using O2O.DTO;
using O2O.IService;
using O2O.Web.Models;
using System;
using System.Web.Mvc;

namespace O2O.Web.Controllers
{
    public class UserController : Controller
    {
        public IUserService _userService { get; set; }

        public IOrderService _orderService { get; set; }

        public UserController()
        {
            int num = Global.USER_ID != "admin" ? 1 : 0;
        }

        public ActionResult Index() => View();

        [HttpGet]
        public ActionResult List(string param) => View(_userService.GetAll(param));

        [HttpGet]
        public ActionResult Add() => View();

        [HttpPost]
        public ActionResult Add(UserAdd userAdd)
        {
            if (userAdd == null)
                throw new ArgumentNullException(nameof(userAdd));
            if (_userService.GetByLoginName(userAdd.LoginName) != null)
                return Json(new AjaxResult()
                {
                    state = "err",
                    msg = "登录账户已存在"
                });
            _userService.Add(userAdd.Id, userAdd.UserName, userAdd.LoginName, userAdd.Password, userAdd.ConnString, userAdd.ket, userAdd.Description);
            return Json(new AjaxResult()
            {
                state = "ok",
                msg = ""
            });
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            UserDTO userDto = _userService.Get(id);
            return View(new UserEditGet()
            {
                Id = userDto.Id,
                UserName = userDto.UserName,
                LoginName = userDto.LoginName,
                Ket = userDto.Ket,
                Description = userDto.Description
            });
        }

        [HttpPost]
        public ActionResult Edit(UserEditPost userEditPost)
        {
            if (_userService.GetByLoginName(userEditPost.LoginName, userEditPost.Id) != null)
            {
                return Json(new AjaxResult() { state = "err", msg = "登录账户已存在" });
            }

            _userService.Update(userEditPost.Id, userEditPost.UserName, userEditPost.LoginName, userEditPost.Password, userEditPost.ConnString, userEditPost.ket, userEditPost.Description);

            return Json(new AjaxResult() { state = "ok", msg = "" });
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            int count = _orderService.GetCount(id);
            if (id == "admin")
            {
                return Json(new AjaxResult() { state = "err", msg = "管理员无法删除" });
            }
            if (count > 0)
            {
                return Json(new AjaxResult() { state = "err", msg = "该商户有业务数据无法删除" });
            }

            _userService.Delete(id);

            return Json(new AjaxResult() { state = "ok", msg = "" });
        }
    }
}
