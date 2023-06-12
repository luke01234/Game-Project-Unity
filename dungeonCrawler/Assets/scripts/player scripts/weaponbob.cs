//===================//
//weapon bob script, makes walking EVEN LESS static
//===================//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponbob : MonoBehaviour
{
    //bool to enable
    [SerializeField] private bool _enable = true;
    //amplitude and frequency variables
    [SerializeField, Range(0,.1f)] private float _amplitude = 0.0002f;
    [SerializeField, Range(0,30f)] private float _frequency = 5f;
    //set transforms for weapon and weapon holder
    [SerializeField] private Transform _weapon =null;
    [SerializeField] private Transform _weaponholder=null;
    //min speed to bob weapon
    private float _toggleSpeed=3.0f;
    //start pos of weapon
    private Vector3 _startPos;
    //reference to character controller
    private CharacterController _controller;

    private void Awake()
    {
      //get reference to character controller and weapon start location
      _controller=GetComponent<CharacterController>();
      _startPos=_weapon.localPosition;
    }

    private void CheckMotion()
    {
      //check motion, if moving fast enough and grounded, play weapon bob
      float speed = new Vector3(_controller.velocity.x, 0, _controller.velocity.z).magnitude;

      if (speed<_toggleSpeed) return;
      if (!_controller.isGrounded)return;

      PlayMotion(FootStepMotion());
    }

    private void PlayMotion(Vector3 motion)
    {
      //add motion to localposition
      _weapon.localPosition += motion; 
    }

    private Vector3 FootStepMotion()
    {
      //calculate motion from function of sin and cos with amplitude and frequency
      Vector3 pos = Vector3.zero;
      pos.y+=Mathf.Sin(Time.time * _frequency) * _amplitude;
      pos.x += Mathf.Cos(Time.time * _frequency) * _amplitude;
      //absolute value the y pos, dont want it moving in a full circle, just half circle
      pos.y=-Mathf.Abs(pos.y);
      return pos;
    }

    private void ResetPosition()
    {
      //reset position using linear interpolation
      if (_weapon.localPosition == _startPos) return;
      _weapon.localPosition = Vector3.Lerp(_weapon.localPosition, _startPos, 1 * Time.deltaTime);
    }
    // Start is called before the first frame update
    void Update()
    {
      //when not enabled, do nothing, else checkmotion and reset position
      if (!_enable) return;

      CheckMotion();
      ResetPosition();
        
    }
}
