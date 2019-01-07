using UnityEngine;

namespace Systems {
    public class PhotonConnector : MonoBehaviour {

        public int MyID => PhotonNetwork.player.ID;
        [field:SerializeField]public bool InRoom { get; set; }
        [field:SerializeField]public bool InRoby { get; set; }
        [field:SerializeField]public bool TransferOwnershipOnRequest { get; set; } = true;

        private void Start() {
            PhotonNetwork.ConnectUsingSettings("v0.0.0");
        }

        void OnJoinedLobby() {
            ScrollLogger.Log("join lobby");
            InRoby = true;
        }
        
        void OnCreatedRoom() {
            ScrollLogger.Log("created room");
        }

        void OnJoinedRoom() {
            ScrollLogger.Log("join room. my id is "+PhotonNetwork.player.ID);
            InRoom = true;
        }

        void OnPhotonRandomJoinFailed() {
            ScrollLogger.Log("failed join room.try create room......");
            PhotonNetwork.CreateRoom("Onigo");
        }

        public void JoinRoom() {
            ScrollLogger.Log("try connect......");
            PhotonNetwork.JoinRandomRoom();
        }
        
        public void OnOwnershipRequest(object[] viewAndPlayer)
        {
            PhotonView view = viewAndPlayer[0] as PhotonView;
            PhotonPlayer requestingPlayer = viewAndPlayer[1] as PhotonPlayer;

            Debug.Log("OnOwnershipRequest(): Player " + requestingPlayer + " requests ownership of: " + view + ".");
            if (this.TransferOwnershipOnRequest)
            {
                view.TransferOwnership(requestingPlayer.ID);
            }
        }

        public void OnOwnershipTransfered (object[] viewAndPlayers)
        {
            PhotonView view = viewAndPlayers[0] as PhotonView;
		
            PhotonPlayer newOwner = viewAndPlayers[1] as PhotonPlayer;
		
            PhotonPlayer oldOwner = viewAndPlayers[2] as PhotonPlayer;
		
            Debug.Log( "OnOwnershipTransfered for PhotonView"+view.ToString()+" from "+oldOwner+" to "+newOwner);
        }
    }
}