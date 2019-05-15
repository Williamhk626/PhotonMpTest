using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void Options()
    {
        SceneManager.LoadScene(3);
    }
    public void Quit()
    {
        Application.Quit(0);
    }
}
