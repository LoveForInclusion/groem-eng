using System;
using System.IO;
using Model;
using RESTClient;
using UnityEngine;
using UnityEngine.Networking;

namespace RESTClient.Request
{
	public static class FileRequest
	{
//		public static void DownloadFile(MonoBehaviour mono,string bucketname, string fileName, Action<float> onProgress, Action<String> onComplete, Action<string, string> onError)
//		{
//			ApiClient.Get(mono, RestConfig.BaseUrl+"api/files/bucket/" + bucketname + "/" + fileName,
//				delegate (float progress) 
//				{
//					onProgress(progress);
//
//				},
//				delegate(UnityWebRequest www)
//				{
//					if (www.error != null)
//					{
//						Debug.Log(www.error + "-----" + www.downloadHandler.text);
//						onError(www.error, www.downloadHandler.text);
//						return;
//					}
//					var fileData = www.downloadHandler.data;
//					File.WriteAllBytes (Application.persistentDataPath + "/" + fileName , fileData);
//					onComplete(Application.persistentDataPath + "/" + fileName);
//				});
//		}

		public static void DownloadFile2(MonoBehaviour mono,string bucketname, string fileName, Action<float> onProgress, Action<String> onComplete, Action<string, string> onError)
		{

			TokenIBMRequest.getToken (mono, 
				delegate(TokenIBM token) {

				ApiClient.Get(mono, "https://s3.eu-gb.objectstorage.softlayer.net/" + bucketname + "/" + fileName, "bearer " + token.access_token,
						delegate (float progress) 
						{
							onProgress(progress);

						},
						delegate(UnityWebRequest www)
						{
							if (www.error != null)
							{
								Debug.Log(www.error + "-----" + www.downloadHandler.text);
								onError(www.error, www.downloadHandler.text);
								return;
							}
                            
							var fileData = www.downloadHandler.data;
							File.WriteAllBytes (Application.persistentDataPath + "/" + fileName , fileData);
							onComplete(Application.persistentDataPath + "/" + fileName);
						});

				}, 
				delegate(string s1, string s2) {
					Debug.Log("Error " + s1 + " " + s2);
				});

		}

		public static void DownloadFileBundle(MonoBehaviour mono, string bucketname, string fileName, Action<float> onProgress, Action<String> onComplete, Action<string, string> onError)
        {

            TokenIBMRequest.getToken(mono,
                delegate (TokenIBM token) {

                    ApiClient.Get(mono, "https://s3.eu-gb.objectstorage.softlayer.net/" + bucketname + "/" + fileName, "bearer " + token.access_token, Application.persistentDataPath + "/" + fileName,
                            delegate (float progress)
                            {
                                onProgress(progress);

                            },
                            delegate (UnityWebRequest www)
                            {
                                if (www.error != null)
                                {
                                    Debug.Log(www.error + "-----" + www.downloadHandler.text);
                                    onError(www.error, www.downloadHandler.text);
                                    return;
                                }
                            //Mod file buffer
                            //var fileData = www.downloadHandler.data;
                            //File.WriteAllBytes (Application.persistentDataPath + "/" + fileName , fileData);
                            onComplete(Application.persistentDataPath + "/" + fileName);
                            });

                },
                delegate (string s1, string s2) {
                    Debug.Log("Error " + s1 + " " + s2);
                });

        }

	}
}