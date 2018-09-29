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
    private List<PlayerParams> players;
    private Player clientPlayer;
    public List<GameObject> playerObjects;

    private Helpers helpers;

    public bool Dbug = true;
    public int PlayerNum = 0;

    void Start()
    {
        socket = GetComponent<SocketIOComponent>();
        players = new List<PlayerParams>();
        playerObjects = new List<GameObject>();
        helpers = new Helpers();

        //We need to wait for few ms before emiting
        StartCoroutine(ConnectToServer());

        socket.On("PLAYER_ID", InstantiatePlayer); //Called when instantiating the client
        socket.On("A_USER_INITIATED", AddNewPlayer); //Called when a new player joins after this client
        socket.On("GET_EXISTING_PLAYER", AddExistingPlayer); //Spawns all players that were already in the game
        socket.On("OTHER_PLAYER_MOVED", SetOtherPlayerMove);
        socket.On("PLAYER_HEALTHCHANGE", SetPlayerHealthChange);
        socket.On("OTHER_PLAYER_DEAD", DestroyDeadPlayer);
        socket.On("USER_DISCONNECTED", DestroyDisconnectedPlayer);

        charPrefab = (GameObject)Resources.Load("Prefabs/PlayerCharacters/Player", typeof(GameObject));
        otherCharPrefab = (GameObject)Resources.Load("Prefabs/PlayerCharacters/OtherPlayer", typeof(GameObject));
        mapPrefab = (GameObject)Resources.Load("Prefabs/Maps/TestMap", typeof(GameObject));

        Instantiate(mapPrefab);
    }

    void Update() { }

    #region Connection 
    IEnumerator ConnectToServer()
    {
        yield return new WaitForSeconds(0.5f);
        socket.Emit("USER_CONNECT");
    }

    private void InstantiatePlayer(SocketIOEvent evt) //Instantiate this client
    {
        SetPlayerNum(evt);
        var newP = JsonUtility.FromJson<PlayerParams>(evt.data.ToString());
        if (Dbug) Debug.Log("The Provided id is " + newP.id);
        var character = Instantiate(charPrefab, new Vector3(0, 0f, 0f), Quaternion.Euler(0, -90, 0));
        PlayerParams playerParams = new PlayerParams(newP.id, newP.name, newP.health, new Vector3(0, 0f, 0f), Quaternion.Euler(0, 0, 0), new ModelHandler.characters(), new ModelHandler.weapons());
        character.GetComponent<Player>().SetFromPlayerParams(playerParams);
        character.GetComponent<Rigidbody>().MovePosition(new Vector3(PlayerNum, 1f, PlayerNum));
        clientPlayer = character.GetComponent<Player>();
        var data = new JSONObject(JsonUtility.ToJson(playerParams));
        socket.Emit("USER_INITIATED", data);
    }

    private void AddNewPlayer(SocketIOEvent evt) //Add each player that joins after this client
    {
        if (Dbug) print("Adding new player");
        PlayerParams pp = PlayerParams.CreateFromJSON(evt.data.ToString());
        var newCharacter = Instantiate(otherCharPrefab, pp.getPosition(), Quaternion.Euler(0, -90, 0));
        newCharacter.GetComponent<Player>().SetFromPlayerParams(pp);
        players.Add(pp);
        playerObjects.Add(newCharacter);
    }


    private void AddExistingPlayer(SocketIOEvent evt) //Add players that joined before this client
    {
        PlayerParams pp = PlayerParams.CreateFromJSON(evt.data.ToString());
        if (Dbug) print("Existing player: " + evt.data.ToString());
        var newCharacter = Instantiate(otherCharPrefab, new Vector3(0, 0f, 0f), Quaternion.Euler(0, -90, 0));
        Player player = newCharacter.GetComponent<Player>();
        player.SetFromPlayerParams(pp);
        players.Add(pp);
        playerObjects.Add(newCharacter);

    }

    private void SetPlayerNum(SocketIOEvent evt) { PlayerNum = Int32.Parse(evt.data.GetField("num").ToString()); } //Used for selecting a spawn posistion
    #endregion

    #region Movement / Actions 

    public void SendClientMovement(int id, Vector3 pos, Quaternion rot)
    {
        var obj = new MovementObjJSON(id, pos, rot);
        socket.Emit("CLIENT_MOVE", JSONObject.Create(JsonUtility.ToJson(obj)));
    }

    public void SendPlayerHealth(int id, float health)
    {
        var obj = new HealthObjJSON(id, health);
        socket.Emit("UPDATE_HEALTH", JSONObject.Create(JsonUtility.ToJson(obj)));

    }
    public void SendPlayerDead(int id)
    {
        socket.Emit("PLAYER_DEAD", JSONObject.Create(id));

    }

    private void SetOtherPlayerMove(SocketIOEvent evt)
    {
        var move = JsonUtility.FromJson<MovementObjJSON>(evt.data.ToString());
        //print("moving player: " + evt.data.ToString());
        foreach (var p in players)
            if (p.id == move.id)
            {
                p.position = move.position;
                p.rotation = move.rotation;
            }
    }

    private void SetPlayerHealthChange(SocketIOEvent evt)
    {
        var healthChange = JsonUtility.FromJson<HealthObjJSON>(evt.data.ToString());
        if (clientPlayer.id == healthChange.id)
        {
            clientPlayer.health = healthChange.health;
        }

        foreach (var p in playerObjects)
            if (p.GetComponent<Player>().id == healthChange.id)
            {
                p.GetComponent<Player>().health = healthChange.health;
            }
    }

    private void DestroyDeadPlayer(SocketIOEvent evt)
    {
        var id = Int32.Parse(evt.data.GetField("id").ToString());
        print("killing " + id);
        foreach (var p in playerObjects)
            if (p.GetComponent<Player>().id == id)
            {
                print("other player dead: " + id);
                p.GetComponent<OtherPlayerController>().Die();
                return;
            }
    }
     private void DestroyDisconnectedPlayer(SocketIOEvent evt)
    {
        var id = Int32.Parse(evt.data.GetField("id").ToString());
        print("Disconnecting " + id);
        foreach (var p in playerObjects)
            if (p.GetComponent<Player>().id == id)
            {
                print("other player disconnected: " + id);
                p.GetComponent<OtherPlayerController>().Die();
                return;
            }
    }
    

    public PlayerParams GetPlayerParams(int id)
    {
        foreach (var p in players)
            if (p.id == id) return p;
        return null;
    }
    #endregion




    private void initiatePlayer(SocketIOEvent evt)
    {

        var id = Int32.Parse(evt.data.GetField("id").ToString());
        if (Dbug) Debug.Log("The Provided id is " + id);
        var character = Instantiate(charPrefab, new Vector3(0, 2f, 0f), Quaternion.Euler(0, -90, 0));
        character.GetComponent<Player>().id = id;
        Quaternion rotation = new Quaternion();
        helpers.setQuaternion(ref rotation, 0, -90, 0);
        PlayerParams playerParams = new PlayerParams(id, "UnNamed", 100f, new Vector3(0, 2f, 0f), rotation, new ModelHandler.characters(), new ModelHandler.weapons());
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
        //players.Add(newPlayerParams);

    }
    private void addExistingPlayer(SocketIOEvent evt)
    {
        if (Dbug) Debug.Log("existing player id is " + evt.data.GetField("id"));
        PlayerParams newPlayerParams = helpers.JSONToPlayerParams(evt.data);
        GameObject newCharPrefab = (GameObject)Resources.Load("Prefabs/PlayerCharacters/OtherPlayer", typeof(GameObject)); ;
        Instantiate(newCharPrefab, newPlayerParams.getPosition(), Quaternion.Euler(newPlayerParams.getRotation().x, newPlayerParams.getRotation().y, newPlayerParams.getRotation().z));
        newCharPrefab.GetComponent<Player>().id = newPlayerParams.getId();
        //players.Add(newPlayerParams);
    }
}

