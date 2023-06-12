//=================//
//camera bobbing script, makes walking less static
//=================//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headbob : MonoBehaviour
{
    //bool to disable bob
    [SerializeField] private bool _enable = true;

    //stats to change the amplitude and frequency of bobs
    //=================//
    [SerializeField, Range(0,.1f)] private float _amplitude = 0.002f;
    [SerializeField, Range(0,30f)] private float _frequency = 10f;
    //=================//
    //set camera transform and camholder (camholder has the original position to reset to when not moving)
    [SerializeField] private Transform _camera =null;
    [SerializeField] private Transform _cameraholder=null;
    
    private float _toggleSpeed=3.0f;
    private Vector3 _startPos;
    private CharacterController _controller;
    private void Awake()
    {
      //initialize controller and camera
      _controller=GetComponent<CharacterController>();
      _startPos=_camera.localPosition;
    }
    private void CheckMotion()
    {
      //check the motion magnitude of the player and if grounded
      float speed = new Vector3(_controller.velocity.x, 0, _controller.velocity.z).magnitude;
      //if the speed is too low or the character is in the air, return without motion
      if (speed<_toggleSpeed) return;
      if (!_controller.isGrounded)return;
      //else play motion
      PlayMotion(FootStepMotion());
    }
    private void PlayMotion(Vector3 motion)
    {
      //add motion to the camera
      _camera.localPosition += motion; 
    }

    private Vector3 FootStepMotion()
    {
      //calculate the motion
      Vector3 pos = Vector3.zero;
      //use a sine wave to calculate the position, then return it
      pos.y+=Mathf.Sin(Time.time * _frequency) * _amplitude;
      //pos.x += Mathf.Cos(Time.time * _frequency) * _amplitude;
      return pos;
    }
    private void ResetPosition()
    {
      //reset to original camera position using linear interp to smooth movement
      if (_camera.localPosition == _startPos) return;
      _camera.localPosition = Vector3.Lerp(_camera.localPosition, _startPos, 1 * Time.deltaTime);
    }
    /*private Vector3 FocusTarget()
    {
      
      Vector3 pos = new Vector3(transform.position.x, transform.position.y +_cameraholder.localPosition.y, transform.position.z);
      pos += _cameraholder.forward *15.0f;
      return pos;
    }*/
    void Update()
    {
        //if not enabled, return, else, checkmotion and reset position
        if (!_enable) return;

        CheckMotion();
        ResetPosition();
        
    }
}
