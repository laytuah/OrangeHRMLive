using System.Text;

namespace OrangeHRMLive.Utilities
{
    class DataGenerator
    {
        static char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
        static char[] digits = "0123456789".ToCharArray();
        static Random random = new Random();

        public static string GenerateRandomString(int length = 10)
        {
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(letters[random.Next(letters.Length)]);
            }
            return result.ToString();
        }

        public static string GenerateRandomIntegerString(int length = 8)
        {
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(digits[random.Next(digits.Length)]);
            }
            return result.ToString();
        }

        public static string GenerateRandomDate(int desiredMinAge = -18, int desiredMaxAge = -100)
        {
            DateTime today = DateTime.Today;
            DateTime minAge = today.AddDays(desiredMinAge);
            int range = (desiredMaxAge - desiredMinAge);
            DateTime randomDate = minAge.AddDays(random.Next(range + 1));
            return randomDate.ToString("yyyy-MM-dd");
        }
    }
}
