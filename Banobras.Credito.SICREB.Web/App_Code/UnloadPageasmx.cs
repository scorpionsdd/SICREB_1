using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;

/// <summary>
/// Summary description for UnloadPageasmx
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
[ScriptService]
public class UnloadPageasmx : WebService {

    public UnloadPageasmx () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public string HelloWorld() {
        JavaScriptSerializer oJavaScriptSerializer = new JavaScriptSerializer();

        return (oJavaScriptSerializer.Serialize("Hello World"));
    }
    
}
