using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

	protected Vector2 startingPoint; //punto di partenza
	protected Player player;

	//public bool isTrigger;
	
	void Start () {
		this.player = Camera.main.GetComponent<Player>();
	}
	
	
	void Update () {
		
	}

	public void SetStartingPoint(bool start, Vector2 point, float distance, int order){

		if(start){ //se è appena stato istanziato
			this.transform.position = point;
		}
		else{
			if (Vector2.Distance(this.transform.position, point) > distance){ //se la distanza tra la posizione attuale e il waypoint è > di quella richiesta
				this.transform.position = point;
			}
		}
		this.GetComponent<SpriteRenderer>().sortingOrder = order;
		startingPoint = point;
	}

	
	/******* TRIGGER  *******/

	



}
