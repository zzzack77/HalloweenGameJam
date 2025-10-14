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
     
    private Entry[] leaderboardEntries;
    private bool entriesLoaded = false;

    private void Start()
    {
        LoadEntries();

        //SetEntry("asd5", 56);
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
            //Debug.Log($"{entry.Username}: {entry.Score}");
        }
        
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

    public void SetEntry(string username, int score)
    {
        
        Leaderboards.HalloweenGameJamLeaderboard.UploadNewEntry(username, score, isSuccessful =>
        {
            if (isSuccessful)
            {
                Debug.Log(username + " " +  score);
                LoadEntries();
            }
            else {
                Debug.Log("Didnt submit entry");
            }
        });
    }
    private void OnError(string error)
    {
        Debug.LogError(error);
    }
}
