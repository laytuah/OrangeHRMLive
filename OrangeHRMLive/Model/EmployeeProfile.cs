using OrangeHRMLive.Utilities;

namespace OrangeHRMLive.Model
{
    public class EmployeeProfile
    {
        public string? Firstname { get; set; } = DataGenerator.GenerateRandomString();
        public string? Middlename { get; set; } = DataGenerator.GenerateRandomString();
        public string? Lastname { get; set; } = DataGenerator.GenerateRandomString();
        public string? DriversLicenseNumber { get; set; } = DataGenerator.GenerateRandomAlphanumerics();
        public string? Street { get; set; } = DataGenerator.GenerateRandomString(6) + "Street";
        public string? City { get; set; } = DataGenerator.GenerateRandomString();
        public string? Telephone { get; set; } = "07"+ DataGenerator.GenerateRandomIntegerString(8);
        public string? Email { get; set; } = DataGenerator.GenerateRandomString(16) + "@yahoo.com";
        public string JobTitle { get; set; } = "automaton tester";
        public string EmploymentStatus { get; set; } = "freelance";
        public string? EmployeeID { get; set; }
    }
}
