using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyPan {
    [CreateAssetMenu(menuName = "LazyPan/LocationInformationSetting")]
    public class LocationInformationSetting : ScriptableObject {
        public string SettingName;
        public List<LocationInformationData> locationInformationDatas;
    }

    [Serializable]
    public class LocationInformationData {
        public Vector3 Position;
        public Vector3 Rotation;
    }
}