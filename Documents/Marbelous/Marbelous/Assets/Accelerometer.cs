﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accelerometer : MonoBehaviour {
    private Rigidbody rigid;
    public bool isFlat = true;
    public bool reverseAxis = false;
    public float acceleration = (float)15;
    public float maxSpeed = 50;
    public float slowDownSpeed = (float)0.9;
    public float startingSpeed = (float)5;
	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        isFlat = false;
        Vector3 direction = Input.acceleration;
        direction = Quaternion.Euler(90, 0, 0) * direction;
        //if the phone is flat. 
        if (Mathf.Abs(direction.x) <= .1 && Mathf.Abs(direction.z) <= .1) isFlat = true;
        //Debug.Log(direction + " : " + isFlat);
        //is the phone being tilted?
        if (!isFlat)
        {
            for(int i = 0; i < 3; i++)
            {
                //clamp the magnitude of the tilt (So there's no reason for the user to have to hold the phone vertically)
                if(direction[i] > .5)
                {
                    direction[i] = (float)0.5;
                }
            }
            
            //normalize the direction
            direction.Normalize();

            //direction and magnitude of the marble
            direction *= acceleration;
            direction.y = rigid.velocity.y;
            Debug.Log(direction);

            //add force to the marble
            rigid.AddForce(direction);
            //Cap at top speed
            if(rigid.velocity.magnitude > maxSpeed)
            {
                rigid.velocity = rigid.velocity.normalized * maxSpeed;
            }
            Debug.DrawRay(transform.position + Vector3.up, direction, Color.blue);
    
        } else {
            //slow the marble down if the phone is being held flat
            Vector3 targetSpeed = new Vector3(rigid.velocity.x * slowDownSpeed,
                                              rigid.velocity.y,
                                              rigid.velocity.z * slowDownSpeed);

            rigid.velocity = targetSpeed;
        }
	}
}
