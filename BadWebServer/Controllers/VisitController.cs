using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace November2018.BadWebServer.Controllers
{
    public class VisitController : Controller
    {
        private static List<string> _visitors = new List<string>();

        //My controller methods can have some pretty complex logic buried inside.  In this example, I'll change my "visit" path to 
        //look for another optional parameter, a string called "format".  By default, visit will return HTML content.
        //But, if my format is XML or JSON, I'll run different logic to return the requested content type.
        //E.G.: http://localhost:5000/visit returns HTML
        //E.G.: http://localhost:5000/visit?format=json returns JSON
        //E.G.: http://localhost:5000/visit?format=xml returns XML
        public IActionResult Index(string format = "html")
        {
            var connectingIp = this.ControllerContext.HttpContext.Connection.RemoteIpAddress;
            var formatted = connectingIp.MapToIPv4().ToString();
            _visitors.Add(formatted);
            switch (format.ToLowerInvariant())
            {
                case "xml":
                    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(_visitors.GetType());
                    StringWriter stringWriter = new StringWriter();
                    xmlSerializer.Serialize(stringWriter, _visitors);
                    return Content(stringWriter.ToString());
                case "json":
                    string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(_visitors);
                    return Content(jsonContent);
                default:
                    return View(_visitors);
            }

        }
    }
}
