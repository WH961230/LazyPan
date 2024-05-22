using System;
using System.IO;
using UnityEngine;

namespace LazyPan {
    public class SaveLoad : Singleton<SaveLoad> {
        //存储数据
        public void Save(string saveFileName, object data) {
            string json = JsonUtility.ToJson(data, true);
            string path = Path.Combine(Application.persistentDataPath, saveFileName);
            try {
                if (!File.Exists(path)) {
                    File.Create(path).Dispose();
                }

                File.WriteAllText(path, json);
#if UNITY_EDITOR
                Debug.LogFormat("存储成功！路径：{0}", path);
#endif
            } catch (Exception e) {
                Debug.LogErrorFormat("错误! 信息:{0}", e.Message);
            }
        }

        //加载数据
        public T Load<T>(string loadFileName) {
            string path = Path.Combine(Application.persistentDataPath, loadFileName);
            try {
                if (!File.Exists(path)) {
                    return default;
                }

                string json = File.ReadAllText(path);
                T data = JsonUtility.FromJson<T>(json);
                return data;
            } catch (Exception e) {
                Debug.LogErrorFormat("错误! 信息:{0}", e.Message);
                throw;
            }
        }

        //删除
        public void Delete(string fileName) {
            string path = Path.Combine(Application.persistentDataPath, fileName);
            try {
                File.Delete(path);
            } catch (Exception e) {
                Debug.LogErrorFormat("错误! 信息:{0}", e.Message);
                throw;
            }
        }
    }
}