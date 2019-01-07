using UnityEngine;
using UnityEngine.UI;

namespace Waits {
    public class WaitButton : MonoBehaviour {
        [SerializeField] private WaitManager waitManager;
        private bool inWait;

        [SerializeField]private Text txt;

        private void Awake() {
            txt.text = "Ready?";
        }

        public void Launch() {
            if (!waitManager.Enable) return;
            inWait = !inWait;
            txt.text = inWait ? "Waiting":"Ready?";
            waitManager.ChangePlayerState(PhotonNetwork.player.ID,inWait);
        }
    }
}