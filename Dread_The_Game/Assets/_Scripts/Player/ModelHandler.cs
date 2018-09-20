using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ModelHandler : MonoBehaviour
{

    public enum weapons { LaserGun, FireThrower }
    public enum characters { CapsuleBot }


    private weapons currentWeapon = weapons.LaserGun;
    private characters currentCharacter = characters.CapsuleBot;

    private GameObject prefab;

    void Start() { }
    void Update() { }

    public Weapon InstantiateWeapon(weapons weapon)
    {
        prefab = (GameObject)Resources.Load("Prefabs/Weapons/" + weapon, typeof(GameObject));
        GameObject w = Instantiate(prefab, new Vector3(0, 0f, 0f), Quaternion.identity);
        w.transform.parent = gameObject.transform;
        //w.transform.position += init posX and posY 
        w.transform.position += new Vector3(-0.55f, 2.06f, 0); //TODO: get values from character attributes (weapon pos depends on char model dimensions)
        return w.GetComponent<Weapon>();
    }

    public void InstantiateCharacter(characters character)
    {
        prefab = (GameObject)Resources.Load("Prefabs/CharacterModels/" + character, typeof(GameObject));
        GameObject c = Instantiate(prefab, new Vector3(0, 2f, 0f), Quaternion.identity);
        c.transform.parent = gameObject.transform;
        //.transform.position += init posX and posY
    }
}
