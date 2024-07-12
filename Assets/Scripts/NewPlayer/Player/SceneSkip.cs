using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SceneSkip : MonoBehaviour {

	private Player player;
	private GameObject prompt;

	int scene;

	// Use this for initialization
	void Start () {
		this.player = Camera.main.transform.GetComponent<Player>();
		this.prompt = this.transform.parent.parent.parent.Find("AskSkip").gameObject;
		this.prompt.transform.Find("Si").GetComponent<Button>().onClick.RemoveListener(SkipScene);
		this.prompt.transform.Find("Si").GetComponent<Button>().onClick.AddListener(SkipScene);
	}

	public void AskSkip(){
		scene = int.Parse(EventSystem.current.currentSelectedGameObject.name.Replace("Scena", ""));
		this.prompt.SetActive(true);
	}

	public void SkipScene(){
		this.prompt.SetActive(false);
		Camera.main.clearFlags = CameraClearFlags.Nothing;
        Camera.main.cullingMask = 0;
		player.SetCurrentScene(scene);
		player.CleanScene(0);
		player.BuildScene();

	}

}
