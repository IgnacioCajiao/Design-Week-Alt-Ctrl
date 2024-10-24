using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI welcomeText;

    // Keeps track of how many players have played(starts at 1).
    private static int playerCount = 1;

    void Start()
    {
        //When the game starts, display "Welcome Player X" on the main menu.
        welcomeText.text = "Welcome Player " + playerCount;

        // Increases the player count so the next player gets the next number (Player 2, Player 3, etc.).
        playerCount++;
    }
}
