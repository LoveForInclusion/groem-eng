using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Figure : MonoBehaviour {

	
	public AudioClip clip;
	public float delay = 0;
	private SceneController controller;
	public EntityAction[] actions;
	private int rip; //ripetizioni Tab
	private OpzioniAudio opzioniAudio; //!!!!!!!!!!! MODIFICARE: prendere dal PLAYER	!!!!!!!!!!

	void Start () {
		
		rip = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetScenecontroller(SceneController sceneController, int num){
		controller = sceneController;
		this.GetComponent<Button>().onClick.AddListener(delegate(){
			controller.PlayFigure(num);
		});
	}

	public AudioClip GetClip(){
		return clip;
	}

	public void setReps(int reps){
		this.rip = reps;
	}

	public int GetReps(){
		return rip;
	}


	public void playTab(){
		if(opzioniAudio.option.standard || opzioniAudio.option.tasti){
			if(controller != null){
				if(rip <= opzioniAudio.option.ripetizioni){
					controller.SetAudioSource(clip);
					controller.StartCoroutine("SingleFigure", this);
					rip++;
				}
			} else {
				if(rip <= opzioniAudio.option.ripetizioni){
					this.transform.GetComponent<AudioSource>().PlayOneShot(clip);
					this.transform.GetComponent<Animator>().Play("buttonBounce");
					rip++;
				}
			}
		}
	}


}
