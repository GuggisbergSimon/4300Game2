using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMove : MonoBehaviour {

    [SerializeField]
    private Rigidbody2D playerRigidbody2D;
    [SerializeField]
    private float playerMaxSpeed = 5.0f;
    [SerializeField]
    private float playerAcceleration = 50.0f;


    void Start ()
    {
		

	}
	
	void Update ()
    {
        float horizontal = Input.GetAxis("Horizontal");

        if(Math.Abs(playerRigidbody2D.velocity.x) <= playerMaxSpeed)
        {
            Vector3 playerNewVelocity = playerRigidbody2D.velocity;

            playerNewVelocity += new Vector3(playerAcceleration, 0.0f, 0.0f) * horizontal * Time.deltaTime;

            playerRigidbody2D.velocity = playerNewVelocity;
        }
	}
}
