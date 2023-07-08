using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obs : MonoBehaviour
{
    public int priority = 0;

    public int health = 100;

    [SerializeField] public int insuranceValue = 0;

    // Dustruction
    [SerializeField] public GameObject debriPrefab;
    [SerializeField] public float rightMod = 0;
    [SerializeField] public float upMod = 0;

    //public void Start()
    //{
    //    debriPrefab.GetComponent<DebrisScript>().ModDir(rightMod, upMod);
    //}

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
}
