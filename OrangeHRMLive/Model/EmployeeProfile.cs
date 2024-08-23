using OrangeHRMLive.Utilities;

namespace OrangeHRMLive.Model
{
    public class EmployeeProfile
    {
        public string? Firstname { get; set; } = DataGenerator.GenerateRandomString();
        public string? Middlename { get; set; } = DataGenerator.GenerateRandomString();
        public string? Lastname { get; set; } = DataGenerator.GenerateRandomString();
        public string? DriversLicenseNumber { get; set; } = DataGenerator.GenerateRandomAlphanumerics();
        public string? EmployeeID { get; set; }
    }
}
