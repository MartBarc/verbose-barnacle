using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class HeroController : MonoBehaviour
{
    // Movement
    [SerializeField] public AIPath pathing;

    // Spawn location // Game state
    public bool isRoundOver = false;
    public GameObject spawner;

    //Health
    [SerializeField] public HealthbarScript healthbar;
    public float stam;
    public float maxStam = 5;

    //Combat
    [SerializeField] public HeroHitbox hitbox;
    public bool canbeHurt = true;
    public bool canAttack = true;
    public int attackDamage = 110;
    //private float shootDistance = 6f;
    private float attackDistance = 2f;
    public GameObject weaponObj;
    
    private float attackDelay = 1f; //How long to wait before able to attack again
    private float beforeAttackDelay = 0.15f;//how long to wait to attack after it gets to you
    private float beforeDamageDelay = 0.3f;//how long to wait to deal damage after starting animation

    public Transform firepos;

    public float bulletForce = 10f;
    public Rigidbody2D rb;

    //[SerializeField] private BulletScriptEnemy projectilePrefab;
    [SerializeField] private MeleHitboxEnemy hitboxPrefab;

    // Targetting list
    [SerializeField] public GameObject player;
    public int playerPriority = 0;
    [SerializeField] public TargetPoint curTarget;
    public List<Obs> obsList;
    public int priorityIndex = 0;
    public int priorityValue = 0;
    public int currentSpeed;
    public int OGSpeed;

    // Animation
    public Animator EnemyAnimation;

    // Start is called before the first frame update
    void Start()
    {
        obsList = new List<Obs> { player.GetComponent<Obs>() };
        //player = GameObject.Find("Player").gameObject;
        //curTarget = GameObject.Find("HeroTarget").gameObject.GetComponent<TargetPoint>();

        stam = maxStam;
        healthbar.SetHealth(stam, maxStam);

        StartCoroutine(WaitToAttack());
        currentSpeed = (int)gameObject.GetComponent<AIPath>().maxSpeed;
        OGSpeed = currentSpeed;
    }


    // Update is called once per frame
    void Update()
    {
        if (isRoundOver)
        {
            // Hero needs to start going back to entrance
            curTarget.UpdateTargetPosition(spawner.transform.position);
        }
        else
        {
            RecalcTargets();

            
            

            // Attack object?
            if (Vector2.Distance(transform.position, curTarget.transform.position) < attackDistance)
            {
                //gameObject.GetComponent<AIPath>().canMove = false;
                if (canAttack)
                {
                    //mele
                    Vector2 lookDir = curTarget.transform.position - transform.position;
                    float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
                    gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); //
                    EnemyAnimation.SetBool("isWalking", false);
                    canAttack = false;
                    ReduceHeroStam(1f);
                    StartCoroutine(meleAttackCooldown());
                }
            }
            else 
            {
                //gameObject.GetComponent<AIPath>().canMove = true;
                EnemyAnimation.SetBool("isWalking", true);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Obs>() != null)
        {
            AddNewObs(other.gameObject.GetComponent<Obs>());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("Lost object");
        if (other.gameObject.GetComponent<Obs>() != null)
        {
            for (int i = 0; i < obsList.Count; i++)
            {
                if (obsList[i] == null)
                {
                    obsList.RemoveAt(i);
                }
            }
        }
    }

    public void ReduceHeroStam(float value)
    {
        stam -= value;
        //if (stam <= 0f)
        //{
        //    HeroStam = 0;
        //}
        //int newHeroStam = (int)HeroStam;
        //HeroStamBar.value = newHeroStam;
        //StamText.text = HeroStamBar.value + " / " + HeroStamBar.maxValue;
    }

    public void AddNewObs(Obs newObs)
    {
        for (int i = 0; i < obsList.Count; i++)
        {
            if (obsList[i] == null)
            {
                obsList.RemoveAt(i);
                continue;
            }

            if (obsList[i].gameObject.GetInstanceID() == newObs.gameObject.GetInstanceID()) //already exists
            {
                return;
            }
        }

        //Debug.Log($"{newObs.gameObject.name} found in player aggro range");
        obsList.Add(newObs);
    }

    public void RecalcTargets()
    {
        priorityValue = 0;

        // Calc distance to player and adjust his value
        if (player != null)
        {
            playerPriority = (int)Vector2.Distance(player.transform.position, this.transform.position) * 5;
        }
        else
        {
            Debug.Log("Player dead!!!");
            return;
        }

        // Update player priority
        obsList[0].priority = playerPriority;

        for (int i = 0; i < obsList.Count; i++) // Search for heighest priority value and start targetting
        {
            if (obsList[i].priority > priorityValue)
            {
                priorityIndex = i;
                priorityValue = obsList[i].priority;
            }
        }

        if (priorityValue <= 0) // If priority 0 start targetting player index (0)
        {
            priorityIndex = 0;
        }

        if (curTarget != null || obsList[0] != null) // Update target pointer
        {
            curTarget.UpdateTargetPosition(obsList[priorityIndex].transform.position);
            return;
        }
    }

    public void TakeHit(float damage)
    {
        if (canbeHurt)
        {
            canbeHurt = false;
            StartCoroutine(damageFromPlayerCooldown());
            stam -= damage;
            healthbar.SetHealth(stam, maxStam);

            if (stam <= 0)
            {
                isRoundOver = false;
                EnemyAnimation.SetTrigger("EnemyDieTrig");
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
    }

    IEnumerator meleAttackCooldown()
    {
        weaponObj.SetActive(false);
        yield return new WaitForSecondsRealtime(beforeAttackDelay);
        if (player != null)
        {
            Vector2 targetposition = hitbox.transform.position;
            //Vector2 lookDir = targetposition - weaponRb.position;
            //float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            //weaponRb.rotation = angle;
            playAttackAnim();

            yield return new WaitForSecondsRealtime(beforeDamageDelay);
            //maybe check if player is still in range of attack to deal damage?
            //or maybe turn into a melehitbox/bullet thing
            ///player.TakeHit(attackDamage);//testing above comments, bring this back if not working
            Swing(player.gameObject.transform);
        }
        yield return new WaitForSecondsRealtime(attackDelay);
        canAttack = true;
        weaponObj.SetActive(true);
    }

    public void Swing(Transform target)
    {
        //Vector2 targetposition = target.position;
        Vector2 lookDir = hitbox.transform.position;//targetposition - weaponRb.position;
        //float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        //weaponRb.rotation = angle;

        MeleHitboxEnemy projectile = Instantiate(hitboxPrefab, hitbox.transform.position, hitbox.transform.rotation);
        projectile.damage = attackDamage;
        Physics2D.IgnoreCollision(projectile.transform.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        projectile.GetComponent<MeleHitboxEnemy>().knockBack = hitboxPrefab.knockBack;
    }

    public void playAttackAnim()
    {
        EnemyAnimation.SetTrigger("EnemyMeleAttackTrig");
    }

    public void playDieAnim()
    {
        EnemyAnimation.SetTrigger("EnemyDieTrig");
    }

    //IEnumerator rangeAttackCooldown(PlayerScript player, Transform moveTo)
    //{
    //    yield return new WaitForSecondsRealtime(beforeAttackDelay);
    //    if (isAlive)
    //    {
    //        playAttackAnim();

    //        yield return new WaitForSecondsRealtime(beforeDamageDelay);
    //        Shoot(moveTo);
    //    }
    //    yield return new WaitForSecondsRealtime(attackDelay);
    //}

    //public void Shoot(Transform target)
    //{
    //    BulletScriptEnemy projectile = Instantiate(projectilePrefab, this.transform.position, Quaternion.identity);

    //    Physics2D.IgnoreCollision(projectile.transform.GetComponent<Collider2D>(), GetComponent<Collider2D>());

    //    Vector3 dir = target.transform.position - transform.position;
    //    dir = dir.normalized;

    //    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    //    projectile.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward); //

    //     projectile.GetComponent<MeleHitboxEnemy>().knockBack = hitboxPrefab.knockBack;
    // }

    public void setNewSpeed(int speed) 
    {
        gameObject.GetComponent<AIPath>().maxSpeed = speed;
    }

    public void setOGSpeed()
    {
        gameObject.GetComponent<AIPath>().maxSpeed = OGSpeed;
    }
}
