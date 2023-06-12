using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeVisable : MonoBehaviour, IDamageable
{
  //====================//
  //variable declaration
    public LayerMask groundLayer;
    public Rigidbody body;
    public Vector3 jumpVector;
    public GameObject parent;
    //pScript=parent script
    public enemy_slime pScript;
    public float jumpHeight;
    public bool canJump;
    public bool waitingToJump;
    //====================//
    public void Awake()
    {
      //on awake, initialize the variables, set parent script to enemy_slime script
      canJump=true;
      waitingToJump=false;
      pScript=parent.GetComponent<enemy_slime>();
      //jumpheight determined by slime generation
      jumpVector.y=jumpHeight/pScript.num;
      body=GetComponent<Rigidbody>();
      body.freezeRotation=true;
    }
    // Update is called once per frame
    void Update()
    {
      //keep setting the position to the same as the parent position, except for the y position
      transform.localPosition= new Vector3(0,transform.localPosition.y,0);
      if (isGrounded())
      {
        //if grounded, parentscript cannot move
        pScript.canMove=false;

        //while can jump, set velocity to jumpvector and canjump to false, if cant jump, start wait to jump coroutine
        if (canJump)
        {
          body.velocity = jumpVector;
          canJump=false;
        }
        else if (!waitingToJump)
        {
          StartCoroutine(WaitToJump());
        }
      }
      //if not grounded, move
      else
      {
        pScript.canMove=true;
      }
    }
    private IEnumerator WaitToJump()
    {
      //wait for random time (function of slime generation)
      waitingToJump=true;

      yield return new WaitForSeconds(Random.Range(2/pScript.num,4/pScript.num));
      //allow slime to jump and waitingToJump set to false
      canJump=true;
      waitingToJump=false;
    }

    private bool isGrounded()
    {
      //check if the rigidbody is grounded with a raycast
      if (Physics.Raycast(transform.position, -transform.up, .6f/pScript.num, groundLayer))
      {
        return true;
      }
      else
      {
        return false;
      }
    }
    
    public void Damage(float damage)
    {
      //take damage, send it to parent script
      pScript.Damage(damage);
    }
}
