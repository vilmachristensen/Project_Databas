using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// LAGT TILL FÖR MAIL
using System.Configuration;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Project_Databas.Models
{
    public class ProfilMetod
    {
        public ProfilMetod()
        {
        }

        public IConfigurationRoot GetConnection()

        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();
            return builder;
        }

        public int SkapaKonto(ProfilDetaljer pd, out string errormsg)
        {
            //Skapa SqlConnection
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            //SqlString och lägg till en user i databasen
            String sqlstring = "INSERT INTO [Tbl_Profil]([Pr_Namn], [Pr_Mail], [Pr_Bor], [Pr_Losenord]) VALUES (@Pr_Namn, @Pr_Mail, @Pr_Bor, @Pr_Losenord)";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("Pr_Namn", SqlDbType.NVarChar, 30).Value = pd.Pr_Namn;
            dbCommand.Parameters.Add("Pr_Mail", SqlDbType.NVarChar, 30).Value = pd.Pr_Mail;
            dbCommand.Parameters.Add("Pr_Bor", SqlDbType.Int).Value = pd.Pr_Bor;
            dbCommand.Parameters.Add("Pr_Losenord", SqlDbType.NVarChar, 30).Value = pd.Pr_Losenord;


            try
            {
                //Öppna connection till databasen
                dbConnection.Open();
                int i = 0;
                //Skicka SQL-frågan till databasen, får tillbaka en int
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Det skapas inte en person i databasen."; }
                return (i);
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public ProfilDetaljer GetProfil(string profil_mail, string profil_losenord, out string errormsg)
        {
            //Skapa SqlConnection
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            String sqlstring = "Select * From Tbl_Profil WHERE Pr_Mail = @mail AND Pr_Losenord = @losenord";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("mail", SqlDbType.NVarChar, 30).Value = profil_mail;
            dbCommand.Parameters.Add("losenord", SqlDbType.NVarChar, 30).Value = profil_losenord;

            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();


            try
            {
                //Öppna connection till databasen
                dbConnection.Open();

                //Fyller dataset med data i en tabell med namnet myPerson
                myAdapter.Fill(myDS, "myPerson");

                int count = 0;
                int i = 0;
                count = myDS.Tables["myPerson"].Rows.Count;

                if (count > 0)
                {
                    //Läser ut data från dataset
                    ProfilDetaljer pd = new ProfilDetaljer();
                    pd.Pr_Id = Convert.ToInt16(myDS.Tables["myPerson"].Rows[i]["Pr_Id"]);
                    pd.Pr_Namn = myDS.Tables["myPerson"].Rows[i]["Pr_Namn"].ToString();
                    pd.Pr_Mail = myDS.Tables["myPerson"].Rows[i]["Pr_Mail"].ToString();
                    pd.Pr_Bor = Convert.ToInt16(myDS.Tables["myPerson"].Rows[i]["Pr_Bor"]);
                    pd.Pr_Losenord = myDS.Tables["myPerson"].Rows[i]["Pr_Losenord"].ToString();


                    errormsg = "";
                    return pd;

                }
                else
                {
                    errormsg = "Det hämtas ingen Person";
                    return (null);
                }
            }

            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }

        }

        // REDIGERA 
        public int EditProfil(ProfilDetaljer pd, out string errormsg)
        {
            // Skapa SqlConnection
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            // sqlstring och lägg till en skidakare i databasen
            String sqlstring = "Update Tbl_Profil Set Pr_Namn = '" + pd.Pr_Namn + "', Pr_Mail = '" + pd.Pr_Mail + "', Pr_Losenord = '" + pd.Pr_Losenord + "', Pr_Bor = '" + pd.Pr_Bor + "' WHERE Pr_Id LIKE '%" + pd.Pr_Id + "%'";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            errormsg = "";

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Det görs inga uppdateringar!"; }
                return (i);
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        // TA BORT 
        public int DeleteProfil(int id, out string errormsg)
        {
            // Skapa SqlConnection
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            // sqlstring och ta bort en skidakare i databasen
            String sqlstring = "Delete From Tbl_Profil Where Pr_Id LIKE '%" + id + "%'";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Det raderas inte någon skidåkare från databasen!"; }
                return (i);
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        // SKICKA MAIL
        /*
        public void SendMail(object sender, EventArgs e)
        {
            // Skapa SqlConnection
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            // sqlstring och ta bort en skidakare i databasen
            String sqlstring = "Select Pr_Mail, Pr_Losenord From Tbl_Profil Where Pr_Mail=@Pr_Mail";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.AddWithValue("@Pr_Mail", TxtEmail.Text);

            SqlDataReader reader = null;

            //errormsg = "";

            try { 
                dbConnection.Open();
                reader = dbCommand.ExecuteReader();

                if (reader.Read())
                {
                    string mail = reader["Pr_Mail"].ToString();
                    string losen = reader["Pr_Losenord"].ToString();

                    MailMessage mm = new MailMessage("linneanilsson00@gmail.com", TxtEmail.Text);
                    mm.Subject = "Återställ ditt lösenord";
                    mm.Body = string.Format("Ditt lösenord är", mail, losen);
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential nc = new NetworkCredential();
                    nc.UserName = "linneanilsson00@gmail.com";
                    nc.Password = "newpassword";
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = nc;
                    smtp.Port = 587;
                    smtp.Send(mm);
                    Labmsg.Text = "Ditt lösenord har skickats till " + TxtEmail.Text;
                    Labmsg.ForeColor = Color.Green;
                }
                else
                {
                    Labmsg.Text = TxtEmail.Text + "Mailen finns inte registrerad";
                    Labmsg.ForeColor = Color.Red;
                }
                reader.Close();
            }
            catch (Exception e)
            {
                //errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }*/
    }
}

