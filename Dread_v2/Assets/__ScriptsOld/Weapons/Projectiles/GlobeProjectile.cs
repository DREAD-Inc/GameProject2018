using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobeProjectile : MonoBehaviour
{
    public string id;
    public float smoothSpeed = 6f;
    public GameObject targetCharachter;
    public bool fromMainPlayer = false;
    private GameController gameController;
    private Vector3 lastpos = Vector3.zero;
    public bool isExploded;
    private string bulletType = "rep";

    void Start(){
        gameController = GameObject.FindGameObjectWithTag("Global").GetComponent<GameController>();
     
    }
    void FixedUpdate()
    {
        if(fromMainPlayer){
            transform.position = Vector3.MoveTowards(transform.position, targetCharachter.transform.position, Time.deltaTime * smoothSpeed);
            gameController.MoveBullet(id, bulletType, transform.position, isExploded);
        }else{
            var bp = gameController.GetBulletParams(id);
            if (bp == null) return;
            if (bp.position != lastpos)
            {
                transform.position = bp.position;
                isExploded = bp.isExploded;
                lastpos = transform.position;
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        ManageCollision(other);
    }

    private void ManageCollision(Collision other)
    {
        //Deal damage to other player
        var hitPlayer = other.transform.parent.parent.GetComponent<Player>();
        print("Colliding " + hitPlayer.name);
        hitPlayer.GetComponent<Player>().TakeDamage(-2f * Time.deltaTime * 10);
    }


  
    // private Vector3 stupidBestFirstSearch( GameObject targetCharachter)
    // {
    // 		Vector3 newVector = new Vector3(0,0,0);
    //   		if(targetCharachter.transform.position.x - transform.position.x > 0) 
    // 		  newVector.x = 0.1f; 
    // 		else if(targetCharachter.transform.position.x - transform.position.x < 0 ) newVector.x = -0.1f;
    // 		else newVector.x = 0;

    // 		if(targetCharachter.transform.position.y - transform.position.y > 0) 
    // 		  newVector.y = 0.1f;
    // 		else if(targetCharachter.transform.position.y - transform.position.y < 0 ) newVector.y = -0.1f;
    // 		else newVector.y = 0;

    // 		if(targetCharachter.transform.position.z - transform.position.z > 0) 
    // 		  newVector.z = 0.1f;
    // 		else if(targetCharachter.transform.position.z - transform.position.z < 0 ) newVector.z = -0.1f;
    // 		else newVector.z = 0;

    // 		return newVector;
    // }
}
