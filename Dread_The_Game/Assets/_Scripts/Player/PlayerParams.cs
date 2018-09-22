using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParams  {
	private int id;
	private string name;
    private Vector3 position;
    private Quaternion rotation;
    private ModelHandler.characters character;
    private ModelHandler.weapons weapon;
    private bool isShooting;

	public PlayerParams(int id, string name, Vector3 position, Quaternion rotation, ModelHandler.characters character, ModelHandler.weapons weapon){
		this.id = id;
		this.name = name;
		this.position = position;
		this.rotation = rotation;
		this.character = character;
		this.weapon = weapon;
		this.isShooting = false;
	}



	public int getId(){
		return this.id;
	}
	public void setId(int id){
		this.id = id;
	}


	public string getName(){
		return this.name;
	}
	public void setName(string name){
		this.name = name;
	}


	public Vector3 getPosition(){
		return this.position;
	}
	public void setPosition(Vector3 position){
		this.position = position;
	}
	

	public Quaternion getRotation(){
		return this.rotation;
	}
	public void setRotation(Quaternion rotation){
		this.rotation = rotation;
	}


	public ModelHandler.characters getCharacter(){
		return this.character;
	}
	public void setCharacter(ModelHandler.characters character){
		this.character = character;
	}

	
	public ModelHandler.weapons getWeapon(){
		return this.weapon;
	}
	public void setWeapon(ModelHandler.weapons weapon){
		this.weapon = weapon;
	}


	public bool getIsShooting(){
		return this.isShooting;
	}
	public void setIsShooting(bool isShooting){
		this.isShooting = isShooting;
	}



 

}
