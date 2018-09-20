using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    private float rangeMultiplier;
    private WeaponHandler weaponHandler;

    private CharacterAttributes ca;
    // Use this for initialization
    void Start()
    {
        ca = GetComponent<CharacterAttributes>();
        weaponHandler = GameObject.FindGameObjectWithTag("Global").GetComponent<WeaponHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            weaponHandler.ShootMain(ca.weapon);

        }
    }
}
