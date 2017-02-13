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
    public class StudentController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Student
        public ViewResult Index(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name desc" : "";
            ViewBag.FNameSortParm = sortOrder == "FName" ? "FName desc" : "FName";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date desc" : "Date";
            var students = from s in db.Students select s;
            switch (sortOrder)
            {
                case "Name desc":students = students.OrderByDescending(s => s.LastName);
                    break;
                case "FName desc":students = students.OrderByDescending(s => s.FirstMidName);
                    break;
                case "Date":students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "Date desc": students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                case "FName":students = students.OrderBy(s => s.FirstMidName);
                    break;
                case "Name": students = students.OrderBy(s => s.LastName);
                    break;
            }
            return View(students.ToList());
        }

        // GET: Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentID,LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Students.Add(student);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            catch (DataException)
            {
                // Запись ошибок в журнал (добавление имени переменной после DataException)
                ModelState.AddModelError("", "Не удалось сохранить изменения. Попытайтесь снова, и если проблема не будет устранена, обратитесь к своему системному администратору.");
            }

            return View(student);
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentID,LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(student).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                // Запись ошибок в журнал (добавление имени переменной после DataException)
                ModelState.AddModelError("", "Не удалось сохранить изменения. Попытайтесь снова, и если проблема не будет устранена, обратитесь к своему системному администратору.");
            }
                    
            return View(student);
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int id, bool? saveChangesError)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Не удалось сохранить изменения. Попытайтесь снова, и если проблема не будет устранена, обратитесь к своему системному администратору.";
            }
            return View(db.Students.Find(id));

            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Student student = db.Students.Find(id);
            //if (student == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Student student = db.Students.Find(id);
                db.Students.Remove(student);
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
