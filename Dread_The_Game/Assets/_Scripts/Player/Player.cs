using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Base Stats")]
    public int id;
    public string playerName = "Player"; //+id
    public float health;

    [Header("Models")]
    public ModelHandler.weapons weapon = ModelHandler.weapons.LaserGun;
    public ModelHandler.characters character = ModelHandler.characters.CapsuleBot;

    public Weapon weaponComponent;


    [Header("Speed / Distance")]
    public float speed = 8f;
    public float jumpDistance = 14f;
    public float dashDistance = 7f;

    private ModelHandler wh;
    private GameController gameController;
    void Start()
    {
        wh = GetComponent<ModelHandler>();
        gameController = GameObject.FindGameObjectWithTag("Global").GetComponent<GameController>();

        InstantiateModels();
    }

    private void InstantiateModels()
    {
        weaponComponent = wh.InstantiateWeapon(weapon);
        wh.InstantiateCharacter(character);
    }

    public void TakeDamage(float amount)
    {

        health -= amount;
        if (health > 0) gameController.SendClientHealth(id, health);
        else
        {
            health = 0;
            print(this.name + " has died");
        }
        //Die()
    }

    public void SetFromPlayerParams(PlayerParams pp)
    {
        id = pp.id;
        name = pp.name;
        playerName = pp.name;
        health = pp.health;
        //gameObject.transform.position = pp.position;

        if (weapon != pp.weapon) { weaponComponent = wh.InstantiateWeapon(pp.weapon); weapon = pp.weapon; }
        if (character != pp.character) { wh.InstantiateCharacter(pp.character); character = pp.character; }
    }

    void Update() { }
}
