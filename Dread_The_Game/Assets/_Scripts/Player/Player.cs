using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Base Stats")]
    public int id;
    public string playerName = "Player"; //+id
    public float health = 100f;

    [Header("Models")]
    public ModelHandler.weapons weapon = ModelHandler.weapons.LaserGun;
    public ModelHandler.characters character = ModelHandler.characters.CapsuleBot;

    public Weapon weaponComponent;


    [Header("Speed / Distance")]
    public float speed = 8f;
    public float jumpDistance = 14f;
    public float dashDistance = 7f;

    private ModelHandler wh;
    void Start()
    {
        wh = GetComponent<ModelHandler>();
        weaponComponent = wh.InstantiateWeapon(weapon);
        wh.InstantiateCharacter(character);
    }

    void Update() { }
}
