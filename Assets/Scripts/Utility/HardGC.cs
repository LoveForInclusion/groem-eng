using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardGC : MonoBehaviour {

	public Transform alert;
	bool called = false;
	private void Start()
	{
		Application.lowMemory += lowAlert;
	}

	// Update is called once per frame
	void Update () {
		if (Time.frameCount % 30 == 0)
        {
            System.GC.Collect();
        }
	}

	void lowAlert(){
		if (!called)
		{
			alert.gameObject.SetActive(true);
		}
	}

}
