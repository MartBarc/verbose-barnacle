using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject PlayMenu;
    public GameObject SettingsMenu;


    private void Start()
    {
        MainMenu.SetActive(true);
        PlayMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        //marty
    }

    public void newGameBtn()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void PlayMenuBtn()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void SettingsMenuBtn()
    {
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    public void QuitMenuBtn()
    {
        #if UNITY_EDITOR
                // Application.Quit() does not work in the editor so
                // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                         Application.Quit();
        #endif
    }

    public void BackBtn() 
    {
        SettingsMenu.SetActive(false);
        MainMenu.SetActive(true);
    }


}
