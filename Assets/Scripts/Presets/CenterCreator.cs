using UnityEngine;

namespace Presets {
    public class CenterCreator : MonoBehaviour {
        [SerializeField]private GameObject centerPrefab;
        
        public GameObject Center { get; set; }
        
        
        public void Create(Vector3 pos) {
            if (!PhotonNetwork.player.IsMasterClient) return;
            Center=PhotonNetwork.Instantiate(centerPrefab.name, pos, Quaternion.identity,0);
        }
                
    }
}