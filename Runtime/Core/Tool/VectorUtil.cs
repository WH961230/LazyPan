using UnityEngine;

namespace LazyPan {
    public class VectorUtil : Singleton<VectorUtil> {
        public static float Dis(Vector3 A, Vector3 B) {
            return Vector3.Distance(A, B);
        }
    }
}