using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Model;
using RESTClient.Request;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;

public class DownloadOrPlay : MonoBehaviour
{
	void HandleAction(string obj)
	{
	}


	WWW www;
	public GameObject panel;
	public Slider slider;
	public Text text;
	public Text istalling;
	public GameObject dontTouch;
	public Codes codiciStoriaLocale = null;
	public Stories storiesLocal = null;
	public Stories storiesOnline = null;
	public Transactions transactionsLocal = null;
	public GameObject libreria;
	public GameObject ripianoLibreria;
	public Sprite buy;
	public Sprite downl;
	public Sprite comingSoon;
	public GameObject panelloDownload;
	public GameObject pannelloAggiornamento;

	public GameObject caricamentoInCorsoText;

	public Sprite scaricaButton;
	public Sprite compraButton;

	string idStoryToDownloadOrBuy;

	float progressDownload = 0f;
	float progressUnzip = 0f;

	bool verifiedFile = false;
	bool loaded = false;
	bool checkedTrans = false;

	public GameObject panelError;
	public Text testoErrore;


	public Transform contenitoreGiochi;
	public GameObject gameSpec;
	public Sprite comprensione;
	public Sprite svago;

	//public PurchaseEngine iapEngine;
	//utility bool
	public bool sliderDone = true;

	[Header("ALERT: in fase di build entrambe \n le spunte devono essere false")]

	[Tooltip("Se spuntata il filename di riferimento diventa iosBundleDev.zip")]
	public bool TestIOSBundle;
	[Tooltip("Se spuntata il filename di riferimento diventa androidBundleDev.zip")]
	public bool TestAndroidBundle;


	public void salvaIdStoria (string stringa)
	{

		//dichiaro le directory 
		string destinationToken = Application.persistentDataPath + "/myIdStoria.dat";
		FileStream fileToken = null;

		//se i file esistono li apre
		if (File.Exists (destinationToken))
			fileToken = File.OpenWrite (destinationToken);
		else
			fileToken = File.Create (destinationToken);

		//salvo i file in locale
		BinaryFormatter bf = new BinaryFormatter ();

		bf.Serialize (fileToken, stringa);
		fileToken.Close ();
	}


	public void salvalibreria (Stories stories)
	{
		
		//dichiaro le directory 
		string destinationToken = Application.persistentDataPath + "/myLibrary.dat";
		FileStream fileToken = null;

		//se i file esistono li apre
		if (File.Exists (destinationToken))
			fileToken = File.OpenWrite (destinationToken);
		else
			fileToken = File.Create (destinationToken);

		//salvo i file in locale
		BinaryFormatter bf = new BinaryFormatter ();

		bf.Serialize (fileToken, stories);
		fileToken.Close ();
	}

	public void salvaReedemed (Codes codes)
	{

		//dichiaro le directory 
		string destinationToken = Application.persistentDataPath + "/myPurchase.dat";
		FileStream fileToken = null;

		//se i file esistono li apre
		if (File.Exists (destinationToken))
			fileToken = File.OpenWrite (destinationToken);
		else
			fileToken = File.Create (destinationToken);

		//salvo i file in locale
		BinaryFormatter bf = new BinaryFormatter ();

		bf.Serialize (fileToken, codes);
		fileToken.Close ();
	}

	public void salvaTransaction (Transactions transaction)
	{

		//dichiaro le directory 
		string destinationToken = Application.persistentDataPath + "/myTransaction.dat";
		FileStream fileToken = null;

		//se i file esistono li apre
		if (File.Exists (destinationToken))
			fileToken = File.OpenWrite (destinationToken);
		else
			fileToken = File.Create (destinationToken);

		//salvo i file in locale
		BinaryFormatter bf = new BinaryFormatter ();

		bf.Serialize (fileToken, transaction);
		fileToken.Close ();
	}

	public void caricaRedeemed ()
	{
		//carico la directory dei salvataggi
		string destinationUtente = Application.persistentDataPath + "/myPurchase.dat";
		FileStream fileUtente;

		//se i file esistono li apre
		if (File.Exists (destinationUtente))
			fileUtente = File.OpenRead (destinationUtente);
		else {
			codiciStoriaLocale = null;
			return;
		}

		//leggo i file locali
		BinaryFormatter bf = new BinaryFormatter ();

		codiciStoriaLocale = (Codes)bf.Deserialize (fileUtente);

		fileUtente.Close ();
	}

