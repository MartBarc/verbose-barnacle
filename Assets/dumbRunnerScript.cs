using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dumbRunnerScript : MonoBehaviour
{
    public GameObject spawn;
    public GameObject door;
    public float speed = 20f;
    public bool isAwake = false;

    private void Awake()
    {
        isAwake = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAwake)
        {
            if (Vector2.Distance(transform.position, door.transform.position) < 1)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, door.transform.position, speed * Time.deltaTime);
            }
        }
    }
}
