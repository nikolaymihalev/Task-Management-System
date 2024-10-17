namespace System.Security.Claims
{
    /// <summary>
    /// Extensions for User Claims
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Get User identifier
        /// </summary>
        /// <returns>User identifier</returns>
        public static string Id(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
