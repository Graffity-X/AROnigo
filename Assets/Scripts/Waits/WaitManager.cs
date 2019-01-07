using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Systems;
using Battles;
using SPU;
using UnityEngine;
using UnityEngine.UI;

namespace Waits {
    public class WaitManager : MonoBehaviour {
        [Serializable]
        class PlayersBox {
            [SerializeField] public PlayerData PlayerData;
            [SerializeField] public bool startAble;

            public PlayersBox(PlayerData player_data) {
                PlayerData = player_data;
                startAble = false;
            }
        }

        [SerializeField] private List<PlayersBox> playersBoxs;
        [SerializeField] private Text infos;
        [SerializeField] private int underLimitPeopleNum = 2;
        [SerializeField] private BattleStarter battleStarter;

        public bool Enable => PhotonNetwork.player.ID != -1;
        
        private PhotonView photonView;

        private void Awake() {
            infos.text = "";
            photonView = this.GetComponent<PhotonView>();
        }

        private void UpdateText() {
            var tx = "";
            foreach (var item in playersBoxs) {
                tx += item.PlayerData.PlayerName +
                      " " + (item.startAble ? "in waiting":"in ready")+
                    "\n";
            }

            infos.text = tx;
        }

        private void CheckReady() {
            if (playersBoxs.All(n => n.startAble)&&playersBoxs.Count>=underLimitPeopleNum) {
                photonView.RPC("TransBattleRPC",PhotonTargets.AllBuffered);
            }
        }
        
        [PunRPC]
        private void TransBattleRPC() {
            this.GetComponent<SPUTransTrigger>().Launch();
            PlayerDataCarry.PlayerData = playersBoxs.Select(n => n.PlayerData).ToList();
            battleStarter.Launch();
        }

     
        
        //プレイヤー情報と待機状態の同期周り
        //OnPhotonSerializeViewを使った変数同期が一番スマートっぽいので要書き直し部分
        //SetPlayerDataがWaitManagerにあるの違う気がする
     
        public void SetPlayerData(PlayerData player_data) {
            photonView.RPC("SetPlayerNum",PhotonTargets.AllBuffered,player_data.PhotonId);
            photonView.RPC("SetPlayerName",PhotonTargets.AllBuffered,player_data.PlayerName);
        }

        private int tempNum = -1;

        [PunRPC]
        public void SetPlayerNum(int num) {
            tempNum = num;
        }

        [PunRPC]
        public void SetPlayerName(string name) {
            if (tempNum == -1) {
                ScrollLogger.Log("PlayerSetUpError");
                return;
            }    
            var p_data=new PlayerData(tempNum,name);
            playersBoxs.Add(new PlayersBox(p_data));
            tempNum = -1;
            UpdateText();
        }
        
        [PunRPC]
        public void SetPlayerState(byte[] bytes) {
            var ints = ByteTranslater.DecodeByteToInt(bytes);
            var p_num = ints[0];
            var state = Convert.ToBoolean(ints[1]);
            
            var players_box = playersBoxs.Find(n => n.PlayerData.PhotonId == p_num);
            players_box.startAble = state;
            p_num = -1;
            UpdateText();
            //チェックするのはMasterだけ
            if (PhotonNetwork.player.IsMasterClient) {
                CheckReady();
            }
        }

        
        public void ChangePlayerState(int player_num,bool state) {
            var bytes = ByteTranslater.CipherIntToByte(new int[] {player_num, Convert.ToInt32(state)});
            photonView.RPC("SetPlayerState",PhotonTargets.AllBuffered,bytes);
        }
    }
}