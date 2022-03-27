using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace Bogey.Networking
{
    public class NetworkManager : MonoBehaviour
    {
        string gameVersion = "1.0";

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        #region Photon
        public void Connected()
        {
            if (PhotonNetwork.IsConnected)
                PhotonNetwork.JoinRandomRoom();
            else
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }
        #endregion
    }
}