using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

public class AudSav : MonoBehaviour {

	[System.Serializable]
	class AudioClipSimple{

		public int frequency;
		public int samples;
		public int channels;
		public float[] sample;
		public int duration;

	}

	public static void SaveAudioClipToDisk(AudioClip audioclip,string path,string pathFile,int tempoRec){

		//crea file
		BinaryFormatter bf = new BinaryFormatter();

		FileStream file=null;
		//= File.Create (Application.persistentDataPath+"/"+filename);

		//se i file esistono li apre

		if (Directory.Exists (path)) {
			if (File.Exists (pathFile)) {

				File.Delete (pathFile);
			
				file = File.OpenWrite (pathFile);

				Debug.Log ("openwrite");
			}
			else {
				file = File.Create (pathFile);
			}
		}
		else {

			Directory.CreateDirectory (path);
			file = File.Create (pathFile);

		}
			
		//serializza 
		AudioClipSimple newSimple = new AudioClipSimple ();

		//audioclip.samples è quanto dovrà essere salvato sul disco 
		//quindi si deve calcolare il tempo tra la pressione del tasto rec di avvio e stop
		//e sostituire con frequenza * secondi
		newSimple.sample = new float[audioclip.frequency* tempoRec * audioclip.channels];
		newSimple.frequency = audioclip.frequency;
		newSimple.samples = audioclip.samples;
		newSimple.channels = audioclip.channels;
		newSimple.duration = tempoRec;

		audioclip.GetData (newSimple.sample, 0);

		Debug.Log (file);
		bf.Serialize (file,newSimple);
		file.Close ();
	}

	public static int LoadAudioClipFromDisk(AudioSource audiosource,string path,string filename){

		if(File.Exists(path)){

			//deserializzo
			BinaryFormatter bf = new BinaryFormatter();

			FileStream file = File.Open (path,FileMode.Open);
			AudioClipSimple clipSample = (AudioClipSimple)bf.Deserialize (file);
			file.Close ();


			AudioClip newClip = AudioClip.Create (filename,clipSample.samples,clipSample.channels,clipSample.frequency,false);
			newClip.SetData (clipSample.sample,0);

			//set to AudioSource
			audiosource.clip = newClip;
			return clipSample.duration;
		}
		return 0;

	}
}
