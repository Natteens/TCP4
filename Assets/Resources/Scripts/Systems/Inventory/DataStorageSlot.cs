using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataStorageSlot: MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI amount;

    public void Setup(Sprite newSprite, int newAmount)
    {
        image.sprite = newSprite;
        amount.text = "x" + newAmount.ToString();
    }

}