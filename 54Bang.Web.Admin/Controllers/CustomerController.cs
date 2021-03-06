﻿using Bang.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _54Bang.Web.Admin.Controllers
{
    public class CustomerController : Controller
    {
        //
        // GET: /Customer/

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Query(string city, string startDate, string endDate, string customerAccount, int pageIndex)
        {
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            var pageSize = 20;
            var recordCount = 0;
            //todo 
            var list = AdminSysManager.GetCustomerList(city, startDate, endDate, customerAccount, pageIndex, pageSize, out recordCount);

            ViewBag.RecordCount = recordCount;
            ViewBag.PageSize = 20;
            ViewBag.CurrentIndex = pageIndex;

            return View(list);
        }

    }
}