	public void caricaTransaction ()
	{
		//carico la directory dei salvataggi
		string destinationUtente = Application.persistentDataPath + "/myTransaction.dat";
		FileStream fileUtente;

		//se i file esistono li apre
		if (File.Exists (destinationUtente))
			fileUtente = File.OpenRead (destinationUtente);
		else {
			codiciStoriaLocale = null;
			return;
		}

		//leggo i file locali
		BinaryFormatter bf = new BinaryFormatter ();

		transactionsLocal = (Transactions)bf.Deserialize (fileUtente);

		fileUtente.Close ();
	}

	//salvare solo il file contenente le storie che scarico così da facilitare il confronto con il database online preso con le richieste

	public void caricalibreria ()
	{
		//carico la directory dei salvataggi
		string destinationUtente = Application.persistentDataPath + "/myLibrary.dat";
		FileStream fileUtente;

		//se i file esistono li apre
		if (File.Exists (destinationUtente))
			fileUtente = File.OpenRead (destinationUtente);
		else {
			storiesLocal = null;
			return;
		}

		//leggo i file locali
		BinaryFormatter bf = new BinaryFormatter ();

		storiesLocal = (Stories)bf.Deserialize (fileUtente);

		fileUtente.Close ();
	}

	public Texture2D LoadTexture (string FilePath)
	{

		// Load a PNG or JPG file from disk to a Texture2D
		// Returns null if load fails

		Texture2D Tex2D;
		byte[] FileData;

		if (File.Exists (FilePath)) {
			FileData = File.ReadAllBytes (FilePath);
			Tex2D = new Texture2D (2, 2);           
			if (Tex2D.LoadImage (FileData))
				return Tex2D;            
		}  
		return null;                
	}
		

