using Systems;
using UnityEngine;

namespace Battles {
    public class Point : MonoBehaviour {
        [field: SerializeField] public int MyID { get; set; }=-1;

        private ChaseControl chaseControl;
        private PhotonView photonView;
        
        private void Awake() {
            chaseControl = this.GetComponent<ChaseControl>();
            photonView = this.GetComponent<PhotonView>();
        }

        public void SetId(int id) {
            this.GetComponent<PhotonView>().RPC("SetIdRPC",PhotonTargets.AllBuffered,id);
        }

        [PunRPC]
        private void SetIdRPC(int id) {
            if (MyID != -1) return;
            MyID = id;
        }

        //一応ChaseControlを用いて座標を所有者オブジェクトと同期しているが、RenderとColliderをDisEnableにしているため現状意味はない
        public void ToChase(int id) {
            if(chaseControl.body==null)photonView.RPC("RenderDisEnableRPC",PhotonTargets.AllBuffered);
            chaseControl.body = PlayerDataCarry.PlayerBodys.Find(n=>n.MyId==id).gameObject;
        }

        [PunRPC]
        private void RenderDisEnableRPC() {
            this.GetComponent<MeshRenderer>().enabled = false;
        }

        public void Inactivate() {
            this.GetComponent<SphereCollider>().enabled = false;
        }

}
}