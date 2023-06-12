//=================//
//GUN SCRIPT FOR AR, WORK IN PROGRESS ALT FIRE
//=================//
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
   [Header("References")]
   //use the Gundata stats from a specific interface (AR)
   [SerializeField] GunData gunData;
   //reference for muzzle (for flash WIP)
   [SerializeField] Transform muzzle;
   //reference to camera, for raycasting
   [SerializeField] Transform Camera;
   //time since last shot for rate of fire calculation
   public float timeSinceLastShot;
   //alt bool, for alt fire (WIP)
   public bool alt;
   
   void Start()
   {
    //initialize ammo and reloading on start
    gunData.reloading=false;
    gunData.currentAmmo=gunData.magSize;
   }

   //private bool CanShoot() => !gunData.reloading && (timeSinceLastShot > 1f  / (gunData.fireRate/60f)) && (gunData.currentAmmo >0);
  //determine if can shoot based on fire rate per second and time since last shot
  private bool CanShoot() => (timeSinceLastShot > 1f  / (gunData.fireRate/60f));
  //determine if can alt shoot (WIP)
   private bool CaltShoot() => (timeSinceLastShot > gunData.altFireRate/60f);
   public void altShoot()
   {
    //alt shooting (WIP) currently the same as normal shoot
    if (CaltShoot())
    {
      
      if (Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hitInfo, gunData.altMaxDistance))
      {
        IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
        damageable?.Damage(gunData.altDamage);
      }
      timeSinceLastShot=0;
    }
   }

   public void Shoot()
   {
    //shoot
    if (CanShoot())
    {
      //if can shoot, shoot
      //send raycast from camera center, call IDamagable interface on whatever it hits, and then damage the target
      if (Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hitInfo, gunData.maxDistance))
      {

        IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
        damageable?.Damage(gunData.damage);
      }
      //subtract 1 from ammo, reset time since last shot
      gunData.currentAmmo--;
      timeSinceLastShot=0;
      //call ongunshot for effects (incomplete)
      OnGunShot();  
    }
   }

   public void StartReload()
   {
    //begin reload, if not reloading, start the reload coroutine
    //reloading and ammo are currently ignored by shoot, for debugging purposes
    if (!gunData.reloading)
    {
      StartCoroutine(Reload());
    }
   }

   private IEnumerator Reload() 
   {
    //set reloading to true, wait reloadTime seconds, then reset reloading and current ammo
    gunData.reloading=true;

    yield return new WaitForSeconds(gunData.reloadTime);

    gunData.currentAmmo = gunData.magSize;

    gunData.reloading = false; 
   }
   
   public void ToggleAlt()
   {
    //toggle between alt and main fire (WIP)
    if (alt)
    {
      alt=false;
    }
    else
    {
      alt=true;
    }
   }

   void Update()
    {
      //update time between last shot and now
      timeSinceLastShot+=Time.deltaTime;
      if (Input.GetMouseButton(0))
      {
        //if not in alt, mainfire, if in alt, altfire
        if (!alt)
        {
          Shoot();
        }
        else
        {
          altShoot();
        }
      }
      //listen for reloadkey and alt key
      if (Input.GetKeyDown(KeyCode.R))
      {
        StartReload();
      }
      if (Input.GetKeyDown(KeyCode.F))
      {
        ToggleAlt();
      }
    }
      //gunshot effects (incomplete)
      private void OnGunShot()
      {
    
      }
}
    
   
   
