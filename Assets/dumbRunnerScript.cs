using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dumbRunnerScript : MonoBehaviour
{
    public GameObject spawn;
    public GameObject door;
    public float speed = 20f;
    public bool isAwake = false;
    public bool dieWhenArrive = true;
    public int id = 0;
    public AudioSource attackSound;

    private void Awake()
    {
        isAwake = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAwake)
        {
            if (id == 1)
            {
                //attackSound = GameObject.Find("Sound/attacks").GetComponent<AudioSource>();
                //attackSound.Play();
            }
            if (Vector2.Distance(transform.position, door.transform.position) < 1)
            {
                if (dieWhenArrive)
                {
                    Destroy(gameObject);
                }
                else 
                {
                    gameObject.GetComponent<Animator>().SetTrigger("Arrive");
                    //attackSound.Play();
                }
                
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, door.transform.position, speed * Time.deltaTime);
            }
        }
    }
}
