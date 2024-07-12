using System;
using Model;
using UnityEngine;
using UnityEngine.Networking;

namespace RESTClient.Request
{
	public static class StoryRequest
	{
		public static void GetAllStory(MonoBehaviour mono, Action<Stories> onComplete, Action<string, string> onError)
		{
			ApiClient.Get(mono, RestConfig.BaseUrl+"api/stories/", null,
				delegate(UnityWebRequest www)
				{
					if (www.error != null)
					{
						onError(www.error, www.downloadHandler.text);
						return;
					}
					var jsonArray = "{\"stories\":" + www.downloadHandler.text + "}";
					onComplete(JsonUtility.FromJson<Stories>(jsonArray));
				});
		}

		public static void GetStoryById(MonoBehaviour mono,  string storyId, Action<Story> onComplete, Action<string, string> onError)
		{
			ApiClient.Get(mono, RestConfig.BaseUrl+"api/stories/" + storyId, null,
				delegate(UnityWebRequest www)
				{
					if (www.error != null)
					{
						onError(www.error, www.downloadHandler.text);
						return;
					}
					onComplete(JsonUtility.FromJson<Story>(www.downloadHandler.text));
				});
		}
	}
}
