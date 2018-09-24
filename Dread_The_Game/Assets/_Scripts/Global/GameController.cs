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

    public bool Dbug = true;
    public int PlayerNum = 0;

    void Start()
    {
        socket = GetComponent<SocketIOComponent>();
        playerList = new List<PlayerParams>();
        helpers = new Helpers();

        //We need to wait for few ms before emiting
        StartCoroutine(ConnectToServer());

        socket.On("PLAYER_ID", InstantiatePlayer); //Called when initiating the client
        socket.On("A_USER_INITIATED", AddNewPlayer); //Called when a new player joins after this client
        socket.On("GET_EXISTING_PLAYER", AddExistingPlayer); //Spawns all players that were already in the game
        //socket.On("ONLINE_PLAYER_NUM", SetPlayerNum); 

        charPrefab = (GameObject)Resources.Load("Prefabs/PlayerCharacters/Player", typeof(GameObject));
        otherCharPrefab = (GameObject)Resources.Load("Prefabs/PlayerCharacters/OtherPlayer", typeof(GameObject));
        mapPrefab = (GameObject)Resources.Load("Prefabs/Maps/TestMap", typeof(GameObject));

        Instantiate(mapPrefab);
    }

    void Update() { }
    IEnumerator ConnectToServer()
    {
        yield return new WaitForSeconds(0.5f);
        socket.Emit("USER_CONNECT");
    }
    /* --------------- Connection --------------- */
    private void InstantiatePlayer(SocketIOEvent evt)
    {
        SetPlayerNum(evt);
        var newP = JsonUtility.FromJson<PlayerParams>(evt.data.ToString());
        if (Dbug) Debug.Log("The Provided id is " + newP.id + " numPlayers: " + PlayerNum);
        var character = Instantiate(charPrefab, new Vector3(0, 2f, 0f), Quaternion.Euler(0, -90, 0));
        PlayerParams playerParams = new PlayerParams(newP.id, newP.name, new Vector3(0, 2f, 0f), Quaternion.Euler(0, -90, 0), new ModelHandler.characters(), new ModelHandler.weapons());
        character.GetComponent<Player>().SetFromPlayerParams(playerParams);
        character.GetComponent<Rigidbody>().MovePosition(new Vector3(PlayerNum, 2f, PlayerNum));
        var data = new JSONObject(JsonUtility.ToJson(playerParams));
        socket.Emit("USER_INITIATED", data);
    }

    private void AddNewPlayer(SocketIOEvent evt)
    {
        if (Dbug) print("Adding new player");
        PlayerParams pp = PlayerParams.CreateFromJSON(evt.data.ToString());
        var newCharacter = Instantiate(otherCharPrefab, pp.getPosition(), Quaternion.Euler(pp.getRotation().x, pp.getRotation().y, pp.getRotation().z));
        newCharacter.GetComponent<Player>().id = pp.id;

        playerList.Add(pp);
    }


    private void AddExistingPlayer(SocketIOEvent evt)
    {
        PlayerParams pp = PlayerParams.CreateFromJSON(evt.data.ToString());
        if (Dbug) print("Existing player: " + evt.data.ToString());
        var newCharacter = Instantiate(otherCharPrefab);
        Player player = newCharacter.GetComponent<Player>();
        player.SetFromPlayerParams(pp);
        player.transform.position = pp.position;
        playerList.Add(pp);
    }

    private void SetPlayerNum(SocketIOEvent evt)
    {
        PlayerNum = Int32.Parse(evt.data.GetField("num").ToString());
        if (Dbug) print("Online players: " + PlayerNum);
    }

    /* --------------- Movement / Actions --------------- */

    public void SendClientMovement(int id, Vector3 pos, Quaternion rot)
    {
        var obj = new MovementObj(id, pos, rot);
        //print(JsonUtility.ToJson(obj));
        socket.Emit("CLIENT_MOVE", JSONObject.Create(JsonUtility.ToJson(obj)));
    }





    private void initiatePlayer(SocketIOEvent evt)
    {

        var id = Int32.Parse(evt.data.GetField("id").ToString());
        if (Dbug) Debug.Log("The Provided id is " + id);
        var character = Instantiate(charPrefab, new Vector3(0, 2f, 0f), Quaternion.Euler(0, -90, 0));
        character.GetComponent<Player>().id = id;
        Quaternion rotation = new Quaternion();
        helpers.setQuaternion(ref rotation, 0, -90, 0);
        PlayerParams playerParams = new PlayerParams(id, "UnNamed", new Vector3(0, 2f, 0f), rotation, new ModelHandler.characters(), new ModelHandler.weapons());
        var data = helpers.playerParamsToJSON(playerParams);
        socket.Emit("USER_INITIATED", data);
    }
    private void addNewPlayer(SocketIOEvent evt)
    {

        PlayerParams newPlayerParams = helpers.JSONToPlayerParams(evt.data);
        GameObject newCharPrefab = (GameObject)Resources.Load("Prefabs/PlayerCharacters/OtherPlayer", typeof(GameObject)); ;//we have loaded this already in otherCharPrefab
        Instantiate(newCharPrefab, newPlayerParams.getPosition(), Quaternion.Euler(newPlayerParams.getRotation().x, newPlayerParams.getRotation().y, newPlayerParams.getRotation().z));
        newCharPrefab.GetComponent<Player>().id = newPlayerParams.getId(); // we need to access the instatiated gameobject instead of the prefab/template var x = Instantiate(y); 
        if (Dbug) Debug.Log(newPlayerParams.getPosition());
        playerList.Add(newPlayerParams);

    }
    private void addExistingPlayer(SocketIOEvent evt)
    {
        if (Dbug) Debug.Log("existing player id is " + evt.data.GetField("id"));
        PlayerParams newPlayerParams = helpers.JSONToPlayerParams(evt.data);
        GameObject newCharPrefab = (GameObject)Resources.Load("Prefabs/PlayerCharacters/OtherPlayer", typeof(GameObject)); ;
        Instantiate(newCharPrefab, newPlayerParams.getPosition(), Quaternion.Euler(newPlayerParams.getRotation().x, newPlayerParams.getRotation().y, newPlayerParams.getRotation().z));
        newCharPrefab.GetComponent<Player>().id = newPlayerParams.getId();
        playerList.Add(newPlayerParams);
    }
}

[System.Serializable]
public class MovementObj
{
    public int id;
    public Vector3 position;
    public Quaternion rotation;

    public MovementObj(int id, Vector3 pos, Quaternion rot)
    {
        this.id = id;
        this.position = pos;
        this.rotation = rot;
    }
}
