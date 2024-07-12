using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using System.Threading;
using System.Net;

public class LoadObjectFromBundle : MonoBehaviour {

	WWW www;
	bool onetime;
	public String NameObject;
	public String Subject;
	string idStoria;
	int version;

	public Image load;

	public bool webgl;


	AssetBundleRequest assetLoadRequest = null;
	AssetBundleCreateRequest bundleLoadRequest = null;

	private static bool created = false;

	void Awake()
	{
		if (SceneManager.GetActiveScene ().name.Equals ("ScenaLettura")) {
			if (!created) {
				DontDestroyOnLoad (this.gameObject);
				DontDestroyOnLoad (load.transform.parent.parent.gameObject);
				created = true;
			} else {
				Destroy (this.gameObject);
				Destroy (load.transform.parent.parent.gameObject);
			}
		}
	}

	// Use this for initialization
	void Start () {

		version = 1;

		if(webgl)
			StartCoroutine("loadWebGL");

		else{
			caricalibreria ();
			//StartCoroutine ("loadObject");
			StartCoroutine ("loadAsyncronius");
			//loadObject ();
		}

    }

	void Update(){
		if (load != null) {
			if (bundleLoadRequest != null) {
				load.fillAmount = bundleLoadRequest.progress/2;
				if (assetLoadRequest != null) {
					load.fillAmount = bundleLoadRequest.progress/2 + assetLoadRequest.progress/2;
				}
			}
			else if (webgl) {
				
					load.fillAmount = 0.9f;

			}
			else if (www != null && load!= null) {
				load.fillAmount = www.progress/2;
				if (assetLoadRequest != null) {
					load.fillAmount = www.progress/2 + assetLoadRequest.progress/2;
				}
			}
		}
		if (SceneManager.GetActiveScene ().name.Equals ("ScenaLettura")) {
			if (load != null && load.fillAmount >= 0.9f && load.transform.parent.parent.gameObject.activeSelf) {


				load.transform.parent.parent.gameObject.SetActive (false);
			
			}
		}
			
	}

	public void destroyMe(){
		load.transform.parent.parent.gameObject.SetActive (true);
		Destroy (load.transform.parent.parent.gameObject);
		created = false;
		Destroy (this.gameObject);
	}

	public void caricalibreria(){
		//carico la directory dei salvataggi
		string destinationUtente = Application.persistentDataPath +"/myIdStoria.dat";
		FileStream fileUtente;

		//se i file esistono li apre
		if (File.Exists (destinationUtente))
			fileUtente = File.OpenRead (destinationUtente);
		else {
			idStoria = null;
			return;
		}

		//leggo i file locali
		BinaryFormatter bf = new BinaryFormatter ();

		idStoria = (string)bf.Deserialize (fileUtente);

		fileUtente.Close ();
	}


	IEnumerator loadWebGL(){
		
		yield return new WaitForSeconds(.5f);

		if (Caching.ClearCache ())
			Debug.Log ("Cache Svuotata");
		else
			Debug.Log ("Cache in uso");

		yield return new WaitForSeconds(.5f);

		//string u = Application.absoluteURL;
		string u = "http://testtadawebgl2.altervista.org/index.html";
		Debug.Log ("Prima del remove:"+u);
		u = u.Replace ("index.html","WebGL/");
		Debug.Log ("Dopo il remove:"+u);

		www = WWW.LoadFromCacheOrDownload (u + Subject,version);
		yield return www;
		Debug.Log ("caricamento eseguito");

		AssetBundle bundle = www.assetBundle;
		if (bundle == null)
			Debug.Log ("sono null");
		assetLoadRequest = bundle.LoadAssetAsync<GameObject>(NameObject);
		yield return assetLoadRequest;
		Debug.Log ("carico");

		GameObject cube = assetLoadRequest.asset as GameObject;

		Instantiate(cube);
		Debug.Log ("instanzio");

		//Pulisco dalla cache il bundle scaricato.
		www.Dispose();
		bundle.Unload (false);

		version++;
	}


