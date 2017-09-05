namespace SessionModule.DomainModels
{
    public class UserSession
    {
        public UserSession(string username)
        {
            Username = username;
        }

        public string Username { get; }
    }
}