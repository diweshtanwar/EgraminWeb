using EgraminWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace EgraminWeb.Controllers
{
    public class PartialController : Controller
    {
        private ApplicationWebEntities db = new ApplicationWebEntities();
        // GET: Partial
        [ChildActionOnly]
        public ActionResult GetNotification()
        {
            var NotificationSubject = db.tblNotifications.Where(d => d.Status == true).Select(d => d.NotificationSubject).FirstOrDefault();
            ViewBag.NotificationSubject = NotificationSubject;          
            return View();
        }

        [ChildActionOnly]
        public ActionResult GetNotificationDetails()
        {
            var NotificationDetails = db.tblNotifications.Where(d=>d.Status==true).Select(d=>d.NotificationDetails).FirstOrDefault();
            ViewBag.NotificationDetails = NotificationDetails;        
            return View();
        }
        [ChildActionOnly]
        public ActionResult GetGalleryImagesForTittle(int? id)
        {
            tblGalleryCategory tblGalleryCategory = db.tblGalleryCategories.Find(id);
            List<string> getImgesList = new List<string>();
            getImgesList = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory+tblGalleryCategory.CategoryImagesPath.ToString(), "*.jpg", SearchOption.AllDirectories).Take(4).ToList();
            List<string> ImgesListWithPath = new List<string>();
            foreach (string file in getImgesList)
            {
                var fileName = Path.GetFileName(file);
                ImgesListWithPath.Add("~\\"+tblGalleryCategory.CategoryImagesPath.ToString()+"\\"+fileName);
            }

            ViewBag.ImgesListWithPath = ImgesListWithPath;
            return View();
        }
        [ChildActionOnly]
        public ActionResult GetLatestNewsForHome()
        {          
            return View(db.tblLatestNews.Where(d => d.Status == true).OrderBy(d=>d.Priority).Take(3).ToList());
        }

        [ChildActionOnly]
        public ActionResult GetSubNewsForNewsDetails()
        {
            return View(db.tblLatestNews.Where(d => d.Status == true).OrderBy(d=>d.Priority).Take(5).ToList());
        }
    }
}
