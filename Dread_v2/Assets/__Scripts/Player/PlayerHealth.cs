using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace DreadInc
{
    public class PlayerHealth : MonoBehaviourPunCallbacks, IPunObservable
    {
        public float Health = 100f;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
                stream.SendNext(Health);// Local player: send the others our data

            else
                this.Health = (float)stream.ReceiveNext();// Network player, receive data
        }

        void Update()
        {
            if (Health <= 0f)
            {
                GameController.Instance.LeaveRoom();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            //print(this.name + " " + other.tag);
            if (!photonView.IsMine || !(other.tag == "DamagingObject")) return;
            //var amount = other.GetComponent<projectileStats?>().GetDamage();
            Health -= 10f;
        }

        void OnTriggerStay(Collider other)
        {
            //print(this.name + " " + other.tag);
            if (!photonView.IsMine || !(other.tag == "DamagingObject")) return;
            //var amount = other.GetComponent<projectileStats?>().GetDOT();
            Health -= 3f * Time.deltaTime;
        }

    }
}
