using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Purchasing;
using RESTClient.Request;
using Model;

/*public class PurchaseEngine : MonoBehaviour, IStoreListener {

	private static IStoreController storeCont; //Il centro di Unity IAP
	private static IExtensionProvider extensionP; //Motore di IAP store-specifico

	//private Stories storiesOnline = null, storiesLocal = new Stories(new List<Story>());

	public GameObject purchaseablePanel, initText, posiPrompt, negaPrompt;
	public GameObject purchaseable;

	// Use this for initialization
	void Start () {
		//Controllo se lo store è inizializzato
		if (Directory.Exists (Path.Combine (Path.Combine (Application.persistentDataPath, "Unity"), "UnityPurchasing"))) {
			Directory.Delete (Path.Combine (Path.Combine (Application.persistentDataPath, "Unity"), "UnityPurchasing"), true);
			Debug.Log ("Transazioni Cancellate");
		} else {
			Debug.Log ("La directory con le transazioni non esiste.");
		}
		if(storeCont == null){
			InitializeIAP ();
		}
	}

	public void InitializeIAP(){
		//Controllo se connesso ad IAP
		if (IsInitialized ()) {
			//Tutto a posto
			return;
		}


		//Setup semplice con il builder;
		var builder = ConfigurationBuilder.Instance (StandardPurchasingModule.Instance());


		builder.AddProduct ("android.test.purchased", ProductType.Consumable);
		UISetup ("android.test.purchased", "Acquisto riuscito" + " - " + (0.50f).ToString("F2")+ "€");
		builder.AddProduct("android.test.canceled", ProductType.NonConsumable);
		UISetup ("android.test.canceled", "Acquisto annullato" + " - " + (0.50f).ToString("F2")+ "€");
		builder.AddProduct("android.test.refunded", ProductType.NonConsumable);
		UISetup ("android.test.refunded", "Acquisto rimborsato" + " - " + (0.50f).ToString("F2")+ "€");
		builder.AddProduct("android.test.item_unavailable", ProductType.NonConsumable);
		UISetup ("android.test.item_unavailable", "Acquisto non disponibile" + " - " + (0.50f).ToString("F2")+ "€");
		builder.AddProduct("groem_story", ProductType.NonConsumable);
		UISetup ("groem_story", "Acquisto Groem" + " - " + (0.50f).ToString("F2")+ "€");

		//Concluso il fetch faccio inizializzare il builder.
		UnityPurchasing.Initialize (this, builder);

//		//Cerco le storie e le aggiungo ai prodotti;
//		StoryRequest.GetAllStory (this,
//			delegate(Stories stories) {
//
//				//storiesOnline = stories;
//
////				if (storiesOnline != null) {
////					foreach (Story s in storiesOnline.stories) {
////						builder.AddProduct(s.id, ProductType.NonConsumable);
////						//Debug.Log (s.id);
////						UISetup (s.id, s.title + " - " + s.price.ToString("F2")+ "€");
////					}
////				}
//
//				builder.AddProduct("groem_story", ProductType.NonConsumable, new IDs(){{"groem_story",GooglePlay.Name}});
//				UISetup ("groem_story", "Il cattivissimo Groem" + " - " + (0.50f).ToString("F2")+ "€");
//
//				//Concluso il fetch faccio inizializzare il builder.
//				UnityPurchasing.Initialize (this, builder);
//
//			},
//			delegate(string code, string error) { 
//				Debug.Log (code + "---" + error);
//				if (!File.Exists (Application.persistentDataPath + "/myLibrary.dat"))
//					Debug.Log ("Connettersi a internet");
//			});

	}

	private bool IsInitialized()
	{
		// Ritorna true solo se tutti i riferimenti sono validi
		return storeCont != null && extensionP != null;
	}

	public void BuyItem(string id){
		//Controlla sempre se è inizializzato
		if (IsInitialized ()) {

			if (!isLocalStory (id)) {

			Product fetchP = storeCont.products.WithID (id);


			//Controllo se esiste e se si può comprare
			//é possibile controllare anche la ricevuta di acquisto
			//ma non si salva a chiusura dell'app (probabilmente si dovrebbero ristorare gli acquisti ad ogni restart)
			//Oppure controllare direttamente prima di inizializzare l'acquisto se la storia è già tra i contenuti locali.
			if (fetchP != null && fetchP.availableToPurchase) {

				Debug.Log ("Sto inizializzando l'acquisto di " + id + ".");

				//Passo la procedura all IAP che risponderà in ProcessPurchase o OnPurchaseFailed;
				storeCont.InitiatePurchase (fetchP);
			} else {
				//Prodotto inesistente o non disponibile;
				Debug.Log ("Il prodotto non è più in vendita o non è possibile acquistarlo.");
			}

			} else {
				Debug.Log ("Hai già questa storia, non puoi comprarla.");
			}

		} else {
			//Non è inizializzato l'IAP;
			Debug.Log("Impossibile comprare, l'IAP non è inizializzato");
		}

	}



	/*
	 * 
	 * 	METODI INTERFACCIA iStoreListener
	 * 
	 * ///

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions){
		//L'inizializzazione è andata a buon fine 
		//posso prendere i riferimenti al motore di IAP e alle estensioni per i market specifici
		storeCont = controller;
		extensionP = extensions;
		initText.GetComponent<Text>().text = "Initialized";
		initText.GetComponent<Text> ().color = Color.green;
	}

	public void OnInitializeFailed(InitializationFailureReason error){
		//C'è stato un problema
		Debug.Log("IAP non inizializzato perché: " + error);
		negaPrompt.transform.Find ("Text").transform.GetComponent<Text> ().text = ""+error;
		negaPrompt.gameObject.SetActive (true);
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args){

		bool recognized = false;

//		foreach (Story s in storiesOnline.stories) {
//			//Controllo quale storia sto comprando (Per ID)
//			if (string.Equals (s.id, args.purchasedProduct.definition.id, System.StringComparison.Ordinal)) {
//				Debug.Log ("Hai acquistato: " + s.title + "! Adesso puoi scaricarlo! YAY!");
//				storiesLocal.stories.Add (s);
//				recognized = true;
//			}
//		}

		if (string.Equals ("android.test.purchased", args.purchasedProduct.definition.id, System.StringComparison.Ordinal)) {
			posiPrompt.transform.Find ("Text").transform.GetComponent<Text> ().text = ("Hai acquistato: Item Acquistabile!\nAdesso puoi scaricarlo! YAY!");
			recognized = true;
			posiPrompt.gameObject.SetActive (true);
		}if (string.Equals ("android.test.canceled", args.purchasedProduct.definition.id, System.StringComparison.Ordinal)) {
			posiPrompt.transform.Find ("Text").transform.GetComponent<Text> ().text = ("Hai acquistato: Item Annullato!\nAdesso puoi scaricarlo! YAY!");
			recognized = true;
			posiPrompt.gameObject.SetActive (true);
		}if (string.Equals ("android.test.refunded", args.purchasedProduct.definition.id, System.StringComparison.Ordinal)) {
			posiPrompt.transform.Find ("Text").transform.GetComponent<Text> ().text = ("Hai acquistato: Item Rimborsato!\nAdesso puoi scaricarlo! YAY!");
			recognized = true;
			posiPrompt.gameObject.SetActive (true);
		}if (string.Equals ("android.test.item_unavailable", args.purchasedProduct.definition.id, System.StringComparison.Ordinal)) {
			posiPrompt.transform.Find ("Text").transform.GetComponent<Text> ().text = ("Hai acquistato: Item non Disponibile!\nAdesso puoi scaricarlo! YAY!");
			recognized = true;
			posiPrompt.gameObject.SetActive (true);
		}if (string.Equals ("groem_story", args.purchasedProduct.definition.id, System.StringComparison.Ordinal)) {
			posiPrompt.transform.Find ("Text").transform.GetComponent<Text> ().text = ("Hai acquistato: Groem!\nAdesso puoi scaricarlo! YAY!");
			recognized = true;
			posiPrompt.gameObject.SetActive (true);
		}

		if(!recognized)//Se l'acquisto va a buon fine ma il prodotto non fa parte delle storie conosciute;
			Debug.Log ("Sono confuso, è stato acquistato un prodotto non riconosciuto.");
		
		return PurchaseProcessingResult.Complete;

	}

	public void OnPurchaseFailed(Product p, PurchaseFailureReason error){
		//L'acquisto non è andato a buon fine, il perché è in error
		Debug.Log ("Acquisto andato male perché: " + error);
		negaPrompt.transform.Find ("Text").transform.GetComponent<Text> ().text = ""+error + " " + p.definition;
		negaPrompt.gameObject.SetActive (true);

	}

	public void UISetup(string id, string title){
		GameObject go = Instantiate (purchaseable.transform, purchaseablePanel.transform).gameObject;
		go.transform.GetComponent<PurchaseElementUI> ().setupUIElement(this, title, id, isLocalStory(id));
	}

	private bool isLocalStory(string id){
		//Controlla tra le storie locali
		Debug.Log(Directory.Exists(Application.persistentDataPath + "/dir/" + id));
		return Directory.Exists(Application.persistentDataPath + "/dir/" + id);
	}

}*/
