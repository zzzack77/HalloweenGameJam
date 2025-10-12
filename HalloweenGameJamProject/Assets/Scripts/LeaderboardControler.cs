using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Dan.Main;
using NUnit.Framework;

public class LeaderboardControler : MonoBehaviour
{
    public List<TextMeshProUGUI> names;
    public List<TextMeshProUGUI> scores;

    private string publicKey = "8015fc7098126da68db4e6c17dfa6c23c1e492bed89323b6ba5c2f3ba24e5fcf";

    private void Start()
    {
        LoadEntries();
    }

    public void LoadEntries()
    {
        Leaderboards.HalloweenGameJamLeaderboard.GetEntries(entries =>
        {
            foreach (TextMeshProUGUI name in names) 
            {
                name.text = "";
            }
            foreach (var score in scores)
            {
                score.text = "";
            }

            float length = Mathf.Min(names.Count, entries.Length);
            for (int i = 0; i < length; i++)
            {
                names[i].text = entries[i].Username;
                scores[i].text = entries[i].Score.ToString();

            }

        });
    }
    public void SetEntry(string username, int score)
    {


        Leaderboards.HalloweenGameJamLeaderboard.UploadNewEntry(username, score, isSuccessful =>
        {
            if (isSuccessful)
            {
                LoadEntries();
            }
        }

        );
    }

}
