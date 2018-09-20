using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Controller : MonoBehaviour {
 public SocketIOComponent socket;
	// Use this for initialization
	void Start () {
		
		StartCoroutine(ConnectToServer());

	}
	IEnumerator ConnectToServer(){
		yield return new WaitForSeconds(0.5f);
	 	socket.Emit("USER_CONNECT");
	}
	// Update is called once per frame
	void Update () {
		
	}
}
