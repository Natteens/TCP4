using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace Tcp4
{
    public class CalendarEditorWindow : EditorWindow
    {
        private int currentYear = 1;
        private int currentMonth = 1;
        private Vector2 scrollPosition;
        private GameEvent selectedEvent;
        private List<GameEvent> events = new List<GameEvent>();
        private const int GRID_SIZE = 35; // 7x5 grid

        // Variável para gerenciamento de arrastar e soltar
        private GameEvent draggedEvent = null;

        [MenuItem("Tools/Game Calendar")]
        public static void ShowWindow()
        {
            GetWindow<CalendarEditorWindow>("Game Calendar");
        }

        void OnGUI()
        {
            // Título
            GUILayout.Label("Game Calendar Editor", EditorStyles.boldLabel);

            // Controles de mês/ano
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("<", GUILayout.Width(30))) PreviousMonth();
            GUILayout.Label($"{GetMonthName(currentMonth)} {currentYear}", EditorStyles.boldLabel);
            if (GUILayout.Button(">", GUILayout.Width(30))) NextMonth();
            EditorGUILayout.EndHorizontal();

            // Grid de dias do calendário
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            DrawCalendarGrid();
            EditorGUILayout.EndScrollView();

            // Editor de eventos
            EditorGUILayout.Space();
            DrawEventEditor();
        }

        private void DrawCalendarGrid()
        {
            int daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);
            int startDay = (int)new DateTime(currentYear, currentMonth, 1).DayOfWeek;

            EditorGUILayout.BeginVertical();
            for (int i = 0; i < GRID_SIZE; i += 7)
            {
                EditorGUILayout.BeginHorizontal();
                for (int j = 0; j < 7; j++)
                {
                    int day = i + j + 1 - startDay;
                    if (day > 0 && day <= daysInMonth)
                    {
                        if (DrawDateCell(day)) CreateNewEvent(day); // Se clicar no dia, cria evento
                    }
                    else
                    {
                        GUILayout.Button("", GUILayout.Width(40), GUILayout.Height(40)); // Espaços vazios
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        // Método para desenhar cada célula de data no grid
        private bool DrawDateCell(int day)
        {
            Rect rect = EditorGUILayout.BeginVertical("box", GUILayout.Width(40), GUILayout.Height(40));
            GUILayout.Label(day.ToString(), EditorStyles.centeredGreyMiniLabel);

            // Checa eventos para o dia
            var dayEvents = events.FindAll(e => e.day == day && e.month == currentMonth && e.year == currentYear);
            if (dayEvents.Count > 0)
            {
                EditorGUI.DrawRect(GUILayoutUtility.GetLastRect(), new Color(0, 1, 0, 0.2f)); // Cor para dias com eventos
            }

            // Detecta clique e arraste de eventos
            bool clicked = GUILayout.Button("", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && rect.Contains(Event.current.mousePosition))
            {
                ShowEventContextMenu(day); // Menu de contexto com botão direito
                Event.current.Use();
            }

            if (Event.current.type == EventType.MouseDrag && draggedEvent != null)
            {
                draggedEvent.day = day;
                Event.current.Use();
            }

            EditorGUILayout.EndVertical();
            return clicked;
        }

        // Método para mostrar o menu de contexto ao clicar com o botão direito
        private void ShowEventContextMenu(int day)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Add Event"), false, () => CreateNewEvent(day));

            var dayEvents = events.FindAll(e => e.day == day && e.month == currentMonth && e.year == currentYear);
            foreach (var evt in dayEvents)
            {
                menu.AddItem(new GUIContent($"Edit/{evt.eventName}"), false, () => selectedEvent = evt);
                menu.AddItem(new GUIContent($"Delete/{evt.eventName}"), false, () => DeleteEvent(evt));
            }

            menu.ShowAsContext();
        }

        // Editor de eventos no lado direito
        private void DrawEventEditor()
        {
            if (selectedEvent != null)
            {
                GUILayout.Label("Event Editor", EditorStyles.boldLabel);
                selectedEvent.eventName = EditorGUILayout.TextField("Event Name", selectedEvent.eventName);
                selectedEvent.description = EditorGUILayout.TextArea(selectedEvent.description, GUILayout.Height(60));
                selectedEvent.hour = EditorGUILayout.IntSlider("Hour", selectedEvent.hour, 0, 23);
                selectedEvent.eventType = (EventCalendarType)EditorGUILayout.EnumPopup("Event Type", selectedEvent.eventType);
                // dfgsdg
                if (GUILayout.Button("Save Event"))
                {
                    EditorUtility.SetDirty(selectedEvent);
                    AssetDatabase.SaveAssets();
                }

                if (GUILayout.Button("Delete Event"))
                {
                    DeleteEvent(selectedEvent);
                }
            }
        }

        // Criação de novos eventos
        private void CreateNewEvent(int day)
        {
            GameEvent newEvent = CreateInstance<GameEvent>();
            newEvent.eventName = "New Event";
            newEvent.day = day;
            newEvent.month = currentMonth;
            newEvent.year = currentYear;

            string path = $"Assets/Resources/DataSO/GameEvents/Event_{currentYear}_{currentMonth}_{day}.asset";
            AssetDatabase.CreateAsset(newEvent, AssetDatabase.GenerateUniqueAssetPath(path));
            AssetDatabase.SaveAssets();

            events.Add(newEvent);
            selectedEvent = newEvent;
        }

        private void DeleteEvent(GameEvent eventToDelete)
        {
            events.Remove(eventToDelete);
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(eventToDelete));
            AssetDatabase.SaveAssets();
            if (selectedEvent == eventToDelete) selectedEvent = null;
        }

        private void NextMonth()
        {
            if (currentMonth == 12)
            {
                currentMonth = 1;
                currentYear++;
            }
            else currentMonth++;
        }

        private void PreviousMonth()
        {
            if (currentMonth == 1)
            {
                currentMonth = 12;
                currentYear--;
            }
            else currentMonth--;
        }

        private string GetMonthName(int month)
        {
            return System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
        }

        void OnEnable()
        {
            LoadEvents();
        }

        void LoadEvents()
        {
            events.Clear();
            string[] guids = AssetDatabase.FindAssets("t:GameEvent");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameEvent evt = AssetDatabase.LoadAssetAtPath<GameEvent>(path);
                if (evt != null) events.Add(evt);
            }
        }
    }
}
