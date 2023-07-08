using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;

public class GameManagerScript : MonoBehaviour
{
    public GameObject player;
    public int score;//money
    public TextMeshProUGUI scoreText;
    public GameObject GameOverUI;

    [SerializeField] AstarPath AStar;

    private void Start()
    {
        player = GameObject.Find("Player");
        score = 0;
        scoreText.text = "Score: " + score;
        GameOverUI.SetActive(false);

        StartCoroutine(logEvery5Second());
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

        //AStar.Scan();
    }

    IEnumerator logEvery5Second()
    {
        while (true)
        {
            Debug.Log("Rescan");
            AStar.ScanAsync();
            yield return new WaitForSeconds(5);
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
