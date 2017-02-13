using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.Models;

namespace ContosoUniversity.Controllers
{
    public class CourseController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Course
        public ActionResult Index(string sortOrderCourse)
        {
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrderCourse) ? "Title desc" : "";
            ViewBag.CreditsSortParm = sortOrderCourse == "Credits" ? "Credits desc" : "Credits";
            var courses = from s in db.Courses select s;
            switch (sortOrderCourse)
            {
                case "Title desc":courses = courses.OrderByDescending(s => s.Title);
                    break;
                case "Credits":courses = courses.OrderBy(s => s.Credits);
                    break;
                case "Credits desc":courses = courses.OrderByDescending(s => s.Credits);
                    break;
                case "Title":courses = courses.OrderBy(s => s.Title);
                    break;

            }
            return View(courses.ToList());
        }

        // GET: Course/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Course/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,Title,Credits")] Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Courses.Add(course);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch(DataException)
            {
                // Запись ошибок в журнал (добавление имени переменной после DataException)
                ModelState.AddModelError("", "Не удалось сохранить изменения. Попытайтесь снова, и если проблема не будет устранена, обратитесь к своему системному администратору.");
            }

            return View(course);
        }

        // GET: Course/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Course/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseID,Title,Credits")] Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(course).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                // Запись ошибок в журнал (добавление имени переменной после DataException)
                ModelState.AddModelError("", "Не удалось сохранить изменения. Попытайтесь снова, и если проблема не будет устранена, обратитесь к своему системному администратору.");
            }
            
            return View(course);
        }

        // GET: Course/Delete/5
        public ActionResult Delete(int id, bool? saveChangesError )
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Не удалось сохранить изменения. Попытайтесь снова, и если проблема не будет устранена, обратитесь к своему системному администратору.";
            }
            return View(db.Courses.Find(id));

            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Course course = db.Courses.Find(id);
            //if (course == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Course course = db.Courses.Find(id);
                db.Courses.Remove(course);
                db.SaveChanges();
            }
            catch (DataException)
            {
                // Запись ошибок в журнал (добавление имени переменной после DataException)
                return RedirectToAction("Delete", new System.Web.Routing.RouteValueDictionary {
                { "id", id },
                { "saveChangesError", true }});
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
