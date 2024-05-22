using UnityEditor;
using UnityEngine;

namespace LazyPan {
    [CustomEditor(typeof(ColliderTool))]
    public class ColliderToolEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            ColliderTool tool = (ColliderTool)target;
            if (GUILayout.Button("添加碰撞体并注入Comp脚本")) {
                GameObject instanceParent = new GameObject("Parent");
                instanceParent.transform.parent = tool.gameObject.transform;

                GameObject trigger = GameObject.CreatePrimitive(PrimitiveType.Cube);
                trigger.transform.parent = instanceParent.transform;
                trigger.transform.position += Vector3.up * 0.5f;
                trigger.transform.name = "Trigger";
                trigger.GetComponent<Collider>().isTrigger = true;
                Comp triggerComp = trigger.AddComponent<Comp>();

                tool.GetComponent<Comp>().Transforms.Add(new Comp.TransformData() {
                    Sign = "Foot",
                    Tran = instanceParent.transform,
                });

                tool.GetComponent<Comp>().Comps.Add(new Comp.CompData() {
                    Sign = "Trigger",
                    Comp = triggerComp,
                });
            }
        }
    }
}