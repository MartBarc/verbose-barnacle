using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisScript : MonoBehaviour
{
    bool m_oneTime = true;

    [SerializeField] public float rightVector = 0f;
    [SerializeField] public float upVector = 0f;

    public float selfDestroyTime = 2f;

    //private float rightMod = 0f;
    //private float upMod = 0f;

    //public void ModDir(float right, float up)
    //{
    //    rightMod = right;
    //    upMod = up;
    //}

    void Start()
    {
        Destroy(gameObject, selfDestroyTime);
        //damage = gameObject.GetComponent<EnemyScript>().attackDamage;
    }

    void FixedUpdate()
    {
        if (m_oneTime)
        {
            //var impulse = (-45f * Mathf.Deg2Rad) * GetComponent<Rigidbody2D>().inertia;
            //GetComponent<Rigidbody2D>().AddTorque(impulse, ForceMode2D.Impulse);
            rightVector += Random.Range(-0.4f, 0.4f);
            upVector += Random.Range(-0.4f, 0.4f);

            GetComponent<Rigidbody2D>().AddForce(new Vector2(rightVector, upVector), ForceMode2D.Impulse);
            m_oneTime = false;
        }
    }
}
