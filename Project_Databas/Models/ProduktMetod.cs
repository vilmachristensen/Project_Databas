using System;
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
        //public ProduktDetaljer GetProdukter(out string errormsg)
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
                //Öppna connection till databasen
                dbConnection.Open();

                //Fyller dataset med data i en tabell med namnet produkt
                myAdapter.Fill(myDS, "produkt");

                int count = 0;
                int i = 0;
                count = myDS.Tables["produkt"].Rows.Count;

                if (count > 0)
                {
                    while (i < count)
                    {
                        //Läser ut data från dataset
                        ProduktDetaljer pd = new ProduktDetaljer();
                        pd.Prd_Id = Convert.ToInt16(myDS.Tables["produkt"].Rows[i]["Prd_Id"]);
                        pd.Prd_Namn = myDS.Tables["produkt"].Rows[i]["Prd_Namn"].ToString();
                        pd.Prd_Pris = Convert.ToInt16(myDS.Tables["produkt"].Rows[i]["Prd_Pris"]);
                        pd.Prd_Beskrivning = myDS.Tables["produkt"].Rows[i]["Prd_Beskrivning"].ToString();

                    i++;
                    ProduktLista.Add(pd);
                    //errormsg = "";
                    //return pd;
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

        // HÄMTA PRODUKTINFO
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
                //Öppna connection till databasen
                dbConnection.Open();

                //Fyller dataset med data i en tabell med namnet produkt
                myAdapter.Fill(myDS, "produkt");

                int count = 0;
                int i = 0;
                count = myDS.Tables["produkt"].Rows.Count;

                if (count > 0)
                {
                    //Läser ut data från dataset
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
    }
}

