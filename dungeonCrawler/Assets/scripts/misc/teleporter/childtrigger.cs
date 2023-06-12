//=================//
//WORK IN PROGRESS SCRIPT, NOT COMPLETE
//=================//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class childtrigger : MonoBehaviour
{
    public teleporter parentScript;
    void Start()
    {
        //get reference to the master teleport script
        parentScript=transform.parent.parent.GetComponent<teleporter>();
    }

    
    void OnTriggerEnter(Collider obj)
    {
        //if parentscript can receive, teleport whatever is in the teleport collider
        if (parentScript.canReceive)
        {
          parentScript.Receive(obj);
        }
    }

    void OnTriggerExit()
    {
      //reset the send and receive variables
      parentScript.canReceive = true;
      parentScript.canSend = true;
    }
}
