using Member.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Member.Controllers
{
    public class MemberTypeController : Controller
    {
        // GET: MemberType
        public ActionResult Index()
        {
            MemberTypeDBHandle dbhandle = new MemberTypeDBHandle();
            ModelState.Clear();
            return View(dbhandle.GetMemberTypes());
        }

        // GET: MemberType/Details/5
        public ActionResult Details(int id)
        {
            MemberTypeDBHandle mtdb = new MemberTypeDBHandle();
            return View(mtdb.GetMemberTypes().Find(memberType => memberType.memberTypeId == id));

        }

        // GET: MemberType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MemberType/Create
        [HttpPost]
        public ActionResult Create(MemberType memberType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MemberTypeDBHandle mtdb = new MemberTypeDBHandle();
                    if (mtdb.AddMemberType(memberType))
                    {
                        TempData["SuccessMessage"] = "MemberType "+memberType.type+ " Added Successfully";
                        ModelState.Clear();
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: MemberType/Edit/5
        public ActionResult Edit(int id)
        {
            MemberTypeDBHandle mtdb = new MemberTypeDBHandle();
            return View(mtdb.GetMemberTypes().Find(memberType => memberType.memberTypeId == id));
        }

        // POST: MemberType/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, MemberType memberType)
        {
            try
            {
                // TODO: Add update logic here
                MemberTypeDBHandle mtdb = new MemberTypeDBHandle();
                mtdb.UpdateDetails(memberType);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MemberType/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                MemberTypeDBHandle mtdb = new MemberTypeDBHandle();
                if (mtdb.DeleteMemberType(id))
                {
                    ViewBag.AlertMsg = "MemberType Deleted Successfully";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
    }
}
