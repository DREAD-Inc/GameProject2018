using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTrigger : MonoBehaviour {
  
  public GlobeController globe;
  public float globeSpeed;

  public float timeBetweenShots;
  private float shotCounter;
  public Transform firePoint;

  LineRenderer line;
    public float maxLineLength = 25f;
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }
    void Update()
    {
        //If laser has been shortened, smoothly expand
        if (line.GetPosition(1).z < maxLineLength)
            line.SetPosition(1, new Vector3(0, 0, Mathf.Lerp(line.GetPosition(1).z, maxLineLength, Time.deltaTime * 4)));
    }
    private void OnTriggerEnter(Collider other)
    {
        ManageCollision(other);
    }


    private void ManageCollision(Collider other)
    {
        //Shorten laser on any collision
        var distance = Vector3.Distance(other.transform.position, transform.position);
        if (distance < maxLineLength)
            line.SetPosition(1, new Vector3(0, 0, distance));


		   // shotCounter -= Time.deltaTime;

            if(shotCounter <= 0){
                shotCounter = timeBetweenShots;
			    GlobeController newGlobe = Instantiate(globe,firePoint.position,firePoint.rotation) as GlobeController;
                newGlobe.speed = globeSpeed;
				newGlobe.targetCharachter = other.gameObject;
		
				
              }
			else {
                 shotCounter = 0;
    	   	}


        //Deal damage to other player
        if (other.gameObject.tag == "OtherPlayer") //the part of the character model containing the collider should have this tag
        {
            var hitPlayer = other.transform.parent.parent.GetComponent<Player>();
            //print("Lasering " + hitPlayer.name);
            hitPlayer.GetComponent<Player>().TakeDamage(-2f * Time.deltaTime * 10);
        }
    }
}
