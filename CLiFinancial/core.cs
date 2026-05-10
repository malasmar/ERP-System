using CLiCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLiFinancial
{
    public class core
    {
        public static decimal AccountBalance(string DB, Guid? Key, DateTime Date,int Type)
        {
            if (Key == null)
                return 0;
            decimal Res;
            Res = 0;
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                string delQuery = "SELECT TOP 100 PERCENT dbo.sfnFin_AccountBalance(@Key,@Date,@Type)";
                SqlCommand command = new SqlCommand();
                command = new SqlCommand();
                command.Connection = con;
                command.CommandType = CommandType.Text;
                command.CommandText = delQuery;
                command.Parameters.Clear();
                command.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                command.Parameters.Add("@Date", SqlDbType.Date).Value = Date;
                command.Parameters.Add("@Type", SqlDbType.Int).Value = Type;
                con.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Res = Convert.ToDecimal(reader[0]);
                }
                reader.Close();
            }
            return Res;
        }
        public static void DeleteVATTransactions(string DB, int Year)
        {
      

            using (SqlConnection conn = new SqlConnection(iCore.GetCon(DB)))
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                conn.Open();
                SqlCommand comm = conn.CreateCommand();
                SqlTransaction transaction;
                transaction = conn.BeginTransaction("Transaction");
                comm.Connection = conn;
                comm.Transaction = transaction;
                comm.CommandType = CommandType.Text;

                comm.CommandText = "DELETE v FROM vatDocument_Transaction v  WHERE DATEPART(YEAR,v.vat_Date)=@Year ";
                comm.Parameters.Clear();
                comm.Parameters.Add("@Year", SqlDbType.Int).Value = Year;
                comm.ExecuteNonQuery();
                try
                {
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }
    }
}
