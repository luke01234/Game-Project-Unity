//=================//
//SCRIPT IS INCOMPLETE
//=================//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maintrigger : MonoBehaviour
{
    public teleporter parentScript;
    void Start()
    {
        //get reference to main teleport script
        parentScript = transform.parent.GetComponent<teleporter>();
    }

    void OnTriggerEnter(Collider obj)
    {
        //if can send, send on trigger enter
        if (parentScript.canSend)
        {
          parentScript.Send(obj);
        }
    }
    
    void OnTriggerExit()
    {
      //on trigger exit, reset variables
      parentScript.canSend = true;
      parentScript.canReceive = true;
    }
}
