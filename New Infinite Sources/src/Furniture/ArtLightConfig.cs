using KSerialization;
using UnityEngine;
using TUNING;
using NewInfiniteResources_DLC.src.Interface;

namespace NewInfiniteResources_DLC.src
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class ArtLightConfig : IBuildingConfig, IContentInfo
    {
        public ContentBasicInfo GetContentBasicInfo() =>
        new ContentBasicInfo
        {
            ID = "NewISArtLight",
            DisplayName = InfiniteReg.language.Equals("schinese") ? "发光的雕塑" : "Beautiful Light",
            Description = InfiniteReg.language.Equals("schinese") ? "发光的雕塑,可能就是里面放了一盏灯!" : "Because Everyone Loves Salt Lamps!",
            Effect = InfiniteReg.language.Equals("schinese") ? "一个好看到发光的雕塑!" : "I can see it from all the way over there!",
            Category = "Furniture"
        };

        public override BuildingDef CreateBuildingDef()
        {
            int width = 1;
            int height = 2;
            string anim = "saltlamp_kanim";
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
            buildingDef.PermittedRotations = PermittedRotations.R360;
            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
            go.AddOrGet<BrisArt>();
        }
        public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
        {
            LightShapePreview lightShapePreview = go.AddComponent<LightShapePreview>();
            lightShapePreview.lux = 3000;
            lightShapePreview.radius = 500f;
            lightShapePreview.shape = LightShape.Circle;
            lightShapePreview.offset = new CellOffset((int)def.BuildingComplete.GetComponent<Light2D>().Offset.x, (int)def.BuildingComplete.GetComponent<Light2D>().Offset.y);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            Light2D light2D = go.AddOrGet<Light2D>();
            light2D.Lux = 3000;
            light2D.overlayColour = LIGHT2D.BIOLUMROCK_OVERLAYCOLOR;
            light2D.Color = LIGHT2D.BIOLUMROCK_COLOR;
            light2D.Range = 8f;
            light2D.Angle = 0.0f;
            light2D.Direction = LIGHT2D.FLOORLAMP_DIRECTION;
            light2D.Offset = LIGHT2D.FLOORLAMP_OFFSET;
            light2D.shape = LightShape.Circle;
            light2D.drawOverlay = true;
            go.AddOrGetDef<LightController.Def>();
        }
    }
}
