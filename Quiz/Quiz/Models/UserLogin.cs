using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiz.Models
{
    public class UserLogin
    {

        public String username { get; set; }
        public String password { get; set; }

        public String firstName { get; set; }
        public String lastName { get; set; }

        public UserLogin(String uname, String pw)
        {
            username = uname;
            password = pw;
        }

        public UserLogin(String uname, String pw, String fname, String lname)
        {
            username = uname;
            password = pw;
            firstName = fname;
            lastName = lname;
        }

        public UserLogin()
        {

        }





    }
}