using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    LineRenderer line;
    public float maxLineLength = 10;
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
    private void OnTriggerStay(Collider other)
    {
        ManageCollision(other);
    }

    private void ManageCollision(Collider other)
    {
        //Shorten laser on any collision
        var distance = Vector3.Distance(other.transform.position, transform.position);
        if (distance < maxLineLength)
            line.SetPosition(1, new Vector3(0, 0, distance));

        //Deal damage to other player
        if (other.gameObject.tag == "OtherPlayer") //the part of the character model containing the collider should have this tag
        {
            var hitPlayer = other.transform.parent.parent.GetComponent<Player>();
            //print("Lasering " + hitPlayer.name);
            hitPlayer.GetComponent<Player>().TakeDamage(-2f * Time.deltaTime * 10);
        }
    }
}
