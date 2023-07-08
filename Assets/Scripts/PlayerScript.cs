using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    public float hitPoints;
    public float maxHitPoints = 10f;
    public bool isAlive = true;
    public bool isFacingRight = true;
    public HealthbarScript healthbar;

    public Rigidbody2D weaponRb;
    public GameObject weaponImage;
    [SerializeField] public GameObject gameOverText;
    private GameObject gameover;
    public bool isGrounded = false; 
    public float jumpForce = 10f;
    public float gravity = 2f;

    Vector2 movement;
    Vector2 moveWeapon;
    Vector2 mousepos;

    public float OGmoveSpeed;

    //[SerializeField] public GameObject playerWeap;

    void Start()
    {
        hitPoints = maxHitPoints;
        healthbar.SetHealth(hitPoints, maxHitPoints);
        rb.gravityScale = 0f;
        OGmoveSpeed = moveSpeed;
    }

    void Update()
    {
        if (!isAlive)
        {
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", 0);
            animator.SetFloat("Speed", 0); 
            return; 
        }

        //Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        
        if (movement.x > 0)
        {
            isFacingRight = true;
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            healthbar.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            //flip weapon hand sprite here
        }
        if (movement.x < 0)
        {
            isFacingRight = false;
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
            healthbar.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
            //flip weapon hand sprite here
        }

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        //if ((Input.GetButtonDown("Fire2") || Input.GetMouseButton(1)) && rollCooldownBool)
        //{
        //    //roll animation
        //    rollCooldownBool = false;
        //    animator.SetTrigger("playerRollTrig");

        //    StartCoroutine(rollAbility());
        //    StartCoroutine(rollCoolDownWait(rollCoolDown));
        //}
    }

    void FixedUpdate()
    {
        if (!isAlive)
        { return; }
        //Movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        

        //weapon movement
        moveWeapon = new Vector2(rb.position.x, rb.position.y - 0.2f);
        weaponRb.MovePosition(moveWeapon + movement * moveSpeed * Time.fixedDeltaTime);

        if (!gameObject.GetComponent<WeaponController>().currentWeapon.GetComponent<WeaponData>().weaponMeleRanged)
        {
            //ranged
            Vector2 lookDir = mousepos - weaponRb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            weaponRb.rotation = angle;

        }
        else
        {
            //mele
            Vector2 lookDir = mousepos - weaponRb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            weaponRb.rotation = angle;

            //GameObject.Find("player/gun/gunImage").transform.position = weaponRb.position;
            //GameObject.Find("player/gun/gunImage").transform.rotation = Quaternion.identity;
        }

    }

    public void TakeHit(float damage)
    {
        hitPoints -= damage;
        healthbar.SetHealth(hitPoints, maxHitPoints);
        if (hitPoints <= 0 && isAlive)
        {
            StartCoroutine(playerDied());
            isAlive = false;
            hitPoints = 0;
            //heartImageHandler();
        }
        //heartImageHandler();
    }

    IEnumerator playerDied()
    {
        animator.SetTrigger("PlayerDieTrig");

        yield return new WaitForSecondsRealtime(5f);

        gameObject.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        //Application.Quit();
        //do other things when die
    }


}
