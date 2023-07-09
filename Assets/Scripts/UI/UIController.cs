using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Scripting;

public class UIController : MonoBehaviour
{
    #region Top Level Managers
    //[SerializeField] MainMenuManager mainMenuManager;
    //[SerializeField] GameManagerScript gameManager;
    [SerializeField] UIDocument mainMenu;
    //[SerializeField] UIDocument inGameOverlay;
    //[SerializeField] UIDocument pauseMenu;
    //[SerializeField] UIDocument gameOverMenu;
    public VisualElement mainMenuRoot;
    //public VisualElement inGameOverlayRoot;
    //public VisualElement pauseMenuRoot;
    //public VisualElement gameOverMenuRoot;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource musicSource;
    #endregion

    #region Menu Variables
    private const string MENU_SLIDE_IN = "menu-slide-in";
    private const string MENU_SLIDE_OUT = "menu-slide-out";
    private const string DISABLED = "disabled";
    [SerializeField] Sprite CHECKMARK; // = " url('project://database/assets/sprites/ui/ui_paper_checkmark_standard.png?fileid=2800000&guid=fbecb287c78e32a458252e3de1a26365&type=3#ui_paper_checkmark_standard')";
    [SerializeField] Sprite CROSS;
    #endregion

    #region Main Menu
    private GroupBox startButtonsContainer;
    private List<Button> mainMenuStartButtons;
    private GroupBox optionsContainer;
    private Button audioToggle;
    private Button musicToggle;
    #endregion

    private void Start()
    {
        #region Main Menu
        mainMenuRoot = mainMenu.rootVisualElement;
        startButtonsContainer = mainMenuRoot.Q<GroupBox>("start-buttons-container");
        //mainMenuStartButtons = new();
        foreach (VisualElement startButton in startButtonsContainer.Children())
        {
            mainMenuStartButtons.Add((Button)startButton);
        }
        //mainMenuStartButtons[0].clicked += newGameStart;
        //mainMenuStartButtons[1].clicked += continueGameStart;
        mainMenuStartButtons[2].clicked += () => { ToggleMenu(optionsContainer);};

        optionsContainer = mainMenuRoot.Q<GroupBox>("options-container");
        audioToggle = (Button)optionsContainer.Q<GroupBox>("audio-option").ElementAt(0);
        audioToggle.clicked += AudioToggle;
        musicToggle = (Button)optionsContainer.Q<GroupBox>("music-option").ElementAt(0);
        musicToggle.clicked += MusicToggle;
        #endregion

        //inGameOverlayRoot = inGameOverlay.rootVisualElement;
        //pauseMenuRoot = pauseMenu.rootVisualElement;
        //gameOverMenuRoot = gameOverMenu.rootVisualElement;
    }

    public void AudioToggle()
    {
        Debug.Log("Toggling audio!");
        if(audioSource.mute)
        {
            audioSource.mute = false;
            audioToggle.style.backgroundImage = Background.FromSprite(CHECKMARK);
        }
        else
        {
            audioSource.mute = true;
            audioToggle.style.backgroundImage = Background.FromSprite(CROSS);
        }
        //audioSource.mute = !audioSource.mute;
        //audioToggle.ToggleInClassList(TOGGLE_OFF);
    }

    public void MusicToggle()
    {
        Debug.Log("Toggling music!");
        if (musicSource.mute)
        {
            musicSource.mute = false;
            musicToggle.style.backgroundImage = Background.FromSprite(CHECKMARK);
        }
        else
        {
            musicSource.mute = true;
            musicToggle.style.backgroundImage = Background.FromSprite(CROSS);
        }
        //musicSource.mute = !musicSource.mute;
        ////musicToggle.ToggleInClassList(TOGGLE_OFF);
        //musicToggle.style.backgroundImage = Background.FromSprite(CHECKMARK);
    }

    public void ToggleMenu(VisualElement visualElement)
    {

    }

}