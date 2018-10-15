using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


/* This file handles movement and physics for the player */
namespace DreadInc
{
    public class PlayerMovement : MonoBehaviourPun
    {

        [Header("Settings")]
        public float speed = 2f;
        public float jumpDistance = 25f;
        public float dashDistance = 5f;
        public float smoothSpeed = 10f;
        public float gravityIncrease = 25f;

        [Header("References")]
        [SerializeField]
        private Transform followTarget;

        //Internally set references
        private Collider playerCollider;
        private Rigidbody rigidBody;
        //private Player player;
        private Slider healthBar;
        private Text healthBarText;

        //Private Fields
        private Vector3 input, inputArrow, velocity, forward;
        private bool doDash, doDie, doJump, isShooting, moved, dying;
        private float distToGround;
        private Vector3 lastPos;
        private Quaternion lastRot;

        //public Weapon weapon;
        //public GameController gameController;

        void Start()
        {
            playerCollider = transform.GetChild(transform.childCount - 1).GetComponent<Collider>(); //charactermodel with collider needs to be the last child of the player obj for this to work
            distToGround = playerCollider.bounds.extents.y;
            //distance from center of collider to end
            //setup gameobjects/components
            //groundChecker = transform.Find("GroundChecker");
            //gameController = GameObject.FindGameObjectWithTag("Global").GetComponent<GameController>();
            //cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            if (photonView.IsMine && PhotonNetwork.IsConnected)
                Camera.main.GetComponent<FollowCam>().target = followTarget;
            //else
            //    GameObject.Destroy(followTarget);

            rigidBody = GetComponent<Rigidbody>();
            rigidBody.drag = 4; // Used for dash. set dashDistance in char attributes instead        
            //weapon = player.weaponComponent;
            lastPos = Vector3.zero;

            // healthBar = GameObject.FindGameObjectWithTag("ClientHealthBar").GetComponent<Slider>();
            // healthBar.value = player.health;
            // healthBarText = GameObject.FindGameObjectWithTag("ClientHealthBar").GetComponentInChildren<Text>();
            // healthBarText.text = player.health + " | " + player.maxHealth;
            //lastRot = Quaternion.Euler(0,-90,0);

        }


        void Update()
        {
            if (!photonView.IsMine && PhotonNetwork.IsConnected)
                return;

            if (dying || checkIfDead()) return;
            //Collect settings from CharacterAttributes
            UpdateSettings();

            //Check if onGround (alternative way of setting onGround that avoids "rocket jump" bug caused by OnCollisionEnter beeing called many times when next to a surface)
            //onGround = Physics.CheckSphere(groundChecker.position, .5f, ground, QueryTriggerInteraction.Ignore);
            //onGround = groundChecker.GetComponent<SphereCollider>().bounds.extents.y < 0.1f;
            //print(groundChecker.GetComponent<SphereCollider>().bounds.extents.y + " " + onGround + " " + IsGrounded());
            //Collect input from WASD and the arrowkeys
            float horiz, horizArrow, verti, vertiArrow;
            horiz = Input.GetAxisRaw("Horizontal");
            horizArrow = Input.GetAxisRaw("HorizontalA");
            verti = Input.GetAxisRaw("Vertical");
            vertiArrow = Input.GetAxisRaw("VerticalA");

            input = new Vector3(horiz, 0, verti); // <-- WASD (For movement and lookDir)
            inputArrow = new Vector3(horizArrow, 0, vertiArrow); // <-- Arrowkeys (overwrites WASD lookDir)

            //Find and set the look direction of the character
            DetermineLookDir();

            //Movement Actions 
            if (Input.GetButtonDown("Jump")) doJump = true;
            if (Input.GetButtonDown("Dash")) doDash = true;

            //Shoot
            // if (weapon && (Input.GetButton("Fire1") || (vertiArrow > 0 || horizArrow > 0)))
            //     weapon.isShooting = true;
            // else if (weapon) weapon.isShooting = false;

            // if (!weapon) weapon = player.weaponComponent;

            //update healthbar
            //healthBar.value = player.health;
            //healthBarText.text = player.health + " / " + player.maxHealth;

            //avoid unintentional rotation due to physics calculations
            moved = input != Vector3.zero || inputArrow != Vector3.zero || Input.GetButton("Fire1") ? true : false;
            if (moved) rigidBody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            else rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        //Physics are not calculated in sync with the normal update (where input should be collected), it should be handled in FixedUpdate
        void FixedUpdate()
        {
            if (!photonView.IsMine && PhotonNetwork.IsConnected)
                return;

            //if (dying) return;
            if (IsGrounded() && doJump) Jump();
            if (doDash) Dash();
            if (doDie && !dying) Die();

            //Move the character
            rigidBody.MovePosition(rigidBody.position + input * speed * Time.fixedDeltaTime);

            //Increase gravity effect on the player
            rigidBody.AddForce(Vector3.down * gravityIncrease * rigidBody.mass);

            if (lastPos != transform.position || lastRot != transform.rotation)
            {
                //gameController.SendClientMovement(player.id, transform.position, transform.rotation);
                lastPos = transform.position;
                lastRot = transform.rotation;
            }
        }

        private bool IsGrounded()
        {
            return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
        }

        private void UpdateSettings()
        {
            //speed = player.speed;
            //jumpDistance = player.jumpDistance;
            //dashDistance = player.dashDistance;
        }

        private void DetermineLookDir()
        {
            //if Mouse1 is pressed, use a rayCast from the cameta to get a position on the map for the char to look towards
            forward = Vector3.zero;
            var cam = Camera.main;
            Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Input.GetButton("Fire1") && Physics.Raycast(camRay, out hit))
            {
                forward = hit.point - transform.position;
                forward.y = 0;
            }
            else if (inputArrow != Vector3.zero) forward = inputArrow; //when Mouse1 isnt pressed set the look direction to the key input
            else if (input != Vector3.zero) forward = input;

            //Update look direction
            if (forward != Vector3.zero) transform.forward = Vector3.Lerp(transform.forward, forward, Time.deltaTime * smoothSpeed);
        }

        private void Dash()
        {
            Vector3 dashVelocity = Vector3.Scale(input/*transform.forward*/, dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * rigidBody.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * rigidBody.drag + 1)) / -Time.deltaTime)));
            rigidBody.AddForce(dashVelocity, ForceMode.VelocityChange);
            doDash = false;
        }

        private void Jump()
        {
            rigidBody.AddForce(Vector3.up * Mathf.Sqrt(jumpDistance * -2f * Physics.gravity.y), ForceMode.VelocityChange);
            //onGround = false;
            doJump = false;
        }
        private void Die()
        {
            //Change to the "Die" animation of a character when we have one
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            rigidBody.AddForce(new Vector3(Random.Range(-.3f, .3f), 1, Random.Range(-.3f, .3f)) * Mathf.Sqrt(jumpDistance * -2f * Physics.gravity.y), ForceMode.Acceleration);
            //onGround = false;
            dying = true;
            //var coroutine = TimedDestroy(5.0f);
            //StartCoroutine(coroutine);
        }
        // public IEnumerator TimedDestroy(float wait)
        // {
        //     while (true)
        //     {
        //         yield return new WaitForSeconds(wait);
        //         print("sending dead player: " + player.id);
        //         gameController.SendPlayerDead(player.id);
        //         Destroy(this);
        //     }
        // }

        public bool checkIfDead()
        {
            // if (player.health <= 0)
            // {
            //     doDie = true;
            //     return true;
            // }
            return false;
        }
        public void setDoJump()
        {
            doDie = true;
        }

    }
}