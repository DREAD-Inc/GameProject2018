using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace DreadInc
{
    public class PlayerManager : MonoBehaviourPun
    {
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        [Header("References")]
        [SerializeField]
        private Transform followTarget;

        private bool targetSet;

        //private Camera mainCam;

        // Use this for initialization
        void Awake()
        {
            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;
            }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
            //mainCam = Camera.main;

        }

        void Update()
        {
            if (targetSet) return;
            SetCamFollowTarget();
        }

        void SetCamFollowTarget()
        {
            //if (Camera.main.GetComponent<FollowCam>().target) return; //do nothing if target already set
            var comp = Camera.main.GetComponent<FollowCam>().target;
            print("set cam follow" + Camera.main.GetComponent<FollowCam>().target);
            if (photonView.IsMine)
                Camera.main.GetComponent<FollowCam>().target = followTarget;
            if (Camera.main.GetComponent<FollowCam>().target) targetSet = true;
        }
    }
}
