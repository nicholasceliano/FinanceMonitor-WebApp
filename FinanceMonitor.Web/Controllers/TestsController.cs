using System;
using System.Web.Http;
using FinanceMonitor.Library;

namespace FinanceMonitor.Web.Controllers
{
    public class TestsController : ApiController
    {
        // GET api/Tests/test
        [HttpGet]
        public void test()
        {
            ValueRetrieval.Test();
            //throw new Exception("test exception");
        }

        // POST api/Tests/WebHookTest
        [HttpPost]
        public void WebHookTest(object value)
        {
            
        }
    }
}