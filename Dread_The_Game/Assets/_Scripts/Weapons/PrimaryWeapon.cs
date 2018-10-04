using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryWeapon : Weapon
{

    void Start()
    {
        //projectile = transform.Find("Controller");
    }

    void Update()
    {
        if (isShooting) Shoot();
        else projectile.gameObject.SetActive(false);
    }

    protected override void Shoot()
    {
        projectile.gameObject.SetActive(true);
    }
}
