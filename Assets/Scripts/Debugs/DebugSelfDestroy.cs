using UnityEngine;

namespace Debugs {
    public class DebugSelfDestroy : MonoBehaviour {
        private void Start() {
            if (Debugger.instance.debug) {
                Destroy(gameObject);
            }
        }
    }
}