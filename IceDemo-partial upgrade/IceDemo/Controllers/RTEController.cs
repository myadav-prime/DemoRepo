using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using RTE;
using System.Configuration;
using DevExpress.Web.Mvc;
using DevExpress.Web.ASPxUploadControl;

namespace IceDemo.Controllers
{
    public class RTEController : Controller
    {
        //
        // GET: /RTE/

        public ActionResult Index(int formid=0, bool clean=false)
        {
            var includeDocument = Convert.ToBoolean( ConfigurationManager.AppSettings.Get("includeDocument"));
            var html = "<p>&nbsp;</p>";
            ViewBag.clean = clean;
            using (var db = new IceDemoEntities())
            {
                var list = new List<SelectListItem>();
                var dataList = db.RTFForms.ToList();
                list.Add(new SelectListItem() { Value = "0", Text ="--Select--" });
                foreach (var tmp in dataList)
                {
                    list.Add(new SelectListItem() { Value = tmp.Id.ToString(), Text = tmp.FormName, Selected = (tmp.Id == formid) });
                }
           
                if (formid > 0)
                {
                    var obj = dataList.Where(x => x.Id == formid).FirstOrDefault();
                    if (obj != null)
                    {
                        html = clean ? obj.FormRTFClean : obj.FormRTF;
                        if (!html.StartsWith("<p>", StringComparison.OrdinalIgnoreCase))
                        {
                            html = "<p>" + html + "</p>";
                        }
                    }
                }
                ViewBag.list = list;
            }

            Editor Editor1 = new Editor(System.Web.HttpContext.Current, "Editor1");
            Editor1.LoadFormData(html);
            Editor1.ContentCss = "~/Contents/ice.css";
            //Editor1.PasteMode = RTEPasteMode.ConfirmWord;
            //Editor1.Toolbar = "minimal";
            //Editor1.Toolbar = "email";
            Editor1.ToolbarItems = @"{bold,italic,underline}{forecolor,fontname,fontsize}{justifymenu}{insertorderedlist,insertunorderedlist,outdent,indent}
          {insertlink,insertgallery,insertimage}{unlink,print"+(includeDocument?",insertdocument":"")+"}";
            Editor1.DisableStaticLinks = true;

            Editor1.MvcInit();
            Editor1.ShowBottomBar = false;
            ViewBag.Editor = Editor1.MvcGetString();
            
            Editor Editor2 = new Editor(System.Web.HttpContext.Current, "Editor2");
            Editor2.LoadFormData("This is content 2");
            Editor2.ToolbarItems = @"{bold,italic,underline}{forecolor,fontname,fontsize}{justifymenu}{insertorderedlist,insertunorderedlist,outdent,indent}
          {insertlink,insertgallery,insertimage}{unlink,print" + (includeDocument ? ",insertdocument" : "") + "}";
            Editor2.DisableStaticLinks = true;
            Editor2.MvcInit();
            Editor2.ShowBottomBar = false;
            ViewBag.Editor2 = Editor2.MvcGetString();
            return View();
        }

        public ActionResult Multiple()
        {
            Editor Editor2 = new Editor(System.Web.HttpContext.Current, "Editor2");
            //Editor2.LoadFormData("This is content 2");
            Editor2.Text = "Type Here";
            Editor2.ToolbarItems = @"{bold,italic,underline}{forecolor,fontname,fontsize}{justifymenu}{insertorderedlist,insertunorderedlist,outdent,indent}
          {insertlink,insertgallery,insertimage}{unlink,print}";
            Editor2.DisableStaticLinks = true;
            Editor2.MvcInit();
            Editor2.ShowBottomBar = false;
            ViewBag.Editor2 = Editor2.MvcGetString();
            return View();
        }
        public ActionResult DevEx(int formid = 0, bool clean = false)
        {
            var includeDocument = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("includeDocument"));
            var html = "<p>&nbsp;</p>";
            ViewBag.clean = clean;
            using (var db = new IceDemoEntities())
            {
                var list = new List<SelectListItem>();
                var dataList = db.RTFForms.ToList();
                list.Add(new SelectListItem() { Value = "0", Text = "--Select--" });
                foreach (var tmp in dataList)
                {
                    list.Add(new SelectListItem() { Value = tmp.Id.ToString(), Text = tmp.FormName, Selected = (tmp.Id == formid) });
                }

                if (formid > 0)
                {
                    var obj = dataList.Where(x => x.Id == formid).FirstOrDefault();
                    if (obj != null)
                    {
                        html = clean ? obj.FormRTFClean : obj.FormRTF;
                        if (!html.StartsWith("<p>", StringComparison.OrdinalIgnoreCase))
                        {
                            html = "<p>" + html + "</p>";
                        }
                    }
                }
                ViewBag.list = list;
                ViewBag.html = html;
                ViewBag.hasHeader = true;
            }
            
            return View();
        }

        public ActionResult HtmlEditorPartial()
        {
            return PartialView("HtmlEditorPartial");
        }
        public ActionResult HtmlEditorPartialMulti()
        {
            return PartialView("HtmlEditorPartialMulti");
        }
        public ActionResult ImageUpload()
        {

            HtmlEditorExtension.SaveUploadedImage("myHtmlEditor", HtmlEditorDemosHelper.ImageUploadValidationSettings, "~/Uploads/");
            return null;
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveHTML(string html, string cleanhtml, int formid, string formname)
        {
            RTFForm form = null;
            using (var db = new IceDemoEntities())
            {
                if (formid > 0)
                {
                    form = db.RTFForms.Where(x => x.Id == formid).FirstOrDefault();

                }
                else
                {
                    form = new RTFForm();
                    db.RTFForms.Add(form);
                    form.FormName = formname;
                }
                form.FormRTF = html;
                form.FormRTFClean = cleanhtml;
                db.SaveChanges();
            }
            return Json(new { success = true, formid = form.Id });
        }
        
    }
    public class HtmlEditorDemosHelper
    {
        public static readonly ValidationSettings ImageUploadValidationSettings = new ValidationSettings
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".png" },
            MaxFileSize = 4000000
        };

    }

}
