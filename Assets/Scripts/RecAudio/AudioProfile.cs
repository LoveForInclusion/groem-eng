using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Runtime;
using System.Runtime.InteropServices;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AudioProfile : MonoBehaviour {



	public AudioSource canv;

	public List<ImageEDidascalia> sceneFacile;
	public List<ImageEDidascalia> sceneDifficile;

	public GameObject pref;

	public GameObject father;

	public GameObject pannelloModificaNome;
	public InputField nomeProfilo;

	public Button MyDropdownProfilo;
	public Button profilo1;
	public Button profilo2;
	public Button profilo3;

	public Button MyDropdownDifficolt;

	public int profiloScelto;

	public GameObject zoomPanel;
	public GameObject permAlert;

	public string easyDicitur;
	public string profileNotVieved1;
	public string profileNotVieved2;
	public string profileNotVieved3;

	OpzioniAudio options;

	public string getMyDropdownDifficoltaString(){
		if (MyDropdownDifficolt.GetComponentInChildren<Text>().text.Equals("Semplice"))
			return "Facile";
		else
			return "Difficile";
		}

	public void modificaNomeProfilo(){
		
		nomeProfilo.text = MyDropdownProfilo.GetComponentInChildren<Text>().text;

		pannelloModificaNome.SetActive (true);

	}

	public void chiudiModificaNomeProfilo(){
		pannelloModificaNome.SetActive (false);
	}

	//cerca il profilo sceltro tramite il nome e avvalora la variabile profiloScelto di tipo int
	public void numProfiloScelto(){
		
		foreach (Transform child in MyDropdownProfilo.gameObject.transform)
			if (child.name.Equals ("Panel")) {
				foreach (Transform childChild in child.transform) {
					if (childChild.GetComponentInChildren<Text> ().text.Equals (MyDropdownProfilo.GetComponentInChildren<Text> ().text)) {
						profiloScelto = int.Parse(childChild.name);
					}
				}
			}

	}

	public void confermaModificaNomeProfilo(){

		numProfiloScelto ();
		MyDropdownProfilo.GetComponentInChildren<Text>().text = nomeProfilo.text;

		if (MyDropdownDifficolt.GetComponentInChildren<Text>().text.Equals(easyDicitur))
        {
			options.option.nomiProfiliModificatiEasy[profiloScelto] = nomeProfilo.text;
        }
        else
        {
			options.option.nomiProfiliModificatiAdvanced[profiloScelto] = nomeProfilo.text;
        }

		GetComponent<OpzioniAudio> ().salvaOpzioniAudio ();

		if (profiloScelto < 3)//Modificato
		{
			foreach (Transform child in MyDropdownProfilo.gameObject.transform)
				if (child.name.Equals ("Panel")) {
					foreach (Transform childChild in child.transform) {
						if (childChild.name.Equals(profiloScelto+"") ) {
							childChild.GetComponentInChildren<Text> ().text = MyDropdownProfilo.GetComponentInChildren<Text> ().text;
						}
					}
				}
		}

		chiudiModificaNomeProfilo ();
		
	}

	public void zoom(){
		zoomPanel.transform.GetChild(0).gameObject.GetComponent<Image> ().sprite = EventSystem.current.currentSelectedGameObject.GetComponent<Image> ().sprite;
		zoomPanel.SetActive(true);
	}

	public void generateScene(List<ImageEDidascalia> scene){


		father.GetComponent<RectTransform> ().offsetMin = new Vector2 (0f,scene.Count*-480);

		for (int i = 0; i < scene.Count; i++) {
			//istanziare l'object e assegnarlo al padre
			GameObject clone = Instantiate(pref);
			clone.name = "" + i;
			foreach (Transform child in clone.transform) {
				
				if (child.name == "MiniaturaScena") {
					//cambiare immagine
					child.GetComponentInChildren<Image>().sprite = scene[i].immagine;
					child.GetComponentInChildren<Image> ().enabled = true;
					child.GetComponent<Button> ().onClick.AddListener (zoom);
					child.GetComponent<Button> ().enabled = true;
				}
					
				//if (child.name == "TestoCopione") {
					//modifica testo
					//clone.transform.Find("Image").Find("Image").Find("TestoCopione").gameObject.GetComponent<Text>().text = scene[i].frase;
				clone.transform.GetChild(1).GetChild(4).Find("TestoCopione").gameObject.GetComponent<Text>().text = scene[i].frase;
				//}
				clone.transform.GetChild(2).GetComponent<Text>().text = "Scena " +(i+1);
				clone.SetActive (true);

				if (child.name == "BottoneRec") {
				//	child.GetComponent<Button> ().onClick.AddListener (recAudio);
				}

				if (child.name == "BottonePlay") {
				//	child.GetComponent<Button> ().onClick.AddListener (playAudio);
				}
			}


			clone.transform.SetParent (father.transform,false);

			//avvalorare l'object con l'immagie e la descrizione de ImageEDidascalia
		}

	}

	public void chageDifficulty(){

		foreach(Transform child in father.transform){

			if(!child.name.Equals("SingolaScenaRecord"))
				Destroy (child.gameObject);
			
		}

		if (MyDropdownDifficolt.GetComponentInChildren<Text>().text.Equals(easyDicitur))
			generateScene (sceneFacile);
		else
			generateScene (sceneDifficile);
		
		modifyMyDropdownProfiliPersonalizzati();

	}


	public void modifyMyDropdownProfiliPersonalizzati(){
		//modifico la MyDropdown

		if (MyDropdownDifficolt.GetComponentInChildren<Text>().text.Equals(easyDicitur))
		{
			MyDropdownProfilo.GetComponentInChildren<Text>().text = options.option.nomiProfiliModificatiEasy[0];

			profilo1.GetComponentInChildren<Text>().text = options.option.nomiProfiliModificatiEasy[0];
			profilo2.GetComponentInChildren<Text>().text = options.option.nomiProfiliModificatiEasy[1];
			profilo3.GetComponentInChildren<Text>().text = options.option.nomiProfiliModificatiEasy[2];

			profileNotVieved1 = options.option.nomiProfiliModificatiAdvanced[0];
            profileNotVieved2 = options.option.nomiProfiliModificatiAdvanced[1];
            profileNotVieved3 = options.option.nomiProfiliModificatiAdvanced[2];
		}
		else
		{
			MyDropdownProfilo.GetComponentInChildren<Text>().text = options.option.nomiProfiliModificatiAdvanced[0];

			profilo1.GetComponentInChildren<Text>().text = options.option.nomiProfiliModificatiAdvanced[0];
			profilo2.GetComponentInChildren<Text>().text = options.option.nomiProfiliModificatiAdvanced[1];
			profilo3.GetComponentInChildren<Text>().text = options.option.nomiProfiliModificatiAdvanced[2];

			profileNotVieved1 = options.option.nomiProfiliModificatiEasy[0];
			profileNotVieved2 = options.option.nomiProfiliModificatiEasy[1];
			profileNotVieved3 = options.option.nomiProfiliModificatiEasy[2];

		}
		
	}


	IEnumerator Start () {
		
		//prendo componente audio
		canv = GetComponent<AudioSource> ();


		//stampo la directory
		Debug.Log (Application.persistentDataPath);

		//carico le impostazioni salvate
		if(!SceneManager.GetActiveScene().name.Equals("NewPlayer")){
			options = GetComponent<OpzioniAudio> ();
			options.caricaOpzioniAudio ();
		} else {
			options = Camera.main.GetComponent<Player>().opzioniAudio;
		}


		//modifico la dropdown dei nomi dei profili
		/*primoElemento.GetComponent<Text> ().text = GetComponent<OpzioniAudio> ().option.nomiProfiliModificati [0];

		for(int i=0; i<3; i++){

			Dropdown.OptionData newItem = new Dropdown.OptionData(GetComponent<OpzioniAudio> ().option.nomiProfiliModificati[i]+"");
			sceltaProfilo.options.RemoveAt(i);
			sceltaProfilo.options.Insert(i, newItem);
			
		}*/



		easyDicitur = MyDropdownDifficolt.GetComponentInChildren<Text>().text;


		modifyMyDropdownProfiliPersonalizzati();		

		if (MyDropdownDifficolt.GetComponentInChildren<Text>().text.Equals(easyDicitur))
			generateScene (sceneFacile);
		else
			generateScene (sceneDifficile);

		yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
		if (Application.HasUserAuthorization (UserAuthorization.Microphone)) {
		} 
		else {
			permAlert.SetActive (true);
		}
	}
		
		
	public void jumpBack(){
		SceneManager.LoadScene ("ScenaScelta",LoadSceneMode.Single);
	}

}
