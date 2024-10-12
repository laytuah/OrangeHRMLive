using OrangeHRMLive.Utilities;

namespace OrangeHRMLive.Model
{
    public class EmployeeProfile
    {
        public string? Firstname { get; set; } = DataGenerator.GenerateRandomString();
        public string? Middlename { get; set; } = DataGenerator.GenerateRandomString();
        public string? Lastname { get; set; } = DataGenerator.GenerateRandomString();
        public string? Street { get; set; } = DataGenerator.GenerateRandomString(6) + "Street";
        public string? City { get; set; } = DataGenerator.GenerateRandomString();
        public string? Telephone { get; set; } = "07" + DataGenerator.GenerateRandomIntegerString(8);
        public string? Email { get; set; } = DataGenerator.GenerateRandomString(16) + "@yahoo.com";
        public string? EmployeeID { get; set; } = DataGenerator.GenerateRandomIntegerString(5);
        public string JobTitle { get; set; } = "qa lead";
        public string EmploymentStatus { get; set; } = "full-time permanent";
        public string MaritalStatus { get; set; } = "single";
        public string Nationality { get; set; } = "nigerian";
        public string BloodGroup { get; set; } = "o+";
        public string Gender { get; set; } = "Male";
    }
}
