using UnityEngine;

namespace Battles {
    public class BodyControl : MonoBehaviour {
        [field:SerializeField]public GameObject body { get; set; }

        private void Update() {
            if (body == null) return;
            body.transform.position = this.transform.position;
        }
    }
}