using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{

    //private CharacterAttributes charAtt;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ShootMain(string weapon)
    {
        switch (weapon)
        {
            case "Grenade":
                //Grenade();
                break;
            default:
                Laser();
                break;
        }
    }

    private void Laser()
    {

    }
}
