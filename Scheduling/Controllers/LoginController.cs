using Scheduling.Models;
using Scheduling.Models.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scheduling.Controllers
{
    public class LoginController : Controller
    {
        DBEntities db = new DBEntities();

        // GET: Login
        public ActionResult Index()
        {
            var accounts = from d in db.AccountTypes
                           select new
                           {
                               Id = d.ATID,
                               Name = d.Description
                           };
            ViewBag.Account_Id = new SelectList(accounts, "Id", "Name");

            return View();

        }


        [HttpPost]
        public ActionResult Index(LoginVM vm)
        {
            Login log = new Login();

            SessionVM sessionVM = new SessionVM();
            if (ModelState.IsValid)
            {
                var account_id = vm.Account_Id;

                if (account_id == 3) // AO
                {
                    if (db.Academics.Where(l => l.Password == vm.Password && (l.AdminId.ToString() == vm.Username || l.Email == vm.Username)).Count() > 0)
                    {
                        Session["Username"] = db.Academics.Where(l => l.Password == vm.Password && (l.AdminId.ToString() == vm.Username || l.Email == vm.Username))
                            .Select(s=>s.Name).Single();
                        return RedirectToAction("Schedule", "AcademicOfficer");
                    }
                    else
                    {
                        var accounts = from d in db.AccountTypes
                                       select new
                                       {
                                           Id = d.ATID,
                                           Name = d.Description
                                       };
                        ViewBag.Account_Id = new SelectList(accounts, "Id", "Name");
                        //ModelState.AddModelError("keyName", "Message");
                        ModelState.AddModelError(string.Empty, "Incorrect details!");
                    }
                }
                else if (account_id == 4) // AAC
                {
                    if (db.Academics.Where(l => l.Password == vm.Password && (l.AdminId.ToString() == vm.Username || l.Email == vm.Username)).Count() > 0)
                    {
                        Session["Username"] = db.Academics.Where(l => l.Password == vm.Password && (l.AdminId.ToString() == vm.Username || l.Email == vm.Username))
                            .Select(s => s.Name).Single();
                        return RedirectToAction("Schedule", "AAC");
                    }
                    else
                    {
                        var accounts = from d in db.AccountTypes
                                       select new
                                       {
                                           Id = d.ATID,
                                           Name = d.Description
                                       };
                        ViewBag.Account_Id = new SelectList(accounts, "Id", "Name");
                        //ModelState.AddModelError("keyName", "Message");
                        ModelState.AddModelError(string.Empty, "Incorrect details!");
                    }
                }
                else if (account_id == 5) // AA
                {
                    if (db.Academics.Where(l => l.Password == vm.Password && (l.AdminId.ToString() == vm.Username || l.Email == vm.Username)).Count() > 0)
                    {
                        Session["Username"] = db.Academics.Where(l => l.Password == vm.Password && (l.AdminId.ToString() == vm.Username || l.Email == vm.Username))
                            .Select(s => s.Name).Single();
                        return RedirectToAction("Schedule", "AcademicAssistant");
                    }
                    else
                    {
                        var accounts = from d in db.AccountTypes
                                       select new
                                       {
                                           Id = d.ATID,
                                           Name = d.Description
                                       };
                        ViewBag.Account_Id = new SelectList(accounts, "Id", "Name");
                        //ModelState.AddModelError("keyName", "Message");
                        ModelState.AddModelError(string.Empty, "Incorrect details!");
                    }
                }
                else if (account_id == 1) // Student
                {
                    if (db.Students.Where(l => l.Password == vm.Password && (l.StudentId.ToString() == vm.Username || l.Email == vm.Username)).Count() > 0)
                    {
                        var data = db.Students.Where(l => l.Password == vm.Password && (l.StudentId.ToString() == vm.Username || l.Email == vm.Username))
                            .Select(s => new { s.StudentId, s.ATID, s.StudentName, s.section.Program.PId ,s.section.Program.Program1,s.secid,s.section.Class.Class1,s.section.sectionname });

                        sessionVM.PId = (int)data.Select(s => s.PId).Single();
                        sessionVM.studentname = data.Select(s => s.StudentName).Single();
                        sessionVM.studentid = data.Select(s => s.StudentId).Single();
                        sessionVM.ATID = (int)data.Select(s => s.ATID).Single();
                        sessionVM.program = data.Select(s => s.Program1).Single();
                        sessionVM.secid = (int)data.Select(s => s.secid).Single();
                        sessionVM.Class = data.Select(s => s.Class1).Single();
                        sessionVM.sectionname = data.Select(s => s.sectionname).Single();


                        Session["Username"] = sessionVM;
                        return RedirectToAction("Dashboard", "Student");
                    }
                    else
                    {
                        var accounts = from d in db.AccountTypes
                                       select new
                                       {
                                           Id = d.ATID,
                                           Name = d.Description
                                       };
                        ViewBag.Account_Id = new SelectList(accounts, "Id", "Name");
                        //ModelState.AddModelError("keyName", "Message");
                        ModelState.AddModelError(string.Empty, "Incorrect details!");
                    }
                }
                else if (account_id == 2) // Teacher
                {
                    if (db.teachers.Where(l => l.Password == vm.Password && (l.teacherid.ToString() == vm.Username || l.Email == vm.Username)).Count() > 0)
                    {
                        //Hashtable TeachersSession = new Hashtable();

                        var data = db.teachers.Where(l => l.Password == vm.Password && (l.teacherid.ToString() == vm.Username || l.Email == vm.Username))
                            .Select(s => new { s.teacherid, s.PId, s.Program.Program1, s.teachername,s.ATID });

                        //TeachersSession.Add("teachername", data.Select(s => s.teachername).Single());
                        //TeachersSession.Add("PId", (int)data.Select(s => s.PId).Single());

                        //session["EmpDetails"] = htEmpInfo;
                        if (data.Select(s => s.PId).Single() == null)
                        {
                            sessionVM.teachername = data.Select(s => s.teachername).Single();
                            sessionVM.teacherid = data.Select(s => s.teacherid).Single();
                            sessionVM.ATID = (int)data.Select(s => s.ATID).Single();
                            

                            Session["Username"] = sessionVM;
                            return RedirectToAction("TeacherDashboard", "Teacher");
                        }
                        else
                        {
                            sessionVM.PId = (int)data.Select(s => s.PId).Single();
                            sessionVM.teachername = data.Select(s => s.teachername).Single();
                            sessionVM.teacherid = data.Select(s => s.teacherid).Single();
                            sessionVM.ATID = (int)data.Select(s => s.ATID).Single();
                            sessionVM.program = data.Select(s => s.Program1).Single();

                            Session["Username"] = sessionVM;
                            return RedirectToAction("PMDashboard", "Teacher");
                        }

                        
                    }
                    else
                    {
                        var accounts = from d in db.AccountTypes
                                       select new
                                       {
                                           Id = d.ATID,
                                           Name = d.Description
                                       };
                        ViewBag.Account_Id = new SelectList(accounts, "Id", "Name");
                        //ModelState.AddModelError("keyName", "Message");
                        ModelState.AddModelError(string.Empty, "Incorrect details!");
                    }
                }

                
                return View();
            }
            else
            {
                var accounts = from d in db.AccountTypes
                               select new
                               {
                                   Id = d.ATID,
                                   Name = d.Description
                               };
                ViewBag.Account_Id = new SelectList(accounts, "Id", "Name");
                //ModelState.AddModelError("keyName", "Message");
                ModelState.AddModelError(string.Empty, "Incorrect details!");
                return View();
            }




        }
    }
}