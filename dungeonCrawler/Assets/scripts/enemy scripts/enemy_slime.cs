//enemy slime script:
//slimes have patrol chase and attack states, upon death slimes spawn smaller slimes (3 generations max)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class enemy_slime : MonoBehaviour, IDamageable
{
  
  //====================//
  //handling child slime spawn
  public GameObject child;
  public int num=1;
  [SerializeField] private GameObject prefab;
  private Vector3 spawnPosition;
  private Vector3 newSpawn;
  //====================//
  public NavMeshAgent agent;

  public Transform player;

  public LayerMask groundLayer, playerLayer;

  //Patroling 
  //====================//
  [Header("Patroling")]
  public Vector3 walkPoint;
  public float patrolSpeed=1.5f;
  bool walkPointSet;
  public float walkPointRange;
  public bool canMove;
  //====================//

  [Header("Health")]
  public float maxHealth = 100f;
  public float health;

  //Attacking 
  //====================//
  [Header("Attacking")]
  public float timeBetweenAttacks=2f;
  public float chaseSpeed=3.5f;
  public float dealsDamage=15f;
  bool alreadyAttacked;
  //====================//

  //States handling
  //====================//
  [Header("States")]
  public float sightRange, attackRange;
  public bool playerInSightRange, playerInAttackRange;
  //====================//

  private void Awake()
  {
    /*
      on awake, enable the slimevisable script, for some reason it defaults itself offline
      assign the player to search for and get the navmesh
      set health and scale as a function of the current slime generation (big slimes die and create small slimes, health scales accordingly)
    */
    
    child.GetComponent<slimeVisable>().enabled=true;
    player = GameObject.Find("Player").transform;
    agent = GetComponent<NavMeshAgent>();
    prefab.GetComponent<enemy_slime>().enabled=true;
    agent.enabled=true;
    canMove=true;
    agent.isStopped=false;
    
    health=maxHealth/num;
    transform.localScale=new Vector3(.5f/(.5f*num),.5f/(.5f*num),.5f/(.5f*num));
  }

  public void PermaDie()
    {
      //final death of enemy slime (final generation)
      Destroy(gameObject);
    }
    public void Die()
    {
      //death before final generation (create new slime)
      /*
        get position of the dead slime, offset slightly to spawn new slimes using instantiate, also destroy old slime
      */
      spawnPosition=transform.position;
      Destroy(gameObject);

      //spawn random amount of slimes between 1 and 4
      for (int spawnNum=0; spawnNum < Random.Range(1,4); spawnNum++)
        {
          newSpawn.x=spawnPosition.x+Random.Range(-.1f,.1f);
          newSpawn.y=spawnPosition.y-.5f;
          newSpawn.z=spawnPosition.z+Random.Range(-.1f,.1f);
          Instantiate(prefab, newSpawn, Quaternion.identity);
        }
        
    }

    public void Damage(float damage)
   {
    //take damage, check if damage kills the slime, if so, check the generation and if the generation is past 4, the slime permanently dies, else, a new generation
    health -= damage;
    if (health <= 0)
    {
      num++;
      if (num >=4)
      {
        PermaDie();
      }
      else
      {
        Die();
      }
    }
   }

  private void Update()
  {
    //check for sight and attack range
    playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
    playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

    //choose a state according to distance to player
    if (!playerInSightRange && !playerInAttackRange) Patroling();
    if (playerInSightRange && !playerInAttackRange) ChasePlayer();
    if (playerInSightRange && playerInAttackRange) AttackPlayer();

    
  }

  private bool isGrounded()
  { 
    //check if the slime is grounded with a raycast
    if (Physics.Raycast(transform.position, -transform.up, 2f, groundLayer))
    {
      //Debug.Log("Grounded");
      return true;
    }
    else
    {
      return false;
    }
  }

  private void Patroling()
  { 
    /*
      while patroling, if slime can move (determined by whether the visable slime that jumps is grounded or not, slimes move while in air not on ground and hop towards destination)
      then the agent speed is set to patrol speed, else set speed to 0 to stop agent
    */
    if (canMove)
    {
      agent.speed=patrolSpeed;
    }
    else
    {
      agent.speed=0f;
    }
    
    //if there is no walkpoint set for the slime, it will set one
    
    if (!walkPointSet)
    {
      SearchWalkPoint();
    } 
    /*if there is a walkpoint, it sets the point as the destination, and walks to it, checks the distance to the walkpoint, and if the magnitude of the distance is small enough
      it searches for a new walkpoint*/
    if (walkPointSet)
    {
      agent.SetDestination(walkPoint);
    
      
    
      Vector3 distanceToWalkPoint=transform.position - walkPoint;

      //walkpoint reached 
      if (distanceToWalkPoint.magnitude <1f)
      {
        walkPointSet=false;
      }
    }
      
  }

  private void SearchWalkPoint()
  {
    /*
      set a new walk point by selecting random position within the walkpoint range
      then check if the walkpoint is on the ground, and then walk to it
    */
    float randomZ= Random.Range(-walkPointRange,walkPointRange);
    float randomX= Random.Range(-walkPointRange,walkPointRange);

    walkPoint=new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z+randomZ);

    if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
      walkPointSet=true;
  }

  private void ChasePlayer()
  {
    /*
      set speed to chase speed while can move, set destination to player
    */
    if (canMove)
    {
      agent.speed=chaseSpeed;
    }
    else
    {
      agent.speed=0f;
    }
    agent.SetDestination(player.position);
  }
  
  private void AttackPlayer()
  {
    /* 
      while attacking, look at player
      if slime not attacked yet, attack with a raycast
    */
    transform.LookAt(player);

    if (!alreadyAttacked) 
    {
      //ADD DAMAGE
      if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 3f))
      {
        IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
        damageable?.Damage(dealsDamage/num);
      }
      //stay at current position, reset the attack, set alreadyattacked to true
      agent.SetDestination(transform.position);
      StartCoroutine(ResetAttak());
      Invoke(nameof(ResetAttak), timeBetweenAttacks);
      alreadyAttacked=true;
    }
  }

  private IEnumerator ResetAttak()
  {
    //reset the already attacked bool after the timer ends
    yield return new WaitForSeconds(timeBetweenAttacks);

    alreadyAttacked=false;
  }

}
