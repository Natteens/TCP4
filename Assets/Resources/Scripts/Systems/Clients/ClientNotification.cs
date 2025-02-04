using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tcp4
{
    public class ClientNotification: MonoBehaviour
    {
        public Image clientImage;
        public TextMeshProUGUI amountStar;
        public Image orderImage;

        public void Setup(Sprite newClientImage, Sprite newOrderImage, float amount)
        {
            clientImage.sprite = newClientImage;
            orderImage.sprite = newOrderImage;
            amountStar.text = amount.ToString();
        }
    }
}
