using System.Collections;
using Tcp4.Assets.Resources.Scripts.Managers;
using UnityEngine;

namespace Tcp4
{
    public class ProductionCard : MonoBehaviour
    {
        public Production myProduction;
        private CollectArea reference;

        public void SetColletArea(CollectArea collectarea) => reference = collectarea;

        public void Setup()
        {
            ProductionManager.Instance.SetupNewProduction(myProduction);
            ProductionManager.Instance.InvokeChooseProduction();
        }
    }
}