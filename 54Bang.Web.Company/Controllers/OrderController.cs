﻿using _54Bang.Web.Company.Authentication;
using Bang.Business;
using Bang.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _54Bang.Web.Company.Controllers
{
    [CompanyAuthorize]
    /// <summary>
    /// 订单管理
    /// </summary>
    public class OrderController : Controller
    {
        //
        // GET: /Order/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Query(string startDate, string endDate, string empAccount, string serviceType, string status, int pageIndex)
        {
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            var pageSize = 20;
            var recordCount = 0;
            //todo 
            var list = CompanyOrderManager.Query(UserContext.Current.CompanyId, startDate, endDate, empAccount, serviceType, status, pageIndex, pageSize, out recordCount);
            ViewBag.RecordCount = recordCount;
            ViewBag.PageSize = 20;
            ViewBag.CurrentIndex = pageIndex;
            return View(list);
        }

        /// <summary>
        /// 结算管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Settlement()
        {

            return View();
        }
        [HttpPost]
        public ActionResult SettlementQuery(string year, string month)
        {
            var list = CompanyOrderManager.SettlementQuery(UserContext.Current.CompanyId, year, month);

            return View(list);
        }

        /// <summary>
        /// 师傅交易统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EmpOrderStat()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EmpOrderStatQuery(string year, string month, string empAccount)
        {
            var list = CompanyOrderManager.EmpOrderStatQuery(UserContext.Current.CompanyId, year, month, empAccount);
            return View(list);
        }

        [HttpPost]
        public ActionResult ExportExcel(string jsonData)//id is candidateid
        {
            #region
            const string title = "Rank$序号,JobTitle$推荐职位,PhaseText$招聘阶段,Recommender$关联顾问,RecommendDateText$关联时间," +
                "Name$姓名,Mobile$个人电话,Email$邮箱,GenderText$性别,BirthdayText$出生日期,Age$年龄,SchoolName$学校名称,MajorName$专业名称,DegreeText$学历," +
                  "Expirence$工作年限,LiveCity$目前居住地,Hukou$户籍,DesiredCity$期望工作地,CurrentCompanyName$最近工作单位,CurrentJobTitle$最近工作职位," +
                  "IndustryText$所属行业,InterviewComment$评语,Commentor$评语沟通人";
            var user = UserContext.Current;
            //var resumeService = new ResumesService();
            //var dataList = resumeService.Export(user.TenantId, jobId, jobTitle, id);
            //if (dataList != null && dataList.Count > 0)
            //{
            //    var buffer = ExcelHelper.DataToExcel(dataList, title);
            //    return File(buffer, "application/octet-stream", string.Format("{0:yyyyMMddHHmmssfff}.xls", DateTime.Now));
            //}
            ViewBag.MsgCode = 20004;
            return View("~/Views/Error/Index.cshtml");
            #endregion
        }

    }
}
