using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisHandler : MonoBehaviour
{
    public SpriteRenderer explisionSprite;
    // Start is called before the first frame update
    void Start()
    {
        if (explisionSprite != null)
        {
            explisionSprite.color = Color.red;
        }
    }

}
