using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 8f;
    public float smoothSpeed = 10f;
    public float jumpStrength = 5f;
    public float DashDistance = 5f;
    public LayerMask Ground;
    private Transform groundChecker;

    private Rigidbody rBody;
    private Camera cam;
    private Vector3 input;
    private Vector3 inputArrow;
    private Vector3 velocity;
    private bool onGround;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        groundChecker = transform.GetChild(0);

    }

    void Update()
    {
        //Enabaling Gravity to the player
        //rBody.AddForce(Vector3.down * Gravity * rBody.mass);

        //alternative way of setting onGround that avoids "rocket jump" bug
        //onGround = Physics.CheckSphere(groundChecker.position, .1f, Ground, QueryTriggerInteraction.Ignore);

        //Collect input from WASD and the arrowkeys (WASD: Horizontal/vertical, Arrowkeys: HorizontalA/VerticalA) and create vectors in their respective directions
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        inputArrow = new Vector3(Input.GetAxisRaw("HorizontalA"), 0, Input.GetAxisRaw("VerticalA"));

        //Set the velocity of the character
        velocity = input * speed;

        //if Mouse1 is pressed, use a rayCast from the cameta to get a position on the map for the char to look towards
        Vector3 forward = Vector3.zero;
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetButton("Fire1") && Physics.Raycast(camRay, out hit))
            forward = hit.point;//transform.forward = hit.point; 
        else if (inputArrow != Vector3.zero) forward = inputArrow; //when Mouse1 isnt pressed set the look direction to the key input
        else if (input != Vector3.zero) forward = input;

        //Update look direction
        if (forward != Vector3.zero) transform.forward = Vector3.Lerp(transform.forward, forward, Time.deltaTime * smoothSpeed);

        //Jumping
        if (onGround && Input.GetButton("Jump"))
        {
            //rBody.velocity = new Vector3(0f, 200f, 0f);
            //rBody.AddForce(new Vector3(0f, jumpStrength, 0f), ForceMode.Impulse);
            rBody.AddForce(Vector3.up * Mathf.Sqrt(jumpStrength * -2f * Physics.gravity.y), ForceMode.VelocityChange); //<-- jumpStrength is ~1 to 1 with unity meters with this approach

            onGround = false;
        }

        //Dashing
        if (Input.GetButtonDown("Dash"))
        {
            Vector3 dashVelocity = Vector3.Scale(transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * rBody.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * rBody.drag + 1)) / -Time.deltaTime)));
            rBody.AddForce(dashVelocity, ForceMode.VelocityChange);
        }
    }

    void FixedUpdate()
    {
        rBody.MovePosition(rBody.position + velocity * Time.fixedDeltaTime);
    }

    //Enabling player to jump again as soon as it hits ground or a surface
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("GroundPlane") || other.gameObject.CompareTag("Surface"))
        {
            rBody.isKinematic = false;
            onGround = true;
        }

    }
}
