using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject PlayMenu;
    public GameObject SettingsMenu;
    private int roundScore = 0;
    private int weWon = 0;
    public bool continueBtnEnable;


    private void Start()
    {
        MainMenu.SetActive(true);
        PlayMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        roundScore = PlayerPrefs.GetInt("ScoreData");
        if (roundScore <= 0)
        {
            roundScore = 100;
            continueBtnEnable = false;
            //dear chris,
            //if the above bool is false, set "continueBtn.SetActive(false);"
            //if the above bool is true,  set "continueBtn.SetActive(true);"
            //ps: i love you 
        }
        weWon = PlayerPrefs.GetInt("WonData");
    }

    public void newGameBtn()
    {
        SceneManager.LoadScene("CutsceneScene");
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
