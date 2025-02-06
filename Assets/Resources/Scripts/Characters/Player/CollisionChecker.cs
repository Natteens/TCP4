using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    private IUpgradable upgradeReference;
    private bool IsUpgradeTag(GameObject obj) => obj.CompareTag("Upgrade");

    void OnTriggerEnter(Collider other)
    {
        if(IsUpgradeTag(other.gameObject))
        {
            if (other.gameObject.TryGetComponent(out IUpgradable upgrade))
            {
                upgradeReference = upgrade;
                Debug.Log("Componente de Upgrade encontrado!");
            }
            else
            {
                Debug.Log("Componente de Upgrade não encontrado.");
            }
        }
        
    }

    void OnTriggerStay(Collider other)
    {
        if (IsUpgradeTag(other.gameObject))
        {
            upgradeReference?.OnStackMoney();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (IsUpgradeTag(other.gameObject))
        {
            upgradeReference = null;
        }
    }
}
