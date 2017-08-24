using System.Net.Http;

namespace SessionModuleClient
{
    public static class UserSessionExtension
    {
        const string UserSessionKey = "_USER_SESSION_";

        public static UserSessionDto GetUserSession(this HttpRequestMessage request)
        {
            return request.Properties.ContainsKey(UserSessionKey)
                ? request.Properties[UserSessionKey] as UserSessionDto
                : null;
        }

        internal static void SetUserSession(
            this HttpRequestMessage request, UserSessionDto session)
        {
            if (session == null) { return; }
            request.Properties[UserSessionKey] = session;
        }
    }
}