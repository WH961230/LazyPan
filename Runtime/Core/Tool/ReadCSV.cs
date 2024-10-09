using System;
using System.IO;
using LazyPan;
using UnityEditor;
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

        try {
            using (StreamReader sr = new StreamReader(filePath)) {
                string str = null;
                string line;
                while ((line = sr.ReadLine()) != null) {
                    str += line + '\n';
                }

                content = str.TrimEnd('\n');
                lines = content.Split('\n');
            }
        } catch {
            content = string.Empty;
            lines = default;
            Debug.LogError($"错误 {filePath} 配置读取错误，需要将外部 Excel 软件关闭，请检查!");
            EditorApplication.isPlaying = false;
        }
    }

    public void Write(string fileName, string[] lines) {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Csv", fileName + ".csv");

        try {
            using (StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8)) {
                foreach (var line in lines) {
                    sw.WriteLine(line);
                }
            }

            Debug.Log($"成功写入文件 {filePath}");
        } catch (Exception ex) {
            Debug.LogError($"写入文件 {filePath} 时出错: {ex.Message}");
        }
    }
}