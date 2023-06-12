//script to allow players to move up and down ladders
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ladder : MonoBehaviour
{
    //get reference to the player on the ladder
    private playermove scriptController;
    void OnTriggerEnter(Collider obj)
    {
      //if the object on the ladder is a player, get reference to their playermove script, then set isLaddered bool to true
      if (obj.GetType()==typeof(CharacterController))
      {
        scriptController=obj.GetComponent<playermove>();
        scriptController.isLaddered=true;
      }
    }
    void OnTriggerExit(Collider obj)
    {
        //if the object leaving collider is a player, get reference to their playerscript, set laddered to false
        if (obj.GetType()==typeof(CharacterController))
        {
          scriptController=obj.GetComponent<playermove>();
          scriptController.isLaddered=false;
        }
    }
}
