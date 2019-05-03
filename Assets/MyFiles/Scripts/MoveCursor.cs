using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCursor : MonoBehaviour {

	StatsSetter stats;

	void Start(){
		stats = GameObject.Find("Controller").GetComponent<StatsSetter>();
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.W)){
			transform.position = new Vector3(transform.position.x, transform.position.y	+ 1, transform.position.z);
			stats.pos.y++;
			stats.Reset();
		}
		if(Input.GetKeyDown(KeyCode.S)){
			transform.position = new Vector3(transform.position.x, transform.position.y	- 1, transform.position.z);
			stats.pos.y--;
			stats.Reset();
		}
		if(Input.GetKeyDown(KeyCode.A)){
			transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
			stats.pos.x--;
			stats.Reset();
		}
		if(Input.GetKeyDown(KeyCode.D)){
			transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
			stats.pos.x++;
			stats.Reset();
		}		
	}
}
