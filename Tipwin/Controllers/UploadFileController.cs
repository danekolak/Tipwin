using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Tipwin.Controllers
{
    public class UploadFileController : Controller
    {


        public ActionResult Index()
        {
            return View();
        }

        private bool IsValidContentType(string contentType)
        {
            return contentType.Equals("image/png") || contentType.Equals("image/gif") || contentType.Equals("image/jpg") || contentType.Equals("image/jpeg");
        }

        private bool IsValidContentLength(double contentLength)
        {
            return ((contentLength / 1024) / 1024) > 0.1 && (((contentLength / 1024) / 1024) < 6); //od 100kb do 5MB
        }


        [HttpPost]
        public ActionResult Process(HttpPostedFileBase photo)
        {
            try
            {
                if (!IsValidContentType(photo.ContentType))
                {
                    ViewBag.Error = "Jedino JPG, JPEG, PNG & GIF su dopušteni";
                    return View("Index");
                }
                else if (!IsValidContentLength(photo.ContentLength))
                {
                    ViewBag.Error = "Slike nisu u rasponu od 100kb do 5 mb";
                    return View("Index");
                }

                else
                {
                    if (photo.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(photo.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                        photo.SaveAs(path);
                        ViewBag.fileName = photo.FileName;
                    }
                    return View("Success");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", $"Error: {e}");
            }
            return View();
        }
    }
}
