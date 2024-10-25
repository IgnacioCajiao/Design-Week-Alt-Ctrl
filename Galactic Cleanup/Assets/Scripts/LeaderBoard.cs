using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    // This is where we will display the leaderboard in the UI.
    public TextMeshProUGUI leaderboardText;
    // List to hold player scores and their names.
    private List<PlayerData> playerScores = new List<PlayerData>();
    // The current player's score (i should replace this with the actual score from the game).
    public int currentPlayerScore; // For example, distance traveled in your endless runner
    public string currentPlayerName; // For example, "Player 1", "Player 2", etc.

    void Start()
    {
        // When the game ends, call this function to add the current player's score and update the leaderboard.
        AddPlayerScore(currentPlayerName, currentPlayerScore);
    }

    // This function adds the player's score to the list and updates the leaderboard.
    public void AddPlayerScore(string playerName, int score)
    {
        // Add the new player's score to the list.
        playerScores.Add(new PlayerData(playerName, score));

        // Sort the list by score in descending order (highest score first).
        for (int i = playerScores.Count - 1; i > 0; i--)
        {
            if (playerScores[i].score > playerScores[i - 1].score)
            {
                var temp = playerScores[i];
                playerScores[i] = playerScores[i - 1];
                playerScores[i - 1] = temp;
            }
        }

        // If there are more than 5 players, we keep only the top 5.
        if (playerScores.Count > 5)
        {
            playerScores.RemoveAt(5); // Remove the 6th place player if there are more than 5 players.
        }

        DisplayLeaderboard();
    }

    // This function displays the top 5 players and the current player's rank if they aren't in the top 5.
    void DisplayLeaderboard()
    {
        // Clear the leaderboard text.
        leaderboardText.text = "Top\n\n";
        // Display the top 5 players.
        for (int i = 0; i < Mathf.Min(5, playerScores.Count); i++)
        {
            leaderboardText.text += (i + 1) + ". " + playerScores[i].playerName + " - " + playerScores[i].score + "\n";
        }

        // Find the rank of the current player.
        int playerRank = playerScores.FindIndex(p => p.playerName == currentPlayerName) + 1;
        // If the current player is outside the top 5, show their rank separately.
        if (playerRank > 5)
        {
            leaderboardText.text += "\nYour Rank: " + playerRank + " - Score: " + currentPlayerScore;
        }
    }
}

// This class holds the player's name and score.
public class PlayerData
{
    public string playerName;
    public int score;
    // Constructor to create a new player data entry.
    public PlayerData(string name, int score)
    {
        this.playerName = name;
        this.score = score;
    }
}


