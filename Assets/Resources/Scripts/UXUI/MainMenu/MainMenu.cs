using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private SceneControllerManager sceneController;

    private void Start()
    {
        sceneController = GetComponent<SceneControllerManager>();
    }

    public void NewGameStart(string sceneName)
    {
        sceneController.StartSceneLoad(sceneName, SceneControllerManager.SceneLoadMode.Sync, SceneControllerManager.SceneType.Normal);
        
    }

    public void ContinueGame()
    {

    }

    public void OpenMenuConfiguration()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
