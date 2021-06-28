using HarmonyLib;
using System;


namespace NewInfiniteResources_DLC.src
{
    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    internal class InfiniteBattery_GeneratedBuildings_LoadGeneratedBuildings
    {
       
        [HarmonyPatch(typeof(Db), "Initialize")]
        internal class InfiniteBattery_Db_Initialize
        {
            private static void Prefix(Db __instance)
            {
                //Debug.Log(" Bri's Infinite Battery Loaded ");
            }
        }
        [HarmonyPatch(typeof(Battery), "ConsumeEnergy", new Type[] { typeof(float), typeof(bool) })]
        internal class InfiniteBattery_Battery_ConsumeEnergy
        {
            private static bool Prefix(Battery __instance)
            {
                if (__instance.gameObject.GetComponent<KPrefabID>().PrefabTag == new InfiniteBatteryConfig().GetContentBasicInfo().ID)
                {
                    return false;
                }
                return true;
            }
        }
        [HarmonyPatch(typeof(Battery), "OnSpawn")]
        internal class InfiniteBattery_Battery_OnSpawn
        {
            private static void Prefix(Battery __instance)
            {
                if (__instance.gameObject.GetComponent<KPrefabID>().PrefabTag == new InfiniteBatteryConfig().GetContentBasicInfo().ID)
                {
                    AccessTools.Field(typeof(Battery), "joulesAvailable").SetValue(__instance, 40000f);
                }
            }
        }
    }
}