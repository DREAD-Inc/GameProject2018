using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using SocketIO;

public class GameController : MonoBehaviour
{
    //private SocketIOComponent socket;
    private GameObject charPrefab;
    private GameObject otherCharPrefab;
    private GameObject repBullet;
    private GameObject mapPrefab;
    private List<PlayerParams> players;
    private Player clientPlayer;
    public List<GameObject> playerObjects;
    public List<BulletParams> bullets;
    //private Helpers helpers;

    public bool Dbug = true;
    public int PlayerNum = 0;

    void Start()
    {
        //socket = GetComponent<SocketIOComponent>();
        players = new List<PlayerParams>();
        playerObjects = new List<GameObject>();
        bullets = new List<BulletParams>();
        //helpers = new Helpers();

        //We need to wait for few ms before emiting
        StartCoroutine(ConnectToServer());

        // socket.On("PLAYER_ID", InstantiatePlayer); //Called when instantiating the client
        // socket.On("A_USER_INITIATED", AddNewPlayer); //Called when a new player joins after this client
        // socket.On("GET_EXISTING_PLAYER", AddExistingPlayer); //Spawns all players that were already in the game
        // socket.On("OTHER_PLAYER_MOVED", SetOtherPlayerMove);
        // socket.On("PLAYER_HEALTHCHANGE", SetPlayerHealthChange);
        // socket.On("OTHER_PLAYER_DEAD", DestroyDeadPlayer);
        // socket.On("USER_DISCONNECTED", DestroyDisconnectedPlayer);
        // socket.On("BULLET_INSTANTIATED", InstantiateOtherBullet);  
        // socket.On("BULLET_MOVE", SetOtherBulletMove);  

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
        //socket.Emit("USER_CONNECT");
    }

    private void InstantiatePlayer(/*SocketIOEvent evt */) //Instantiate this client
    {
        // SetPlayerNum(evt);
        // var newP = JsonUtility.FromJson<PlayerParams>(evt.data.ToString());
        // if (Dbug) Debug.Log("The Provided id is " + newP.id);
        // var character = Instantiate(charPrefab, new Vector3(0, 0f, 0f), Quaternion.Euler(0, -90, 0));
        // PlayerParams playerParams = new PlayerParams(newP.id, newP.name, newP.health, new Vector3(0, 0f, 0f), Quaternion.Euler(0, 0, 0), new ModelHandler.characters(), new ModelHandler.weapons());
        // var data = new JSONObject(JsonUtility.ToJson(playerParams));
        // socket.Emit("USER_INITIATED", data);
        // Debug.Log("USER_INITIATED emitted");
        // character.GetComponent<Rigidbody>().MovePosition(new Vector3(PlayerNum, 1f, PlayerNum));
        // character.GetComponent<Player>().SetFromPlayerParams(playerParams);
        // clientPlayer = character.GetComponent<Player>();

    }

    private void AddNewPlayer(/*SocketIOEvent evt */) //Add each player that joins after this client
    {
        // if (Dbug) print("Adding new player");
        // PlayerParams pp = PlayerParams.CreateFromJSON(evt.data.ToString());
        // var newCharacter = Instantiate(otherCharPrefab, pp.getPosition(), Quaternion.Euler(0, -90, 0));
        // newCharacter.GetComponent<Player>().SetFromPlayerParams(pp);
        // players.Add(pp);
        // playerObjects.Add(newCharacter);
    }


    private void AddExistingPlayer(/*SocketIOEvent evt */) //Add players that joined before this client
    {
        // PlayerParams pp = PlayerParams.CreateFromJSON(evt.data.ToString());
        // if (Dbug) print("Existing player: " + evt.data.ToString());
        // var newCharacter = Instantiate(otherCharPrefab, new Vector3(0, 0f, 0f), Quaternion.Euler(0, -90, 0));
        // Player player = newCharacter.GetComponent<Player>();
        // player.SetFromPlayerParams(pp);
        // players.Add(pp);
        // playerObjects.Add(newCharacter);

    }

    private void SetPlayerNum(/*SocketIOEvent evt */)
    {
        // PlayerNum = Int32.Parse(evt.data.GetField("num").ToString());
    } //Used for selecting a spawn posistion
    #endregion

    #region Movement / Actions 

