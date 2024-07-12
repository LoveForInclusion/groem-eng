using System;

namespace Model
{
    [Serializable]
	public class TokenIBM
    {
        public string access_token;
        public string refresh_token;
		public string token_type;
		public int expires_in;
		public long expiration;

        public TokenIBM (string access_token, string refresh_token, string token_type, int expires_in, long expiration)
		{
			this.access_token = access_token;
			this.refresh_token = refresh_token;
			this.token_type = token_type;
			this.expires_in = expires_in;
			this.expiration = expiration;
		}
		
    }
}