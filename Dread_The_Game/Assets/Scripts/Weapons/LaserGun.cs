using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : Weapon
{

    void Start()
    {
        //projectile = (GameObject)Resources.Load("Prefabs/Projectiles/LaserGun", typeof(GameObject));
        projectile = transform.Find("Projectile");
        //projectile = transform.GetChild(2);
        //print();
    }

    void Update()
    {
        if (Input.GetButton("Fire1") || (Input.GetButton("VerticalA") || (Input.GetButton("HorizontalA")))) Shoot();
        else projectile.gameObject.SetActive(false);
    }

    public override void Shoot()
    {
        //print("Shooting Laser");
        projectile.gameObject.SetActive(true);
    }
}
