using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obs : MonoBehaviour
{
    public int id = 0;
    public int priority = 0;

    public int health = 100;

    //public GameObject obstacle;

    public void TriggerDestroy()
    {
        health = 0;
        priority = 0;
    }

    public void TriggerDamage(int help)
    {
        health -= help;
    }
}
