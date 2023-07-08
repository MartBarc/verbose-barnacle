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
    public float newSpeed = 10f;

    private void Start()
    {
        buyText.SetText("Press E to purchase " + this.gameObject.name + " for $" + price + ".");
        buyText.gameObject.SetActive(false);
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
        if (GameObject.Find("Hero_peter(Clone)"))//change this to final hero name
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
                    GameObject.Find("Player").gameObject.GetComponent<PlayerScript>().moveSpeed = newSpeed;
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
        if (collision.gameObject.tag == "Player")
        {
            if (purchased)
            {
                GameObject.Find("Player").gameObject.GetComponent<PlayerScript>().moveSpeed = newSpeed;
            }
            else 
            {
                buyText.gameObject.SetActive(true);
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (purchased)
            {
                GameObject.Find("Player").gameObject.GetComponent<PlayerScript>().moveSpeed = GameObject.Find("Player").gameObject.GetComponent<PlayerScript>().OGmoveSpeed;
            }
            else
            {
                buyText.gameObject.SetActive(false);
            }
            
        }
    }

}
