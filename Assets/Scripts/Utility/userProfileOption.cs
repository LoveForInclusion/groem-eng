using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using RESTClient;
using RESTClient.Request;
using UnityEngine.Networking;
using Model;

public class userProfileOption : MonoBehaviour {

	public InputField name;
	public InputField surname;
	public InputField pass;
	public InputField confPass;
	public InputField mail;
	public GameObject haveDeleted;
	public GameObject panelMessage;
	public Text t;

	float timer;

	void Start(){
		timer = 1.5f;
		mail.text = PlayerPrefs.GetString ("userMail");

		UserRequest.GetUser (this,
			delegate (User user){
				if(name!=null && surname!=null){
				name.text = user.firstName;
				surname.text = user.lastName;
				}
			},delegate {
				Debug.Log("Connessione al server fallita,informazoni non reuperate");
			});

	}

	public void deleteAllFile(){
		if (Directory.Exists (Application.persistentDataPath + "/dir")) {
			DeleteDirectory (Application.persistentDataPath + "/dir");
			//visualizza eliminazione completata
			haveDeleted.SetActive (true);
		} else {
				panelMessage.SetActive (true);
				t.text = "Nulla da eliminare";
			}
	}

	void Update(){

		if (haveDeleted.activeSelf) {
			if (timer <= 0) {
				haveDeleted.SetActive (false);
				timer = 1.5f;
			}
			else
				timer -= Time.deltaTime;
		}
	}

	public void timerToZero(){
		timer = 0f;
		haveDeleted.SetActive (false);
	}

	public static void DeleteDirectory(string target_dir)
	{
			string[] files = Directory.GetFiles (target_dir);
			string[] dirs = Directory.GetDirectories (target_dir);

			foreach (string file in files) {
				File.SetAttributes (file, FileAttributes.Normal);
				File.Delete (file);
			}

			foreach (string dir in dirs) {
				DeleteDirectory (dir);
			}

			Directory.Delete (target_dir, false);
	}

	public void aggiornaProfilo(){
		if(name.text.Replace(" ","").Equals("")){
			panelMessage.SetActive (true);
				t.text = "Campo nome non valido.";
		}
		else if(surname.text.Replace(" ","").Equals("")){
			panelMessage.SetActive (true);
				t.text = "Campo cognome non valido.";
		}
		else if(pass.text.Replace(" ","").Equals("")){
			panelMessage.SetActive (true);
				t.text = "Campo password non valido.";
		}
		else if(!pass.text.Equals (confPass.text)){
			panelMessage.SetActive (true);
				t.text = "Campo conferma password non valido.";
		}
		else if(pass.text.Equals (confPass.text)){

			UserRequest.GetUser (this,delegate (User user){
				//modifica password user
				user.password = pass.text;
				user.firstName = name.text;
				user.lastName = surname.text;
				Debug.Log(PlayerPrefs.GetString("token"));
				Debug.Log(JsonUtility.ToJson(user));

				UserRequest.UserUpdate(this,user,
				
				delegate(User newuser){
					
					Debug.Log("Password cambiata in: " + newuser.password);
					Debug.Log("Nome cambiato in: " + newuser.firstName);
					Debug.Log("Cognome cambiato in: " + newuser.lastName);
					panelMessage.SetActive (true);
					t.text = "Modifiche salvate con successo.";
		

				},delegate(string errore , string errore2){
					panelMessage.SetActive (true);
					t.text = "Modifica fallita, riprovare.";
					Debug.Log("Password non cambiata, " + errore+" "+errore2);
				});
			},delegate {
				panelMessage.SetActive (true);
				t.text = "Modifica fallita, riprovare.";
			});

		}


		/*byte[] myData = System.Text.Encoding.UTF8.GetBytes("This is some test data");
		UnityWebRequest www = UnityWebRequest.Put("https://httpbin.org/put", myData);
		yield return www.SendWebRequest();

		if(www.isNetworkError || www.isHttpError) {
			Debug.Log(www.error);
		}
		else {
			Debug.Log(www.downloadHandler.text);
		}*/


		/*ApiClient.Put (this,"https://httpbin.org/put",null,null,delegate(WWW www){
			Debug.Log(www.text);
			
		});*/
	}

	public void openTadaWeb(){
		Application.OpenURL ("http://www.tadabook.it");
	}


	
}