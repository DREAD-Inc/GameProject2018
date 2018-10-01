using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTrigger : MonoBehaviour {
  
  public GlobeController globe;
  public float globeSpeed;

  public Transform firePoint;
  public bool hasTriggered;


  LineRenderer line;
    private float maxLineLength = 12f;
    
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
        if(!hasTriggered) ManageCollision(other);
    }


    private void ManageCollision(Collider other)
    {
        hasTriggered = true;
        //Shorten laser on any collision
        var distance = Vector3.Distance(other.transform.position, transform.position);
        if (distance < maxLineLength)
            line.SetPosition(1, new Vector3(0, 0, distance));


			    GlobeController newGlobe = Instantiate(globe,firePoint.position,firePoint.rotation) as GlobeController;
                newGlobe.speed = globeSpeed;
				newGlobe.targetCharachter = other.gameObject;
		
    }
}
