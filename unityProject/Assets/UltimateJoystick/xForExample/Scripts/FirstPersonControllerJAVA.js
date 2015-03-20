/* Written by Kaz Crowe */
#pragma strict
@script RequireComponent( CharacterController )

/* Joysticks */
var joystickLeft : UltimateJoystickJAVA;
var joystickRight : UltimateJoystickJAVA;

/* Transfroms */
var cameraPivot : Transform;
private var myTransform : Transform;

private var character : CharacterController;

// These values are hard coded, but feel free to make them public and mess with the values
private var rotationSpeed : Vector2 = new Vector2( 50, 20 );
private var forwardSpeed : float = 0.1f;
private var backwardSpeed : float = 0.075f;
private var sidestepSpeed : float = 0.075f;
	

function Start ()
{
	// Get our component references
	myTransform = GetComponent( Transform );
	character = GetComponent( CharacterController );
}

function Update ()
{
	// Vector2 to store our JoystickPosition;
	var joystickLeftPos : Vector2 = joystickLeft.JoystickPosition;

	// If our joystickLeftPos is not equal to Vector2.zero, then that means we are touching it
	if( joystickLeftPos != Vector2.zero )
	{
		// This moves our character
		var movement : Vector3 = myTransform.TransformDirection( new Vector3( joystickLeftPos.x, 0, joystickLeftPos.y ) );

		// JoystickPosition gives + and - numbers. We want just + to check here so Mathf.Abs
		var fixedMovement : Vector2 = new Vector2( Mathf.Abs( joystickLeftPos.x ), Mathf.Abs( joystickLeftPos.y ) );
		
		// if our joystick is up or down then we want to move forward or backward	
		if ( fixedMovement.y > fixedMovement.x )
		{
			if ( joystickLeftPos.y > 0 )
				movement *= forwardSpeed * fixedMovement.y;
			else
				movement *= backwardSpeed * fixedMovement.y;
		}
		// Our joystick is not farther up or down, so we are sidestepping
		else
			movement *= sidestepSpeed * fixedMovement.x;	

		// Apply the movement
		character.Move( movement );
	}

	// Vector2 for our right JoystickMovement
	var joystickRightPos : Vector2 = joystickRight.JoystickPosition;

	// And another to store our camera rotation
	var camRotation : Vector2 = Vector2.zero;
	if ( joystickRightPos != Vector2.zero )
	{
		// Apply our joystick's position to our camera rotation
		camRotation = joystickRightPos;

		// Apply our speed
		camRotation.x *= rotationSpeed.x;
		camRotation.y *= rotationSpeed.y;
		camRotation *= Time.deltaTime;
		
		// We want to rotate our character around our Y axis
		myTransform.Rotate( 0, camRotation.x, 0, Space.World );
		
		// We only want out camera to rotate up and down
		cameraPivot.Rotate( -camRotation.y, 0, 0 );
	}
}