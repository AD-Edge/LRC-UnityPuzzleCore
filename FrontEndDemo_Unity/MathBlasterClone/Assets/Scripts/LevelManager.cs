using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    void Update()
    {
        //Check for escape button event
        //On Android doubles as the 'back' button
        if (Input.GetKeyDown(KeyCode.Escape)) {
            QuitGame();
        }
    }

    public void LoadNewScene(string scenename)
    {
        Debug.Log("sceneName to load: " + scenename);
        SceneManager.LoadScene(scenename);
    }

    public void QuitGame()
    {
        Debug.Log("Quit requested - Exit");
        Application.Quit();
    }
}