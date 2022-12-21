using LoginPageViaRepositoryPattern.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace LoginPageViaRepositoryPattern.Controllers
{

    public class HomeController : Controller
    {

        private readonly IUsers iu;

        public HomeController()
        {
            iu = new UsersRepository("Nothing");
        }
       
        public IActionResult Index()
        {
            return View();
        }
        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        public IActionResult Login(string email, string password)
        {
            password = EncodePasswordToBase64(password);    
            bool result = iu.Verify(email,password);

            if (result == true)
            {
                return RedirectToAction("Index", "New");
            }
            else
            {
                ViewBag.message = "Login Failed";
                return View("Index");
            }
        }

        public IActionResult Registration()
        {
            return View();
        }

        public IActionResult Register(Users u)
        {
            ValidationError v = new ValidationError();

            v = Validation(u);

            if (v.retval == true)
            {
                bool x = iu.Register(u);

                if (x == true)
                {
                    TempData["message"] = "User has been registered successfully.";
                    return RedirectToAction("Registration");
                }
                else
                {
                    ViewBag.message = "Unspecified Error in Data Insertion.";
                    return View("Registration", u);
                }
            }
            else
            {
                ViewBag.message = v.retmsg;
                return View("Registration", u);
            }
        }


        public IActionResult Add()
        {
            return View();
        }

        /*
        public IActionResult Add(Users u)
        {
            ValidationError v = new ValidationError();

            v = Validation(u);

            if (v.retval == true)
            {
                bool x = iu.User(u);

                if (x == true)
                {
                    TempData["message"] = "User details added successfully.";
                    return RedirectToAction("User");
                }
                else
                {
                    ViewBag.message = "Unspecified Error in Data Insertion.";
                    return View("User", u);
                }
            }
            else
            {
                ViewBag.message = v.retmsg;
                return View("User", u);
            }
        }
        */

        public ValidationError Validation(Users u)
        {
            ValidationError ve = new ValidationError();

            if (u.fullname == null || u.email == null || u.password == null || u.confirmpass == null)
            {
                ve.retval = false;
                ve.retmsg = "Input can no be blank.";

                return ve;
            }
            else if (iu.FindDuplicate(u.email) == false)
            {
                ve.retval = false;
                ve.retmsg = "You have already registered with this email.";

                return ve;
            }
            else if (u.password.Length < 4 || u.password.Length > 8)
            {
                ve.retval = false;
                ve.retmsg = "Password must be more than 4 and less than 8 characters.";

                return ve;
            }
            else if (!u.password.Equals(u.confirmpass))
            {
                ve.retval = false;
                ve.retmsg = "Password and Confirm Password are not equal.";

                return ve;
            }
            else
            {
                ve.retval = true;
                ve.retmsg = null;

                return ve;
            }
        }
    }
}
