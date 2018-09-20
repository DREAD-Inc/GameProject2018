using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttributes : MonoBehaviour
{
    [Header("Base Stats")]
    public string playerName = "Player"; //+id
    public float health = 100f;

    [Header("Weapon")]
    public string weapon = "Laser";
    public float rangeMultiplier = 10f;

    [Header("Speed / Distance")]
    public float speed = 8f;
    public float jumpDistance = 14f;
    public float dashDistance = 7f;

    void Start() { }

    void Update() { }
}
