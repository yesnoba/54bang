﻿using _54Bang.Web.Company.Authentication;
using Bang.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _54Bang.Web.Company.Controllers
{
    [CompanyAuthorize]
    /// <summary>
    /// 员工管理:师傅等等
    /// </summary>
    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 增加师傅
        /// </summary>
        /// <param name="mobileList"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(string mobileList)
        {
            return Json(new { success = true, msg = mobileList });
        }

        /// <summary>
        /// 解除师傅 login_status 0：正常，1：冻结
        /// </summary>
        /// <param name="empIds"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Layoff(string empIds)
        {
            return Json(new { success = true, msg = empIds });
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public ActionResult Query(string empAccount, string status, string serviceType, int pageIndex)
        {
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            var pageSize = 20;
            var recordCount = 0;
            //todo 
            var list = CompanyManager.GetEmpListBy(UserContext.Current.CompanyId, empAccount, status, serviceType, pageIndex, pageSize, out recordCount);

            //ViewBag.List = list;
            ViewBag.RecordCount = recordCount;
            ViewBag.PageSize = 20;
            ViewBag.CurrentIndex = pageIndex;

            return View(list);
        }
    }
}
