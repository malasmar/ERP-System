using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiMobile.SelfService
{
    public class core
    {
        public static bool CheckUserDisable(Guid? Key)
        {
            bool result = false;
            string selQuery = "SELECT TOP 100 PERCENT x.ssu_Disable FROM px_SelfServiceUsers x WHERE x.ssu_Employee=@Key";
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
                    result =Convert.ToBoolean(reader[0]);
                };
                reader.Close();
            }
            return result;
        }
        public static decimal GetEmployeeAdvance(string DB,Guid? Key)
        {
            decimal result = 0;
            string selQuery = "SELECT TOP 100 PERCENT dbo.fnsAppSelfService_TotalAdvance(@Key) ";
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
                    result = Convert.ToDecimal(reader[0]);
                };
                reader.Close();
            }
            return result;
        }
        public static decimal GetEmployeeClosedAdvance(string DB,Guid? Key)
        {
            decimal result = 0;
            string selQuery = "SELECT TOP 100 PERCENT dbo.fnsAppSelfService_ClosedAdvance(@Key) ";
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
                    result = Convert.ToDecimal(reader[0]);
                };
                reader.Close();
            }
            return result;
        }

        public static decimal GetEmployeeLoan(string DB, Guid? Key)
        {
            decimal result = 0;
            string selQuery = "SELECT TOP 100 PERCENT dbo.fnsAppSelfService_TotalLoan(@Key) ";
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
                    result = Convert.ToDecimal(reader[0]);
                };
                reader.Close();
            }
            return result;
        }
        public static decimal GetEmployeeClosedLoan(string DB, Guid? Key)
        {
            decimal result = 0;
            string selQuery = "SELECT TOP 100 PERCENT dbo.fnsAppSelfService_ClosedLoan(@Key) ";
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
                    result = Convert.ToDecimal(reader[0]);
                };
                reader.Close();
            }
            return result;
        }
        public static int GetEmployeeNormalLeave(string DB, Guid? Key)
        {
            int result = 0;
            string selQuery = "SELECT TOP 100 PERCENT dbo.fnsAppSelfService_TotalLeave(@Key) ";
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
                    result = Convert.ToInt32(reader[0]);
                };
                reader.Close();
            }
            return result;
        }

        public static void UpdateEmployeeImage(string DB,Guid? Key, string ImageName)
        {
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder ScStr = new System.Text.StringBuilder();
                ScStr.Clear();
                ScStr.Append("UPDATE [hrCard_Employee] SET ");
                ScStr.Append(" [emp_Image] =@Image");
                ScStr.Append(" where [emp_Key]=@Key");
                SqlCommand ScCom = new SqlCommand();
                ScCom = new SqlCommand();
                ScCom.Connection = con;
                ScCom.CommandType = CommandType.Text;
                ScCom.CommandText = ScStr.ToString();
                ScCom.Parameters.Clear();
                ScCom.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                ScCom.Parameters.Add("@Image", SqlDbType.NVarChar, 500).Value = ImageName ?? "";
                con.Open();
                ScCom.ExecuteNonQuery();
            }
        }


        public static bool GetIfManager(string DB, Guid? Key)
        {
            int result = 0;
            string selQuery = "SELECT TOP 100 PERCENT COUNT(*) FROM HRStructure_Organizational s WHERE s.str_Manager=@Key AND s.str_Disable=0 ";
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
                    result = Convert.ToInt32(reader[0]);
                };
                reader.Close();
            }
            return result > 0 ? true : false;
        }
        public static int UpdateLogin(string DB,Guid? Key,Guid? Employee,Guid? Location)
        {
            DateTime date = DateTime.UtcNow.AddMinutes(180);
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("INSERT INTO AppSelfService_Attendance");
                str.Append("([att_Key]");
                str.Append(",[att_Employee]");
                str.Append(",[att_Date]");
                str.Append(",[att_Status]");
                str.Append(",[att_Login]");
                str.Append(",[att_Logout]");
                str.Append(",[att_Location]");
                str.Append(",[att_LogoutLocation]");
                str.Append(",[att_CostCenter]");
                str.Append(",[att_Project])");
                str.Append(" VALUES ");
                str.Append("(@att_Key");
                str.Append(",@att_Employee");
                str.Append(",@att_Date");
                str.Append(",@att_Status");
                str.Append(",@att_Login");
                str.Append(",@att_Logout");
                str.Append(",@att_Location");
                str.Append(",@att_LogoutLocation");
                str.Append(",@att_CostCenter");
                str.Append(",@att_Project)");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@att_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                comm.Parameters.Add("@att_Employee", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Employee);
                comm.Parameters.Add("@att_Date", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(date);
                comm.Parameters.Add("@att_Status", SqlDbType.Bit).Value = false;
                comm.Parameters.Add("@att_Login", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(date);
                comm.Parameters.Add("@att_Logout", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(null);
                comm.Parameters.Add("@att_Location", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Location);
                comm.Parameters.Add("@att_LogoutLocation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                comm.Parameters.Add("@att_CostCenter", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(GetLocationCostCenter(DB,Location));
                comm.Parameters.Add("@att_Project", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(GetLocationProject(DB,Location));
                con.Open();
             return   comm.ExecuteNonQuery();
            }
        }
        public static int UpdateLogout(string DB, Guid? Key, Guid? Location)
        {
            DateTime date = DateTime.UtcNow.AddMinutes(180);
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Clear();
                str.Append("Update AppSelfService_Attendance SET ");
                str.Append(" [att_Status]=@att_Status");
                str.Append(",[att_Logout]=@att_Logout");
                str.Append(",[att_LogoutLocation]=@att_LogoutLocation");
                str.Append(" WHERE att_Key=@att_Key");
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;
                comm.CommandType = CommandType.Text;
                comm.CommandText = str.ToString();
                comm.Parameters.Clear();
                comm.Parameters.Add("@att_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Key);
                comm.Parameters.Add("@att_Status", SqlDbType.Bit).Value = true;
                comm.Parameters.Add("@att_Logout", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(date);
                comm.Parameters.Add("@att_LogoutLocation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Location);
 
                con.Open();
               return comm.ExecuteNonQuery();
            }
        }
        public static Guid? GetLocationCostCenter(string DB, Guid? Location)
        {
            if (Location == null)
                return null;
            Guid? result = null;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "SELECT TOP 100 PERCENT [Loc_CostCenter] from [hrCard_Location] where [loc_Key]=@Key ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Location;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = iCore.IsDbNullRtNull(reader[0]);
                };
                reader.Close();
            }
            return result;
        }
        public static Guid? GetLocationProject(string DB, Guid? Location)
        {
            if (Location == null)
                return null;
            Guid? result = null;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string selQuery = "SELECT TOP 100 PERCENT [Loc_Project] from [hrCard_Location] where [loc_Key]=@Key ";
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Clear();
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Location;
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    result = iCore.IsDbNullRtNull(reader[0]);
                };
                reader.Close();
            }
            return result;
        }
    }
}