    public void SendClientMovement(int id, Vector3 pos, Quaternion rot)
    {
        var obj = new MovementObjJSON(id, pos, rot);
        //socket.Emit("CLIENT_MOVE", JSONObject.Create(JsonUtility.ToJson(obj)));
    }

    public void SendPlayerHealth(int id, float health)
    {
        var obj = new HealthObjJSON(id, health);
        //socket.Emit("UPDATE_HEALTH", JSONObject.Create(JsonUtility.ToJson(obj)));

    }
    public void SendPlayerDead(int id)
    {
        //socket.Emit("PLAYER_DEAD", JSONObject.Create(id));

    }

    private void SetOtherPlayerMove(/*SocketIOEvent evt */)
    {
        // var move = JsonUtility.FromJson<MovementObjJSON>(evt.data.ToString());
        // //print("moving player: " + evt.data.ToString());
        // foreach (var p in players)
        //     if (p.id == move.id)
        //     {
        //         p.position = move.position;
        //         p.rotation = move.rotation;
        //     }
    }

    private void SetPlayerHealthChange(/*SocketIOEvent evt */)
    {
        // var healthChange = JsonUtility.FromJson<HealthObjJSON>(evt.data.ToString());
        // if (clientPlayer.id == healthChange.id)
        // {
        //     clientPlayer.health = healthChange.health;
        // }

        // foreach (var p in playerObjects)
        //     if (p.GetComponent<Player>().id == healthChange.id)
        //     {
        //         p.GetComponent<Player>().health = healthChange.health;
        //     }
    }

    private void DestroyDeadPlayer(/*SocketIOEvent evt */)
    {
        // var id = Int32.Parse(evt.data.GetField("id").ToString());
        // print("killing " + id);
        // foreach (var p in playerObjects)
        //     if (p.GetComponent<Player>().id == id)
        //     {
        //         print("other player dead: " + id);
        //         p.GetComponent<OtherPlayerController>().Die();
        //         return;
        //     }
    }
    private void DestroyDisconnectedPlayer(/*SocketIOEvent evt */)
    {
        // var id = Int32.Parse(evt.data.GetField("id").ToString());
        // print("Disconnecting " + id);
        // foreach (var p in playerObjects)
        //     if (p.GetComponent<Player>().id == id)
        //     {
        //         print("other player disconnected: " + id);
        //         p.GetComponent<OtherPlayerController>().Die();
        //         return;
        //     }
    }


    public PlayerParams GetPlayerParams(int id)
    {
        foreach (var p in players)
            if (p.id == id) return p;
        return null;
    }

    public BulletParams GetBulletParams(string id)
    {
        foreach (var b in bullets)
            if (b.id == id) return b;
        return null;
    }

    public void InstantiatePlayerBullet(string id, string bt, Vector3 pos, bool isExp)
    {

        var obj = new BulletParams(id, bt, pos, isExp);
        //socket.Emit("BULLET_INSTANTIATED", JSONObject.Create(JsonUtility.ToJson(obj)));
    }

    public void MoveBullet(string id, string bt, Vector3 pos, bool isExp)
    {
        var obj = new BulletParams(id, bt, pos, isExp);
        //socket.Emit("BULLET_MOVE", JSONObject.Create(JsonUtility.ToJson(obj)));
    }

    #endregion

    public void InstantiateOtherBullet(/*SocketIOEvent evt */)
    {
        // var bp = JsonUtility.FromJson<BulletParams>(evt.data.ToString());
        // bullets.Add(bp);
        // if (bp.bulletType == "rep")
        // {
        //     GameObject newBullet;
        //     newBullet = (GameObject)Resources.Load("Prefabs/Weapons/ReptileGunAssets/ReptileGlobe", typeof(GameObject));
        //     newBullet.GetComponent<GlobeProjectile>().id = bp.id;
        //     Instantiate(newBullet, bp.position, Quaternion.Euler(0, -90, 0));
        // }

    }

    public void SetOtherBulletMove(/*SocketIOEvent evt */)
    {
        // var bp = JsonUtility.FromJson<BulletParams>(evt.data.ToString());
        // foreach (var b in bullets)
        //     if (b.id == bp.id)
        //     {
        //         b.position = bp.position;
        //         b.isExploded = bp.isExploded;
        //     }

    }

}

