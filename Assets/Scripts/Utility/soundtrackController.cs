using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class soundtrackController : MonoBehaviour {

	[System.Serializable]
	public class Muto{
		public bool muto;
	};

	public Muto m = new Muto();
	public Sprite soundOn;
	public Sprite soundOff;
	public GameObject soundObject;

	void Start(){
		caricaOpzioniAudioSottofondo ();
	}

	void Update(){

		if (!m.muto)
			soundObject.GetComponent<Image> ().sprite = soundOn;
		else
			soundObject.GetComponent<Image> ().sprite = soundOff;

		soundObject.GetComponent<AudioSource> ().mute = m.muto;
	}


	public void onOff(){
		m.muto = !m.muto;
		salvaOpzioneAudioSottofondo ();
	}

	public void settaOff(){
		m.muto = true;
		salvaOpzioneAudioSottofondo ();
	}

	public void caricaOpzioniAudioSottofondo(){
		//carico la directory dei salvataggi
		string destinationUtente = Application.persistentDataPath +"/mute.dat";
		FileStream fileUtente;

		//se i file esistono li apre
		if (File.Exists (destinationUtente))
			fileUtente = File.OpenRead (destinationUtente);
		else {
			m.muto = false;
			return;
		}

		//leggo i file locali
		BinaryFormatter bf = new BinaryFormatter ();

		m = (Muto)bf.Deserialize (fileUtente);

		fileUtente.Close ();
	}

	public void salvaOpzioneAudioSottofondo(){
		
		//dichiaro le directory 
		string destinationToken = Application.persistentDataPath + "/mute.dat";
		FileStream fileToken= null;

		//se i file esistono li apre
		if (File.Exists (destinationToken))
			fileToken = File.OpenWrite (destinationToken);
		else
			fileToken = File.Create (destinationToken);

		//salvo i file in locale
		BinaryFormatter bf = new BinaryFormatter ();

		bf.Serialize (fileToken,m);
		fileToken.Close ();
	}
}