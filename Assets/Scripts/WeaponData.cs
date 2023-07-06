using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : MonoBehaviour
{
    public string weaponName;
    public int weaponId;
    public bool weaponUnlocked; //true = useable, false = not usable yet
    public float weaponDamage;
    public float bulletSpeed;
    public bool weaponMeleRanged; //true = mele, false = range
    public int ammoMax;
    public GameObject bulletPrefab;
    public AudioSource attackSound;
    public AudioSource explosionSound;
    public Sprite UISprite;
    public Sprite inHandSprite;
    public Sprite CrossHair;
    public float meleKnockback;
    public float firerate;
}
