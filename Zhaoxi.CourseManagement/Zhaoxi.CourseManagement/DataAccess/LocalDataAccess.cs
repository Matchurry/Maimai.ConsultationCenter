using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Annotations;
using Zhaoxi.CourseManagement.Common;
using Zhaoxi.CourseManagement.DataAccess.DataEntity;

namespace Zhaoxi.CourseManagement.DataAccess
{
    public class LocalDataAccess
    {
        private static LocalDataAccess instance;
        private LocalDataAccess() { }
        public static LocalDataAccess GetInstance()
        {
            return instance??(instance=new LocalDataAccess());
        }
        MySqlConnection conn;
        MySqlCommand comm;
        MySqlDataAdapter adapter;
        private void Dispose()
        {
            if (adapter != null)
            {
                adapter.Dispose();
                adapter = null;
            }
            if (comm != null)
            {
                comm.Dispose();
                comm = null;
            }
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
                conn = null;
            }
        }
        private bool DBConnection()
        {
            string connStr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
            if (conn == null) 
                conn = new MySqlConnection("Server=localhost;Database=zx_data;User Id=root;Password=123456;");
            try
            {
                conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public UserEntity CheckUserInfo(string userName, string pwd)
        {
            try { 
                if (DBConnection())
                {
                    //Console.WriteLine("登陆成功");
                    string userSql = "select * from users where user_name=@user_name and password=@pwd and is_validation=1";
                    //string userSql = "select * from users";
                    comm = new MySqlCommand(userSql,conn);
                    comm.Parameters.AddWithValue("user_name", userName);
                    comm.Parameters.AddWithValue("pwd", MD5Provider.GetMD5STring(pwd + "@" + userName));
                    MySqlDataReader rd = comm.ExecuteReader();
                    

                    if (!rd.Read())
                        throw new Exception("用户名或密码不正确");
                    if (rd[5].ToString()=="0")
                        throw new Exception("当前用户没有权限使用此平台！");

                    UserEntity userInfo = new UserEntity();
                    userInfo.UserName = rd[1].ToString();
                    userInfo.RealName = rd[2].ToString();
                    userInfo.Password = rd[3].ToString();
                    userInfo.Avatar = rd[7].ToString();
                    userInfo.Gender = rd[8].ToString()[0] - '0';
                    //Console.WriteLine(userInfo.UserName);
                    return userInfo;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                throw ex;
            }
            finally
            {
                this.Dispose();

            }
            return null;
        }
    }
}
