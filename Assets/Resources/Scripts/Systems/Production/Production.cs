using System.Collections;
using UnityEngine;

namespace Tcp4
{

    [CreateAssetMenu(fileName = "Production", menuName = "Production/Production")]
    public class Production : ScriptableObject
    {
        public BaseProduct product;
        public float timeToGrow;
        public float timeToHarvest;
    }
}