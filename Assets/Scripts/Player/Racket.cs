using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racket : MonoBehaviour {

    [SerializeField]
    private bool isHitting;
    [SerializeField]
    private float racketRotationSpeed = 50.0f;

	void Start ()
    {
		// TODO add gameManager method to change racketRotationSpeed
	}
	
    // Controller sets direction to the joystick's direction

    // Keyboard rotates direction clockwise or counter clockwise

	void Update ()
    {
        if (!isHitting)
        {
            float vertical = Input.GetAxis("Vertical");

            gameObject.transform.Rotate(transform.forward * Time.deltaTime * -vertical * racketRotationSpeed,Space.Self);
        }
	}
}
