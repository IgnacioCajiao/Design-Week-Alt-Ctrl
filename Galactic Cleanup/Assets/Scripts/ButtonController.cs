using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("SampleScene"); // Replace "SampleScene" with the exact name of your scene
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }
}