	IEnumerator loadObject()
    {
		//upload/download/5a2fa82346e0fb000150e485/lettura.0
		//www = WWW.LoadFromCacheOrDownload("http://52.233.152.195:8081/upload/download/5a2fa82346e0fb000150e485/lettura.0", 1);


		//funzionante
		//www = new WWW("file:///C:/Users/Ciro%20De%20Martino/Desktop/Unity%20Project/tada/Assets/AssetBundles/test.zip");
		//yield return www;
		//

		//test effettivo tramite swagger
		/*WWWForm form = new WWWForm();
		Dictionary<string, string> postHeader = form.headers;

		if (postHeader.ContainsKey("container"))
			postHeader["container"] =  "595cf9d146e0fb0001c79dcb";
		else
			postHeader.Add("container",  "595cf9d146e0fb0001c79dcb");

		if (postHeader.ContainsKey("filename"))
			postHeader["filename"] =  "unityBundle.zip";
		else
			postHeader.Add("filename",  "unityBundle.zip");

		//esguo la richiesta
		www = new WWW("http://api.tadabook.it:8081/api/upload/download/595cf9d146e0fb0001c79dcb/unityBundle.zip");

		yield return www; // aspetto i dati
		if (www.error != null) {
			Debug.Log("Errore: " + www.error);
		} else {
			Debug.Log("Risultato: " + www.text);
		}
	
		t.text = Application.persistentDataPath;
		Debug.Log (Application.persistentDataPath);

		byte[] fileScaricato = www.bytes;

		string docPath = Application.dataPath;
		docPath = docPath.Substring(0, docPath.Length - 5);
		docPath = docPath.Substring(0, docPath.LastIndexOf("/"));
		docPath += "/Documents/test.zip";
		t.text = "docPath=" + docPath;
		Debug.Log("docPath=" + docPath);

		//System.IO.File.WriteAllBytes(Application.persistentDataPath, fileScaricato);

		t.text = "0.10";
		File.WriteAllBytes (Application.persistentDataPath+"/test.zip" , www.bytes);
		t.text = "0.33";

		ZipUtil.Unzip (Application.persistentDataPath+"/test.zip" , Application.persistentDataPath + "/dir");
		t.text = "0.66";

		File.Delete (Application.persistentDataPath+"/test.zip");
*/

		string pathName = "";
		if (SystemInfo.operatingSystem.Contains ("iOS") || SystemInfo.operatingSystem.Contains ("Mac")) {
			pathName = "Ios";
		} else
			pathName = "Android";
	/*	
		AssetBundle assets = AssetBundle.LoadFromFile ( Application.persistentDataPath + "/dir/" + idStoria +"/"+ pathName+ "/lettura."+number);
		GameObject c = assets.LoadAsset<GameObject> (s);
		Instantiate (c);
		assets.Unload (false);
	*/

		yield return new WaitForSeconds(.5f);

		www = WWW.LoadFromCacheOrDownload ("file://" + Application.persistentDataPath + "/dir/" + idStoria +"/"+ pathName+ "/lettura."+Subject,1);
		yield return www;
		Debug.Log ("caricamento eseguito");

		AssetBundle bundle = www.assetBundle;
		if (bundle == null)
			Debug.Log ("sono null");
		AssetBundleRequest request = bundle.LoadAssetAsync<GameObject>(NameObject);
		yield return request;
		Debug.Log ("carico");

		GameObject cube = request.asset as GameObject;

		Instantiate(cube);
		Debug.Log ("instanzio");

		//Pulisco dalla cache il bundle scaricato.
		www.Dispose();
		bundle.Unload (false);

		
		/*FileDownloader.downloadfromURL ("file:///C:/Users/Ciro%20De%20Martino/Desktop/Unity%20Project/tada/Assets/AssetBundles/test.zip",
			Application.persistentDataPath);
		Debug.Log("download terminato "+ Application.persistentDataPath);*/


        /*AssetBundle bundle = www.assetBundle;
        AssetBundleRequest request = bundle.LoadAssetAsync<GameObject>("Canvas");
        yield return request;

        cube = request.asset as GameObject;

        Instantiate<GameObject>(cube);*/
    }

	/*public IEnumerator LoadAssets(string bundleName)
	{


		this.assetsByBundle[bundleName] = new Dictionary<string, GameObject>();

		AssetBundle assetBundle = assetBundles[bundleName];
		AssetBundleRequest assetBundleRequest = assetBundle.LoadAllAssetsAsync<GameObject>();
		yield return assetBundleRequest;
		Object[] objects = assetBundleRequest.allAssets;
		foreach (GameObject gameObject in objects)
		{
			this.assetsByBundle[bundleName][gameObject.name] = this.assets[gameObject.name] = gameObject;
		}


	}*/

	IEnumerator loadAsyncronius()
	{

		string pathName = "";
		if (SystemInfo.operatingSystem.Contains ("iOS") || SystemInfo.operatingSystem.Contains ("Mac")) {
			pathName = "Ios";
		} else
			pathName = "Android";


	//carico i file che mi occorrono per il bundle principale
	AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.persistentDataPath + "/dir/" + idStoria +"/"+ pathName+ "/" + pathName);
        AssetBundleManifest manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        string[] dependencies = manifest.GetAllDependencies(Subject);
        foreach (string depe in dependencies)
        {
		
        AssetBundle bundlesharedLoadRequest = AssetBundle.LoadFromFile(Application.persistentDataPath + "/dir/" + idStoria +"/"+ pathName + "/" + depe);
        var myLoadedAssetBundleShared = bundlesharedLoadRequest;

        myLoadedAssetBundleShared.LoadAllAssets<GameObject>();
        }





	//carico e instanzio il bundle principale
		bundleLoadRequest = AssetBundle.LoadFromFileAsync(Application.persistentDataPath + "/dir/" + idStoria +"/"+ pathName + "/"+Subject);
		yield return bundleLoadRequest;

		var myLoadedAssetBundle = bundleLoadRequest.assetBundle;
		if (myLoadedAssetBundle == null)
		{
			Debug.Log("Failed to load AssetBundle!");
			yield break;
		}

		assetLoadRequest = myLoadedAssetBundle.LoadAssetAsync<GameObject>(NameObject);
		yield return assetLoadRequest;

		GameObject prefab = assetLoadRequest.asset as GameObject;
		Instantiate(prefab);

		Debug.Log("im here2");
		myLoadedAssetBundle.Unload(false);
		assetBundle.Unload(false);
		AssetBundle.UnloadAllAssetBundles(false);
	}
		
}
