using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Base Stats")]
    public string playerName = "Player"; //+id
    public float health = 100f;

    [Header("Weapon")]
    public WeaponHandler.weapons weapon = WeaponHandler.weapons.LaserGun;
    public float rangeMultiplier = 10f;


    [Header("Speed / Distance")]
    public float speed = 8f;
    public float jumpDistance = 14f;
    public float dashDistance = 7f;

    private WeaponHandler wh;
    void Start()
    {
        wh = GetComponent<WeaponHandler>();
        wh.SetCurrentWeapon(weapon);
    }

    void Update() { }
}
