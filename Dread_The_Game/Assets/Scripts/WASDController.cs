using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDController : MonoBehaviour
{

    public float speed = 8f;
    public float Gravity = 300f;
    private Rigidbody rBody;
    private Camera cam;
    private Vector3 input;
    private Vector3 inputArrow;
    private Vector3 velocity;
    private Vector3 lookDir; //default instantiated as (0,0,0)
    private bool onGround;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

    }

    void Update()
    {
        //Enabaling Gravity to the player
        rBody.AddForce(Vector3.down * Gravity * rBody.mass);

        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        inputArrow = new Vector3(Input.GetAxisRaw("HorizontalA"), 0f, Input.GetAxisRaw("VerticalA"));

        //input
        velocity = input * speed;

        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit; //--
        if (Input.GetButton("Fire1") && Physics.Raycast(camRay, out hit))
        {
            lookDir = hit.point - transform.position;
            Debug.DrawLine(cam.transform.position, hit.point, Color.red);
        }
        else if (inputArrow.normalized != Vector3.zero) lookDir = inputArrow.normalized;
        else if (input.normalized != Vector3.zero) lookDir = input.normalized;


       
   
   
    }

    void FixedUpdate()
    {
        //print(lookDir +" "+transform.rotation);
        Quaternion rotation = Quaternion.identity;
        if (lookDir != Vector3.zero && transform.forward != Vector3.zero)
            rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 10);

        rBody.rotation = rotation;
        rBody.velocity = velocity;

         //Jumping
        if(onGround)
            {
                if( Input.GetButton("Jump"))
                {
                    rBody.velocity = new Vector3(0f, 200f , 0f);
                    onGround = false;
                }
            }
    }

    // Enablaling player to jump again as soon as it hitted ground or a surface
    void OnCollisionEnter(Collision other)
        {
        if(other.gameObject.CompareTag("GroundPlane") || other.gameObject.CompareTag("Suface"))
            {
             onGround = true;
            }
        }
}
