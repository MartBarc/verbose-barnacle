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
    public bool purchased = true;
    public int towerid = -1;//1=villan speed, 2=hero slow
    public GameObject AoeImage;
    public bool abilityOn = false;
    public float newPlayerSpeed = 10f;
    public int newHeroSpeed = 2;

    private void Start()
    {
        abilityOn = false;
        hitPoints = maxHitPoints;
        healthbar.SetHealth(hitPoints, maxHitPoints);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.GetComponent<Obs>().isActive)
        {
            return;
        }
        if ((collision.gameObject.tag == "Player" && towerid == 1) || (collision.gameObject.tag == "Hero" && towerid == 2) || (collision.gameObject.tag == "Hero" && towerid == 3) && !abilityOn)
        {
            towerxability(towerid);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!gameObject.GetComponent<Obs>().isActive)
        {
            return;
        }
        if ((collision.gameObject.tag == "Player" && towerid == 1) || (collision.gameObject.tag == "Hero" && towerid == 2) || (collision.gameObject.tag == "Hero" && towerid == 3) && abilityOn)
        {
            towerxability(towerid);
        }
    }

    public void towerxability(int id) 
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
                GameObject.Find("GameManager").gameObject.GetComponent<GameManagerScript>().canCountStam = true;
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
                GameObject.Find("GameManager").gameObject.GetComponent<GameManagerScript>().canCountStam = false;
            }
        }
    }
}
