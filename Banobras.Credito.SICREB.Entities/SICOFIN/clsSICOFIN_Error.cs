using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Banobras.Credito.SICREB.Entities.SICOFIN
{
    public class clsSICOFIN_Error
    {
        public class Link
        {
            public string href { get; set; }
            public string rel { get; set; }
            public string method { get; set; }
        }

        public class Data
        {
            public string statusCode { get; set; }
            public string errorCode { get; set; }
            public string errorSource { get; set; }
            public string errorMessage { get; set; }
            public string errorHint { get; set; }
            public List<object> requestIssues { get; set; }
        }

        public class Root
        {
            public string status { get; set; }
            public string dataType { get; set; }
            public string payloadType { get; set; }
            public string version { get; set; }
            public int dataItems { get; set; }
            public string requestId { get; set; }
            public List<Link> links { get; set; }
            public Data data { get; set; }
        }
    }
}
