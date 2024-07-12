using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFigureAntiMultiTouch : MonoBehaviour {

	public Transform io, voglio, randomString;

	public AudioSource sottofondoAudio;
	// Update is called once per frame

	public void IOPlay(){
		if (!io.transform.GetComponent<FigureController> ().mute) {
			voglio.transform.GetComponent<FigureController> ().mute = true;
			randomString.transform.GetComponent<FigureController> ().mute = true;
			sottofondoAudio.volume=0f;
			StartCoroutine ("resetAudio", io.transform.GetComponent<FigureController> ().audioTest.length);
		}
	}

	public void VOGLIOPlay(){
		if (!voglio.transform.GetComponent<FigureController> ().mute) {
			io.transform.GetComponent<FigureController> ().mute = true;
			randomString.transform.GetComponent<FigureController> ().mute = true;
			sottofondoAudio.volume=0f;
			StartCoroutine ("resetAudio", voglio.transform.GetComponent<FigureController> ().audioTest.length);
		}
	}

	public void RANDOMSTRINGPlay(){
		if (!randomString.transform.GetComponent<FigureController> ().mute) {
			voglio.transform.GetComponent<FigureController> ().mute = true;
			io.transform.GetComponent<FigureController> ().mute = true;
			sottofondoAudio.volume=0f;
			StartCoroutine ("resetAudio", randomString.transform.GetComponent<FigureController> ().audioTest.length);
		}
	}

	IEnumerator resetAudio(float delay){

		yield return new WaitForSeconds (delay);

		io.transform.GetComponent<FigureController> ().mute = false;
		voglio.transform.GetComponent<FigureController> ().mute = false;
		randomString.transform.GetComponent<FigureController> ().mute = false;
		sottofondoAudio.volume=1f;

	}

}
