using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using RTE;

namespace IceDemo.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Test()
        {
            return View();
        }

        public ActionResult StandAlone()
        {
            return View();
        }
        public ActionResult WithTE()
        {
            return View();
        }
        public ActionResult WithTinyMCE()
        {
            return View();
        }
        public ActionResult ProvidedDemo()
        {
            return View();
        }
        public ActionResult WithRTE(bool tracked=true) {
            //var tracked = true;
            Editor Editor1 = new Editor(System.Web.HttpContext.Current, "Editor1");
            //Editor1.LoadFormData("Type Here...");
            var path = tracked ? @"d:\html.txt" : @"d:\cleanhtml.txt";
            var html = System.IO.File.ReadAllText(path);
            //var html = System.IO.File.ReadAllText("d:\\test.rtf");

            
            Editor1.LoadFormData(html);
            //Editor1.LoadRTF("d:\\t.rtf");
            Editor1.ContentCss = "~/Contents/ice.css";
            //Editor1.Toolbar = "minimal";
            
            
            Editor1.MvcInit();
            ViewBag.Editor = Editor1.MvcGetString();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveHTML(string html, string cleanhtml)
        {
            System.IO.File.WriteAllText(@"d:\html.txt", html);
            System.IO.File.WriteAllText(@"d:\cleanhtml.txt", cleanhtml);
            return Json(new {success=true});
        }
        public string GetHTML(bool tracked) {
            var path = tracked ? @"d:\html.txt" : @"d:\cleanhtml.txt";
            var html = System.IO.File.ReadAllText(path);
            return html;
        }
    }
}
