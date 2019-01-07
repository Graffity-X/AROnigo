using Systems;
using UnityEngine;

namespace Battles {
    public class PlayersCreator : MonoBehaviour {
        [SerializeField] private GameObject playerBody;

        //全てのプレイヤーオブジェクトを作りIDをセットする
        public void Create() {
            foreach (var item in PlayerDataCarry.PlayerData) {
                var temp=PhotonNetwork.Instantiate(
                    playerBody.name, transform.position, Quaternion.identity, 0);
                temp.GetComponent<PlayerBody>().SetId(item.PhotonId);
            }
        }
    }
}