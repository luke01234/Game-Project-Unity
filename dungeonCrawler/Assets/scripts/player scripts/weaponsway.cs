//===================//
//script to make weapons sway depending on mouse movement
//makes screen less static when moving around
//===================//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponsway : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float smooth;
    [SerializeField] private float swayMult;

    private void Update()
    {
      //get mouse
      float mouseX=Input.GetAxisRaw("Mouse X") *swayMult;
      float mouseY=Input.GetAxisRaw("Mouse Y") *swayMult;

      //calculate rotation
      Quaternion rotationX= Quaternion.AngleAxis(-mouseY, Vector3.right);
      Quaternion rotationY= Quaternion.AngleAxis(mouseX, Vector3.up);

      Quaternion targetRotation = rotationX * rotationY;

      //rotate
      transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
      
    }
}
