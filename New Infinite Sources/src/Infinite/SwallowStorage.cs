using UnityEngine;

namespace NewInfiniteResources_DLC.src
{
    public class SwallowStorage : KMonoBehaviour, ISim1000ms
    {
        [MyCmpGet]
        internal Storage storage;

        [MyCmpGet]
        internal KBatchedAnimController animController;

        private void UpdateColor()
        {
            animController.TintColour = new Color32(0 ,0 ,0,255);
        }

        public void Sim1000ms(float dt)
        {
            UpdateColor();
            foreach (GameObject go in storage.items)
                go.DeleteObject();
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            UpdateColor();
        }
    }
}
