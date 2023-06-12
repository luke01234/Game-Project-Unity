//===================//
//PLAYER MOVEMENT SCRIPT
//HANDLES ALL PLAYER MOVEMENT AND MOST PLAYER INPUTS
//===================//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermove : MonoBehaviour, IDamageable
{
    
    private float rotX, rotY; //cam rotation
    private CharacterController controller;
    public Vector3 velocity; //vector to move player 
    private bool jump; //jump bool
    public bool inLadder=false; //bool to tell if player is on a ladder
    private Vector3 moveDirectionNorm = Vector3.zero; //movedirection
    private float accel; //acceleration speed
    private float groundDistance=1f; //distance of ground checking raycast
    private RaycastHit slopeHit; //slopechecking raycast to handle sloped surface movement

    [Header("Transforms")]
    public Transform playerCam;
    public Transform spawnPoint; //reset point 

    [Header("Health")]
    public float maxHealth=100f;
    public float health;

    [Header("movement floats")]
    //movement stats
    //all air acceleration related stats have been gutted, no plans to include in final build
    public float ladderSpeed = 5f;
    public float mouseSense= 100f;
    public float gravity = -9.81f;
    public float moveSpeed = 7f;
    public float runAcceleration = 14.0f;
    public float jumpHeight = 3f;
    public float forwardMove, rightMove;
    public float friction =6f;
    public float runDecceleration = 10.0f; 
    public float slideSpeed=.2f;

    [Header("Misc")]
    public bool isLaddered;
    public healthbar healthbar;

    
    

    // Start is called before the first frame update
    void Start()
    {
      //these are redundant cause for some reason they wouldnt update to the correctly value till i did this
      gravity = -9.81f;
      jumpHeight=3f;
      mouseSense= 100f;
      Cursor.lockState = CursorLockMode.Locked;
      controller=GetComponent<CharacterController>();
      health=maxHealth;
      healthbar.SetMaxHealth(maxHealth);
        
    }

    
    //Reset player position at spawn point on call
    void PressRToRestart()
    {
      
      controller.enabled = false;
      controller.gameObject.transform.position = spawnPoint.position;
      controller.enabled=true; 
      health=maxHealth;
      healthbar.SetMaxHealth(maxHealth);
    }
    //check if on steep slope with raycast
    private bool OnSteepSlope()
    {
      //Debug.DrawRay(transform.position, Vector3.down, Color.red, (controller.height/2)+groundDistance);
      if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, (controller.height/2)+groundDistance))
      {
        float slopeAngle = Vector3.Angle(slopeHit.normal,Vector3.up);
        if (slopeAngle>45f) return true;
      }
      return false; 
    }
    //if on steep slope, push you off of it
    void SteepSlopeMovement()
    {
      Vector3 slopeDirection = Vector3.up - slopeHit.normal * Vector3.Dot(Vector3.up, slopeHit.normal);
      

      velocity += slopeDirection*-slideSpeed;
      slideSpeed += .025f;
      //optional really just adds a lil momentum after sliding
      //velocity.y-=slopeHit.point.y;
    }

    //sets the intended movement direction by getting input
    void SetMovementDir()
    {
      forwardMove=Input.GetAxisRaw("Vertical");
      rightMove=Input.GetAxisRaw("Horizontal");
    }
    
    void Accelerate(Vector3 wishDir, float wishSpeed, float accel)
    {
      float addSpeed;
      float accelSpeed;
      float currentSpeed;
      //multiply vectors magnitudes, set added speed to wishspeed passed in - vector multiplication
      currentSpeed=Vector3.Dot(velocity,wishDir);
      addSpeed=wishSpeed-currentSpeed;
      //do nothing if not moving
      if(addSpeed <= 0)
      {
        return;
      }
      //speed that you accelerate = your ecceleration speed x change in time x wishspeed
      accelSpeed=accel*Time.deltaTime*wishSpeed;
      //accleration never above addspeed
      if(accelSpeed>addSpeed)
      {
        accelSpeed=addSpeed;
      }
      //apply accelleration
      velocity.x+=accelSpeed*wishDir.x;
      velocity.z+=accelSpeed*wishDir.z;
    }

    void LadderMove()
    {
      //laddermove identacle to groundmove except move up and down depending on the rotation angle of the camera
      Vector3 wishDir;

      ApplyFriction(1.0f);
      
      //get the inputs
      SetMovementDir();

      //normalize the wish direction and make relative to player
      wishDir=new Vector3(rightMove,0,forwardMove);
      wishDir= transform.TransformDirection(wishDir);
      wishDir.Normalize();
      moveDirectionNorm=wishDir;

      var wishSpeed = wishDir.magnitude;
      wishSpeed *=moveSpeed;
      
      //accelerate in chosen direction 
      Accelerate(wishDir,wishSpeed,runAcceleration);

      //if moving forward and looking up, move up
      //else if moving forward and looking down, move down
      if (forwardMove==1f)
      {
        if (rotX<0)
        {
          velocity.y=ladderSpeed;
        }
        else
        {
          velocity.y=-ladderSpeed;
        }
      }
      //if moving backward and looking up, move down
      //else if moving backward and looking down, move up
      else if (forwardMove==-1f)
      {
        if (rotX<0)
        {
          velocity.y=-ladderSpeed;
        }
        else
        {
          velocity.y=ladderSpeed;
        }
      }
      //else stay stationary on y axis
      else
      {
        velocity.y=0;
      }
    }

    //give player full control of movement, and apply friction, take away control once off ground
    void GroundMove()
    {
      Vector3 wishDir;

      ApplyFriction(1.0f);
      
      //get the inputs
      SetMovementDir();

      //normalize the wish direction and make relative to player
      wishDir=new Vector3(rightMove,0,forwardMove);
      wishDir= transform.TransformDirection(wishDir);
      wishDir.Normalize();
      moveDirectionNorm=wishDir;

      var wishSpeed = wishDir.magnitude;
      wishSpeed *=moveSpeed;
      
      //accelerate in chosen direction 
      Accelerate(wishDir,wishSpeed,runAcceleration);

      //reset gravity velocity
      velocity.y=-4.5f;

      if (jump && controller.isGrounded)
      {
        velocity.y=jumpHeight;
        jump=false;
      }
    }    
    void ApplyFriction(float t)
    { 
      //copy player velocity
      Vector3 vec = velocity;
      float speed, newSpeed, control, drop;
      

      vec.y=0.0f;
      speed=vec.magnitude;
      drop=0.0f;

      //only apply fiction if grounded
      if (controller.isGrounded || isLaddered)
      {
        //if set control to either speed or rundecceleration, whichever is greater
        control = speed < runDecceleration ? runDecceleration : speed;
        //calculate the speed lost by friction
        drop = control * friction * Time.deltaTime *t;

      }
      //drop the friction
      newSpeed = speed - drop;
      //make sure friction never sends you backwards
      if (newSpeed < 0)
      {
        newSpeed=0;
      }
      if (speed > 0)
      {
        newSpeed /= speed;
      }

      //apply friction to player
      velocity.x*=newSpeed;
      velocity.z*=newSpeed;
    }
    public void Damage(float damage)
    {
      //script to take damage,
      //redude damage from healt pool, change healthbar accordingly
      //if health reaches zero, restart player
      health-=damage;
      healthbar.SetHealth(health);
      if (health <= 0)
      {
        PressRToRestart();
      }
    }
    // Update is called once per frame
    void Update()
    {
        //cam controls
        rotX-=Input.GetAxisRaw("Mouse Y") * mouseSense*.02f;
        rotY+=Input.GetAxisRaw("Mouse X") * mouseSense*.02f;
        //clamp camera angle
        rotX= Mathf.Clamp(rotX, -90f, 90f);
        //rotate player, rotate cam
        transform.rotation=Quaternion.Euler(0,rotY,0);
        playerCam.rotation=Quaternion.Euler(rotX,rotY,0);

        //jumping
        jump=Input.GetButtonDown("Jump");

      

        //when grounded = ground move
        //when ladder = ladder
        //not grounded = apply grav
        if (isLaddered)
        {
          LadderMove();
        }
        else if (controller.isGrounded)
        {
          
          GroundMove();
        
          if (OnSteepSlope())
          {
            //push player off steep slopes
            SteepSlopeMovement();
          }
          else
          {
            slideSpeed=.2f;
          }
          
        }
        else
        {
          //not laddered or grounded, apply grav
          velocity.y+=gravity*Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Tab))
        { 
          //if you get tab key, restart player
          PressRToRestart();  
        }
        
        //move player
        controller.Move(velocity*Time.deltaTime);

    }
}
