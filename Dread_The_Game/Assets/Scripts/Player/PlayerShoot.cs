using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    private float rangeMultiplier;
    private WeaponHandler weaponHandler;
    // Use this for initialization
    void Start()
    {
        //if type==enemy
        //if type==player
        rangeMultiplier = GetComponent<CharacterAttributes>().rangeMultiplier;
        weaponHandler = GameObject.FindGameObjectWithTag("Global").GetComponent<WeaponHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            weaponHandler.ShootMain("");

        }
    }
}
