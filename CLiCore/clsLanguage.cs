using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data;
using System.Text;


namespace CLiCore
{
    public class clsLanguage
    {
        public int LanID { get; set; }

        [Display(Name = "Code  ")]
        public string LanCode { get; set; }

        [Required]
        [Display(Name = " Arabic")]
        public string LanArabic { get; set; }



        [Display(Name = "Russian")]
        public string LanRussian { get; set; }



        [Display(Name = "French")]
        public string LanFrench { get; set; }


        [Display(Name = "Indian ")]
        public string LanIndian { get; set; }


        [Required]
        [Display(Name = "English")]
        public string LanEnglish { get; set; }



        [Display(Name = "Turkish")]
        public string LanTurkish { get; set; }

        [Display(Name = "Key ")]
        public string LanKey { get; set; }

        public string Display { get; set; }


        public int Key { get; set; }


        public clsLanguage()
        {
            LanID = 0;
            LanCode = "";
            LanKey = "";
            LanArabic = "";
            LanEnglish = "";
            LanTurkish = "";
            LanIndian = "";
            LanFrench = "";
            LanRussian = "";
        }


        public static clsLanguage GetItem(string DB, int LanID)
        {
            clsLanguage item = new clsLanguage();

            try
            {
                string selQuery = "GetLanguage";
                using (SqlConnection con = new SqlConnection(iCore.Conn))
                {
                    con.Open();
                    SqlCommand com = new SqlCommand();
                    com.CommandText = selQuery;
                    com.CommandType = CommandType.StoredProcedure;
                    com.Connection = con;
                    com.Parameters.Add("@LanID", SqlDbType.Int).Value = LanID;
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        //LanID	LanKey	LanCode	LanArabic	LanEnglish	LanTurkish	LanIndian	LanFrench	LanRussian

                        item.LanID = clsGeneral.IsDbNullRtZero(reader["LanID"]);
                        item.LanKey = clsGeneral.IsDbNullRtEmpty(reader["LanKey"]);
                        item.LanCode = clsGeneral.IsDbNullRtEmpty(reader["LanCode"]);

                        item.LanArabic = clsGeneral.IsDbNullRtEmpty(reader["LanArabic"]);
                        item.LanEnglish = clsGeneral.IsDbNullRtEmpty(reader["LanEnglish"]);
                        item.LanTurkish = clsGeneral.IsDbNullRtEmpty(reader["LanTurkish"]);
                        item.LanIndian = clsGeneral.IsDbNullRtEmpty(reader["LanIndian"]);
                        item.LanFrench = clsGeneral.IsDbNullRtEmpty(reader["LanFrench"]);
                        item.LanRussian = clsGeneral.IsDbNullRtEmpty(reader["LanRussian"]);

                        item.Key = clsGeneral.IsDbNullRtZero(reader["LanID"]);
                        item.Display = item.LanArabic;
                    }
                    reader.Close();
                }
            }
            catch (SqlException ex)
            {
                string msg = ex.Message;

            }
            return item;
        }


        public static List<clsLanguage> GetList(string DB)
        {
            List<clsLanguage> SHLL = new List<clsLanguage>();
            try
            {
                string selQuery = "GetLanguage";
                using (SqlConnection con = new SqlConnection(iCore.Conn))
                {
                    con.Open();
                    SqlCommand com = new SqlCommand();
                    com.CommandText = selQuery;
                    com.CommandType = CommandType.StoredProcedure;
                    com.Connection = con;
                    com.Parameters.Add("@LanID", SqlDbType.Int).Value = 0;
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        clsLanguage item = new clsLanguage();
                        //LanID	LanKey	LanCode	LanArabic	LanEnglish	LanTurkish	LanIndian	LanFrench	LanRussian

                        item.LanID = clsGeneral.IsDbNullRtZero(reader["LanID"]);
                        item.LanKey = clsGeneral.IsDbNullRtEmpty(reader["LanKey"]);
                        item.LanCode = clsGeneral.IsDbNullRtEmpty(reader["LanCode"]);

                        item.LanArabic = clsGeneral.IsDbNullRtEmpty(reader["LanArabic"]);
                        item.LanEnglish = clsGeneral.IsDbNullRtEmpty(reader["LanEnglish"]);
                        item.LanTurkish = clsGeneral.IsDbNullRtEmpty(reader["LanTurkish"]);
                        item.LanIndian = clsGeneral.IsDbNullRtEmpty(reader["LanIndian"]);
                        item.LanFrench = clsGeneral.IsDbNullRtEmpty(reader["LanFrench"]);
                        item.LanRussian = clsGeneral.IsDbNullRtEmpty(reader["LanRussian"]);

                        item.Key = clsGeneral.IsDbNullRtZero(reader["LanID"]);
                        item.Display = item.LanArabic;

                        SHLL.Add(item);
                    }
                    reader.Close();
                }

            }
            catch (SqlException ex)
            {
                string msg = ex.Message;

            }

