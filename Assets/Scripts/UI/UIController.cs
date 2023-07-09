using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Scripting;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    #region Variable Declaration
    #region Top Level Managers
    //[SerializeField] MainMenuManager mainMenuManager;
    //[SerializeField] GameManagerScript gameManager;
    [SerializeField] protected UIDocument uiDocument;
    //[SerializeField] UIDocument inGameOverlay;
    //[SerializeField] UIDocument pauseMenu;
    //[SerializeField] UIDocument gameOverMenu;
    protected VisualElement root;
    //VisualElement inGameOverlayRoot;
    //VisualElement pauseMenuRoot;
    //VisualElement gameOverMenuRoot;
    [SerializeField] protected AudioController audioController;
    //[SerializeField] protected AudioSource musicSource;
    #endregion

    #region Menu Variables
    Dictionary<VisualElement, VisualElement> parentMenus;
    protected const string MENU_SLIDE_OFF = "menu-slide-off";
    protected const string DISABLED = "disabled";
    protected const string BUTTON = "button";
    [SerializeField] protected Sprite CHECKMARK; // = " url('project://database/assets/sprites/ui/ui_paper_checkmark_standard.png?fileid=2800000&guid=fbecb287c78e32a458252e3de1a26365&type=3#ui_paper_checkmark_standard')";
    [SerializeField] protected Sprite CROSS;
    #endregion

    #region Main Menu
    private GroupBox startButtonsContainer;
    private List<Button> mainMenuStartButtons;
    private GroupBox optionsContainer;
    private Button audioToggle;
    private Button musicToggle;
    private Button optionsReturn;
    #endregion

    #region In Game Overlay
    private Label score;
    #endregion
    #endregion

    private void Start()
    {
        #region Main Menu
        root = uiDocument.rootVisualElement;
        startButtonsContainer = root.Q<GroupBox>("start-buttons-container");
        mainMenuStartButtons = new();
        foreach (VisualElement startButton in startButtonsContainer.Children())
        {
            mainMenuStartButtons.Add((Button)startButton);
        }
        mainMenuStartButtons[0].clicked += NewGameStart;
        CheckContinue();
        mainMenuStartButtons[1].clicked += ContinueGameStart;
        mainMenuStartButtons[2].clicked += () =>
        {
            ToggleMenu(startButtonsContainer);
            ToggleMenu(optionsContainer);
        };

        optionsContainer = root.Q<GroupBox>("options-container");
        audioToggle = (Button)optionsContainer.Q<GroupBox>("audio-option").ElementAt(0);
        audioToggle.clicked += AudioToggle;
        musicToggle = (Button)optionsContainer.Q<GroupBox>("music-option").ElementAt(0);
        musicToggle.clicked += MusicToggle;
        optionsReturn = (Button)optionsContainer.Q<Button>("options-return");
        optionsReturn.clicked += () =>
        {
            ToggleMenu(optionsContainer);
            ToggleMenu(startButtonsContainer);
        };
        #endregion

        //#region In Game Overlay
        //inGameOverlayRoot = inGameOverlay.rootVisualElement;
        //#endregion

        //pauseMenuRoot = pauseMenu.rootVisualElement;
        //gameOverMenuRoot = gameOverMenu.rootVisualElement;
    }

    public void SetTogglesOnStart()
    {
        if(PlayerPrefs.GetInt("MuteAudio") == 0)
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

    public void AudioToggle()
    {
        Debug.Log("Toggling audio!");
        if(audioController.audioMuted)
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

    public void MusicToggle()
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

    public void ToggleMenu(VisualElement visualElement)
    {
        //Debug.Log("Toggling " + visualElement.name + ". Starting from y = " + visualElement.transform.position.y);

        //if(visualElement.transform.position.y < Screen.height)
        //{
        //    visualElement.style.translate = new StyleTranslate(new Translate(0, visualElement.transform.position.y + Screen.height));
        //}
        //else
        //{
        //    visualElement.style.translate = new StyleTranslate(new Translate(0, visualElement.transform.position.y - Screen.height));
        //}

        //Debug.Log("Moved to y = " + visualElement.transform.position.y);




        visualElement.ToggleInClassList(MENU_SLIDE_OFF);
    }

    public void NewGameStart()
    {
        Debug.Log("Starting new game!");
        SceneManager.LoadScene("CutsceneScene");
    }

    public void CheckContinue()
    {
        //if(mainMenuManager.continueBtnEnable)
        //if(true)
        if(PlayerPrefs.GetInt("ScoreData") > 0)
        {
            mainMenuStartButtons[1].RemoveFromClassList(DISABLED);
            mainMenuStartButtons[1].AddToClassList(BUTTON);
            mainMenuStartButtons[1].clicked += ContinueGameStart;
        }
        else
        {
            mainMenuStartButtons[1].RemoveFromClassList(BUTTON);
            mainMenuStartButtons[1].AddToClassList(DISABLED);
            mainMenuStartButtons[1].clicked -= ContinueGameStart;
        }
    }

    public void ContinueGameStart()
    {
        Debug.Log("Continuing Game from Previous Save");
        SceneManager.LoadScene("MAP1_SCENE");
    }

}