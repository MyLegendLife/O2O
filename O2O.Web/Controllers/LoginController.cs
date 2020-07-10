using CaptchaGen;
using O2O.Common;
using O2O.IService;
using O2O.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace O2O.Web.Controllers
{
    public class LoginController : Controller
    {
        public IUserService _service { get; set; }

        // GET: Login
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                Session["UserId"] = null;
                Session.Abandon();
            }

            return View();
        }

        public ActionResult GetVerCode()
        {
            //利用CaptchaGen验证码组件，用法参照文档，先通过Nuget安装组件 Install-PackAge CaptchaGen
            //1:获取验证码文字
            var VerNum = ToolsCommon.CreateVerifyCode(4);
            TempData["code"] = VerNum;//保存到Sesion里面，使用过一次后就自动清除，这就是TempData的用法
            MemoryStream ms = ImageFactory.GenerateImage(VerNum, 60, 100, 20, 6);  //此处不能用using语法
            return File(ms, "image/jpeg");
        }

        [HttpPost]
        public ActionResult LoginIndex(LoginPost model)
        {
            //服务端的校验不通过的话
            //Statues返回状态码，ErrorMsg返回错误信息，用在前端提示，代码不用加try catch，都是AJAX请求，如果与服务端报错，那么程序有错误日志的处理会记录，并且前端的error事件会弹出相应的提示信息
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult { state = "error", msg = MVCHelper.GetValidMsg(ModelState) });
            }
            //看验证码否一致
            if (model.VarCode != TempData["code"].ToString())
            {
                return Json(new AjaxResult { state = "error", msg = "验证码不一致" });
            }
            bool b = _service.CheckLogin(model.LoginName, model.Pwd);
            if (b)
            {
                //保存用户
                var user = _service.GetByLoginName(model.LoginName);
                Session["UserId"] = user.Id;

                return Json(new AjaxResult { state = "ok" });
            }
            else
            {
                return Json(new AjaxResult { state = "error", msg = "用户名或者密码不正确" });
            }
        }
    }
}