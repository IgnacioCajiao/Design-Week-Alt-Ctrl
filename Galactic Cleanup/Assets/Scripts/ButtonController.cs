using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("How to Play"); 
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }

    public void ContinueButton()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
