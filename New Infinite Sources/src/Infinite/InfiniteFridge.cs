using KSerialization;
using UnityEngine;
using System.Collections.Generic;
using TUNING;
using STRINGS;
using TemplateClasses;
using System;

namespace NewInfiniteResources_DLC.src
{
    public class InfiniteFridge : KMonoBehaviour, IUserControlledCapacity, IGameObjectEffectDescriptor, ISim1000ms, ISidescreenButtonControl
    {
        //STRINGS.CREATURES s;
        [SerializeField]
        public float simulatedInternalTemperature = 274.26f;
        [SerializeField]
        public float simulatedInternalHeatCapacity = 4000f;
        [SerializeField]
        public float simulatedThermalConductivity = 10000f;
        [Serialize]
        private float userMaxCapacity = float.PositiveInfinity;
        [MyCmpGet]
        private Storage storage;
        [MyCmpGet]
        private Operational operational;
        [MyCmpGet]
        private LogicPorts ports;
        private FilteredStorage filteredStorage;
        private SimulatedTemperatureAdjuster temperatureAdjuster;
        private bool isIncreaseAlone = true;


        [MyCmpReq]
        private TreeFilterable treeFilterable;
        [MyCmpGet]
        internal KBatchedAnimController controller;
        [MyCmpGet]
        public Refrigerator refs;

        public void Sim1000ms(float dt)
        {
            if (!(IsOperational)) return;
            //Console.WriteLine("当前最大值：" + AmountStored);
            if (AmountStored < UserMaxCapacity)
            {
                List<Tag> acceptedTags = treeFilterable.AcceptedTags;// 获取选中的标签
                float averageTags = UserMaxCapacity / acceptedTags.Count; // 计算选中的物品平均数量
                // 往储物箱中填充指定的物品
                foreach (Tag item in acceptedTags)
                    if (storage.GetAmountAvailable(item) < averageTags)
                    {
                        GameObject myFood = Instantiate(item.Prefab());
                        myFood.SetActive(true);
                        storage.Store(myFood, false, false, false, true);
                        //storage.Store(myFood, false, false, false, false);
                        //storage.Drop(myFood);
                        if (isIncreaseAlone)
                            break;
                    }
            }
        }
        protected override void OnPrefabInit()
        {
            filteredStorage = new FilteredStorage(this, null, new Tag[] {GameTags.Compostable}, this, true, Db.Get().ChoreTypes.FoodFetch);
           
        }
        private bool IsOperational
        {
            get
            {
                return GetComponent<Operational>().IsOperational;
            }
        }
        protected override void OnSpawn()
        {
            Tag myTag;
            foreach (EdiblesManager.FoodInfo foodTypes in EdiblesManager.GetAllFoodTypes())
            {
                Tag tag = foodTypes.Id.ToTag();
                if (foodTypes.CaloriesPerUnit > 0.0)
                    DiscoveredResources.Instance.Discover(tag, GameTags.Edible);
                if (foodTypes.CaloriesPerUnit == 0.0)
                    DiscoveredResources.Instance.Discover(tag, GameTags.CookingIngredient);
            }

            List<GameObject> myObjects = (Assets.GetPrefabsWithTag(GameTags.Medicine));
            foreach (GameObject myObject in myObjects)
            {
                myTag = myObject.PrefabID();
                if (myTag != "Untagged")
                    DiscoveredResources.Instance.Discover(myTag, GameTags.Medicine); ;
            }

            operational.SetActive(operational.IsOperational, false);
            GetComponent<KAnimControllerBase>().Play("off", KAnim.PlayMode.Once, 1f, 0.0f);
            filteredStorage.FilterChanged();
            temperatureAdjuster = new SimulatedTemperatureAdjuster(simulatedInternalTemperature, simulatedInternalHeatCapacity, simulatedThermalConductivity, base.GetComponent<Storage>());

            this.UpdateLogicCircuit();
        }

        private void OnOperationalChanged(object data)
        {
            this.operational.SetActive(this.operational.IsOperational, false);
        }

        protected override void OnCleanUp()
        {
            filteredStorage.CleanUp();
            temperatureAdjuster.CleanUp();
        }

        public bool IsActive()
        {
            return operational.IsActive;
        }

        private void OnCopySettings(object data)
        {
            GameObject gameObject = (GameObject)data;
            if (gameObject == null)
                return;
            InfiniteFridge component = gameObject.GetComponent<InfiniteFridge>();
            if (component == null)
                return;
            UserMaxCapacity = component.UserMaxCapacity;
        }

        public List<Descriptor> GetDescriptors(BuildingDef def)
        {
            return GetDescriptors(def.BuildingComplete);
        }

        public List<Descriptor> GetDescriptors(GameObject go)
        {
            return SimulatedTemperatureAdjuster.GetDescriptors(this.simulatedInternalTemperature);
        }

        public float UserMaxCapacity
        {
            get { return Mathf.Min(userMaxCapacity, storage.capacityKg); }
            set
            {
                userMaxCapacity = value;
                filteredStorage.FilterChanged();
                this.UpdateLogicCircuit();
            }
        }

        public float AmountStored
        {
            get { return storage.MassStored(); }
        }

        public float MinCapacity
        {
            get { return 0.0f; }
        }

        public float MaxCapacity
        {
            get { return storage.capacityKg; }
        }

        public bool WholeValues
        {
            get { return false; }
        }

        public LocString CapacityUnits
        {
            get { return GameUtil.GetCurrentMassUnit(false); }
        }

        public string SidescreenTitleKey => "";

        public string SidescreenStatusMessage => "";

        public string SidescreenButtonText => isIncreaseAlone ? InfiniteReg.language.Equals("schinese") ? "单一" : "Single" : InfiniteReg.language.Equals("schinese") ? "多个" : "Multiple";

        public string SidescreenButtonTooltip => "";

        private void UpdateLogicCircuitCB(object data)
        {
            this.UpdateLogicCircuit();
        }

        private void UpdateLogicCircuit()
        {
            bool flag = this.filteredStorage.IsFull();
            bool isOperational = this.operational.IsOperational;
            bool on = flag && isOperational;
            this.ports.SendSignal(FilteredStorage.FULL_PORT_ID, !on ? 0 : 1);
            this.filteredStorage.SetLogicMeter(on);
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

        private static readonly EventSystem.IntraObjectHandler<InfiniteFridge> UpdateLogicCircuitCBDelegate = new EventSystem.IntraObjectHandler<InfiniteFridge>(delegate (InfiniteFridge component, object data)
        {
            component.UpdateLogicCircuitCB(data);
        });

        private static readonly EventSystem.IntraObjectHandler<InfiniteFridge> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<InfiniteFridge>(delegate (InfiniteFridge component, object data)
        {
            component.OnCopySettings(data);
        });
    }
}
