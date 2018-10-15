// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace DreadInc
// {
//     public class LobbyInteractable : MonoBehaviour
//     {
//         public Launcher launcher;
//         private bool started, clicked;


//         void Start()
//         {
//             transform.Rotate(Vector3.up * Random.Range(0, 360));
//         }
//         void Update()
//         {
//             transform.Rotate(Vector3.up * Time.deltaTime * 30);
//         }

//         public void StartFreePlayGame()
//         {
//             launcher.Connect();
//         }

//         // void OnMouseDown()
//         // {
//         //     if (clicked) return;
//         //     clicked = true;

//         //     RaycastHit hit;
//         //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//         //     Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);

//         //     if (!started && Physics.Raycast(ray, out hit))
//         //     {
//         //         Transform objectHit = hit.transform;

//         //         if (objectHit.name == "StartObject")
//         //         {
//         //             launcher.Connect();
//         //             //launcher.SendMessage("Connect", this, SendMessageOptions.RequireReceiver);

//         //             started = true;
//         //         }
//         //     }
//         // }
//     }
// }
