using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SqlTests
{
    [Binding]
    class CRUDSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private static readonly SqlHelper _sqlHelper;

        public CRUDSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"DB exists")]
        public void GivenBDIsExist()
        {
            
        }

        [Given(@"Computer is connected to DB")]
        public void ComputerIsConnectedToDB()
        {
            _sqlHelper.OpenConnection();
        }

        [Given(@"Record exist")]
        public void RecordExist()
        {
            _sqlHelper.Insert("Products",
                new Dictionary<string, string> { { "Id", "100" }, { "Name", "'Test100'" }, { "Count", "100" } });
        }

        [When(@"I send create request")]
        public void WhenInsertToDB()
        {
            _sqlHelper.Insert("Products",
                new Dictionary<string, string> { { "Id", "23" }, { "Name", "'Test23'" }, { "Count", "234" } });
        }

        [When(@"I send delete request")]
        public void WhenDeleteFromDB()
        {
            _sqlHelper.Delete("Products",
                new Dictionary<string, string> { { "Id", "100" }, { "Name", "'Test100'" }, { "Count", "100" } });
        }

        [When(@"I send update request")]
        public void WhenUpdateInDB()
        {
            _sqlHelper.Update("Products",
                new Dictionary<string, string> { { "Id", "23" }, { "Name", "'Test23'" }, { "Count", "234" } },
                new Dictionary<string, string> { { "Id", "31" }, { "Name", "'Kinder'" }, { "Count", "12" } });
        }

        [When(@"I send update request with dont exist record")]
        public void WhenUpdateNotExistedRecordInDB()
        {
            _sqlHelper.Update("Products",
                new Dictionary<string, string> { { "Id", "123" }, { "Name", "'VladA4'" }, { "Count", "12" } },
                new Dictionary<string, string> { { "Id", "222" }, { "Name", "'NotExist'" }, { "Count", "1" } });
        }

        [Then(@"record become in DB")]
        public void ThenRecordInDB()
        {
            var res = _sqlHelper.IsRowExistedInTable("Products",
                new Dictionary<string, string> { { "Id", "23" }, { "Name", "'Test23'" }, { "Count", "234" } });

            Assert.True(res);
        }

        [Then(@"record deleted from DB")]
        public void ThenRecordDeletedFromDB()
        {
            var res = _sqlHelper.IsRowExistedInTable("Products",
                new Dictionary<string, string> { { "Id", "100" }, { "Name", "'Test100'" }, { "Count", "100" } });

            Assert.False(res);
        }

        [Then(@"record updated in DB")]
        public void ThenRecordUpdatedInDB()
        {
            var res = _sqlHelper.IsRowExistedInTable("Products",
                new Dictionary<string, string> { { "Id", "31" }, { "Name", "'Kinder'" }, { "Count", "12" } });

            Assert.True(res);
        }


        [Then(@"records dont change")]
        public void ThenRecordDontUpdatedInDB()
        {
            var res = _sqlHelper.IsRowExistedInTable("Products",
                new Dictionary<string, string> { { "Id", "222" }, { "Name", "'NotExist'" }, { "Count", "1" } });

            Assert.False(res);
        }
    }
}