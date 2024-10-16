using System.Collections;
using UnityEngine;
//using Steamworks;
using System.Collections.Generic;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    //public void ResetAchievement()
    //{
    //    if (!SteamManager.Initialized)
    //        return;
    //    SteamUserStats.ResetAllStats(true);
    //    SteamUserStats.RequestCurrentStats();
    //}

    //public void UnlockAchievement(string id)
    //{
    //    if (!SteamManager.Initialized)
    //        return;
    //    SteamUserStats.SetAchievement(id);
    //    SteamUserStats.StoreStats();
    //}

    //public void SetStat(string id, int value)
    //{
    //    if (!SteamManager.Initialized)
    //        return;
    //    SteamUserStats.SetStat(id, value);
    //    SteamUserStats.StoreStats();
    //}
}
