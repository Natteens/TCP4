using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tcp4
{
    public class InitialMenu : MonoBehaviour
    {
        public GameObject settingsPanel;
        public GameObject creditsPanel;
        public GameObject loadingScreen;
        public Slider loadingBar;

        private void Start()
        {
            settingsPanel.SetActive(false);
            creditsPanel.SetActive(false);
            loadingScreen.SetActive(false);
        }

        public void StartGame(int sceneId)
        {
            StartCoroutine(LoadSceneAsync(sceneId));
        }

        IEnumerator LoadSceneAsync(int sceneId)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
            operation.allowSceneActivation = false;
            loadingScreen.SetActive(true);
            loadingBar.value = 0;
            float progress = 0;

            while (progress < 1f)
            {
                progress += Time.deltaTime * 0.5f;
                loadingBar.value = progress;

                yield return null;
            }

            operation.allowSceneActivation = true;
        }

        public void OpenSettings()
        {
            settingsPanel.SetActive(true);
        }

        public void CloseSettings()
        {
            settingsPanel.SetActive(false);
        }

        public void OpenCredits()
        {
            creditsPanel.SetActive(true);
        }

        public void CloseCredits()
        {
            creditsPanel.SetActive(false);
        }

        public void QuitGame()
        {
            Application.Quit();

            // p ver no editor
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}