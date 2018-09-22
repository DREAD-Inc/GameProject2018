using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParams  {
	private int id;
    private Vector3 position;
    private Quaternion rotation;
    private ModelHandler.characters character;
    private ModelHandler.weapons weapon;
    private bool isShooting;

	PlayerParams(int id, Vector3 position, Quaternion rotation,ModelHandler.characters character, ModelHandler.weapons weapon){
		this.id = id;
		this.position = position;
		this.rotation = rotation;
		this.character = character;
		this.weapon = weapon;
		this.isShooting = false;
	}
}
