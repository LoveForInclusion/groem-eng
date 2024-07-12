using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scene0Controller : SceneController {

	public int allScenesSmp = 0;
	public int allScenesElb = 0;
	public float delay = 0;

	protected override void SetupController(){
		base.SetupController();
		base.player.SetAllScenes(this.player.opzioniAudio.option.tipoLettura == 0? allScenesSmp : allScenesElb);
		this.transform.Find("Button").GetComponent<Button>().onClick.AddListener(base.player.NextScene);
	}

}
