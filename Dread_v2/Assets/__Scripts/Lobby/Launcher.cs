using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


namespace DreadInc
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Fields
        // This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        private string gameVersion = "1";

        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        [SerializeField]
        private GameObject progressLabel;

        /// <summary>
        /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
        /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
        /// Typically this is used for the OnConnectedToMaster() callback.
        /// </summary>
        bool isConnecting;
        #endregion

        #region Monobehavior Methods
        void Awake()
        {
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        void Start()
        {
            progressLabel.SetActive(false);
        }

        #endregion

        /// <summary>
        /// Start the connection process.
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            // keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
            isConnecting = true;
            progressLabel.SetActive(true);

            // if connected, join - else initiate connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                // attempt joining a Random Room. If it fails, call OnJoinRandomFailed() and create new room.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                //connect to Photon Online Server.
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        #region MonoBehaviourPunCallbacks
        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster() - was called by PUN");
            // we don't want to do anything if we are not attempting to join a room.
            // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
            // we don't want to do anything.
            if (isConnecting)
                PhotonNetwork.JoinRandomRoom(); //calls back OnJoinRandomFailed() if no room found
        }


        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("OnDisconnected() - reason {0}", cause);
            progressLabel.SetActive(false);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed() - No random room available - Calling: PhotonNetwork.CreateRoom");

            // failed to join a random room, maybe none exists or they are all full. create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom() - client is in a room.");
            // #Critical: We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("Load deathmatch test ");

                // Load the Room Level.
                PhotonNetwork.LoadLevel("test_deathmatch");
            }
        }
        #endregion
    }
}