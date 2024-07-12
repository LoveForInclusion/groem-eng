using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityAction {


	public string name;
	public int type;
	public int action;
	public float speed;
	public float distance;
	public string startWP;
	public string[] targerWP;
	public string stateLayer;
	public bool flip;

}
