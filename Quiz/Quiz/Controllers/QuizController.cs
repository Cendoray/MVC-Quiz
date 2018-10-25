using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quiz.Controllers
{
    public class QuizController : Controller
    {
        // GET: Quiz
        [HttpGet]
        [Authorize]
        public ActionResult choseCategory(String categoryID)
        {
            using (var db = new QuizEntities())
            {
                var query = db.Questions.Where(quest => quest.username == User.Identity.Name 
                && quest.category_ID == categoryID 
                && db.QuestionsAnswereds
                .Contains(db.QuestionsAnswereds
                .Where(questA => questA.question_ID == quest.question_ID)
                .FirstOrDefault()) == false)
               .OrderBy(quest => quest.question_ID)
                .Select(quest => quest).ToList();
                QuizQuestions qa = new QuizQuestions(query);
                return View(qa);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult choseCategory(FormCollection frm)
        {
            QuizQuestions qq = (QuizQuestions)TempData.First(x => x.Key == "quiz").Value;
            QuestionsAnswered qa = new QuestionsAnswered();
            if (ModelState.IsValid)
            {
                using (var db = new QuizEntities())
                {
                    qa.username = User.Identity.Name;
                    qa.question_ID = qq.questionID[qq.questionPosition];
                    qa.user_choice = frm["Answer " + qq.categoryID];
                    qa.User = (from u in db.Users where u.username == qa.username select u).FirstOrDefault();
                    qa.Question = (from u in db.Questions where u.question_ID == qa.question_ID select u).FirstOrDefault();
                    db.QuestionsAnswereds.Add(qa);
                    //db.SaveChanges();
                }
            }
            qq.userAnswer[qq.questionPosition] = frm["Answer " + qq.categoryID];
            qq.questionPosition++;
            if (qq.questionPosition >= qq.questionID.Count)
            {
                return RedirectToAction("endScreen", "Quiz");

            }
            TempData.Remove("quiz");
            return View(qq);
        }


        // GET: Quiz
        [HttpGet]
        [Authorize]
        public ActionResult endScreen()
        {
            QuizQuestions qq = (QuizQuestions)TempData.First(x => x.Key == "quiz").Value;
            return View(qq);
        }
    }
}