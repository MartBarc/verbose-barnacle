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
    public bool preventPurchase = false;
    public TextMeshProUGUI buyText;
    public CircleCollider2D buyableCollider;
    public CircleCollider2D normalColliderTower;
    public BoxCollider2D normalColliderObs;
    public AudioSource destroySound;

    bool canDecay = true;
    public int basePriority;
    public int priorityDecay = 20;

    public bool stunHero = false;

    // Dustruction
    [SerializeField] public GameObject debriPrefab;
    public GameObject moneyPrefab;

    private void Start()
    {
        basePriority = priority;

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
        destroySound = GameObject.Find("Sounds/ObsDestroy").GetComponent<AudioSource>();
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

            if (priority > basePriority && canDecay)
            {
                StartCoroutine(DecayPriority());
            }
        }
        else 
        {
            if (purchaseable && !preventPurchase)
            {
                buyText.SetText($"Insurance cost: {insuranceCost}G\nInsurance payout: {insuranceValue}G\nPress [E]");
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //purchase sound//destroySound.Play();//testing sounds here
                    buyText.gameObject.SetActive(false);
                    purchaseable = false;
                    isActive = true;
                    purchased = true;
                    GameObject.Find("GameManager").gameObject.GetComponent<GameManagerScript>().ScoreSub(insuranceCost);
                    buyText.SetText("");
                    if (isTower)
                    {
                        Debug.Log("Purchased tower!");
                        buyableCollider.enabled = false;
                        normalColliderTower.enabled = true;
                        if (this.gameObject.GetComponent<TowerScript>().towerid == 1)
                        {
                            this.gameObject.GetComponent<TowerScript>().abilityOn = true;
                            this.gameObject.GetComponent<TowerScript>().towerxability(1);
                        }
                        this.gameObject.GetComponent<TowerScript>().AoeImage.SetActive(true);

                    }
                    else
                    {
                        normalColliderObs.enabled = true;
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

    IEnumerator DecayPriority()
    {
        canDecay = false;
        priority -= priorityDecay;
        if (basePriority > priority)
        {
            priority= basePriority;
        }
        yield return new WaitForSecondsRealtime(1f);
        canDecay = true;
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
                if (stunHero)
                {
                    GameObject.Find("GameManager").GetComponent<GameManagerScript>().StunPlayer();
                }
                TriggerDestroy();
            }
        }
    }

    IEnumerator obsDestroyed()
    {
        //asdf
        if (debriPrefab != null) 
        {
            GameObject spawnedDebris = Instantiate(debriPrefab, this.transform.position, this.transform.rotation);
        }
        if (moneyPrefab != null)
        {
            GameObject spawnedMoney = Instantiate(moneyPrefab, this.transform.position, this.transform.rotation);
            if (insuranceValue > 0 && insuranceValue < 50)
            {
                spawnedMoney.GetComponent<Animator>().SetTrigger("playAnim1");
            }
            if (insuranceValue > 50 && insuranceValue < 100)
            {
                spawnedMoney.GetComponent<Animator>().SetTrigger("playAnim2");
            }
            if (insuranceValue > 100)
            {
                spawnedMoney.GetComponent<Animator>().SetTrigger("playAnim3");
            }
            Destroy(spawnedMoney, 2f);

        }

        Destroy(this.gameObject);
        destroySound.Play();

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
