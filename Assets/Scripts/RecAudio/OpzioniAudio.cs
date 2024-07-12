using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


[System.Serializable]
public class OpzioniAudio : MonoBehaviour {

	[System.Serializable]
	public class OpzioniAllocate{

	//array nomi profili
		public string[] nomiProfiliModificatiEasy={"Semplice1","Semplice2","Semplice3"};//Aumentati
		public string[] nomiProfiliModificatiAdvanced = { "Elaborato1", "Elaborato2", "Elaborato3" };

	//array nomi cartelle per gli audio
        public string[] nomiProfiliOriginaliEasy={"Semplice1","Semplice2","Semplice3"};//Aumentati
        public string[] nomiProfiliOriginaliAdvanced = { "Elaborato1", "Elaborato2", "Elaborato3" };

	//bool standard tasti muto
	public bool standard = true;
	public bool tasti = false;
	public bool muto = false;

	//personalizzata 
	public bool personalizzata = false;
	public int profileSelected=0;

	//numero ripetizioni
	public int ripetizioni = 5;

	//0 semplice 1 avanzato
	public int tipoLettura = 0;

	}

	public OpzioniAllocate option=null;
	public bool isloaded = false;
	public string idStoria = "";

	public void caricalID(){
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

	public void caricaOpzioniAudio(){

		caricalID ();

		//carico la directory dei salvataggi
		string destinationUtente = Application.persistentDataPath +"/ProfiliAudio/"+ idStoria +"/Opzioni.dat";
		FileStream fileUtente;

		//se i file esistono li apre
		if (File.Exists (destinationUtente))
			fileUtente = File.OpenRead (destinationUtente);
		else {
			Debug.Log("File not found, creating new file.");
			option = new OpzioniAllocate();
			salvaOpzioniAudio();
			caricaOpzioniAudio();
			return;
		}

		//leggo i file locali
		BinaryFormatter bf = new BinaryFormatter ();

		option = (OpzioniAllocate)bf.Deserialize(fileUtente);

		fileUtente.Close ();
		isloaded = true;

	}

	public void salvaOpzioniAudio(){

		caricalID ();

		//dichiaro le directory 
		string destinationToken = Application.persistentDataPath +"/ProfiliAudio/"+ idStoria +"/Opzioni.dat";

		if(!Directory.Exists(Application.persistentDataPath +"/ProfiliAudio/"+ idStoria))
			Directory.CreateDirectory (Application.persistentDataPath +"/ProfiliAudio/"+ idStoria);

		FileStream fileToken= null;

		//se i file esistono li apre
		if (File.Exists (destinationToken))
			fileToken = File.OpenWrite (destinationToken);
		else
			fileToken = File.Create (destinationToken);

		//salvo i file in locale
		BinaryFormatter bf = new BinaryFormatter ();

		bf.Serialize (fileToken,option);
		fileToken.Close ();

	}

		
}
