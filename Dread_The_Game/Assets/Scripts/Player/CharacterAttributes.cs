using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttributes : MonoBehaviour
{

    public string playerName = "Player"; //+id
    public float health = 100f;
    public float rangeMultiplier = 10f;
    public string weapon = "Laser";
    //public WeaponHandler wh;

    // Use this for initialization
    void Start()
    {
        //wh = GetComponent<WeaponHandler>();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
