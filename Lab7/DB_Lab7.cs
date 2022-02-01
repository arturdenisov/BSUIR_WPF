using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Lab7
{
    class CarDB
    {
        //read connection data from App.config file (with help of ConfigurationManager)
        private static string connectionStrParam = ConfigurationManager.ConnectionStrings["connectionDataFromAppConfig"].ConnectionString;
        static SqlConnection connection = null;

        public Int32 ID { get; set; }
        public String Model { get; set; }
        public Int32 Price { get; set; }
        public Int32 YearOfProduction { get; set; }
        public Int32 Quantity { get; set; }


        //-- Connection with database ---- (need for a first connection and read of a DB)
        static CarDB()
        {
            connection = new SqlConnection(connectionStrParam);
        }

        public CarDB()
        {
            connection = new SqlConnection(connectionStrParam);
        }

        //--- Load data from SQL Database ---
        public static IEnumerable<CarDB> LoadData()
        {
            SqlCommand command = new SqlCommand("SELECT * FROM Cars", connection);

            //we need a static constr for the first use of this method
            connection.Open();  
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    Int32 id = dataReader.GetInt32(0);
                    String model = dataReader.GetString(1);
                    Int32 price = dataReader.GetInt32(2);
                    Int32 yearOfProduction = dataReader.GetInt32(3);
                    Int32 quantitty = dataReader.GetInt32(4);

                    CarDB carItem = new CarDB
                    {
                        ID = id,
                        Model = model,
                        Price = price,
                        YearOfProduction = yearOfProduction,
                        Quantity = quantitty
                    };
                    yield return carItem;
                }
            }
            connection.Close();
        }


        public void Insert()
        {
            SqlCommand insertCommand = new SqlCommand("" +
                "INSERT INTO cars (Model, Price, YearOfProduction, Quantity) " +
                "VALUES (@model, @price, @year, @quantity)" +
                "", connection);
            insertCommand.Parameters.Add(new SqlParameter("@model", typeof(string))).Value = Model;
            insertCommand.Parameters.Add(new SqlParameter("@price", typeof(int))).Value = Price;
            insertCommand.Parameters.Add(new SqlParameter("@year", typeof(int))).Value = YearOfProduction;
            insertCommand.Parameters.Add(new SqlParameter("@quantity", typeof(int))).Value = Quantity;

            connection.Open();
            insertCommand.ExecuteNonQuery();
            connection.Close();
        }

        public void Delete()
        {
            SqlCommand insertCommand = new SqlCommand("" +
                "DELETE FROM cars " +
                "WHERE ID_car=@id" +
                "", connection);
            insertCommand.Parameters.Add(new SqlParameter("@id", typeof(int))).Value = ID;

            connection.Open();
            insertCommand.ExecuteNonQuery();
            connection.Close();
        }

        public void Update()
        {
            SqlCommand insertCommand = new SqlCommand("" +
               "UPDATE cars " +
               "SET Model=@model, Price=@price, YearOfProduction=@year, Quantity=@quantity " +
               "WHERE ID_car=@id " +
                "", connection);

            insertCommand.Parameters.Add(new SqlParameter("@id", typeof(int))).Value = ID;
            insertCommand.Parameters.Add(new SqlParameter("@model", typeof(string))).Value = Model;
            insertCommand.Parameters.Add(new SqlParameter("@price", typeof(int))).Value = Price;
            insertCommand.Parameters.Add(new SqlParameter("@year", typeof(int))).Value = YearOfProduction;
            insertCommand.Parameters.Add(new SqlParameter("@quantity", typeof(int))).Value = Quantity;

            connection.Open();
            insertCommand.ExecuteNonQuery();
            connection.Close();
        }


    }
}
