using System;
using System.Text;
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
    public List<PlayerParams> playerList;
    private Helpers helpers = new Helpers();
    void Start()
    {
        
        socket = GetComponent<SocketIOComponent>();

        //We need to wait for few ms before emiting
        StartCoroutine(ConnectToServer());
        socket.On("PLAYER_ID", initiatePlayer);

        charPrefab = (GameObject)Resources.Load("Prefabs/PlayerCharacters/Player", typeof(GameObject));
        otherCharPrefab = (GameObject)Resources.Load("Prefabs/PlayerCharacters/OtherPlayer", typeof(GameObject));
        mapPrefab = (GameObject)Resources.Load("Prefabs/Maps/TestMap", typeof(GameObject));

        Instantiate(mapPrefab);
        //for(connected c) spawn at c.position with c.rotation 
        Instantiate(otherCharPrefab, new Vector3(3, 2f, 0f), Quaternion.Euler(0, -90, 0));

    }

    void Update()
    {

    }
	IEnumerator ConnectToServer(){
		yield return new WaitForSeconds(0.5f);
	 	socket.Emit("USER_CONNECT");
		//socket.Emit("PLAY", new JSONObject(data));
	}



     private void initiatePlayer( SocketIOEvent evt){

        var id = Int32.Parse(evt.data.GetField("id").ToString()); 
		Debug.Log("The Provided id is " + id);
        // We should make it possible to initiate with the id
        Instantiate(charPrefab, new Vector3(0, 2f, 0f), Quaternion.Euler(0, -90, 0));

	   //Sample data for testing
	
        var data = helpers.playerParamsToDict(id, "UnNamed", "Default", "Default",new Vector3(0, 2f, 0f), new int[] {0, -90, 0}, false);

        socket.Emit("USER_INITIATED", new JSONObject(data));

	}


    
    
}
