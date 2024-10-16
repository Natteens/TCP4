using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.Animations;
using System.Collections.Generic;
using System.Linq;

namespace Tcp4
{
    //public class EntitySetupWindow : EditorWindow
    //{
    //    private string entityName = "NewEntity";
    //    private string basePrefabPath = "Assets/Resources/Prefabs/Characters";
    //    private string scriptFolderPath = "Assets/Resources/Scripts/Entities";
    //    private string animFolderPath = "Assets/Resources/Art/Animations";

    //    private GameObject entityPrefab;
    //    private GameObject createdEntity;
    //    private MonoScript customScript;
    //    private bool includeDefaultClips = true;
    //    private List<string> additionalAnimationClips = new List<string>();
    //    private Vector2 scrollPosition;

    //    private int selectedTab = 0;
    //    private string[] tabNames = { "Setup", "Components", "Templates", "Batch Creation" };

    //    private List<EntityTemplate> entityTemplates = new List<EntityTemplate>();
    //    private int selectedTemplateIndex = -1;

    //    private List<string> batchEntityNames = new List<string>();

    //    private Texture2D previewTexture;

    //    [MenuItem("Window/Improved Entity Creation Setup")]
    //    public static void ShowWindow()
    //    {
    //        GetWindow<EntitySetupWindow>("Entity Setup").Show();
    //    }

    //    private void OnEnable()
    //    {
    //        LoadEntityTemplates();
    //    }

    //    private void OnGUI()
    //    {
    //        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

    //        DrawHeader();

    //        selectedTab = GUILayout.Toolbar(selectedTab, tabNames);

    //        EditorGUILayout.Space(10);

    //        switch (selectedTab)
    //        {
    //            case 0:
    //                DrawSetupTab();
    //                break;
    //            case 1:
    //                DrawComponentsTab();
    //                break;
    //            case 2:
    //                DrawTemplatesTab();
    //                break;
    //            case 3:
    //                DrawBatchCreationTab();
    //                break;
    //        }

    //        EditorGUILayout.EndScrollView();
    //    }

    //    private void DrawHeader()
    //    {
    //        EditorGUILayout.BeginHorizontal();
    //        GUILayout.FlexibleSpace();
    //        GUILayout.Label("Entity Creation Setup", EditorStyles.boldLabel);
    //        GUILayout.FlexibleSpace();
    //        EditorGUILayout.EndHorizontal();

    //        EditorGUILayout.Space(10);
    //    }

    //    private void DrawSetupTab()
    //    {
    //        entityName = EditorGUILayout.TextField(new GUIContent("Entity Name", "Enter the name for your new entity"), entityName);

    //        DrawFolderSelection("Prefab Folder", ref basePrefabPath);
    //        DrawFolderSelection("Script Folder", ref scriptFolderPath);
    //        DrawFolderSelection("Animation Folder", ref animFolderPath);

    //        EditorGUILayout.Space(10);

    //        customScript = (MonoScript)EditorGUILayout.ObjectField(new GUIContent("Custom Script", "Assign a custom script (optional)"), customScript, typeof(MonoScript), false);

    //        includeDefaultClips = EditorGUILayout.Toggle(new GUIContent("Include Default Animations", "Add Idle, Run, and Attack animations"), includeDefaultClips);

    //        EditorGUILayout.Space(10);

    //        DrawAnimationClipList();

    //        if (GUILayout.Button("Create Entity"))
    //        {
    //            CreateEntityPrefab();
    //        }

    //        if (createdEntity != null)
    //        {
    //            DrawEntityPreview();
    //        }
    //    }

    //    private void DrawComponentsTab()
    //    {
    //        if (createdEntity == null)
    //        {
    //            EditorGUILayout.HelpBox("Create an entity first to manage components.", MessageType.Info);
    //            return;
    //        }

    //        EditorGUILayout.LabelField("Entity Components", EditorStyles.boldLabel);

    //        Component[] components = createdEntity.GetComponents<Component>();
    //        foreach (var component in components)
    //        {
    //            EditorGUILayout.BeginHorizontal();
    //            EditorGUILayout.LabelField(component.GetType().Name);
    //            if (GUILayout.Button("Remove", GUILayout.Width(60)))
    //            {
    //                DestroyImmediate(component);
    //            }
    //            EditorGUILayout.EndHorizontal();
    //        }

    //        EditorGUILayout.Space(10);

    //        if (GUILayout.Button("Add Component"))
    //        {
    //            GenericMenu menu = new GenericMenu();
    //            menu.AddItem(new GUIContent("Rigidbody"), false, () => AddComponent<Rigidbody>());
    //            menu.AddItem(new GUIContent("BoxCollider"), false, () => AddComponent<BoxCollider>());
    //            menu.AddItem(new GUIContent("SphereCollider"), false, () => AddComponent<SphereCollider>());
    //            menu.AddItem(new GUIContent("CapsuleCollider"), false, () => AddComponent<CapsuleCollider>());
    //            menu.AddItem(new GUIContent("AudioSource"), false, () => AddComponent<AudioSource>());
    //            menu.ShowAsContext();
    //        }
    //    }

