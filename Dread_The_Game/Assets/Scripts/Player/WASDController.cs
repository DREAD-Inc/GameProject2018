using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDController : MonoBehaviour
{

    public float speed = 8f;
    public float smoothSpeed = 10f;
    public float jumpStrength = 5f;
    //public float Gravity = 300f;
    private Rigidbody rBody;
    private Camera cam;
    private Vector3 input;
    private Vector3 inputArrow;
    private Vector3 velocity;
    private Vector3 lookDir; //default instantiated as (0,0,0)
    private bool onGround;
    Quaternion rotation = Quaternion.identity;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

    }

    void Update()
    {
        //Enabaling Gravity to the player - not needed 
        //rBody.AddForce(Vector3.down * Gravity * rBody.mass);

        //Collect input from WASD and the arrowkeys (WASD: Horizontal/vertical, Arrowkeys: HorizontalA/VerticalA)
        //and create vectors in their respective directions
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        inputArrow = new Vector3(Input.GetAxisRaw("HorizontalA"), 0, Input.GetAxisRaw("VerticalA"));

        //Set the velocity of the character
        velocity = input * speed;
        velocity.y = rBody.velocity.y; //Make sure not to make gravity very slow by zeroing velocity.y each frame

        //if Mouse1 is pressed, use a rayCast from the cameta to get a position on the map for the char to look towards
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit; //--
        if (Input.GetButton("Fire1") && Physics.Raycast(camRay, out hit))
            lookDir = hit.point - transform.position;

        //when lMouseButton isnt pressed set the look direction to the keyinput
        else if (inputArrow != Vector3.zero) lookDir = inputArrow.normalized;
        else if (input != Vector3.zero) lookDir = input.normalized;

        //Use Lerp (Liniar Interpolation) to rotate smoothly (over several frames) rather than instantly
        if (lookDir != Vector3.zero && transform.forward != Vector3.zero)
            rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * smoothSpeed);

    }

    void FixedUpdate()
    {
        rBody.rotation = rotation;
        rBody.velocity = velocity;

        //Jumping
        if (onGround && Input.GetButton("Jump"))
        {
            //rBody.velocity = new Vector3(0f, 200f, 0f);
            rBody.AddForce(new Vector3(0f, jumpStrength, 0f), ForceMode.Impulse);
            onGround = false;
        }
    }

    // Enabling player to jump again as soon as it hits ground or a surface
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("GroundPlane") || other.gameObject.CompareTag("Surface"))
            onGround = true;

    }
}
