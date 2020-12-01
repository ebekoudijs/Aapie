using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Aapie
{
    public class Database
    {
        public void Dbconn() {
            var connection = new MySqlConnection("Server=localhost;User ID=root;Password=;Database=COHRoboticstest");
            connection.Open();
            var command = new MySqlCommand("SELECT field FROM table;", connection);
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
