using UnityEngine;
[System.Serializable]
public class BulletParams
{
    public int id;
    public string bulletType;
    public Vector3 position;
    public Quaternion rotation;
    public bool isExploded;

    public BulletParams(int id, string bt, Vector3 pos, Quaternion rot, bool isExp)
    {
        this.id = id;
        this.bulletType = bt;
        this.position = pos;
        this.rotation = rot;
        this.isExploded = isExp;
    }
}