using UnityEngine;
using UnityEngine.XR.iOS;

namespace Presets {
    public static class WorldOriginSetter{
        public static void Set(Transform tf) {
            UnityARSessionNativeInterface.GetARSessionNativeInterface().SetWorldOrigin (tf);            
        }
    }
}