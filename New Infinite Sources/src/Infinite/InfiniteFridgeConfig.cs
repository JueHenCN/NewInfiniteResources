using KSerialization;
using UnityEngine;
using TUNING;
using NewInfiniteResources_DLC.src.Interface;
using System.Collections.Generic;

namespace NewInfiniteResources_DLC.src
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class InfiniteFridgeConfig : IBuildingConfig, IContentInfo
    {

        public ContentBasicInfo GetContentBasicInfo() =>
            new ContentBasicInfo
            {
                ID = "NewISAutoFridge",
                DisplayName = InfiniteReg.language.Equals("schinese") ? "量子冰箱" : "Infinite Fridge",
                Description = InfiniteReg.language.Equals("schinese") ? "遇事不决,量子力学!" : "Yay Moar Foooodzzzz!.",
                Effect = InfiniteReg.language.Equals("schinese") ? "吃货们的梦想!" : "Everytime you open it, theres more!",
                Category = "Food"
            };

        public override BuildingDef CreateBuildingDef()
        {
            int width = 1;
            int height = 2;
            string anim = "fridge_kanim";
            int hitpoints = BUILDINGS.HITPOINTS.TIER4;
            float construction_time = BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER0;
            float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
            string[] rawMinerals = MATERIALS.RAW_MINERALS;
            float melting_point = 1600f;
            BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
            EffectorValues nONE = NOISE_POLLUTION.NONE;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(GetContentBasicInfo().ID, width, height, anim, hitpoints, construction_time, tieR4, rawMinerals, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER5, nONE, 0.2f);
            buildingDef.RequiresPowerInput = false;
            buildingDef.EnergyConsumptionWhenActive = 0;
            buildingDef.ExhaustKilowattsWhenActive = 0;
            buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
            {
                LogicPorts.Port.OutputPort
                (
                    FilteredStorage.FULL_PORT_ID,
                    new CellOffset(0, 1), 
                    STRINGS.BUILDINGS.PREFABS.REFRIGERATOR.LOGIC_PORT, 
                    STRINGS.BUILDINGS.PREFABS.REFRIGERATOR.LOGIC_PORT_ACTIVE, 
                    STRINGS.BUILDINGS.PREFABS.REFRIGERATOR.LOGIC_PORT_INACTIVE, 
                    false, false
                )
            };
            buildingDef.Floodable = false;
            buildingDef.AudioCategory = "Metal";
            buildingDef.ViewMode = OverlayModes.Power.ID;
            buildingDef.Overheatable = false;
            buildingDef.PermittedRotations = PermittedRotations.R360;
            SoundEventVolumeCache.instance.AddVolume("fridge_kanim", "Refrigerator_open", NOISE_POLLUTION.NOISY.TIER1);
            SoundEventVolumeCache.instance.AddVolume("fridge_kanim", "Refrigerator_close", NOISE_POLLUTION.NOISY.TIER1);

            return buildingDef;
        }
        //public const string ID = "AutoFridge";
        /*public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
        {

            //GeneratedBuildings.RegisterLogicPorts(go, AutoFridgeConfig.OUTPUT_PORT);
            GeneratedBuildings.RegisterSingleLogicInputPort(go);

        }
        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            GeneratedBuildings.RegisterSingleLogicInputPort(go);
        }*/

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddOrGet<InfiniteFridge>();
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            Storage storage = go.AddOrGet<Storage>();
            storage.showInUI = true;
            storage.showDescriptor = true;
            storage.storageFilters = STORAGEFILTERS.FOOD;
            storage.allowItemRemoval = true;
            storage.capacityKg = 100f;
            storage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
            storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
            Prioritizable.AddRef(go);
            go.AddOrGet<TreeFilterable>();
            //go.AddOrGet<StorageLocker>();
            go.AddComponent<Refrigerator>();

            go.AddOrGet<InfiniteFridge>();
            go.AddOrGet<UserNameable>();
            go.AddOrGet<DropAllWorkable>();
            go.AddOrGetDef<StorageController.Def>();
        }
    }
}