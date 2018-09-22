using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpers{
public Helpers(){

}

// This method converts JSON to Vector3 object
public Vector3 JsonToVector3(string target){
	Vector3 myVector3;
	string[] arrayOfData = Regex.Split(target, ",");
	myVector3 = new Vector3(float.Parse(arrayOfData[0]), float.Parse(arrayOfData[1]), float.Parse(arrayOfData[2]));
	return myVector3;
} 

// This method converts the player parameters to a data type of Dictionary for passing it to JSON 
public Dictionary<string,string> playerParamsToDict (int id, string name , string charachter ,string weapon, Vector3 position, int [] rotation, bool isShooting)
{
		Dictionary<string,string> data = new Dictionary<string,string>();
		data["id"] = id.ToString();
		data["name"] = name;
		data["charachter"] = charachter;
		data["weapon"] = weapon;
		data["position"] = position.x + " , " + position.y + " , " + position.z;
		data["rotation"] = rotation[0] + " , " + rotation[1] + " , " + rotation[2];
		data["isShooting"] = isShooting? "true":"false";
		return data;

} 



}
