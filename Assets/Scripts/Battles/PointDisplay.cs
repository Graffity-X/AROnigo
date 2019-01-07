using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Systems;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Battles {
    public class PointDisplay : MonoBehaviour {
        [SerializeField]private Text txt;

        private PointManager pointManager;
        private List<PlayerData> playerData;
        private PhotonView photonView;

        private string cache;

        private void Start() {
            pointManager = this.GetComponent<PointManager>();
            photonView = this.GetComponent<PhotonView>();

            //毎フレーム更新をかけキャッシュと異なった場合のみRPCで表示を更新
            //ObservableValueChangedだと複数変数の監視が面倒だったのでEveryUpdateにした
            Observable.EveryUpdate()
                .Where(n=>PhotonNetwork.player.IsMasterClient)
                .Where(n=>playerData!=null)
                .Subscribe(n => {
                    var str = "";
                    
                    foreach (var item in playerData) {
                        //清算済みの点
                        var clear_p= Enumerable.Range(0, pointManager.goaledPlayer
                                .Count(m => m == item.PhotonId))
                            .Aggregate("", (current, variable) => current + "☆");

                        //移送中の点
                        var move_p = Enumerable.Range(0, pointManager.HavePointMans
                                .Count(k => k.PlayerID == item.PhotonId))
                            .Aggregate("", (current, i) => current + "○");

                        str += item.PlayerName+" " +clear_p+":"+move_p+" \n";
                    }

                    if (cache != str) {
                        photonView.RPC("UpdateTextRPC",PhotonTargets.AllBuffered,str);
                        cache = str;
                    }
                }).AddTo(this);
        }

        [PunRPC]
        private void UpdateTextRPC(string mess) {
            txt.text = mess;
        }

        public void Launch() {
            playerData = PlayerDataCarry.PlayerData;
        }

    }
}