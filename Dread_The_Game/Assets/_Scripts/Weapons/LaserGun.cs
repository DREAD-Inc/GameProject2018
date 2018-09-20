using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : Weapon
{

    void Start()
    {
        projectile = transform.Find("Projectile");
    }

    void Update()
    {
        if (Input.GetButton("Fire1") || (Input.GetButton("VerticalA") || (Input.GetButton("HorizontalA")))) Shoot();
        else projectile.gameObject.SetActive(false);
    }

    public override void Shoot()
    {
        projectile.gameObject.SetActive(true);
    }
}
