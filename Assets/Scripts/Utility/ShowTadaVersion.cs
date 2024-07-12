using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTadaVersion : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.transform.GetComponent<Text>().text = SystemInfo.operatingSystem + "\nv. " + Application.version;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
