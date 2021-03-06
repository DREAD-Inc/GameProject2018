﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/* This file handles movement and physics for the player */
public class PlayerController : MonoBehaviour
{
    public float smoothSpeed = 10f;
    public LayerMask ground;

    //Setting and Input variables
    private float speed, jumpDistance, dashDistance;
    private Vector3 input, inputArrow, velocity, forward;
    private bool onGround, doDash, doDie, doJump, isShooting, moved, dying;

    //Game Objects etc.
    private Transform groundChecker;
    private Rigidbody rBody;
    private Camera cam;
    private Player player;
    private Vector3 lastPos;
    private Quaternion lastRot;

    private Slider healthBar;
    private Text healthBarText;


    public Weapon weapon;
    public GameController gameController;

    void Start()
    {
        //setup gameobjects/components
        groundChecker = transform.Find("GroundChecker");
        gameController = GameObject.FindGameObjectWithTag("Global").GetComponent<GameController>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        cam.GetComponent<FollowCam>().target = transform.Find("FollowTarget");

        rBody = GetComponent<Rigidbody>();
        rBody.drag = 4; // Used for dash. Change dashDistance in char attributes instead        
        player = GetComponent<Player>();
        weapon = player.weaponComponent;
        lastPos = Vector3.zero;

        healthBar = GameObject.FindGameObjectWithTag("ClientHealthBar").GetComponent<Slider>();
        healthBar.value = player.health;
        healthBarText = GameObject.FindGameObjectWithTag("ClientHealthBar").GetComponentInChildren<Text>();
        healthBarText.text = player.health + " | " + player.maxHealth;
        //lastRot = Quaternion.Euler(0,-90,0);
    }


    void Update()
    {
        if (dying || checkIfDead()) return;
        //Collect settings from CharacterAttributes
        UpdateSettings();

        //Check if onGround (alternative way of setting onGround that avoids "rocket jump" bug caused by OnCollisionEnter beeing called many times when next to a surface)
        onGround = Physics.CheckSphere(groundChecker.position, .5f, ground, QueryTriggerInteraction.Ignore);

        //Collect input from WASD and the arrowkeys
        float horiz, horizArrow, verti, vertiArrow;
        horiz = Input.GetAxisRaw("Horizontal");
        horizArrow = Input.GetAxisRaw("HorizontalA");
        verti = Input.GetAxisRaw("Vertical");
        vertiArrow = Input.GetAxisRaw("VerticalA");

        input = new Vector3(horiz, 0, verti); // <-- WASD (For movement and lookDir)
        inputArrow = new Vector3(horizArrow, 0, vertiArrow); // <-- Arrowkeys (overwrites WASD lookDir)

        //Find and set the look direction of the character
        DetermineLookDir();

        //Movement Actions 
        if (Input.GetButtonDown("Jump")) doJump = true;
        if (Input.GetButtonDown("Dash")) doDash = true;

        //Shoot
        if (weapon && (Input.GetButton("Fire1") || (vertiArrow > 0 || horizArrow > 0)))
            weapon.isShooting = true;
        else if (weapon) weapon.isShooting = false;

        if (!weapon) weapon = player.weaponComponent;

        //update healthbar
        healthBar.value = player.health;
        healthBarText.text = player.health + " / " + player.maxHealth;

        //avoid unintentional rotation due to physics calculations
        moved = input != Vector3.zero || inputArrow != Vector3.zero || Input.GetButton("Fire1") ? true : false;
        if (moved) rBody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        else rBody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    //Physics are not calculated in sync with the normal update (where input should be collected), it should be handled in FixedUpdate
    void FixedUpdate()
    {
        //if (dying) return;

        if (onGround && doJump) Jump();
        if (doDash) Dash();
        if (doDie && !dying) Die();
        //Move the character
        rBody.MovePosition(rBody.position + input * speed * Time.fixedDeltaTime);

        //Increase gravity effect on the player
        rBody.AddForce(Vector3.down * 15f * rBody.mass);

        if (lastPos != transform.position || lastRot != transform.rotation)
        {
            gameController.SendClientMovement(player.id, transform.position, transform.rotation);
            lastPos = transform.position;
            lastRot = transform.rotation;
        }
    }

    private void UpdateSettings()
    {
        speed = player.speed;
        jumpDistance = player.jumpDistance;
        dashDistance = player.dashDistance;
    }

    private void DetermineLookDir()
    {
        //if Mouse1 is pressed, use a rayCast from the cameta to get a position on the map for the char to look towards
        forward = Vector3.zero;
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetButton("Fire1") && Physics.Raycast(camRay, out hit))
        {
            forward = hit.point - transform.position;
            forward.y = 0;
        }
        else if (inputArrow != Vector3.zero) forward = inputArrow; //when Mouse1 isnt pressed set the look direction to the key input
        else if (input != Vector3.zero) forward = input;

        //Update look direction
        if (forward != Vector3.zero) transform.forward = Vector3.Lerp(transform.forward, forward, Time.deltaTime * smoothSpeed);
    }

    private void Dash()
    {
        Vector3 dashVelocity = Vector3.Scale(input/*transform.forward*/, dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * rBody.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * rBody.drag + 1)) / -Time.deltaTime)));
        rBody.AddForce(dashVelocity, ForceMode.VelocityChange);
        doDash = false;
    }

    private void Jump()
    {
        rBody.AddForce(Vector3.up * Mathf.Sqrt(jumpDistance * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        onGround = false;
        doJump = false;
    }
    private void Die()
    {
        //Change to the "Die" animation of a character when we have one
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        rBody.AddForce(new Vector3(Random.Range(-.3f, .3f), 1, Random.Range(-.3f, .3f)) * Mathf.Sqrt(jumpDistance * -2f * Physics.gravity.y), ForceMode.Acceleration);
        onGround = false;
        dying = true;
        var coroutine = TimedDestroy(5.0f);
        StartCoroutine(coroutine);
    }
    public IEnumerator TimedDestroy(float wait)
    {
        while (true)
        {
            yield return new WaitForSeconds(wait);
            print("sending dead player: " + player.id);
            gameController.SendPlayerDead(player.id);
            Destroy(this);
        }
    }

    public bool checkIfDead()
    {
        if (player.health <= 0)
        {
            doDie = true;
            return true;
        }
        return false;
    }
    public void setDoJump()
    {
        doDie = true;
    }

}
