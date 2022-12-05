using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RitProject
{
    class DataBase
    {

        private SqlConnection connection;

        public DataBase(string stringConnection)
        {

            try
            {
                connection = new SqlConnection(stringConnection);
                connection.Open();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public void deleteEntry(int id)
        {
            SqlCommand command = new SqlCommand("DELETE FROM Markers WHERE Id = @id;", connection);
            command.Parameters.AddWithValue("id", id);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public void insertNewData(string data)
        {
            string[] dataArray = data.Split(';');

            SqlCommand command = new SqlCommand("INSERT INTO Markers (Name,Lat,Lng) VALUES (@name,@lat,@lng);", connection);
            command.Parameters.AddWithValue("name", dataArray[0]);
            command.Parameters.AddWithValue("lat", Convert.ToDouble(dataArray[1]));
            command.Parameters.AddWithValue("lng", Convert.ToDouble(dataArray[2]));
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }


        public List<Machinery> SelectAll()
        {
            SqlCommand command = new SqlCommand("SELECT * FROM Markers", connection);

            List<Machinery> data = new List<Machinery>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    data.Add(new Machinery((int)reader[0], reader[1].ToString(), (double)reader[2], (double)reader[3]));
                }

            }

            return data;

        }

        public void ChangePosition(int id, double lat, double lng)
        {


            SqlCommand command = new SqlCommand("UPDATE Markers SET Lat = @lat, Lng = @lng WHERE id = @id", connection);
            command.Parameters.AddWithValue("@lat", lat);
            command.Parameters.AddWithValue("@lng", lng);
            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();


        }

        public void closeConnection()
        {
            connection.Close();
        }

    }
}
