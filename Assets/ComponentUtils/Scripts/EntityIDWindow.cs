using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ComponentUtils
{
    public class EntityIDWindow : EditorWindow
    {
        private List<ScriptableObject> entitiesWithId = new List<ScriptableObject>();
        private string filterText = "";
        private IdGroup? filterGroup;
        private bool ascendingOrder = true;
        private Vector2 scrollPosition;
        private string newGroupName = "";
        private GUIStyle buttonStyle;
        private GUIStyle headerStyle;
        private GUIStyle flexibleTextFieldStyle;

        [MenuItem("Window/Gerenciador de IDs")]
        public static void ShowWindow()
        {
            GetWindow<EntityIDWindow>("Gerenciador de IDs").LoadEntitiesWithID();
        }

        private void OnEnable()
        {
            LoadEntitiesWithID();
            EditorApplication.projectChanged += LoadEntitiesWithID;
            minSize = new Vector2(300, 200);
        }

        private void OnDisable()
        {
            EditorApplication.projectChanged -= LoadEntitiesWithID;
        }

        private void OnGUI()
        {
            InitializeStyles();

            EditorGUILayout.BeginVertical();
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            DrawTitle();
            DrawGroupManagement();
            DrawExistingGroups();
            DrawEntityListHeader();
            DrawFilterAndSort();
            DrawEntityList();

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            HandleRepaint();
        }

        private void InitializeStyles()
        {
            if (buttonStyle == null)
            {
                buttonStyle = new GUIStyle(GUI.skin.button);
                buttonStyle.padding = new RectOffset(4, 4, 2, 2);
            }

            if (headerStyle == null)
            {
                headerStyle = new GUIStyle(EditorStyles.boldLabel);
                headerStyle.fontSize = 12;
                headerStyle.margin = new RectOffset(0, 0, 10, 5);
            }

            if (flexibleTextFieldStyle == null)
            {
                flexibleTextFieldStyle = new GUIStyle(EditorStyles.textField);
                flexibleTextFieldStyle.stretchWidth = true;
            }
        }

        private void DrawTitle()
        {
            EditorGUILayout.LabelField("Gerenciador de IDs", headerStyle);
        }

        private void DrawFilterAndSort()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Filtrar por Grupo:", GUILayout.Width(80));
            filterGroup = (IdGroup)EditorGUILayout.EnumPopup(filterGroup ?? IdGroup.ALL);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(); 
            EditorGUILayout.LabelField("Pesquisar:", GUILayout.Width(80));
            filterText = EditorGUILayout.TextField(filterText, flexibleTextFieldStyle);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginHorizontal(GUILayout.Width(90)); 

            if (GUILayout.Button(ascendingOrder ? "↓ Ordenar" : "↑ Ordenar", buttonStyle))
            {
                ascendingOrder = !ascendingOrder;
                SortEntities();
            }

            if (GUILayout.Button("↻", buttonStyle, GUILayout.Width(25)))
            {
                LoadEntitiesWithID();
            }

            if (GUILayout.Button("✖", buttonStyle, GUILayout.Width(25)))
            {
                ID.CleanUnusedIds<ScriptableObject>();
                LoadEntitiesWithID();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndHorizontal();
        }

        private void DrawGroupManagement()
        {
            EditorGUILayout.Space(); 
            EditorGUILayout.LabelField("Gerenciar Grupos de ID", headerStyle);

            EditorGUILayout.BeginHorizontal(); 
            newGroupName = EditorGUILayout.TextField("Nome do Novo Grupo:", newGroupName, flexibleTextFieldStyle);

            if (GUILayout.Button("Criar", buttonStyle, GUILayout.Width(60)))
            {
                CreateNewGroup(newGroupName);
                newGroupName = "";
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawExistingGroups()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Grupos Existentes", headerStyle);

            foreach (IdGroup group in Enum.GetValues(typeof(IdGroup)))
            {
                if (group == IdGroup.ALL) continue; // Skip ALL as it's not a real group

                EditorGUILayout.BeginHorizontal();

                string groupName = group.ToString();
                int idCount = ID.GetIdCountForGroup(group);
                EditorGUILayout.LabelField($"{groupName}: {idCount} IDs", GUILayout.ExpandWidth(true));

                if (GUILayout.Button("Renomear", buttonStyle, GUILayout.Width(60)))
                {
                    string newName = EditorInputDialog.Show("Renomear Grupo", "Digite o novo nome:", groupName);
                    if (!string.IsNullOrEmpty(newName))
                    {
                        RenameGroup(group, newName);
                    }
                }

                if (GUILayout.Button("Remover", buttonStyle, GUILayout.Width(60)))
                {
                    if (EditorUtility.DisplayDialog("Remover Grupo", $"Você tem certeza que deseja remover o grupo {group}?", "Sim", "Não"))
                    {
                        RemoveGroup(group);
                    }
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawEntityListHeader()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Entidades com IDs", headerStyle);
        }

        private void DrawEntityList()
        {
            var filteredEntities = entitiesWithId.Where(entity => ShouldDisplayEntity(entity)).ToList();

            if (filteredEntities.Count == 0)
            {
                if (string.IsNullOrEmpty(filterText) && (filterGroup == null || filterGroup == IdGroup.ALL))
                {
                    EditorGUILayout.LabelField("Nenhuma entidade com IDs encontrada.", EditorStyles.boldLabel);
                }
                else
                {
                    EditorGUILayout.LabelField("Nenhuma entidade correspondente encontrada.", EditorStyles.boldLabel);
                }
                return;
            }

            foreach (var entity in filteredEntities)
            {
                if (entity != null)
                {
                    var idField = entity.GetType().GetField("Id", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (idField != null && idField.GetValue(entity) is ID idInstance)
                    {
                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button($"ID: {idInstance.Id} | {entity.name} | {idInstance.Group}", buttonStyle))
                        {
                            Selection.activeObject = entity;
                            EditorGUIUtility.PingObject(entity);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
        }

        private bool ShouldDisplayEntity(ScriptableObject entity)
        {
            var idField = entity.GetType().GetField("Id", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (idField != null && idField.GetValue(entity) is ID idInstance)
            {
                bool groupMatch = filterGroup == null || filterGroup == IdGroup.ALL || idInstance.Group == filterGroup;
                bool textMatch = string.IsNullOrEmpty(filterText) ||
                                 entity.name.Contains(filterText, StringComparison.OrdinalIgnoreCase) ||
                                 idInstance.Id.ToString().Contains(filterText);
                return groupMatch && textMatch;
            }
            return false;
        }

        private void CreateNewGroup(string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName) || Enum.IsDefined(typeof(IdGroup), groupName))
            {
                Debug.LogWarning("Nome de grupo inválido ou o grupo já existe.");
                return;
            }

            string enumFilePath = "Assets/ComponentUtils/Scripts/IdGroup.cs";
            string[] lines = File.ReadAllLines(enumFilePath);
            List<string> newLines = new List<string>();
            bool enumFound = false;

            foreach (string line in lines)
            {
                if (line.Trim().StartsWith("public enum IdGroup"))
                {
                    enumFound = true;
                    newLines.Add(line);
                }
                else if (enumFound && line.Trim() == "}")
                {
                    string formattedGroupName = FormatGroupName(groupName);
                    newLines.Add($"    {formattedGroupName},");
                    newLines.Add(line);
                    enumFound = false;
                }
                else
                {
                    newLines.Add(line);
                }
            }

            File.WriteAllLines(enumFilePath, newLines);
            AssetDatabase.Refresh();

            Debug.Log($"Grupo '{groupName}' criado e adicionado ao enum IdGroup.");
        }

        private void RenameGroup(IdGroup group, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName) || Enum.IsDefined(typeof(IdGroup), newName))
            {
                Debug.LogWarning("Nome de grupo inválido ou o grupo já existe.");
                return;
            }

            string enumFilePath = "Assets/ComponentUtils/Scripts/IdGroup.cs";
            string[] lines = File.ReadAllLines(enumFilePath);
            List<string> newLines = new List<string>();

            foreach (string line in lines)
            {
                if (line.Trim().StartsWith(group.ToString()))
                {
                    newLines.Add($"    {FormatGroupName(newName)},");
                }
                else
                {
                    newLines.Add(line);
                }
            }

            File.WriteAllLines(enumFilePath, newLines);
            AssetDatabase.Refresh();

            Debug.Log($"Grupo '{group}' renomeado para '{newName}'.");
        }

        private void RemoveGroup(IdGroup group)
        {
            string enumFilePath = "Assets/ComponentUtils/Scripts/IdGroup.cs";
            string[] lines = File.ReadAllLines(enumFilePath);
            List<string> newLines = new List<string>();
            bool removed = false;

            foreach (string line in lines)
            {
                if (!line.Trim().StartsWith(group.ToString() + ","))
                {
                    newLines.Add(line);
                }
                else
                {
                    removed = true;
                }
            }

            if (removed)
            {
                File.WriteAllLines(enumFilePath, newLines);
                AssetDatabase.Refresh();
                Debug.Log($"Grupo '{group}' removido do enum IdGroup.");
            }
            else
            {
                Debug.LogWarning($"Grupo '{group}' não encontrado no enum IdGroup.");
            }
        }

        private void LoadEntitiesWithID()
        {
            entitiesWithId.Clear();
            var allEntities = Resources.LoadAll<ScriptableObject>("");

            foreach (var entity in allEntities)
            {
                var fields = entity.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (var field in fields)
                {
                    if (field.GetCustomAttribute<AutoIDAttribute>() != null)
                    {
                        if (field.GetValue(entity) is ID idInstance)
                        {
                            entitiesWithId.Add(entity);
                            break;
                        }
                    }
                }
            }

            SortEntities();
        }

        private void SortEntities()
        {
            entitiesWithId = ascendingOrder
                ? entitiesWithId.OrderBy(e => GetEntityId(e)).ToList()
                : entitiesWithId.OrderByDescending(e => GetEntityId(e)).ToList();
        }

        private int GetEntityId(ScriptableObject entity)
        {
            var idField = entity.GetType().GetField("Id", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (idField != null && idField.GetValue(entity) is ID idInstance)
            {
                return idInstance.Id;
            }
            return 0;
        }

        private string FormatGroupName(string groupName)
        {
            return string.Concat(groupName.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToUpper();
        }

        private void HandleRepaint()
        {
            if (Event.current.type == EventType.MouseMove || Event.current.type == EventType.MouseDrag)
            {
                Repaint();
            }
        }
    }

    public class EditorInputDialog : EditorWindow
    {
        public static string Show(string title, string message, string defaultText = "")
        {
            EditorInputDialog window = CreateInstance<EditorInputDialog>();
            window.titleContent = new GUIContent(title);
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 100);
            window.ShowModalUtility();
            return window.result;
        }

        private string result;
        private string message;
        private string input;

        private void OnGUI()
        {
            EditorGUILayout.LabelField(message);
            input = EditorGUILayout.TextField(input);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Salvar"))
            {
                result = input;
                Close();
            }
            if (GUILayout.Button("Cancelar"))
            {
                result = null;
                Close();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}