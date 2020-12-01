using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//gemaakt met deze tut https://www.codeproject.com/articles/43438/connect-c-to-mysql
namespace Aapie
{
    public class Database
    {
        //datafields initialiseren
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        
        //de constructor roept standaard de initliaseer functie aan, welke waardes geeft aan alle nodige datafields voor een verbinding
        public Database() {
            Initialize();
        }
        private void Initialize() {
            server = "localhost";
            database = "cohroboticstest";
            uid = "root";
            password = "";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }
        
        //Nu alle instellingen goed staan kan je met deze functie een verbinding naar de database openen, try catch zo ergt ervoor dat het programma niet crasht als er geen verbinding mogelijk is met de database
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
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
                return false;
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
        public void Insert(string query)
        {
            
            if (this.OpenConnection() == true)
            {
               //Command maken
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Command uitvoeren
                cmd.ExecuteNonQuery();

                //Verbinding sluiten
                CloseConnection();
            }
        }
        //Nu staan er alleen nog extreem algemene functies in (zoals de functie insert) deze moeten nog vervangen worden voor functies zoals adduser, dit is te doen door delen van de query aanpasbaar te maken als parameters in de functie

    }
}

