using Harmony;
using Steamworks;
using System.Collections.Generic;
using TUNING;
using NewInfiniteResources_DLC.src.Interface;

namespace NewInfiniteResources_DLC.src
{
    public class InfiniteReg
	{
        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                Debug.Log("Mod load success!");
            }
        }

        public static string language = SteamUtils.GetSteamUILanguage();

        [HarmonyPatch(typeof(GeneratedBuildings))]
        [HarmonyPatch("LoadGeneratedBuildings")]
        public class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            private static void Prefix()
            {
                IContentInfo[] newContent = new IContentInfo[]
                {
                    new InfiniteStorageConfig(),new ArtConfig(),
                    new LightConfig(),new InfiniteBatteryConfig(),
                    new ArtLightConfig(),new ArtNightLightConfig()
                    ,new InfiniteFridgeConfig(),new SwallowStorageConfig()
                };
                foreach (IContentInfo info in newContent)
                {
                    Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ info.GetContentBasicInfo().ID.ToUpperInvariant() }.NAME", info.GetContentBasicInfo().DisplayName);
                    Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ info.GetContentBasicInfo().ID.ToUpperInvariant() }.DESC", info.GetContentBasicInfo().Description);
                    Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ info.GetContentBasicInfo().ID.ToUpperInvariant() }.EFFECT", info.GetContentBasicInfo().Effect);
                    ModUtil.AddBuildingToPlanScreen(info.GetContentBasicInfo().Category, info.GetContentBasicInfo().ID);
                }

            }
        }

        /*[HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
        public class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            private static void Postfix()
            {
                /*RegUtils.AddAllStrings(InfiniteStorageConfig.InfiniteStorageID, InfiniteStorageConfig.InfiniteStorageName, InfiniteStorageConfig.InfiniteStorageDescription, InfiniteStorageConfig.InfiniteStorageEffect);
                RegUtils.AddToPlanning(InfiniteStorageConfig.InfiniteStorageTypesOf, InfiniteStorageConfig.InfiniteStorageID, "StorageLocker");
        IContentInfo[] newContent = new IContentInfo[]
                {
                    new InfiniteStorageConfig()
                    /*new AutoStorageConfig(),new ArtConfig(),
                    new LightConfig(),new InfiniteBatteryConfig(),
                    new ArtLightConfig(),new ArtNightLightConfig()
                    ,new AutoFridgeConfig()
                };
                foreach (IContentInfo info in newContent)
                {
                    RegUtils.AddAllStrings
                        (
                            info.GetContentBasicInfo().ID,
                            info.GetContentBasicInfo().DisplayName,
                            info.GetContentBasicInfo().Description,
                            info.GetContentBasicInfo().Effect
                        );
                    RegUtils.AddToPlanning(info.GetContentBasicInfo().Category, info.GetContentBasicInfo().ID, info.GetContentBasicInfo().RecentID);
                }
            }
        }*/
    }

}
