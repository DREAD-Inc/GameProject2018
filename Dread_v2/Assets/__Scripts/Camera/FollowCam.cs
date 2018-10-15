using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DreadInc
{
    public class FollowCam : MonoBehaviour
    {

        public Transform target;

        [Header("Distance")]
        public float distance = 18f;
        public float minDistance = 14f;
        public float maxDistance = 22f;
        public Vector3 offset;

        [Header("Smoothing/Speeds")]
        public float smooth = 8f;
        public float scrollSens = 10f;


        void Start()
        {

        }

        void FixedUpdate()
        {
            if (!target) { return; }

            float num = Input.GetAxis("Mouse ScrollWheel");
            distance -= num * scrollSens;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);

            Vector3 pos = target.position + offset;
            pos -= transform.forward * distance;

            transform.position = Vector3.Lerp(transform.position, pos, smooth * 0.5f * Time.deltaTime);
        }
    }
}
