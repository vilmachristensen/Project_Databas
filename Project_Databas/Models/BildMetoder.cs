﻿using System;
using System.Data;
using System.Data.SqlClient;

namespace Project_Databas.Models
{
    public class BildMetoder
    {
        public BildMetoder()
        {
        }

        public IConfigurationRoot GetConnection()

        {

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();

            return builder;

        }

        // LADDA UPP BILD
        public Byte[] Upload(out string errormsg, ProfilDetaljer pd, int profilId)
        {
            try
            {
                Byte[] bytes = null;
                if (pd.Pr_Bild != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        pd.Pr_Bild.OpenReadStream().CopyTo(ms);
                        bytes = ms.ToArray();
                    }

                    string connectionstring = GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
                    SqlConnection con = new SqlConnection(connectionstring);

                    SqlCommand cmd = new SqlCommand("UPDATE Tbl_Profil SET Pr_Bild = @bild WHERE Pr_Id = @profilId", con);

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("bild", bytes);
                    cmd.Parameters.AddWithValue("profilId", profilId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    errormsg = "";
                    return bytes;

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            errormsg = "";
            return null;
        }

        // VISA BILD
        public Byte[] ViewPicture(out string errormsg, int profilId)
        {
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            String sqlstring = "SELECT Pr_Bild FROM Tbl_Profil WHERE Pr_Id = @profilId";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("profilId", SqlDbType.NVarChar, 30).Value = profilId;

            SqlDataReader reader = null;

            Byte[] bytes = null;

            errormsg = "";

            try
            {
                dbConnection.Open();

                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    bytes = (byte[])(reader["Pr_Bild"]);
                }
                reader.Close();
                return bytes;
            }
            catch (Exception)
            {
                errormsg = "Bilden kan inte visas";
                return null;
            }
            finally
            {
                dbConnection.Close();
            }

        }
    }
}

