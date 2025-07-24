using Microsoft.Data.Sqlite;
using StudentLog.Models;

namespace StudentLog.Services
{
    class StudentFilename
    {
        public static string filename { get; } = "Student.db";
    }
    class StudentDbServices
    {
        public void ValidateDB()
        {
            using (var connection = new SqliteConnection($"Data Source={StudentFilename.filename}"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                            CREATE TABLE IF NOT EXISTS student (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            name TEXT NOT NULL UNIQUE,
                            sex TEXT NOT NULL,
                            birth_date TEXT NOT NULL
                        )";

                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertStudent(Student student)
        {
            using (var connection = new SqliteConnection($"Data Source={StudentFilename.filename}"))
            {
                connection.Open();
                ValidateDB();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO student (name, sex, birth_date) VALUES (@name, @sex, @birth_date)";

                    command.Parameters.AddWithValue("@name", student.Name);
                    command.Parameters.AddWithValue("@sex", student.Sex);
                    command.Parameters.AddWithValue("birth_date", student.BirthDate);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}