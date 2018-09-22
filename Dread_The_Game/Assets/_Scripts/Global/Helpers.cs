using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpers{
public Helpers(){

}

public void setQuaternion(Quaternion rotation, int x, int y, int z){
	rotation.x = x;
	rotation.y = y;
	rotation.z = z;
}

// This method converts JSON to Vector3 object
public Vector3 jsonToVector3(string target){
	Vector3 myVector3;
	string[] arrayOfData = Regex.Split(target, ",");
	myVector3 = new Vector3(float.Parse(arrayOfData[0]), float.Parse(arrayOfData[1]), float.Parse(arrayOfData[2]));
	return myVector3;
} 

// This method converts the PlayerParams object to JSON 
public JSONObject playerParamsToJSON (PlayerParams playerParams)
{
		Dictionary<string,string> data = new Dictionary<string,string>();
		data["id"] = playerParams.getId().ToString();
		data["name"] = playerParams.getName();
		data["position"] = playerParams.getPosition().x + " , " + playerParams.getPosition().y + " , " + playerParams.getPosition().z;
		data["rotation"] = playerParams.getRotation().x + " , " + playerParams.getRotation().y + " , " + playerParams.getRotation().z;
		data["charachter"] = "default";
		data["weapon"] = "default";
		data["isShooting"] = playerParams.getIsShooting()? "true":"false";
		return new JSONObject(data);
} 
public PlayerParams JSONToPlayerParams (JSONObject data){

return null; 
}
}