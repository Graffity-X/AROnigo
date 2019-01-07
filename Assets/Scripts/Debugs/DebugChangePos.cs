using Systems;
using UnityEngine;

namespace Debugs {
    public class DebugChangePos : MonoBehaviour {
        public Vector3 pos;

        private void Start() {
            if (Debugger.instance.debug) {
                transform.position = pos;
            }
        }
    }
}