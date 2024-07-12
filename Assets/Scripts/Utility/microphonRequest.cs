using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class microphonRequest : MonoBehaviour {

	public Text a;

	IEnumerator Start() {
		yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
		if (Application.HasUserAuthorization(UserAuthorization.Microphone)) {
			a.text = "true";
		} else {
			a.text = "false";
		}
	}
}
