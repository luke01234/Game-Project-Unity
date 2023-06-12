//==================//
//healthbar handling script
//==================//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{
    //reference slider representing health
    public Slider slider;
    //gradient that changes depending on health value
    public Gradient gradient;
    //image that fills healthbar
    public Image fill;

    public void SetMaxHealth(float health)
    {
      //set max health from slider and set gradient
      slider.maxValue = health;
      slider.value = health;
      fill.color = gradient.Evaluate(1);
    }

    public void SetHealth(float health)
    {
      //set health value from slider and change gradient
      slider.value=health;
      fill.color = gradient.Evaluate(slider.normalizedValue);

    }
}
