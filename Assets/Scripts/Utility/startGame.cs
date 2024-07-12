using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startGame : MonoBehaviour {

	public GameObject clip;

	IEnumerator Start(){

		Input.multiTouchEnabled = false;

		yield return Application.RequestUserAuthorization (UserAuthorization.Microphone);
		if (Application.HasUserAuthorization (UserAuthorization.Microphone)) {
		} else {
		}
	}
	

	void Update () {

		if(this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("endStart2")){
			clip.SetActive (true);
			StartCoroutine("delayStart", clip.GetComponent<AudioSource> ().clip.length);
		}
	}

	IEnumerator delayStart(float delay){
		//Aspetto che laudio finisce e switcho scena
		yield return new WaitForSeconds (delay);

		if (!PlayerPrefs.GetString ("token").Equals("")) {
			SceneManager.LoadScene ("ScenaLibreria",LoadSceneMode.Single);
		} else {
			this.GetComponent<SceneLoader> ().loadLogin ();
		}

	}

}
