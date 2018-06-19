using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class GravityEntity:MonoBehaviour {

    protected CharacterController cc;
    float _vertSpeed = 0;
    Vector3 movement;

    // Use this for initialization
    protected virtual void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    protected virtual void FixedUpdate()
    {
        if (!cc.isGrounded)
        {
            _vertSpeed += Physics.gravity.y * 5 * Time.deltaTime;//不在地面上 
            if (_vertSpeed < -10.0f)
            {
                _vertSpeed = -10.0f;
            }

            movement.y = _vertSpeed;
            movement *= Time.deltaTime;
            cc.Move(movement);
            return;
        }
    }
}
