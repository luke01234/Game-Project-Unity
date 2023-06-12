//===================//
//scriptable object that allows for custom stats of multiple gun iterations
//===================//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName="Gun", menuName="Weapon/Gun")]

public class GunData : ScriptableObject
{
    //declare scriptable object that holds different stats for different gun types
    //this script has stats for altfire modes
    [Header("Info")]
    public new string name;

    [Header("Shooting")]
    public float damage;
    public float maxDistance;
    public float altDamage;
    public float altMaxDistance;

    [Header("Reloading")]
    public int currentAmmo;
    public int altCurrentAmmo;
    public int magSize;
    public int altMagSize;
    public float fireRate;
    public float altFireRate;
    public float reloadTime;
    public float altReloadTime;
    public bool reloading;
    public bool altReloading;
}
