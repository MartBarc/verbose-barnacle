using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obs : MonoBehaviour
{
    public int priority = 0;

    public int health = 100;

    [SerializeField] public int insuranceValue = 0;

    //public GameObject obstacle;

    public void TriggerDestroy()
    {
        priority = 0;

        this.gameObject.GetComponent<Rigidbody2D>().drag = 0f;
        this.gameObject.GetComponent<Rigidbody2D>().mass = 4f;

        StartCoroutine(obsDestroyed());
    }

    public void TakeHit(int help)
    {
        health -= help;

        if (health <= 0)
        {
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
        yield return new WaitForSecondsRealtime(2f);

        //Spawn destruction

        Destroy(this.gameObject);
    }
}
