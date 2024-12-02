using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeState : MonoBehaviour
{
    public void ToPlanet()
    {
        SceneManager.LoadScene("Planet");
    }

    public void ToSpace()
    {
        SceneManager.LoadScene("Space");
    }

    public void ToBoss()
    {
        SceneManager.LoadScene("Boss");
    }

    public void ToTitle()
    {
        SceneManager.LoadScene("GameStart");
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
