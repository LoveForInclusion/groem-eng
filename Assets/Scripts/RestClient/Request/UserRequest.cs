using System;
using Model;
using UnityEngine;
using UnityEngine.Networking;

namespace RESTClient.Request
{
	public class UserRequest
	{
		public static void Login(MonoBehaviour mono, UserInput userInput, Action<Token> onSuccess, Action<string, string> onError)
		{
			ApiClient.Post(mono, RestConfig.BaseUrl+"api/auth/signin", JsonUtility.ToJson(userInput), null,
				delegate(UnityWebRequest www)
				{
					//Debug.Log("assd: " + www.error + " " + www.method + " " + www.ToString());
					if (www.error != null)
					{
						onError(www.error, www.downloadHandler.text);
						return;
					}
					var token = JsonUtility.FromJson<Token>(www.downloadHandler.text);
					ApiClient.SaveToken(token);
					onSuccess(token);
				});
		}

		public static void SignupUser(MonoBehaviour mono, User newUser, Action<User> onSucccess, Action<string, string> onError)
		{
			ApiClient.Post(mono, RestConfig.BaseUrl+"api/auth/signup", JsonUtility.ToJson(newUser), null,
				delegate(UnityWebRequest www)
				{
					if (www.error != null)
					{
						onError(www.error, www.downloadHandler.text);
						return;
					}
					onSucccess(JsonUtility.FromJson<User>(www.downloadHandler.text));
				});
		}

		public static void GetUser(MonoBehaviour mono, Action<User> onSuccess, Action<string, string> onError)
		{
			ApiClient.Get(mono, RestConfig.BaseUrl+"api/users/" + ApiClient.GetUserId(), null,
				delegate(UnityWebRequest www)
				{
					if (www.error != null)
					{
						onError(www.error, www.downloadHandler.text);
						return;
					}
					onSuccess(JsonUtility.FromJson<User>(www.downloadHandler.text));
				});
		}

		public static void RecoveryPassword(MonoBehaviour mono, string mail, Action<string> onSuccess, Action<string, string> onError)
		{
			var user = new User {email = mail};
			ApiClient.Post(mono, RestConfig.BaseUrl+"api/auth/recovery", JsonUtility.ToJson(user), null,
				delegate(UnityWebRequest www)
				{
					if (www.error != null)
					{
						onError(www.error, www.downloadHandler.text);
						return;
					}
					onSuccess(www.downloadHandler.text);
				});
		}

		public static void RefreshToken(MonoBehaviour mono, Action<Token> onSuccess, Action<string, string> onError)
		{
			ApiClient.Get(mono, RestConfig.BaseUrl+"api/auth/refresh", null,
				delegate(UnityWebRequest www)
				{
					if (www.error != null)
					{
						onError(www.error, www.downloadHandler.text);
						return;
					}
					var token = JsonUtility.FromJson<Token>(www.downloadHandler.text);
					ApiClient.SaveToken(token);
					onSuccess(token);
				});
		}

		public static void UserUpdate(MonoBehaviour mono, User user, Action<User> onSuccess, Action<string, string> onError)
		{
			ApiClient.Put(mono, RestConfig.BaseUrl + "api/users/" + ApiClient.GetUserId(), JsonUtility.ToJson(user), null,
				delegate(UnityWebRequest www)
				{
					if (www.error != null)
					{
						onError(www.error, www.downloadHandler.text);
						return;
					}
					onSuccess(JsonUtility.FromJson<User>(www.downloadHandler.text));
                });
		}

	}
}
