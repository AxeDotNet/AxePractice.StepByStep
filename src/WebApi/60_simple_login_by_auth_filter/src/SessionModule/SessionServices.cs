using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SessionModule.DomainModels;

namespace SessionModule
{
    public class SessionServices
    {
        readonly ITokenGenerator tokenGenerator;

        readonly ConcurrentDictionary<string, UserSession> sessions =
            new ConcurrentDictionary<string, UserSession>();

        static readonly Dictionary<Credential, string> users =
            new Dictionary<Credential, string>
            {
                {new Credential("nancy", "1111aaaa"), "Nancy Gilbert"},
                {new Credential("kayla", "1111aaaa"), "Kayla Logan"}
            };

        public SessionServices(ITokenGenerator tokenGenerator)
        {
            this.tokenGenerator = tokenGenerator;
        }

        public string Create(Credential credential)
        {
            if (!users.ContainsKey(credential)) { return null; }
            string username = users[credential];
            string sessionToken = tokenGenerator.GenerateToken();
            if (!sessions.TryAdd(sessionToken, new UserSession(username)))
            {
                return null;
            }

            return sessionToken;
        }

        public UserSession Get(string token)
        {
            bool getSessionSuccess = sessions.TryGetValue(token, out UserSession session);
            if (!getSessionSuccess) { return null; }
            return session;
        }

        public bool Delete(string token)
        {
            if (string.IsNullOrEmpty(token)) return false;
            UserSession session;
            return sessions.TryRemove(token, out session);
        }
    }
}