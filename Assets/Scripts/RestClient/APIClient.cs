using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Model;
using UnityEngine.Networking;

namespace RESTClient
{
    public static  class ApiClient
    {

        static string version = "Unity client v.0.0.7 - S.O: " + SystemInfo.operatingSystem;

        public static void Get(MonoBehaviour mono, string url, Action<float> onProgress, Action<UnityWebRequest> onComplete)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            www.timeout = 300;
			www.downloadHandler = new DownloadHandlerBuffer ();
			www.SetRequestHeader ("Content-Type", "application/json");
            www.SetRequestHeader ("User-Agent", version);

			www.chunkedTransfer = false;


            var token = RestoreToken();
            if (token != null)
				SetToken(token.token, www);
            mono.StartCoroutine(WaitForRequest(www, onProgress, onComplete));
        }

		public static void Get(MonoBehaviour mono, string url, string token, Action<float> onProgress, Action<UnityWebRequest> onComplete)
		{
			UnityWebRequest www = UnityWebRequest.Get(url);
			www.timeout = 300;
			www.downloadHandler = new DownloadHandlerBuffer();
			www.SetRequestHeader ("Content-Type", "application/json");
			www.SetRequestHeader ("User-Agent", version);

			www.chunkedTransfer = false;
			SetToken(token, www);
			mono.StartCoroutine(WaitForRequest(www, onProgress, onComplete));
		}

		public static void Get(MonoBehaviour mono, string url, string token, string path, Action<float> onProgress, Action<UnityWebRequest> onComplete)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            www.timeout = 300;
            www.downloadHandler = new DownloadHandlerFile(path);
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("User-Agent", version);

            www.chunkedTransfer = false;
            SetToken(token, www);
            mono.StartCoroutine(WaitForRequest(www, onProgress, onComplete));
        }

        public static void Post(MonoBehaviour mono, string url, string jsonData, Action<float> onProgress, Action<UnityWebRequest> onComplete)
        {
			UnityWebRequest www = new UnityWebRequest (url, "POST");
            www.timeout = 30;
            byte[] myData = System.Text.Encoding.UTF8.GetBytes(jsonData);
			www.uploadHandler = (UploadHandler) new UploadHandlerRaw(myData);
			www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
			www.SetRequestHeader ("Content-Type", "application/json");
			www.SetRequestHeader ("User-Agent", version);
			var token = RestoreToken();
			if (token != null)
				SetToken(token.token, www);
			mono.StartCoroutine(WaitForRequest(www, onProgress, onComplete));
        }

		public static void Post(MonoBehaviour mono, string url, string jsonData, string token, Action<float> onProgress, Action<UnityWebRequest> onComplete)
		{
			UnityWebRequest www = new UnityWebRequest (url, "POST");
			www.timeout = 30;
			byte[] myData = System.Text.Encoding.UTF8.GetBytes(jsonData);
			www.uploadHandler = (UploadHandler) new UploadHandlerRaw(myData);
			www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
			www.SetRequestHeader ("Content-Type", "application/json");
			www.SetRequestHeader ("User-Agent", version);
			SetToken(token, www);
			mono.StartCoroutine(WaitForRequest(www, onProgress, onComplete));
		}
			

		public static void Put(MonoBehaviour mono, string url, string jsonData, Action<float> onProgress, Action<UnityWebRequest> onComplete)
        {
			byte[] myData = System.Text.Encoding.UTF8.GetBytes(jsonData);
			UnityWebRequest www = UnityWebRequest.Put (url, myData);
            www.timeout = 30;
            www.SetRequestHeader ("Content-Type", "application/json");
			www.SetRequestHeader ("User-Agent", version);
			var token = RestoreToken();
			if (token != null)
				SetToken(token.token, www);
			mono.StartCoroutine(WaitForRequest(www, onProgress, onComplete));
		}

		private static IEnumerator WaitForRequest(UnityWebRequest www, Action<float> onProgress, Action<UnityWebRequest> onComplete)
		{
            /*var LastProgress = -2.0f;
            var PossibleTimeout = false;*/
			UnityWebRequestAsyncOperation asyncWWW = www.SendWebRequest ();
            while (!asyncWWW.isDone) {
                /*if (LastProgress < asyncWWW.progress)
                {
                    PossibleTimeout = false;
                    LastProgress = asyncWWW.progress;
                }
                else
                    PossibleTimeout = true;*/
				
                if (onProgress != null)
					onProgress (asyncWWW.progress);
                yield return null;
			}

            /*if (PossibleTimeout)
            {
                if (www.error != null)
                    Debug.Log("Timeout");
            }*/

            onComplete(www);
		}

        public static string GetUserId()
        {
            var token = RestoreToken();
            return token != null ? token.userId : null;
        }

        public static void SaveToken(Token token)
        {
            PlayerPrefs.SetString("token", token.token);
            PlayerPrefs.SetString("userId", token.userId);
        }

        private static Token RestoreToken()
        {
            var tokenValue = PlayerPrefs.GetString("token", null);
            var userId = PlayerPrefs.GetString("userId", null);
            if(tokenValue != null && userId != null)
                return new Token(userId, tokenValue);
            return null;
        }

		private static void SetToken(string token, UnityWebRequest www)
        {
				www.SetRequestHeader ("Authorization",token);
        }
    }
}