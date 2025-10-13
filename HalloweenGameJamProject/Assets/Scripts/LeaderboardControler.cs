using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Dan.Main;
using Dan.Models;
using NUnit.Framework;
using System;

public class LeaderboardControler : MonoBehaviour
{
    private MainMenuScript mainMenuScript;

    private Entry[] leaderboardEntries;
    private bool entriesLoaded = false;

    private void Start()
    {
        mainMenuScript = FindFirstObjectByType<MainMenuScript>();
        LoadEntries();
    }

    private void LoadEntries()
    {
        Leaderboards.HalloweenGameJamLeaderboard.GetEntries(OnEntriesLoaded, OnError);
    }

    private void OnEntriesLoaded(Entry[] entries)
    {
        leaderboardEntries = entries;
        entriesLoaded = true;

        foreach (Entry entry in entries)
        {
            Debug.Log($"{entry.Username}: {entry.Score}");
        }
        mainMenuScript.LoadLeaderboardUI();
    }
    public Entry[] GetLeaderboardEntries()
    {
        if (!entriesLoaded)
        {
            Debug.LogWarning("Leaderboard data not loaded yet!");
            return null;
        }
        return leaderboardEntries;
    }
    private void OnError(string error)
    {
        Debug.LogError(error);
    }
}
