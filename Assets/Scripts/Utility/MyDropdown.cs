using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyDropdown : MonoBehaviour {

	public AudioProfile aud;
	public Button bottonePadre;
	public GameObject panel;

	public void drop(){
		panel.SetActive (!panel.activeSelf);
	}

	public void clickChildren(GameObject butt){

		bottonePadre.GetComponentInChildren<Text> ().text = butt.GetComponentInChildren<Text> ().text;
		bottonePadre.name = butt.name;
		aud.numProfiloScelto ();
		drop ();
		
	}

}
