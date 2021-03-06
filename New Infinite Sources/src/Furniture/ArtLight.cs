namespace NewInfiniteResources_DLC.src
{
    public class BrisArtLight : GameStateMachine<BrisArtLight, BrisArtLight.Instance>
    {
        public State Off;
        public State On;

        public override void InitializeStates(out BaseState defaultState)
        {
            defaultState = Off;
            Off.PlayAnim("misc").EventTransition(GameHashes.OperationalChanged, On, smi => smi.GetComponent<Operational>().IsOperational);
            On.Enter("SetActive", smi => smi.GetComponent<Operational>().SetActive(true, false)).PlayAnim("on").EventTransition(GameHashes.OperationalChanged, Off, smi => !smi.GetComponent<Operational>().IsOperational).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null);
        }

        public class Instance : GameInstance
        {
            public Instance(IStateMachineTarget master) : base(master) { }
        }
    }
}
