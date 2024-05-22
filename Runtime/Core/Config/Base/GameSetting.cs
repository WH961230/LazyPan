using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyPan {
    [CreateAssetMenu(menuName = "LazyPan/GameSetting", fileName = "Setting")]
    public class GameSetting : ScriptableObject {
        [Header("配置脚本模板路径")] public string TxtPath;
        [Header("配置脚本输出路径")] public string ConfigScriptPath;
        [Header("生成点位配置路径")] public string LocationInformationSettingPath;

        [Header("资源配置")] public List<AssetTypeAddress> AssetTypeAddress;
        [Header("通用配置")] public List<CommonAssetAddress> CommonAssetAddress;

        public (string, string) GetAddress(AssetType type) {
            foreach (AssetTypeAddress data in AssetTypeAddress) {
                if (data.AssetType == type) {
                    return (data.AssetAddress, data.AssetSuffix);
                }
            }

            return default;
        }

        public string GetCommonAddress(CommonAssetType type) {
            foreach (CommonAssetAddress data in CommonAssetAddress) {
                if (data.AssetType == type) {
                    return data.AssetAddress;
                }
            }

            return default;
        }
    }

    [Serializable]
    public class AssetTypeAddress {
        [Header("资源类型")] public AssetType AssetType;
        [Header("资源地址")] public string AssetAddress;
        [Header("资源后缀")] public string AssetSuffix;
    }

    [Serializable]
    public class CommonAssetAddress {
        [Header("资源类型")] public CommonAssetType AssetType;
        [Header("资源地址")] public string AssetAddress;
    }

    [Serializable]
    public enum AssetType {
        ASSET,
        PREFAB,
        INPUTACTIONASSET,
        SOUND,
        SPRITE,
    }

    [Serializable]
    public enum CommonAssetType {
        CURSOR,//光标
    }
}