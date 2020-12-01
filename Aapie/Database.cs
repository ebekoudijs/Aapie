using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Aapie
{
    public class Database
    {
        //Functie voor verbinden met database
        public void Dbconn() {
            //Door middel van de mysqlconnector library een nieuwe variabele declaren waarin ip-adres, username, wachtwoord en naam van de database staat om te verbinden met de database server
            var connection = new MySqlConnection("Server=localhost;User ID=root;Password=;Database=COHRoboticstest");
            //functie van de library om te verbinden met de database door middel van bovenstaande variabele
            connection.Open();
            //Commando in SQL code tussen aanhalingstekens, atm selecteert deze van alle rijen de kolom field van de database table
            var command = new MySqlCommand("SELECT field FROM table;", connection);
            //Script hieronder komt rechtstreeks van de library, leest per keer een rij van de tabel en print deze in de console als string
            var reader = command.ExecuteReader();
                 while (reader.Read())
                    Console.WriteLine(reader.GetString(0));  
        }

        public void AddOrder() {
            
        
        }
        public void AddUser() { 
        
        
        }
        
    }
}
