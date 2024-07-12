using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MemoryGameScript : MonoBehaviour {


	//0 facile 1 medio 2 difficile
	//6 carte 8 carte 12 carte
	public int DifficoltaScelta;

	//1 giocatore 2 giocatori
	public int Giocatori;
	public int turno;

	public int puntiPlayer1;
	public int puntiPlayer2;

	public GameObject baseCard;

	List<GameObject> cards = new List<GameObject>();

	public List<Sprite> retro = new List<Sprite> ();
	public List<Sprite> immagini = new List<Sprite> ();

	public GameObject canTouchPanel;
	public Transform spacer;

	public int[] a;

	//0 card1 1 card 2 
	public int carta;
	public string card1;
	public string card2;
	public GameObject card1GO;
	public GameObject card2GO;

	public int movimenti;
	public Text movimentiGioco;
	public Text haiVintoCon;

	public GameObject vittoriaSinglePlayer;
	public GameObject vittoriaPlayer1;
	public GameObject vittoriaPlayer2;
	public GameObject pareggio;
	public GameObject player1mugshotActive;
	public GameObject player2mugshotActive;
	public GameObject player1mugshotBN;
	public GameObject player2mugshotBN;

	public GameObject mazzettoPlayer1;
	public GameObject mazzettoPlayer2;



	int cardNumber;
	int coppieTrovate;

	bool endGame;

	void Start () {		
		Input.multiTouchEnabled = false;
		if(Giocatori == 1){
			movimentiGioco.text = "Movimenti: 0";
		}

		carta = 0;	
		movimenti = 0;
		canTouchPanel.SetActive (false);
	}


	void Update () {

		if (Giocatori == 1) {
			movimentiGioco.gameObject.SetActive (true);
		} else if(Giocatori==2) {
			movimentiGioco.gameObject.SetActive (false);
			if (turno == 0) {
				player1mugshotBN.SetActive (false);
				player2mugshotBN.SetActive (true);
				player1mugshotActive.SetActive (true);
				player2mugshotActive.SetActive (false);
			} else {
				player1mugshotBN.SetActive (true);
				player2mugshotBN.SetActive (false);
				player1mugshotActive.SetActive (false);
				player2mugshotActive.SetActive (true);
			}
				
		}

		if (endGame) {
			giocoFinito ();
		}

		if(card2GO!=null && card1GO!=null) 
		if(card2GO.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("StayFace") &&
			card1GO.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("StayFace"))
			controllaCarteUguali ();

	}

	public void giocoFinito(){

		foreach (Transform child in spacer)
			Destroy (child.gameObject);

		if (Giocatori == 1) {
			vittoriaSinglePlayer.SetActive (true);
			haiVintoCon.gameObject.SetActive(true);
			movimentiGioco.text = movimenti + " Movimenti";
			movimentiGioco.color = Color.yellow;
		}
		else {
			//aggiungere cose
			if (puntiPlayer1 > puntiPlayer2)
				vittoriaPlayer1.SetActive (true);
			else if (puntiPlayer1 < puntiPlayer2)
				vittoriaPlayer2.SetActive (true);
			else
				pareggio.SetActive (true);
		}

	}

	public void pulisciScena(){

		movimentiGioco.text = "Movimenti: 0";
		vittoriaSinglePlayer.SetActive(false);
		vittoriaPlayer1.SetActive(false);
		vittoriaPlayer2.SetActive(false);;
		pareggio.SetActive(false);
		player1mugshotActive.SetActive (false);
		player2mugshotActive.SetActive(false);
		player1mugshotBN.SetActive (false);
		player2mugshotBN.SetActive(false);
		carta = 0;
		movimenti = 0;
		puntiPlayer1 = 0;
		puntiPlayer2 = 0;
		coppieTrovate = 0;
		endGame = false;
		haiVintoCon.gameObject.SetActive(false);
		movimentiGioco.color = Color.black;
		movimentiGioco.gameObject.SetActive (false);
		foreach (Transform child in spacer)
			Destroy (child.gameObject);
		
		foreach (Transform child in mazzettoPlayer1.transform)
			child.gameObject.SetActive (false);
		foreach (Transform child in mazzettoPlayer2.transform)
			child.gameObject.SetActive (false);
		
	}

	public void generator(){


		foreach (Transform child in spacer)
			Destroy (child.gameObject);
		
		if (DifficoltaScelta == 0)
			spacer.gameObject.GetComponent<GridLayoutGroup> ().constraintCount = 3;
		else 
			spacer.gameObject.GetComponent<GridLayoutGroup> ().constraintCount = 4;

		List<Sprite> immaginiCarte = new List<Sprite> ();
		bool trovato;

		if (DifficoltaScelta == 0) {
			cardNumber = 6;
		} else if (DifficoltaScelta == 1) {
			cardNumber = 8;
		} else {
			cardNumber = 12;
		}
		//scelgo immagini in modo casuale le aggiungo ad un array 2 volte 
		do{
			trovato=false;
			Sprite s = immagini[Random.Range (0, immagini.Count)];
			for(int i=0; i < immaginiCarte.Count;i++)
				if(immaginiCarte[i].name.Equals(s.name))
					trovato=true;
			if(!trovato){
				immaginiCarte.Add (s);
				immaginiCarte.Add (s);
			}

		}while(immaginiCarte.Count < cardNumber);

		//poi le shuffle 
		Shuffle(immaginiCarte);
		//e le instanzio aggiungendole allo spacer che si occuperà di posizionarle
		for(int i=0; i < immaginiCarte.Count;i++){
			GameObject clone = Instantiate(baseCard,spacer);
			clone.transform.GetChild (0).GetComponent<Image> ().sprite = immaginiCarte [a[i]];
			clone.transform.GetChild (1).GetComponent<Image> ().sprite = retro[Random.Range(0,retro.Count)];
			clone.SetActive (true);
		}


	}

	public void controllaCarteUguali(){
		
		canTouchPanel.SetActive (true);
		movimenti++;
		movimentiGioco.text = "Movimenti: " + movimenti;

		if (card1.Equals (card2)) {
			coppieTrovate++;
			if (coppieTrovate == cardNumber / 2)
				endGame = true;
			if (Giocatori == 2) {
				if (turno == 0) {
					mazzettoPlayer1.transform.GetChild (puntiPlayer1).GetComponent<Image> ().sprite = card1GO.transform.GetChild (0).GetComponent<Image> ().sprite;
					mazzettoPlayer1.transform.GetChild (puntiPlayer1).gameObject.SetActive (true);
					puntiPlayer1++;
				} else {
					mazzettoPlayer2.transform.GetChild (puntiPlayer2).GetComponent<Image> ().sprite = card1GO.transform.GetChild (0).GetComponent<Image> ().sprite;
					mazzettoPlayer2.transform.GetChild (puntiPlayer2).gameObject.SetActive (true);
					puntiPlayer2++;
				}
			}
			card1GO = null;
			card2GO = null;
		}
		else {
			card1GO.GetComponent<Animator>().Play("FlipcardToRetro");
			card1GO = null;
			card1 = null;
			card2GO.GetComponent<Animator>().Play("FlipcardToRetro");
			card2GO = null;
			card2 = null;
			if (Giocatori == 2)
				turno++;
			if (turno > 1)
				turno = 0;
				
		}
		
		canTouchPanel.SetActive (false);
	}

	public void flipCardToFace(){
		
		canTouchPanel.SetActive (true);
		EventSystem.current.currentSelectedGameObject.transform.parent.GetComponent<Animator>().Play("FlipcardToFace");
		if (carta == 0) {
			card1 = EventSystem.current.currentSelectedGameObject.transform.parent.GetChild (0).GetComponent<Image> ().sprite.name;
			card1GO = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
			canTouchPanel.SetActive (false);
		}
		else if(carta == 1) {
			card2 = EventSystem.current.currentSelectedGameObject.transform.parent.GetChild (0).GetComponent<Image> ().sprite.name;
			card2GO = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
			canTouchPanel.SetActive (true);
		}
		carta++;
		if (carta > 1)
			carta = 0;
	}

	public void Shuffle( List<Sprite> list){

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

	public void OnePlayer(){
		Giocatori = 1;
	}

	public void TwoPlayer(){
		Giocatori = 2;
	}


	public void ChoseDifficultyEasy(){
		DifficoltaScelta = 0;
		pulisciScena ();

	}

	public void ChoseDifficultyMedium(){
		DifficoltaScelta = 1;
		pulisciScena ();
	}

	public void ChoseDifficultyHard(){
		DifficoltaScelta = 2;
		pulisciScena ();
	}

	public void logoutGame(){

		SceneManager.LoadScene ("ScenaScelta",LoadSceneMode.Single);

	}
}
