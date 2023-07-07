using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject currentWeapon;
    public GameObject bulletPrefab;
    public AudioSource attackSound;
    //public AudioSource explosionSound1;
    //public AudioSource hitSound1;
    //public bool explosionSoundPlaying1;
    public float reloadTime = 1.6f;
    public bool reloading;
    public int ammoMax;
    public int ammo;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI currentWeaponText;
    public int currentWeaponTracker;

    [SerializeField]
    public List<GameObject> CurrentWeaponList = new List<GameObject>();

    private void Start()
    {
        currentWeapon = CurrentWeaponList[0];
        ammo = currentWeapon.GetComponent<WeaponData>().ammoMax;
        this.gameObject.GetComponent<PlayerScript>().weaponImage.GetComponent<SpriteRenderer>().sprite = currentWeapon.GetComponent<WeaponData>().inHandSprite;
        getWeaponSound();
        //currentWeaponText.text = "1";
        //weaponTrackerImage.sprite = currentWeapon.GetComponent<WeaponData>().UISprite;
        currentWeaponTracker = 1;
        updateAmmo();
    }


    private void Update()
    {
        if (ammo == 0)
        {
            reloading = true;
            StartCoroutine(reloadwait(reloadTime));
            ammo = currentWeapon.GetComponent<WeaponData>().ammoMax;
            updateAmmo();
        }
    }

    public void getWeaponSound()
    {
        if (currentWeapon.GetComponent<WeaponData>().weaponId == 0) //laser
        {
            attackSound = GameObject.Find("Sounds/LaserSound").GetComponent<AudioSource>();
            //explosionSound1 = GameObject.Find("Sounds/explosionSound").GetComponent<AudioSource>();
            //hitSound1 = GameObject.Find("Sounds/explosionSound (5)").GetComponent<AudioSource>();
        }
        if (currentWeapon.GetComponent<WeaponData>().weaponId == 1) //sword
        {
            attackSound = GameObject.Find("Sounds/SwingSound").GetComponent<AudioSource>();
            //explosionSound1 = GameObject.Find("Sounds/explosionSound").GetComponent<AudioSource>();
            //hitSound1 = GameObject.Find("Sounds/explosionSound (5)").GetComponent<AudioSource>();
        }
        if (currentWeapon.GetComponent<WeaponData>().weaponId == 3) //bat
        {
            attackSound = GameObject.Find("Sounds/SwingSound").GetComponent<AudioSource>();
            //explosionSound1 = GameObject.Find("Sounds/explosionSound").GetComponent<AudioSource>();
            //hitSound1 = GameObject.Find("Sounds/explosionSound (5)").GetComponent<AudioSource>();
        }
    }

    public void updateAmmo()
    {

        bulletPrefab = currentWeapon.GetComponent<WeaponData>().bulletPrefab;
        if (bulletPrefab.GetComponent<BulletScript>())
        {
            bulletPrefab.GetComponent<BulletScript>().damage = currentWeapon.GetComponent<WeaponData>().weaponDamage;
        }
        //if (bulletPrefab.GetComponent<meleAttack>())
        //{
        //    bulletPrefab.GetComponent<meleAttack>().damage = currentWeapon.GetComponent<WeaponData>().weaponDamage;
        //}

        ammoMax = currentWeapon.GetComponent<WeaponData>().ammoMax;
        //ammoText.text = ammo + " / " + ammoMax;//add this back when i do ui

        //GameObject.Find("crossHair").GetComponent<cursorFollow>().setCrossHair(currentWeapon.GetComponent<WeaponData>().CrossHair);
    }

    IEnumerator reloadwait(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        reloading = false;
    }
}
