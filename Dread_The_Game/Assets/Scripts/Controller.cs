using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Controller : MonoBehaviour {
 public SocketIOComponent socket;
	// Use this for initialization
	void Start () {
		
		StartCoroutine(ConnectToServer());
		socket.On("USER_CONNECTED", OnUserConnected );
		socket.On("PLAY", OnUserPlay );


	}
	IEnumerator ConnectToServer(){
		yield return new WaitForSeconds(0.5f);

	 	socket.Emit("USER_CONNECT");

		yield return new WaitForSeconds(1f);

		//Sample data for testing
		Dictionary<string,string> data = new Dictionary<string,string>();
		data["name"] = "Fuckerman";
		Vector3 position = new Vector3(0,0,0);
		data["position"] = position.x + " , " + position.y + " , " + position.z;

		socket.Emit("PLAY", new JSONObject(data));
	}

	private void OnUserConnected( SocketIOEvent evt){
		Debug.Log("The message from server is "+ evt.data );
	}
	
	private void OnUserPlay( SocketIOEvent evt){
		Debug.Log("The message from server is "+ evt.data );
	}
}
