using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class GameController : MonoBehaviour
{
    public SocketIOComponent socket;
    private GameObject charPrefab;
    private GameObject otherCharPrefab;
    private GameObject mapPrefab;
    public List<Object> PlayerList;
    void Start()
    {

        StartCoroutine(ConnectToServer());
		//socket.Emit("USER_CONNECT");
       //socket.On("PLAYER_ID", OnIdProvided);
        socket.On("PLAYER_ID", OnIdProvided);

        charPrefab = (GameObject)Resources.Load("Prefabs/PlayerCharacters/Player", typeof(GameObject));
        otherCharPrefab = (GameObject)Resources.Load("Prefabs/PlayerCharacters/OtherPlayer", typeof(GameObject));
        mapPrefab = (GameObject)Resources.Load("Prefabs/Maps/TestMap", typeof(GameObject));

        Instantiate(mapPrefab);
        Instantiate(charPrefab, new Vector3(0, 2f, 0f), Quaternion.Euler(0, -90, 0));
        //for(connected c) spawn at c.position with c.rotation 
        Instantiate(otherCharPrefab, new Vector3(3, 2f, 0f), Quaternion.Euler(0, -90, 0));

    }

    void Update()
    {

    }
	IEnumerator ConnectToServer(){
		yield return new WaitForSeconds(0.5f);
	 	socket.Emit("USER_CONNECT");


		//Sample data for testing
		Dictionary<string,string> data = new Dictionary<string,string>();
		data["name"] = "Fuckerman";
		Vector3 position = new Vector3(0,0,0);
		data["position"] = position.x + " , " + position.y + " , " + position.z;

		socket.Emit("PLAY", new JSONObject(data));
	}


	// private void OnUserConnected( SocketIOEvent evt){
	// 	Debug.Log("The message from server is "+ evt.data );
	// }
     private void OnIdProvided( SocketIOEvent evt){
		Debug.Log("The Provided id is " + evt.data );
	}
    
}
