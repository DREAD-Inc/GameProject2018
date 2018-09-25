using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        print("Lasering " + other.transform.parent.parent.name);
        other.transform.parent.parent.GetComponent<Player>().TakeDamage(2f * Time.deltaTime * 10);
    }
}
