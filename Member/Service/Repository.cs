using Member.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Member.Service
{
    public class Repository
    {
        string connectionstring = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        public string md5_string(string password)
        {
            string md5_password = string.Empty;
            using (MD5 hash = MD5.Create())
            {
                md5_password = string.Join("", hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("x2")));
            }

            return md5_password;
        }
        public async Task<ResponseModel<string>> login(Login user)
        {
            ResponseModel<string> response = new ResponseModel<string>();

            if (user != null)
            {
                string md5_password = md5_string(user.password);

                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = string.Format("Select * FROM MUser WHERE Password = '{0}' and Email='{1}'", md5_password, user.email);
                    cmd.Connection = conn;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    await Task.Run(() => da.Fill(dt));

                    if (dt.Rows.Count > 0)
                    {
                        response.Data = JsonConvert.SerializeObject(dt);
                        response.resultCode = 200;
                    }
                    else
                    {
                        response.message = "User Not Found!";
                        response.resultCode = 500;
                    }


                }
            }
            return response;
        }

        public async Task<ResponseModel<string>> Register(Login user)
        {
            ResponseModel<string> response = new ResponseModel<string>();

            if (user != null)
            {
                string md5_password = md5_string(user.password);



                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = string.Format("Insert INTO MUser(Email,Password) VALUES('{0}','{1}')", user.email, md5_password);
                    cmd.Connection = conn;

                    conn.Open();
                    var result = await cmd.ExecuteNonQueryAsync();
                    conn.Close();
                    if (result == 1) //row changes in the database - successfull
                    {
                        response.message = "User has been registered!";
                        response.resultCode = 200;
                    }
                    else
                    {
                        response.message = "Unable to register User!";
                        response.resultCode = 500;
                    }

                }
            }
            return response;
        }
    }
}