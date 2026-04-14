using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LeaderboardEntry
{
    public string playerName;
    public int score;
}

[Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
}

public class LeaderBoard : MonoBehaviour
{
    private const string SaveKey = "Top5Leaderboard";
    public LeaderboardData data = new LeaderboardData();

    private void Awake()
    {
        LoadLeaderboard();
    }

    public void AddScore(string playerName, int score)
    {
        LeaderboardEntry newEntry = new LeaderboardEntry
        {
            playerName = playerName,
            score = score
        };

        data.entries.Add(newEntry);
        data.entries.Sort((a, b) => b.score.CompareTo(a.score));

        if (data.entries.Count > 5)
        {
            data.entries.RemoveRange(5, data.entries.Count - 5);
        }

        SaveLeaderboard();
    }

    public List<LeaderboardEntry> GetEntries()
    {
        return data.entries;
    }

    public void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public void LoadLeaderboard()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey);
            data = JsonUtility.FromJson<LeaderboardData>(json);
        }
        else
        {
            data = new LeaderboardData();
        }
    }
    public void ClearLeaderboard()
    {
        PlayerPrefs.DeleteKey("Top5Leaderboard");
        data = new LeaderboardData();
    }

    public bool IsTop5Score(int score)
    {
        if (data.entries.Count < 5)
            return true;

        return score > data.entries[data.entries.Count - 1].score;
    }
}