using System.Collections.Generic;
using System.Linq;
using Systems;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Battles {
    public class PlayerBody : MonoBehaviour {
        [field:SerializeField]public int MyId { get; set; } = -1;
        [field:SerializeField]public bool InHaving { get; set; }
        [SerializeField] private float searchRange=10;
        private PointManager pointManager;

        
        private PhotonView photonView;
        private PhotonTransformView photonTransformView;

        private Transform goalTransform;
        private Rigidbody rigidbody;

        private void Awake() {
            photonView = this.GetComponent<PhotonView>();
            photonTransformView = this.GetComponent<PhotonTransformView>();
            rigidbody = this.GetComponent<Rigidbody>();
        }
        

        private void Start() {
            pointManager = GameObject.Find("Battles").GetComponent<PointManager>();
            goalTransform = GameObject.FindWithTag("Goal").transform;
            Observable.EveryUpdate()
                .Where(n=>PhotonNetwork.player.IsMasterClient)
                .Subscribe(n => {
                    var goal_p = goalTransform.position;
                    goal_p.y = transform.position.y;
                    transform.LookAt(goal_p);

                    if (InHaving) {
                        //ゴールと自身の座標から方向ベクトル取ってRayを射出
                        var my_pos=new Vector3(transform.position.x,goalTransform.position.y,transform.position.z);
                        var ray=new Ray(my_pos,my_pos-goalTransform.position);
                        Debug.DrawRay(ray.origin,ray.direction*searchRange,Color.red);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, searchRange,LayerMask.GetMask(new string[]{"Player"}))) {
                            var target = hit.transform.gameObject;
                            ScrollLogger.Log("Hit! to "+target.GetComponent<PlayerBody>().MyId);
                            pointManager.GrabRequest(MyId,target.GetComponent<PlayerBody>().MyId);
                            InHaving = false;
                        }
                    }
                });
            Observable.EveryUpdate()
                .Where(n=>photonView.isMine)
                .Subscribe(n => {
                    photonTransformView.SetSynchronizedValues(rigidbody.velocity,0);
                });
        }

        public void SetId(int id) {
            photonView.RPC("SetIdRPC",PhotonTargets.AllBuffered,id);
        }

        [PunRPC]
        private void SetIdRPC(int id) {
            if (MyId != -1) return;
            MyId = id;
        }

        private void OnTriggerEnter(Collider other) {
            if (!PhotonNetwork.player.IsMasterClient) return;
            
            if (other.gameObject.CompareTag("Point")) {
                pointManager.GetRequest(MyId,other.gameObject.GetComponent<Point>().MyID);
            }

            if (other.gameObject.CompareTag("Goal")&&InHaving) {
                pointManager.GoalRequest(MyId);
                InHaving = false;
            }
        }
    }
}