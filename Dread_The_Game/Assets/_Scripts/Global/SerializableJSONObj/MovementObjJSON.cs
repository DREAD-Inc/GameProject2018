using UnityEngine;
[System.Serializable]
public class MovementObjJSON
{
    public int id;
    public Vector3 position;
    public Quaternion rotation;

    public MovementObjJSON(int id, Vector3 pos, Quaternion rot)
    {
        this.id = id;
        this.position = pos;
        this.rotation = rot;
    }
}