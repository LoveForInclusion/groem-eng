using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {

	public GameObject[] figures;
	private AudioSource audioTab;
	private OpzioniAudio opzioniAudio; //!!!!!!!!!!! MODIFICARE: prendere dal PLAYER	!!!!!!!!!!

	protected Player player;
	int dur;
	int ind;

	void Start () {
		
		SetupController();
	}

	protected virtual void SetupController(){
		audioTab = this.GetComponent<AudioSource>();
		if(Camera.main.transform.GetComponent<Player>()!=null){
			player = Camera.main.transform.GetComponent<Player>();
			this.opzioniAudio = player.opzioniAudio;
		}
		
		for(int i = 0; i < figures.Length; i++){
			figures[i].GetComponent<Figure>().SetScenecontroller(this, i);		
		}
		Debug.Log("Trovato tutto.");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayFigure(int num){
		//Debug.Log(num);
		
		if(num > -1){
			if(figures[num].GetComponent<Figure>().GetReps() < player.opzioniAudio.option.ripetizioni){
				IEnumerator ie;
				if(player.UIControl.donTouch.activeSelf)
					ie = SingleFigure(figures[num], false);	
				else{
					player.UIControl.donTouch.SetActive(true);
					ie = SingleFigure(figures[num], true);
				}
				StartCoroutine(ie);
				figures[num].GetComponent<Figure>().setReps(figures[num].GetComponent<Figure>().GetReps()+1);
				}
		} else {
			StartCoroutine(PlayAndWait());
		}
	}

	IEnumerator PlayAndWait(){
		//Debug.Log("P&W");
		//Debug.Log("Starto?");
		yield return null;
		if(setAudioPers()){
			audioTab.Play();
			Debug.Log(audioTab.clip.name);
		}
		yield return new WaitForSeconds(dur);
		//Debug.Log("Suonato");
		//manda informazioni al player
		player.ReadyController();
		
	}

	IEnumerator SingleFigure(GameObject figure, bool isSingle){
		//Debug.Log("SF");
		for(int i = 0; i<figure.GetComponent<Figure>().actions.Length; i++){
			player.Action(figure.GetComponent<Figure>().actions[i], i);
		}
		//yield return null;
		figure.GetComponent<Animator>().Play("buttonBounce");

		if(player.opzioniAudio.option.standard || player.opzioniAudio.option.tasti){
			audioTab.clip = figure.GetComponent<Figure>().clip;
		} else {
			audioTab.clip = null;
		}

		audioTab.Play();
		yield return new WaitWhile(() => audioTab.isPlaying /*|| figure.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("buttonBounce")*/);
		//Debug.Log("!");
		//manda informazioni al player
		if(/*figure.GetComponent<Figure>().delay != null ||*/ figure.GetComponent<Figure>().delay != 0){//If this figure is given a delay override controller to use this delay duration
			yield return new WaitForSeconds(figure.GetComponent<Figure>().delay);
			player.OverrideController();
		} else {
			player.ReadyController();
		}

		if(isSingle)
			player.UIControl.donTouch.SetActive(false);
	}







	public void SetAudioSource(AudioClip clip){
		this.audioTab.clip = clip;
	}

	public bool setAudioPers(){
		
		string profilo;

		if(player.opzioniAudio.option.tipoLettura == 1)
			profilo = player.opzioniAudio.option.nomiProfiliOriginaliAdvanced[player.opzioniAudio.option.profileSelected];
		else 
			profilo = player.opzioniAudio.option.nomiProfiliOriginaliEasy[player.opzioniAudio.option.profileSelected];
		
		string nomeFile = (player.GetCurrentScene() - 1) + ".dat";

		string difficolta = player.opzioniAudio.option.tipoLettura == 1 ? "Difficile" : "Facile";

		string path = System.IO.Path.Combine(Application.persistentDataPath, "ProfiliAudio");
		path = System.IO.Path.Combine(path, AssetHandler.caricaIdStoria());
		path = System.IO.Path.Combine(path, difficolta);
		path = System.IO.Path.Combine(path, profilo);
		path = System.IO.Path.Combine(path, nomeFile);

		Debug.Log(path);
		//Debug.Log(player.GetCurrentScene);
		if(File.Exists(path))
		dur = AudSav.LoadAudioClipFromDisk(this.audioTab, path, nomeFile);
		if(dur == 0)
			return false;
		Debug.Log(dur + "");
		return true;
	}

}
