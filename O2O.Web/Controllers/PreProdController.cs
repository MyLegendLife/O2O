using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O2O.Common;
using O2O.DTO;
using O2O.IService;
using O2O.Service;
using O2O.Web.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace O2O.Web.Controllers
{
    public class PreProdController : Controller
    {
        public IPreProdService _preProdService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 给页面提供json格式的节点数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Tree()
        {
            var cateProdList = Bak365Service.GetCateProdList();

            var cateList = cateProdList.GroupBy(a => new { a.PareNo, a.CateNo, a.CateName }, (a, b) => a);

            var nodes = new ConcurrentBag<ZTreeNode>();
            foreach (var cate in cateList)
            {
                nodes.Add(new ZTreeNode()
                {
                    id = cate.CateNo,
                    name = cate.CateName,
                    pId = cate.PareNo,
                    isParent = true
                });
            }

            var preProdList = _preProdService.Get(Global.USER_ID);

            Parallel.ForEach(cateList, cate =>
            {
                var prodList = cateProdList.Where(a => a.CateNo == cate.CateNo && !string.IsNullOrWhiteSpace(a.ProdNo));

                foreach (var prod in prodList)
                {
                    var node = new ZTreeNode();

                    node.id = prod.ProdNo;
                    node.name = prod.ProdName;
                    node.pId = prod.CateNo;
                    if (preProdList.Exists(a => a.ProdNo == prod.ProdNo))
                        node.@checked = true;

                    nodes.Add(node);
                }
            });

            var sortNodes = nodes.OrderBy(a => a.id);

            //将获取的节点集合转换为json格式字符串，并返回
            string json = JsonConvert.SerializeObject(sortNodes);
            return json;
        }

        [HttpPost]
        public ActionResult Save(List<ZTreeNode> checkedNodes)
        {
            //父节点为商品类别，不做保存
            var list = checkedNodes.Where(a => a.isParent == false).ToList().Select(a => new PreProdDTO { UserId = Global.USER_ID, ProdNo=a.id }).ToList();

            try
            {
                _preProdService.Delete(Global.USER_ID);

                _preProdService.Add(list);

                return Json(new AjaxResult() { state = "ok" });
            }
            catch (Exception e)
            {
                return Json(new AjaxResult() { state = "no",msg = e.Message });
            }
        }
    }
}