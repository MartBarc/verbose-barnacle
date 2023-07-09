using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Obs : MonoBehaviour
{
    public int priority = 0;
    public int health = 100;
    [SerializeField] public int insuranceValue = 20;
    [SerializeField] public int insuranceCost = 10;
    public bool isActive;
    public bool isTower;
    public bool purchased;
    public bool purchaseable;
    public TextMeshProUGUI buyText;
    public CircleCollider2D buyableCollider;
    public CircleCollider2D normalColliderTower;
    public BoxCollider2D normalColliderObs;

    // Dustruction
    [SerializeField] public GameObject debriPrefab;
    [SerializeField] public float rightMod = 0;
    [SerializeField] public float upMod = 0;

    private void Start()
    {
        if (gameObject.tag == "Enemy" || gameObject.tag == "Player" || gameObject.tag == "Door")//Bat is only obs or minion with tag "Enemy"
        {
            return;
        }
        if (this.gameObject.GetComponent<TowerScript>())
        {
            isTower = true;
            normalColliderTower.enabled = false;
        }
        else 
        {
            normalColliderObs.enabled = false;
        }
        buyableCollider.enabled = true;
    }

    private void Update()
    {
        if (gameObject.tag == "Enemy" || gameObject.tag == "Player" || gameObject.tag == "Door")//Bat is only obs or minion with tag "Enemy"
        {
            return;
        }
        if (GameObject.Find("GameManager").GetComponent<GameManagerScript>().GameStarted)
        {

            //if gamemanager.gamestarted = true; 
            //destroy this gameobject
            if (!isActive && isTower)
            {
                Destroy(gameObject);
            }
            if (!isActive && !isTower)
            {
                normalColliderObs.enabled = true;
                buyableCollider.enabled = false;
                insuranceValue = 0;
            }
        }
        else 
        {
            if (purchaseable)
            {
                buyText.SetText("Press E to purchase " + gameObject.tag + " for $" + insuranceValue + ".");
                if (Input.GetKeyDown(KeyCode.E))
                {
                    buyText.gameObject.SetActive(false);
                    purchaseable = false;
                    isActive = true;
                    purchased = true;
                    GameObject.Find("GameManager").gameObject.GetComponent<GameManagerScript>().ScoreSub(insuranceCost);
                    buyText.SetText("");
                    if (isTower)
                    {
                        buyableCollider.enabled = false;
                        normalColliderTower.enabled = true;
                        if (this.gameObject.GetComponent<TowerScript>().towerid == 1)
                        {
                            this.gameObject.GetComponent<TowerScript>().abilityOn = true;
                            this.gameObject.GetComponent<TowerScript>().towerxability(1);
                        }
                        this.gameObject.GetComponent<TowerScript>().AoeImage.SetActive(true);

                    }
                }
            }
            
        }
    }
    

    public void TriggerDestroy()
    {
        priority = 0;

        //this.gameObject.GetComponent<Rigidbody2D>().drag = 0f;
        //this.gameObject.GetComponent<Rigidbody2D>().mass = 4f;

        StartCoroutine(obsDestroyed());
    }

    public void TakeHit(int help)
    {
        health -= help;

        if (health <= 0)
        {
            GameObject.Find("GameManager").GetComponent<GameManagerScript>().ScoreAdd(insuranceValue);
            if (this.gameObject.GetComponent<PlayerScript>())//dont destroy the player
            {
                this.gameObject.GetComponent<PlayerScript>().TakeHit(help);
            }
            else if (this.gameObject.GetComponent<MinionScript>()) 
            {
                this.gameObject.GetComponent<MinionScript>().killMinion();
                priority = 0;

            }
            else
            {
                TriggerDestroy();
            }
        }
    }

    IEnumerator obsDestroyed()
    {
        if (debriPrefab != null) 
        {
            GameObject spawnedDebris = Instantiate(debriPrefab, this.transform.position, this.transform.rotation);
        }
       
        //spawnedDebris.GetComponent<DebrisScript>().ModDir(rightMod, upMod);

        //yield return new WaitForSecondsRealtime(0f);

        //Spawn destruction

        Destroy(this.gameObject);

        //do blow up physics on delete
        yield return new WaitForSecondsRealtime(0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!purchased)
        {
            if (collision.gameObject.tag == "Player")
            {
                buyText.gameObject.SetActive(true);
                purchaseable = true;
            }
            return;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!purchased)
        {
            if (collision.gameObject.tag == "Player")
            {
                buyText.gameObject.SetActive(false);
                purchaseable = false;
            }
            return;
        }
    }
}
