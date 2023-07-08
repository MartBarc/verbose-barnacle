using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerScript : MonoBehaviour
{
    public float hitPoints;
    public float maxHitPoints = 5;
    public bool isAlive = true;
    public HealthbarScript healthbar;
    public bool purchaseable;
    public bool isActive;
    public TextMeshProUGUI buyText;
    public int price = 10;
    public float defaultCollider = 10f;
    private bool purchased = true;
    public int towerid = -1;//1=villan speed, 2=hero slow
    public GameObject AoeImage;
    public bool abilityOn = false;
    public float newPlayerSpeed = 10f;
    public int newHeroSpeed = 2;

    private void Start()
    {
        buyText.SetText("Press E to purchase " + this.gameObject.name + " for $" + price + ".");
        buyText.gameObject.SetActive(false);
        abilityOn = false;
        if (purchaseable)
        {
            purchased = false;
            CircleCollider2D myCollider = transform.GetComponent<CircleCollider2D>();
            myCollider.radius = 3f;
            //buyText.gameObject.SetActive(false);
        }
        else
        {
            purchased = true;
            CircleCollider2D myCollider = transform.GetComponent<CircleCollider2D>();
            myCollider.radius = defaultCollider;
        }

        hitPoints = maxHitPoints;
        healthbar.SetHealth(hitPoints, maxHitPoints);
    }

    private void Update()
    {
        if (GameObject.Find("Hero(Clone)"))//change this to final hero name
        {
            
        }
        else
        {
            
        }

        if (!isActive)
        {
            if (purchaseable)
            {
                //if gamemanager.gamestarted = true; 
                //destroy this gameobject
                if (GameObject.Find("GameManager").GetComponent<GameManagerScript>().GameStarted)
                {
                    Destroy(gameObject);
                }
                purchased = false;
                //buyText.SetText("Press E to purchase " + this.gameObject.name + " for $" + price + ".");
                CircleCollider2D myCollider = transform.GetComponent<CircleCollider2D>();
                myCollider.radius = 3f;
                AoeImage.SetActive(false);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    purchased = true;
                    purchaseable = false;
                    isActive = true;
                    GameObject.Find("GameManager").gameObject.GetComponent<GameManagerScript>().ScoreSub(price);
                    myCollider.radius = defaultCollider;
                    AoeImage.SetActive(true);
                    buyText.SetText("");
                    if (towerid == 1) 
                    {
                        towerxability(towerid); 
                    }
                    
                }
            }
            else
            {
                CircleCollider2D myCollider = transform.GetComponent<CircleCollider2D>();
                myCollider.radius = defaultCollider;
            }
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!purchased)
        {
            if (collision.gameObject.tag == "Player")
            {
                buyText.gameObject.SetActive(true);
            }
            return;
        }
        if ((collision.gameObject.tag == "Player" && towerid == 1) || (collision.gameObject.tag == "Hero" && towerid == 2) && !abilityOn)
        {
            Debug.Log("Hero entered tower");
            towerxability(towerid);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!purchased)
        {
            if (collision.gameObject.tag == "Player") 
            {
                buyText.gameObject.SetActive(false);
            }
            return;
        }
        if ((collision.gameObject.tag == "Player" && towerid == 1) || (collision.gameObject.tag == "Hero" && towerid == 2) && abilityOn)
        {
            Debug.Log("Hero left tower");
            towerxability(towerid);
        }
    }

    void towerxability(int id) 
    {
        if (abilityOn)
        {
            abilityOn = false;
            if (id == 1)
            {
                GameObject.Find("Player").gameObject.GetComponent<PlayerScript>().moveSpeed = GameObject.Find("Player").gameObject.GetComponent<PlayerScript>().OGmoveSpeed;
            }
            if (id == 2)
            {
                GameObject.Find("Hero(Clone)").gameObject.GetComponent<HeroController>().setOGSpeed();
            }
            if (id == 3)
            {

            }
        }
        else 
        {
            abilityOn = true;
            if (id == 1)
            {
                GameObject.Find("Player").gameObject.GetComponent<PlayerScript>().moveSpeed = newPlayerSpeed;
            }
            if (id == 2)
            {
                GameObject.Find("Hero(Clone)").gameObject.GetComponent<HeroController>().setNewSpeed(newHeroSpeed);
            }
            if (id == 3)
            {

            }
        }
    }
}
