using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obs : MonoBehaviour
{
    public int priority = 0;

    public int health = 100;

    //public GameObject obstacle;

    public void TriggerDestroy()
    {
        priority = 0;

        StartCoroutine(obsDestroyed());
    }

    public void TakeHit(int help)
    {
        Debug.Log("Hero did damage");
        health -= help;

        if (health <= 0)
        {
            TriggerDestroy();
        }
    }

    IEnumerator obsDestroyed()
    {
        Debug.Log("Hero destroyed obs");
        yield return new WaitForSecondsRealtime(1f);

        //Spawn destruction

        Destroy(this.gameObject);
    }
}
