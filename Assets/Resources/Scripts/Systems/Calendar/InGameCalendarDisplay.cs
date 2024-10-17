using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace Tcp4
{
    public class InGameCalendarDisplay : MonoBehaviour
    {
        public Text yearMonthText;
        public GridLayoutGroup calendarGrid;
        public GameObject dateCellPrefab;
        public Text eventDetailsText;

        private int currentYear = 1;
        private int currentMonth = 1;
        private List<GameEvent> events = new List<GameEvent>();

        void Start()
        {
            LoadEvents();
            UpdateCalendarDisplay();
        }

        public void NextMonth()
        {
            if (currentMonth == 12)
            {
                currentMonth = 1;
                currentYear++;
            }
            else
            {
                currentMonth++;
            }
            UpdateCalendarDisplay();
        }

        public void PreviousMonth()
        {
            if (currentMonth == 1)
            {
                currentMonth = 12;
                currentYear--;
            }
            else
            {
                currentMonth--;
            }
            UpdateCalendarDisplay();
        }

        private void UpdateCalendarDisplay()
        {
            yearMonthText.text = $"{GetMonthName(currentMonth)} {currentYear}";

            foreach (Transform child in calendarGrid.transform)
            {
                Destroy(child.gameObject);
            }

            int daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);
            int startDay = (int)new DateTime(currentYear, currentMonth, 1).DayOfWeek;

            for (int i = 0; i < startDay; i++)
            {
                CreateEmptyCell();
            }

            for (int day = 1; day <= daysInMonth; day++)
            {
                CreateDateCell(day);
            }
        }

        private void CreateEmptyCell()
        {
            Instantiate(dateCellPrefab, calendarGrid.transform);
        }

        private void CreateDateCell(int day)
        {
            GameObject cell = Instantiate(dateCellPrefab, calendarGrid.transform);
            Text dateText = cell.GetComponentInChildren<Text>();
            dateText.text = day.ToString();

            var dayEvents = events.FindAll(e => e.day == day && e.month == currentMonth && e.year == currentYear);
            if (dayEvents.Count > 0)
            {
                cell.GetComponent<Image>().color = new Color(0, 1, 0, 0.2f);
            }

            Button cellButton = cell.GetComponent<Button>();
            cellButton.onClick.AddListener(() => ShowEventDetails(dayEvents));
        }

        private void ShowEventDetails(List<GameEvent> dayEvents)
        {
            string details = "";
            foreach (var evt in dayEvents)
            {
                details += $"{evt.eventName} ({evt.hour:00}:00) - {evt.description}\n";
            }
            eventDetailsText.text = details;
        }

        private void LoadEvents()
        {
            events.Clear();
            GameEvent[] loadedEvents = Resources.LoadAll<GameEvent>("GameEvents");
            events.AddRange(loadedEvents);
        }

        private string GetMonthName(int month)
        {
            return System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
        }
    }
}
