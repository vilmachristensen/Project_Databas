using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
            dbCommand.Parameters.Add("Pr_Losenord", SqlDbType.Int).Value = pd.Pr_Losenord;


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


            //SqlString och lägg till en user i databasen
            String sqlstring = "Select * From Tbl_Profil WHERE Pr_Mail = @mail and Pr_Losenord = @losenord";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("mail", SqlDbType.Int).Value = profil_mail;
            dbCommand.Parameters.Add("losenord", SqlDbType.Int).Value = profil_losenord;

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
                    pd.Pr_Id = Convert.ToInt16(myDS.Tables["myPerson"].Rows[i]["Pe_Id"]);
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

    }
}

