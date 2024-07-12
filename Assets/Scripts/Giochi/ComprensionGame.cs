using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ComprensionGame : MonoBehaviour {

	public GameObject tesseraBase;
	public GameObject questArea;
	public List<Transform> immaginiPosition= new List<Transform>();
	public List<GameObject> immagini = new List<GameObject>();

	[System.Serializable]
	public class Domanda{
		
		public List<GameObject> parole;
		public List<GameObject> risposteErrate;
		public GameObject rispostaCorretta;

	}

	public int[] a;
	public List<Domanda> Facile = new List<Domanda>();
	public List<Domanda> Medio = new List<Domanda>();
	public List<Domanda> Difficile = new List<Domanda>();

	public GameObject panelCanTouch;

	public GameObject responsGiusto;
	public GameObject responsSbagiato;

	public GameObject errorMouse;
	public GameObject correctMouse;

	public GameObject questionMark;

	//0 facile 1 medio 2 difficile
	public int DifficoltaScelta;

	public int rispostaData;
	public int rispostaCorretta;

	public bool goTimer=false;

	int numeroDomanda=0;

	float timer;
	bool canTouch = true;
	int figuraCorrente=0;
	int figureCount;
	bool rispostaDataBool;
	bool rispostaDataGiustaBool;
	bool parlato=false;

	public bool delayTimer;
	public float delay;

	void Start () {
		responsGiusto.SetActive (false);
		responsSbagiato.SetActive (false);
		timer = 1.5f;
		delayTimer = false;
		delay = 0.5f;
	}

	void Update () {

		if (!errorMouse.GetComponent<AudioSource> ().isPlaying) {
			responsSbagiato.SetActive (false);
			errorMouse.SetActive (false);
		}

		if (delay <= 0.0f) {
			canTouch = true;
		} else if (delayTimer) {
			delay -= Time.deltaTime;
		}

		if (rispostaDataBool)
			errorAnswer ();
		
		if (rispostaDataGiustaBool)
			correctAnswer ();

		panelCanTouch.SetActive (!canTouch);

		scenaPlayer ();

		if (timer <= 0.0f) {
			//do nothing
		}
		else if(goTimer)
			timer -= Time.deltaTime;
	}

	public void Shuffle( List<Domanda> list){

		a = new int[list.Count];

		for (int i = 0; i < list.Count; i++)
			a [i] = i;

		int n = a.Length;

		while(n>1){
			n--;
			int k = Random.Range ( 0 , n);
			int value= a[k];
			a [k] = a [n];
			a [n] = value;
		}
	}

	public void errorAnswer(){
		
		immagini [immagini.Count - 1].GetComponent<AudioSource> ().volume = 0f;

		errorMouse.SetActive (true);
		responsSbagiato.transform.position = new Vector3 (immaginiPosition [immaginiPosition.Count - 5 + rispostaData].position.x,
			immaginiPosition [immaginiPosition.Count - 5 + rispostaData].position.y, 
			immaginiPosition [immaginiPosition.Count - 5 + rispostaData].position.z);
		responsSbagiato.SetActive (true);

		errorMouse.GetComponent<AudioSource> ().Play ();
	
		//immagini [immagini.Count - 1].GetComponent<AudioSource> ().volume = 1f;
				
		rispostaDataBool = false;
			
	}

	public void correctAnswer(){


		immagini [immagini.Count - 1].GetComponent<AudioSource> ().volume = 1f;

		responsGiusto.transform.position = new Vector3 (immaginiPosition [immaginiPosition.Count - 5 + rispostaData].position.x,
			immaginiPosition [immaginiPosition.Count - 5 + rispostaData].position.y, 
			immaginiPosition [immaginiPosition.Count - 5 + rispostaData].position.z);
		

		for (int i = 0; i < immagini.Count; i++)
			if (immagini [i].GetComponent<FigureController> ().playerIsInPla ()) {

			} else {
				if (!parlato) {
					responsGiusto.SetActive (true);
					correctMouse.SetActive (true);
					correctMouse.GetComponentInChildren<AudioSource> ().Play ();
					parlato = true;
				} else {
					if (correctMouse.GetComponentInChildren<AudioSource> ().isPlaying)
						canTouch = false;
					else {
						canTouch = true;
					rispostaDataGiustaBool = false;
					}
				}
			}
	}
		
	public void controllaRisposta(){

		//risposta corretta
		if(rispostaCorretta == rispostaData){
			//Risposta giusta function
			rispostaDataGiustaBool = true;
		}
		//risposta errata
		else{
			rispostaDataBool = true;
		}
	}

	public void scenaPlayer(){
		
		if (timer<=0 && !canTouch) {
			if (figuraCorrente < immagini.Count) {
				for (int i = 0; i < immagini.Count; i++)
					if (immagini [i].GetComponent<FigureController> ().playerIsInPla ()) {

					} else {

						immagini [figuraCorrente].GetComponent<FigureController> ().playAudio ();
						figuraCorrente++;
					}
			} else {
				for (int i = 0; i < immagini.Count; i++)
					if (immagini [i].GetComponent<FigureController> ().playerIsInPla ()) {

					} else {
						delayTimer = true;
					}
				
			}
		}
	}


	public void generator(List<Domanda> list){

		figureCount = list [a[numeroDomanda]].parole.Count;

		for (int i = 0; i < immagini.Count; i++) {
			Destroy (immagini [i]);
		}
		immagini.Clear ();

		for (int i = 0; i < figureCount; i++) {

			tesseraBase.GetComponent<Image> ().sprite = list [a [numeroDomanda]].parole [i].GetComponent<Image> ().sprite;
			tesseraBase.GetComponent<FigureController> ().audioTest = list [a [numeroDomanda]].parole [i].GetComponent<FigureController> ().audioTest;
			GameObject clone = Instantiate (tesseraBase,questArea.transform);
			clone.transform.position = new Vector3 (immaginiPosition[i].position.x,immaginiPosition[i].position.y,immaginiPosition[i].position.z);
			clone.SetActive (true);
			immagini.Add (clone);
		}
			
		if(figureCount!=(immaginiPosition.Count-4)){
		questionMark.transform.position = new Vector3 (immaginiPosition[figureCount].position.x,
			immaginiPosition[figureCount].position.y,immaginiPosition[figureCount].position.z);
		questionMark.SetActive (true);
		}
		else
			questionMark.SetActive (false);
		rispostaCorretta = Random.Range ( 1 , 4 );

		int generato = 0;
		for(int i = 0;i < 4 ; i++){

			if (i + 1 == rispostaCorretta) {
				
				tesseraBase.GetComponent<Image> ().sprite = list [a [numeroDomanda]].rispostaCorretta.GetComponent<Image> ().sprite;
				tesseraBase.GetComponent<FigureController> ().audioTest = list [a [numeroDomanda]].rispostaCorretta.GetComponent<FigureController> ().audioTest;
				GameObject clone = Instantiate (tesseraBase, questArea.transform);
				clone.transform.position = new Vector3 (immaginiPosition [immaginiPosition.Count - 4 + i].position.x,
					immaginiPosition [immaginiPosition.Count - 4 + i].position.y,
					immaginiPosition [immaginiPosition.Count - 4 + i].position.z);
				clone.SetActive (true);
				immagini.Add (clone);

				if (i == 0) {
					clone.GetComponent<Button> ().onClick.AddListener (rispondiUno);
				} else if (i == 1) {
					clone.GetComponent<Button> ().onClick.AddListener (rispondiDue);
				} else if (i == 2) {
					clone.GetComponent<Button> ().onClick.AddListener (rispondiTre);
				} else {
					clone.GetComponent<Button> ().onClick.AddListener (rispondiQuat);
				}

			} else {
				tesseraBase.GetComponent<Image> ().sprite = list [a [numeroDomanda]].risposteErrate[generato].GetComponent<Image> ().sprite;
				tesseraBase.GetComponent<FigureController> ().audioTest = list [a [numeroDomanda]].risposteErrate[generato].GetComponent<FigureController> ().audioTest;
				GameObject clone = Instantiate (tesseraBase,questArea.transform);
				clone.transform.position = new Vector3 (immaginiPosition[immaginiPosition.Count - 4 + i ].position.x,
					immaginiPosition[immaginiPosition.Count - 4 + i ].position.y,
					immaginiPosition[immaginiPosition.Count - 4 + i ].position.z);
				clone.SetActive (true);
				immagini.Add (clone);
				generato++;

				if (i == 0) {
					clone.GetComponent<Button> ().onClick.AddListener (rispondiUno);
				} else if (i == 1) {
					clone.GetComponent<Button> ().onClick.AddListener (rispondiDue);
				} else if (i == 2) {
					clone.GetComponent<Button> ().onClick.AddListener (rispondiTre);
				} else {
					clone.GetComponent<Button> ().onClick.AddListener (rispondiQuat);
				}
			}
			
		}



		canTouch = false;
		goTimer=true;
		scenaPlayer ();

	}

	public void generaDomanda(){
		
		if (DifficoltaScelta == 0) {
			Shuffle (Facile);
			generator (Facile);

		}
		else if (DifficoltaScelta == 1) {
			Shuffle (Medio);
			generator (Medio);
		}
		else if (DifficoltaScelta == 2) {
			Shuffle (Difficile);
			generator (Difficile);
		}
	}

	public void pulisciScena(){

		delayTimer = false;
		delay = 0.5f;
		goTimer = false;
		timer = 1.5f;
		figuraCorrente = 0;
		numeroDomanda = 0;
		parlato = false;
		rispostaDataBool = false;
		rispostaDataGiustaBool = false;
		correctMouse.SetActive (false);
		responsGiusto.SetActive (false);
		responsSbagiato.SetActive (false);
		panelCanTouch.SetActive (false);

	}

	public void nextQuest(){

		delayTimer = false;
		delay = 0.5f;
		parlato = false;
		rispostaDataBool = false;
		responsGiusto.SetActive (false);
		correctMouse.SetActive (false);
		numeroDomanda++;
		goTimer = false;
		timer = 1.5f;
		canTouch = false;
		figuraCorrente = 0;

		if (DifficoltaScelta == 0) {
			if (numeroDomanda > Facile.Count - 1) {
				questionMark.transform.parent.gameObject.SetActive (false);
				pulisciScena ();
				canTouch = true;
			}
			else 
				generator (Facile);

		}
		else if (DifficoltaScelta == 1) {
			if (numeroDomanda > Medio.Count - 1) {
				questionMark.transform.parent.gameObject.SetActive (false);
				pulisciScena ();
				canTouch = true;
			}
			else
				generator (Medio);
		}
		else if (DifficoltaScelta == 2) {
			if (numeroDomanda > Difficile.Count - 1) {
				questionMark.transform.parent.gameObject.SetActive (false);
				pulisciScena ();
				canTouch = true;
			}
			else 
				generator (Difficile);
		}

	}

	public void rispondiUno(){
		rispostaData = 1;
		controllaRisposta ();
	}

	public void rispondiDue(){
		rispostaData = 2;
		controllaRisposta ();
	}

	public void rispondiTre(){
		rispostaData = 3;
		controllaRisposta ();
	}

	public void rispondiQuat(){
		rispostaData = 4;
		controllaRisposta ();
	}

	public void gameHome(){

		delayTimer = false;
		delay = 0.5f;
		goTimer = false;
		timer = 1.5f;
		canTouch = true;
		figuraCorrente = 0;
		numeroDomanda = 0;
		rispostaDataBool = false;
		rispostaDataGiustaBool = false;
		correctMouse.SetActive (false);
		responsGiusto.SetActive (false);
		responsSbagiato.SetActive (false);
		panelCanTouch.SetActive (false);

	}



	public void ChoseDifficultyEasy(){
		DifficoltaScelta = 0;
		generaDomanda ();
	}

	public void ChoseDifficultyMedium(){
		DifficoltaScelta = 1;
		generaDomanda ();
	}

	public void ChoseDifficultyHard(){
		DifficoltaScelta = 2;
		generaDomanda ();
	}

	public void backScene(){

		SceneManager.LoadScene ("ScenaScelta",LoadSceneMode.Single);

	}
}
