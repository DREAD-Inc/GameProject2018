using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReptileWeapon : Weapon
{

    void Start()
    {
        reptileBall = transform.Find("ReptileBall");
    }

    void Update()
    {
        if (isShooting) Shoot();
        else reptileBall.gameObject.SetActive(false);
    }

    protected override void Shoot()
    {
        reptileBall.gameObject.SetActive(true);
    }
}
