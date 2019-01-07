using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Systems;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;

namespace Battles {
    /// <summary>
    /// リクエスト受付と処理全部請け負ってるのでかなり肥大化、インナークラスに分割したい
    /// 当たり判定などは全てプレイヤーオブジェクト側で行い、リクエストを受け付ける形でポイントの遷移を行う
    /// Masterでしか判定と処理は行わないのでRPCは終了表示以外使っていない
    /// </summary>
    public class PointManager : MonoBehaviour{  
        //プレイヤーとポイントの対応づけ用
        [Serializable]
        public class HavePointMan {
            public int PlayerID;
            public int PointID;

            public HavePointMan(int player_id, int point_id) {
                PlayerID = player_id;
                PointID = point_id;
            }
        }
        
        [SerializeField] private List<HavePointMan> havePointMans;
        public List<HavePointMan> HavePointMans => havePointMans;
        [field:SerializeField] public List<int> goaledPlayer { get; private set; }//ゴールしたことをIDのみで記録


        public List<PlayerBody> players { get;private set; }
        private List<Point> points;

        private PointHavingDisplay pointHavingDisplay;
        
        private PhotonView photonView;

        private void Awake() {
            goaledPlayer=new List<int>();
            photonView = this.GetComponent<PhotonView>();
            pointHavingDisplay = this.GetComponent<PointHavingDisplay>();           
        }

        //プレイヤーとポイントを全取得
        public void SetUp() {
            players = GameObject.FindGameObjectsWithTag("Player")
                .Select(n => n.GetComponent<PlayerBody>())
                .ToList();

            points = GameObject.FindGameObjectsWithTag("Point")
                .Select(n => n.GetComponent<Point>())
                .ToList();
        }

        
        //最初にポイントに触れたリクエスト
        public void GetRequest(int p_id,int b_id) {
            if (havePointMans.Any(n => n.PointID == b_id)) return;
            
            var hpm = new HavePointMan(p_id, b_id);
            GetPoint(b_id).Inactivate();
            havePointMans.Add(hpm);
            HaveUpdate(hpm);
            pointHavingDisplay.SetIcon(p_id);
        }


        //奪うリクエスト
        public void GrabRequest(int current_id,int next_id) {
            foreach (var item in havePointMans.Where(n=>n.PlayerID==current_id)) {
                item.PlayerID = next_id;
                HaveUpdate(item);
            }
            pointHavingDisplay.UnSetIcon(current_id);
            pointHavingDisplay.SetIcon(next_id);

        }        
        
        //ゴールリクエスト
        public void GoalRequest(int p_id) {
            if (havePointMans.All(n => n.PlayerID != p_id)) return;
            var result_points_numbers = havePointMans.Where(n => n.PlayerID == p_id).Select(n=>n.PointID).ToArray();
            ScrollLogger.Log("Player-"+p_id+" get "+result_points_numbers.Count()+" points!!");
            goaledPlayer.Add(p_id);
            //獲得されたポイントは操作対象から外す
            foreach (var item in result_points_numbers) {
                var temp = GetPoint(item);
                points.Remove(temp);
            }

            havePointMans.RemoveAll(n => n.PlayerID == p_id);
            pointHavingDisplay.UnSetIcon(p_id);
            
            //操作対象のポイントが０になった段階で終了処理
            if (points.Count == 0) {
                ScrollLogger.Log("BattleEnd");
                var mes = "Winner ";
                //最大数を求め、一致するプレイヤー全てを勝者として表示
                var max=goaledPlayer.GroupBy(i=>i)
                    .Select(n=>n.Count())
                    .Max();
                foreach (var item in goaledPlayer.GroupBy(n=>n)) {
                    if (item.Count() == max) {
                        mes+=" "+PlayerDataCarry.PlayerData.Find(i=>i.PhotonId==item.First()).PlayerName+" ";
                    }
                }
                photonView.RPC("FinishRPC",PhotonTargets.AllBuffered,mes);
            }
        }

        [PunRPC]
        private void FinishRPC(string mes) {
            CenterMessage.Message(mes);
        }
        
        //所持者の更新
        private void HaveUpdate(HavePointMan man) {
            GetPoint(man.PointID).ToChase(man.PlayerID);
            GetPlayer(man.PlayerID).InHaving = true;
        }

        private PlayerBody GetPlayer(int i) {
            return players.Find(n => n.MyId == i);
        }

        private Point GetPoint(int i) {
            return points.Find(n => n.MyID == i);
        }

    }
}