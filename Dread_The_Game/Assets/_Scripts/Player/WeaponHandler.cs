using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponHandler : MonoBehaviour
{

    public enum weapons { LaserGun, FireThrower }

    private weapons currentWeapon = weapons.LaserGun;
    private GameObject prefab;

    void Start()
    {

    }
    void Update() { }

    private void InstantiateWeapon()
    {
        //currentWeapon = w;
        print("Prefabs/Weapons/" + currentWeapon);
        prefab = (GameObject)Resources.Load("Prefabs/Weapons/" + currentWeapon, typeof(GameObject));
        //prefab.transform.position = 
        GameObject w = Instantiate(prefab, new Vector3(0, 0f, 0f), Quaternion.identity);
        w.transform.parent = gameObject.transform;
        w.transform.position += new Vector3(-0.55f, 2.25f, 0); //TODO: get values from character attributes 
    }
    public void SetCurrentWeapon(weapons w)
    {
        currentWeapon = w;
        InstantiateWeapon();
    }

}