            return SHLL;
        }


        public static int Insert(string DB, clsLanguage objEnt)
        {
            int newId = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(iCore.Conn))
                {
                    System.Text.StringBuilder ScStr = new System.Text.StringBuilder();
                    ScStr.Clear();
                    ScStr.Append("InsertLanguage");
                    SqlCommand ScCom = new SqlCommand();
                    ScCom = new SqlCommand();
                    ScCom.Connection = con;
                    ScCom.CommandType = CommandType.StoredProcedure;
                    ScCom.CommandText = ScStr.ToString();
                    ScCom.Parameters.Clear();
                    ScCom.Parameters.Add("@LanKey", SqlDbType.NVarChar, 150).Value = objEnt.LanKey.Trim().ToLower().Replace(" ", "");
                    ScCom.Parameters.Add("@LanCode", SqlDbType.NVarChar, 150).Value = objEnt.LanEnglish.Trim().ToLower().Replace(" ", "");

                    //LanID	LanKey	LanCode	LanArabic	LanEnglish	LanTurkish	LanIndian	LanFrench	LanRussian
                    ScCom.Parameters.Add("@LanArabic", SqlDbType.NVarChar, 1000).Value = objEnt.LanArabic;
                    ScCom.Parameters.Add("@LanEnglish", SqlDbType.NVarChar, 1000).Value = objEnt.LanEnglish;
                    ScCom.Parameters.Add("@LanIndian", SqlDbType.NVarChar, 1000).Value = objEnt.LanIndian;
                    ScCom.Parameters.Add("@LanTurkish", SqlDbType.NVarChar, 1000).Value = objEnt.LanTurkish;
                    ScCom.Parameters.Add("@LanFrench", SqlDbType.NVarChar, 1000).Value = objEnt.LanFrench;
                    ScCom.Parameters.Add("@LanRussian", SqlDbType.NVarChar, 1000).Value = objEnt.LanRussian;

                    con.Open();

                    object ob = (object)ScCom.ExecuteScalar();
                    int.TryParse(ob.ToString(), out newId);

                    return newId;
                }
            }
            catch (Exception ex)
            {
                string c = ex.Message;
                throw;
            }
        }


        public static void Update(string DB, clsLanguage objEnt)
        {
            try
            {


                using (SqlConnection con = new SqlConnection(iCore.Conn))
                {
                    System.Text.StringBuilder ScStr = new System.Text.StringBuilder();
                    ScStr.Clear();
                    ScStr.Append("UpdateLanguage");
                    SqlCommand ScCom = new SqlCommand();
                    ScCom = new SqlCommand();
                    ScCom.Connection = con;
                    ScCom.CommandType = CommandType.StoredProcedure;
                    ScCom.CommandText = ScStr.ToString();
                    ScCom.Parameters.Clear();
                    ScCom.Parameters.Add("@LanID", SqlDbType.Int).Value = objEnt.LanID;
                    ScCom.Parameters.Add("@LanKey", SqlDbType.NVarChar, 150).Value = objEnt.LanKey.Trim().ToLower().Replace(" ", "");
                    ScCom.Parameters.Add("@LanCode", SqlDbType.NVarChar, 150).Value = objEnt.LanEnglish.Trim().ToLower().Replace(" ", "");
                    ScCom.Parameters.Add("@LanArabic", SqlDbType.NVarChar, 1000).Value = objEnt.LanArabic;
                    ScCom.Parameters.Add("@LanEnglish", SqlDbType.NVarChar, 1000).Value = objEnt.LanEnglish;
                    ScCom.Parameters.Add("@LanIndian", SqlDbType.NVarChar, 1000).Value = objEnt.LanIndian;
                    ScCom.Parameters.Add("@LanTurkish", SqlDbType.NVarChar, 1000).Value = objEnt.LanTurkish;
                    ScCom.Parameters.Add("@LanFrench", SqlDbType.NVarChar, 1000).Value = objEnt.LanFrench;
                    ScCom.Parameters.Add("@LanRussian", SqlDbType.NVarChar, 1000).Value = objEnt.LanRussian;

                    con.Open();
                    ScCom.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                string c = ex.Message;
                throw;
            }
        }


        public static Boolean Delete(string DB, int? LanID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(iCore.Conn))
                {
                    string delQuery = "DeleteLanguage";
                    SqlCommand command = new SqlCommand();
                    command = new SqlCommand();
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = delQuery;
                    command.Parameters.Clear();
                    command.Parameters.Add("@LanID", SqlDbType.Int).Value = LanID;
                    con.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                string msg = ex.Message;
                return false;
            }
        }

    }
}
