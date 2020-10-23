namespace ContentAggregator.Services.Helpers
{
    internal static class HashHelpers
    {
        internal static bool CheckPasswordWithHash(string password, string hash) =>
            BCrypt.Net.BCrypt.Verify(password, hash);

        internal static string CreateHash(string password) => BCrypt.Net.BCrypt.HashPassword(password);
    }
}