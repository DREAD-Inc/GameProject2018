using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace DreadInc
{
    public class AbilityController : MonoBehaviourPunCallbacks, IPunObservable
    {

        private DreadInc.Weapon weapon;
        private bool isShooting;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
                stream.SendNext(isShooting);// We own this player: send the others our data
            else
                this.isShooting = (bool)stream.ReceiveNext();// Network player, receive data   
        }

        void Start()
        {
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
                isShooting = Input.GetButton("Fire1");

            weapon.isShooting = isShooting;
        }

        // void GetInput()
        // {
        //     isShooting = Input.GetButton("Fire1");
        // }

        public void SetWeapon()
        {
            weapon = GetComponentInChildren<Weapon>();
            print("SetWeapon called - " + weapon);
        }
    }
}
