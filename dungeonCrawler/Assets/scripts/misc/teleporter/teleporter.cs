//=================//
//SCRIPT IS INCOMPLETE
//=================//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleporter : MonoBehaviour
{
    //initialize set of variables to keep track of main, and child teleporters
    public Transform parentLocationMarker;
    public Transform childTeleporter;
    public Transform childLocationMarker;
    private CharacterController controller;

    public bool canSend=true;
    public bool canReceive=true;
    
    // Start is called before the first frame update
    void Start()
    {
        //set child teleporter to the first child of the main teleporter (this one)
        childTeleporter = transform.GetChild(0);
        childLocationMarker = childTeleporter.GetChild(0);
        //parent location is the second child
        parentLocationMarker = transform.GetChild(1);
        //initialize variables to true
        canReceive=true;
        canSend=true;
    }

    public void Send(Collider obj)
    {
        //if the object being sent is a character controller, change the variables accordingly (to stop constant teleportation) disable character controller(so it can be teleported)
        //and change the position and rotation of the character, then reenable the controller
        if (obj.GetType()==typeof(CharacterController))
        {
          canSend=false;
          canReceive=false;
          controller=obj.GetComponent<CharacterController>();
          controller.enabled=false;
          obj.transform.position=childLocationMarker.position;
          obj.transform.rotation=childLocationMarker.rotation;
          controller.enabled=true;
        }
    }

    public void Receive(Collider obj)
    {
        //if the object being received is a character controller, change the variables accordingly (to stop constant teleportation) disable character controller(so it can be teleported)
        //and change the position and rotation of the character, then reenable the controller
        if (obj.GetType()==typeof(CharacterController))
        {
          canSend=false;
          canReceive=false;
          controller=obj.GetComponent<CharacterController>();
          controller.enabled=false;
          obj.transform.position=parentLocationMarker.position;
          controller.enabled=true;
          obj.transform.rotation=parentLocationMarker.rotation;
        }
    }
}
