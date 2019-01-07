using System;
using System.Runtime.InteropServices;

namespace Systems {
    [StructLayout(LayoutKind.Sequential)]
    public struct PlayerData {
        public int PhotonId;
        public string PlayerName;

        public PlayerData(int photon_id,string player_name) {
            PhotonId= photon_id;
            PlayerName = player_name;
        }
    }

}