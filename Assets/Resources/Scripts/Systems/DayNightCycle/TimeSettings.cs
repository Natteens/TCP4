using UnityEngine;

namespace Tcp4.Resources.Scripts.Systems.DayNightCycle
{
    [CreateAssetMenu(fileName = "TimeSettings", menuName = "DayNightCycle/TimeSettings", order = 0)]
    public class TimeSettings : ScriptableObject
    {
        [Header("Hours")]
        public float timeMultiplier = 2000;
        public float startHour = 12;
        public float sunriseHour = 6;
        public float sunsetHour = 18;
        
        [Header("Calender")]
        public int startDay = 1;
        public int startMonth = 1;
        public int startYear = 2023;
    }
}
