//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerParams
{
    public int id;
    public string name;
    public Vector3 position;
    public Quaternion rotation;
    public ModelHandler.characters character;
    public ModelHandler.weapons weapon;
    public bool isShooting;

    public PlayerParams(int id, string name, Vector3 position, Quaternion rotation, ModelHandler.characters character, ModelHandler.weapons weapon)
    {
        this.id = id;
        this.name = name;
        this.position = position;
        this.rotation = rotation;
        this.character = character;
        this.weapon = weapon;
        this.isShooting = false;
    }

    public static PlayerParams CreateFromJSON(string jsonStr) { return JsonUtility.FromJson<PlayerParams>(jsonStr); }

    public int getId() { return id; }
    public void setId(int id) { this.id = id; }
    public string getName() { return name; }
    public void setName(string name) { this.name = name; }
    public Vector3 getPosition() { return position; }
    public void setPosition(Vector3 position) { this.position = position; }
    public Quaternion getRotation() { return rotation; }
    public void setRotation(Quaternion rotation) { this.rotation = rotation; }
    public ModelHandler.characters getCharacter() { return character; }
    public void setCharacter(ModelHandler.characters character) { this.character = character; }
    public ModelHandler.weapons getWeapon() { return weapon; }
    public void setWeapon(ModelHandler.weapons weapon) { this.weapon = weapon; }
    public bool getIsShooting() { return isShooting; }
    public void setIsShooting(bool isShooting) { this.isShooting = isShooting; }
}
