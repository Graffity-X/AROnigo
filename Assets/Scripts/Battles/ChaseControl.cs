using UnityEngine;

namespace Battles {
    public class ChaseControl : MonoBehaviour {
        [field:SerializeField]public GameObject body { get; set; }

        private void Update() {
            if (body == null) return;
            this.transform.position = body.transform.position;
        }
    }
}