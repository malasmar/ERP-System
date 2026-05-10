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
    public class core
    {
        private readonly static object Locker = new object();
        public static CLiCore.OperationResult UpdateJobOrder(string DB, Documents.JobOrder Header, List<Documents.Vehicles> Details, bool IsNew)
        {
            lock (Locker)
            {
                Guid? opk;
                int VoucherNo;

                if (IsNew == false)
                {
                    opk = Header.OperationKey;
                    VoucherNo = Header.OrderNo;
                   
                }
                else
                {
                    opk = Guid.NewGuid();
                    VoucherNo = 1;// xcore.MaxTransaction(DB, Header.DocumentKind, Header.InvoiceDate.Value.Year);
                }

           
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

                    comm.CommandText = " delete from TrackingDocument_JobOrder where [job_OperationKey]=@key ";
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@key", SqlDbType.UniqueIdentifier).Value = opk;
                    comm.ExecuteNonQuery();

                    //Header Data
                    str.Clear();
                    str.Append("INSERT INTO TrackingDocument_JobOrder");
                    str.Append("([job_OperationKey]");
                    str.Append(",[job_CreateUser]");
                    str.Append(",[job_CreateDate]");
                    str.Append(",[job_LastupUser]");
                    str.Append(",[job_LastupDate]");
                    str.Append(",[job_Status]");
                    str.Append(",[job_Branch]");
                    str.Append(",[job_Prefix]");
                    str.Append(",[job_OrderNo]");
                    str.Append(",[job_OrderDate]");
                    str.Append(",[job_ReferenceNo]");
                    str.Append(",[job_Client]");
                    str.Append(",[job_Vehicles]");
                    str.Append(",[job_Description]");
                    str.Append(",[job_Invoice]");
                    str.Append(",[job_Quotation])");
                    str.Append(" VALUES ");
                    str.Append("(@job_OperationKey");
                    str.Append(",@job_CreateUser");
                    str.Append(",@job_CreateDate");
                    str.Append(",@job_LastupUser");
                    str.Append(",@job_LastupDate");
                    str.Append(",@job_Status");
                    str.Append(",@job_Branch");
                    str.Append(",@job_Prefix");
                    str.Append(",@job_OrderNo");
                    str.Append(",@job_OrderDate");
                    str.Append(",@job_ReferenceNo");
                    str.Append(",@job_Client");
                    str.Append(",@job_Vehicles");
                    str.Append(",@job_Description");
                    str.Append(",@job_Invoice");
                    str.Append(",@job_Quotation)");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@job_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@job_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@job_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@job_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@job_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@job_Status", SqlDbType.Int).Value = Header.Status;
                    comm.Parameters.Add("@job_Branch", SqlDbType.Int).Value = Header.Branch;
                    comm.Parameters.Add("@job_Prefix", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Prefix);
                    comm.Parameters.Add("@job_OrderNo", SqlDbType.Int).Value = Header.OrderNo;
                    comm.Parameters.Add("@job_OrderDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.OrderDate);
                    comm.Parameters.Add("@job_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@job_Client", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Client);
                    comm.Parameters.Add("@job_Vehicles", SqlDbType.Int).Value = Details.Count();
                    comm.Parameters.Add("@job_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@job_Invoice", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Invoice);
                    comm.Parameters.Add("@job_Quotation", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Quotation);
                    comm.ExecuteNonQuery();

                    foreach (Vehicles item in Details)
                    {
                        str.Clear();
                        str.Append("INSERT INTO TrackingDocument_JobOrderVehicles ");
                        str.Append("([veh_OperationKey]");
                        str.Append(",[veh_Key]");
                        str.Append(",[veh_Plate]");
                        str.Append(",[veh_Arabic]");
                        str.Append(",[veh_City]");
                        str.Append(",[veh_Person]");
                        str.Append(",[veh_Phone]");
                        str.Append(",[veh_Technician]");
                        str.Append(",[veh_InstallDate]");
                        str.Append(",[veh_Status])");
                        str.Append(" VALUES ");
                        str.Append("(@veh_OperationKey");
                        str.Append(",@veh_Key");
                        str.Append(",@veh_Plate");
                        str.Append(",@veh_Arabic");
                        str.Append(",@veh_City");
                        str.Append(",@veh_Person");
                        str.Append(",@veh_Phone");
                        str.Append(",@veh_Technician");
                        str.Append(",@veh_InstallDate");
                        str.Append(",@veh_Status)");
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = str.ToString();
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@veh_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@veh_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                        comm.Parameters.Add("@veh_Plate", SqlDbType.NVarChar, 10).Value = item.Plate ?? "";
                        comm.Parameters.Add("@veh_Arabic", SqlDbType.NVarChar, 10).Value = item.Arabic ?? "";
                        comm.Parameters.Add("@veh_City", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.City);
                        comm.Parameters.Add("@veh_Person", SqlDbType.NVarChar, 100).Value = item.Person ?? "";
                        comm.Parameters.Add("@veh_Phone", SqlDbType.NVarChar, 15).Value = item.Phone ?? "";
                        comm.Parameters.Add("@veh_Technician", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Technician);
                        comm.Parameters.Add("@veh_InstallDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(null);
                        comm.Parameters.Add("@veh_Status", SqlDbType.Int).Value = item.Status;
                        comm.ExecuteNonQuery();

                        for(int i = 0; i <=5; i++)
                        {
                            Guid? itemKey;
                            itemKey = null;
                            switch (i)
                            {
                                case 0:
                                    itemKey = item.Device1;
                                    break;
                                case 1:
                                    itemKey = item.Device2;
                                    break;
                                case 2:
                                    itemKey = item.Device3;
                                    break;
                                case 3:
                                    itemKey = item.Device4;
                                    break;
                                case 4:
                                    itemKey = item.Device5;
                                    break;
                                case 5:
                                    itemKey = item.Device6;
                                    break;
                            }
                            if (itemKey != null)
                            {
                                str.Clear();
                                str.Append("INSERT INTO TrackingDocument_JobOrderVehiclesDevices");
                                str.Append("([dev_OperationKey]");
                                str.Append(",[dev_VehicleKey]");
                                str.Append(",[dev_Key]");
                                str.Append(",[dev_Item]");
                                str.Append(",[dev_SerialNo]");
                                str.Append(",[dev_SerialKey]");
                                str.Append(",[dev_Status])");
                                str.Append(" VALUES ");
                                str.Append("(@dev_OperationKey");
                                str.Append(",@dev_VehicleKey");
                                str.Append(",@dev_Key");
                                str.Append(",@dev_Item");
                                str.Append(",@dev_SerialNo");
                                str.Append(",@dev_SerialKey");
                                str.Append(",@dev_Status)");
                                comm.CommandType = CommandType.Text;
                                comm.CommandText = str.ToString();
                                comm.Parameters.Clear();
                                comm.Parameters.Add("@dev_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                                comm.Parameters.Add("@dev_VehicleKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.Key);
                                comm.Parameters.Add("@dev_Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Guid.NewGuid());
                                comm.Parameters.Add("@dev_Item", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(itemKey);
                                comm.Parameters.Add("@dev_SerialNo", SqlDbType.NVarChar, 100).Value = "";
                                comm.Parameters.Add("@dev_SerialKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(null);
                                comm.Parameters.Add("@dev_Status", SqlDbType.Int).Value = item.Status;
                                comm.ExecuteNonQuery();
                            }

                        }
                    }

                    try
                    {
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }

                OperationResult res = new OperationResult();
                res.OperationKey = opk;
                res.VoucherNo = VoucherNo;
                return res;
            }
        }
        public static CLiCore.OperationResult UpdateCreationDevices(string DB, Operation.DeviceCreation Header, List<Operation.Devices> Details)
        {
            lock (Locker)
            {
                Guid? opk;
            opk=Guid.NewGuid();
 
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
                     
                    //Header Data
                    str.Clear();
                    str.Append("INSERT INTO TrackingOperation_DeviceCreation");
                    str.Append("([crt_OperationKey]");
                    str.Append(",[crt_Item]");
                    str.Append(",[crt_CreateUser]");
                    str.Append(",[crt_CreateDate]");
                    str.Append(",[crt_LastupUser]");
                    str.Append(",[crt_LastupDate]");
                    str.Append(",[crt_SourceWarehouse]");
                    str.Append(",[crt_OrderNo]");
                    str.Append(",[crt_OrderDate]");
                    str.Append(",[crt_ReferenceNo]");
                    str.Append(",[crt_Description])");
                    str.Append(" VALUES ");
                    str.Append("(@crt_OperationKey");
                    str.Append(",@crt_Item");
                    str.Append(",@crt_CreateUser");
                    str.Append(",@crt_CreateDate");
                    str.Append(",@crt_LastupUser");
                    str.Append(",@crt_LastupDate");
                    str.Append(",@crt_SourceWarehouse");
                    str.Append(",@crt_OrderNo");
                    str.Append(",@crt_OrderDate");
                    str.Append(",@crt_ReferenceNo");
                    str.Append(",@crt_Description)");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@crt_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@crt_Item", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Item);
                    comm.Parameters.Add("@crt_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@crt_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@crt_LastupUser", SqlDbType.Int).Value = Header.LastupUser;
                    comm.Parameters.Add("@crt_LastupDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.LastupDate);
                    comm.Parameters.Add("@crt_SourceWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                    comm.Parameters.Add("@crt_OrderNo", SqlDbType.Int).Value = Header.OrderNo;
                    comm.Parameters.Add("@crt_OrderDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.OrderDate);
                    comm.Parameters.Add("@crt_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@crt_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.ExecuteNonQuery();

                    foreach (Operation.Devices item in Details)
                    {
                        str.Clear();
                        str.Append("INSERT INTO TrackingOperation_Devices");
                        str.Append("([dev_OperationKey]");
                        str.Append(",[dev_Item]");
                        str.Append(",[dev_SerialNo]");
                        str.Append(",[dev_Sim]");
                        str.Append(",[dev_SimSerial]");
                        str.Append(",[dev_Status])");
                        str.Append(" VALUES ");
                        str.Append("(@dev_OperationKey");
                        str.Append(",@dev_Item");
                        str.Append(",@dev_SerialNo");
                        str.Append(",@dev_Sim");
                        str.Append(",@dev_SimSerial");
                        str.Append(",@dev_Status)");
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = str.ToString();
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@dev_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@dev_Item", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Item);
                        comm.Parameters.Add("@dev_SerialNo", SqlDbType.NVarChar, 100).Value = item.Serial ?? "";
                        comm.Parameters.Add("@dev_Sim", SqlDbType.NVarChar, 25).Value = item.Sim ?? "";
                        comm.Parameters.Add("@dev_SimSerial", SqlDbType.NVarChar, 25).Value = item.SimSerial ?? "";
                        comm.Parameters.Add("@dev_Status", SqlDbType.Int).Value = false;
                        comm.ExecuteNonQuery();
                    }

                    try
                    {
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }

                OperationResult res = new OperationResult();
                res.OperationKey = opk;
                res.VoucherNo = 0;
                return res;
            }
        }
        public static CLiCore.OperationResult UpdateTechnicalDelivery(string DB, Operation.TechnicalDelivery Header, List<Operation.TechnicalDeliveryDetails> Details)
        {
            lock (Locker)
            {
                Guid? opk;
                opk = Guid.NewGuid();

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

                    //Header Data
                    str.Clear();
                    str.Append("INSERT INTO TrackingOperation_TechnicalDelivery");
                    str.Append("([tdd_OperationKey]");
                    str.Append(",[tdd_CreateUser]");
                    str.Append(",[tdd_CreateDate]");
                    str.Append(",[tdd_SourceWarehouse]");
                    str.Append(",[tdd_Technical]");
                    str.Append(",[tdd_No]");
                    str.Append(",[tdd_Date]");
                    str.Append(",[tdd_ReferenceNo]");
                    str.Append(",[tdd_Comment]");
                    str.Append(",[tdd_Description]");
                    str.Append(",[tdd_Status])");
                    str.Append(" VALUES ");
                    str.Append("(@tdd_OperationKey");
                    str.Append(",@tdd_CreateUser");
                    str.Append(",@tdd_CreateDate");
                    str.Append(",@tdd_SourceWarehouse");
                    str.Append(",@tdd_Technical");
                    str.Append(",@tdd_No");
                    str.Append(",@tdd_Date");
                    str.Append(",@tdd_ReferenceNo");
                    str.Append(",@tdd_Comment");
                    str.Append(",@tdd_Description");
                    str.Append(",@tdd_Status)");
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = str.ToString();
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@tdd_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                    comm.Parameters.Add("@tdd_CreateUser", SqlDbType.Int).Value = Header.CreateUser;
                    comm.Parameters.Add("@tdd_CreateDate", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.CreateDate);
                    comm.Parameters.Add("@tdd_SourceWarehouse", SqlDbType.Int).Value = Header.SourceWarehouse;
                    comm.Parameters.Add("@tdd_Technical", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(Header.Technical);
                    comm.Parameters.Add("@tdd_No", SqlDbType.Int).Value = Header.No;
                    comm.Parameters.Add("@tdd_Date", SqlDbType.DateTime).Value = iCore.IsNullRtDbNull(Header.Date);
                    comm.Parameters.Add("@tdd_ReferenceNo", SqlDbType.NVarChar, 25).Value = Header.ReferenceNo ?? "";
                    comm.Parameters.Add("@tdd_Comment", SqlDbType.NVarChar, 500).Value = Header.Comment ?? "";
                    comm.Parameters.Add("@tdd_Description", SqlDbType.NVarChar, 500).Value = Header.Description ?? "";
                    comm.Parameters.Add("@tdd_Status", SqlDbType.Bit).Value = Header.Status;
                    comm.ExecuteNonQuery();

                    foreach (Operation.TechnicalDeliveryDetails item in Details)
                    {
                        str.Clear();
                        str.Append("INSERT INTO TrackingOperation_TechnicalDeliveryDetails");
                        str.Append("([dev_OperationKey]");
                        str.Append(",[dev_Item]");
                        str.Append(",[dev_SerialNo]");
                        str.Append(",[dev_Sim]");
                        str.Append(",[dev_SimSerial]");
                        str.Append(",[dev_Status])");
                        str.Append(" VALUES ");
                        str.Append("(@dev_OperationKey");
                        str.Append(",@dev_Item");
                        str.Append(",@dev_SerialNo");
                        str.Append(",@dev_Sim");
                        str.Append(",@dev_SimSerial");
                        str.Append(",@dev_Status)");
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = str.ToString();
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@dev_OperationKey", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(opk);
                        comm.Parameters.Add("@dev_Item", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.ItemKey);
                        comm.Parameters.Add("@dev_SerialNo", SqlDbType.NVarChar, 100).Value = item.Serial ?? "";
                        comm.Parameters.Add("@dev_Sim", SqlDbType.NVarChar, 25).Value = item.Sim ?? "";
                        comm.Parameters.Add("@dev_SimSerial", SqlDbType.NVarChar, 25).Value = item.SimSerial ?? "";
                        comm.Parameters.Add("@dev_Status", SqlDbType.Bit).Value = false;
                        comm.ExecuteNonQuery();

                        str.Clear();
                        str.Append("UPDATE[TrackingOperation_Devices] SET dev_Status=1 where dev_Key=@Key");
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = str.ToString();
                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Key", SqlDbType.UniqueIdentifier).Value = iCore.IsNullRtDbNull(item.SourceKey);
                        comm.ExecuteNonQuery();

                    }

                    try
                    {
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }

                OperationResult res = new OperationResult();
                res.OperationKey = opk;
                res.VoucherNo = 0;
                return res;
            }
        }
    }
}
