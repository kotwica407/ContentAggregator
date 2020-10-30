namespace ContentAggregator.Services.Helpers
{
    public static class HashHelpers
    {
        public static bool CheckPasswordWithHash(string password, string hash) =>
            BCrypt.Net.BCrypt.Verify(password, hash);

        public static string CreateHash(string password) => BCrypt.Net.BCrypt.HashPassword(password);
    }
}