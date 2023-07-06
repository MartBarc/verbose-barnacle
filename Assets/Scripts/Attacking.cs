using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : MonoBehaviour
{
    public Transform firepos;
    //public Transform meleFirePos;
    public Transform Gun;
    public float firerate;
    public bool fireRateWaitBool = false; //if true = shooting on cd
    public float bulletForce = 20f;
    //public bool ShootingEnabled = true;


    void Update()
    {
        if (!gameObject.GetComponent<PlayerScript>().isAlive)
        {
            return;
        }
      
        if ((Input.GetButtonDown("Fire1") || Input.GetMouseButton(0)))// && ShootingEnabled)
        {
            if (!fireRateWaitBool)
            {
                firerate = this.gameObject.GetComponent<WeaponController>().currentWeapon.GetComponent<WeaponData>().firerate;
                fireRateWaitBool = true;
                StartCoroutine(fireRateWait(firerate));
                this.gameObject.GetComponent<WeaponController>().updateAmmo();
                if (this.gameObject.GetComponent<WeaponController>().currentWeapon.GetComponent<WeaponData>().weaponMeleRanged) //true = mele
                {
                    //mele
                    Swing();
                }
                else
                {
                    //ranged
                    Shoot();
                }
                this.gameObject.GetComponent<WeaponController>().attackSound.enabled = true;
                this.gameObject.GetComponent<WeaponController>().attackSound.Play();
                this.gameObject.GetComponent<WeaponController>().ammo--;
                this.gameObject.GetComponent<WeaponController>().updateAmmo();
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

        IEnumerator fireRateWait(float waitTime)
        {
            yield return new WaitForSecondsRealtime(waitTime);
            fireRateWaitBool = false;
        }
    }
}
