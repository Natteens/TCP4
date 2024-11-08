using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Threading;
using System.Threading.Tasks;
using System;
using GDX.Collections.Generic;

namespace Tcp4
{
    public class PerformanceManager : MonoBehaviour
    {
        #region Singleton
        public static PerformanceManager Instance { get; private set; }
        private CancellationTokenSource cancellationTokenSource;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                cancellationTokenSource = new CancellationTokenSource();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        #endregion

        [Header("Debug UI")]
        [SerializeField] private bool showDebugUI = true;
        [SerializeField] private TextMeshProUGUI fpsText;
        [SerializeField] private TextMeshProUGUI memoryText;
        [SerializeField] private TextMeshProUGUI settingsText;

        [Header("Performance Settings")]
        [SerializeField, Range(1, 560)] private int targetFrameRate = 60;
        [SerializeField, Range(0.05f, 1f)] private float updateInterval = 0.1f;
        [SerializeField, Range(50f, 300f)] private float renderDistance = 100f;

        [Header("Graphics Settings")]
        [SerializeField] private bool useDynamicResolution = true;
        [SerializeField, Range(0.1f, 1f)] private float minResolutionScale = 0.5f;
        [SerializeField, Range(0.1f, 1f)] private float maxResolutionScale = 1f;
        [SerializeField, Range(0, 3)] private int textureQuality = 1;
        [SerializeField, Range(0f, 150f)] private float shadowDistance = 50f;

        [Header("Mobile Specific")]
        [SerializeField] private bool optimizeForMobile = true;
        [SerializeField, Range(1, 200)] private int mobileFPSTarget = 30;
        [SerializeField] private bool disableShadowsOnMobile = true;

        [Header("Object Pooling")]
        [SerializeField] private bool useObjectPooling = true;
        [SerializeField, Range(10, 100)] private int defaultPoolSize = 20;

        // Performance monitoring
        private readonly Queue<float> fpsQueue = new Queue<float>(30);
        private float lastFPSUpdate;
        private const float UPDATE_FPS_INTERVAL = 0.5f;
        private const int MAX_FPS_SAMPLES = 30;

        // Component caching
        [field : SerializeField]private readonly SerializableDictionary<int, WeakReference<Component>> componentCache = new SerializableDictionary<int, WeakReference<Component>>();
        private Camera mainCamera;
        private bool isApplicationQuitting;

        private async void Start()
        {
            InitializeSettings();

            if (showDebugUI)
            {
                CreateDebugUI();
            }

            try
            {
                await StartPerformanceMonitoring();
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Performance monitoring cancelled.");
            }
        }

        private void InitializeSettings()
        {
            mainCamera = Camera.main;
            SetupGraphicsSettings();
            SetupMobileSettings();
            SetupDynamicResolution();
        }

        private void SetupGraphicsSettings()
        {
            Application.targetFrameRate = GetTargetFrameRate();
            QualitySettings.vSyncCount = 0;
            QualitySettings.globalTextureMipmapLimit = textureQuality;
            QualitySettings.shadowDistance = shadowDistance;
        }

        private int GetTargetFrameRate() =>
            Application.isMobilePlatform && optimizeForMobile ? mobileFPSTarget : targetFrameRate;

        private void SetupMobileSettings()
        {
            if (!Application.isMobilePlatform || !optimizeForMobile) return;

            QualitySettings.shadows = disableShadowsOnMobile ? ShadowQuality.Disable : ShadowQuality.HardOnly;
            QualitySettings.antiAliasing = 0;
            QualitySettings.realtimeReflectionProbes = false;
            QualitySettings.skinWeights = SkinWeights.TwoBones;
        }

        private void SetupDynamicResolution()
        {
#if UNITY_2019_3_OR_NEWER
            if (useDynamicResolution)
            {
                ScalableBufferManager.ResizeBuffers(minResolutionScale, maxResolutionScale);
            }
#endif
        }

        private async Task StartPerformanceMonitoring()
        {
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(updateInterval), cancellationTokenSource.Token);

                if (isApplicationQuitting) break;

