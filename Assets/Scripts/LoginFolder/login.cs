using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using Model;
using RESTClient.Request;

public class login : MonoBehaviour {
	public GameObject panelLoading;

	public GameObject ErrorPanel;
	public Text ErrorMessage;

	public void autoLogin(){
		
		panelLoading.SetActive (true);

		UserRequest.RefreshToken (this,
			delegate(Token token){
				if(token.token == null){
					panelLoading.SetActive(false);
					ErrorMessage.text = "Rieseguire Login";
					ErrorPanel.SetActive(true);
				}
				else{
					Debug.Log("new token:" + token);
					SceneManager.LoadSceneAsync ("ScenaLibreria",LoadSceneMode.Single);
					//SceneManager.LoadScene ("ScenaLibreria",LoadSceneMode.Single);
				}
			},delegate(string code, string error) { 
				var tokenValue = PlayerPrefs.GetString("token");
				if (!tokenValue.Equals("")) {
					SceneManager.LoadScene ("ScenaLibreria",LoadSceneMode.Single);
				}
				else{
				panelLoading.SetActive(false);
				ErrorMessage.text = "Riprovare più tardi";
				ErrorPanel.SetActive(true);
				Debug.Log("errore codice: " + code + " errore: " + error + " errore login"); 
				}
			});
	}


	//Login utente
	public void LoginPressed(string mail,string password){

		panelLoading.SetActive (true);
		PlayerPrefs.SetString("userMail",mail);
		UserRequest.Login (this, new UserInput(mail, password),
			delegate(Token token){
				if(token.token == null){
					panelLoading.SetActive(false);
					ErrorMessage.text = "Username o Password errati";
					ErrorPanel.SetActive(true);
				}
				else{
					//Debug.Log("token:" + token);
					SceneManager.LoadScene ("ScenaLibreria",LoadSceneMode.Single);
				}
		},delegate(string code, string error) { 

				if(error.Contains("401")){
					panelLoading.SetActive(false);
					ErrorMessage.text = "Mail o password errata";
					ErrorPanel.SetActive(true);
				}else {
					panelLoading.SetActive(false);
					ErrorMessage.text = "Errore di connesione, riprovare più tardi";
					ErrorPanel.SetActive(true);
				}
				Debug.Log("errore codice: " + code + " errore: " + error + " errore login"); 
		});
	}


	//Registrazione utente
	public void SinginPressed(string mail,string password){
		
		panelLoading.SetActive (true);

		UserRequest.SignupUser (this, new User(null,mail,password,"","",null),
			delegate(User user){
				
				LoginPressed(mail,password);

			},delegate(string code, string error) { 
				panelLoading.SetActive(false);

				if(error.Contains("Email Already Taken")){
					ErrorMessage.text = "Mail già utilizzata.\nControllare le credenziali.\n(Password dimenticata?)";
				} else {
					ErrorMessage.text = "Errore di connesione, riprovare più tardi";
				}
				ErrorPanel.SetActive(true);
				Debug.Log("errore codice: " + code + " errore: " + error + " errore login"); 
			});
	}

	//Recupera Password utente
	public void ForgotPasswordPressed(string mail){
		panelLoading.SetActive (true);

		UserRequest.RecoveryPassword (this, mail,
			delegate(string s){
				
				panelLoading.SetActive(false);
				ErrorMessage.text = "Controlla la tua casella di posta elettronica \n ti è stata inviata la tua nuova password";
				ErrorPanel.SetActive(true);
				Debug.Log(s);

			},delegate(string code, string error) { 
				panelLoading.SetActive(false);
				ErrorMessage.text = "Errore di connesione, riprovare più tardi";
				ErrorPanel.SetActive(true);
				Debug.Log("errore codice: " + code + " errore: " + error + " errore login"); 
			});

	}

	void Start(){

		var tokenValue = PlayerPrefs.GetString("token");
		if (!tokenValue.Equals("")) {
			autoLogin ();
		}

		Debug.Log (Application.persistentDataPath);
	}

}
