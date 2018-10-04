using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    /*Parentclass for weapons. will be needed for other weapons with traveltime such as regular guns or grenades that 
    cant just be set to inactive like a laser can, because a laser is a "solid object" that is either there or not.  */

    public bool isShooting;
    void Start() { }
    void Update() { }
    //protected Transform projectile;
    //protected Transform reptileBall;
    protected virtual void Shoot() { }
    protected virtual void StopShooting() { }

    //protected virtual void OnTriggerEnter(Collider other) {}
    //protected virtual void OnTriggerStay(Collider other) {}
    protected virtual void DealDamage(Collider other, float amount)
    {
        print(other.gameObject.tag);
        //Deal damage to other player
        if (other.gameObject.tag == "OtherPlayer") //the part of the character model containing the collider should have this tag
        {
            var hitPlayer = other.transform.parent.parent.GetComponent<Player>();
            //print("Lasering " + hitPlayer.name);
            hitPlayer.GetComponent<Player>().TakeDamage(-amount * Time.deltaTime * 10);
        }
    }
}
