using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OtherPlayerController : MonoBehaviour
{

    //contains methods for handling values recieved from the server
    //position, rotation, isShooting ... etc
    private GameController gameController;
    private int id;
    private Vector3 position;
    private Vector3 rotation;
    private ModelHandler.characters character;
    private ModelHandler.weapons weapon;
    private bool isShooting, doDie, dying; //we could find a different way to sync shooting, but i think this would work well for main weapons (if that doesn't include grenades etc(stuff that needs physics))
    public Image healthBar;
    private Vector3 lastpos = Vector3.zero;
    private Player player;
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("Global").GetComponent<GameController>();
        player = GetComponent<Player>();
        id = player.id;

    }

    void Update()
    {
        if (doDie && !dying)
        {
            var coroutine = TimedDestroy(5.0f);
            StartCoroutine(coroutine);
            dying = true;
        }
        healthBar.fillAmount = player.health / 100;

        var pp = gameController.GetPlayerParams(id);
        if (pp == null) return;
        if (pp.position != lastpos)
        {
            transform.position = pp.position;
            transform.rotation = pp.rotation;
            lastpos = transform.position;
            //print(id + ": " + pp.position);
        }
    }
    public void Die() { doDie = true; }

    public IEnumerator TimedDestroy(float wait)
    {
        while (true)
        {
            yield return new WaitForSeconds(wait);
            print("destroying " + id);
            Destroy(gameObject); //destroy controller only
        }
    }
}
