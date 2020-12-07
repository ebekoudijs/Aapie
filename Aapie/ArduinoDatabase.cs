using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



//gemaakt met deze tut https://www.codeproject.com/articles/43438/connect-c-to-mysql
namespace Aapie
{
    public class ArduinoDatabase
    {
        //datafields initialiseren
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        
        //de constructor roept standaard de initliaseer functie aan, welke waardes geeft aan alle nodige datafields voor een verbinding
        public ArduinoDatabase() {
            Initialize();
        }
        private void Initialize() {
            server = "localhost";
            database = "botcontroller";
            uid = "root";
            password = "";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }
        
        //Nu alle instellingen goed staan kan je met deze functie een verbinding naar de database openen, try catch zo ergt ervoor dat het programma niet crasht als er geen verbinding mogelijk is met de database
        public async Task<MySqlConnection> OpenConnection()
        {
            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }
                
                return connection;
            }
            catch (MySqlException exception)
            {
                //Als er een mysql error is wordt ie doorgegeven naar deze switch statement
                switch (exception.Number)
                {
                    case 0:
                        Console.WriteLine("Server is offline");
                        break;

                    case 1045:
                        Console.WriteLine("Foute credentials");
                        break;
                }
                return connection;
            }
        }

        //Zelfde als bovenstaande functie maar dan om verbinding te closen
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
 
        public async Task UpdateData(int left, int right, int straight, int id) {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.Connection = await OpenConnection();
            cmd.CommandText = "UPDATE controls SET `left`= @left, `right` = @right, `straight` = @straight  WHERE id=@id";
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@left", left);
            cmd.Parameters.AddWithValue("@right", right);
            cmd.Parameters.AddWithValue("@straight", straight);
            cmd.ExecuteNonQuery();
            CloseConnection();
        }
    }
}

