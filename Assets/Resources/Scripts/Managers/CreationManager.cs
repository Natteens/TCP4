using System;
using System.Collections.Generic;
using ComponentUtils.ComponentUtils.Scripts;


namespace Tcp4
{
    public class CreationManager : Singleton<CreationManager>
    {
        public event Action OnChangeInventory;
        public List<BaseProduct> Ingredients = new(2);
        

        public bool CanAdd() {return Ingredients.Count < 3;}
        public void SelectProduct(BaseProduct pd)
        {
            if(CanAdd())
            {
                StorageManager.Instance.playerInventory.RemoveProduct(pd, 1);
                AddIngredient(pd);
                OnChangeInventory?.Invoke();
            }
        }

        public void UnselectProduct(BaseProduct pd)
        {
            StorageManager.Instance.playerInventory.AddProduct(pd, 1);
            RemoveIngredient(pd);
            OnChangeInventory?.Invoke();
        }

        void AddIngredient(BaseProduct pd)
        {
            if(CanAdd())
            {
                Ingredients.Add(pd);
                return;
            }
        }

        void RemoveIngredient(BaseProduct pd)
        {
            Ingredients.Remove(pd);
            return;     
        }

        public void Create()
        {
            Drink newDrink = RefinamentManager.Instance.CreateDrink(Ingredients);
            StorageManager.Instance.playerInventory.AddProduct(newDrink, 1); //so por enquanto
        }
    }


}