using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace LazyPan {
    public partial class Loader {
        private static string SPRITE_PATH = "Assets/LazyPan/Bundles/Arts/Images/";
        private static string SPRITE_SUFFIX = ".png";

        public static Setting LoadSetting() {
            return Addressables.LoadAssetAsync<Setting>("Assets/LazyPan/Bundles/Configs/Setting/Setting.asset").WaitForCompletion();
        }

        public static LocationInformationSetting LoadLocationInfSetting(string sign) {
            return LoadAsset<LocationInformationSetting>(AssetType.ASSET,
                string.Concat("Setting/LocationInformationSetting/", sign));
        }

        public static GameSetting LoadGameSetting() {
            return Addressables.LoadAssetAsync<GameSetting>("Assets/LazyPan/Bundles/Configs/Setting/GameSetting.asset").WaitForCompletion();
        }

        public static T LoadAsset<T>(AssetType type, string assetName) {
            (string, string) addressData = LoadGameSetting().GetAddress(type);
            return Addressables.LoadAssetAsync<T>(string.Concat(addressData.Item1, assetName, addressData.Item2)).WaitForCompletion();
        }

        public static T LoadAsset<T>(CommonAssetType type) {
            string addressData = LoadGameSetting().GetCommonAddress(type);
            return Addressables.LoadAssetAsync<T>(addressData).WaitForCompletion();
        }

        // 加载游戏物体
        public static GameObject LoadGo(string finalName, string assetName, Transform parent, bool active) {
            (string, string) addressData = LoadGameSetting().GetAddress(AssetType.PREFAB);
            GameObject go = Addressables.InstantiateAsync(string.Concat(addressData.Item1, assetName, addressData.Item2), parent).WaitForCompletion();
            go.SetActive(active);
            go.name = finalName;
            return go;
        }

        public static Comp LoadComp(string finalName, string assetName, Transform parent, bool isActive) {
            GameObject go = LoadGo(finalName, assetName, parent, isActive);
            return go.GetComponent<Comp>();
        }

        public static AsyncOperation LoadSceneAsync(string name) {
            return SceneManager.LoadSceneAsync(name);
        }
    }
}