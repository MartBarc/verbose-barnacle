using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InsurableObject : MonoBehaviour
{
    public bool purchased;
    public bool purchaseable;
    public TextMeshProUGUI buyText;
    public int price = 10;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    buyText.SetText("Press E to purchase " + this.gameObject.name + " for $" + price + ".");
    //    buyText.gameObject.SetActive(false);
    //    if (purchaseable)
    //    {
    //        purchased = false;
    //        CircleCollider2D myCollider = transform.GetComponent<CircleCollider2D>();
    //        myCollider.gameObject.SetActive(true);
    //        myCollider.radius = 3f;
    //        //buyText.gameObject.SetActive(false);
    //    }
    //    else
    //    {
    //        purchased = true;
    //        CircleCollider2D myCollider = transform.GetComponent<CircleCollider2D>();
    //        myCollider.radius = 3f;
    //    }
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (!purchased)
    //    {
    //        if (collision.gameObject.tag == "Player")
    //        {
    //            buyText.gameObject.SetActive(true);
    //        }
    //        return;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (!purchased)
    //    {
    //        if (collision.gameObject.tag == "Player")
    //        {
    //            buyText.gameObject.SetActive(false);
    //        }
    //        return;
    //    }
    //}
}
