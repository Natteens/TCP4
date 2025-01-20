using ComponentUtils.ComponentUtils.Scripts;
using UnityEngine;

namespace Tcp4
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private GameObject productionMenu;

        public void ControlProductionMenu(bool _bool)
        { 
            productionMenu.SetActive(_bool);
        }
    }
}
