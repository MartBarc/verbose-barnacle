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


    private void Awake()
    {
        root = uiDocument.rootVisualElement;
        buttonsContainer = root.Q<GroupBox>("start-buttons-container");
        buttons = new();
        foreach (VisualElement button in buttonsContainer.Children())
        {
            buttons.Add((Button)button);
        }

        buttons[0].clicked += gameManager.ResumeGame;
        
        buttons[1].clicked += () =>
        {
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
        optionsReturn = (Button)optionsContainer.Q<Button>("options-return");
        optionsReturn.clicked += () =>
        {
            ToggleMenu(optionsContainer);
            ToggleMenu(buttonsContainer);
        };
    }

    //public void SetTogglesOnStart()
    //{
    //    if (PlayerPrefs.GetInt("MuteAudio") == 0)
    //    {
    //        audioSource.mute = false;
    //        audioToggle.style.backgroundImage = Background.FromSprite(CHECKMARK);
    //    }
    //    else
    //    {
    //        audioSource.mute = true;
    //        audioToggle.style.backgroundImage = Background.FromSprite(CROSS);
    //    }

    //    if (PlayerPrefs.GetInt("MuteMusic") == 0)
    //    {
    //        musicSource.mute = false;
    //        musicToggle.style.backgroundImage = Background.FromSprite(CHECKMARK);
    //    }
    //    else
    //    {
    //        musicSource.mute = true;
    //        musicToggle.style.backgroundImage = Background.FromSprite(CROSS);
    //    }
    //}

    //public void AudioToggle()
    //{
    //    Debug.Log("Toggling audio!");
    //    if (audioSource.mute)
    //    {
    //        audioSource.mute = false;
    //        audioToggle.style.backgroundImage = Background.FromSprite(CHECKMARK);
    //        PlayerPrefs.SetInt("MuteAudio", 0);
    //    }
    //    else
    //    {
    //        audioSource.mute = true;
    //        audioToggle.style.backgroundImage = Background.FromSprite(CROSS);
    //        PlayerPrefs.SetInt("MuteAudio", 1);
    //    }
    //    //audioSource.mute = !audioSource.mute;
    //    //audioToggle.ToggleInClassList(TOGGLE_OFF);
    //}

    //public void MusicToggle()
    //{
    //    Debug.Log("Toggling music!");
    //    if (musicSource.mute)
    //    {
    //        musicSource.mute = false;
    //        musicToggle.style.backgroundImage = Background.FromSprite(CHECKMARK);
    //        PlayerPrefs.SetInt("MuteMusic", 0);
    //    }
    //    else
    //    {
    //        musicSource.mute = true;
    //        musicToggle.style.backgroundImage = Background.FromSprite(CROSS);
    //        PlayerPrefs.SetInt("MuteMusic", 0);
    //    }
    //    //musicSource.mute = !musicSource.mute;
    //    ////musicToggle.ToggleInClassList(TOGGLE_OFF);
    //    //musicToggle.style.backgroundImage = Background.FromSprite(CHECKMARK);
    //}

    //public void ToggleMenu(VisualElement visualElement)
    //{
    //    //Debug.Log("Toggling " + visualElement.name + ". Starting from y = " + visualElement.transform.position.y);

    //    //if(visualElement.transform.position.y < Screen.height)
    //    //{
    //    //    visualElement.style.translate = new StyleTranslate(new Translate(0, visualElement.transform.position.y + Screen.height));
    //    //}
    //    //else
    //    //{
    //    //    visualElement.style.translate = new StyleTranslate(new Translate(0, visualElement.transform.position.y - Screen.height));
    //    //}

    //    //Debug.Log("Moved to y = " + visualElement.transform.position.y);




    //    visualElement.ToggleInClassList(MENU_SLIDE_OFF);
    //}
}