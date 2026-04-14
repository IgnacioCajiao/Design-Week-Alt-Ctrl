using UnityEngine;
using TMPro;

public class GameOverScore : MonoBehaviour
{
    public TMP_Text finalScoreText;
    public TMP_Text leaderboardText;
    public TMP_InputField nameInput;
    public GameObject submitPanel;
    public LeaderBoard leaderboard;

    private int finalScore;
    private bool hasSubmitted = false;

    void Start()
    {
        finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        finalScoreText.text = "Final Score: " + finalScore;

        ShowLeaderboard();

        bool qualifies = leaderboard.IsTop5Score(finalScore);
        submitPanel.SetActive(qualifies);

        if (qualifies)
        {
            nameInput.text = "";
            nameInput.ActivateInputField();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Period))
        {
            leaderboard.ClearLeaderboard();
            ShowLeaderboard();
            submitPanel.SetActive(true);
        }
    }

    public void SubmitScore()
    {
        if (hasSubmitted)
            return;

        string playerName = nameInput.text.Trim();

        if (string.IsNullOrEmpty(playerName))
            playerName = "AAA";

        leaderboard.AddScore(playerName, finalScore);
        ShowLeaderboard();

        hasSubmitted = true;
        submitPanel.SetActive(false);
    }

    void ShowLeaderboard()
    {
        var entries = leaderboard.GetEntries();
        leaderboardText.text = "TOP 5\n";

        for (int i = 0; i < 5; i++)
        {
            if (i < entries.Count)
                leaderboardText.text += $"{i + 1}. {entries[i].playerName} - {entries[i].score}\n";
            else
                leaderboardText.text += $"{i + 1}. --- - 0\n";
        }
    }
}