    //    private void DrawTemplatesTab()
    //    {
    //        EditorGUILayout.LabelField("Entity Templates", EditorStyles.boldLabel);

    //        for (int i = 0; i < entityTemplates.Count; i++)
    //        {
    //            EditorGUILayout.BeginHorizontal();
    //            if (GUILayout.Button(entityTemplates[i].name))
    //            {
    //                ApplyTemplate(entityTemplates[i]);
    //            }
    //            if (GUILayout.Button("X", GUILayout.Width(20)))
    //            {
    //                entityTemplates.RemoveAt(i);
    //                SaveEntityTemplates();
    //                i--;
    //            }
    //            EditorGUILayout.EndHorizontal();
    //        }

    //        EditorGUILayout.Space(10);

    //        if (GUILayout.Button("Save Current as Template"))
    //        {
    //            SaveCurrentAsTemplate();
    //        }
    //    }

    //    private void DrawBatchCreationTab()
    //    {
    //        EditorGUILayout.LabelField("Batch Entity Creation", EditorStyles.boldLabel);

    //        for (int i = 0; i < batchEntityNames.Count; i++)
    //        {
    //            EditorGUILayout.BeginHorizontal();
    //            batchEntityNames[i] = EditorGUILayout.TextField($"Entity {i + 1}", batchEntityNames[i]);
    //            if (GUILayout.Button("X", GUILayout.Width(20)))
    //            {
    //                batchEntityNames.RemoveAt(i);
    //                i--;
    //            }
    //            EditorGUILayout.EndHorizontal();
    //        }

    //        if (GUILayout.Button("Add Entity"))
    //        {
    //            batchEntityNames.Add("");
    //        }

    //        EditorGUILayout.Space(10);

    //        if (GUILayout.Button("Create All Entities"))
    //        {
    //            CreateBatchEntities();
    //        }
    //    }

    //    private void DrawFolderSelection(string label, ref string path)
    //    {
    //        EditorGUILayout.BeginHorizontal();
    //        EditorGUILayout.LabelField(label, GUILayout.Width(100));
    //        EditorGUILayout.LabelField(path, EditorStyles.textField);
    //        if (GUILayout.Button("...", GUILayout.Width(30)))
    //        {
    //            string newPath = EditorUtility.OpenFolderPanel($"Select {label}", "Assets", "");
    //            if (!string.IsNullOrEmpty(newPath))
    //            {
    //                path = newPath.Replace(Application.dataPath, "Assets");
    //            }
    //        }
    //        EditorGUILayout.EndHorizontal();
    //    }

    //    private void DrawAnimationClipList()
    //    {
    //        EditorGUILayout.LabelField("Custom Animation Clips", EditorStyles.boldLabel);

    //        for (int i = 0; i < additionalAnimationClips.Count; i++)
    //        {
    //            EditorGUILayout.BeginHorizontal();
    //            additionalAnimationClips[i] = EditorGUILayout.TextField($"Clip {i + 1}", additionalAnimationClips[i]);
    //            if (GUILayout.Button("X", GUILayout.Width(20)))
    //            {
    //                additionalAnimationClips.RemoveAt(i);
    //                i--;
    //            }
    //            EditorGUILayout.EndHorizontal();
    //        }

    //        if (GUILayout.Button("Add Animation Clip"))
    //        {
    //            additionalAnimationClips.Add("");
    //        }
    //    }

    //    private void DrawEntityPreview()
    //    {
    //        EditorGUILayout.Space(10);
    //        EditorGUILayout.LabelField("Entity Preview", EditorStyles.boldLabel);

    //        if (previewTexture == null)
    //        {
    //            previewTexture = AssetPreview.GetAssetPreview(createdEntity);
    //        }

    //        if (previewTexture != null)
    //        {
    //            GUILayout.Label(previewTexture, GUILayout.Width(200), GUILayout.Height(200));
    //        }
    //        else
    //        {
    //            EditorGUILayout.LabelField("Preview not available");
    //        }
    //    }

    //    private void CreateEntityPrefab()
    //    {
    //        createdEntity = new GameObject(entityName);

    //        AddComponent<Rigidbody>();
    //        AddComponent<Animator>();
    //        AddComponent<SpriteRenderer>();
    //        AddComponent<StatusComponent>();

    //        if (customScript != null)
    //        {
    //            createdEntity.AddComponent(customScript.GetClass());
    //        }

    //        SetupAnimator();
    //        SavePrefab();

    //        previewTexture = null; // Reset preview texture
    //    }

    //    private void AddComponent<T>() where T : Component
    //    {
    //        if (createdEntity != null && createdEntity.GetComponent<T>() == null)
    //        {
    //            createdEntity.AddComponent<T>();
    //            Debug.Log($"{typeof(T).Name} added to entity.");
    //        }
    //    }

    //    private void SetupAnimator()
    //    {
    //        string animationsFolder = $"{animFolderPath}/{entityName}/Animations";
    //        if (!Directory.Exists(animationsFolder))
    //        {
    //            Directory.CreateDirectory(animationsFolder);
    //        }

