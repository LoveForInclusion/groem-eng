using System;
using Model;
using UnityEngine;
using UnityEngine.Networking;

namespace RESTClient.Request
{
	public static class CodeRequest
	{
		public static void GetRedeemed(MonoBehaviour mono, Action<Codes> onComplete, Action<string, string> onError)
		{
			ApiClient.Get(mono, RestConfig.BaseUrl+"api/codes/user/" + ApiClient.GetUserId() + "/redeemed", null,
				delegate(UnityWebRequest www)
				{
					if (www.error != null)
					{
						onError(www.error, www.downloadHandler.text);
						return;
					}
					var jsonArray = "{\"codes\":" + www.downloadHandler.text + "}";
					onComplete(JsonUtility.FromJson<Codes>(jsonArray));
				});
		}
	}
}

