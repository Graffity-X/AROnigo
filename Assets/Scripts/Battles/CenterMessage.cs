using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Battles {
    public class CenterMessage : MonoBehaviour {
        private static Text txt;

        private void Start() {
            txt = this.GetComponent<Text>();
            
        }

        public static void Message(string str) {
            txt.text = str;
        }

        public static void Message(string str, float time) {
            Message(str);
            Observable.Timer(TimeSpan.FromSeconds(0.5f)).Subscribe(n=> {
                Message("");
            });
        }
    }
}