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
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        
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
        private async Task<bool> CloseConnection()
        {
            try
            {
                await connection.CloseAsync();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        //Nu staan er alleen nog extreem algemene functies in (zoals de functie insert) deze moeten nog vervangen worden voor functies zoals adduser, dit is te doen door delen van de query aanpasbaar te maken als parameters in de functie
        public async Task AddUser(User user) {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.Connection = await OpenConnection();
            cmd.CommandText = "INSERT INTO user(username, password, phonenumber) VALUES(@username, @password, @phonenumber)";
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@username", user.Username);
            cmd.Parameters.AddWithValue("@password", user.Password);
            cmd.Parameters.AddWithValue("@phonenumber", user.PhoneNumber);
            cmd.ExecuteNonQuery();
            await CloseConnection();
        }
        public async Task RemoveUser(int id)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.Connection = await OpenConnection();
            cmd.CommandText = "DELETE FROM user WHERE id = @id";
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            await CloseConnection();
        }
        public async Task<User> GetUser(int id)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.Connection = await OpenConnection();
            cmd.CommandText = "SELECT * FROM user WHERE userID = @id";
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@id", id);
            
            MySqlDataReader myReader = (await cmd.ExecuteReaderAsync() as MySqlDataReader);

            bool result = await myReader.ReadAsync();

            if (result)
            {
                string username = myReader.GetString("username");
                string phonenumber = myReader.GetString("phonenumber");
                User user = new User(username, null, phonenumber);
                await CloseConnection();
                return user;
            }
            else 
            {
                await CloseConnection();
                User user = new User("test", "test", "test");
                return null;
            }
            
            
        }
    }
}

