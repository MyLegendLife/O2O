using O2O.Common;
using O2O.DTO;
using O2O.DTO.Meituan;
using O2O.IService;
using O2O.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace O2O.Web.Controllers
{
    public class MtAccountController : Controller
    {
        public IUserService _userService { get; set; }

        public IMtAccountService _accountService { get; set; }

        public IOrderService _orderService { get; set; }

        public MtAccountController()
        {
            int num = Global.USER_ID != "admin" ? 1 : 0;
        }

        public ActionResult Index() => View();

        [HttpGet]
        public ActionResult List() => View(_accountService.GetAccountAll());

        [HttpGet]
        public ActionResult Add() => View(_userService.GetAll().Where(a => a.Id != "admin").ToList());

        [HttpPost]
        public ActionResult Add(MtAccountAdd mtAccountAdd)
        {
            if (mtAccountAdd == null)
                throw new ArgumentNullException(nameof(mtAccountAdd));
            _accountService.Add(mtAccountAdd.UserId, mtAccountAdd.AccountNo, mtAccountAdd.AccountName, mtAccountAdd.WaimaiAppId, mtAccountAdd.WaimaiAppSecret, mtAccountAdd.TuangouAppKey, mtAccountAdd.TuangouAppSecret, mtAccountAdd.Description);
            return Json(new AjaxResult()
            {
                state = "ok",
                msg = ""
            });
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            Mt_AccountDTO account = _accountService.GetAccount(id);
            List<UserDTO> list = _userService.GetAll().Where(a => a.Id != "admin").ToList();
            return View(new MtAccountEditGet()
            {
                Id = account.Id,
                UserId = account.UserId,
                AccountNo = account.AccountNo,
                AccountName = account.AccountName,
                WaimaiAppId = account.WaimaiAppId,
                WaimaiAppSecret = account.WaimaiAppSecret,
                TuangouAppKey = account.TuangouAppKey,
                TuangouAppSecret = account.TuangouAppSecret,
                Description = account.Description,
                Users = list
            });
        }

        [HttpPost]
        public ActionResult Edit(MtAccountEditPost accountEditPost)
        {
            _accountService.Update(accountEditPost.Id, accountEditPost.UserId, accountEditPost.AccountNo, accountEditPost.AccountName, accountEditPost.WaimaiAppId, accountEditPost.WaimaiAppSecret, accountEditPost.TuangouAppKey, accountEditPost.TuangouAppSecret, accountEditPost.Description);
            return Json(new AjaxResult()
            {
                state = "ok",
                msg = ""
            });
        }

        [HttpPost]
        public ActionResult Delete(Guid id, string userId)
        {
            if (_orderService.GetCount(userId) > 0)
                return Json(new AjaxResult()
                {
                    state = "err",
                    msg = "该商户有业务数据无法删除"
                });
            _accountService.Delete(id);
            return Json(new AjaxResult()
            {
                state = "ok",
                msg = ""
            });
        }
    }
}