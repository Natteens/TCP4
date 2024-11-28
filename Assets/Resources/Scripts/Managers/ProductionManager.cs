using ComponentUtils.ComponentUtils.Scripts;
using System;
using System.Collections;
using UnityEngine;

namespace Tcp4.Assets.Resources.Scripts.Managers
{
    public class ProductionManager : Singleton<ProductionManager>
    {
        private Production productionToSet;
        public event Action OnChooseProduction;

        public void SetupNewProduction(Production newProduction) => productionToSet = newProduction;

        public Production GetNewProduction() => productionToSet;

        public void InvokeChooseProduction()  => OnChooseProduction?.Invoke();

    }

}