namespace TMDbLib.Objects.Authentication
{
    /// <summary>
    /// Session object that can be retrieved after the user has correctly authenticated himself on the TMDb site. (using the referal url from the token provided previously)
    /// </summary>
    public class UserSession
    {
        public string SessionId { get; set; }
        public bool Success { get; set; }
    }
}
