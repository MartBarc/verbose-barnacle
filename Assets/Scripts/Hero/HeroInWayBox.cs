using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroInWayBox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Obs>() != null)
        {
            Debug.Log($"{other.gameObject.name} found in player aggro range");
            //Destroy(other.gameObject);

            other.gameObject.GetComponent<Obs>().priority += 120;
        }
    }
}
