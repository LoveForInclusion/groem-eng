using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour {

	System.Text.RegularExpressions.Regex mailValidator = new System.Text.RegularExpressions.Regex(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$");

	public login newlog;

	//Canvas e inputField passati da Unity 
	public Canvas loginCanvas;
	public Canvas singinCanvas;
	public Canvas forgotPassword;

	public InputField singinMail;
	public InputField singinPassword;
	public InputField singinConfirmPassword;

	public InputField forgotPasswordMail;

	public InputField loginMail;
	public InputField loginPassword;

	public GameObject ErrorPanel;
	public Text ErrorMessage;


	//funzione per il login chiamata da unity 
	public void loginFunction(){

		if (IsValidEmail(loginMail.text.ToLower())) {
			ErrorMessage.text = "Mail non valida";
			ErrorPanel.SetActive (true);
			Debug.Log ("mail non valida");
		} else {
			newlog.LoginPressed (loginMail.text.ToLower(), loginPassword.text);
		}
	}

	//funzione per il singin chiamata da unity
	public void singinFunction(){

		if (IsValidEmail (singinMail.text.ToLower())) {
			ErrorMessage.text = "Mail non valida";
			ErrorPanel.SetActive (true);
			Debug.Log ("mail non valida");
		} else if (!singinPassword.text.Equals (singinConfirmPassword.text)) {
			ErrorMessage.text = "Password errata";
			ErrorPanel.SetActive (true);
		}
		else {
			newlog.SinginPressed (singinMail.text.ToLower(),singinPassword.text);
		}

	}

	//funzione password dimenticata chiamata da unity 
	public void forgotPasswordFunction(){

		if (IsValidEmail (forgotPasswordMail.text.ToLower())) {
			ErrorMessage.text = "Mail non valida";
			ErrorPanel.SetActive (true);
		} else {
			newlog.ForgotPasswordPressed (forgotPasswordMail.text.ToLower());
		}
	}


	//Controlla se la mail è valida
	bool IsValidEmail(string email)
	{
		return !mailValidator.IsMatch (email.ToLower());
	}



	//Ritorna al Canvas della schermata di login
	public void backCanvas(){

		//Attiva il canvas di login e disattiva gli altri
		singinCanvas.gameObject.SetActive (false);
		forgotPassword.gameObject.SetActive (false);
		loginCanvas.gameObject.SetActive (true);
	}

	//Visualizza il Canvas della schermata della registrazione account
	public void registratiCanvas(){

		//Attiva il canvas della registrazione e disattiva gli altri
		forgotPassword.gameObject.SetActive (false);
		loginCanvas.gameObject.SetActive (false);
		singinCanvas.gameObject.SetActive (true);
		
	}

	//Visualizza il Canvas della schermata del recupero password
	public void recuperaPasswordCanvas(){

		//Attiva il canvas del recupero password e disattiva gli altri
		loginCanvas.gameObject.SetActive (false);
		singinCanvas.gameObject.SetActive (false);
		forgotPassword.gameObject.SetActive (true);
	}

	//Visualizzare come schermata il Canvas Login
	void Start () {
		backCanvas ();
	}


}
