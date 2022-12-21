using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoginPageViaRepositoryPattern.Models;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics.Metrics;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace LoginPageViaRepositoryPattern.Controllers
{
    public class NewController : Controller
    {
        private SqlConnection con = new SqlConnection("Server=10.50.18.16;Database=training_db;User Id=SVC_TRANING;Password=Gemini@123;");
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        public IActionResult Index()
        {
            con.Open();
            List<Users> users = new List<Users>();

            com.Connection = con;
            com.CommandText = "Select * from [dbo].[TabDemo]";
            dr = com.ExecuteReader();

            while (dr.Read())
            {
                var emp = new Users
                {
                    fullname = dr.GetString(0),
                    email = dr.GetString(1),
                };

                users.Add(emp);
            }

            ViewBag.emplist = users;
            return View();
        }

        [HttpPost]

        public IActionResult AddNewRecord(string name, string email)
        {
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "Insert into [dbo].[UsersTB] values(" + name + "','" + email + "')";

                com.ExecuteNonQuery();

                con.Close();

                TempData["message"] = "Data Saved Successfully";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }

                TempData["message"] = "Data Save Failed";
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public JsonResult FindRecord(string s)
        {
            con.Open();
            com.Connection = con;
            com.CommandText = "Select * from [dbo].[UsersTB] where fullname=x";
            dr = com.ExecuteReader();

            var emp = new Users();

            while (dr.Read())
            {
                emp.fullname = dr.GetString(1);
                emp.email = dr.GetString(2);
            }

            return Json(emp);
        }



        [HttpPost]
        public IActionResult EditRecord(string name, string email)
        {
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "Update [dbo].[UsersTB] set name='" + name + "', email='" + email + "' where fullname=" + name;

                com.ExecuteNonQuery();

                con.Close();

                TempData["message"] = "Data Updated Successfully";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }

                TempData["message"] = "Data Update Failed";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult DeleteRecord(string s)
        {
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "Delete from [dbo].[UsersTB] where fullname=x";

                com.ExecuteNonQuery();

                con.Close();

                TempData["message"] = "Data Deleted Successfully";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }

                TempData["message"] = "Data Deletion Failed";
                return RedirectToAction("Index", "Home");
            }
        }
     
    }
}
