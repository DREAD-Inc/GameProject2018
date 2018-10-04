using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaRifle : Weapon
{


    public PlasmaRoundController prc;
    public float globeSpeed = 5f;

    public float timeBetweenShots;
    private float shotCounter = 5;

    public Transform firePoint;
    private Transform ReptileTrigger;
    //private CharacterTrigger charachterTrigger;
    void Start()
    {
        firePoint = transform.Find("bullet_init");
        //prc = GetComponent<PlasmaRoundController>();
    }

    void Update()
    {
        if (isShooting) Shoot();
        else StopShooting();
    }

    protected override void Shoot()
    {
        shotCounter -= Time.deltaTime;

        if (shotCounter <= 0)
        {
            shotCounter = timeBetweenShots;
            Debug.Log(firePoint);
            PlasmaRoundController newGlobe = Instantiate(prc, firePoint.position, firePoint.rotation) as PlasmaRoundController;
            newGlobe.speed = globeSpeed;
        }
    }
    protected override void StopShooting() { shotCounter = 0; }

}
