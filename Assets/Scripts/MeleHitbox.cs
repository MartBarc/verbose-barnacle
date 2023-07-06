using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleHitbox : MonoBehaviour
{
    public GameObject player;
    public GameObject hitEffect;
    public float damage = 1f;
    public float knockBack;
    //public Animator weaponAnimator;
    public float selfDestroyTime = 0.3f;

    void Start()
    {
        Destroy(gameObject, selfDestroyTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyScript>().TakeHit(damage);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * knockBack);
        }
    }
}
