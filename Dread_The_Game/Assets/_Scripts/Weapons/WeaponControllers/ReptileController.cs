using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReptileController : Weapon
{

    public GlobeProjectile globe;

    public Transform firePoint;
    public GameObject laserBeam;
    public bool hasTriggered;
    private GameController gameController;


    LineRenderer line;
    private float maxLineLength = 12f;

    void Start()
    {
        line = laserBeam.GetComponent<LineRenderer>();
        gameController = GameObject.FindGameObjectWithTag("Global").GetComponent<GameController>();
    }

    void Update()
    {
        print("is main player "+transform.parent.GetComponent<Player>().IsMainPlayer());

        //If laser has been shortened, smoothly expand
        if (line.GetPosition(1).z < maxLineLength)
            line.SetPosition(1, new Vector3(0, 0, Mathf.Lerp(line.GetPosition(1).z, maxLineLength, Time.deltaTime * 4)));

        if (isShooting) Shoot();
        else StopShooting();
    }

    protected override void Shoot() { laserBeam.SetActive(true); }
    protected override void StopShooting()
    {
        laserBeam.SetActive(false);
        //charachterTrigger = ReptileTrigger.GetComponent<ReptileController>();
        hasTriggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered) ManageCollision(other);
    }


    private void ManageCollision(Collider other)
    {
        hasTriggered = true;
        //Shorten laser on any collision
        var distance = Vector3.Distance(other.transform.position, transform.position);
        if (distance < maxLineLength)
            line.SetPosition(1, new Vector3(0, 0, distance));

        GlobeProjectile newGlobe = Instantiate(globe, firePoint.position, firePoint.rotation) as GlobeProjectile;
        newGlobe.fromMainPlayer = true;
        newGlobe.targetCharachter = other.gameObject;
        
        gameController.InitiatePlayerBullet();

    }
}
