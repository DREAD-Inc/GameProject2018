using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

	private float range;
	public Transform projectile;
	// Use this for initialization
	void Start () {
		//if type==enemy
		//if type==player
		range = GetComponent<CharacterAttributes>().shootRange;
		//projectile = GetComponent<CharacterAttributes>().weapon.projectile;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
