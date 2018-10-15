using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DreadInc
{
    public class InteractableController : MonoBehaviour
    {

        private bool started, clicked;

        void Start()
        {
            transform.Rotate(Vector3.up * Random.Range(0, 360));
        }

        void Update()
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 30);
        }

        public virtual void Interact()
        {
            print("Interact not implemented yet for " + this.name);

        }

        void OnMouseDown()
        {
            if (clicked) return;
            //clicked = true; 

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                if (objectHit.tag == "LobbyInteractable")
                    Interact();
            }
        }
    }
}
