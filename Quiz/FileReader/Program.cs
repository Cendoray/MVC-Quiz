using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FileReader
{
    class Program    
    {
        static void Main(string[] args)
        {

            XElement questionsXml = XDocument.Load("./xml/questions.xml").Root;
            XElement usersXml = XDocument.Load("./xml/users.xml").Root;

            using (QuizEntities con = new QuizEntities())
            {
                // Gets all users from the users.xml file
                var users = from user in usersXml.Descendants("user")
                            select new
                            {
                                firstname = user.Attribute("firstName").Value,
                                lastname = user.Attribute("lastName").Value,
                                username = user.Attribute("userId").Value
                            };

                // Adds all the users to the database User table
                foreach (var u in users)
                {
                    User newUser = new User()
                    {
                        firstname = u.firstname,
                        lastname = u.lastname,
                        username = u.username,
                        password = u.username
                    };

                    con.Users.Add(newUser);
                }

                // Gets all questions (and existing categories) from the questions.xml file
                var questions = from question in questionsXml.Descendants("question")
                                let options = question.Elements("option").ToList()
                                select new
                                {
                                    User = question.Element("user").Value,
                                    Category = question.Element("category").Value,
                                    Title = question.Element("title").Value,
                                    Option1 = options[0].Value,
                                    Option2 = options[1].Value,
                                    Option3 = options[2].Value,
                                    Option4 = options[3].Value,
                                    Correct = options.Where(x => x.Attributes().Count() > 1).Select(x => x.Value).FirstOrDefault()
                                };

                int categoryId = 1;

                // Loop that populates Category table
                foreach (var q in questions)
                {
                    var checkCategory = con.Categories.Where(x => x.title.CompareTo(q.Category.Trim()) == 0);

                    // If the category of current question isn't already defined in the Category table, it gets added
                    if (checkCategory.FirstOrDefault() == null)
                    {
                        Console.WriteLine("===== Catgeory " + q.Category + " doesn't exist =====");
                        Console.WriteLine(categoryId.ToString() + "\t" + q.Category.Trim());
                        try
                        {
                            Category category = new Category()
                            {
                                category_ID = categoryId.ToString(),
                                title = q.Category.Trim().ToString()
                            };
                            con.Categories.Add(category);
                            con.SaveChanges();
                            Console.WriteLine("===== Created the " + q.Category.Trim() + " category =====");
                            categoryId++;
                        }
                        catch (System.Data.Entity.Validation.DbEntityValidationException e)
                        {
                            foreach (var eve in e.EntityValidationErrors)
                            {
                                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                foreach (var ve in eve.ValidationErrors)
                                {
                                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                        ve.PropertyName, ve.ErrorMessage);
                                }
                            }
                            throw;
                        }
                    }
                }

                int questionId = 1;

                // Loop for Question Table
                foreach (var q in questions)
                {
                    Question question;

                    var category = con.Categories.Where(x => x.title.CompareTo(q.Category.Trim()) == 0);

                    try
                    {
                        question = new Question()
                        {
                            question_ID = "" + questionId,
                            category_ID = category.First().category_ID,
                            question_title = q.Title.Trim(),
                            username = q.User.Trim(),
                            answer1 = q.Option1.Trim(),
                            answer2 = q.Option2.Trim(),
                            answer3 = q.Option3.Trim(),
                            answer4 = q.Option4.Trim(),
                            correct_answer = q.Correct.Trim()
                        };

                        con.Questions.Add(question);
                        con.SaveChanges();
                        questionId++;
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }

                }

                con.SaveChanges();

            } // end of using
        }
    }
}
