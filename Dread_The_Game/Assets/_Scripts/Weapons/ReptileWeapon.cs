using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReptileWeapon : Weapon
{


    public GlobeController globe;
    public float globeSpeed;

    public float timeBetweenShots;
    private float shotCounter;

    public Transform firePoint;
    private Transform ReptileTrigger;

    void Start()
    {
          //firePoint =  transform.Find("GameObject");
          ReptileTrigger = transform.Find("ReptileTrigger");
          //globe = GetComponent<GlobeController>();
    }

    void Update()
    {
        if (isShooting){

            Shoot();
            
            // shotCounter -= Time.deltaTime;

            // if(shotCounter <= 0){
            //     shotCounter = timeBetweenShots;
            //     Debug.Log(firePoint);
            //     GlobeController newGlobe = Instantiate(globe,firePoint.position,firePoint.rotation) as GlobeController;
            //     newGlobe.speed = globeSpeed;
            // }
        }else {
            //shotCounter = 0;
             ReptileTrigger.gameObject.SetActive(false);

        }
    }

    protected override void Shoot()
    {
        ReptileTrigger.gameObject.SetActive(true);
    }
}
