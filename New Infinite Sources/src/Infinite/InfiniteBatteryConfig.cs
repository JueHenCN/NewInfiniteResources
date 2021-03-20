
using NewInfiniteResources_DLC.src.Interface;
using UnityEngine;
namespace NewInfiniteResources_DLC.src
{
    class InfiniteBatteryConfig : BaseBatteryConfig, IContentInfo
    {
        public ContentBasicInfo GetContentBasicInfo() =>
        new ContentBasicInfo()
        {
            ID = "InfiniteBattery",
            DisplayName = InfiniteReg.language.Equals("schinese") ? "暗物质电池":"Infinite Battery",
            Description = InfiniteReg.language.Equals("schinese") ? "无尽的能源!" : "INFINITE POWER!!!",
            Effect = InfiniteReg.language.Equals("schinese") ? "你永远无法用完的电!": "No really, just infinite power...",
            Category = "Power"
        };

        public override BuildingDef CreateBuildingDef()
        {
            int width = 1;
            int height = 2;
            int hitpoints = 30;
            string anim = "batterysm_kanim";
            float construction_time = TUNING.BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER0;
            float[] tIER = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
            string[] aLL_METALS = TUNING.MATERIALS.ALL_METALS;
            float melting_point = TUNING.BUILDINGS.MELTING_POINT_KELVIN.TIER4;
            float exhaust_temperature_active = 0f;
            float self_heat_kilowatts_active = 0f;
            EffectorValues tIER2 = TUNING.NOISE_POLLUTION.NOISY.TIER0;
            BuildingDef result = base.CreateBuildingDef(GetContentBasicInfo().ID, width, height, hitpoints, anim, construction_time, tIER, aLL_METALS, melting_point, exhaust_temperature_active, self_heat_kilowatts_active, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tIER2);
            SoundEventVolumeCache.instance.AddVolume("batterysm_kanim", "Battery_sm_rattle", TUNING.NOISE_POLLUTION.NOISY.TIER2);
            result.Floodable = false;
            result.Entombable = false;
            result.ContinuouslyCheckFoundation = false;
            result.PermittedRotations = PermittedRotations.R360;
            result.ObjectLayer = ObjectLayer.Wire;
            result.BuildLocationRule = BuildLocationRule.Conduit;
            return result;
        }


        public override void DoPostConfigureComplete(GameObject go)
        {
            Battery battery = go.AddOrGet<Battery>();
            battery.capacity = 40000f;
            battery.joulesLostPerSecond = battery.capacity * 0.05f / 600f;
            
            base.DoPostConfigureComplete(go);
        }
    }
}