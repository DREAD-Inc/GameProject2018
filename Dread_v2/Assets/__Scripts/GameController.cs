﻿using System;
using System.Collections;
using System.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

namespace DreadInc
{
    public class GameController : MonoBehaviourPunCallbacks
    {
        public static GameController Instance;
        public GameObject playerPrefab;
        public bool usingTeams;

        public GameObject[] spawnPoints;

        void Start()
        {
            Instance = this;
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManager.GetActiveScene().name);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate

                if (!usingTeams)
                {
                    //spawn at random position
                    PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                    PhotonNetwork.LocalPlayer.SetTeam(PunTeams.Team.none);
                }
                else
                    SetTeamInstantiate();
            }
        }

        #region Photon Callbacks
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0); //first scene in build
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                //LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


            // if (PhotonNetwork.IsMasterClient)
            // {
            //     Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            //     LoadLevel("test_deathmatch");
            // }

        }

        #endregion


        #region Public Methods

        public void SetTeamInstantiate()
        {
            print(PhotonNetwork.LocalPlayer.GetPlayerNumber() + " " + PhotonNetwork.PlayerList.Length % 2);
            if (PhotonNetwork.PlayerList.Length % 2 == 0)//even / uneven
                PhotonNetwork.LocalPlayer.SetTeam(PunTeams.Team.red);
            else
                PhotonNetwork.LocalPlayer.SetTeam(PunTeams.Team.blue);

            var spawn = spawnPoints[PhotonNetwork.PlayerList.Length].transform; //not -1 because player is added after this
            PhotonNetwork.Instantiate(this.playerPrefab.name, spawn.position, Quaternion.identity, 0);

            //foreach (var x in PhotonNetwork.PlayerList)
            //    print(x.GetTeam());
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void LoadLevel(string name)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.LogFormat("PhotonNetwork : Loading : {0}", name);
            PhotonNetwork.LoadLevel(name);
        }
        #endregion
    }
}