using ComponentUtils.ComponentUtils.Scripts;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tcp4.Assets.Resources.Scripts.Managers
{
    public class ProductionManager : Singleton<ProductionManager>
    {
        [SerializeField] private GameObject holderChoices;

        private List<ProductionCard> choices = new();
        private CollectArea reference;

        private Production productionToSet;
        public event Action OnChooseProduction;

        public void SetupNewProduction(Production newProduction) => productionToSet = newProduction;

        public void SetupNewReference(CollectArea reference) => this.reference = reference;

        public Production GetNewProduction() => productionToSet;

        public void Clean() { productionToSet = null; reference = null; }

        public void InvokeChooseProduction()  => OnChooseProduction?.Invoke();

        public void ReloadCards()
        {
            choices.Clear();

            GameObject[] goList = GameObject.FindGameObjectsWithTag("ProductionCard");

            for(int i = 0; i < goList.Length - 1; i++)
            {
                ProductionCard p = goList[i].GetComponent<ProductionCard>();
                p.SetColletArea(reference);
                choices.Add(p);
            }
        }



    }

}