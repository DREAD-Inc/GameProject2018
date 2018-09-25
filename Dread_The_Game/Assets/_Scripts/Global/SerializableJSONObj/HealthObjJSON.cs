using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HealthObjJSON
{

    public int id;
    public float health;

    public HealthObjJSON(int id, float health)
    {
        this.id = id;
        this.health = health;
    }
}
