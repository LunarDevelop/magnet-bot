using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot
{
    public class DBConnection
    {
        public DBConnection()
        {
            Server = Environment.GetEnvironmentVariable("DBSever");
            Port = Environment.GetEnvironmentVariable("DBPort");
            DatabaseName = Environment.GetEnvironmentVariable("DBDatabase");
            UserName = Environment.GetEnvironmentVariable("DBUser");
            Password = Environment.GetEnvironmentVariable("DBPassword");
            SslM = Environment.GetEnvironmentVariable("DBsslM");

            string connString = GetConnectionString();

            Connection = new MySqlConnection(connString);

            TestConnect();
        }

        public string? Server { get; set; }
        public string? Port { get; set; }
        public string? DatabaseName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? SslM { get; set; }

        public MySqlConnection Connection { get; set; }

        private string GetConnectionString()
        {
            string connString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", Server, Port, UserName, Password, DatabaseName, SslM);
            return connString;
        }

        public void OpenConnection()
        {
            Connection.Open();
        }

        public void CloseConnection()
        {
            Connection.Close();
        }

        public void TestConnect()
        {
            try
            {
                Connection.Open();

                Console.WriteLine("Connection Successful!");

                Connection.Close();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message + Connection.ConnectionString);
            }

        }

        public bool QueryExecutor(string query)
        {
            MySqlCommand command = new(query, Connection);
            bool result = false;
            try
            {
                Connection.Open();
                int linesEffected = command.ExecuteNonQuery();
                Console.WriteLine($"Command Executed, {linesEffected} lines were effected.");
                result = true;
            }
            catch (MySqlException e)
            {
                Console.WriteLine($"Error! Details: {e}");
            }
            finally
            {
                Connection.Close();
            }
            return result;
        }

        public MySqlDataReader? SelectQueryExecutor(string col, string table, string? where)
        {
            string query = $"SELECT {col} FROM {table}";
            MySqlDataReader? reader = null;

            if (where != null)
            {
                query += $"WHERE {where}";
            }

            MySqlCommand command = new(query, Connection);
            
            try
            {
                reader = command.ExecuteReader();
            }
            catch (MySqlException e)
            {
                Console.WriteLine($"Error! Details: {e}");
                reader = null;
            }

            return reader;

        }
    }
}
