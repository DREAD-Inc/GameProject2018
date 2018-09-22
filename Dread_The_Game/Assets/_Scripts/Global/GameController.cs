using System;
using System.Text;
using System.Text.RegularExpressions;
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
        socket.On("A_USER_INITIATED", addNewPlayer);



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

       // Debug.Log(evt.data);
        var id = Int32.Parse(evt.data.GetField("id").ToString()); 
		Debug.Log( evt.data.GetField("id").ToString());
        // We should make it possible to initiate with the id
        Instantiate(charPrefab, new Vector3(0, 2f, 0f), Quaternion.Euler(0, -90, 0));

        Quaternion rotation = new Quaternion();
        helpers.setQuaternion(ref rotation, 0 , -90 , 0);
	    PlayerParams playerParams = new PlayerParams(id, "UnNamed", new Vector3(0, 2f, 0f), rotation, new ModelHandler.characters(), new ModelHandler.weapons());
        var data = helpers.playerParamsToJSON(playerParams);

        socket.Emit("USER_INITIATED", data);

	}
    private void addNewPlayer(SocketIOEvent evt){
                // Debug.Log(evt.data);

       PlayerParams newPlayerParams = helpers.JSONToPlayerParams(evt.data);
       
        Debug.Log(newPlayerParams.getPosition());



    
    }


    
    
}
