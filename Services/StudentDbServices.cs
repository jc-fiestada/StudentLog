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
        // creates table for student if table does not exist's

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

        // delete student using name
        public void DeleteStudent(string name, out bool isDeleted)
        {
            using (var connection = new SqliteConnection($"Data Source={StudentFilename.filename}"))
            {
                connection.Open();
                ValidateDB();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM student WHERE name = @name";

                    command.Parameters.AddWithValue("@name", name);

                    int changed = command.ExecuteNonQuery();

                    if (changed == 0)
                    {
                        isDeleted = false;
                    }
                    else
                    {
                        isDeleted = true;
                    }
                }

            }
        }

        public void UpdateStudent(Student student)
        {
            using (var connection = new SqliteConnection($"Data Source={StudentFilename.filename}"))
            {
                connection.Open();
                ValidateDB();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        UPDATE student
                        SET birth_date = @birth_date, sex = @sex
                        WHERE name = @name;
                    ";

                    command.Parameters.AddWithValue("@name", student.Name);
                    command.Parameters.AddWithValue("sex", student.Sex);
                    command.Parameters.AddWithValue("birth_date", student.BirthDate);

                    command.ExecuteNonQuery();
                }

            }
        }



        // update later this to give back a response when a name has already been saved in the db
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
                    command.Parameters.AddWithValue("@birth_date", student.BirthDate);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Student> ViewStudent()
        {
            List<Student> student_list = new List<Student>();

            using (var connection = new SqliteConnection($"Data Source={StudentFilename.filename}"))
            {
                connection.Open();
                ValidateDB();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM student";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Student student = new Student();

                            student.Name = reader["name"].ToString();
                            student.Sex = reader["sex"].ToString();
                            student.BirthDate = reader["birth_date"].ToString();

                            student_list.Add(student);
                        }
                    }
                }
            }
            return student_list;
        }
    }
}