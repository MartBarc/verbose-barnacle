using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class RoundEndMenuController : MonoBehaviour
{
    [SerializeField] GameManagerScript gameManager;
    [SerializeField] UIDocument uiDocument;

    VisualElement root;
    VisualElement dialogContainer;
    Label heroText;
    Label goldEarnedText;
    Button playAgainButton;
    Button returnToMainButton;

    int startingGold;
    int goldEarned;

    private void Start()
    {
        root = uiDocument.rootVisualElement;
        dialogContainer = root.Q<VisualElement>("dialog-container");
        heroText = dialogContainer.Q<Label>("hero-text");
        goldEarnedText = dialogContainer.Q<Label>("gold-earned-text");
        playAgainButton = root.Q<Button>("play-again");
        playAgainButton.clicked += () =>
        {
            SceneManager.LoadScene("MAP1_SCENE");
        };

        returnToMainButton = root.Q<Button>("return-to-main");
        returnToMainButton.clicked += () =>
        {
            SceneManager.LoadScene("MainMenuScene");
        };

        //startingGold = PlayerPrefs.GetInt("ScoreData");
    }

    private void Update()
    {
        //goldEarned = gameManager.roundScore - startingGold;
        //if(goldEarned < 0)
        //{
        //    goldEarnedText.text = "You Lost " + goldEarned + "Gold...";
        //}
        //else
        //{
        //    goldEarnedText.text = "You Earned " + goldEarned + "Gold!";
        //}
        goldEarnedText.text = "You have " + gameManager.roundScore + " Gold!";
    }


}
