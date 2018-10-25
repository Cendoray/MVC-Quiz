using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quiz.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        [HttpGet]
        public ActionResult Index()
        {
            if (User.Identity.Name != "")
            {
                return Redirect("~/Category/LoggedIndex");
            }
            using (QuizEntities con = new QuizEntities())
            {

                var query = from category in con.Categories
                            join qestion in con.Questions
                            on category.category_ID equals qestion.category_ID
                            group category by category.title;

                //query.Select(x => x.Count());
                return View(query.OrderBy(x => x.Count()).Select(x => x.Key).ToList());
            }
        }


        // GET: Category
        [Authorize]
        [HttpGet]
        public ActionResult LoggedIndex()
        {
            if (User.Identity.Name == "")
            {
                return Redirect("~/Category/Index");
            }
            using (QuizEntities con = new QuizEntities())
            {

                var query = from category in con.Categories
                            join question in con.Questions
                            on category.category_ID equals question.category_ID
                            group category by category.title;

                //query.Select(x => x.Count());
                return View(query.OrderBy(x => x.Count()).Select(x => "Category : " + x.Key + " ( " + x.Count() + " questions )").ToList());
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Description";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Pages";

            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult addQuestion()
        {
            AddQuestion aq = new AddQuestion();
            return View(aq);
        }
        [Authorize]
        [HttpPost]
        public ActionResult addQuestion(String t, String cat, String qId, String a1, String a2, String a3, String a4, String cA)
        {
            using (var db = new QuizEntities())
            {
                if (t == "" || cat == "" || a1 == "" || a2 == "" || a3 == "" || a4 == "" || cA == "")
                {
                    ModelState.AddModelError("", "Invalid parameters");
                }
                //add question ID

                Question question = new Question();
                question.answer1 = a1;
                question.answer2 = a2;
                question.answer3 = a3;
                question.answer4 = a4;
                question.correct_answer = cA;
                question.question_title = t;
                question.category_ID = cat;
                question.question_ID = qId;
                db.Questions.Add(question);
                db.SaveChanges();


                return View();
            }

        }

        [HttpGet]
        [Authorize]
        public ActionResult AddCategory()
        {
            using (var db = new QuizEntities())
            {

                var query = (from q in db.Categories
                             orderby q.title
                             select q);

                return View();
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddCategory(string title)
        {
            System.Diagnostics.Debug.WriteLine("CATEGORY = " + title);
            if (ModelState.IsValid)
            {
                using (QuizEntities con = new QuizEntities())
                {
                    var checkCategory = con.Categories.Where(x => x.title.CompareTo(title.Trim()) == 0);
                    if (checkCategory.FirstOrDefault() != null)
                    {
                        ModelState.AddModelError("", "Category already exists");
                    }
                    else
                    {

                        int newId = Convert.ToInt32(con.Categories.Count()) + 1;
                        System.Diagnostics.Debug.WriteLine(newId);
                        Category newCategory = new Category()
                        {
                            category_ID = newId.ToString(),
                            title = title
                        };

                        con.Categories.Add(newCategory);
                        con.SaveChanges();
                    }
                }
            }
            return View();
        }


    }
}
