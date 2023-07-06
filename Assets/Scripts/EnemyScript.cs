using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float speed = 5;
    public float hitPoints;
    public float maxHitPoints = 5;
    public int attackDamage = 1;
    public float shootDistance = 6f;
    public bool canbeHurt = true;
    public bool isMeleEnemy; //1=mele, 0=ranged
    public float attackDistance = 1f;
    public bool canAttack = true;
    public Transform moveTo;
    public bool waitToSpawn = true;
    public bool isAlive = true;
    public HealthbarScript healthbar;
    public bool isWalking = false;
    public float attackDelay = 1f; //How long to wait before able to attack again
    public float beforeAttackDelay = 0.15f;//how long to wait to attack after it gets to you
    public float beforeDamageDelay = 0.3f;//how long to wait to deal damage after starting animation
    public int id = -1;//301 = orc (EnemyMele), 302 = skele (EnemyRanged)
    public float bulletForce = 10f;
    public Animator EnemyAnimation;
    [SerializeField] private BulletScriptEnemy projectilePrefab;
    [SerializeField] private MeleHitboxEnemy hitboxPrefab;
    public Rigidbody2D weaponRb;
    public Transform firepos;
    private bool isTopdown; //taken from gamemanager, true = topdown, false = 2d side scroller
    public float gravity = 2f;

    void Start()
    {
        hitPoints = maxHitPoints;
        healthbar.SetHealth(hitPoints, maxHitPoints);
        //healthbar.SetHealth(hitPoints, maxHitPoints);
        //healthbar.transform.position = new Vector3(0f, healthbarOffset);
        //MoveToTransform = GameObject.Find("player").transform;


        //attackSound = GameObject.Find("Sounds/enemyAttackNoise").GetComponent<AudioSource>();

        isTopdown = GameObject.Find("GameManager").GetComponent<GameManagerScript>().isTopdown;
        if (isTopdown)
        {
            this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
        }
        else
        {
            this.gameObject.GetComponent<Rigidbody2D>().gravityScale = gravity;
        }

        if (id == 301)//orc sprite was facing wrong way
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        
        //debug
        moveTo = GameObject.Find("Player").transform;

        StartCoroutine(WaitToAttack());
    }

    public void Update()
    {
        this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        if (isAlive)
        {
            if (!isMeleEnemy && canAttack)//ranged
            {
                if (Vector2.Distance(transform.position, moveTo.position) < shootDistance)
                {
                    isWalking = false;
                    EnemyAnimation.SetBool("isWalking", false);
                    canAttack = false;
                    PlayerScript player = moveTo.gameObject.GetComponent<PlayerScript>();
                    StartCoroutine(rangeAttackCooldown(player, moveTo));
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, moveTo.position, speed * Time.deltaTime);
                    isWalking = true;
                    EnemyAnimation.SetBool("isWalking", true);
                }
                return;
            }
            if (isMeleEnemy)//mele
            {
                if (Vector2.Distance(transform.position, moveTo.position) < attackDistance)
                {
                    isWalking = false;
                    EnemyAnimation.SetBool("isWalking", false);
                    PlayerScript player = moveTo.gameObject.GetComponent<PlayerScript>();
                    if (canAttack)
                    {
                        canAttack = false;
                        StartCoroutine(meleAttackCooldown(player));
                    }
                    return;
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, moveTo.position, speed * Time.deltaTime);
                    isWalking = true;
                    EnemyAnimation.SetBool("isWalking", true);
                }

                Vector2 targetPos = moveTo.position;
                Vector2 lookDir = targetPos - weaponRb.position;
                float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
                weaponRb.rotation = angle;
            }

            Vector3 Dir = gameObject.transform.position - moveTo.position;
            if (Dir.x > 0)
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                healthbar.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                weaponRb.position = new Vector3(0f, -0.75f, 0);
            }
            if (Dir.x < 0)
            {
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
                healthbar.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
                weaponRb.position = new Vector3(0f, -0.75f, 0);
            }
        }
    }

    public void TakeHit(float damage)
    {
        if (canbeHurt)
        {
            canbeHurt = false;
            StartCoroutine(damageFromPlayerCooldown());
            hitPoints -= damage;
            healthbar.SetHealth(hitPoints, maxHitPoints);
            //Debug.Log("hitPoints = " + hitPoints + "/" + maxHitPoints);


            if (hitPoints <= 0 && isAlive)
            {
                GameObject.Find("GameManager").GetComponent<GameManagerScript>().addEnemyScore();
                isAlive = false;
                //Destroy(gameObject);
                EnemyAnimation.SetTrigger("EnemyDieTrig");
                if (id == 301 || id == 302)  //301 = orc, 302 = skele
                {
                    Destroy(gameObject, 2f);
                }
            }
        }
    }

    IEnumerator damageFromPlayerCooldown()
    {
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        //maybe play flinch animation
        yield return new WaitForSecondsRealtime(0.1f);
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        canbeHurt = true;
    }

    private IEnumerator WaitToAttack() // go to zero
    {
        yield return new WaitForSeconds(2f);
        waitToSpawn = false;
    }

    IEnumerator rangeAttackCooldown(PlayerScript player, Transform moveTo)
    {
        yield return new WaitForSecondsRealtime(beforeAttackDelay);
        if (isAlive)
        {
            playAttackAnim();
            if (id == 300)  //300 = frog, 301 = skele, 302 = orc, 303 = zombie, 304 = imp
            {
                GameObject.Find("Sounds/frogAttackNoise").GetComponent<AudioSource>().Play();
            }
            yield return new WaitForSecondsRealtime(beforeDamageDelay);
            Shoot(moveTo);
        }
        yield return new WaitForSecondsRealtime(attackDelay);
        canAttack = true;
    }

    IEnumerator meleAttackCooldown(PlayerScript player)
    {
        yield return new WaitForSecondsRealtime(beforeAttackDelay);
        if (player != null)
        {
            Vector2 targetposition = moveTo.position;
            Vector2 lookDir = targetposition - weaponRb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            weaponRb.rotation = angle;
            playAttackAnim();
            if (id == 300)  //300 = frog, 301 = skele, 302 = orc, 303 = zombie, 304 = imp
            {
                GameObject.Find("Sounds/frogAttackNoise").GetComponent<AudioSource>().Play();
            }
            yield return new WaitForSecondsRealtime(beforeDamageDelay);
            //maybe check if player is still in range of attack to deal damage?
            //or maybe turn into a melehitbox/bullet thing
            ///player.TakeHit(attackDamage);//testing above comments, bring this back if not working
            Swing(player.gameObject.transform);
        }
        yield return new WaitForSecondsRealtime(attackDelay);
        canAttack = true;
        //canShoot = true;
    }

    public void playAttackAnim()
    {
        EnemyAnimation.SetTrigger("EnemyMeleAttackTrig");
    }

    public void playDieAnim()
    {
        EnemyAnimation.SetTrigger("EnemyDieTrig");
    }

    public void Shoot(Transform target)
    {
        BulletScriptEnemy projectile = Instantiate(projectilePrefab, this.transform.position, Quaternion.identity);

        Physics2D.IgnoreCollision(projectile.transform.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        Vector3 dir = target.transform.position - transform.position;
        dir = dir.normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward); //

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.AddForce(dir * bulletForce, ForceMode2D.Impulse);
    }

    public void Swing(Transform target)
    {
        //Vector2 targetposition = target.position;
        //Vector2 lookDir = targetposition - weaponRb.position;
        //float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        //weaponRb.rotation = angle;

        MeleHitboxEnemy projectile = Instantiate(hitboxPrefab, firepos.position, firepos.rotation);
        projectile.damage = attackDamage;
        Physics2D.IgnoreCollision(projectile.transform.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        projectile.GetComponent<MeleHitboxEnemy>().knockBack = hitboxPrefab.knockBack;
    }

}
