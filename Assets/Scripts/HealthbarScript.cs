using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarScript : MonoBehaviour
{
    public Slider Slider;
    public Color Low;
    public Color High;
    //public Vector3 Offset;
    //public bool ShowHealthText;


    public void SetHealth(float health, float maxHealth)
    {
        Slider.gameObject.SetActive(health < maxHealth);
        //Slider.minValue = 0;
        Slider.value = health;
        Slider.maxValue = maxHealth;

        Slider.fillRect.GetComponent<Image>().color = Color.Lerp(Low, High, Slider.normalizedValue);
    }
}
