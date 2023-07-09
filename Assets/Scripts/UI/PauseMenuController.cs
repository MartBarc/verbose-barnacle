using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PauseMenuController : UIController
{
    [SerializeField] GameManagerScript gameManager;
    //SerializeField] UIDocument uIDocument;
    //VisualElement root;
    GroupBox buttonsContainer;
    List<Button> buttons;
    GroupBox optionsContainer;
    Button audioToggle;
    Button musicToggle;
    Button optionsReturn;


    private void Start()
    {
        root = uiDocument.rootVisualElement;
        buttonsContainer = root.Q<GroupBox>("buttons-container");
        buttons = new();
        foreach (VisualElement button in buttonsContainer.Children())
        {
            buttons.Add((Button)button);
        }

        buttons[0].clicked += () =>
        {
            Debug.Log("resuming");
            gameManager.ResumeGame();
        };
        
        buttons[1].clicked += () =>
        {
            Debug.Log("toggle menu");
            ToggleMenu(buttonsContainer);
            ToggleMenu(optionsContainer);
        };

        buttons[2].clicked += () =>
        {
            SceneManager.LoadScene("MainMenuScene");
        };

        optionsContainer = root.Q<GroupBox>("options-container");
        audioToggle = (Button)optionsContainer.Q<GroupBox>("audio-option").ElementAt(0);
        audioToggle.clicked += AudioToggle;
        musicToggle = (Button)optionsContainer.Q<GroupBox>("music-option").ElementAt(0);
        musicToggle.clicked += MusicToggle;
        SetTogglesOnStart();
        optionsReturn = optionsContainer.Q<Button>("options-return");
        optionsReturn.clicked += () =>
        {
            ToggleMenu(optionsContainer);
            ToggleMenu(buttonsContainer);
        };
    }

    public new void SetTogglesOnStart()
    {
        if (PlayerPrefs.GetInt("MuteAudio") == 0)
        {
            //audioSource.mute = false;
            audioToggle.style.backgroundImage = Background.FromSprite(CHECKMARK);
        }
        else
        {
            //audioSource.mute = true;
            audioToggle.style.backgroundImage = Background.FromSprite(CROSS);
        }

        if (PlayerPrefs.GetInt("MuteMusic") == 0)
        {
            //musicSource.mute = false;
            musicToggle.style.backgroundImage = Background.FromSprite(CHECKMARK);
        }
        else
        {
            //musicSource.mute = true;
            musicToggle.style.backgroundImage = Background.FromSprite(CROSS);
        }
    }

    public new void AudioToggle()
    {
        Debug.Log("Toggling audio!");
        if (audioController.audioMuted)
        {
            //audioSource.mute = false;
            audioController.ToggleSounds();
            audioToggle.style.backgroundImage = Background.FromSprite(CHECKMARK);
            PlayerPrefs.SetInt("MuteAudio", 0);
        }
        else
        {
            //audioSource.mute = true;
            audioController.ToggleSounds();
            audioToggle.style.backgroundImage = Background.FromSprite(CROSS);
            PlayerPrefs.SetInt("MuteAudio", 1);
        }


        //audioSource.mute = !audioSource.mute;
        //audioToggle.ToggleInClassList(TOGGLE_OFF);
    }

    public new void MusicToggle()
    {
        Debug.Log("Toggling music!");
        if (audioController.musicMuted)
        {
            //musicSource.mute = false;
            audioController.ToggleMusic();
            musicToggle.style.backgroundImage = Background.FromSprite(CHECKMARK);
            PlayerPrefs.SetInt("MuteMusic", 0);
        }
        else
        {
            //musicSource.mute = true;
            audioController.ToggleMusic();
            musicToggle.style.backgroundImage = Background.FromSprite(CROSS);
            PlayerPrefs.SetInt("MuteMusic", 0);
        }
        //musicSource.mute = !musicSource.mute;
        ////musicToggle.ToggleInClassList(TOGGLE_OFF);
        //musicToggle.style.backgroundImage = Background.FromSprite(CHECKMARK);
    }
}
