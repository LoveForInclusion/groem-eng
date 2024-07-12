using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SceneLoader : MonoBehaviour {

	public GameObject loading;
	public GameObject panelLoad;
	AsyncOperation sceneAO = null;
	string idStoria;
	string libreria = "Libreria2";

	public static void deleteAllKey(){
		PlayerPrefs.DeleteKey("token");
		PlayerPrefs.DeleteKey("userId");
		PlayerPrefs.DeleteKey("Back");
		PlayerPrefs.DeleteKey("OfflineMode");
	}


	public void loadLogin(){
        if (SceneManager.GetActiveScene().name.Equals(libreria))
		{
            //GameObject.Find("Session").GetComponent<Session>().resetSession();
            deleteAllKey();
			PlayerPrefs.SetInt("logout", 1);
		}
		SceneManager.LoadScene ("LoginScene",LoadSceneMode.Single);


	}

	public void loadlettura(){

		caricaIdStoria();
		Debug.Log(idStoria);
		//Instanzio prefab con la selezione della modalità di lettura
		if (!idStoria.Equals("5a7c69bac85a450007226ed6"))
		{
			Transform canvas = GameObject.Find("CanvasScelta(Clone)").transform;
			var variableForPrefab = (GameObject)Resources.Load("SplitScreen", typeof(GameObject));
			Transform t = Instantiate(variableForPrefab, canvas).transform;
            t.GetComponent<RectTransform>().offsetMin = new Vector2(-100, -100);
            t.GetComponent<RectTransform>().offsetMax = new Vector2(100, 100);
			t.Find("Semplice").GetComponent<Button>().onClick.AddListener(delegate
			{//Imposto dalle opzioni audio il tipo di lettura desiderato.
				panelLoad.SetActive(true);
				OpzioniAudio opzioni = new OpzioniAudio();
				opzioni.caricaOpzioniAudio();
				opzioni.option.tipoLettura = 0;
				opzioni.salvaOpzioniAudio();
				StartCoroutine(LoadingSceneRealProgress("ScenaLettura"));

			});

			t.Find("Elaborata").GetComponent<Button>().onClick.AddListener(delegate
			{//Same here.
				panelLoad.SetActive(true);
				OpzioniAudio opzioni = new OpzioniAudio();
				opzioni.caricaOpzioniAudio();
				opzioni.option.tipoLettura = 1;
				opzioni.salvaOpzioniAudio();
				StartCoroutine(LoadingSceneRealProgress("ScenaLettura"));

			});
		} else {
			StartCoroutine(LoadingSceneRealProgress("ScenaLettura"));
		}

	}

	public void LoadNewLettura(){
		
		caricaIdStoria();
		Debug.Log(idStoria);
		//Instanzio prefab con la selezione della modalità di lettura
		if (!idStoria.Equals("5a7c69bac85a450007226ed6"))
		{
			Transform canvas = GameObject.Find("CanvasScelta(Clone)").transform;
			var variableForPrefab = (GameObject)Resources.Load("SplitScreen", typeof(GameObject));
			Transform t = Instantiate(variableForPrefab, canvas).transform;
            t.GetComponent<RectTransform>().offsetMin = new Vector2(-100, -100);
            t.GetComponent<RectTransform>().offsetMax = new Vector2(100, 100);
			t.Find("Semplice").GetComponent<Button>().onClick.AddListener(delegate
			{//Imposto dalle opzioni audio il tipo di lettura desiderato.
				panelLoad.SetActive(true);
				OpzioniAudio opzioni = new OpzioniAudio();
				opzioni.caricaOpzioniAudio();
				opzioni.option.tipoLettura = 0;
				opzioni.salvaOpzioniAudio();
				StartCoroutine(LoadingSceneRealProgress("ScenaLettura"));

			});

			t.Find("Elaborata").GetComponent<Button>().onClick.AddListener(delegate
			{//Same here.
				panelLoad.SetActive(true);
				OpzioniAudio opzioni = new OpzioniAudio();
				opzioni.caricaOpzioniAudio();
				opzioni.option.tipoLettura = 1;
				opzioni.salvaOpzioniAudio();
				StartCoroutine(LoadingSceneRealProgress("NewPlayer"));

			});
		} else {
			StartCoroutine(LoadingSceneRealProgress("NewPlayer"));
		}
	}

	private void Awake()
	{
		if (this.transform.name.Contains("CanvasScelta") && this.transform.GetComponent<ScalingManager>() == null)
			this.gameObject.AddComponent<ScalingManager>();
	}

	void Update(){
		if(sceneAO != null)
		if (!sceneAO.isDone) {
			loading.GetComponent<Image> ().fillAmount =sceneAO.progress;
			if (sceneAO.progress >= 0.9f) {
				loading.GetComponent<Image> ().fillAmount = 1;
				sceneAO.allowSceneActivation = true;
			}
		}
	}

	IEnumerator LoadingSceneRealProgress(string s){
		sceneAO = SceneManager.LoadSceneAsync (s);
		sceneAO.allowSceneActivation = false;
        yield return null;

	}

	public void loadlibreria(){
		SceneManager.LoadScene (libreria,LoadSceneMode.Single);
	}

	public void loadGioco(){		
		//non esiste ancora
		//SceneManager.LoadScene ("ScenaGiochi",LoadSceneMode.Single);
	}

	public void loadAudioRec(){
		panelLoad.SetActive (true);
		StartCoroutine (LoadingSceneRealProgress("AudioRec"));
		//SceneManager.LoadScene ("AudioRec",LoadSceneMode.Single);
	}

	public void loadTrueFalseGame(){
		panelLoad.SetActive (true);
		StartCoroutine (LoadingSceneRealProgress("TrueFalseGame"));
		//SceneManager.LoadScene ("TrueFalseGame",LoadSceneMode.Single);
	}

	public void loadMemoryGame(){
		panelLoad.SetActive (true);
		StartCoroutine (LoadingSceneRealProgress("MemoryGame"));
		//SceneManager.LoadScene ("MemoryGame",LoadSceneMode.Single);
	}

	public void loadComprensionGame(){
		panelLoad.SetActive (true);
		StartCoroutine (LoadingSceneRealProgress("ComprensionGame"));
		//SceneManager.LoadScene ("ComprensionGame",LoadSceneMode.Single);
	}

	public void loadCompletaFraseGame(){
		panelLoad.SetActive (true);
		StartCoroutine (LoadingSceneRealProgress("CompletaLaFraseGame"));
		//SceneManager.LoadScene ("CompletaLaFraseGame",LoadSceneMode.Single);
	}

    public void loadSequenceGame(){
        panelLoad.SetActive(true);
        StartCoroutine(LoadingSceneRealProgress("SequenceGame"));
    }

	public void loadScena(string scena){
		panelLoad.SetActive(true);
		StartCoroutine(LoadingSceneRealProgress(scena));
	}

	public void caricaIdStoria()
    {
        //carico la directory dei salvataggi
        string destinationUtente = Application.persistentDataPath + "/myIdStoria.dat";
        FileStream fileUtente;

        //se i file esistono li apre
        if (File.Exists(destinationUtente))
            fileUtente = File.OpenRead(destinationUtente);
        else
        {
            idStoria = null;
            return;
        }

        //leggo i file locali
        BinaryFormatter bf = new BinaryFormatter();

        idStoria = (string)bf.Deserialize(fileUtente);

        fileUtente.Close();
    }

}
