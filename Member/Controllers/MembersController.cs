using Member.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace Member.Controllers
{
    public class MembersController : Controller
    {
        // GET: Members

       
        public ActionResult Index(string option, string searchString, string searchStringG, string searchMT, string searchStringD)
        {
            MemberTypeDBHandle mtdb = new MemberTypeDBHandle();
            ViewBag.MemberType = mtdb.GetMemberTypes();
            ViewBag._Option = option;
            MemberDBHandle dbhandle = new MemberDBHandle();
            if (option == "Name")
            {
                if (searchString == null || searchString=="")
                {
                    return View(dbhandle.GetMembers());
                     }
                else {
                    return View(dbhandle.GetMembers().FindAll(member => member.fullName.ToLower() == searchString.ToLower()));

                }
            }

            else if (option == "Address")
            {
                if (searchString != null)
                {
                    return View(dbhandle.GetMembers().FindAll(member => member.address.ToLower() == searchString.ToLower()));

                }
                else
                {
                    return View(dbhandle.GetMembers());
                }
            }
            else if (option == "Gender") {
                return View(dbhandle.GetMembers().FindAll(member => member.gender.ToLower().Trim() == searchStringG.ToLower()));
            }
            else if (option == "Member Type") {

                return View(dbhandle.GetMembers().FindAll(member => member.memberTypeId == int.Parse(searchMT)));
            }
            else if (option == "Entry Date") {
                if (searchStringD == null || searchStringD == "")
                {
                    return View(dbhandle.GetMembers());
                         }
                else {
                    var ed = GetDateFormatInYYYMMDD(searchStringD);
                    return View(dbhandle.GetMembers().FindAll(member => member.entryDate == ed));


                }

            }
            else if (option == "Expiry Date") {
                if (searchStringD == null || searchStringD == "")
                {
                    return View(dbhandle.GetMembers());
                    }
                else
                {
                    var exd = GetDateFormatInYYYMMDD(searchStringD);
                    return View(dbhandle.GetMembers().FindAll(member => member.expiryDate == exd));

                }
            }
            else
            {
                return View(dbhandle.GetMembers());
            }


        }
       

        // GET: Members/Details/5
        public ActionResult Details(int id)
        {
            MemberDBHandle mdb = new MemberDBHandle();
            return View(mdb.GetMembers().Find(member => member.memberId == id));
        }

        // GET: Members/Create
        public ActionResult Create()
        {
            MemberTypeDBHandle mtdb = new MemberTypeDBHandle();
            ViewBag.MemberType = mtdb.GetMemberTypes();
            return View();
            
        }

        // POST: Members/Create
        [HttpPost]
        public ActionResult Create(Members member, FormCollection frm)
        {
            
            try

            {
                
                if (member.imageFile != null)
                {
                    string imgFileName = Path.GetFileNameWithoutExtension(member.imageFile.FileName);
                    string imgFileExtension = Path.GetExtension(member.imageFile.FileName);
                    imgFileName = DateTime.Now.ToString("yyyy-MM-dd-h-m-ff-tt") + "-" + imgFileName.Trim() + imgFileExtension;
                    member.image = imgFileName;
                    string fullImagePath = Path.Combine(Server.MapPath("~/MemberImages"), imgFileName);
                    member.imageFile.SaveAs(fullImagePath);
                }
                else { 
                    member.image = null;
                }

                if (member.attachmentFile != null) {
                    string attachmentFileName = Path.GetFileNameWithoutExtension(member.attachmentFile.FileName);
                    string attachmentFileExtension = Path.GetExtension(member.attachmentFile.FileName);
                    attachmentFileName = DateTime.Now.ToString("yyyy-MM-dd-h-m-ff-tt") + "-" + attachmentFileName.Trim() + attachmentFileExtension;
                    member.attachment = attachmentFileName;
                    string fullAttachmentPath = Path.Combine(Server.MapPath("~/MemberAttachments"), attachmentFileName);
                    member.attachmentFile.SaveAs(fullAttachmentPath);
                }
                else
                {
                    member.attachment = null;
                }
                member.entryDate = GetDateFormatInYYYMMDD(frm["entryDate"].ToString());
                member.expiryDate = GetDateFormatInYYYMMDD(frm["expiryDate"].ToString());
                if (User.Identity.IsAuthenticated)
                {
                    if (!ModelState.IsValid)
                    {
                        MemberDBHandle mdb = new MemberDBHandle();
                        if (mdb.AddMember(member))
                        {
                            TempData["SuccessMessage"] = "Member " + member.fullName + " added successfully on " + DateTime.Now.ToString();

                            ModelState.Clear();
                        }
                    }

                    return RedirectToAction("Create");

                }
                else {
                    return RedirectToAction("Index","Account");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Members/Edit/5
        public ActionResult Edit(int id)
        {
            MemberTypeDBHandle mtdb = new MemberTypeDBHandle();
            ViewBag.MemberType = mtdb.GetMemberTypes();
            MemberDBHandle mdb = new MemberDBHandle();
            return View(mdb.GetMembers().Find(member => member.memberId == id));
            
        }

        // POST: Members/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Members member,FormCollection frm)
        {
            try
            {
                string oldImageFile = member.image;
                string oldAttachmentFile = member.attachment;

                if (member.imageFile != null)
                {
                    
                    try { string oldimagePath = Path.Combine(Server.MapPath("~/MemberImages"), oldImageFile); System.IO.File.Delete(oldimagePath); } catch { }
                    string imgFileName = Path.GetFileNameWithoutExtension(member.imageFile.FileName);
                    string imgFileExtension = Path.GetExtension(member.imageFile.FileName);
                    imgFileName = DateTime.Now.ToString("yyyy-MM-dd-h-m-ff-tt") + "-" + imgFileName.Trim() + imgFileExtension;
                    member.image = imgFileName;
                    string fullImagePath = Path.Combine(Server.MapPath("~/MemberImages"), imgFileName);
                    member.imageFile.SaveAs(fullImagePath);
                }
                else
                {
                    if (oldImageFile == null)
                    {
                        member.image = null;
                    }
                    else {
                        member.image = oldImageFile;
                    }
                    
                }

                if (member.attachmentFile != null)
                {
                    
                    try { string oldattachmentPath = Path.Combine(Server.MapPath("~/MemberAttachments"), oldAttachmentFile); System.IO.File.Delete(oldattachmentPath); } catch { }
                    string attachmentFileName = Path.GetFileNameWithoutExtension(member.attachmentFile.FileName);
                    string attachmentFileExtension = Path.GetExtension(member.attachmentFile.FileName);
                    attachmentFileName = DateTime.Now.ToString("yyyy-MM-dd-h-m-ff-tt") + "-" + attachmentFileName.Trim() + attachmentFileExtension;
                    member.attachment = attachmentFileName;
                    string fullAttachmentPath = Path.Combine(Server.MapPath("~/MemberAttachments"), attachmentFileName);
                    member.attachmentFile.SaveAs(fullAttachmentPath);
                }
                else
                {
                    if (oldAttachmentFile == null)
                    {
                        member.attachment = null;
                    }
                    else
                    {
                        member.attachment = oldAttachmentFile;
                    }
                }
                member.entryDate = GetDateFormatInYYYMMDD(frm["entryDate"].ToString());
                member.expiryDate = GetDateFormatInYYYMMDD(frm["expiryDate"].ToString());

                if (User.Identity.IsAuthenticated)
                {

                    MemberDBHandle mdb = new MemberDBHandle();
                    mdb.UpdateDetails(member);
                    return RedirectToAction("Details", new { id = member.memberId });
                }
                else {
                    return RedirectToAction("Index", "Account");
                }
                }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public DateTime GetDateFormatInYYYMMDD(string date)
        {
            //dd//mm/yyyy
            string[] _date = date.Split('/');
            string d = _date[2] + "/" + _date[1] + "/" + _date[0];
            return Convert.ToDateTime(d);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                MemberDBHandle mdb = new MemberDBHandle();
                Member.Models.Members m = mdb.GetMembers().Find(member => member.memberId == id);
                if (User.Identity.IsAuthenticated)
                {
                    if (m.image != null && m.image != "")
                    {
                        string fullImagePath = Path.Combine(Server.MapPath("~/MemberImages"), m.image);
                        System.IO.File.Delete(fullImagePath);
                    }
                    if (m.attachment != null && m.attachment != "") {
                        string fullAttachmentPath = Path.Combine(Server.MapPath("~/MemberAttachments"), m.attachment);
                        System.IO.File.Delete(fullAttachmentPath);

                    }

                    if (mdb.DeleteMember(id))
                    {
                        ViewBag.AlertMsg = "Member Deleted Successfully";
                    }
                    return RedirectToAction("Index"); }
                else { return RedirectToAction("Index", "Account"); }
            }
            catch
            {
                return View();
            }
        }

    }
}
