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
    public TextMeshProUGUI timerText;
    public GameObject GameOverUI;
    public bool GameStarted;
    public float setupTime = 30;
    public float currentTimeLeft;
    public GameObject heroSpawnLocation;
    public GameObject HeroPrefab;

    private void Start()
    {
        GameStarted = false;
        currentTimeLeft = setupTime;
        player = GameObject.Find("Player");
        scoreText.text = "Score: " + score;
        timerText.text = "";
        GameOverUI.SetActive(false);
        //start counting down from 30sec when game starts, spawn hero after 30 seconds

    }

    private void Update()
    {
        if (!GameStarted)
        {
            currentTimeLeft -= 1 * Time.deltaTime;
            if (currentTimeLeft < 0)
            {
                currentTimeLeft = 0;
                GameStarted = true;
                timerText.text = "";
                GameObject bullet = Instantiate(HeroPrefab, heroSpawnLocation.transform.position, heroSpawnLocation.transform.rotation);//add this later
                bullet.GetComponent<HeroController>().player = player;
                bullet.GetComponent<HeroController>().curTarget = GameObject.Find("HeroTarget").gameObject.GetComponent<TargetPoint>();
                bullet.GetComponent<AIDestinationSetter>().target = GameObject.Find("HeroTarget").gameObject.transform;
            }
            int newCurrentTime = (int)currentTimeLeft;
            timerText.text = newCurrentTime.ToString();
        }
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
        if (score <= 0)
        {
            //no money = bad
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
