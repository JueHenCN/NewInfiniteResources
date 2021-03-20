using KSerialization;
using UnityEngine;
using TUNING;
using NewInfiniteResources_DLC.src.Interface;

namespace NewInfiniteResources_DLC.src
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class ArtConfig : IBuildingConfig, IContentInfo
    {

        public ContentBasicInfo GetContentBasicInfo() =>
            new ContentBasicInfo
            {
                ID = "NewISArt",
                DisplayName = InfiniteReg.language.Equals("schinese") ? "空白的画框": "Super Art",
                Description = InfiniteReg.language.Equals("schinese") ? "你看不懂的,才是艺术!": "Because Everyone Loves Art!.",
                Effect = InfiniteReg.language.Equals("schinese") ? "真的是空白的,什么也没有!":"I can see it from all the way over there!",
                Category = "Furniture"
            }; 

        public override BuildingDef CreateBuildingDef()
        {
            int width = 2;
            int height = 2;
            string anim = "painting_kanim";
            int hitpoints = BUILDINGS.HITPOINTS.TIER4;
            float c_time = BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER0;
            float[] c_mass = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
            string[] c_mats = MATERIALS.RAW_MINERALS;
            float melting_point = BUILDINGS.MELTING_POINT_KELVIN.TIER4;
            BuildLocationRule b_loc = BuildLocationRule.Anywhere;
            EffectorValues myArt = new EffectorValues() { amount = 220, radius = 50 };
            EffectorValues noisy = NOISE_POLLUTION.NONE;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(GetContentBasicInfo().ID, width, height, anim, hitpoints, c_time, c_mass, c_mats, melting_point, b_loc, myArt, noisy, 0.2f);
            buildingDef.Floodable = false;
            buildingDef.SceneLayer = Grid.SceneLayer.InteriorWall;
            buildingDef.Overheatable = false;
            buildingDef.AudioCategory = "Metal";
            buildingDef.BaseTimeUntilRepair = -1f;
            buildingDef.ViewMode = OverlayModes.Decor.ID;
            buildingDef.DefaultAnimState = "off";
            buildingDef.PermittedRotations = PermittedRotations.FlipH;
            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
            go.AddOrGet<BrisArt>();
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
        }
    }
}
