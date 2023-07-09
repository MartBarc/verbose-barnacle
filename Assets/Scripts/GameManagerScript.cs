using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public GameObject player;
    public int roundScore;//money
    public TextMeshProUGUI scoreText;
    //public TextMeshProUGUI totalDestroyedText;
    public TextMeshProUGUI timerText;
    public GameObject GameOverUI;
    public GameObject RoundOverUI;
    public GameObject WinUI;
    public GameObject PauseUI;
    public bool GameStarted;
    public float setupTime = 30;
    public float currentTimeLeft;
    public GameObject heroSpawnLocation;
    public GameObject HeroPrefab;
    public Slider HeroStamBar;
    //public float HeroStam;
    public TextMeshProUGUI StamText;
    public bool canCountStam = true;
    public int ScoreToWin = 1000;
    public bool GameIsPaused = false;

    public GameObject curHero;
    public bool canUpdate;

    public bool isRoundOverState = false;

    float _time;
    [SerializeField] float _interval = 1f;

    [SerializeField] public InsuredObsHandler insuredObs;
    public int weWon = 0;
    public AudioSource GameMusic;



    ///// <summary>
    ///// Grid graph to update.
    ///// This will be set at Start based on <see cref="graphIndex"/>.
    ///// During runtime you may set this to any graph or to null to disable updates.
    ///// </summary>
    //public GridGraph graph;
    //[HideInInspector]
    //public int graphIndex;

    private void Start()
    {
        GameMusic = GameObject.Find("Sounds/GameMusic").GetComponent<AudioSource>();
        GameMusic.Play();
        //load stuff here
        roundScore = PlayerPrefs.GetInt("ScoreData");
        if (roundScore <= 0)
        {
            roundScore = 100;
        }
        weWon = PlayerPrefs.GetInt("WonData");

        GameIsPaused = false;
        GameStarted = false;
        currentTimeLeft = setupTime;
        player = GameObject.Find("Player");
        scoreText.text = $"{roundScore} Gold";
        timerText.text = "";
        GameOverUI.SetActive(false);
        RoundOverUI.SetActive(false);
        WinUI.SetActive(false);
        PauseUI.SetActive(false);
        //start counting down from 30sec when game starts, spawn hero after 30 seconds

        _time = 0f;

        //if (graph == null)
        //{
        //    if (graphIndex < 0) throw new System.Exception("Graph index should not be negative");
        //    if (graphIndex >= AstarPath.active.data.graphs.Length) throw new System.Exception("The ProceduralGridMover was configured to use graph index " + graphIndex + ", but only " + AstarPath.active.data.graphs.Length + " graphs exist");

        //    graph = AstarPath.active.data.graphs[graphIndex] as GridGraph;
        //    if (graph == null) throw new System.Exception("The ProceduralGridMover was configured to use graph index " + graphIndex + " but that graph either does not exist or is not a GridGraph or LayerGridGraph");
        //}
        //HeroStam = 100;
        HeroStamBar.value = 0;
        HeroStamBar.maxValue = 0;
        StamText.text = "";
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isRoundOverState)
        {
            if (GameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        if (GameStarted)
        {
            timerText.text = "";
            
            //totalDestroyedText.text = "Total: " + tempScore;

            if (canCountStam)
            {
                curHero.GetComponent<HeroController>().stam -= 1 * Time.deltaTime;
            }

            int newHeroStam = 0;
            if (curHero.GetComponent<HeroController>().stam > 0f)
            {
                newHeroStam = (int)curHero.GetComponent<HeroController>().stam;
            }

            HeroStamBar.value = newHeroStam;
            StamText.text = HeroStamBar.value + " / " + HeroStamBar.maxValue;

            if (canUpdate)
            {
                StartCoroutine(updateScan());
            }
            //UpdatePathing();
        }
        else 
        {
            currentTimeLeft -= 1 * Time.deltaTime;
            if (currentTimeLeft < 0)
            {
                currentTimeLeft = 0;
                GameStarted = true;
                timerText.text = "";
                curHero = Instantiate(HeroPrefab, heroSpawnLocation.transform.position, heroSpawnLocation.transform.rotation);//add this later
                curHero.GetComponent<HeroController>().spawner = heroSpawnLocation;
                curHero.GetComponent<HeroController>().player = player;
                curHero.GetComponent<HeroController>().curTarget = GameObject.Find("HeroTarget").gameObject.GetComponent<TargetPoint>();
                curHero.GetComponent<AIDestinationSetter>().target = GameObject.Find("HeroTarget").gameObject.transform;

                HeroStamBar.value = curHero.GetComponent<HeroController>().stam;
                HeroStamBar.maxValue = curHero.GetComponent<HeroController>().maxStam;
            }
            int newCurrentTime = (int)currentTimeLeft;
            timerText.text = $"{newCurrentTime} seconds\nuntil the hero arrives!";
        }
    }

    private void FixedUpdate()
    {
        
        if (isRoundOverState) return;

        if (roundScore <= 0)
        {
            StartCoroutine(roundOverRoutine());
        }
        if (roundScore >= ScoreToWin && weWon == 0)
        {
            //pause and pull up window saying you won, would you like to continue? and dont ask this ever again, 
            //change round over window to ask if they also would like to start from scratch
            weWon = 1;
            PlayerPrefs.SetInt("WonData", weWon);
            //pause game here
            //GameIsPaused = true;
            //WinUI.SetActive(true);
            //Time.timeScale = 0f;//pause
        }

        if (GameStarted)
        {
            // Check player splattered
            if (player == null || !player.GetComponent<PlayerScript>().isAlive)
            {
                //ScoreSub(int scoreDecrease)
                StartCoroutine(roundOverRoutine());
                return;
            }

            // Check if hero big baby
            if (curHero == null || curHero.GetComponent<HeroController>().stam <= 0f)
            {
                StartCoroutine(roundOverRoutine());
                return;
            }

            // If all objects destroyed
            //if (insuredObs.obsList.Count <= 0)
            //{
            //    StartCoroutine(roundOverRoutine());
            //    return;
            //}
        }
    }

    IEnumerator updateScan()
    {
        //Debug.Log("Scanning map");
        canUpdate = false;
        UpdatePathing();
        yield return new WaitForSecondsRealtime(2f);
        canUpdate = true;
    }

    public void UpdatePathing()
    {
        // Move the center (this is in world units, so we need to convert it back from graph space)
        //graph.center = curHero.transform.position;
        //graph.UpdateTransform();

        AstarPath.active.Scan();
        //Debug.Log("AStar Rescanned");
    }

    public void ScoreAdd(int scoreIncrease)
    {
        roundScore += scoreIncrease;
        scoreText.text = $"{roundScore} Gold";
        //if above 1000, you win
        //bring up window that asks if you want to keep going for highscore
    }

    public void ScoreSub(int scoreDecrease)
    {
        roundScore -= scoreDecrease;
        if (roundScore <= 0)
        {
            roundScore = 0;
        }
        scoreText.text = $"{roundScore} Gold";
    }

    public void newGameBtn() 
    {
        SceneManager.LoadScene("MAP1_SCENE");
    }

    public void QuitGameBtn()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    IEnumerator roundOverRoutine()
    {
        player.GetComponent<PlayerScript>().isAlive = false;//not actually dead, just want to stop movement
        isRoundOverState = true;
        if (curHero != null)
        {
            curHero.GetComponent<HeroController>().isRoundOver = true;
        }
        

        //SharedInfo.InsurancePayoff = insuredObs.GetDestroyedInsurance();

        yield return new WaitForSecondsRealtime(0f);



        if (roundScore <= 0)
        {
            roundScore = 0;

            //show player all their stats for the run
            GameOverUI.SetActive(true);

            //delete stuff here
            PlayerPrefs.DeleteAll();
        }
        else 
        {
            
            RoundOverUI.SetActive(true);
            
        }

        //save stuff here
        PlayerPrefs.SetInt("ScoreData", roundScore);
    }

    public void StunPlayer()
    {
        curHero.GetComponent<HeroController>().isStunned = true;
        curHero.GetComponent<HeroController>().stam -= 20;
    }
    
    public void deleteData() 
    {
        //delete stuff here
        PlayerPrefs.DeleteAll();

        //debug
        scoreText.text = $"{roundScore} Gold";
    }

    public void AddData()
    {
        roundScore = roundScore + 100;
        //save stuff here
        PlayerPrefs.SetInt("ScoreData", roundScore);

        //debug
        scoreText.text = $"{roundScore} Gold";
    }

    public void SubData()
    {
        roundScore = roundScore - 100;
        //save stuff here
        PlayerPrefs.SetInt("ScoreData", roundScore);

        //debug
        scoreText.text = $"{roundScore} Gold";
    }

    public void ResumeGame() 
    {
        GameIsPaused = false;
        WinUI.SetActive(false);
        PauseUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        GameIsPaused = true;
        PauseUI.SetActive(true);
        Time.timeScale = 0f;

    }
}
