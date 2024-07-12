using System;
using Model;
using UnityEngine;
using UnityEngine.Networking;

namespace RESTClient.Request
{
	public static class TransactionRequest
	{

		public static void GetAllTransactions(MonoBehaviour mono, Action<Transactions> onComplete, Action<string, string> onError)
		{
			ApiClient.Get(mono, RestConfig.BaseUrl+"api/transactions/me", null,
				delegate(UnityWebRequest www)
				{
					//Debug.Log(www.downloadHandler.text);

					if (www.error != null)
					{
						onError(www.error, www.downloadHandler.text);
						return;
					}
					var jsonArray = "{\"transactions\":" + www.downloadHandler.text + "}";
					onComplete(JsonUtility.FromJson<Transactions>(jsonArray));
				});
		}

		public static void checkTransaction(MonoBehaviour mono,  string storyId, Action<Transaction> onComplete, Action<string, string> onError)
		{
			ApiClient.Get(mono, RestConfig.BaseUrl+"api/transactions/check" + storyId, null,
				delegate(UnityWebRequest www)
				{
					if (www.error != null)
					{
						onError(www.error, www.downloadHandler.text);
						return;
					}
					onComplete(JsonUtility.FromJson<Transaction>(www.downloadHandler.text));
				});
		}


		public static void createTransaction(MonoBehaviour mono, Transaction transaction, Action<Transaction> onComplete, Action<string, string> onError){
			ApiClient.Post(mono, RestConfig.BaseUrl + "api/transactions/", 
				JsonUtility.ToJson(transaction), 
				"eyJhbGciOiJIUzUxMiJ9.eyJyb2xlIjoiUk9MRV9VTklUWSJ9.x2W1_eEMnpM0L-i7xppW9S_s5u0t64f6aBYgC_A_ChGNQk-z0eupwohQgIqVaOKndWldt80j5ENsgKc78_Apjw",
				null,
				delegate(UnityWebRequest www)
				{
					if (www.error != null)
					{
						onError(www.error, www.downloadHandler.text);
						return;
					}
					onComplete(JsonUtility.FromJson<Transaction>(www.downloadHandler.text));
				});
		}

	}
}
