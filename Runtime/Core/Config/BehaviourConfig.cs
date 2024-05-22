using System;
using System.Collections.Generic;

namespace LazyPan {
    public class BehaviourConfig {
		public string Sign;
		public string Name;
		public string Description;

        private static bool isInit;
        private static string content;
        private static string[] lines;
        private static Dictionary<string, BehaviourConfig> dics = new Dictionary<string, BehaviourConfig>();

        public BehaviourConfig(string line) {
            try {
                string[] values = line.Split(',');
				Sign = values[0];
				Name = values[1];
				Description = values[2];

            } catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }

        public static void Init() {
            if (isInit) {
                return;
            }
            ReadCSV.Instance.Read("BehaviourConfig", out content, out lines);
            dics.Clear();
            for (int i = 0; i < lines.Length; i++) {
                if (i > 2) {
                    BehaviourConfig config = new BehaviourConfig(lines[i]);
                    dics.Add(config.Sign, config);
                }
            }

            isInit = true;
        }

        public static BehaviourConfig Get(string sign) {
            if (dics.TryGetValue(sign, out BehaviourConfig config)) {
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