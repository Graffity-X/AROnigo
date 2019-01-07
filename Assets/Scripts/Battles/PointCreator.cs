using System.Linq;
using Systems;
using Presets;
using UnityEngine;

namespace Battles {
    public class PointCreator : MonoBehaviour {
        [SerializeField] private GameObject point;
        [SerializeField] private CenterCreator centerCreator;
        [SerializeField] private int bollNumCorrection=-1;
        [SerializeField] private float createRange;
        [SerializeField] private float height;

        private float[] angles = new float[] {0, 180, 90, 270, 45, 135, 225, 315};

        //原点からanglesの角度を順に使って生成。生成順にIDをセット
        public void Create() {
            var num = PlayerDataCarry.PlayerData.Count+bollNumCorrection;
            var center = centerCreator.Center.transform.position;
            foreach (var i in Enumerable.Range(0,num)) {
                if (i >= angles.Length) {
                    angles = angles.Select(n => n+22.5f).ToArray();
                }
                var rad = angles[i]* Mathf.Deg2Rad;
                
                var pos=new Vector3(center.x+createRange*Mathf.Sin(rad),
                    center.y+height,
                    center.z+createRange*Mathf.Cos(rad));
                
                var temp = PhotonNetwork.Instantiate(point.name,pos, Quaternion.identity,0);
                temp.GetComponent<Point>().SetId(i);
            }
        }
    }
}