    //        string animatorControllerPath = $"{animationsFolder}/{entityName}AnimController.controller";
    //        AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(animatorControllerPath);

    //        if (includeDefaultClips)
    //        {
    //            CreateAnimationClip("Idle", animationsFolder, animatorController);
    //            CreateAnimationClip("Run", animationsFolder, animatorController);
    //            CreateAnimationClip("Attack", animationsFolder, animatorController);
    //        }

    //        foreach (string clipName in additionalAnimationClips)
    //        {
    //            if (!string.IsNullOrWhiteSpace(clipName))
    //            {
    //                CreateAnimationClip(clipName, animationsFolder, animatorController);
    //            }
    //        }

    //        createdEntity.GetComponent<Animator>().runtimeAnimatorController = animatorController;
    //        Debug.Log($"{entityName} Animator setup complete.");
    //    }

    //    private void CreateAnimationClip(string clipName, string folderPath, AnimatorController controller)
    //    {
    //        AnimationClip newClip = new AnimationClip();
    //        AssetDatabase.CreateAsset(newClip, $"{folderPath}/{entityName}_{clipName}.anim");
    //        controller.AddMotion(newClip);

    //        Debug.Log($"Animation clip {clipName} created for {entityName}.");
    //    }

    //    private void SavePrefab()
    //    {
    //        string entityFolder = $"{basePrefabPath}/{entityName}";
    //        if (!Directory.Exists(entityFolder))
    //        {
    //            Directory.CreateDirectory(entityFolder);
    //        }

    //        string prefabPath = $"{entityFolder}/{entityName}.prefab";
    //        entityPrefab = PrefabUtility.SaveAsPrefabAsset(createdEntity, prefabPath);

    //        Debug.Log($"{entityName} prefab saved at {prefabPath}");
    //    }

    //    private void SaveCurrentAsTemplate()
    //    {
    //        if (createdEntity == null)
    //        {
    //            EditorUtility.DisplayDialog("Error", "Create an entity first before saving as a template.", "OK");
    //            return;
    //        }

    //        string templateName = EditorUtility.SaveFilePanel("Save Entity Template", "", entityName, "json");
    //        if (string.IsNullOrEmpty(templateName)) return;

    //        EntityTemplate template = new EntityTemplate
    //        {
    //            name = Path.GetFileNameWithoutExtension(templateName),
    //            components = createdEntity.GetComponents<Component>().Select(c => c.GetType().FullName).ToList(),
    //            includeDefaultClips = includeDefaultClips,
    //            additionalAnimationClips = new List<string>(additionalAnimationClips)
    //        };

    //        string json = JsonUtility.ToJson(template, true);
    //        File.WriteAllText(templateName, json);

    //        LoadEntityTemplates();
    //    }

    //    private void LoadEntityTemplates()
    //    {
    //        entityTemplates.Clear();
    //        string[] templateFiles = Directory.GetFiles(Application.dataPath, "*.json", SearchOption.AllDirectories);

    //        foreach (string file in templateFiles)
    //        {
    //            string json = File.ReadAllText(file);
    //            EntityTemplate template = JsonUtility.FromJson<EntityTemplate>(json);
    //            if (template != null)
    //            {
    //                entityTemplates.Add(template);
    //            }
    //        }
    //    }

    //    private void ApplyTemplate(EntityTemplate template)
    //    {
    //        entityName = template.name;
    //        includeDefaultClips = template.includeDefaultClips;
    //        additionalAnimationClips = new List<string>(template.additionalAnimationClips);

    //        CreateEntityPrefab();

    //        foreach (string componentName in template.components)
    //        {
    //            System.Type componentType = System.Type.GetType(componentName);
    //            if (componentType != null && createdEntity.GetComponent(componentType) == null)
    //            {
    //                createdEntity.AddComponent(componentType);
    //            }
    //        }

    //        SavePrefab();
    //    }

    //    private void CreateBatchEntities()
    //    {
    //        foreach (string name in batchEntityNames)
    //        {
    //            if (!string.IsNullOrEmpty(name))
    //            {
    //                entityName = name;
    //                CreateEntityPrefab();
    //            }
    //        }

    //        batchEntityNames.Clear();
    //        EditorUtility.DisplayDialog("Batch Creation", "All entities have been created successfully.", "OK");
    //    }

    //    private void SaveEntityTemplates()
    //    {
    //        for (int i = 0; i < entityTemplates.Count; i++)
    //        {
    //            string json = JsonUtility.ToJson(entityTemplates[i], true);
    //            File.WriteAllText($"{Application.dataPath}/EntityTemplate_{i}.json", json);
    //        }

    //        AssetDatabase.Refresh();
    //    }
    //}
    //[System.Serializable]
    //public class EntityTemplate
    //{
    //    public string name;
    //    public List<string> components = new List<string>();
    //    public bool includeDefaultClips;
    //    public List<string> additionalAnimationClips = new List<string>();
    //}
}