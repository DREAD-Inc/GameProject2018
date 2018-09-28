using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Loads and instantiates the needed model prefabs under the player gameobject. (Character, Weapon etc)*/
public class ModelHandler : MonoBehaviour
{

    public enum weapons { LaserGun, FireThrower, ReptileGun }
    public enum characters { CapsuleBot }

    private GameObject prefab;

    public Weapon InstantiateWeapon(weapons weapon)
    {
        prefab = (GameObject)Resources.Load("Prefabs/Weapons/" + weapon, typeof(GameObject));
        GameObject w = Instantiate(prefab, new Vector3(0, 0f, 0f), Quaternion.identity);
        w.transform.parent = gameObject.transform;
        w.transform.position += new Vector3(-0.55f, 2.06f, 0); //TODO: add empty gameobject to charactermodel and set it to weapons parent
        return w.GetComponent<Weapon>();
    }

    public void InstantiateCharacter(characters character)
    {
        prefab = (GameObject)Resources.Load("Prefabs/CharacterModels/" + character, typeof(GameObject));
        GameObject c = Instantiate(prefab, new Vector3(0, 2f, 0f), Quaternion.identity);
        c.transform.parent = gameObject.transform;
    }
}
