using System.IO;
using LazyPan;
using UnityEngine;

public class ReadCSV : Singleton<ReadCSV> {
    public void Read(string fileName, out string content, out string[] lines) {
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