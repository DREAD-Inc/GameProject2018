using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDController : MonoBehaviour
{

    public float speed;
    private Rigidbody rBody;
    private Camera cam;
    private Vector3 input;
    private Vector3 velocity;

    private Vector3 lookDir;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        cam = FindObjectOfType<Camera>();
    }

    void Update()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        velocity = input * speed;

        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetButton("Fire1") && Physics.Raycast(camRay, out hit))
            lookDir = hit.point - transform.position;
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
    }
}
