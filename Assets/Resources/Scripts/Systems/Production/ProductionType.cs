using System.Collections;
using Tcp4.Assets.Resources.Scripts.Managers;
using UnityEngine;

namespace Tcp4
{
    public class ProductionType : MonoBehaviour
    {
        public Production myProduction;

        public void Setup()
        {
            ProductionManager.Instance.SetupNewProduction(myProduction);
            ProductionManager.Instance.InvokeChooseProduction();
        }
    }
}