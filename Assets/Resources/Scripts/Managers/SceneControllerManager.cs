using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControllerManager : MonoBehaviour
{
    public enum SceneType
    {
        Normal,
        Additive
    }

    public enum SceneLoadMode
    {
        Sync,
        Async
    }

    [Serializable]
    public class SceneLoadEvent : UnityEngine.Events.UnityEvent { }

    [Header("Scene Settings")]
    public string sceneName;
    public SceneType sceneType = SceneType.Normal;
    public SceneLoadMode sceneLoadMode = SceneLoadMode.Async;

    [Header("Fade Settings")]
    public Material dissolveMaterial;
    public float fadeDuration = 1.5f;

    [Header("Events")]
    public SceneLoadEvent OnSceneLoadStart;
    public SceneLoadEvent OnSceneLoadComplete;


    public void StartSceneLoad(string sceneName, SceneLoadMode mode, SceneType type)
    {
        // Trigger the event for scene load start
        OnSceneLoadStart?.Invoke();

        // Fade in before loading the scene
        LoadingShaderEvent.FadeIn(dissolveMaterial, fadeDuration, () =>
        {
            if (sceneLoadMode == SceneLoadMode.Async)
            {
                StartCoroutine(LoadSceneAsync(sceneName, type));
            }
            else
            {
                LoadSceneSync(sceneName,type);
            }
        });
    }

    private IEnumerator LoadSceneAsync(string sceneName, SceneType type)
    {
        AsyncOperation asyncOperation;

        if (sceneType == SceneType.Normal)
        {
            asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        }
        else
        {
            asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        // Fade out after loading the scene
        LoadingShaderEvent.FadeOut(dissolveMaterial, fadeDuration, () =>
        {
            // Trigger the event for scene load complete
            OnSceneLoadComplete?.Invoke();
        });
    }

    private void LoadSceneSync(string sceneName, SceneType sceneType)
    {
        if (sceneType == SceneType.Normal)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

        // Fade out after loading the scene
        LoadingShaderEvent.FadeOut(dissolveMaterial, fadeDuration, () =>
        {
            // Trigger the event for scene load complete
            OnSceneLoadComplete?.Invoke();
        });
    }
}
