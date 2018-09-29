using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobeController : MonoBehaviour {

	public float speed;
	public GameObject targetCharachter;
	Vector3 newVector;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		newVector = stupidBestFirstSearch(targetCharachter);
		transform.Translate(newVector * speed * Time.deltaTime);
	}


	private Vector3 stupidBestFirstSearch( GameObject targetCharachter)
	{
			Vector3 newVector = new Vector3(0,0,0);
	  		if(targetCharachter.transform.position.x - transform.position.x > 0) 
			  newVector.x = 1; 
			else if(targetCharachter.transform.position.x - transform.position.x < 0 ) newVector.x = -1;
			else newVector.x = 0;

			if(targetCharachter.transform.position.y - transform.position.y > 0) 
			  newVector.y = 1;
			else if(targetCharachter.transform.position.y - transform.position.y < 0 ) newVector.y = -1;
			else newVector.y = 0;
			
			if(targetCharachter.transform.position.z - transform.position.z > 0) 
			  newVector.z = 1;
			else if(targetCharachter.transform.position.z - transform.position.z < 0 ) newVector.z = -1;
			else newVector.z = 0;

			return newVector;
	}
}
