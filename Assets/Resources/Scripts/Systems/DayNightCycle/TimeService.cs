using System;
using Tcp4.Resources.Scripts.Systems.Utility;
using UnityEngine;

namespace Tcp4.Resources.Scripts.Systems.DayNightCycle
{
    public class TimeService 
    {
        readonly TimeSettings settings;
        DateTime currentTime;
        readonly TimeSpan sunriseTime;
        readonly TimeSpan sunsetTime;

        public event Action OnSunrise = delegate { }; 
        public event Action OnSunset = delegate { }; 
        public event Action OnHourChange = delegate { }; 
        public event Action OnDayPassed = delegate { };
        
        private readonly Observable<bool> isDayTime;
        private readonly Observable<int> currentHour;
        
        public TimeService(TimeSettings settings)
        {
            this.settings = settings;
            currentTime = new DateTime(settings.startYear, settings.startMonth, settings.startDay) 
                          + TimeSpan.FromHours(settings.startHour);
            sunriseTime = TimeSpan.FromHours(settings.sunriseHour);
            sunsetTime = TimeSpan.FromHours(settings.sunsetHour);

            isDayTime = new Observable<bool>(IsDayTime());
            currentHour = new Observable<int>(currentTime.Hour);
            
            isDayTime.ValueChanged += day => (day ? OnSunrise : OnSunset)?.Invoke();
            currentHour.ValueChanged += _ => OnHourChange?.Invoke();
        }

        public void UpdateTime(float deltaTime)
        {
            DateTime previousTime = currentTime;
            currentTime = currentTime.AddSeconds(deltaTime * settings.timeMultiplier);
            isDayTime.Value = IsDayTime();
            currentHour.Value = currentTime.Hour;
            if (previousTime.Day != currentTime.Day) OnDayPassed?.Invoke();
        }
        
        public float CalculateSunAngle()
        {
            bool isDay = IsDayTime();
            float startDegree = isDay ? 0 : 180;
            TimeSpan start = isDay ? sunriseTime : sunsetTime;
            TimeSpan end = isDay ? sunsetTime : sunriseTime;

            TimeSpan totalTime = CalculateDifference(start, end);
            TimeSpan elapsedTime = CalculateDifference(start, currentTime.TimeOfDay);
            
            double percentage = elapsedTime.TotalMinutes / totalTime.TotalMinutes;
            return Mathf.Lerp(startDegree, startDegree + 180, (float)percentage);
        }

        public DateTime CurrentTime => currentTime;
        bool IsDayTime() => currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime;

        TimeSpan CalculateDifference(TimeSpan from, TimeSpan to)
        {
            TimeSpan difference = to - from;
            return difference.TotalHours < 0 ? difference + TimeSpan.FromHours(24) : difference;
        }
    }
}
