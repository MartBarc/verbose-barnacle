using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;
using Unity.VisualScripting;

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

    public GameObject curHero;

    float _time;
    [SerializeField] float _interval = 1f;

    /// <summary>
    /// Grid graph to update.
    /// This will be set at Start based on <see cref="graphIndex"/>.
    /// During runtime you may set this to any graph or to null to disable updates.
    /// </summary>
    public GridGraph graph;

    /// <summary>
    /// Index for the graph to update.
    /// This will be used at Start to set <see cref="graph"/>.
    ///
    /// This is an index into the AstarPath.active.data.graphs array.
    /// </summary>
    [HideInInspector]
    public int graphIndex;

    private void Start()
    {
        GameStarted = false;
        currentTimeLeft = setupTime;
        player = GameObject.Find("Player");
        scoreText.text = "Score: " + score;
        timerText.text = "";
        GameOverUI.SetActive(false);
        //start counting down from 30sec when game starts, spawn hero after 30 seconds

        _time = 0f;

        if (graph == null)
        {
            if (graphIndex < 0) throw new System.Exception("Graph index should not be negative");
            if (graphIndex >= AstarPath.active.data.graphs.Length) throw new System.Exception("The ProceduralGridMover was configured to use graph index " + graphIndex + ", but only " + AstarPath.active.data.graphs.Length + " graphs exist");

            graph = AstarPath.active.data.graphs[graphIndex] as GridGraph;
            if (graph == null) throw new System.Exception("The ProceduralGridMover was configured to use graph index " + graphIndex + " but that graph either does not exist or is not a GridGraph or LayerGridGraph");
        }
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
            _time += Time.deltaTime;
            while (_time >= _interval)
            {
                UpdatePathing();
                _time -= _interval;
            }
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

        //AstarPath.active.Scan();
    }

    public void UpdatePathing()
    {
        // Move the center (this is in world units, so we need to convert it back from graph space)
        graph.center = curHero.transform.position;
        graph.UpdateTransform();

        AstarPath.active.Scan();
        //Debug.Log("AStar Rescanned");
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
