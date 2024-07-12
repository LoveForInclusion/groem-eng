using System;
using System.Collections;

namespace Model
{
    [Serializable]
    public class User
    {
        public string id;
        public string email;
		public string password;
        public string firstName;
        public string lastName;
        public string role;

        public User()
        {
        }

		public User(string id, string email, string password, string firstName, string lastName, string role)
        {
            this.id = id;
            this.email = email;
			this.password = password;
            this.firstName = firstName;
            this.lastName = lastName;
            this.role = role;
        }
    }
}