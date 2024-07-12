using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureController : MonoBehaviour {

	public bool mute=false;

	public int touch_limit=5;
	public int my_touch;
	public AudioClip audioTest;
	public static AudioSource playerFigure;
	public  AudioSource playerFigureSpecial;
	public Animator animatorTasto;

	public GameObject animazioneScenica;
	public string nomeAnimazioneScenica;

	public void playAnimation(){
		animatorTasto.Play ("AnimazioneTasto");
	}

	public Animator GetAnimatorSource(){
		return animatorTasto;
	}

	public AudioSource GetAudioSource(){
		return playerFigure;
	}

	public void risetFigureController(){
		animatorTasto = GetComponent<Animator> ();
		if(this.transform.parent.parent.name.Equals("PanelSpecialFigure"))
		    playerFigureSpecial = GetComponent<AudioSource> (); 
		else{
			playerFigure = GameObject.Find("AudioGlobale").GetComponent<AudioSource> ();
		}
	}

	public bool playerIsInPla(){
		if (playerFigure == null)
			return false;
		else
			return playerFigure.isPlaying;
	}

	public void ResetMyTouch(){
		my_touch = 0;
	}

	void Start () {
		ResetMyTouch ();
		risetFigureController ();
	}

	public void playSoloAudioEAnimation(){
			animatorTasto.Play ("AnimazioneTasto");
			if (animazioneScenica != null)
				playAnimationInScene ();
		Debug.Log(playerFigure.name);
		if (!mute)
		{
			if(playerFigure.enabled)
			    playerFigure.PlayOneShot(audioTest);
		}
	}

	public void playSoloAudioEAnimationBounce(){
		if(!playerIsInPla())
		{
		animatorTasto.Play ("buttonBounce");
		if (animazioneScenica != null)
			playAnimationInScene ();
		if(!mute)
			playerFigure.PlayOneShot (audioTest);
		}
	}


	public void playAudio(){
		string fatherName = transform.parent.name;
		Debug.Log("Playing tab: " + fatherName);
		if (fatherName.Equals ("QuestArea")) {
			if (!playerFigure.isPlaying) {
				animatorTasto.Play ("buttonBounce");
				playerFigure.PlayOneShot (audioTest);
			}

		} 
		else if (fatherName.Equals ("PanelSpecialFigure")) {
			if (!playerFigureSpecial.isPlaying) {
				animatorTasto.Play ("buttonBounce");
				playerFigureSpecial.PlayOneShot (audioTest);
			}

		}else {
			
		}
	}

	public void playAnimationInScene(){
		animazioneScenica.GetComponent<Animator>().Play (nomeAnimazioneScenica);
	}

	public void resetAnimationInScene(){
		if (animazioneScenica != null)
			animazioneScenica.GetComponent<Animator>().Rebind();
	}		
}
