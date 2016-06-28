using Arsha.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Arsha.Controllers
{
    public class AccountController : Controller
    {
        private MySqlConnection Connection = new MySqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["mysqldb"].ConnectionString);
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var user = db.Users.Where(m => m.UserName == username && m.Password == password).FirstOrDefault();
            if (user != null)
            {
                Session["IsAuth"] = true;
                return RedirectToAction("Index", "Home");
            }
            ViewBag.HasError = true;
            ViewBag.Message = "نام کاربری یا کلمه عبور اشتباه است.";
            return View();
        }

        public ActionResult Register()
        {
            return View("Login");
        }
        [HttpPost]
        public ActionResult Register(string username, string firstname, string lastname, string password,
            string mobile, string email, string discount)
        {

            db.Users.Add(new User
                {
                    UserId = Guid.NewGuid(),
                    UserName = username,
                    Password = password,
                    FirstName = firstname,
                    LastName = lastname,
                    Mobile = mobile,
                    Email = email,
                    Discount = discount
                });

            db.SaveChanges();

            string Query = "insert into account(username,firstname,lastname,password,mobile,email) values(@username,@firstname,@lastname,@password,@mobile,@email);";
            //This is command class which will handle the query and connection object.  
            MySqlCommand command = new MySqlCommand(Query, Connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@firstname", firstname);
            command.Parameters.AddWithValue("@lastname", lastname);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@mobile", mobile);
            command.Parameters.AddWithValue("@email", email);
            Connection.Open();
            int result = command.ExecuteNonQuery();     // Here our query will be executed and data saved into the database.  
            if (result <= 0)
            {
                ViewBag.Message = "امکان ساخت اکانت نمی باشد.";
            }
            Connection.Close();
            return View("Login");
        }

        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendPassword(string mobile)
        {
           
            var user = db.Users.Where(m => m.Mobile == mobile).FirstOrDefault();
            
            ir.smsapp.v2 ws = new ir.smsapp.v2();
            string smsUsername = System.Web.Configuration.WebConfigurationManager.AppSettings["SMS_UserName"];
            string smsPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["SMS_Password"];
            string[] senderNumbers = { System.Web.Configuration.WebConfigurationManager.AppSettings["SMS_Sender"] };
            string[] recipientNumbers = { user.Mobile };
            string bodyText = string.Format("گیم سرور آرشا - کلمه عبور: {0}", user.Password);
            string[] messageBodies = { bodyText };            
            
            long[] result = ws.SendSMS(smsUsername, smsPassword, senderNumbers, recipientNumbers, messageBodies, null, null, null);

            return View("SendPassword");
        }

       
    }

}