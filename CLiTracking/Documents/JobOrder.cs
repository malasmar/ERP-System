using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CLiCore;

namespace CLiTracking.Documents
{
    public class JobOrder
    {
        public int RecNo { get; set; }
        public Guid? OperationKey { get; set; }
        public int CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public int LastupUser { get; set; }
        public DateTime? LastupDate { get; set; }
        public int Status { get; set; }
        public int Branch { get; set; }
        public Guid? Prefix { get; set; }
        public int OrderNo { get; set; }
        public DateTime? OrderDate { get; set; }
        public string ReferenceNo { get; set; }
        public Guid? Client { get; set; }
        public int Vehicles { get; set; }
        public string Description { get; set; }
        public Guid? Invoice { get; set; }
        public Guid? Quotation { get; set; }
        public List<JobOrder> GetList(string DB,int Year)
        {
            DateTime First = new DateTime(Year, 1, 1);
            DateTime Last = new DateTime(Year, 12, 31);
            List<JobOrder> items = new List<JobOrder>();
            string selQuery = "select top 100 percent * from TrackingDocument_JobOrder where [job_OrderDate]<=@Last and [job_OrderDate]>=@First";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@First", SqlDbType.Date).Value = First;
                com.Parameters.Add("@Last", SqlDbType.Date).Value = Last;

                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    JobOrder item = new JobOrder();
                    item.RecNo = Convert.ToInt32(reader["job_RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["job_OperationKey"]);
                    item.CreateUser = Convert.ToInt32(reader["job_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["job_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["job_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["job_LastupDate"]);
                    item.Status = Convert.ToInt32(reader["job_Status"]);
                    item.Branch = Convert.ToInt32(reader["job_Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["job_Prefix"]);
                    item.OrderNo = Convert.ToInt32(reader["job_OrderNo"]);
                    item.OrderDate = iCore.IsDbNullRtNullDate(reader["job_OrderDate"]);
                    item.ReferenceNo = Convert.ToString(reader["job_ReferenceNo"]);
                    item.Client = iCore.IsDbNullRtNull(reader["job_Client"]);
                    item.Vehicles = Convert.ToInt32(reader["job_Vehicles"]);
                    item.Description = Convert.ToString(reader["job_Description"]);
                    item.Invoice = iCore.IsDbNullRtNull(reader["job_Invoice"]);
                    item.Quotation = iCore.IsDbNullRtNull(reader["job_Quotation"]);
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }

        public JobOrder GetItem(string DB,Guid? Key)
        {
            JobOrder item = new JobOrder();
            item.OrderDate = DateTime.UtcNow.AddMinutes(180);
            if (Key == null)
                return item;

            string selQuery = "select top 100 percent * from TrackingDocument_JobOrder where [job_OperationKey]=@Key";
            using (SqlConnection con = new SqlConnection(iCore.GetCon(DB)))
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.CommandText = selQuery;
                com.CommandType = CommandType.Text;
                com.Connection = con;
                com.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = Key;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item.RecNo = Convert.ToInt32(reader["job_RecNo"]);
                    item.OperationKey = iCore.IsDbNullRtNull(reader["job_OperationKey"]);
                    item.CreateUser = Convert.ToInt32(reader["job_CreateUser"]);
                    item.CreateDate = iCore.IsDbNullRtNullDate(reader["job_CreateDate"]);
                    item.LastupUser = Convert.ToInt32(reader["job_LastupUser"]);
                    item.LastupDate = iCore.IsDbNullRtNullDate(reader["job_LastupDate"]);
                    item.Status = Convert.ToInt32(reader["job_Status"]);
                    item.Branch = Convert.ToInt32(reader["job_Branch"]);
                    item.Prefix = iCore.IsDbNullRtNull(reader["job_Prefix"]);
                    item.OrderNo = Convert.ToInt32(reader["job_OrderNo"]);
                    item.OrderDate = iCore.IsDbNullRtNullDate(reader["job_OrderDate"]);
                    item.ReferenceNo = Convert.ToString(reader["job_ReferenceNo"]);
                    item.Client = iCore.IsDbNullRtNull(reader["job_Client"]);
                    item.Vehicles = Convert.ToInt32(reader["job_Vehicles"]);
                    item.Description = Convert.ToString(reader["job_Description"]);
                    item.Invoice = iCore.IsDbNullRtNull(reader["job_Invoice"]);
                    item.Quotation = iCore.IsDbNullRtNull(reader["job_Quotation"]);
                }
                reader.Close();
            }
            return item;
        }
    }
}
