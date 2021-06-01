using NUnit.Framework;
using System.Net.Security;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace SqlTests
{
    public class Tests
    {
        private SqlHelper _sqlHelper;

        [SetUp]
        public void Setup()
        {
            _sqlHelper = new SqlHelper("Shop");
            _sqlHelper.OpenConnection();
        }

        [TearDown]
        public void TearDown()
        {
        //    _sqlHelper.ExecuteNonQuery("delete from [Shop].[dbo].[Products] where id = 23");
            _sqlHelper.CloseConnection();
        }

        [Test]
        public void Test1()
        {
            _sqlHelper.Insert("Products",
                new Dictionary<string, string> { { "Id", "23" }, { "Name", "'Test23'" }, { "Count", "234" } });
            var res = _sqlHelper.IsRowExistedInTable("Products",
                new Dictionary<string, string> { { "Id", "23" }, { "Name", "'Test23'" }, { "Count", "234" } });

            Assert.True(res);
        }

        [Test]
        public void Test2()
        {
            _sqlHelper.Update("Products",
                new Dictionary<string, string> { { "Id", "23" }, { "Name", "'Test23'" }, { "Count", "234" } },
                new Dictionary<string, string> { { "Id", "1" }, { "Name", "'Apaxic'" }, { "Count", "10" } });

            var res = _sqlHelper.IsRowExistedInTable("Products",
                new Dictionary<string, string> { { "Id", "1" }, { "Name", "'Apaxic'" }, { "Count", "10" } });

            Assert.True(res);
        }

        [Test]
        public void Test3()
        {
            _sqlHelper.Delete("Products",
                new Dictionary<string, string> { { "Id", "23" }, { "Name", "'Test23'" }, { "Count", "234" } });

            var res = _sqlHelper.IsRowExistedInTable("Products",
                new Dictionary<string, string> { { "Id", "23" }, { "Name", "'Test23'" }, { "Count", "234" } });

            Assert.False(res);
        }
    }
}