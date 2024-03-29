﻿/* Written by Kaz Crowe */
using UnityEngine;
using System.Collections;

[RequireComponent( typeof( CharacterController ) )]
public class ThirdPersonController : MonoBehaviour 
{
	/* Transform's and Movement Variables */
	Transform myTransform;
	public Transform followJoystick;
	public Transform playerCamera;
	public float speed = 0.1f;
	Vector3 followDistance = new Vector3( 0, 6, -7.5f );
	int rotationSpeed = 10;
	CharacterController character;
	/* Joystick's */
	public UltimateJoystick joystickLeft;
	public UltimateJoystick joystickRight;


	void Start () 
	{
		// We need to store some variables before we do anything else
		myTransform = GetComponent<Transform>();
		character = GetComponent<CharacterController>();
	}
	
	void Update ()
	{
		// In order to use our joystick, we will call our  JoystickPosition which will return our Joystick's position as a Vector2.
		Vector2 joystickLeftPos = joystickLeft.JoystickPosition; 
		Vector2 joystickRightPos = joystickRight.JoystickPosition;

		// If our joystickLeftPos is not equal to Vector2.zero, then that means we are touching it
		if( joystickLeftPos != Vector2.zero )
		{
			// This moves our character
			Vector3 movement = new Vector3( joystickLeftPos.x, 0, joystickLeftPos.y );
			character.Move( movement * speed );
		}

		// This will follow our player's position
		followJoystick.position = myTransform.position;

		// If we are touching our right joystick, then we want to follow it
		if( joystickRightPos != Vector2.zero )
		{
			// Store our joystick positions into a temporary Vector3 facingDirection
			Vector3 facingDirection = new Vector3( joystickRightPos.x, 0, joystickRightPos.y );

			// Store that Vector3 into a Quaternion by using LookRotation
			Quaternion targetRotation = Quaternion.LookRotation( facingDirection );

			// Now we will use Quaternion.Slerp to get a more smooth transition
			followJoystick.rotation = Quaternion.Slerp( followJoystick.rotation, targetRotation, Time.deltaTime * rotationSpeed );
		}

		// If our right joystick is not being touched, then we want to follow our left joystick
		else if( joystickLeftPos != Vector2.zero )
		{
			// Now we repeat the steps above
			Vector3 facingDirection = new Vector3( joystickLeftPos.x, 0, joystickLeftPos.y );
			Quaternion targetRotation = Quaternion.LookRotation( facingDirection );
			followJoystick.rotation = Quaternion.Slerp( followJoystick.rotation, targetRotation, Time.deltaTime * rotationSpeed );
		}

		// This will make the camera follow our player
		CameraFollow();
	}

	void CameraFollow ()
	{
		// Make our camera's position = our trans + our followDistance
		playerCamera.position = myTransform.position + followDistance;

		// And look at our player so we are in the middle of the screen
		playerCamera.LookAt( myTransform );
	}
}