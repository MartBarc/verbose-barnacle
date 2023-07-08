using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class HeroController : MonoBehaviour
{
    // Important parameters
    public int id = -1;//301 = orc (EnemyMele), 302 = skele (EnemyRanged)

    // Movement
    [SerializeField] public AIPath pathing;

    //Health
    [SerializeField] public HealthbarScript healthbar;
    public float hitPoints;
    private float maxHitPoints = 5;
    public bool isAlive = true;

    //Combat
    public bool canbeHurt = true;
    private int attackDamage = 1;
    private float shootDistance = 6f;
    private float attackDistance = 1f;
    
    private float attackDelay = 1f; //How long to wait before able to attack again
    private float beforeAttackDelay = 0.15f;//how long to wait to attack after it gets to you
    private float beforeDamageDelay = 0.3f;//how long to wait to deal damage after starting animation

    public Transform firepos;

    public float bulletForce = 10f;

    [SerializeField] public Rigidbody2D weaponRb;

    [SerializeField] private BulletScriptEnemy projectilePrefab;
    [SerializeField] private MeleHitboxEnemy hitboxPrefab;

    // Targetting list
    [SerializeField] public GameObject player;
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

        hitPoints = maxHitPoints;
        healthbar.SetHealth(hitPoints, maxHitPoints);

        StartCoroutine(WaitToAttack());
        currentSpeed = (int)gameObject.GetComponent<AIPath>().maxSpeed;
        OGSpeed = currentSpeed;
    }


    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            RecalcTargets();
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
        if (other.gameObject.GetComponent<Obs>() != null)
        {
            //Debug.Log($"{other.gameObject.name} found outside in player aggro range");
        }
    }

    public void AddNewObs(Obs newObs)
    {
        for (int i = 0; i < obsList.Count; i++)
        {
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

        for (int i = 0; i < obsList.Count; i++)
        {
            if (obsList[i].priority > priorityValue)
            {
                priorityIndex = i;
                priorityValue = obsList[i].priority;
            }
        }

        if (priorityValue <= 0)
        {
            priorityIndex = 0;
        }

        curTarget.UpdateTargetPosition(obsList[priorityIndex].transform.position);
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
                //GameObject.Find("GameManager").GetComponent<GameManagerScript>().addEnemyScore();
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
    }

    IEnumerator meleAttackCooldown(PlayerScript player)
    {
        yield return new WaitForSecondsRealtime(beforeAttackDelay);
        if (player != null)
        {
            Vector2 targetposition = this.transform.position;
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
        Vector2 targetposition = target.position;
        Vector2 lookDir = targetposition - weaponRb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        weaponRb.rotation = angle;

        MeleHitboxEnemy projectile = Instantiate(hitboxPrefab, firepos.position, firepos.rotation);
        projectile.damage = attackDamage;
        Physics2D.IgnoreCollision(projectile.transform.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        projectile.GetComponent<MeleHitboxEnemy>().knockBack = hitboxPrefab.knockBack;
    }

    public void setNewSpeed(int speed) 
    {
        gameObject.GetComponent<AIPath>().maxSpeed = speed;
    }

    public void setOGSpeed()
    {
        gameObject.GetComponent<AIPath>().maxSpeed = OGSpeed;
    }
}
