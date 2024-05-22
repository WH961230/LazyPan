using System;
using System.Collections.Generic;

namespace LazyPan {
    public class UIConfig {
		public string Sign;
		public string Description;
		public int Type;
		public int RenderQueue;

        private static bool isInit;
        private static string content;
        private static string[] lines;
        private static Dictionary<string, UIConfig> dics = new Dictionary<string, UIConfig>();

        public UIConfig(string line) {
            try {
                string[] values = line.Split(',');
				Sign = values[0];
				Description = values[1];
				Type = int.Parse(values[2]);
				RenderQueue = int.Parse(values[3]);

            } catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }

        public static void Init() {
            if (isInit) {
                return;
            }
            ReadCSV.Instance.Read("UIConfig", out content, out lines);
            dics.Clear();
            for (int i = 0; i < lines.Length; i++) {
                if (i > 2) {
                    UIConfig config = new UIConfig(lines[i]);
                    dics.Add(config.Sign, config);
                }
            }

            isInit = true;
        }

        public static UIConfig Get(string sign) {
            if (dics.TryGetValue(sign, out UIConfig config)) {
                return config;
            }

            return null;
        }

        public static List<string> GetKeys() {
              if (!isInit) {
                   Init();
              }
              var keys = new List<string>();
              keys.AddRange(dics.Keys);
              return keys;
        }
    }
}