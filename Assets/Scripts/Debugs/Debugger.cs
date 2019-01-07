using UnityEngine;

namespace Debugs {
    public class Debugger:MonoBehaviour {
        public bool debug;
        public static Debugger instance;

        private void Awake() {
            instance = this;
    #if UNITY_EDITOR
            debug = debug;
    #else
            debug=false;
    #endif
        }
    }
}