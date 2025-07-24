using System.Text.RegularExpressions;

namespace StudentLog.Models
{
    class Student
    {
        private string? name = null;
        public string? Name
        {
            get { return name; }
            set
            {
                name = value?.Trim().ToUpper();
            }
        }
        public string? Sex { get; set; } = null;
        public string? BirthDate { get; set; } = null;
        public int PrimaryId { get; set; }


        // use as an conditional, if contains error then true, else false
        public List<string> ErrorList()
        {
            List<string> errors = new List<string>();

            Regex pattern = new Regex("^[a-zA-Z ]+$"); // Allows only all capital and lower alphabet
            
            // Name Validation

            if (string.IsNullOrWhiteSpace(Name))
            {
                errors.Add("Name should not be empty");
            }
            else if (Name.Length < 5)
            {
                errors.Add("Name should not exceed lower than 5 characters");
            }
            else if (Name.Length > 60)
            {
                errors.Add("Name should not exceed more than 60 characters");
            }
            else if (!pattern.IsMatch(Name))
            {
                errors.Add("Name should only contain letters and spaces");
            }

            // Sex Validation

            if (string.IsNullOrWhiteSpace(Sex))
            {
                errors.Add("Sex should not be empty");
            }
            else if (Sex.Trim().ToUpper() != "MALE" && Sex.Trim().ToUpper() != "FEMALE")
            {
                errors.Add("Invalid Sex value, it should only be either \"Male\" or \"Female\"");
            }

            DateOnly converted_date;

            DateOnly MinimumDate = DateOnly.Parse("1970-01-01");
            DateOnly MaximumDate = DateOnly.Parse("2015-01-01");

            // Birth Date Validation
            if (string.IsNullOrWhiteSpace(BirthDate))
            {
                errors.Add("Birth Date should not be empty");
            }
            else if (!DateOnly.TryParse(BirthDate, out converted_date))
            {
                errors.Add("Malformed date has been detected");
            }
            else if (converted_date < MinimumDate) {
                errors.Add("Student must have a birth of date of 1970-01-01 onwards to be accepted");
            }
            else if (converted_date >= MaximumDate) {
                errors.Add("Student must have a birth of date before 2015-01-01 to be accepted");
            }


            return errors;
        }
    }
}