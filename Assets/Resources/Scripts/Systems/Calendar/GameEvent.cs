// GameEvent.cs
using UnityEngine;

public enum EventCalendarType
{
    Tutorial,
    StoryEvent,
    RandomEvent,
    SeasonalEvent
}

public class GameEvent : ScriptableObject
{
    public string eventName;
    public string description;
    public int day;
    public int month;
    public int year;
    public int hour;
    public EventCalendarType eventType;
    public bool isRecurring;
}