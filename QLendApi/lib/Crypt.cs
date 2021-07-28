using BCryptHelper = BCrypt.Net.BCrypt;

namespace QLendApi.lib
{
    public class Crypt
    {
        public static string Hash(string Text)
        {
            var Salt = BCryptHelper.GenerateSalt();
            return BCryptHelper.HashPassword(Text, Salt);
        }
        public static bool VerifyHash(string Text, string HashedText)
        {
            return BCryptHelper.Verify(Text, HashedText);
        }
    }
}