﻿using TUNING;
using UnityEngine;

namespace InfiniteSourceSink
{
    public class InfiniteLiquidSinkConfig : IBuildingConfig
    {
        public const string Id = "LiquidSink";
        public const string DisplayName = "Infinite Liquid Sink";
        public const string Description = "Voids all liquid sent into it.";
        public const string Effect = "Where does all the liquid go?";

        public override BuildingDef CreateBuildingDef()
        {
            var buildingDef = BuildingTemplates.CreateBuildingDef(
                id: Id,
                width: 1,
                height: 2,
                anim: "liquid_sink_kanim",
                hitpoints: BUILDINGS.HITPOINTS.TIER2,
                construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
                construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
                construction_materials: MATERIALS.ALL_METALS,
                melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER0,
                build_location_rule: BuildLocationRule.Anywhere,
                decor: BUILDINGS.DECOR.PENALTY.TIER1,
                noise: NOISE_POLLUTION.NOISY.TIER4,
                0.2f
                );
            buildingDef.InputConduitType = ConduitType.Liquid;
            buildingDef.Floodable = false;
            buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
            buildingDef.AudioCategory = "Metal";
            buildingDef.PermittedRotations = PermittedRotations.R360;
            buildingDef.UtilityInputOffset = new CellOffset(0, 0);
            buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
            go.AddOrGet<InfiniteSink>().Type = ConduitType.Liquid;
        }

        public override void DoPostConfigureComplete(GameObject go) {
            go.AddOrGet<LogicOperationalController>();
            go.AddOrGet<Operational>();

            Object.DestroyImmediate(go.GetComponent<RequireInputs>());
            Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
            Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());

            go.AddOrGetDef<OperationalController.Def>();
            BuildingTemplates.DoPostConfigure(go);
        }
    }
}
