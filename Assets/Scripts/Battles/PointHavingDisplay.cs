using Systems;
using UnityEngine;

namespace Battles {
    public class PointHavingDisplay : MonoBehaviour {
        [SerializeField] private GameObject icon;

        private PhotonView photonView;

        private void Awake() {
            photonView = this.GetComponent<PhotonView>();
            icon.SetActive(false);
        }

        public void SetIcon(int id) {
            photonView.RPC("SetIconRPC",PhotonTargets.AllBuffered,id);
        }

        [PunRPC]
        private void SetIconRPC(int id) {
            if (id == PhotonNetwork.player.ID) {
                icon.SetActive(true);
                if (SystemInfo.supportsVibration) {
                    Handheld.Vibrate ();
                }
            }
            else {
                CenterMessage.Message(PlayerDataCarry.PlayerData.Find(n=>n.PhotonId==id).PlayerName+" get point",1f);
            }
        }

        public void UnSetIcon(int id) {
            photonView.RPC("UnSetIconRPC",PhotonTargets.AllBuffered,id);
        }

        [PunRPC]
        private void UnSetIconRPC(int id) {
            if (id == PhotonNetwork.player.ID) {
                icon.SetActive(false);
                if (SystemInfo.supportsVibration) {
                    Handheld.Vibrate ();
                }
            }
        }
        
    }
}