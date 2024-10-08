﻿using System;
using System.Data;
using System.Data.SqlClient;

namespace Project_Databas.Models
{
	public class ProduktMetod
	{
		public ProduktMetod()
		{
		}

        public IConfigurationRoot GetConnection()

        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();
            return builder;
        }

        // HÄMTA ALL PRODUKTINFO
        public List<ProduktDetaljer>GetProdukter(out string errormsg)
        {
            //Skapa SqlConnection
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            String sqlstring = "Select * From Tbl_Produkt";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            List<ProduktDetaljer> ProduktLista = new List<ProduktDetaljer>();

            try
            {
                dbConnection.Open();

                myAdapter.Fill(myDS, "produkt");

                int count = 0;
                int i = 0;
                count = myDS.Tables["produkt"].Rows.Count;

                if (count > 0)
                {
                    while (i < count)
                    {
                        ProduktDetaljer pd = new ProduktDetaljer();
                        pd.Prd_Id = Convert.ToInt16(myDS.Tables["produkt"].Rows[i]["Prd_Id"]);
                        pd.Prd_Namn = myDS.Tables["produkt"].Rows[i]["Prd_Namn"].ToString();
                        pd.Prd_Pris = Convert.ToInt16(myDS.Tables["produkt"].Rows[i]["Prd_Pris"]);
                        pd.Prd_Beskrivning = myDS.Tables["produkt"].Rows[i]["Prd_Beskrivning"].ToString();

                    i++;
                    ProduktLista.Add(pd);
                    }
                    errormsg = "";
                    return ProduktLista;

                }
                else
                {
                    errormsg = "Det hämtas ingen produkt";
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

        // HÄMTA PRODUKTINFO FÖR EN PRODUKT
        public ProduktDetaljer GetProdukt(int produktId, out string errormsg)
        {
            //Skapa SqlConnection
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            String sqlstring = "Select * From Tbl_Produkt WHERE Prd_Id = @id";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("id", SqlDbType.Int).Value = produktId;
       
            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            try
            {
                dbConnection.Open();

                myAdapter.Fill(myDS, "produkt");

                int count = 0;
                int i = 0;
                count = myDS.Tables["produkt"].Rows.Count;

                if (count > 0)
                {
                    ProduktDetaljer pd = new ProduktDetaljer();
                    pd.Prd_Id = Convert.ToInt16(myDS.Tables["produkt"].Rows[i]["Prd_Id"]);
                    pd.Prd_Namn = myDS.Tables["produkt"].Rows[i]["Prd_Namn"].ToString();
                    pd.Prd_Pris = Convert.ToInt16(myDS.Tables["produkt"].Rows[i]["Prd_Pris"]);
                    pd.Prd_Beskrivning = myDS.Tables["produkt"].Rows[i]["Prd_Beskrivning"].ToString();

                    errormsg = "";
                    return pd;

                }
                else
                {
                    errormsg = "Det hämtas ingen produkt";
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

        // HÄMTA INNEHÅLL I KUNDKORG
        public List<KundkorgDetaljer>GetKundkorg(int profilId, out string errormsg)
        {
            //Skapa SqlConnection
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            String sqlstring = "SELECT Tbl_Profil.Pr_Namn, Tbl_Produkt.Prd_Namn, Tbl_Produkt.Prd_Pris, Tbl_Produkt.Prd_Id\nFROM ((Tbl_Profil\nINNER JOIN Tbl_Kundkorg ON Tbl_Profil.Pr_Id = Tbl_Kundkorg.Pr_Id)\nINNER JOIN Tbl_Produkt ON Tbl_Kundkorg.Prd_Id = Tbl_Produkt.Prd_Id) WHERE Tbl_Profil.Pr_Id = @profilId";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("profilId", SqlDbType.Int).Value = profilId;

            SqlDataReader reader = null;

            List<KundkorgDetaljer> KundkorgLista = new List<KundkorgDetaljer>();

            errormsg = "";

            try
            {
                dbConnection.Open();
                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    KundkorgDetaljer kd = new KundkorgDetaljer();
                    kd.Prd_Namn = reader["Prd_Namn"].ToString();
                    kd.Pr_Namn = reader["Pr_Namn"].ToString();
                    kd.Prd_Pris = Convert.ToInt16(reader["Prd_Pris"]);
                    kd.Prd_Id = Convert.ToInt16(reader["Prd_Id"]);

                    KundkorgLista.Add(kd);
                }
                reader.Close();
                return KundkorgLista;
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

        // LÄGG TILL PRODUKT I KUNDKORG
        public int InsertKundkorg(ProduktDetaljer pd, int profilId, int produktId, out string errormsg)
        {
            //Skapa SqlConnection
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            String sqlstring = "INSERT INTO [Tbl_Kundkorg]([Prd_Id], [Pr_Id]) VALUES (@produktId, @profilId)";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("produktId", SqlDbType.Int).Value = produktId;
            dbCommand.Parameters.Add("profilId", SqlDbType.Int).Value = profilId;

            errormsg = "";

            try
            {
                dbConnection.Open();
                int i = 0;

                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Det läggs inte till i kundkorgen."; }
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

        // TA BORT PRODUKT
        public int DeleteProdukt(int id, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection(GetConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            String sqlstring = "Delete From Tbl_Kundkorg Where Prd_Id LIKE '%" + id + "%'";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Det raderas inte någon produkt från databasen!"; }
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

    }
}

