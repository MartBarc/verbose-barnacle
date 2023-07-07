using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeroBody : MonoBehaviour
{
    //public float hitPoints;
    //public float maxHitPoints = 5;
    //public int attackDamage = 1;
    //public float shootDistance = 6f;
    //public bool canbeHurt = true;
    ////public bool isMeleEnemy; //1=mele, 0=ranged
    //public float attackDistance = 1f;
    //public bool canAttack = true;
    ////public Transform moveTo;
    //public bool waitToSpawn = true;
    //public bool isAlive = true;
    //public HealthbarScript healthbar;
    //public bool isWalking = false;
    //public float attackDelay = 1f; //How long to wait before able to attack again
    //public float beforeAttackDelay = 0.15f;//how long to wait to attack after it gets to you
    //public float beforeDamageDelay = 0.3f;//how long to wait to deal damage after starting animation
    //public int id = -1;//301 = orc (EnemyMele), 302 = skele (EnemyRanged)
    //public float bulletForce = 10f;
    //public Animator EnemyAnimation;
    //[SerializeField] private BulletScriptEnemy projectilePrefab;
    //[SerializeField] private MeleHitboxEnemy hitboxPrefab;
    //public Rigidbody2D weaponRb;
    //public Transform firepos;

    void Start()
    {

    }

    public void Update()
    {
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.z * -1.0f);
    }
}
