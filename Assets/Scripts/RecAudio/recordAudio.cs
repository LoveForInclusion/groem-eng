using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class recordAudio : MonoBehaviour {

	public AudioProfile audioprofile;
	public OpzioniAudio opzioniaudio;

	public static bool rec=false;
	public static bool onPlay=false;
	public AudioSource canv;

	float timer = 0.0f;
	float timerRip = 0.0f;
	int intTimer = 0;
	string idStoria = "";

	float clipDuration = 0f;

	bool playclip = false;


	void Start(){
		caricaIdStoria ();
		//prendo la classe contenuta nel canvas
		audioprofile = gameObject.transform.parent.parent.parent.parent.GetComponent<AudioProfile> ();
		opzioniaudio = gameObject.transform.parent.parent.parent.parent.GetComponent<OpzioniAudio> ();
	}

	void Update(){
		//conta quanto tempo registro
		if (rec)
			timer += Time.deltaTime;

		if (playclip) {
			timerRip += Time.deltaTime;
			transform.GetChild (1).GetChild (1).gameObject.GetComponent<Image> ().fillAmount = (timerRip%60)/clipDuration;
			if (clipDuration-timerRip <= 0f) {
				playclip = false;
				timerRip = 0f;
				transform.GetChild (1).GetChild (1).gameObject.GetComponent<Image> ().fillAmount = 1;
				onPlay = false;
			}

		}

	}

	public void caricaIdStoria(){
		//carico la directory dei salvataggi
		string destinationUtente = Application.persistentDataPath +"/myIdStoria.dat";
		FileStream fileUtente;

		//se i file esistono li apre
		if (File.Exists (destinationUtente))
			fileUtente = File.OpenRead (destinationUtente);
		else {
			idStoria = null;
			return;
		}

		//leggo i file locali
		BinaryFormatter bf = new BinaryFormatter ();

		idStoria = (string)bf.Deserialize (fileUtente);

		fileUtente.Close ();
	}

	public void recAudio(){
		
		if (!rec && !onPlay) {
			//inizio registrazione
			timer = 0f;
			transform.GetChild (1).GetChild (1).gameObject.SetActive(false);
			transform.GetChild (1).GetChild (3).gameObject.SetActive(false);

     		canv.clip = Microphone.Start (null, false, 30, 44100);

			rec = true;


			//Debug.Log ("Inizio registrazione");
		} else if(!transform.GetChild (1).GetChild (3).gameObject.activeSelf){
			//Debug.Log ("Fine registrazione");
			//fermo registrazione

			transform.GetChild (1).GetChild (1).gameObject.SetActive(true);
			transform.GetChild (1).GetChild (3).gameObject.SetActive(true);

			/*
			 *Commentare in caso di build web GL 
			 */

			Microphone.End (null);


			//stop cronometro
			rec = false;
			//converto in secondi
			intTimer = (int)(timer % 60) + 1;
			//resetto il timer
			timer = 0f;


			string difficolta = audioprofile.getMyDropdownDifficoltaString();

			string profilo="";

			if (audioprofile.MyDropdownDifficolt.GetComponentInChildren<Text>().text.Equals(audioprofile.easyDicitur)){
				//sostituire con il nome del figlio che è un numero
				profilo = opzioniaudio.option.nomiProfiliOriginaliEasy[audioprofile.profiloScelto];
			}
			else{
				profilo = opzioniaudio.option.nomiProfiliOriginaliAdvanced[audioprofile.profiloScelto];
			}

			string nomeFile = gameObject.name+".dat";  

			string path = Path.Combine (Application.persistentDataPath, "ProfiliAudio");
			path = Path.Combine (path, idStoria);
			path = Path.Combine (path, difficolta);
			path = Path.Combine (path, profilo);
			string pathFile = Path.Combine (path, nomeFile);


			Debug.Log (path);
			Debug.Log (pathFile);
			Debug.Log (intTimer);
			Debug.Log (canv.clip);



			AudSav.SaveAudioClipToDisk (canv.clip,path,pathFile,intTimer);

			canv.clip = null;

		}

	}

	public void playAudio(){

		if (!onPlay && !rec) {
			canv.clip = null;

			string difficolta = audioprofile.getMyDropdownDifficoltaString ();

			string profilo = "";

            if (audioprofile.MyDropdownDifficolt.GetComponentInChildren<Text>().text.Equals(audioprofile.easyDicitur))
            {
                //sostituire con il nome del figlio che è un numero
                profilo = opzioniaudio.option.nomiProfiliOriginaliEasy[audioprofile.profiloScelto];
            }
            else
            {
                profilo = opzioniaudio.option.nomiProfiliOriginaliAdvanced[audioprofile.profiloScelto];
            }

			string nomeFile = gameObject.name + ".dat";  

			string path = Path.Combine (Application.persistentDataPath, "ProfiliAudio");
			path = Path.Combine (path, idStoria);
			path = Path.Combine (path, difficolta);
			path = Path.Combine (path, profilo);
			path = Path.Combine (path, nomeFile);

			Debug.Log (path);

			clipDuration = AudSav.LoadAudioClipFromDisk (canv, path, nomeFile);
		 
			Debug.Log (clipDuration);

			transform.GetChild (1).GetChild (3).gameObject.GetComponent<Image> ().fillAmount = 0;
			playclip = true;

			canv.Play ();
			onPlay = true;
		}
	}


}
