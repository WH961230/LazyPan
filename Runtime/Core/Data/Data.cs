using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyPan {
    public class Data : MonoBehaviour {
       public List<BoolData> Bools = new List<BoolData>();
        public List<IntData> Ints = new List<IntData>();
        public List<FloatData> Floats = new List<FloatData>();
        public List<StringData> Strings = new List<StringData>();
        public List<Vector3Data> Vector3s = new List<Vector3Data>();

        public virtual bool Get<T>(string sign, out T t) {
            if (typeof(T) == typeof(BoolData)) {
                foreach (BoolData data in Bools) {
                    if (data.Sign == sign) {
                        t = (T)Convert.ChangeType(data, typeof(T));
                        return true;
                    }
                }
            } else if (typeof(T) == typeof(IntData)) {
                foreach (IntData data in Ints) {
                    if (data.Sign == sign) {
                        t = (T)Convert.ChangeType(data, typeof(T));
                        return true;
                    }
                }
            } else if (typeof(T) == typeof(FloatData)) {
                foreach (FloatData data in Floats) {
                    if (data.Sign == sign) {
                        t = (T)Convert.ChangeType(data, typeof(T));
                        return true;
                    }
                }
            } else if (typeof(T) == typeof(StringData)) {
                foreach (StringData data in Strings) {
                    if (data.Sign == sign) {
                        t = (T)Convert.ChangeType(data, typeof(T));
                        return true;
                    }
                }
            } else if (typeof(T) == typeof(Vector3Data)) {
                foreach (Vector3Data data in Vector3s) {
                    if (data.Sign == sign) {
                        t = (T)Convert.ChangeType(data, typeof(T));
                        return true;
                    }
                }
            }

            t = default;
            return false;
        }
    }

    [Serializable]
    public class BoolData {
        public string Sign;
        public string Description;
        public bool Bool;
    }

    [Serializable]
    public class IntData {
        public string Sign;
        public string Description;
        public int Int;
    }

    [Serializable]
    public class FloatData {
        public string Sign;
        public string Description;
        public float Float;
    }

    [Serializable]
    public class StringData {
        public string Sign;
        public string Description;
        public string String;
    }

    [Serializable]
    public class Vector3Data {
        public string Sign;
        public string Description;
        public Vector3 Vector3;
    }
}
