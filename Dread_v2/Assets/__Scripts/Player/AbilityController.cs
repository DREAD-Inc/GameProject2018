using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace DreadInc
{
    public class AbilityController : MonoBehaviourPun, IPunObservable
    {

        private DreadInc.Weapon weapon;
        private bool isShooting;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(isShooting);
            }
            else
            {
                // Network player, receive data
                this.isShooting = (bool)stream.ReceiveNext();
            }
        }

        void Start()
        {
            //weapon = transform.GetChild(transform.childCount - 2).GetComponent<Weapon>(); // only works if the weapon component is the second to last child (see modelhandler)
            //weapon = GetComponentInChildren<Weapon>();
            SetWeapon();
        }

        void Update()
        {
            if (!weapon)
            {
                //print("weapon component not set in abilitycontroller");
                SetWeapon();
                return;
            }
            if (photonView.IsMine) //only get input from local client
                weapon.isShooting = Input.GetButton("Fire1");
        }
        // void GetInput()
        // {
        //     isShooting = Input.GetButton("Fire1");
        // }

        public void SetWeapon()
        {
            weapon = GetComponentInChildren(typeof(LaserController)) as Weapon;
            print("SetWeapon called - " + weapon);
        }
    }
}
