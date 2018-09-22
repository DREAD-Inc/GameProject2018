using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Helpers{
public Helpers(){

}

public string removeQuotationMark( string data){
	var tempArray = data.Split('"');
	return tempArray[1];
}

public void setQuaternion(ref Quaternion rotation, float x, float y, float z){
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

// This method converts the JSON object to PlayerParams object
public PlayerParams JSONToPlayerParams (JSONObject data){

   
		var id = Int32.Parse(removeQuotationMark(data.GetField("id").ToString())); 

		string name = removeQuotationMark(data.GetField("name").ToString());

	    //position
		var positionString = removeQuotationMark(data.GetField("position").ToString());
		var tempPositionAsArray = Regex.Split(positionString, ",");

		float[] positionAsArray = new float[3];
		for(int i = 0; i<3; i++){
			positionAsArray[i] = float.Parse(tempPositionAsArray[i]);
		}
		
		Vector3 position = new Vector3(positionAsArray[0],positionAsArray[1],positionAsArray[2]);
		//rotation
		var rotationString = removeQuotationMark(data.GetField("rotation").ToString());
		var tempRotationAsArray = Regex.Split(rotationString, ",");
		float[] rotationAsArray = new float[3];
		for(int i = 0; i<3; i++){
			rotationAsArray[i] = float.Parse(tempRotationAsArray[i]);
		}

		Quaternion rotation = new Quaternion();
		setQuaternion(ref rotation,rotationAsArray[0],rotationAsArray[1],rotationAsArray[2]);

		//-------
		// Yet have nothing to do with characters and weapons
		//-------
        var isShootingString = removeQuotationMark(data.GetField("isShooting").ToString());
		bool isShooting = isShootingString == "true"? true:false;

		PlayerParams playerParams = new PlayerParams(id,name,position, rotation , new ModelHandler.characters(), new ModelHandler.weapons());
		playerParams.setIsShooting(isShooting);
		return playerParams; 
}
}