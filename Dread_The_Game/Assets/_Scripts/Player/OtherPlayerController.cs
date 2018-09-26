using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayerController : MonoBehaviour
{

    //contains methods for handling values recieved from the server
    //position, rotation, isShooting ... etc
    private GameController gameController;
    private int id;
    private Vector3 position;
    private Vector3 rotation;
    private ModelHandler.characters character;
    private ModelHandler.weapons weapon;
    private bool isShooting; //we could find a different way to sync shooting, but i think this would work well for main weapons (if that doesn't include grenades etc(stuff that needs physics))

    private Vector3 lastpos = Vector3.zero;
    private Player player;
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("Global").GetComponent<GameController>();
        player = GetComponent<Player>();
        id = player.id;

    }

    void Update()
    {
        var pp = gameController.GetPlayerParams(id);
        if (pp == null) return;
        if (pp.position != lastpos)
        {
            transform.position = pp.position;
            transform.rotation = pp.rotation;
            lastpos = transform.position;
            //print(id + ": " + pp.position);
        }
    }
}
