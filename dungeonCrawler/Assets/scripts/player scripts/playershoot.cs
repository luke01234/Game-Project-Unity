//===================//
//PLAYERSHOOT INPUT HANDLER
//===================//
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playershoot : MonoBehaviour
{
    //define actions to call on input reads
    public static Action shootInput;
    public static Action reloadInput;

    //assign reload key
    [SerializeField] private KeyCode reloadKey;
    
    void Update()
    {
        //listen for inputs and act on them
        if (Input.GetMouseButton(0))
        {
          shootInput?.Invoke();
        }
        if (Input.GetKeyDown(reloadKey))
        {
          reloadInput?.Invoke();
        }

    }
}
