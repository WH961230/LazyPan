using System.IO;
using LazyPan;
using UnityEngine;

public class ReadCSV : Singleton<ReadCSV> {
    public void Read(string fileName, out string content, out string[] lines) {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Csv", fileName + ".csv");
        // 检查文件是否存在
        if (!File.Exists(filePath)) {
            content = string.Empty;
            lines = default;
            return; // 文件不存在，直接返回
        }

        using (StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/Csv/" + fileName + ".csv")) {
            string str = null;
            string line;
            while ((line = sr.ReadLine()) != null) {
                str += line + '\n';
            }

            content = str.TrimEnd('\n');
            lines = content.Split('\n');
        }
    }
}