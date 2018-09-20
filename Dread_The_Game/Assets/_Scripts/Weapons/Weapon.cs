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
    protected Transform projectile;
    protected virtual void Shoot() { }
}
