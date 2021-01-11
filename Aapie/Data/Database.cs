using Microsoft.Extensions.Configuration;
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
        public Database(IConfiguration config) {
            Initialize(config);
        }
        private void Initialize(IConfiguration config) {

            server = config.GetValue<string>("server");
            database = config.GetValue<string>("database");
            uid = config.GetValue<string>("uid");
            password = config.GetValue<string>("password");
            
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
                Console.WriteLine(exception.Message);
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
        public async Task<User> AddUser(User user) {
            user.UserId = Guid.NewGuid().ToString();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = await OpenConnection();
            cmd.CommandText = "INSERT INTO klant(UserID, Naam, Geslacht, Leeftijd, Email, Wachtwoord) VALUES(@userID, @naam, @geslacht, @leeftijd, @email, @password)";
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@userID", user.UserId);
            cmd.Parameters.AddWithValue("@naam", user.Name);
            cmd.Parameters.AddWithValue("@geslacht", user.Gender);
            cmd.Parameters.AddWithValue("@leeftijd", user.Age);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@password", user.Password);
            cmd.ExecuteNonQuery();
            await CloseConnection();
            user.Password = null;
            return user;
        }
        public async Task RemoveUser(string id)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = await OpenConnection();
            cmd.CommandText = "DELETE FROM user WHERE id = @id";
            cmd.Prepare();

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
            await CloseConnection();
        }
        public async Task<User> GetUser(string email) {
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = await OpenConnection();
            cmd.CommandText = "SELECT * FROM klant WHERE Email = @email";
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@email", email);

            MySqlDataReader myReader = (await cmd.ExecuteReaderAsync() as MySqlDataReader);
            bool result = await myReader.ReadAsync();
            if (result)
            {
                string dbid = myReader.GetString("userID");
                string dbusername = myReader.GetString("Naam");
                string dbGender = myReader.GetString("Geslacht");
                int dbAge = myReader.GetInt32("Leeftijd");
                string dbEmail = myReader.GetString("Email");

                await myReader.CloseAsync();
                User loggedUser = new User(dbid, dbGender, dbAge, dbEmail, null, dbusername);
                await CloseConnection();
                return loggedUser;
            }
            else
            {
                await CloseConnection();
                return null;
            }
        }

        public async Task<User> Authenticate(string email, string password)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = await OpenConnection();
            cmd.CommandText = "SELECT * FROM klant WHERE Email = @email AND Wachtwoord = @password";
            cmd.Prepare();

            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@password", password);

            MySqlDataReader myReader = (await cmd.ExecuteReaderAsync() as MySqlDataReader);
            bool result = await myReader.ReadAsync();
            if (result)
            {
                string dbid = myReader.GetString("userID");
                string dbusername = myReader.GetString("Naam");
                string dbGender = myReader.GetString("Geslacht");
                int dbAge = myReader.GetInt32("Leeftijd");
                string dbEmail = myReader.GetString("Email");

                await myReader.CloseAsync();
                User loggedUser = new User(dbid, dbGender, dbAge, dbEmail, null, dbusername);
                await CloseConnection();
                return loggedUser;
            }
            else
            {
                await CloseConnection();
                return null;
            }
        }
        public async Task<List<Product>> GetProducts() {
            List<Product> Productlist = new List<Product>();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = await OpenConnection();
            cmd.CommandText = "SELECT * FROM artikel";
            cmd.Prepare();

            MySqlDataReader myReader = (await cmd.ExecuteReaderAsync() as MySqlDataReader);
            while (await myReader.ReadAsync())
            {
                int id = myReader.GetInt32("DRANKid");
                string name = myReader.GetString("Naam");
                double price = myReader.GetDouble("Prijs");
                string Description = myReader.GetString("Beschrijving");
                double AmountCl = myReader.GetDouble("HoeveelheidCL");
                int Stock = myReader.GetInt32("Stock");
                double AlcPercent = myReader.GetDouble("AlcPercent");
                Product drink = new Product(id, name, price, AlcPercent, Stock, AmountCl, Description);
                Productlist.Add(drink);
            }
            await myReader.CloseAsync();
            await CloseConnection();
            return Productlist;
        }
        public async Task<Order> AddOrder(Order order, string userId) {
            Guid guid = Guid.NewGuid();
            order.OrderId = guid.ToString();

            foreach (var orderline in order.OrderLines)
            {
                if (orderline.Quantity == 0)
                {

                }
                else
                {
                    await AddOrderLine(orderline, order.OrderId);
                }
            }
            
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = await OpenConnection();
            cmd.CommandText = "INSERT INTO bestelling(OrderID, UserID, MdwID, TableID, RobotID, DatumBestel, Comment) VALUES(@OrderID, @UserID, @MdwID, @TableID, @RobotID, @date, @comment)";
            cmd.Prepare();

            cmd.Parameters.AddWithValue("@OrderID", order.OrderId);
            cmd.Parameters.AddWithValue("@UserID", userId);
            cmd.Parameters.AddWithValue("@MdwID", 1);
            cmd.Parameters.AddWithValue("@TableID", 1);
            cmd.Parameters.AddWithValue("@RobotID", 1);
            cmd.Parameters.AddWithValue("@date", order.OrderDate);
            cmd.Parameters.AddWithValue("@comment", order.Message);

            cmd.ExecuteNonQuery();
            return order;
        }

        public async Task AddOrderLine(OrderLine orderLine, string guidString) {

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = await OpenConnection();
            cmd.CommandText = "INSERT INTO bestelregel(OrderID, DrankID, Aantal) VALUES(@OrderID, @DrankID, @Quantity)";
            cmd.Prepare();

            cmd.Parameters.AddWithValue("@OrderID", guidString);
            cmd.Parameters.AddWithValue("@DrankID", orderLine.ProductId);
            cmd.Parameters.AddWithValue("@Quantity", orderLine.Quantity);

            cmd.ExecuteNonQuery();
        }
        public async Task SetDeliveryDate(string orderId) {
            DateTime DateDelivered = DateTime.Now;
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = await OpenConnection();
            cmd.CommandText = "UPDATE bestelregel SET DatumLever = @DateDelivered WHERE OrderID = @OrderId";
            cmd.Prepare();

            cmd.Parameters.AddWithValue("@DateDelivered", DateDelivered);
            cmd.Parameters.AddWithValue("@OrderId", orderId);

            cmd.ExecuteNonQuery();
        }
        public async Task<List<Order>> GetOrders(string userid) {
            List<Order> OrderList = new List<Order>();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = await OpenConnection();
            cmd.CommandText = "SELECT * FROM bestelling WHERE UserID = @userid";
            cmd.Prepare();

            cmd.Parameters.AddWithValue("@userid", userid);

            MySqlDataReader myReader = (await cmd.ExecuteReaderAsync() as MySqlDataReader);
            while (await myReader.ReadAsync())
            {
                string Message = myReader.GetString("Comment");
                int Table = myReader.GetInt32("TableID");
                string OrderId = myReader.GetString("OrderID");
                DateTime OrderDate = myReader.GetDateTime("DatumBestel");
                //DateTime DeliverDate = myReader.GetDateTime("DatumLever");
                
                Order order = new Order(OrderId, OrderDate, Message, Table);
                
                OrderList.Add(order);
            } 
            await myReader.CloseAsync();
            await CloseConnection();


            return OrderList;
        }
        public async Task<List<OrderLine>> GetOrderLines(string orderid)
        {
            List<OrderLine> OrderLineList = new List<OrderLine>();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = await OpenConnection();
            cmd.CommandText = "SELECT * FROM bestelregel WHERE OrderID = @orderid";
            cmd.Prepare();

            cmd.Parameters.AddWithValue("@orderid", orderid);

            MySqlDataReader myReader = (await cmd.ExecuteReaderAsync() as MySqlDataReader);
            while (await myReader.ReadAsync())
            {
                string OrderId = myReader.GetString("OrderID");
                int Amount = myReader.GetInt32("Aantal");
                int ProductId = myReader.GetInt32("DrankID");

                OrderLine orderline = new OrderLine(OrderId, ProductId, Amount);

                OrderLineList.Add(orderline);
            }
            await myReader.CloseAsync();
            await CloseConnection();


            return OrderLineList;
        }

    }
}