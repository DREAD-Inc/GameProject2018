using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class GameController : MonoBehaviour
{
    private SocketIOComponent socket;
    private GameObject charPrefab;
    private GameObject otherCharPrefab;
    private GameObject mapPrefab;
    public List<PlayerParams> playerList;
    private Helpers helpers;

    void Start()
    {
        socket = GetComponent<SocketIOComponent>();
        playerList = new List<PlayerParams>();
        helpers = new Helpers();

        //We need to wait for few ms before emiting
        StartCoroutine(ConnectToServer());

        socket.On("PLAYER_ID", initiatePlayer);
        socket.On("A_USER_INITIATED", addNewPlayer2);
        socket.On("GET_EXISTING_PLAYER", addExistingPlayer2);

        charPrefab = (GameObject)Resources.Load("Prefabs/PlayerCharacters/Player", typeof(GameObject));
        otherCharPrefab = (GameObject)Resources.Load("Prefabs/PlayerCharacters/OtherPlayer", typeof(GameObject));
        mapPrefab = (GameObject)Resources.Load("Prefabs/Maps/TestMap", typeof(GameObject));

        Instantiate(mapPrefab);
        Instantiate(otherCharPrefab, new Vector3(3, 2f, 0f), Quaternion.Euler(0, -90, 0));

    }

    void Update() { }
    IEnumerator ConnectToServer()
    {
        yield return new WaitForSeconds(0.5f);
        socket.Emit("USER_CONNECT");
    }

    private void initiatePlayer(SocketIOEvent evt)
    {

        var id = Int32.Parse(evt.data.GetField("id").ToString());
        Debug.Log("The Provided id is " + id);
        var character = Instantiate(charPrefab, new Vector3(0, 2f, 0f), Quaternion.Euler(0, -90, 0));
        character.GetComponent<Player>().id = id;
        Quaternion rotation = new Quaternion();
        helpers.setQuaternion(ref rotation, 0, -90, 0);
        rotation = Quaternion.Euler(0, -90, 0); //alternatively using the Euler constructer https://docs.unity3d.com/ScriptReference/Quaternion.Euler.html
        PlayerParams playerParams = new PlayerParams(id, "UnNamed", new Vector3(0, 2f, 0f), rotation, new ModelHandler.characters(), new ModelHandler.weapons());

        //var data = helpers.playerParamsToJSON(playerParams);
        var data = new JSONObject(JsonUtility.ToJson(playerParams)); //Using https://docs.unity3d.com/ScriptReference/JsonUtility.html

        print("PlayerParams json" + data);
        socket.Emit("USER_INITIATED", data);

    }
    private void addNewPlayer(SocketIOEvent evt)
    {

        PlayerParams newPlayerParams = helpers.JSONToPlayerParams(evt.data);
        GameObject newCharPrefab = (GameObject)Resources.Load("Prefabs/PlayerCharacters/OtherPlayer", typeof(GameObject)); ;//we have loaded this already in otherCharPrefab
        Instantiate(newCharPrefab, newPlayerParams.getPosition(), Quaternion.Euler(newPlayerParams.getRotation().x, newPlayerParams.getRotation().y, newPlayerParams.getRotation().z));
        newCharPrefab.GetComponent<Player>().id = newPlayerParams.getId(); // we need to access the instatiated gameobject instead of the prefab/template var x = Instantiate(y); 
        Debug.Log(newPlayerParams.getPosition());
        playerList.Add(newPlayerParams);

    }
    private void addNewPlayer2(SocketIOEvent evt)
    {

        PlayerParams newPlayerParams = JsonUtility.FromJson<PlayerParams>(evt.data.ToString());//helpers.JSONToPlayerParams(evt.data);
        var newCharacter = Instantiate(otherCharPrefab, newPlayerParams.getPosition(), Quaternion.Euler(newPlayerParams.getRotation().x, newPlayerParams.getRotation().y, newPlayerParams.getRotation().z));
        newCharacter.GetComponent<Player>().id = newPlayerParams.id;
        Debug.Log(newPlayerParams.getPosition());
        playerList.Add(newPlayerParams);
    }

    private void addExistingPlayer(SocketIOEvent evt)
    {
        Debug.Log("existing player id is " + evt.data.GetField("id"));
        PlayerParams newPlayerParams = helpers.JSONToPlayerParams(evt.data);
        GameObject newCharPrefab = (GameObject)Resources.Load("Prefabs/PlayerCharacters/OtherPlayer", typeof(GameObject)); ;
        Instantiate(newCharPrefab, newPlayerParams.getPosition(), Quaternion.Euler(newPlayerParams.getRotation().x, newPlayerParams.getRotation().y, newPlayerParams.getRotation().z));
        newCharPrefab.GetComponent<Player>().id = newPlayerParams.getId();
        playerList.Add(newPlayerParams);
    }
    private void addExistingPlayer2(SocketIOEvent evt)
    {
        PlayerParams newPlayerParams = PlayerParams.CreateFromJSON(evt.data.ToString());
        print("Existing player: " + evt.data.ToString());
        var newCharacter = Instantiate(otherCharPrefab, newPlayerParams.getPosition(), Quaternion.Euler(newPlayerParams.getRotation().x, newPlayerParams.getRotation().y, newPlayerParams.getRotation().z));
        newCharacter.GetComponent<Player>().id = newPlayerParams.id;
        playerList.Add(newPlayerParams);
    }




}
