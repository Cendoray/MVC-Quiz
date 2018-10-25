using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiz.Models
{



    public class QuizQuestions
    {
        public String  categoryID { get; set; }
        public String  username { get; set; }
        public List<String>  questionID { get; set; }
        public List<String>  title { get; set; }
        public List<String>  userAnswer{ get; set; }
        public List<String> correctAnswer { get; set; }
        public int questionPosition { get; set; }
        public List<String> answer1 { get; set; }
        public List<String> answer2 { get; set; }
        public List<String> answer3 { get; set; }
        public List<String> answer4 { get; set; }



        public QuizQuestions(List<Question> item)
        {
            questionID = new List<String>();
            title = new List<String>();
            userAnswer = new List<String>();
            correctAnswer = new List<String>();
            answer1 = new List<String>();
            answer2 = new List<String>();
            answer3 = new List<String>();
            answer4 = new List<String>();
            questionPosition = 0;
            username = item[0].username;
            categoryID = item[0].category_ID;
            using (var db = new QuizEntities())
            {
                for(int i = 0; i < item.Count; i ++) {
                    questionID.Add(item[i].question_ID);
                    title.Add(item[i].question_title);
                    userAnswer.Add(item[i].answer1);
                    correctAnswer.Add(item[i].correct_answer);
                    answer1.Add(item[i].answer1);
                    answer2.Add(item[i].answer2);
                    answer3.Add(item[i].answer3);
                    answer4.Add(item[i].answer4);
                }
            }
        }
    }


}