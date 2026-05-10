using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiMobile.Sales.Platx
{
    public class core
    {
        public static bool CheckAppBlocked(Guid? Key)
        {
            bool result = false;
            string selQuery = "SELECT top 100 percent [app_Blocked] from [Application_SubscribeBlocked] where [app_Subscribe]=@Key and [app_Id]=@ID ";
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                com.Parameters.Add("@ID", SqlDbType.Int).Value = 0;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = (bool)reader[0];
                };
                reader.Close();
            }
            return result;
        }
        public static bool CheckUserIfDisable(Guid? Key)
        {
            bool result = false;
            string selQuery = "SELECT top 100 percent [user_Disable] from [px_Users] where [user_Key]=@Key ";
            using (SqlConnection con = new SqlConnection(iCore.Conn))
            {
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = (bool)reader[0];
                };
                reader.Close();
            }
            return result;
        }
        public static Guid? ClientCategoryParent(string DB,Guid? Key)
        {
            Guid? result=null;
            string selQuery = "SELECT top 100 percent [cat_Parent] from [AppSales_ClientCategories] where [cat_Key]=@Key ";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result =iCore.IsDbNullRtNull(reader[0]);
                };
                reader.Close();
            }
            return result;
        }
    }
}
