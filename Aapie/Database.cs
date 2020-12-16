﻿using MySql.Data.MySqlClient;
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
            server = "192.168.68.117";
            database = "database busn";
            uid = "Root";
            password = "Root";
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
                        Console.WriteLine("Database server is offline");
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
        public async Task<User> AddUser(User user) {
            Guid guid = Guid.NewGuid();
            string guidString = guid.ToString();
            user.UserId = guidString;
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = await OpenConnection();
            cmd.CommandText = "INSERT INTO klant(UserID, Naam, Geslacht, Leeftijd, Email, Wachtwoord) VALUES(@userID, @username, @geslacht, @leeftijd, @email, @password)";
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@userID", user.UserId);
            cmd.Parameters.AddWithValue("@username", user.Username);
            cmd.Parameters.AddWithValue("@geslacht", user.Gender);
            cmd.Parameters.AddWithValue("@leeftijd", user.Age);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@password", user.Password);
            cmd.ExecuteNonQuery();
            await CloseConnection();
            user.Password = null;
            return user;
        }
        public async Task RemoveUser(int id)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = await OpenConnection();
            cmd.CommandText = "DELETE FROM user WHERE id = @id";
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            await CloseConnection();
        }

        public async Task<User> CheckCredentials(string username, string password)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = await OpenConnection();
            cmd.CommandText = "SELECT * FROM klant WHERE Naam = @username AND Wachtwoord = @password";
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@username", username);
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
                User loggedUser = new User(dbid, dbusername, dbGender, dbAge, dbEmail, null);
                await CloseConnection();
                return loggedUser;
            }
            else
            {
                await CloseConnection();
                return null;
            }
        }
        public async Task<List<Product>> GetDrinks() {
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
        public async Task<Order> addOrder(Order order, User user) {
            Guid guid = Guid.NewGuid();
            string guidString = guid.ToString();
            foreach (var orderline in order.OrderLines)
            {
                await addOrderLine(orderline, guidString);
            }
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = await OpenConnection();
            cmd.CommandText = "INSERT INTO bestelling(OrderID, UserID, MdwID, TableID, RobotID) VALUES(@OrderID, @UserID, @MdwID, @TableID, @RobotID)";
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@OrderID", guidString);
            cmd.Parameters.AddWithValue("@UserID", user.UserId);
            cmd.Parameters.AddWithValue("@MdwID", 5);
            cmd.Parameters.AddWithValue("@TableID", 5);
            cmd.Parameters.AddWithValue("@RobotID", 5);
            return order;
        }
        public async Task<OrderLine> addOrderLine(OrderLine orderLine, string guidString) {
            Order order = new Order();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = await OpenConnection();
            cmd.CommandText = "INSERT INTO bestelregel(OrderID, DrankID, Aantal) VALUES(@OrderID, @DrankID, @Quantity)";
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@OrderID", guidString);
            cmd.Parameters.AddWithValue("@DrankID", orderLine.Product.Id);
            cmd.Parameters.AddWithValue("@Quantity", orderLine.Quantity);

            return null;
        }
    }
}

