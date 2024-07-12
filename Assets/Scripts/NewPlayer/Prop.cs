using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : Entity {

	public int idleState;
	private int maxStateNum;


	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeState(int stateNum){
		if(stateNum > 0)
			this.GetComponent<Animator>().Play("state" + stateNum);
		else
			this.GetComponent<Animator>().Play("Idle");

	}

	public void SetStartingState(int stateNum){
		if(this.GetComponent<Animator>()!=null){
			if(stateNum > 0)
				this.GetComponent<Animator>().Play("state" + stateNum + "Idle");
			else
				this.GetComponent<Animator>().Play("Idle");
		}
	}
}
