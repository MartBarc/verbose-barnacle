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
    public TextMeshProUGUI timerText;
    public GameObject GameOverUI;
    public bool GameStarted;
    public float setupTime = 30;
    public float currentTimeLeft;
    public GameObject heroSpawnLocation;
    public GameObject HeroPrefab;
    public Slider HeroStamBar;
    public float HeroStam;
    public TextMeshProUGUI StamText;
    public bool canCountStam = true;

    public GameObject curHero;
    public bool canUpdate;

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
        HeroStamBar.value = HeroStam;
        HeroStamBar.maxValue = HeroStam;
        StamText.text = "";
        
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
                curHero = Instantiate(HeroPrefab, heroSpawnLocation.transform.position, heroSpawnLocation.transform.rotation);//add this later
                curHero.GetComponent<HeroController>().player = player;
                curHero.GetComponent<HeroController>().curTarget = GameObject.Find("HeroTarget").gameObject.GetComponent<TargetPoint>();
                curHero.GetComponent<AIDestinationSetter>().target = GameObject.Find("HeroTarget").gameObject.transform;
            }
            int newCurrentTime = (int)currentTimeLeft;
            timerText.text = newCurrentTime.ToString();
        }
        else 
        {
            timerText.text = "";
            //Check player dead
            if (player == null)
            {
                SharedInfo.InsurancePayoff = insuredObs.GetDestroyedInsurance();
            }

            //Check if hero dead
            if (curHero == null)
            {
                SharedInfo.InsurancePayoff = insuredObs.GetDestroyedInsurance();
            }


            if (canCountStam)
            {
                HeroStam -= 1 * Time.deltaTime;
            }
            //HeroStam -= 1 * Time.deltaTime;
            if (HeroStam < 0)
            {
                HeroStam = 0;
            }
            int newHeroStam = (int)HeroStam;
            HeroStamBar.value = newHeroStam;
            StamText.text = HeroStamBar.value + " / " + HeroStamBar.maxValue;

            if (canUpdate)
            {
                StartCoroutine(updateScan());
            }
            //UpdatePathing();
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
        if (roundScore <= 0)
        {
            //no money = bad
        }

        //AstarPath.active.Scan();
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
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGameBtn()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void reduceHeroStam(int value)
    {
        HeroStam = HeroStam - value;
        if (HeroStam < 0)
        {
            HeroStam = 0;
        }
        int newHeroStam = (int)HeroStam;
        HeroStamBar.value = newHeroStam;
        StamText.text = HeroStamBar.value + " / " + HeroStamBar.maxValue;
    }


}
