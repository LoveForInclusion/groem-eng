using System;
using System.IO;
using Model;
using RESTClient;
using UnityEngine;
using UnityEngine.Networking;

namespace RESTClient.Request{
	public static class TokenIBMRequest{
		public static void getToken(MonoBehaviour mono,  Action<TokenIBM> onComplete, Action<string, string> onError)
		{
			ApiClient.Get(mono, RestConfig.BaseUrl+"api/files/gettoken" , null,
				delegate(UnityWebRequest www)
				{
					if (www.error != null)
					{
						onError(www.error, www.downloadHandler.text);
						return;
					}
					onComplete(JsonUtility.FromJson<TokenIBM>(www.downloadHandler.text));
				});
		}
	}
}
