using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayerParams {

    private int id;
    private Vector3 position;
    private Quaternion rotation;

	public OtherPlayerParams(int id, Vector3 position, Quaternion rotation){
		this.id = id;
		this.position = position;
		this.rotation = rotation;
	}

    
	public void setId(int id){
	this.id = id;
	}

	public int getId(){
		return this.id;
	}

	public void setPosition(Vector3 position){
		this.position = position;
	}

	public Vector3 getPosition(){
		return this.position;
	}

	public void setRotation(Quaternion rotation){
		this.rotation = rotation;
	}

	public Quaternion getRotation(){
		return this.rotation;
	}

	}

