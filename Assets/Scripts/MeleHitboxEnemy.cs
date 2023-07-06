using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleHitboxEnemy : MonoBehaviour
{
    public GameObject hitEffect;
    public float damage = 1f;
    public float knockBack;
    //public Animator weaponAnimator;
    public float selfDestroyTime = 0.3f;

    void Start()
    {
        Destroy(gameObject, selfDestroyTime);
        //damage = gameObject.GetComponent<EnemyScript>().attackDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Enemy hit player");
            collision.gameObject.GetComponent<PlayerScript>().TakeHit(damage);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * knockBack);
        }
    }
}
