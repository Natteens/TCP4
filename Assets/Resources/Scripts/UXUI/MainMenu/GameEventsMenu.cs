using System;
using UnityEngine;

public class GameEventsMenu : MonoBehaviour
{
    public static event Action OnEnableInput;
    public static event Action OnClickForPlayGame;

    public static void EnableInput()
    {
        if (OnEnableInput != null)
        {
            OnEnableInput.Invoke();
        }
    }

    public static void ClickToPlay()
    {
        if (OnClickForPlayGame != null)
        {
            OnClickForPlayGame.Invoke();
        }
    }
}