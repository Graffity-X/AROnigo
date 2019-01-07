using System;
using Systems;
using Presets.ByPrintImage;
using SPU;
using UniRx;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Waits;

namespace Presets {
    public class PresetManager : MonoBehaviour {
        [SerializeField] private InputField inputField;
        [SerializeField] private PhotonConnector photonConnector;
        [SerializeField] private WaitManager waitManager;

        private ImageAnchorObserver imageAnchorObserver;

        private void Start() {
            imageAnchorObserver = this.GetComponent<ImageAnchorObserver>();
            imageAnchorObserver.CompleteSetUpPresets
                .First()
                .Delay(TimeSpan.FromSeconds(1f))
                .Subscribe(n => {
                    this.GetComponent<SPUTransTrigger>().Launch();
                    photonConnector.JoinRoom();
                    //ルームに入るのを待つ。もっとスマートにRxで待つ処理かけた気がする
                    photonConnector.ObserveEveryValueChanged(m => m.InRoom)
                        .Where(m => m)
                        .First()
                        .Subscribe(m => {
                            var name=inputField.text;
                            if (name == "") {
                                name = "PLAYER" + photonConnector.MyID;
                            }
                            waitManager.SetPlayerData(new PlayerData(photonConnector.MyID,name));
                            
                            this.GetComponent<CenterCreator>()?.Create(
                                imageAnchorObserver.imageAnchorCreator.ImageAnchorGo.transform.position);
                        });
                });
        }

        public void Launch() {
            if (!photonConnector.InRoby) return;
            imageAnchorObserver.Launch();
        }

    }
}