	void downloadFile ()
	{				
		panelloDownload.SetActive (false);
		slider.gameObject.SetActive (true);
		istalling.gameObject.SetActive (true);
		istalling.text = "Download in corso...";

		string fileName = "";

		//Child non viene trascinato in altri contesti quindi devo salvarmi il prezzo somewhere
		float price = 0f;

		foreach (var child in storiesLocal.stories) {
			if (idStoryToDownloadOrBuy.Equals (child.id)) {
				if (SystemInfo.operatingSystem.Contains ("iOS") || SystemInfo.operatingSystem.Contains("Mac")) {
					fileName = child.iosData;
				} else
					fileName = child.androidData;
				price = child.price;
			}
		}

		if (TestIOSBundle)
			fileName = "iosBundleDev.zip";
		if (TestAndroidBundle)
			fileName = "androidBundleDev.zip";


		Debug.Log("Inizio Download");
		sliderDone = false;
		FileRequest.DownloadFileBundle (this, "tada-" + idStoryToDownloadOrBuy, fileName, delegate(float p) {

			progressDownload = p;
			//Debug.Log(progressDownload);

		}, delegate {//Delegate On Complete DownloadFile
			
			Debug.Log ("Download complete");

			if (price == 0f && !CheckTransaction(idStoryToDownloadOrBuy))
            {
				sliderDone = true;
				Transaction trans = new Transaction(null, idStoryToDownloadOrBuy, PlayerPrefs.GetString("userId"), null, null, "mobile", SystemInfo.operatingSystem, 0f, 0f, 0f, 0, 0, false, false);
                transactionsLocal.transactions.Add(trans);
                TransactionRequest.createTransaction(this, trans,
                    delegate (Transaction obj) {
                        Debug.Log("Transazione Registrata: " + obj.id);
                        salvaTransaction(transactionsLocal);
						Session.setupExtraction(idStoryToDownloadOrBuy, fileName);
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    },
                    delegate (string arg1, string arg2) {
                        Debug.Log("Errore in creazione transazione: " + arg1 + " : " + arg2);
                    }
                );
            }
            else
            {
                Debug.Log("Transazione Esistente");
				Session.setupExtraction(idStoryToDownloadOrBuy, fileName);
				Caching.ClearCache();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

			//Debug.Log("Inizio Unzip");

            //ZipUtil.Unzip (Application.persistentDataPath + "/" + fileName, Application.persistentDataPath + "/dir/" + idStoryToDownloadOrBuy);

            //MioZip.NewUnzipWindows(this,Application.persistentDataPath + "/" + fileName, Application.persistentDataPath + "/dir/TestWindows");

            //MioZip.UnZipWindowsMerda(Application.persistentDataPath + "/dir/TestWindows", File.ReadAllBytes(Application.persistentDataPath + "/" + fileName));



        }, delegate(string error, string errortext) {//Delegate OnError Download File
			testoErrore.text = "Download fallito riprovare più tardi";
			panelError.SetActive (true);
			progressDownload = 0f;
			slider.transform.gameObject.SetActive (false);
			istalling.gameObject.SetActive (false);
			Debug.Log (error);
		});  //Fine DownloadFile
	}

	public void svuotaGochini(){
		foreach (Transform child in contenitoreGiochi) {
			Destroy (child.gameObject);
		}
	}

	void setDownloadOrBuyButton ()
	{
		idStoryToDownloadOrBuy = EventSystem.current.currentSelectedGameObject.name;
		Story st = null;
		foreach(Story storia in storiesLocal.stories){
			if (storia.id.Equals (idStoryToDownloadOrBuy)) {
				st = storia;
			}
		}

		Text desc = panelloDownload.transform.Find ("InfoStoria").transform.Find ("Scroll View DescrizioneStoria").transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
		Image img = panelloDownload.transform.Find ("InfoStoria").transform.Find ("ImageStoryMiniature").GetComponent<Image> ();
		desc.text = st.description;
		panelloDownload.transform.Find ("InfoStoria").transform.Find ("Specifiche2").GetComponent<Text> ().text = st.age;

		foreach (var giochi in st.activities) {
			if (giochi.type.Equals ("svago"))
				gameSpec.transform.Find ("ImageStoryMiniature").GetComponent<Image> ().sprite = svago;
			else if (giochi.type.Equals ("cognitivo"))
				gameSpec.transform.Find ("ImageStoryMiniature").GetComponent<Image> ().sprite = comprensione;

			gameSpec.transform.Find ("Specifiche2").GetComponent<Text> ().text = giochi.name;
			gameSpec.transform.Find ("Specifiche1").GetComponent<Text> ().text = giochi.description;

			Instantiate (gameSpec , contenitoreGiochi);
		}

		Texture2D spr = LoadTexture (Application.persistentDataPath + "/" + st.shelfCover);
		Sprite s = Sprite.Create (spr, new Rect (0, 0, spr.width, spr.height), new Vector2 (0, 0), 100f);
		img.sprite = s;

		//Debug.Log (EventSystem.current.currentSelectedGameObject.name);
		if (EventSystem.current.currentSelectedGameObject.transform.GetChild (0).GetComponent<Image> ().sprite.name.Equals ("CardDownload")) {
			panelloDownload.transform.GetChild (0).Find ("ButtonScaricaCompra").GetComponent<Image> ().sprite = scaricaButton;
			//Debug.Log("Rimuovo Listener");
			panelloDownload.transform.GetChild(0).Find("ButtonScaricaCompra").GetComponent<Button>().onClick.RemoveListener(downloadFile);
			panelloDownload.transform.GetChild(0).Find("ButtonScaricaCompra").GetComponent<Button>().onClick.RemoveListener(buyButton);
			panelloDownload.transform.GetChild (0).Find ("ButtonScaricaCompra").GetComponent<Button> ().onClick.AddListener (downloadFile);
		} else if (EventSystem.current.currentSelectedGameObject.transform.GetChild (0).GetComponent<Image> ().sprite.name.Equals ("CardAcquista")) {
			panelloDownload.transform.GetChild (0).Find ("ButtonScaricaCompra").GetComponent<Image> ().sprite = compraButton;
            //Fare in modo che il pulsante precedente trascina l'ID storia fino a qui
			panelloDownload.transform.GetChild(0).Find("ButtonScaricaCompra").GetComponent<Button>().onClick.RemoveListener(downloadFile);
            panelloDownload.transform.GetChild(0).Find("ButtonScaricaCompra").GetComponent<Button>().onClick.RemoveListener(buyButton);
			panelloDownload.transform.GetChild (0).Find ("ButtonScaricaCompra").GetComponent<Button> ().onClick.AddListener (buyButton);

			//funzione compra

		}
		panelloDownload.SetActive (true);

	}

	public void generaLibreria ()
	{
		caricalibreria ();
		caricaRedeemed ();

		//Controlla ambiente
		string pathName = "";
		if (SystemInfo.operatingSystem.Contains ("iOS") || SystemInfo.operatingSystem.Contains ("Mac")) {
			pathName = "Ios";
		} else
			pathName = "Android";
		//Fine Controlla Ambiente

		int i = 0;
		GameObject rip = null;
		List<GameObject> ripianiDaOrdinare = new List<GameObject> ();
		foreach (var story in storiesLocal.stories) {

			if (i % 3 == 0) {
				rip = Instantiate (ripianoLibreria);
				ripianiDaOrdinare.Add (rip);

			}

			Texture2D img = LoadTexture (Application.persistentDataPath + "/" + story.shelfCover);
			Sprite s = Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0), 100f);
			rip.transform.GetChild (i % 3).GetComponent<Image> ().sprite = s;
			rip.transform.GetChild (i % 3).gameObject.SetActive (true);
			rip.transform.GetChild (i % 3).name = story.id;


			//Se è coming soon blocca
			if (story.comingSoon) {
				rip.transform.GetChild (i % 3).GetChild (0).GetComponent<Image> ().sprite = comingSoon;
				//Se non è coming soon allora...
			} else if(story.price > 0f){
				//Se è stata scaricata ed è stata pagata

				//Debug.Log (Application.persistentDataPath + "/dir/" + story.id);
				if ((Directory.Exists (Application.persistentDataPath + "/dir/" + story.id)) && CheckTransaction (story.id)) {

					rip.transform.GetChild (i % 3).GetChild (0).gameObject.SetActive (false);
					rip.transform.GetChild (i % 3).GetComponent<Button> ().onClick.AddListener (buttonPressed);
					//Se è stata scaricata ma non è stata pagata o viceversa
				} else {

					//Se non è stata pagata
					if (!CheckTransaction (story.id)) {
						
						rip.transform.GetChild (i % 3).GetChild (0).GetComponent<Image> ().sprite = buy;

						//Cambiare buyButton con ErrorPanel e viceversa
						rip.transform.GetChild (i % 3).GetComponent<Button> ().onClick.AddListener (setDownloadOrBuyButton);
					} 
					//Se è stata pagata
					else if (CheckTransaction (story.id)) {
						rip.transform.GetChild (i % 3).GetChild (0).GetComponent<Image> ().sprite = downl;
						rip.transform.GetChild (i % 3).GetComponent<Button> ().onClick.AddListener (setDownloadOrBuyButton);
					} 
					else
						Debug.Log ("Logica errata rivedere ordine condizioni");
				}
			}
			//Se è gratis
			else{
				//Se è in locale
				if (Directory.Exists (Application.persistentDataPath + "/dir/" + story.id)) {
					rip.transform.GetChild (i % 3).GetChild (0).gameObject.SetActive (false);
					rip.transform.GetChild (i % 3).GetComponent<Button> ().onClick.AddListener (buttonPressed);
				}
				//Se non è in locale
				else {
					rip.transform.GetChild (i % 3).GetChild (0).GetComponent<Image> ().sprite = downl;
					rip.transform.GetChild (i % 3).GetComponent<Button> ().onClick.AddListener (setDownloadOrBuyButton);
				}
			}

			i++;
		}

