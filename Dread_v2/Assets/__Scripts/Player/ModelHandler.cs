using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Loads and instantiates the needed model prefabs under the player gameobject. (Character, Weapon etc)*/
public class ModelHandler : MonoBehaviour
{

    public enum weapons { LaserGun, FireThrower, ReptileGun, PlasmaRifle }
    public enum characters { CapsuleBot }

    void Awake()
    {
        InstantiateWeapon(weapons.LaserGun);
        InstantiateCharacter(characters.CapsuleBot);
    }

    public void InstantiateWeapon(weapons weapon)
    {
        var prefab = (GameObject)Resources.Load("Prefabs/Weapons/" + weapon, typeof(GameObject));
        Instantiate(prefab, gameObject.transform);
    }

    public void InstantiateCharacter(characters character)
    {
        var prefab = (GameObject)Resources.Load("Prefabs/CharacterModels/" + character, typeof(GameObject));
        Instantiate(prefab, gameObject.transform);
    }
}
