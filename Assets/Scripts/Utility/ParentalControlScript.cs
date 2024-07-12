using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ParentalControlScript : MonoBehaviour {

	public Text domanda;

	public List<Button> risposte;
	public GameObject panel;

	int rispostaCorretta;

	public void generaOperazione(){

		int operatore = Random.Range (0 , 2);

		string op;


		int numero1 = Random.Range (1, 10);
		int numero2 = Random.Range (1, 10);


		if (operatore == 1) {
			while (numero1 % numero2 != 0) {
				numero2 = Random.Range (1, 10);
			}
		}

		if (operatore == 0) {
			op = " x ";
			rispostaCorretta = numero1 * numero2;
		} else {
			op = " : ";
			rispostaCorretta = numero1 / numero2;
		}


		domanda.text = numero1 + op + numero2;

		int pulsanteCorretto = Random.Range (0 , 4);

		for(int i =0 ; i < risposte.Count;i++){
			int possibileRisposta;

			do {
				
				possibileRisposta = Random.Range (1, 10) * Random.Range (1, 10);
				
			} while(possibileRisposta == rispostaCorretta);

			risposte [i].gameObject.GetComponentInChildren<Text> ().text = possibileRisposta + "";
			
		}

		risposte [pulsanteCorretto].gameObject.GetComponentInChildren<Text> ().text = rispostaCorretta + "";

	}
		
	public void risposta1(int r){
		if (int.Parse (risposte [r].gameObject.GetComponentInChildren<Text> ().text) == rispostaCorretta) {
			SceneManager.LoadScene ("AudioRec", LoadSceneMode.Single);
		} else {
			this.gameObject.SetActive (false);
		}
	}

	public void risposta2(int r){
		if (int.Parse (risposte [r].gameObject.GetComponentInChildren<Text> ().text) == rispostaCorretta) {
			this.gameObject.SetActive (false);
			panel.SetActive (true);
		} else {
			this.gameObject.SetActive (false);
		}
	}

}
