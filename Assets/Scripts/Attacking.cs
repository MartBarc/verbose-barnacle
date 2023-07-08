using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attacking : MonoBehaviour
{
    public Transform firepos;
    //public Transform meleFirePos;
    public Transform Gun;
    public float firerate;
    public bool fireRateWaitBool = false; //if true = shooting on cd
    public float bulletForce = 20f;
    //public bool ShootingEnabled = true;
    public Image abilityImage;

    private void Start()
    {
        //firerate = this.gameObject.GetComponent<WeaponController>().currentWeapon.GetComponent<WeaponData>().firerate;
        abilityImage.fillAmount = 0;
    }

    void Update()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManagerScript>().GameIsPaused)
        {
            return;
        }
        if (!gameObject.GetComponent<PlayerScript>().isAlive)
        {
            return;
        }

        if (fireRateWaitBool)
        {
            abilityImage.fillAmount -= 1 / firerate * Time.deltaTime;
            if (abilityImage.fillAmount <= 0)
            {
                abilityImage.fillAmount = 0;
                fireRateWaitBool = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && !fireRateWaitBool)// && ShootingEnabled)
        {
            firerate = this.gameObject.GetComponent<WeaponController>().currentWeapon.GetComponent<WeaponData>().firerate;
            fireRateWaitBool = true;
            abilityImage.fillAmount = 1;
            //StartCoroutine(fireRateWait(firerate));
            if (this.gameObject.GetComponent<WeaponController>().currentWeapon.GetComponent<WeaponData>().weaponName == "BatWeapon")
            {
                SpawnBat();
            }
            this.gameObject.GetComponent<WeaponController>().attackSound.enabled = true;
            this.gameObject.GetComponent<WeaponController>().attackSound.Play();
        }
    }

    void Shoot() //ranged attack
    {
        //Debug.Log("shot fired");
        GameObject bullet = Instantiate(this.gameObject.GetComponent<WeaponController>().bulletPrefab, firepos.position, firepos.rotation);
        bullet.transform.Rotate(0, 0, 0);
        bullet.GetComponent<BulletScript>().player = this.gameObject;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(Gun.up * this.gameObject.GetComponent<WeaponController>().currentWeapon.GetComponent<WeaponData>().bulletSpeed, ForceMode2D.Impulse);

    }

    void Swing() //mele attack
    {
        //Debug.Log("shot swung");
        GameObject bullet = Instantiate(this.gameObject.GetComponent<WeaponController>().bulletPrefab, firepos.position, firepos.rotation);
        bullet.GetComponent<MeleHitbox>().player = this.gameObject;
        bullet.GetComponent<MeleHitbox>().knockBack = this.gameObject.GetComponent<WeaponController>().currentWeapon.GetComponent<WeaponData>().meleKnockback;
    }

    void SpawnBat() 
    {
        Debug.Log("make bat");
        GameObject bullet = Instantiate(this.gameObject.GetComponent<WeaponController>().bulletPrefab, firepos.position, firepos.rotation);
        Vector3 newRotation = new Vector3(0, 0, 0);
        bullet.transform.eulerAngles = newRotation;
    }

    IEnumerator fireRateWait(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        fireRateWaitBool = false;
    }
}

