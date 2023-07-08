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
    public TextMeshProUGUI totalDestroyedText;
    public TextMeshProUGUI timerText;
    public GameObject GameOverUI;
    public bool GameStarted;
    public float setupTime = 30;
    public float currentTimeLeft;
    public GameObject heroSpawnLocation;
    public GameObject HeroPrefab;
    public Slider HeroStamBar;
    //public float HeroStam;
    public TextMeshProUGUI StamText;
    public bool canCountStam = true;

    public GameObject curHero;
    public bool canUpdate;

    public bool isRoundOverState = false;

    float _time;
    [SerializeField] float _interval = 1f;

    [SerializeField] public InsuredObsHandler insuredObs;

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
        roundScore = SharedInfo.Funds;
        GameStarted = false;
        currentTimeLeft = setupTime;
        player = GameObject.Find("Player");
        scoreText.text = "Score: " + roundScore;
        totalDestroyedText.text = "Score: " + 0;
        timerText.text = "";
        GameOverUI.SetActive(false);
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
        if (GameStarted)
        {
            timerText.text = "";
            int tempScore = roundScore + insuredObs.GetDestroyedInsurance();
            totalDestroyedText.text = "Total: " + tempScore;

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
            timerText.text = newCurrentTime.ToString();
        }
    }

    private void FixedUpdate()
    {
        if (isRoundOverState) return;

        if (GameStarted)
        {
            // Check player splattered
            if (player == null || !player.GetComponent<PlayerScript>().isAlive)
            {
                roundScore -= 50;
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
            if (insuredObs.obsList.Count <= 0)
            {
                StartCoroutine(roundOverRoutine());
                return;
            }
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
        scoreText.text = "Score: " + roundScore;
    }

    public void ScoreSub(int scoreDecrease)
    {
        roundScore -= scoreDecrease;
        scoreText.text = "Score: " + roundScore;
    }

    public void newGameBtn() 
    {
        SceneManager.LoadScene("TEST_SCENE");
    }

    public void QuitGameBtn()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    IEnumerator roundOverRoutine()
    {
        //GameOverUI.SetActive(true);
        isRoundOverState = true;
        curHero.GetComponent<HeroController>().isRoundOver = true;

        SharedInfo.InsurancePayoff = insuredObs.GetDestroyedInsurance();

        yield return new WaitForSecondsRealtime(4f);

        GameOverUI.SetActive(true);
    }

    IEnumerator restartRountRoutine()
    {
        GameOverUI.SetActive(false);

        yield return new WaitForSecondsRealtime(10f);
    }
}
