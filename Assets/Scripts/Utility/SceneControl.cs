using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneControl : MonoBehaviour
{
    public bool mainMenu;

    void LeaveMainMenu()
    {
        if (mainMenu)
        {
            ChangeScene("LevelPrototype");
        }
    }

    public static void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void ChangeSceneAdditive(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public static void ExitGame(int code)
    {
        Debug.Log("Quit Game");
        Application.Quit(code);
    }

    public static void ExitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
