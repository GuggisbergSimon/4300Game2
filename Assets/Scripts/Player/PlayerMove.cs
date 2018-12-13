using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    [SerializeField]
    private float playerMaxSpeed = 10.0f;
    [SerializeField]
    private float playerAcceleration = 2.0f;


    void Start ()
    {
		

	}
	
	void Update ()
    {
        float horizontal = Input.GetAxis("Horizontal");


	}
}
