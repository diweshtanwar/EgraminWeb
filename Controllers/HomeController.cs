using EgraminWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace EgraminWeb.Controllers
{

    public class HomeController : Controller
    {
        private ApplicationWebEntities db = new ApplicationWebEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult OurVision()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(tblEnquiry tblEnquiry)
        {
            tblEnquiry.Name = tblEnquiry.NewName;
            tblEnquiry.Email = tblEnquiry.NewEmail;
            tblEnquiry.Mobile = tblEnquiry.NewMobile;
            tblEnquiry.Message = tblEnquiry.NewMessage;
            tblEnquiry.CreatedBy= tblEnquiry.NewName;
            tblEnquiry.CreatedDate = DateTime.UtcNow;
            tblEnquiry.UpdatedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.tblEnquiries.Add(tblEnquiry);
                db.SaveChanges();
                return RedirectToAction("ContactSuccess", tblEnquiry);
            }

            return View(tblEnquiry);
        }
        public ActionResult ContactSuccess(tblEnquiry tblEnquiry)
        {
            tblMailConfiguration tblMailConfiguration = db.tblMailConfigurations.Where(d => d.Status == true).FirstOrDefault();
            if (tblMailConfiguration.IsNeedToSendMail==true)
            {
                SendEmail(tblMailConfiguration, tblEnquiry);
            }
            
            return View();
        }

        public ActionResult Careers()
        {
            return View();
        }
        public ActionResult FindBranch()
        {
            return View();
        }
  
        public ActionResult Downloads()
        {
            return View(db.tblDownloadDetails.Where(d=>d.Status==true).OrderBy(d => d.Priority).ToList());
        }

        public FileResult DownloadFile(string fileName)
        {
            //Build the File Path.
            string path = Server.MapPath(fileName);

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }
        public ActionResult GalleryCategories()
        {
            return View(db.tblGalleryCategories.Where(d=>d.Status==true).OrderBy(d=>d.Priority).ToList());
        }
        public ActionResult GalleryImages(int? id)
        {
            tblGalleryCategory tblGalleryCategory = db.tblGalleryCategories.Find(id);
            List<string> getImgesList = new List<string>();
            getImgesList = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + tblGalleryCategory.CategoryImagesPath.ToString(), "*.jpg", SearchOption.AllDirectories).ToList();
            List<string> ImgesListWithPath = new List<string>();
            foreach (string file in getImgesList)
            {
                var fileName = Path.GetFileName(file);
                ImgesListWithPath.Add("~\\" + tblGalleryCategory.CategoryImagesPath.ToString() + "\\" + fileName);
            }

            ViewBag.ImgesListWithPath = ImgesListWithPath;
            return View();
        }
        public ActionResult NewsAll()
        {
            return View(db.tblLatestNews.Where(d => d.Status == true).OrderBy(d => d.Priority).ToList());
        }
        public ActionResult NewsDetails(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLatestNew tblLatestNew = db.tblLatestNews.Find(id);
            if (tblLatestNew == null)
            {
                return HttpNotFound();
            }
            return View(tblLatestNew);
        }

        public ActionResult Error()
        {
            return View();

        }


        public static void SendEmail(tblMailConfiguration tblMailConfiguration, tblEnquiry tblEnquiry)
        {
            try
            {

                string smtpAddress = tblMailConfiguration.SmtpAddress;
                int portNumber = Convert.ToInt32(tblMailConfiguration.PortNumber);
                bool enableSSL = Convert.ToBoolean(tblMailConfiguration.EnableSSL);
                string emailFromAddress = tblMailConfiguration.EmailFromAddress;
                string password = tblMailConfiguration.Password;
                string emailToAddress = tblMailConfiguration.EmailToAddress + tblEnquiry.Email;
                string emailBccAddress = tblMailConfiguration.EmailBccAddress;
                string subject = tblMailConfiguration.Subject;
                string body = tblMailConfiguration.Body;
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailFromAddress);
                    mail.To.Add(emailToAddress);
                    mail.Bcc.Add(emailBccAddress);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = Convert.ToBoolean(tblMailConfiguration.IsBodyHtml);
                    if (tblMailConfiguration.IsNeedToSendAttachments == true)
                    {
                        mail.Attachments.Add(new Attachment(tblMailConfiguration.AttachmentsPath));
                    }

                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}