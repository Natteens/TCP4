using System.Collections;
using Tcp4.Assets.Resources.Scripts.Managers;
using UnityEngine;

namespace Tcp4.Assets.Resources.Scripts.Systems.Clients
{
    public class Client: MonoBehaviour
    {
        public float stars;
        public float minimum;
        public string nameClient;
        public Sprite sprite;

        public void Setup(float _starts, float _minimum)
        {
            stars = _starts;
            minimum = _minimum;
            sprite = GameAssets.Instance.clientSprites[Random.Range(0, GameAssets.Instance.clientSprites.Count)];
            nameClient = GameAssets.Instance.clientNames[Random.Range(0, GameAssets.Instance.clientSprites.Count)];
        }

    }
}