using Microsoft.Data.Sqlite;
using BCrypt.Net;

namespace StudentLog.Services
{
    // use this to avoid filename confusion
    class Filename
    {
        public static string filename { get; } = "Admin.db";
    }

    class AdminDbServices
    {
        public void ValidateDB()
        {
            using (var connection = new SqliteConnection($"Data Source={Filename.filename}"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS admin (
                        username TEXT NOT NULL,
                        password TEXT NOT NULL
                        );
                    ";

                    command.ExecuteNonQuery();
                }
            }
        }

        public bool ValidateSignIn(string input_username, string input_password, out bool isValidationSuccess)
        {
            string? password = null;
            string? username = null;

            try
            {
                using (var connection = new SqliteConnection($"Data Source={Filename.filename}"))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = @"SELECT * FROM admin";

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                password = (string)reader["password"];
                                username = (string)reader["username"];
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                isValidationSuccess = false;
                return false;
            }


            if (username is null || password is null)
            {
                isValidationSuccess = false;
                return false;
            }

            Console.WriteLine(username + password);
            Console.WriteLine(input_password + input_username);


            if (input_username != username || !BCrypt.Net.BCrypt.Verify(input_password, password))
            {
                Console.WriteLine();
                isValidationSuccess = true;
                return false;
            }
            
            isValidationSuccess = true;
            return true;

        }

        /* 
        
        USE THIS METHOD ONLY WHEN CREATING A FRESH DB, THIS WOULD INSERT DEFAULT USERNAME AND PASSWORD VALUES

        public void QuickInsert()
        {
            ValidateDB();
            string pass = "admin123";
            string username = "admin";

            string password = BCrypt.Net.BCrypt.HashPassword(pass);
            

            using (var connection = new SqliteConnection($"Data Source={Filename.filename}"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO admin (username, password) VALUES (@username, @password);";

                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    command.ExecuteNonQuery();
                }
            }
        }
        */

    }
}