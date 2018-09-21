using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayerController : MonoBehaviour
{

    //contains methods for handling values recieved from the server
    //position, rotation, isShooting ... etc
    private GameObject global;
    private int id;
    private Vector3 position;
    private Quaternion rotation;
    private ModelHandler.characters character;
    private ModelHandler.weapons weapon;
    private bool isShooting; //we could find a different way to sync shooting, but i think this would work well for main weapons (if that doesn't include grenades etc(stuff that needs physics))


    void Start()
    {
        global = GameObject.FindGameObjectWithTag("Global");
    }

    void Update()
    {

        //this.pos equals socket.pos
    }
}
