using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrimsonReaper
{
    [System.Serializable]
    public struct BaseStatus
    {
        [HideInInspector] public string statusName;
        public StatusType statusType;
        public float value;
    }
}
