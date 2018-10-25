using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiz.Models
{
    public class AddQuestion
    {
        public String answer1 { get; set; }
        public String answer2 { get; set; }

        public String answer3 { get; set; }
        public String answer4 { get; set; }

        public String correctanswer { get; set; }
        public String questionID { get; set; }

        public String title { get; set; }
        public String category { get; set; }
        public List<Category> allCategory { get; set; }

        public AddQuestion()
        {
            allCategory = new List<Category>();
        }

        public AddQuestion(String t, String cat, String qID, String a1, String a2, String a3, String a4, String cA)
        {
            allCategory = new List<Category>();
            using (var db = new QuizEntities())
            {
                allCategory = db.Categories.Where(cate => cate.category_ID != null).Select(cate => cate).ToList();
            }
            answer1 = a1;
            answer2 = a2;
            answer3 = a3;
            answer4 = a4;
            correctanswer = cA;
            questionID = qID;
            title = t;
            category = cat;
        }
    }
}
