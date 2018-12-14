using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using System;

public class Racket : MonoBehaviour {

    [SerializeField]
    private bool isHitting;
    [SerializeField]
    private float racketRotationSpeed = 50.0f;

	void Start ()
    {
      // gameObject.transform.forward = new Vector3(1.0f, 0.0f, 0.0f);
        // TODO add gameManager method to change racketRotationSpeed
    }
	
    // Controller sets direction to the joystick's direction

    // Keyboard rotates direction clockwise or counter clockwise

	void Update ()
    {
        if (!isHitting)
        {
            float vertical = Input.GetAxis("Vertical");
            var inputDevice = InputManager.ActiveDevice;

            Vector3 racketDirection = new Vector3(inputDevice.RightStickX, inputDevice.RightStickY, 0.0f);
            
           // gameObject.transform.Rotate(transform.forward * Time.deltaTime * -vertical * racketRotationSpeed,Space.Self); // Keyboard control

           if (racketDirection != Vector3.zero)
           {
                gameObject.transform.right = racketDirection;
           } 
        }
	}
}
