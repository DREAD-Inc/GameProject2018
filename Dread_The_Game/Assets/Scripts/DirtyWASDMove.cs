using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyWASDMove : MonoBehaviour
{
    public float speed = 20f;

    void Start()
    {
    }

    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey(KeyCode.W))
            pos.z += speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            pos.z -= speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            pos.x += speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
            pos.x -= speed * Time.deltaTime;

        Vector3 targetDir;
        if (Input.GetButton("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.gameObject.transform.position, ray.direction, out hit))
                print("Found an object - distance: " + hit.distance);
            targetDir = hit.point - transform.position;
            targetDir.y += 1;
        }
        else targetDir = pos - transform.position;

        // The step size is equal to speed times frame time.
        float step = speed / 2.5f * Time.deltaTime;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        //Debug.DrawRay(transform.position, newDir * 3, Color.red);

        // Move a step closer to the target.
        transform.rotation = Quaternion.LookRotation(newDir);
        transform.position = Vector3.Lerp(transform.position, pos, 2f);
    }
}
