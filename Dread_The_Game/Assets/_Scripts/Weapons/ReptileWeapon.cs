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

    void Start()
    {
          //firePoint =  transform.Find("GameObject");
          //reptileBall = transform.Find("ReptileBall");
          //globe = GetComponent<GlobeController>();
    }

    void Update()
    {
        if (isShooting){
            
            shotCounter -= Time.deltaTime;

            if(shotCounter <= 0){
                shotCounter = timeBetweenShots;
                Debug.Log(firePoint);
                GlobeController newGlobe = Instantiate(globe,firePoint.position,firePoint.rotation) as GlobeController;
                newGlobe.speed = globeSpeed;
            }
        }else {
            shotCounter = 0;
        }
      //  else reptileBall.gameObject.SetActive(false);
    }

    // protected override void Shoot()
    // {
    //     reptileBall.gameObject.SetActive(true);
    // }
}
