using System;
using System.Collections.Generic;
using System.Linq;
using Systems;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Battles {
    public class BattleStarter : MonoBehaviour {
        [SerializeField] private BodyControl bodyControl;
        
        public void Launch() {
            if (PhotonNetwork.player.IsMasterClient) {
                this.GetComponent<PlayersCreator>().Create();
                this.GetComponent<PointCreator>().Create();
                this.GetComponent<PointManager>().SetUp();
                this.GetComponent<PointDisplay>().Launch();
                Observable.Timer(TimeSpan.FromSeconds(0.5f)).Subscribe(n=> {
                    this.GetComponent<PhotonView>().RPC("StarterSyncRPC", PhotonTargets.AllBuffered);
                });
            }
        }
        
        [PunRPC]
        private void StarterSyncRPC() {
            CenterMessage.Message("Start!!",1f);

            
            PlayerDataCarry.PlayerBodys=GameObject.FindGameObjectsWithTag("Player")
                .Select(n => n.GetComponent<PlayerBody>())
                .ToList();

            var my_body = PlayerDataCarry.PlayerBodys.Find(n => n.MyId == PhotonNetwork.player.ID).gameObject;

            bodyControl.body = my_body;
            my_body.GetComponent<PhotonView>().RequestOwnership();

        }
        
    }
}