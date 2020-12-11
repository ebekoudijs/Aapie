using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
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
                return null;
            }
        }
        public async Task<bool> CheckCredentials(string username, string password)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.Connection = await OpenConnection();
            cmd.CommandText = "SELECT * FROM user WHERE username = @username AND password = @password";
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            MySqlDataReader myReader = (await cmd.ExecuteReaderAsync() as MySqlDataReader);
            bool result = await myReader.ReadAsync();
            if (result)
            {
                string dbusername = myReader.GetString("username");
                string dbphonenumber = myReader.GetString("phonenumber");
                await myReader.CloseAsync();
                User loggedUser = new User(dbusername, null, dbphonenumber);
                await CloseConnection();
                return true;
            }
            else
            {
                await CloseConnection();
                return false;
            }
        }
        public async Task<List<Product>> GetDrinks() {
            List<Product> Productlist = new List<Product>();


            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.Connection = await OpenConnection();
            cmd.CommandText = "SELECT * FROM products";
            cmd.Prepare();
            MySqlDataReader myReader = (await cmd.ExecuteReaderAsync() as MySqlDataReader);
            while (await myReader.ReadAsync() )
            {
                string name = myReader.GetString("name");
                string Description = myReader.GetString("description");
                double price = myReader.GetDouble("price");
                double AmountCl = myReader.GetDouble("amountcl");
                int Stock = myReader.GetInt32("stock");
                double AlcPercent = myReader.GetDouble("alcpercent");
                Product drink = new Product(name, price, AlcPercent, Stock, AmountCl, Description);
                Productlist.Add(drink);
            }
            await myReader.CloseAsync();
            await CloseConnection();
            return Productlist;
        }
    }
}

