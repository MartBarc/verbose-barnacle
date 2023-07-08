using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisScript : MonoBehaviour
{
    bool m_oneTime = false;

    [SerializeField] public float rightVector = 0f;
    [SerializeField] public float upVector = 0f;

    void FixedUpdate()
    {
        if (!m_oneTime)
        {
            //var impulse = (-45f * Mathf.Deg2Rad) * GetComponent<Rigidbody2D>().inertia;
            //GetComponent<Rigidbody2D>().AddTorque(impulse, ForceMode2D.Impulse);

            GetComponent<Rigidbody2D>().AddForce(new Vector2(rightVector, upVector), ForceMode2D.Impulse);
            m_oneTime = false;
        }
    }
}
