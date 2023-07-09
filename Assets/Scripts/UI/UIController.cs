using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Scripting;

public class UIController : MonoBehaviour
{
    #region Variable Declaration
    #region Top Level Managers
    //[SerializeField] MainMenuManager mainMenuManager;
    //[SerializeField] GameManagerScript gameManager;
    [SerializeField] UIDocument mainMenu;
    [SerializeField] UIDocument inGameOverlay;
    //[SerializeField] UIDocument pauseMenu;
    //[SerializeField] UIDocument gameOverMenu;
    VisualElement mainMenuRoot;
    VisualElement inGameOverlayRoot;
    //VisualElement pauseMenuRoot;
    //VisualElement gameOverMenuRoot;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource musicSource;
    #endregion

    #region Menu Variables
    Dictionary<VisualElement, VisualElement> parentMenus;
    private const string MENU_SLIDE_OFF = "menu-slide-off";
    private const string DISABLED = "disabled";
    private const string BUTTON = "button";
    [SerializeField] Sprite CHECKMARK; // = " url('project://database/assets/sprites/ui/ui_paper_checkmark_standard.png?fileid=2800000&guid=fbecb287c78e32a458252e3de1a26365&type=3#ui_paper_checkmark_standard')";
    [SerializeField] Sprite CROSS;
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
        mainMenuRoot = mainMenu.rootVisualElement;
        startButtonsContainer = mainMenuRoot.Q<GroupBox>("start-buttons-container");
        mainMenuStartButtons = new();
        foreach (VisualElement startButton in startButtonsContainer.Children())
        {
            mainMenuStartButtons.Add((Button)startButton);
        }
        //mainMenuStartButtons[0].clicked += newGameStart;
        CheckContinue();
        //mainMenuStartButtons[1].clicked += continueGameStart;
        mainMenuStartButtons[2].clicked += () =>
        {
            ToggleMenu(startButtonsContainer);
            ToggleMenu(optionsContainer);
        };

        optionsContainer = mainMenuRoot.Q<GroupBox>("options-container");
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

        #region In Game Overlay
        inGameOverlayRoot = inGameOverlay.rootVisualElement;
        #endregion

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

    public void CheckContinue()
    {
        //if(mainMenuManager.continueBtnEnable)
        if(true)
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
    }

}