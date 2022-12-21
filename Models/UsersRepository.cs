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

namespace LoginPageViaRepositoryPattern.Models
{
    public class UsersRepository : IUsers
    {
        private SqlConnection con = new SqlConnection("Server=10.50.18.16;Database=training_db;User Id=SVC_TRANING;Password=Gemini@123;");
        private SqlCommand com = new SqlCommand();
        private SqlDataReader dr;

        private string str;

        public UsersRepository(string str)
        {
            this.str = str;
        }

        public bool FindDuplicate(string email)
        {
            con.Open();
            com.Connection = con;
            com.CommandText = "Select * From [dbo].TabDemo where email='" + email + "'";
            dr = com.ExecuteReader();

            List<Users> u = new List<Users>();

            while (dr.Read())
            {
                var x = new Users
                {
                    fullname = dr.GetString(0),
                    email = dr.GetString(1),
                    password = dr.GetString(3)
                };

                u.Add(x);
            }

            con.Close();

            if (u.Count >= 1)
            {
                return false;
            }
            else
            {
                return true;
            }
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

        public string DecodeFrom64(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }

        public bool Register(Users u)
        {

            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "Insert into [dbo].[TabDemo] values ('" + u.fullname + "','" + u.email + "','" + EncodePasswordToBase64(u.password) + "')";
                com.ExecuteNonQuery();
                con.Close();

                return true;
            }
            catch (Exception)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                return false;
            }
        }

        public bool User(Users u)
        {

            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "Insert into [dbo].[TabDemo] values ('" + u.fullname + "','" + u.email + "','" + u.password + "')";
                com.ExecuteNonQuery();
                con.Close();

                return true;
            }
            catch (Exception)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                return false;
            }
        }
        public bool Verify(string email, string password)
        {
            con.Open();
            com.Connection = con;
             com.CommandText = "Select * from [dbo].[TabDemo] where email='" + email + "' and password='" + password+ "'";
            dr = com.ExecuteReader();

            List<Users> u = new List<Users>();

            while (dr.Read())
            {
                var x = new Users()
                {
                    fullname = dr.GetString(0),
                    email = dr.GetString(1),
                    password = dr.GetString(2),
                    confirmpass = null
                };

                u.Add(x);
            }

            con.Close();

            if (u.Count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
