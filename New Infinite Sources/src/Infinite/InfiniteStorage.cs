using KSerialization;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace NewInfiniteResources_DLC.src
{
    //[SkipSaveFileSerialization]
    public class InfiniteStorage : KMonoBehaviour, IUserControlledCapacity , ISim1000ms, IIntSliderControl, ISidescreenButtonControl
    {
        [Serialize]
        private float userMaxCapacity = float.PositiveInfinity;

        [Serialize]
        public string lockerName = string.Empty;

        [MyCmpGet]
        private UserNameable nameable;

        private LoggerFS log;

        protected FilteredStorage filteredStorage;

        [MyCmpGet]
        private Storage storage;

        [MyCmpReq]
        private TreeFilterable treeFilterable;

        public float moreTimes = 1f;

        private bool isIncreaseAlone = true;




        public void Sim1000ms(float dt)
        {
            if (this.AmountStored < this.UserMaxCapacity)
            {
                List<Tag> acceptedTags = this.treeFilterable.AcceptedTags; // 获取选中的标签
                float averageTags = this.UserMaxCapacity / acceptedTags.Count; // 计算选中的物品平均数量
                foreach (Tag item in acceptedTags)
                {
                    if (storage.GetAmountAvailable(item) < averageTags)
                    {
                        for (int i = 0; i < (int)moreTimes; i++)
                        {
                            GameObject myItem = Instantiate(item.Prefab());
                            myItem.SetActive(true);
                            if (myItem.HasTag(GameTags.Clothes)) { storage.Store(myItem, false, false, false, true); }
                            else storage.Store(myItem, false, false, false, false);
                        }
                        if (isIncreaseAlone)
                            break;
                    }
                }
            }
        }

        protected override void OnSpawn()
        {
            List<GameObject> myObjects = new List<GameObject>();
            foreach (Tag myTagCat in GameTags.UnitCategories)
            {
                myObjects = (Assets.GetPrefabsWithTag(myTagCat));
                foreach (GameObject myObject in myObjects)
                {
                    Tag myTag;
                    if (myTagCat == GameTags.Compostable || myTagCat == GameTags.HighEnergyParticle) 
                        myTag = myObject.tag;
                    else 
                    { 
                        myTag = myObject.name.ToString();
                        if (myTagCat == GameTags.Compostable)
                        {
                            if (myTag.ToString() != "Untagged")
                                DiscoveredResources.Instance.Discover(myTag, GameTags.Seed);
                        }
                        else DiscoveredResources.Instance.Discover(myTag, myTagCat);
                    }

                }
            }
            filteredStorage.FilterChanged();
            if (!this.lockerName.IsNullOrWhiteSpace())
                SetName(lockerName);
        }

        protected override void OnPrefabInit()
        {
            Initialize(false);
        }

        protected void Initialize(bool use_logic_meter)
        {
            base.OnPrefabInit();
            //log = new LoggerFS("StorageLocker", 35);
            filteredStorage = new FilteredStorage(this, null, null, this, use_logic_meter, Db.Get().ChoreTypes.StorageFetch);
        }


        protected override void OnCleanUp()
        {
            filteredStorage.CleanUp();
        }

        private void OnCopySettings(object data)
        {
            GameObject gameObject = (GameObject)data;
            if (gameObject == null)
                return;
            InfiniteStorage component = gameObject.GetComponent<InfiniteStorage>();
            if (component == null)
                return;
            UserMaxCapacity = component.UserMaxCapacity;
        }

        public virtual float UserMaxCapacity
        {
            get { return Mathf.Min(userMaxCapacity, GetComponent<Storage>().capacityKg); }
            set
            {
                userMaxCapacity = value;
                filteredStorage.FilterChanged();
            }
        }

        public float AmountStored
        {
            get { return GetComponent<Storage>().MassStored(); }
        }

        public float MinCapacity
        {
            get { return 0.0f; }
        }

        public float MaxCapacity
        {
            get { return this.GetComponent<Storage>().capacityKg; }
        }

        public bool WholeValues
        {
            get { return false; }
        }

        public LocString CapacityUnits
        {
            get { return GameUtil.GetCurrentMassUnit(false); }
        }

        public void SetName(string name)
        {
            KSelectable component = this.GetComponent<KSelectable>();
            this.name = name;
            lockerName = name;
            if (component != null)
                component.SetName(name);
            gameObject.name = name;
            NameDisplayScreen.Instance.UpdateName(this.gameObject);
        }

        public string SliderTitleKey => "";

        public string SliderUnits => InfiniteReg.language.Equals("schinese") ? "倍" : "Times";

        public string SidescreenTitleKey => "";

        public string SidescreenStatusMessage => "";

        public string SidescreenButtonText => isIncreaseAlone ? InfiniteReg.language.Equals("schinese") ? "单一" : "Single" : InfiniteReg.language.Equals("schinese") ? "多个" :"Multiple";

        public string SidescreenButtonTooltip => "";

        public float GetSliderMax(int index)
        {
            return 100;
        }

        public float GetSliderMin(int index)
        {
            return 1;
        }

        public string GetSliderTooltipKey(int index)
        {
            return "";
        }

        public string GetSliderTooltip()
        {
            return "";
        }

        public float GetSliderValue(int index)
        {
            return moreTimes;
        }

        public void SetSliderValue(float percent, int index)
        {
            moreTimes = percent;
        }

        public int SliderDecimalPlaces(int index)
        {
            return 0;
        }

        public void OnSidescreenButtonPressed()
        {
            isIncreaseAlone = isIncreaseAlone ? false : true;
        }

        public bool SidescreenEnabled()
        {
            return true;
        }

        public bool SidescreenButtonInteractable()
        {
            return true;
        }

        public int ButtonSideScreenSortOrder()
        {
            return 0;
        }
    }
}