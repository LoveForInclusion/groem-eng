using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoad : MonoBehaviour {

	private static bool createdObj = false;

	void Awake()
	{
		if (!createdObj) {
			DontDestroyOnLoad (this.gameObject);
			createdObj = true;
		} else
			Destroy (this.gameObject);
	}

	public void destroyMe(){
		createdObj = false;
		Destroy (this.gameObject);
	}
		
}