		libreria.transform.parent.GetComponent<VerticalScrollSnap> ().StartingScreen = ripianiDaOrdinare.Count - 1;
		ripianiDaOrdinare.Reverse ();
		foreach (var child in ripianiDaOrdinare) {
			libreria.transform.parent.GetComponent<VerticalScrollSnap> ().AddChild (child);
		}


	}

	void Awake ()
	{

		Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if (!GameObject.Find("Session").GetComponent<Session>().sessionLoaded)
        {

            //scarico acquisti fatti
            CodeRequest.GetRedeemed(this, delegate (Codes codes)
            {
                salvaReedemed(codes);
            }, delegate (string code, string error)
            {
                Debug.Log(code + "---" + error);
            });

            TransactionRequest.GetAllTransactions(this,
                delegate (Transactions transactions)
                {
                    this.transactionsLocal = transactions;
                    checkedTrans = true;
                    salvaTransaction(transactionsLocal);
                },
                delegate (string code, string error)
                {
                    caricaTransaction();
                    Debug.Log(code + "---" + error);
                }
            );

            //get all story
            caricalibreria();

            StoryRequest.GetAllStory(this,
                delegate (Stories stories)
                {

                    var v = JsonUtility.ToJson(stories);

                    storiesOnline = stories;

                    Stories toDestroy = JsonUtility.FromJson<Stories>(v);

                    downloadSchelfs(stories);

                    if (storiesLocal == null)
                    {
                        storiesLocal = stories;
                        salvalibreria(stories);
                    }
                    else
                    {
                        for (int i = 0; i < toDestroy.stories.Count; i++)
                        {
                            if (storiesLocal.stories.Count < i)
                                storiesLocal.stories.Add(toDestroy.stories[i]);
                            else
                            {
                                long lu = storiesLocal.stories[i].lastUpdate;
                                storiesLocal.stories[i] = toDestroy.stories[i];
                                storiesLocal.stories[i].lastUpdate = lu;
                            }
                        }
                        salvalibreria(storiesLocal);

                    }



                },
                delegate (string code, string error)
                {
                    Debug.Log(code + "---" + error);
                    if (File.Exists(Application.persistentDataPath + "/myLibrary.dat"))
                    {
                        generaLibreria();
                        caricamentoInCorsoText.SetActive(false);
                    }
                });
            GameObject.Find("Session").GetComponent<Session>().sessionLoaded = true;
        }
        else
        {
            Debug.Log("Carico transaction");
            caricaTransaction();
            checkedTrans = true;
            Debug.Log("Carico Libreria");
            caricalibreria();
            Debug.Log("Carico reedemed");
        }

	}

	public void downloadSchelfs (Stories s)
	{
		foreach (var story in s.stories) {
			//scarico immagini
			if (!File.Exists (Application.persistentDataPath + "/" + story.shelfCover)) {
				FileRequest.DownloadFile2 (this, "tada-" + story.id, story.shelfCover, delegate {
				}, delegate {
					Debug.Log ("Download success");
				}, delegate {
					Debug.Log ("Download error");
				});
			}
		}
	}

	void Start ()
	{
		caricaRedeemed ();

		if (Session.extraction)
		{
			Debug.Log(Session.zipPath() + " - " + Session.dirPath());
			istalling.gameObject.SetActive(true);
			istalling.text = "Installazione in corso...";
			sliderDone = false;
			MioZip.NewUnzip(this, (Session.zipPath()), (Session.dirPath()),
							delegate (float p)//Delegate OnProgress NewUnzip
							{

								progressDownload = p;
								//Debug.Log(progressDownload);

							}, delegate (string s)//Delegate OnComplete NewUnzip;
							{
								//this.transform.gameObject.GetComponent<MioZip>().path = Application.persistentDataPath + "/" + fileName;
								//this.transform.gameObject.GetComponent<MioZip>().outFolder = Application.persistentDataPath + "/dir/" + idStoryToDownloadOrBuy;
								//StartCoroutine(this.transform.gameObject.GetComponent<MioZip>().ExtractZipFile());

				                sliderDone = true;
								Debug.Log("Unzipping complete");
								Debug.Log(Session.zipPath());
								File.Delete(Session.zipPath());

								slider.value = 100f;
								dontTouch.SetActive(false);
								slider.gameObject.SetActive(false);
								text.gameObject.SetActive(false);
								istalling.gameObject.SetActive(false);
								progressDownload = 0f;
								Session.extraction = false;
								Session.resetExtraction();


								foreach (Transform child in libreria.transform)
								{
									foreach (Transform nepiu in child)
									{
										if (nepiu.name.Equals(idStoryToDownloadOrBuy))
										{
											nepiu.gameObject.GetComponent<Button>().onClick.AddListener(buttonPressed);
											nepiu.GetChild(0).gameObject.SetActive(false);
										}
									}
								}

								foreach (var child in storiesLocal.stories)
								{
									if (child.id.Equals(idStoryToDownloadOrBuy))
									{
										foreach (var child2 in storiesOnline.stories)
										{
											if (child.id.Equals(child2.id))
											{
												child.lastUpdate = child2.lastUpdate;
												salvalibreria(storiesLocal);
											}
										}

									}
								}

								foreach (Transform child in libreria.transform)
								{
									foreach (Transform nepiu in child)
									{
										if (nepiu.name.Equals(idStoryToDownloadOrBuy))
										{

											nepiu.GetComponent<Animator>().Play("shake");
										}
									}
								}
				            
								SceneManager.LoadScene(SceneManager.GetActiveScene().name);
							});
			//Fine NewUnzip*/
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		if (storiesLocal != null && checkedTrans && !verifiedFile && !loaded) {
			verifiedFile = true;
			foreach (var story in storiesLocal.stories) {
				caricamentoInCorsoText.SetActive (true);
				verifiedFile = (verifiedFile && File.Exists (Application.persistentDataPath + "/" + story.shelfCover));
				
			}
		}
		
		if (verifiedFile && !loaded) {
			generaLibreria ();
			loaded = true;
			caricamentoInCorsoText.SetActive (false);
		}


		if (progressDownload != 0f && progressDownload < 1f && !sliderDone) {
			slider.gameObject.SetActive (true);
			//istalling.gameObject.SetActive (false);
			slider.value = progressDownload * 100;
			text.text = (Mathf.RoundToInt (progressDownload * 100)) + "%";

		} else {

			slider.gameObject.SetActive (false);
			text.text = "";
		}
	}

	public void refreshLibrary ()
	{
		//eliminare la libreria e ricreala
		foreach (Transform v in libreria.transform)
			Destroy (v.gameObject);
		GameObject[] a;
		libreria.transform.parent.GetComponent<VerticalScrollSnap> ().RemoveAllChildren (out a);
		generaLibreria ();
	}

	public void buttonPressed ()
	{
		panelloDownload.SetActive (false);
		
		salvaIdStoria (EventSystem.current.currentSelectedGameObject.name);


		/*
		string pathName = "";

		foreach (var child in storiesLocal.stories) {
			if (idStoryToDownloadOrBuy.Equals (child.id)) {
				if (SystemInfo.operatingSystem.Contains ("iOs")) {
					pathName = "Ios";
				} else
					pathName = "Android";
			}
		}

		if (!File.Exists (Application.persistentDataPath + "/dir/" + EventSystem.current.currentSelectedGameObject.name + "/" + pathName + "/lettura.0")) {
			slider.gameObject.SetActive (true);
			text.gameObject.SetActive (true);
		} 
		*/
		//controlla se ci sono aggiornameti da fare 
		bool updateDisponibile = false;

		idStoryToDownloadOrBuy = EventSystem.current.currentSelectedGameObject.name;

		foreach (var child in storiesLocal.stories) {
			if (child.id.Equals (idStoryToDownloadOrBuy)) {
				Debug.Log ("trovato child");
				foreach (var child2 in storiesOnline.stories) {
					Debug.Log ("child: " + child.lastUpdate + "child2: " + child2.lastUpdate);
					if (child.id.Equals (child2.id) && child.lastUpdate < child2.lastUpdate) {
						Debug.Log ("Aggiornamento disponibile");
						//show aggiornareStoriaPanel
						pannelloAggiornamento.SetActive (true);
						updateDisponibile = true;
						//se ci sono aggiornamenti puo scegliere di scaricarli o di continuare normalmente
					}
				}
			}
		}
		//se non ci sono continua normalmente
		if (!updateDisponibile)
			updateNegate ();			
	}

	public void updateNegate ()
	{
		SceneManager.LoadScene ("ScenaScelta", LoadSceneMode.Single);
	}

	public void updateAccept ()
	{
		downloadFile ();
	}

	public void buyButton(){
		//Trascinare ID storia da pulsante Storia
		Debug.Log ("Attempting to buy: " + idStoryToDownloadOrBuy);
		//iapEngine.BuyItem (idStoryToDownloadOrBuy);
	}

	public void ErrorPanel(){
		testoErrore.text = "Acquisti momentaneamente non disponibili";
		panelError.SetActive (true);
	}

	public void ErrorPanel(string s)
    {
        testoErrore.text = s;
        panelError.SetActive(true);
    }

	public bool  CheckTransaction(string storyId){
		foreach (Transaction t in transactionsLocal.transactions) {
			//Debug.Log (storyId + " " + transactionsLocal.transactions.Count + " - " + t.gift);
			if (string.Equals (t.storyId, storyId) && !t.gift && !t.refunded) {
				return true;
			}
		}
		return false;
	}

	public void RefundTransaction(string storyId)
    {
        foreach (Transaction t in transactionsLocal.transactions)
        {
			//Debug.Log (storyId + " " + transactionsLocal.transactions.Count + " - " + t.gift);
			if (string.Equals(t.storyId, storyId))
			{
				if (!t.gift && !t.refunded && !t.origin.Equals("mobile"))
				{
					t.refunded = true;
					salvaTransaction(transactionsLocal);
					return;
				}
            }
        }
    }

}
