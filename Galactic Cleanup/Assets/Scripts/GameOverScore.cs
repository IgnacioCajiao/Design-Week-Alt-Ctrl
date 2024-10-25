using UnityEngine;
using TMPro;

public class GameOverDisplay : MonoBehaviour
{
    public TMP_Text finalScoreText;

    void Start()
    {
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0); 
        finalScoreText.text = "Final Score: " + finalScore.ToString();
    }
}
