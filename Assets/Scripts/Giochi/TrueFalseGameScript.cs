using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrueFalseGameScript : MonoBehaviour
{


	public GameObject tesseraBase;
	public GameObject questArea;
	public List<Transform> immaginiPosition = new List<Transform> ();
	public List<GameObject> immagini = new List<GameObject> ();

	[System.Serializable]
	public class Domanda
	{
		public List<GameObject> parole;
		public bool risposta;
	}

	public int[] a;
	public List<Domanda> Facile = new List<Domanda> ();
	public List<Domanda> Medio = new List<Domanda> ();
	public List<Domanda> Difficile = new List<Domanda> ();

	public GameObject panelCanTouch;
	public GameObject errorTrue;
	public GameObject errorFalse;
	public GameObject errorMouse;
	public GameObject correctMouse;


	//0 facile 1 medio 2 difficile
	public int DifficoltaScelta;

	public bool rispostaData;

	public bool goTimer = false;

	public bool delayTimer;
	public float delay;

	int numeroDomanda = 0;

	float timer;
	bool canTouch = true;
	int figuraCorrente = 0;
	int figureCount;
	bool parlato = false;
	bool risposto= false;


	void Start ()
	{
		timer = 1.5f;
		delayTimer = false;
		delay = 0.5f;
	}

	void Update ()
	{

		if (risposto)
			controllaRisposta();

		if (!errorMouse.GetComponent<AudioSource> ().isPlaying) {
			errorTrue.SetActive (false);
			errorFalse.SetActive (false);
			errorMouse.SetActive (false);
		}

		panelCanTouch.SetActive (!canTouch);

		scenaPlayer ();

		if (delay <= 0.0f) {
			canTouch = true;
		} else if (delayTimer) {
			delay -= Time.deltaTime;
		}


		if (timer <= 0.0f) {
			//do nothing
		} else if (goTimer)
			timer -= Time.deltaTime;
		
	}

	public void Shuffle (List<Domanda> list)
	{
		
		a = new int[list.Count];

		for (int i = 0; i < list.Count; i++)
			a [i] = i;
		
		int n = a.Length;

		while (n > 1) {
			n--;
			int k = Random.Range (0, n);
			int value = a [k];
			a [k] = a [n];
			a [n] = value;
		}
	}

	public void scenaPlayer ()
	{
		if (timer <= 0 && !canTouch) {
			if (figuraCorrente < figureCount) {
				for (int i = 0; i < figureCount; i++)
					if (immagini [i].GetComponent<FigureController> ().playerIsInPla ()) {

					} else {

						Debug.Log ("play audio " + figuraCorrente + " su " + figureCount);

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



	public void generator (List<Domanda> list)
	{
		
		figureCount = list [a [numeroDomanda]].parole.Count;


		for (int i = 0; i < immagini.Count; i++) {
			Destroy (immagini [i]);
		}
		immagini.Clear ();

		for (int i = 0; i < figureCount; i++) {

			tesseraBase.GetComponent<Image> ().sprite = list [a [numeroDomanda]].parole [i].GetComponent<Image> ().sprite;
			tesseraBase.GetComponent<FigureController> ().audioTest = list [a [numeroDomanda]].parole [i].GetComponent<FigureController> ().audioTest;
			GameObject clone = Instantiate (tesseraBase, questArea.transform);
			clone.transform.position = new Vector3 (immaginiPosition [i].position.x, immaginiPosition [i].position.y, immaginiPosition [i].position.z);
			clone.SetActive (true);
			immagini.Add (clone);
		}
		canTouch = false;
		goTimer = true;
		scenaPlayer ();

	}

	public void generaDomanda ()
	{
		if (DifficoltaScelta == 0) {
			Shuffle (Facile);
			generator (Facile);

		} else if (DifficoltaScelta == 1) {
			Shuffle (Medio);
			generator (Medio);
		} else if (DifficoltaScelta == 2) {
			Shuffle (Difficile);
			generator (Difficile);
		}
	}

	public void nextQuest ()
	{

		parlato = false;
		delayTimer = false;
		delay = 0.5f;
		numeroDomanda++;
		goTimer = false;
		timer = 1.5f;
		canTouch = false;
		figuraCorrente = 0;

		if (DifficoltaScelta == 0) {
			if (numeroDomanda > Facile.Count - 1) {
				errorMouse.transform.parent.gameObject.SetActive (false);
				pulisciScena ();
				canTouch = true;
			} else
				generator (Facile);

		} else if (DifficoltaScelta == 1) {
			if (numeroDomanda > Medio.Count - 1) {
				errorMouse.transform.parent.gameObject.SetActive (false);
				pulisciScena ();
				canTouch = true;
			} else
				generator (Medio);
		} else if (DifficoltaScelta == 2) {
			if (numeroDomanda > Difficile.Count - 1) {
				errorMouse.transform.parent.gameObject.SetActive (false);
				pulisciScena ();
				canTouch = true;
			} else
				generator (Difficile);
		}

	}



	public void controllaRisposta ()
	{
		List<Domanda> question = null;
		switch (DifficoltaScelta) {

		case 0:
			question = Facile;
			break;
		case 1:
			question = Medio;
			break;
		case 2:
			question = Difficile;
			break;
		default:
			Debug.Log ("Errore");
			break;
		}
		
		//risposta corretta
		if (question [a [numeroDomanda]].risposta == rispostaData) {
			correctMouse.SetActive (true);
			correctMouse.GetComponentInChildren<AudioSource> ().playOnAwake = false;
			if (!parlato) {
				correctMouse.GetComponentInChildren<AudioSource> ().Play ();
				parlato = true;
			} else {
				if (correctMouse.GetComponentInChildren<AudioSource> ().isPlaying)
				{
					canTouch = false;
				}
				else {
					canTouch = true;
					risposto = false;
				}
			}
		}
			//risposta errata
			else {
			errorMouse.SetActive (true);
			if (rispostaData)
				errorTrue.SetActive (true);
			else
				errorFalse.SetActive (true);
			risposto = false;
		}

	}

	public void rispondiTrue ()
	{
		rispostaData = true;
		risposto = true;
	}

	public void rispondiFalse ()
	{
		rispostaData = false;
		risposto = true;
	}

	public void gameHome ()
	{
		
		delayTimer = false;
		delay = 0.5f;
		goTimer = false;
		timer = 1.5f;
		canTouch = true;
		figuraCorrente = 0;
		numeroDomanda = 0;
		correctMouse.SetActive (false);
		canTouch = true;

	}

	public void pulisciScena ()
	{
		
		delayTimer = false;
		delay = 0.5f;
		goTimer = false;
		timer = 1.5f;
		canTouch = false;
		figuraCorrente = 0;
		numeroDomanda = 0;
		correctMouse.SetActive (false);
		canTouch = true;

	}

	public void ChoseDifficultyEasy ()
	{
		DifficoltaScelta = 0;
		generaDomanda ();
	}

	public void ChoseDifficultyMedium ()
	{
		DifficoltaScelta = 1;
		generaDomanda ();
	}

	public void ChoseDifficultyHard ()
	{
		DifficoltaScelta = 2;
		generaDomanda ();
	}

	public void backScene ()
	{

		SceneManager.LoadScene ("ScenaScelta", LoadSceneMode.Single);

	}
}
