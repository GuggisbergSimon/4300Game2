using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;

public class Racket : MonoBehaviour {

    [SerializeField]
    private bool isHitting;
    [SerializeField]
    private float racketRotationSpeed = 50.0f;

	private InputDevice controller;

	void Start ()
	{
		controller = GetComponentInParent<PlayerMove>().MyController;
		// TODO add gameManager method to change racketRotationSpeed
	}
	
    // Controller sets direction to the joystick's direction

    // Keyboard rotates direction clockwise or counter clockwise

	void Update ()
    {
        if (!isHitting)
        {
            //float vertical = Input.GetAxis("Vertical");
	        float vertical = controller.LeftStickY;

            gameObject.transform.Rotate(transform.forward * Time.deltaTime * -vertical * racketRotationSpeed,Space.Self);
        }
	}
}
