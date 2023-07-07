using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public GameObject player;
    public int score;//money
    public TextMeshProUGUI scoreText;
    public GameObject GameOverUI;

    private void Start()
    {
        player = GameObject.Find("Player");
        score = 0;
        scoreText.text = "Score: " + score;
        GameOverUI.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            return;
        }
        if (!player.GetComponent<PlayerScript>().isAlive)
        {
            //Debug.Log("player dead ahahahahaha");
            GameOverUI.SetActive(true);
        }
    }

    public void ScoreAdd(int scoreIncrease)
    {
        score += scoreIncrease;
        scoreText.text = "Score: " + score;
    }

    public void ScoreSub(int scoreDecrease)
    {
        score -= scoreDecrease;
        scoreText.text = "Score: " + score;
    }

    public void newGameBtn() 
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGameBtn()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
