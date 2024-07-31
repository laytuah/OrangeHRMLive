using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrangeHRMLive.Utilities
{
    class DataGenerator
    {
        static char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
        static char[] digits = "0123456789".ToCharArray();
        static char[] alphanumeric = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        static Random random = new Random();

        public static string GenerateRandomString(int length = 10) => BuildString(length, letters);
        public static string GenerateRandomIntegerString(int length = 6) => BuildString(length, digits);
        public static string GenerateRandomAlphanumerics(int length = 12) => BuildString(length, alphanumeric);


        public static string GenerateRandomDate(int desiredMinAge = -18, int desiredMaxAge = -100)
        {
            DateTime today = DateTime.Today;
            DateTime minAge = today.AddDays(desiredMinAge);
            int range = (desiredMaxAge - desiredMinAge);
            DateTime randomDate = minAge.AddDays(random.Next(range + 1));
            return randomDate.ToString("yyyy-MM-dd");
        }

        public static string BuildString(int length, char[] dataType)
        {
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(dataType[random.Next(dataType.Length)]);
            }
            return result.ToString();
        }
    }
}
