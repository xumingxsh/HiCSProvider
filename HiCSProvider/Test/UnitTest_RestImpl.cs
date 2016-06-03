using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using HiCSProvider.DB.Impl;

using HiCSUtil;

namespace HiCSProvider.Test
{
    [TestClass]
    public class UnitTest_RestImpl
    {
        public UnitTest_RestImpl()
        {
            RestHepler.RemoteURI = "http://localhost:49653";
        }
        
        [TestMethod]
        public void RestImpl_LoginP2()
        {
            string ret = RestHepler.RequestRest8GetOnID("User.Login_param2", "zhs", "zhs");
            Assert.IsTrue(ret.Length > 0);
            ret = ret.Replace("\\", "");
            ret = ret.Substring(1, ret.Length - 2);
            DataTable dt = Json.Json2DataTable(ret);
            Assert.IsTrue(dt.Rows.Count > 0);
        }
        [TestMethod]
        public void RestImpl_sql()
        {
            string ret = RestHepler.RequestRest8GetOnID("Sql.ExecuteScalarInt",
                "Select count(1) from [InputValue$] where ProductID='P1' and Num='1' and InputID='1'");

            Assert.IsTrue(ret != "-1");
        }
    }
}
