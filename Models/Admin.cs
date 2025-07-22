namespace StudentLog.Models
{
    class Admin
    {
        public string? Username { get; set; }
        public string? Password { get; set; }

        // use this as a condition, false if contains an error, else true
        public List<string> ErrorList()
        {
            List<string> errors = new List<string>();

            // Username

            if (string.IsNullOrWhiteSpace(Username))
            {
                errors.Add("Username should not be empty");
            }
            else if (Username.Length > 15)
            {
                errors.Add("Username should not exceed more than 15 characters");
            }
            else if (Username.Length < 5)
            {
                errors.Add("Username should not exceed lower than 5 characters");
            }

            // Password

            if (string.IsNullOrWhiteSpace(Password))
            {
                errors.Add("Password should not be empty");
            }
            else if (Password.Length > 15)
            {
                errors.Add("Password should not exceed more than 15 characters");
            }
            else if (Password.Length < 5)
            {
                errors.Add("Password should not exceed lower than 5 characters");
            }

            return errors;
        }
    }
}