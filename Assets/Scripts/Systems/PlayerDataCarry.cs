using System.Collections.Generic;
using Battles;
using UnityEngine;

namespace Systems {
    //IDと名前の対応をメンバで持たせるのは面倒だったのでstaticにした。
    //何度かプレイヤーオブジェクトに検索をかけることがあるのでパフォーマンスのためにこれも保持
    public static class PlayerDataCarry {
        public static List<PlayerData> PlayerData;
        public static List<PlayerBody> PlayerBodys=new List<PlayerBody>();
    }
}