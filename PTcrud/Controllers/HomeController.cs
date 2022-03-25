using PTcrud.Dbcomp;
using PTcrud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PTcrud.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(adminmodel obj)
         {
            comp_detailEntities ent = new comp_detailEntities();
            var res = ent.admindetails.Where(m => m.usr_name == obj.usr_name).FirstOrDefault();
            if (res == null)
            {
                TempData["invalidemail"] = "enter the valid Username";

            }
            else
            {
                if (res.usr_name == obj.usr_name && res.pass == obj.pass)
                {
                    FormsAuthentication.SetAuthCookie(res.usr_name, false);
                    Session["email"] = res.usr_name;
                    return RedirectToAction("Dashboard", "Home");

                }
                else
                {
                    TempData["wrngpawrd"] = "Your passwor is wrong";
                }

            }
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }


        [Authorize]
        [HttpGet]
        public ActionResult MyPage()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult MyPage(modcl1 obj)
        {
            empl_dtl emp = new empl_dtl();
            comp_detailEntities res = new comp_detailEntities();


            emp.Emp_id = obj.Emp_id;
            emp.Emp_name = obj.Emp_name;
            emp.mb_no = obj.mb_no;
            emp.email = obj.email;

            


            if (obj.Emp_id == 0)
            {
                res.empl_dtl.Add(emp);
                res.SaveChanges();
            }
            else
            {
                res.Entry(emp).State = System.Data.Entity.EntityState.Modified;
                res.SaveChanges();

            }


            return RedirectToAction("Table","Home");
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            modcl1 md = new modcl1();

            comp_detailEntities res = new comp_detailEntities();
            var edt = res.empl_dtl.Where(m => m.Emp_id == id).First();
            md.Emp_id = edt.Emp_id;
            md.Emp_name = edt.Emp_name;
            md.mb_no = edt.mb_no;
            md.email = edt.email;



            return View("MyPage", md);

        }







        [Authorize]
        public ActionResult Table()
        {
            comp_detailEntities obj = new comp_detailEntities();
            var obj1 = obj.empl_dtl.ToList();




            return View(obj1);
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            comp_detailEntities obj = new comp_detailEntities();
            var deletedata = obj.empl_dtl.Where(m => m.Emp_id == id).First();

            obj.empl_dtl.Remove(deletedata);
            obj.SaveChanges();

            return RedirectToAction("Table","Home");
        }

        [HttpGet]

        public ActionResult usrregister()
        {


            return View();

        }


        [HttpPost]

        public ActionResult usrregister(adminmodel obj)
        {
            comp_detailEntities res = new comp_detailEntities();
            admindetail tab = new admindetail();
            tab.id = obj.id;
            tab.usr_name = obj.usr_name;
            tab.pass = obj.pass;
            res.admindetails.Add(tab);
            res.SaveChanges();


            return RedirectToAction("Index", "Home");

        }



        public ActionResult logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");

        }











        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}