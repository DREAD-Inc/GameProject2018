using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This file handles movement and physics for the player. */
public class PlayerController : MonoBehaviour
{
    public float smoothSpeed = 10f;
    public LayerMask ground;

    //Setting and Input variables
    private float speed, jumpDistance, dashDistance;
    private Vector3 input, inputArrow, velocity, forward;
    private bool onGround, doDash, doJump;

    //Game Objects etc.
    private Transform groundChecker;
    private Rigidbody rBody;
    private Camera cam;
    private CharacterAttributes ca;

    void Start()
    {
        groundChecker = transform.GetChild(0);
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rBody = GetComponent<Rigidbody>();
        rBody.drag = 4; // Used for dash. Change dashDistance in char attributes instead
        ca = GetComponent<CharacterAttributes>();
    }


    void Update()
    {
        //Collect settings from CharacterAttributes
        UpdateSettings();

        //Check if onGround (alternative way of setting onGround that avoids "rocket jump" bug caused by OnCollisionEnter beeing called many times when next to a surface)
        onGround = Physics.CheckSphere(groundChecker.position, .1f, ground, QueryTriggerInteraction.Ignore);

        //Collect input from WASD and the arrowkeys and create vectors in their respective directions. These are used for moving the char and rotating it.
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")); // <-- WASD (For movement and lookDir)
        inputArrow = new Vector3(Input.GetAxisRaw("HorizontalA"), 0, Input.GetAxisRaw("VerticalA")); // <-- Arrowkeys (overwrites WASD lookDir)

        //Find and set the look direction of the character
        DetermineLookDir();

        //Movement Actions 
        if (Input.GetButtonDown("Jump")) doJump = true;
        if (Input.GetButtonDown("Dash")) doDash = true;
    }

    //Physics are not calculated in sync with the normal update (where input should be collected),  
    void FixedUpdate()
    {
        if (onGround && doJump) Jump();
        if (doDash) Dash();
        //Move the character
        rBody.MovePosition(rBody.position + input * speed * Time.fixedDeltaTime);

        //Increase gravity effect on the player
        rBody.AddForce(Vector3.down * 15f * rBody.mass);
    }

    private void UpdateSettings()
    {
        speed = ca.speed;
        jumpDistance = ca.jumpDistance;
        dashDistance = ca.dashDistance;
    }

    private void DetermineLookDir()
    {
        //if Mouse1 is pressed, use a rayCast from the cameta to get a position on the map for the char to look towards
        forward = Vector3.zero;
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetButton("Fire1") && Physics.Raycast(camRay, out hit))
            forward = hit.point - transform.position;
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
}
