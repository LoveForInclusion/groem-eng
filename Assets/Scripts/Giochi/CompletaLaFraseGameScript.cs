using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class CompletaLaFraseGameScript : MonoBehaviour {
	public GameObject tesseraBase;
	public GameObject tesseraRisposta;
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
	bool rispostaDataBool = false;
	bool rispostaDataGiustaBool;
	bool parlato=false;

	void Start () {
		responsGiusto.SetActive (false);
		responsSbagiato.SetActive (false);
		timer = 0.5f;
	}

	void Update () {

		if (!errorMouse.GetComponent<AudioSource> ().isPlaying) {
			responsSbagiato.SetActive (false);
			errorMouse.SetActive (false);
		}

		panelCanTouch.SetActive (!canTouch);

		scenaPlayer ();

		if (rispostaDataGiustaBool)
			correctAnswer ();

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

		errorMouse.SetActive (true);
		/*
		responsSbagiato.transform.position = new Vector3 (immaginiPosition [immaginiPosition.Count - 5 + rispostaData].position.x,
			immaginiPosition [immaginiPosition.Count - 5 + rispostaData].position.y, 
			immaginiPosition [immaginiPosition.Count - 5 + rispostaData].position.z);
		responsSbagiato.SetActive (true);
		*/
		for (int i = 0; i < immagini.Count; i++)
			if (immagini [i].GetComponent<FigureController> ().playerIsInPla ()) {

			} else {
				immagini [immagini.Count - 1].GetComponent<AudioSource> ().volume = 1f;
				errorMouse.GetComponent<AudioSource> ().Play ();

				rispostaDataBool = false;
			}
	}

	public void correctAnswer(){

		for (int i = 0; i < immagini.Count; i++)
			if (immagini [i].GetComponent<FigureController> ().playerIsInPla ()) {

			} else {
				if (!parlato) {
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

	public void scenaPlayer(){

		if (timer<=0 && !canTouch) {
			if (figuraCorrente < immagini.Count-4) {
				for (int i = 0; i < immagini.Count; i++)
					if (immagini [i].GetComponent<FigureController> ().playerIsInPla ()) {

					} else {

						if(immagini [figuraCorrente].GetComponent<Image>().sprite.name.Equals("blank")){

							for (int j = 0; j < 4; j++) {
									if (DifficoltaScelta == 0) {
									if (Facile [a[numeroDomanda]].rispostaCorretta.GetComponent<Image> ().sprite.name.Equals (immagini [immagini.Count - j - 1].GetComponent<Image> ().sprite.name)) {

										immagini[immagini.Count - j - 1].GetComponent<FigureController> ().playAudio ();

										}
									}
								else if (DifficoltaScelta == 1) {
									if (Medio [a[numeroDomanda]].rispostaCorretta.GetComponent<Image> ().sprite.name.Equals (immagini [immagini.Count - j - 1].GetComponent<Image> ().sprite.name)) {

										immagini[immagini.Count - j - 1].GetComponent<FigureController> ().playAudio ();

									}
								}
								else if (DifficoltaScelta == 2) {
									if (Difficile [a[numeroDomanda]].rispostaCorretta.GetComponent<Image> ().sprite.name.Equals (immagini [immagini.Count - j - 1].GetComponent<Image> ().sprite.name)) {

										immagini[immagini.Count - j - 1].GetComponent<FigureController> ().playAudio ();

									}
								}
							}
						}

						immagini [figuraCorrente].GetComponent<FigureController> ().playAudio ();
						figuraCorrente++;
					}
			} else {
				for (int i = 0; i < immagini.Count; i++)
					if (immagini [i].GetComponent<FigureController> ().playerIsInPla ()) {

					} else {
						canTouch = true;
						rispostaDataGiustaBool = true;
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

			if (list [a [numeroDomanda]].parole [i].GetComponent<Image>().sprite.name.Equals("blank")) {
				questionMark.transform.position = immaginiPosition[i].position;
				questionMark.SetActive (true);
			}

			tesseraBase.GetComponent<Image> ().sprite = list [a [numeroDomanda]].parole [i].GetComponent<Image> ().sprite;
			tesseraBase.GetComponent<FigureController> ().audioTest = list [a [numeroDomanda]].parole [i].GetComponent<FigureController> ().audioTest;
			GameObject clone = Instantiate (tesseraBase,questArea.transform);
			clone.transform.position = new Vector3 (immaginiPosition[i].position.x,immaginiPosition[i].position.y,immaginiPosition[i].position.z);
			clone.SetActive (true);
			immagini.Add (clone);
		}

		rispostaCorretta = Random.Range ( 1 , 4 );

		int generato = 0;
		for(int i = 0;i < 4 ; i++){

			if ((i + 1) == rispostaCorretta) {

				tesseraRisposta.GetComponent<Image> ().sprite = list [a [numeroDomanda]].rispostaCorretta.GetComponent<Image> ().sprite;
				tesseraRisposta.GetComponent<FigureController> ().audioTest = list [a [numeroDomanda]].rispostaCorretta.GetComponent<FigureController> ().audioTest;
				GameObject clone = Instantiate (tesseraRisposta, questArea.transform);
				clone.transform.position = immaginiPosition [immaginiPosition.Count - 4 + i].position;
				clone.SetActive (true);
				immagini.Add (clone);

			} else {
				tesseraRisposta.GetComponent<Image> ().sprite = list [a [numeroDomanda]].risposteErrate[generato].GetComponent<Image> ().sprite;
				tesseraRisposta.GetComponent<FigureController> ().audioTest = list [a [numeroDomanda]].risposteErrate[generato].GetComponent<FigureController> ().audioTest;
				GameObject clone = Instantiate (tesseraRisposta,questArea.transform);
				clone.transform.position = immaginiPosition[immaginiPosition.Count - 4 + i ].position;
				clone.SetActive (true);
				immagini.Add (clone);
				generato++;
			}

		}
		/*
		canTouch = false;
		goTimer=true;
		scenaPlayer ();
		*/
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
		
		goTimer = false;
		timer = 1.5f;
		figuraCorrente = 0;
		numeroDomanda = 0;
		rispostaDataBool = false;
		correctMouse.SetActive (false);
		responsGiusto.SetActive (false);
		responsSbagiato.SetActive (false);
		canTouch = true;
		parlato = false;
		rispostaDataGiustaBool = false;

	}

	public void nextQuest(){

		parlato = false;
		rispostaDataBool = false;
		responsGiusto.SetActive (false);
		numeroDomanda++;
		goTimer = false;
		timer = 0.5f;
		canTouch = true;
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





	public void Drag(GameObject g){
		

		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0); // get first touch since touch count is greater than zero

			if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
				g.transform.position = Input.GetTouch(0).position;
			}
		} else {
			g.transform.position = Input.mousePosition;
		}
	}

	public void Drop(GameObject g){

		float distance = Vector3.Distance(g.transform.position,questionMark.transform.position);
		if (distance < 150) {
			
			g.transform.position = questionMark.transform.position;

			//if l'immagine è blank e la risposta è uguale all'immagine leggi e dici "giusto"

			if(DifficoltaScelta == 0){
				if (Facile [a[numeroDomanda]].rispostaCorretta.GetComponent<Image> ().sprite.name.Equals (g.GetComponent<Image> ().sprite.name)) {
					//Avvia lettura scena
					canTouch = false;
					goTimer=true;

				} else {
					//else identifica quale sto spostando e rimettilo alla sua posizione originale e dici "nono"
					//scorro le risposte e trovo qual'è, dopo aver fatto ciò lo riporto al suo posto tramite la lista dei transform originali 

					for(int i = 0; i < 4 ; i++){

						if (immagini [immagini.Count - i - 1].GetComponent<Image> ().sprite.name.Equals (g.GetComponent<Image> ().sprite.name)) {
							g.transform.position = immaginiPosition [immaginiPosition.Count - i - 1].position;
							errorAnswer ();
						}
					}
				}
			}
			if(DifficoltaScelta == 1){
				if (Medio [a[numeroDomanda]].rispostaCorretta.GetComponent<Image> ().sprite.name.Equals (g.GetComponent<Image> ().sprite.name)) {
					//Avvia lettura scena
					canTouch = false;
					goTimer=true;

				} else {

					//else identifica quale sto spostando e rimettilo alla sua posizione originale e dici "nono"
					//scorro le risposte e trovo qual'è, dopo aver fatto ciò lo riporto al suo posto tramite la lista dei transform originali 

					for(int i = 0; i < 4 ; i++){

						if (immagini [immagini.Count - i - 1].GetComponent<Image> ().sprite.name.Equals (g.GetComponent<Image> ().sprite.name)) {
							g.transform.position = immaginiPosition [immaginiPosition.Count - i - 1].position;
							errorAnswer ();
						}
					}
				}
			}
			if(DifficoltaScelta == 2){
				if (Difficile [a[numeroDomanda]].rispostaCorretta.GetComponent<Image> ().sprite.name.Equals (g.GetComponent<Image> ().sprite.name)) {
					//Avvia lettura scena
					canTouch = false;
					goTimer=true;

				} else {

					//else identifica quale sto spostando e rimettilo alla sua posizione originale e dici "nono"
					//scorro le risposte e trovo qual'è, dopo aver fatto ciò lo riporto al suo posto tramite la lista dei transform originali 

					for(int i = 0; i < 4 ; i++){

						if (immagini [immagini.Count - i - 1].GetComponent<Image> ().sprite.name.Equals (g.GetComponent<Image> ().sprite.name)) {
							g.transform.position = immaginiPosition [immaginiPosition.Count - i - 1].position;
							errorAnswer ();
						}
					}
				}
			}

		} else {
			
			for (int i = 0; i < 4; i++) {

				if (immagini [immagini.Count - i - 1].GetComponent<Image> ().sprite.name.Equals (g.GetComponent<Image> ().sprite.name)) {
					g.transform.position = immaginiPosition [immaginiPosition.Count - i - 1].position;
				}
			}

		}

		
	}

	public void gameHome(){
		goTimer = false;
		timer = 1.5f;
		figuraCorrente = 0;
		numeroDomanda = 0;
		correctMouse.SetActive (false);
		responsGiusto.SetActive (false);
		responsSbagiato.SetActive (false);
		canTouch = true;

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