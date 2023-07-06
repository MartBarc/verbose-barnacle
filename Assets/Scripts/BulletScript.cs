using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public GameObject player;
    public GameObject hitEffect;
    public float damage = 1f;
    public bool playExplosion = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.ToString() == "Player")
        {
            return;
        }
        else 
        {
            Debug.Log("Bullet collided with " + collision.gameObject.name.ToString());
            if (playExplosion)
            {
                GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
                //player.GetComponent<weaponController>().playExplosionSound();//add these scripts later
                Destroy(effect, 1f);
            }
            else
            {
                //player.GetComponent<weaponController>().playHitSound();//add these scripts later
            }

            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Destroy(gameObject, 5f);
    }
}
