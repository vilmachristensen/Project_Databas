using System;
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

        public Byte[] Upload(out string errormsg, ProfilDetaljer pd, string user)
        {
            try
            {
                Byte[] bytes = null;
                if (pd.ImageFile != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        pd.ImageFile.OpenReadStream().CopyTo(ms);
                        bytes = ms.ToArray();
                    }

                    string connectionstring = GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
                    SqlConnection con = new SqlConnection(connectionstring);

                    //SqlCommand cmd = new SqlCommand("INSERT INTO [Tbl_Profil]([Pr_Bild]) VALUES (@bild) WHERE Pr_Mail = @Pr_Mail", con);
                    SqlCommand cmd = new SqlCommand("UPDATE Tbl_Profil SET Pr_Bild = @bild WHERE Pr_Mail = @user", con);
                    //SqlCommand cmd = new SqlCommand("UPDATE Tbl_Profil SET Pr_Bild = '" + pd.ImageFile + "' WHERE Pr_Mail LIKE '%" + pd.Pr_Mail + "%'", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("bild", bytes);
                    cmd.Parameters.AddWithValue("user", user);
                    //cmd.Parameters.AddWithValue("Pr_Mail", pd.Pr_Mail);
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

        public Byte[] ViewPicture(string profil_mail, string profil_losenord, out string errormsg, string user)
        {
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            String sqlstring = "SELECT Pr_Bild FROM Tbl_Profil WHERE Pr_Mail = @user";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("user", SqlDbType.NVarChar, 30).Value = user;

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

