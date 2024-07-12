using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : Entity{


	private GameObject[] target;
	public float speed;
	public float stoppingDistance;
	public int maxActionNum;
	private Vector2 moveDirection = Vector2.zero;
	private Vector2 targetScale;
	private Vector2 lastScale;
	public string lastState = "";
	private int targetIndex = 0;
	int state;

	

	public void SetTarget(GameObject[] target, float distance, float speed){
		this.target = target;
		if(distance>0)
		this.stoppingDistance = distance;
		if(speed>0)
			this.speed = speed;

		//Debug.Log("Speed: " + speed + " - Stopping in " + distance);
		//Debug.Log(this.transform.name + ": - Setting Target Index: " + targetIndex);
		targetScale = target[targetIndex].transform.localScale;
		moveDirection = this.transform.InverseTransformPoint(target[targetIndex].transform.position);
	}

	public void NextTarget(){
		//Debug.Log("Speed: " + speed + " - Stopping in " + distance);
		Debug.Log("Next Target Index: " + targetIndex);
		this.SetStartingPoint(false, this.transform.position, stoppingDistance, this.GetComponent<SpriteRenderer>().sortingOrder);
		lastScale = this.transform.localScale;
		targetScale = target[targetIndex].transform.localScale;
		moveDirection = this.transform.InverseTransformPoint(target[targetIndex].transform.position);
	}

	public void Action(int actionNum, GameObject startPoint, string layerState = "Base Layer"){
		//Debug.Log(this.name + " - Eseguo l'azione:" + layerState + ".action" + actionNum);
		if(layerState.Equals(""))
			layerState = "Base Layer";
		this.ChangeLayer(layerState, true);
		moveDirection = Vector2.zero;
		target = null;
		this.SetStartingPoint(false, startPoint.transform.position, this.stoppingDistance, this.GetComponent<SpriteRenderer>().sortingOrder);
		lastScale = startPoint.transform.localScale;
		this.transform.localScale = lastScale;
		this.GetComponent<Animator>().SetBool("action", true);
		this.GetComponent<Animator>().Play(layerState+".action" + actionNum);
		this.StartCoroutine(waitAnimation());
	}

	public void Action(int actionNum, float speed, float stoppingDistance, GameObject startPoint, GameObject[] target, string layerState = "Base Layer"){
		if(layerState.Equals(""))
			layerState = "Base Layer";
		this.ChangeLayer(layerState, true);
		this.SetStartingPoint(false, startPoint.transform.position, this.stoppingDistance, this.GetComponent<SpriteRenderer>().sortingOrder);
		lastScale = startPoint.transform.localScale;
		this.GetComponent<Animator>().SetBool("action", true);
		this.SetTarget(target, stoppingDistance, speed);
		this.GetComponent<Animator>().Play(layerState + ".action" + actionNum);
	}

	public void Move(float speed, float stoppingDistance, GameObject start, GameObject[] target){
		//Debug.Log(this.name + " - Mi muovo da:" + start.name + " verso " + target.name + "?");
		this.GetComponent<Animator>().SetBool("action", false);
		this.SetStartingPoint(false, start.transform.position, stoppingDistance, this.GetComponent<SpriteRenderer>().sortingOrder);
		lastScale = start.transform.localScale;
		this.SetTarget(target, stoppingDistance, speed);
	}

	public void ChangeLayer(string layer, bool action){
		if(layer.Equals(""))
			layer = "Base Layer";
		
		if(lastState!=null && !lastState.Equals("Base Layer") && !lastState.Equals(""))
			this.GetComponent<Animator>().SetLayerWeight(this.GetComponent<Animator>().GetLayerIndex(lastState), 0);

		if(this.GetComponent<Animator>().GetLayerIndex(layer) != -1)
			this.GetComponent<Animator>().SetLayerWeight(this.GetComponent<Animator>().GetLayerIndex(layer), 1);
		lastState = layer;
		if(!action)
			this.GetComponent<Animator>().Play(layer+".Idle");
	}

	public float GetStoppingDistance(){
		return stoppingDistance;
	}


	void Update(){
		
		if(moveDirection.x>0.1){
			this.transform.GetComponent<SpriteRenderer>().flipX = false;
		} else if(moveDirection.x < -0.1){
			this.transform.GetComponent<SpriteRenderer>().flipX = true;
		}
	}
	void FixedUpdate () {
		// if(this.transform.GetComponent<Animator>().GetCurrentAnimatorStateInfo(this.transform.GetComponent<Animator>().GetLayerIndex("MangiaNonna")).fullPathHash!=state){
		// 	state = this.transform.GetComponent<Animator>().GetCurrentAnimatorStateInfo(this.transform.GetComponent<Animator>().GetLayerIndex("MangiaNonna")).fullPathHash;
		// 	Debug.Log("CRISTO!");
		// }

		

		if(moveDirection.magnitude < 0.1)
			this.transform.GetComponent<Rigidbody2D>().drag = 10;
		else {
			this.transform.GetComponent<Rigidbody2D>().drag = 0;
		}

		moveDirection.Normalize();

		this.transform.GetComponent<Rigidbody2D>().velocity = moveDirection * speed;
		if(moveDirection.magnitude > 0.1f){

			float dist = Mathf.Abs(Vector2.Distance(this.startingPoint, this.transform.position));
			float full = Mathf.Abs(Vector2.Distance(this.startingPoint, this.target[targetIndex].transform.position));

			this.transform.localScale = Vector2.Lerp(this.lastScale, targetScale, dist/full);
		}

		if(target!=null){
			if (Vector2.Distance(this.transform.position, target[targetIndex].transform.position) < stoppingDistance){
				targetIndex++;
				this.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
				if(targetIndex >= target.Length){
					moveDirection = Vector3.zero;
					this.transform.position = target[targetIndex-1].transform.position;
					target = null;
					Debug.Log(this.transform.name + ": - Stopping at target index: " + targetIndex);
					this.player.ActionReturn();
					moveDirection = Vector2.zero;
					this.transform.GetComponent<Animator>().SetFloat("moveVector", 0);
					targetIndex = 0;
					if(this.GetComponent<Animator>().GetBool("action"))
						this.GetComponent<Animator>().SetBool("action", false);
				} else {
					NextTarget();
				}
			}
		}

		this.transform.GetComponent<Animator>().SetFloat("moveVector", moveDirection.magnitude);
	}

	public void RemoveTarget(){
		this.player.ActionReturn();
		moveDirection = Vector2.zero;
		target = null;
	}

	public void setFlip(bool flip){
		this.transform.GetComponent<SpriteRenderer>().flipX = flip;
	}

	IEnumerator waitAnimation(){

		//Debug.Log(this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

		yield return new WaitForSeconds(this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
		
		if(this.GetComponent<Animator>().GetBool("action"))
			this.GetComponent<Animator>().SetBool("action", false);
		
		this.player.ActionReturn();

	}

}
