using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinionScript : MonoBehaviour
{
    public float speed = 5;
    public float hitPoints;
    public float maxHitPoints = 5;
    public int attackDamage = 1;
    public float shootDistance = 6f;
    public bool canbeHurt = true;
    public bool isTower; //1=tower, 0=nontower
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
    [SerializeField] private BulletScript projectilePrefab;
    public Rigidbody2D weaponRb;
    public Transform firepos;
    public float gravity = 2f;
    public bool purchaseable;
    public bool isActive;
    public TextMeshProUGUI buyText;
    public int price = 10;
    public float defaultCollider = 0.77f;
    private bool purchased = true;

    void Start()
    {
        buyText.SetText("Press E to purchase " + this.gameObject.name + " for $" + price + ".");
        buyText.gameObject.SetActive(false);
        if (purchaseable)
        {
            purchased = false;
            CircleCollider2D myCollider = transform.GetComponent<CircleCollider2D>();
            myCollider.radius = 3f;
            myCollider.isTrigger = true;
            LayerMask mask1 = LayerMask.GetMask("Minion");
            this.gameObject.GetComponent<Rigidbody2D>().excludeLayers = mask1;
            //buyText.gameObject.SetActive(false);
        }
        else 
        {
            purchased = true;
            CircleCollider2D myCollider = transform.GetComponent<CircleCollider2D>();
            myCollider.radius = defaultCollider;
            myCollider.isTrigger = false;
            LayerMask mask1 = LayerMask.GetMask("Player") | LayerMask.GetMask("Minion");
            this.gameObject.GetComponent<Rigidbody2D>().excludeLayers = mask1;
        }
        
        hitPoints = maxHitPoints;
        healthbar.SetHealth(hitPoints, maxHitPoints);
        //healthbar.SetHealth(hitPoints, maxHitPoints);
        //healthbar.transform.position = new Vector3(0f, healthbarOffset);
        //MoveToTransform = GameObject.Find("player").transform;

        //attackSound = GameObject.Find("Sounds/enemyAttackNoise").GetComponent<AudioSource>();
        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;


        if (id == 301 || id == 100)//orc sprite was facing wrong way
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }

        //debug
        if (GameObject.Find("Hero"))//change this to final hero name
        {
            moveTo = GameObject.Find("Hero").transform;//change this to final hero name
        }
        else 
        {
            moveTo = null;
        }
        

        StartCoroutine(WaitToAttack());
    }

    public void Update()
    {
        this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        if (GameObject.Find("Hero(Clone)"))//change this to final hero name
        {
            moveTo = GameObject.Find("Hero(Clone)").transform;//change this to final hero name
        }
        else
        {
            moveTo = null;
        }
        if (!isActive)
        {
            EnemyAnimation.SetBool("isActive", false);
            if (purchaseable)
            {
                //if gamemanager.gamestarted = true; 
                //destroy this gameobject
                if (GameObject.Find("GameManager").GetComponent<GameManagerScript>().GameStarted)
                {
                    Destroy(gameObject);
                }
                purchased = false;
                //buyText.SetText("Press E to purchase " + this.gameObject.name + " for $" + price + ".");
                CircleCollider2D myCollider = transform.GetComponent<CircleCollider2D>();
                myCollider.radius = 3f;
                myCollider.isTrigger = true;
                LayerMask mask1 = LayerMask.GetMask("Minion");
                this.gameObject.GetComponent<Rigidbody2D>().excludeLayers = mask1;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    purchased = true;
                    purchaseable = false;
                    isActive = true;
                    GameObject.Find("GameManager").gameObject.GetComponent<GameManagerScript>().ScoreSub(price);
                    myCollider.radius = defaultCollider;
                    myCollider.isTrigger = false;
                    mask1 = LayerMask.GetMask("Player") | LayerMask.GetMask("Minion");
                    this.gameObject.GetComponent<Rigidbody2D>().excludeLayers = mask1;
                }
            }
            else
            {
                CircleCollider2D myCollider = transform.GetComponent<CircleCollider2D>();
                myCollider.radius = defaultCollider;
                myCollider.isTrigger = false;
                LayerMask mask1 = LayerMask.GetMask("Player") | LayerMask.GetMask("Minion");
                this.gameObject.GetComponent<Rigidbody2D>().excludeLayers = mask1;
            }
            return;
        }
        else 
        {
            EnemyAnimation.SetBool("isActive", true);
        }



        if (isAlive && moveTo != null)
        {
            if (isTower)//ranged
            {
                if (canAttack)
                {
                    canAttack = false;
                    StartCoroutine(rangeAttackCooldown(moveTo));
                }
                
                return;
            }
            if (!isTower)//mele
            {
                if (Vector2.Distance(transform.position, moveTo.position) < attackDistance)
                {
                    isWalking = false;
                    EnemyAnimation.SetBool("isWalking", false);
                    HeroController target = moveTo.gameObject.GetComponent<HeroController>();
                    if (canAttack)
                    {
                        canAttack = false;
                        StartCoroutine(meleAttackCooldown(target));
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
                isAlive = false;
                //Destroy(gameObject);
                EnemyAnimation.SetTrigger("EnemyDieTrig");
                if (id == 301 || id == 302 || id == 100)  //301 = orc, 302 = skele, 100 = batminion
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

    IEnumerator rangeAttackCooldown(Transform moveTo)
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

    IEnumerator meleAttackCooldown(HeroController target)
    {
        Debug.Log("Bat attack hero");
        yield return new WaitForSecondsRealtime(beforeAttackDelay);
        if (target != null)
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
            //target.TakeHit(attackDamage);//testing above comments, bring this back if not working
            //Swing(player.gameObject.transform);

            GameObject.Find("GameManager").gameObject.GetComponent<GameManagerScript>().reduceHeroStam((int)attackDamage);

        }
        yield return new WaitForSecondsRealtime(attackDelay);
        canAttack = true;
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
        BulletScript projectile = Instantiate(projectilePrefab, this.transform.position, Quaternion.identity);

        Physics2D.IgnoreCollision(projectile.transform.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        Vector3 dir = target.transform.position - transform.position;
        dir = dir.normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward); //

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.AddForce(dir * bulletForce, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            buyText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            buyText.gameObject.SetActive(false);
        }
    }

}