                try
                {
                    await UnityMainThread.ExecuteInUpdate(() =>
                    {
                        UpdatePerformanceStats();
                    });
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error updating performance stats: {e}");
                }
            }
        }

        private void UpdatePerformanceStats()
        {
            UpdateFPS();
            UpdateMemoryStats();
            UpdateSettingsInfo();
            CleanupComponentCache();
        }

        private void UpdateFPS()
        {
            if (Time.unscaledTime - lastFPSUpdate < UPDATE_FPS_INTERVAL) return;

            float currentFPS = 1f / Time.unscaledDeltaTime;

            if (fpsQueue.Count >= MAX_FPS_SAMPLES)
            {
                fpsQueue.Dequeue();
            }
            fpsQueue.Enqueue(currentFPS);

            float averageFPS = CalculateAverageFPS();
            UpdateFPSDisplay(averageFPS);

            lastFPSUpdate = Time.unscaledTime;
        }

        private float CalculateAverageFPS()
        {
            float sum = 0f;
            foreach (float fps in fpsQueue)
            {
                sum += fps;
            }
            return sum / fpsQueue.Count;
        }

        private void UpdateFPSDisplay(float averageFPS)
        {
            if (fpsText == null) return;

            string colorHex = GetFPSColorHex(averageFPS);
            fpsText.text = $"FPS: <color={colorHex}>{averageFPS:F1}</color>";
        }

        private string GetFPSColorHex(float fps) =>
            fps >= targetFrameRate ? "#00FF00" :
            fps >= targetFrameRate * 0.7f ? "#FFFF00" : "#FF0000";

        private void UpdateMemoryStats()
        {
            if (memoryText == null) return;

            float totalMemoryMB = GC.GetTotalMemory(false) / (1024f * 1024f);
            memoryText.text = $"Memory: {totalMemoryMB:F1}MB";
        }

        private void UpdateSettingsInfo()
        {
            if (settingsText == null) return;

            settingsText.text = $"Settings:\n" +
                              $"Resolution: {Screen.currentResolution.width}x{Screen.currentResolution.height}\n" +
                              $"Quality: {QualitySettings.GetQualityLevel()}\n" +
                              $"Shadows: {QualitySettings.shadowDistance}m\n" +
                              $"Textures: {QualitySettings.globalTextureMipmapLimit}";
        }

        private void CleanupComponentCache()
        {
            var keysToRemove = new List<int>();

            foreach (var kvp in componentCache)
            {
                if (!kvp.Value.TryGetTarget(out _))
                {
                    keysToRemove.Add(kvp.Key);
                }
            }

            foreach (int key in keysToRemove)
            {
                componentCache.Remove(key);
            }
        }

        public T GetCachedComponent<T>(GameObject obj) where T : Component
        {
            if (obj == null) return null;

            int id = obj.GetInstanceID();

            if (componentCache.TryGetValue(id, out WeakReference<Component> weakRef))
            {
                if (weakRef.TryGetTarget(out Component cached) && cached is T typedComponent)
                {
                    return typedComponent;
                }
            }

            T component = obj.GetComponent<T>();
            if (component != null)
            {
                componentCache[id] = new WeakReference<Component>(component);
            }

            return component;
        }

        public bool IsInRenderDistance(Vector3 position)
        {
            if (mainCamera == null) return false;
            return Vector3.Distance(mainCamera.transform.position, position) <= renderDistance;
        }

        private void CreateDebugUI()
        {
            if (fpsText == null || memoryText == null || settingsText == null)
            {
                Debug.LogWarning("Debug UI components not assigned.");
                return;
            }

            fpsText.gameObject.SetActive(true);
            memoryText.gameObject.SetActive(true);
            settingsText.gameObject.SetActive(true);
        }

        private void OnApplicationQuit()
        {
            isApplicationQuitting = true;
            cancellationTokenSource?.Cancel();
        }

        private void OnDestroy()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            componentCache.Clear();
        }
    }
    public static class UnityMainThread
    {
        private static readonly TaskScheduler unityScheduler;

        static UnityMainThread()
        {
            unityScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }

        public static Task ExecuteInUpdate(Action action)
        {
            return Task.Factory.StartNew(action, CancellationToken.None,
                TaskCreationOptions.None, unityScheduler);
        }
    }
}