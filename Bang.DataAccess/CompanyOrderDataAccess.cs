﻿using Bang.Models;
using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bang.DataAccess
{
    public class CompanyOrderDataAccess
    {
        public static List<OrderModel> Query(string companyCode, string startDate, string endDate, string empAccount, string serviceType, string status, int pageIndex, int pageSize, out int recordCount)
        {
            #region
            var result = new List<OrderModel>();

            pageIndex = pageIndex - 1;
            recordCount = 0;
            var startIndex = pageIndex * pageSize + 1;
            var endIndex = startIndex + pageSize - 1;

            OracleParameterCollection paramList = new OracleParameterCollection();
            OracleParameterCollection countParamList = new OracleParameterCollection();
            var paramArray = new OracleParameter[] { };
            var countParamArray = new OracleParameter[] { };

            var countSqlString = "  select count(*) as rowCount from v_company_order v where 1=1 ";
            var sqlString = "select * from ( select rownum as rn,v.order_number,v.order_time,v.pay_total,v.c_phone,v.shifu_phone,v.category_value,v.order_status,v.release_status,v.money_status,v.category_code from v_company_order v ) m where rn >=" + startIndex + " and rn <=" + endIndex;//

            sqlString += " and shifu_phone in (select shifu_phone from shifu_reg where company_code=':companyCode'" + (string.IsNullOrEmpty(empAccount) ? "" : " and shifu_phone =':empAccount')") + ")";
            countSqlString += " and shifu_phone in (select shifu_phone from shifu_reg where company_code=':companyCode'" + (string.IsNullOrEmpty(empAccount) ? "" : " and shifu_phone =':empAccount')") + ")";
            paramList.Add(new OracleParameter { ParameterName = "companyCode", Value = companyCode });
            countParamList.Add(new OracleParameter { ParameterName = "companyCode", Value = companyCode });

            if (!string.IsNullOrEmpty(empAccount))
            {
                //paramList.Add(new OracleParameter { ParameterName = "", Value =  });
                paramList.Add(new OracleParameter { ParameterName = "empAccount", Value = empAccount });
                countParamList.Add(new OracleParameter { ParameterName = "empAccount", Value = empAccount });
            }

            if (serviceType != "-1")
            {
                #region
                sqlString += " and v.category_code=':serviceType'";
                countSqlString += " and v.category_code=':serviceType'";
                countParamList.Add(new OracleParameter { ParameterName = "serviceType", Value = serviceType });
                paramList.Add(new OracleParameter { ParameterName = "serviceType", Value = serviceType });
                #endregion
            }

            if (status != "-1")
            {
                #region
                /*
                    order_status
                        15：等待支付
                        20，25：服务中
                        90:服务已完成
                    release_status
                        5:已取消
                    money_status
                        -10,-15:待退款
                 */
                if (status == "1" || status == "2" || status == "3")
                {
                    sqlString += " and v.order_status=':status'";
                    countSqlString += " and v.order_status=':status'";
                }
                else if (status == "4")
                {
                    sqlString += " and v.release_status=':status'";
                    countSqlString += " and v.release_status=':status'";
                }
                else
                {//5
                    sqlString += " and v.money_status=':status'";
                    countSqlString += " and v.money_status=':status'";
                }
                countParamList.Add(new OracleParameter { ParameterName = "status", Value = status });
                paramList.Add(new OracleParameter { ParameterName = "status", Value = status });
                #endregion
            }

            if (string.IsNullOrEmpty(startDate) == false)
            {
                sqlString += " and v.order_time>=':startDate'";
                countSqlString += " and v.order_time>=':startDate'";
                paramList.Add(new OracleParameter { ParameterName = "startDate", Value = startDate });
                countParamList.Add(new OracleParameter { ParameterName = "startDate", Value = startDate });
            }

            if (string.IsNullOrEmpty(endDate) == false)
            {
                sqlString += " and v.order_time<=':endDate'";
                countSqlString += " and v.order_time<=':endDate'";
                paramList.Add(new OracleParameter { ParameterName = "endDate", Value = endDate });
                countParamList.Add(new OracleParameter { ParameterName = "endDate", Value = endDate });
            }

            return result;

            paramList.CopyTo(paramArray, 0);
            countParamList.CopyTo(countParamArray, 0);
            recordCount = Convert.ToInt32(OracleHelper.ExecuteScalar(OracleHelper.OracleConnString, System.Data.CommandType.Text, countSqlString, countParamArray));
            var reader = OracleHelper.ExecuteReader(OracleHelper.OracleConnString, System.Data.CommandType.Text, sqlString, paramArray);
            while (reader.Read())
            {
                var emp = new OrderModel
                {
                    OrderId = reader["order_number"].ToString(),
                    TradeDate = reader["order_time"].ToString(),
                    Status = reader["order_status"].ToString(),
                    ReleaseStatus = reader["release_status"].ToString(),
                    MoneyStatus = reader["money_status"].ToString(),
                    ServiceType = reader["category_value"].ToString(),
                    Total = decimal.Parse(reader["pay_total"].ToString()),
                    CompanyEmpAccount = reader["shifu_phone"].ToString(),
                    Customer = reader["c_phone"].ToString()
                };
                result.Add(emp);
            }
            #endregion
            return result;
        }

        public static List<CompanySettlementModel> SettlementQuery(string companyCode, string year, string month)
        {
            var result = new List<CompanySettlementModel>();
            var sqlString = "select other_number,shifu_money,balance_source,b.shifu_code,create_date from shifu_balance_log b left join shifu_reg s on s.shifu_code=b.shifu_code where 1=1 ";

            sqlString += "create_date between and";
            sqlString += "create_date between and";

            return result;

            var reader = OracleHelper.ExecuteReader(OracleHelper.OracleConnString, System.Data.CommandType.Text, sqlString);
            while (reader.Read())
            {
                var emp = new CompanySettlementModel
                {
                    OrderId = reader["other_number"].ToString(),
                    EmpAccount = reader["shifu_money"].ToString(),
                    OrderDate = DateTime.Parse(reader["create_date"].ToString()),

                    ServiceType = reader["shifu_code"].ToString(),
                    TradeTotal = decimal.Parse(reader["shifu_money"].ToString())
                };
                result.Add(emp);
            }
            return result;
        }

        /// <summary>
        /// 师傅交易统计列表
        /// </summary>
        /// <param name="companyCode"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static List<CompanyEmpOrderStatModel> EmpOrderStatQuery(string companyCode, string year, string month, string empAccount)
        {
            var result = new List<CompanyEmpOrderStatModel>();
            var sqlString = "select other_number,shifu_money,balance_source,b.shifu_code,create_date from shifu_balance_log b left join shifu_reg s on s.shifu_code=b.shifu_code where 1=1 ";

            sqlString += "create_date between and";
            sqlString += "create_date between and";

            return result;

            var reader = OracleHelper.ExecuteReader(OracleHelper.OracleConnString, System.Data.CommandType.Text, sqlString);
            while (reader.Read())
            {
                var emp = new CompanyEmpOrderStatModel
                {
                    EmpName = reader["other_number"].ToString(),
                    EmpAccount = reader["shifu_money"].ToString(),
                    OrderCount = int.Parse(reader["create_date"].ToString()),
                    OrderTotal = decimal.Parse(reader["shifu_money"].ToString())
                };
                result.Add(emp);
            }
            return result;
        }
    }
}