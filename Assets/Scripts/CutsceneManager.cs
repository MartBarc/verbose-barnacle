using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    
    public Image[] images;

    public int currentImage = 0;

    private void Start()
    {
        currentImage = 0;
        foreach (Image image in images)
        {
            image.gameObject.SetActive(false);
        }
        images[currentImage].gameObject.SetActive(true);
    }

    public void goNextImage() 
    {
        images[currentImage].gameObject.SetActive(false);
        currentImage++;
        if (currentImage < 7)
        {
            images[currentImage].gameObject.SetActive(true);
        }
        else 
        {
            currentImage = 0;
            //load game scene
            //Debug.Log("load game here");
            SceneManager.LoadScene("MAP1_SCENE");
        }
    }
}
