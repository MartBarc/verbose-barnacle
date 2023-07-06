using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckScript : MonoBehaviour
{
    bool isTopdown;
    GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
        isTopdown = GameObject.Find("GameManager").GetComponent<GameManagerScript>().isTopdown;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTopdown)
        {
            return;
        }
        else 
        {
            if (collision.tag == "Ground")
            {
                player.GetComponent<PlayerScript>().isGrounded = true;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isTopdown)
        {
            return;
        }
        else
        {
            if (collision.tag == "Ground")
            {
                player.GetComponent<PlayerScript>().isGrounded = false;
            }
        }

    }
}
