using System;

namespace Model
{
    [Serializable]
    public class Token
    {
        public string userId;
        public string token;

        public Token(string userId, string token)
        {
            this.userId = userId;
            this.token = token;
        }
    }
}