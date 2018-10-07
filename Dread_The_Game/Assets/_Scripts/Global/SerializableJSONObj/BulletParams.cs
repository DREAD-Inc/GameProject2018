using UnityEngine;
[System.Serializable]
public class BulletParams
{
    public string id;
    public string bulletType;
    public Vector3 position;
    public bool isExploded;

    public BulletParams(string id, string bt, Vector3 pos, bool isExp)
    {
        this.id = id;
        this.bulletType = bt;
        this.position = pos;
        this.isExploded = isExp;
    }
}