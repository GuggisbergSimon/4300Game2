using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using System;

public class Racket : MonoBehaviour {

    [SerializeField]
    private bool isHitting = false;
    [SerializeField]
    private bool isChargingHit = false;
    [SerializeField]
    private bool hitBall = false;
    [SerializeField]
    private float timeChargingHit = 0.0f;
    [SerializeField]
    private float hitBasePower = 50.0f;
    [SerializeField]
    private float hitPowerPerSecCharging = 5.0f;
    [SerializeField]
    private float maximumChargingTime = 5.0f;
    [SerializeField]
    private float hitDuration = 0.2f;
    [SerializeField]
    private float timeHitting = 0.0f;
    [SerializeField]
    private float hitSpeed = 500.0f;
    [SerializeField]
    private float racketCounterRotation = -0.5f;

    [SerializeField]
    private Ball ball;
    [SerializeField]
    private PlayerMove player;

    [SerializeField]
    private float racketRotationSpeed = 50.0f;

    [SerializeField]
    private float smashMaxCharge = 100.0f;
    [SerializeField]
    private float smashCurrentCharge = 0.0f;
    [SerializeField]
    private float amountOfChargeToSmash = 25.0f;

    void Start ()
    {
        // TODO add gameManager method to change racketRotationSpeed
    }
	
    // Controller sets direction to the joystick's direction

    // Keyboard rotates direction clockwise or counter clockwise

	void Update ()
    {
        if()



        if (player.GetController().Action2.WasPressed && !isHitting)
        {
            isChargingHit = true;
            gameObject.transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f) * racketCounterRotation * hitSpeed * hitDuration);
        }

        if(isChargingHit)
        {
            timeChargingHit += Time.deltaTime;
        }

        if(player.GetController().Action2.WasReleased)
        {
            Debug.Log("Hitting");
            isChargingHit = false;
            isHitting = true;
        }

        if(isHitting)
        {
            timeHitting += Time.deltaTime;
            gameObject.transform.Rotate(new Vector3( 0.0f, 0.0f, 1.0f) * hitSpeed * Time.deltaTime);

            if(timeHitting >= hitDuration)
            {
                Debug.Log("Hitting end");
                isHitting = false;
                hitBall = false;
                timeHitting = 0.0f;
                timeChargingHit = 0.0f;
            }
        }

        if (!isHitting && !isChargingHit)
        {
            float vertical = Input.GetAxis("Vertical");
            var inputDevice = InputManager.ActiveDevice;

            Vector3 racketDirection = new Vector3(inputDevice.RightStickX, inputDevice.RightStickY, 0.0f);
            
           // gameObject.transform.Rotate(transform.forward * Time.deltaTime * -vertical * racketRotationSpeed,Space.Self); // Keyboard control

           if (racketDirection != Vector3.zero)
           {
                gameObject.transform.right = racketDirection.normalized;
           } 
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == ("Ball") && !hitBall && isHitting)
        {
            Debug.Log("Ball hit");
            hitBall = true;
            float hitPower = hitBasePower + hitPowerPerSecCharging * timeChargingHit;
            ball.SetVelocity(gameObject.transform.up * hitPower);
        }
    }
}
