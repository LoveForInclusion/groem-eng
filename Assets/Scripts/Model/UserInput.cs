using System;
using System.Runtime.Serialization;

namespace Model
{
    [Serializable]
    public class UserInput
    {
        public string email;
        public string password;

        public UserInput(string email, string password)
        {
            this.email = email;
            this.password = password;
        }
    }
}