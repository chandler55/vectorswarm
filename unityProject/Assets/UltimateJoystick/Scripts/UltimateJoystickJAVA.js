/* Written by Kaz Crowe */
/* UltimateJoystickJAVA.js ver. 1.5 */
#pragma strict

public class UltimateJoystickJAVA extends MonoBehaviour implements UnityEngine.EventSystems.IPointerDownHandler, UnityEngine.EventSystems.IDragHandler, UnityEngine.EventSystems.IPointerUpHandler
{
	/* Basic Variables */
	var joystick : Transform;
	var joystickSizeFolder : RectTransform;
	private var joystickCenter : Vector3;
	private var textureCenter : Vector2;
	private var defaultPos : Vector2;
	/* Size and Placement */
	enum Position
	{
		Left,
		Right
	}
	var position : Position;
	enum JoystickTouchSize
	{
		Default,
		Medium,
		Large
	}
	var joystickTouchSize : JoystickTouchSize;
	var joystickSize : float = 1.75;
	var radiusModifier : float = 4.5;
	private var radius : float;
	var touchBasedPositioning : boolean = false;
	var joystickSpacing : Vector2 = new Vector2( 5.0f, 5.0f );
	/* Style and Options */
	enum JoystickStyle
	{
		UltimateJoystick,
		Standard
	}
	var joystickStyle : JoystickStyle;
	var showHighlight : boolean = false;
	var highlightColor : Color = new Color( 1, 1, 1, 1 );
	var highlightBase : UnityEngine.UI.Image;
	var highlightJoystick : UnityEngine.UI.Image;
	var showTension : boolean = false;
	var tensionColorNone : Color = new Color( 1, 1, 1, 1 );
	var tensionColorFull : Color = new Color( 1, 1, 1, 1 );
	var tensionAccentUp : UnityEngine.UI.Image;
	var tensionAccentDown : UnityEngine.UI.Image;
	var tensionAccentLeft : UnityEngine.UI.Image;
	var tensionAccentRight : UnityEngine.UI.Image;
	enum Axis
	{
		Both,
		X,
		Y
	}
	var axis : Axis;
	enum Boundary
	{
		Circular,
		Square
	}
	var boundary : Boundary;
	/* Touch Actions */
	var useAnimation : boolean;
	var joystickAnimator : Animator;
	var useFade : boolean;
	var fadeUntouched : float = 1.0f;
	var fadeTouched : float = 0.5f;
	var joystickBase : UnityEngine.UI.Image;


	function Start ()
	{
		// Since Start() is only called in Play Mode, call UpdatePositioning with our current screen dimentions
		UpdatePositioning( Screen.width, Screen.height );

		// We need to check our options here, and call the appropriate functions to get them set up
		if( joystickStyle == JoystickStyle.UltimateJoystick && showHighlight == true )
			SetHighlight();
		if( joystickStyle == JoystickStyle.UltimateJoystick && showTension == true )
			SetTensionAccent();
	}
	
	// This function allows other scripts to get our joysticks position data
	function get JoystickPosition () : Vector2
	{
		// Make a temporary Vector2 and divide it by our radius to give us a value between -1 and 1
		var tempVec : Vector2 = joystick.position - joystickCenter;
		return tempVec / radius;
	}
	
	// This means we have touched, so process where we have touched
	function OnPointerDown ( touchInfo : UnityEngine.EventSystems.PointerEventData )
	{
		// If we are using the Touch Based Positioning we need to do a few things here
		if( touchBasedPositioning == true )
		{
			// We need to move our joysticSizeFolder to the point of our touch
			joystickSizeFolder.position = touchInfo.position - textureCenter;

			// Set our center to the touch position since it has changed
			joystickCenter = touchInfo.position;
		}

		// Call UpdateJoystick with the info from our PointerEventData
		UpdateJoystick( touchInfo );

		// If we want to show animations on Touch, do that here
		if( useAnimation == true )
			joystickAnimator.SetBool( "Touch", true );

		// If we are using our fade on touch do that here as well
		if( useFade == true )
			HandleFade( "Touched" );
	}
	
	// This means we are moving
	function OnDrag ( touchInfo : UnityEngine.EventSystems.PointerEventData )
	{
		// We need to call our UpdateJoystick and process where we are touching
		UpdateJoystick( touchInfo );
	}
	
	// This means we have let go
	function OnPointerUp ( touchInfo : UnityEngine.EventSystems.PointerEventData )
	{
		// If we are using Touch Based Positioning, then we need to change a few things
		if( touchBasedPositioning == true )
		{
			// we need to set our folder back to defaultPos
			joystickSizeFolder.position = defaultPos;

			// set our joystickCenter again since it changed when we touched down
			joystickCenter = joystickSizeFolder.position + textureCenter;
		}

		// Set the joystick to our center
		joystick.position = joystickCenter;

		// If we have our highlightJoystick then we want to move it back to center
		if( joystickStyle == JoystickStyle.UltimateJoystick && showHighlight == true && highlightJoystick != null )
			highlightJoystick.transform.position = joystickCenter;

		// If we are showing Tension Accents, then we need to reset
		if( joystickStyle == JoystickStyle.UltimateJoystick && showTension == true )
			TensionAccentReset();

		// If we want to show animations on touch up, do that here
		if( useAnimation == true )
			joystickAnimator.SetBool( "Touch", false );

		// If we are using our fade on touch up
		if( useFade == true )
			HandleFade( "Untouched" );
	}
	
	// This function updates our joystick according to our touch
	function UpdateJoystick ( touchInfo : UnityEngine.EventSystems.PointerEventData )
	{
		// Create a new Vector2 to equal the vector from our curret touch to the center of joystick
		var tempVector : Vector2 = touchInfo.position - joystickCenter;

		// If we are using just X or Y, then we just need to zero out the right value
		if( axis == Axis.X )
			tempVector.y = 0;
		else if( axis == Axis.Y )
			tempVector.x = 0;

		if( boundary == Boundary.Circular )
		{
			// Clamp the Vector, which will give us a round boundary
			tempVector = Vector2.ClampMagnitude( tempVector, radius );
		}
		else if( boundary == Boundary.Square )
		{
			// We want to Clamp both X and Y seperately so we get a square boundary
			tempVector.x = Mathf.Clamp( tempVector.x,  - radius,  radius );
			tempVector.y = Mathf.Clamp( tempVector.y,  - radius,  radius );
		}

		// Apply the Vector to our Joystick's position
		joystick.transform.position = joystickCenter + tempVector;

		// If we have our highlightJoystick then we want to move to the same place as our joystick
		if( joystickStyle == JoystickStyle.UltimateJoystick && highlightJoystick != null && showHighlight == true )
			highlightJoystick.transform.position = joystick.transform.position;

		// If we are using the style Ultimate Joystick and if showTension is true, display Tension
		if( joystickStyle == JoystickStyle.UltimateJoystick && showTension == true )
			TensionAccentDisplay();
	}
	
	// This function updates our options. It is public so we can call it from other scripts to update our positioning
	public function UpdatePositioning ( screenWidth : float, screenHeight : float )
	{
		// We want our joystick size to be larger when we have a larger number, so we need to calculate that out
		var textureSize : float = screenHeight * ( joystickSize / 10 );

		// Same with our radius modifier
		radius = textureSize * ( radiusModifier / 10 );

		// We need to store this object's RectTrans so that we can position it
		var baseTrans : RectTransform = GetComponent( RectTransform );

		// We need to get a position for our joystick based on our position variable
		var joystickTexturePosition : Vector2 = ConfigureTexturePosition( textureSize, screenWidth );

		// If we are using Touch Based Positioning, then we need to configure our touch area
		if( touchBasedPositioning == true )
		{
			// Depending on our joystickTouchSize options, we need to configure our size
			if( joystickTouchSize == JoystickTouchSize.Large )
				baseTrans.sizeDelta = new Vector2( screenWidth / 2, screenHeight );
			else if( joystickTouchSize == JoystickTouchSize.Medium )
				baseTrans.sizeDelta = new Vector2( screenWidth / 2, screenHeight / 4 * 3 );
			else
				baseTrans.sizeDelta = new Vector2( screenWidth / 2, screenHeight / 2 );

			// If position is set to Left, then we want to set it in the bottom left corner
			if( position == Position.Left )
				baseTrans.position = new Vector2( 0, 0 );
			else // it is right, then we want it to be in the center of our screen
				baseTrans.position = new Vector2( screenWidth / 2, 0 );

			// We need to know our texture's center so that we can move it to our touch position correctly
			textureCenter = new Vector2( textureSize / 2, textureSize / 2 );

			// Also need to store our default position so that we can return after the touch has been lifted
			defaultPos = joystickTexturePosition;
		}
		else
		{
			// Our touch size needs to be fixed to a float value
			var fixedTouchSize : float;
			if( joystickTouchSize == JoystickTouchSize.Large )
				fixedTouchSize = 2.0f;
			else if( joystickTouchSize == JoystickTouchSize.Medium )
				fixedTouchSize = 1.51f;
			else
				fixedTouchSize = 1.01f;

			// Make a temporary Vector2
			var tempVector : Vector2 = new Vector2( textureSize, textureSize );

			// Our touch area is standard, so set it up with our tempVector multiplied by our fixedTouchSize
			baseTrans.sizeDelta = tempVector * fixedTouchSize;

			// We get our texture position and modify it with our sizeDelta - tempVector ( gives us the difference ) and divide by 2
			baseTrans.position = joystickTexturePosition - ( ( baseTrans.sizeDelta - tempVector ) / 2 );
		}
		// Our joystickSizeFolder needs to be textureSize and texture position
		joystickSizeFolder.sizeDelta = new Vector2( textureSize, textureSize );
		joystickSizeFolder.position = joystickTexturePosition;

		// Store the joystick center so we can return to it
		joystickCenter = joystick.position;

		// If we are using our fade we should set it up in here
		if( useFade == true )
			HandleFade( "Untouched" );
		else
			HandleFade( "Reset" );
	}
	
	// This function will configure a Vector2 for the position of our Joystick
	function ConfigureTexturePosition ( textureSize : float, screenWidth : float ) : Vector2
	{
		// We need a few temporary variables to work with
		var positionX : float;
		var positionY : float;

		// We need to fix our joystickSpacing Vector2
		var fixedVector : Vector2 = joystickSpacing / 10;

		// And apply that to some positionSpacer temporary variables
		var positionSpacerX : float = textureSize * fixedVector.x;
		var positionSpacerY : float = textureSize * fixedVector.y;

		if( position == Position.Left )
		{
			// We need to apply our positionSpacerX
			positionX = positionSpacerX;
		}
		else
		{
			// We need to calculate out our right side and apply our positionSpaceX
			positionX = ( screenWidth - textureSize ) - positionSpacerX;
		}

		// Here we just apply our positionSpacerY
		positionY = positionSpacerY;
		return new Vector2( positionX, positionY );
	}
	
	// This function is called to set up our Highlight Images
	function SetHighlight ()
	{
		// If we are using fade, then we want to modify our highlight's color
		if( useFade == true )
			highlightColor.a = fadeUntouched;

		// Here we need to check if each variable is assigned so we don't get a null reference log error when applying color
		if( highlightBase != null )
			highlightBase.color = highlightColor;

		if( highlightJoystick != null )
			highlightJoystick.color = highlightColor;
	}
	
	/* These next functions are for our Tension Accents */
	function SetTensionAccent ()
	{
		// We need to check if ANY of our tension accents are unassiggned
		if( tensionAccentUp == null || tensionAccentDown == null || tensionAccentLeft == null || tensionAccentRight == null )
		{
			// Disable showTension to avoid errors, and let the user know with a Debug
			showTension = false;
			Debug.LogError( "Not all Tension Accent's are assign. Disabling Show Tension." );
		}
		else
		{
			// we have all variables assigned and now we can use them without errors
			TensionAccentReset();
		}
	}
	
	function TensionAccentDisplay ()
	{
		// We need a Vector2 to store our joystickPosition
		var tension : Vector2 = JoystickPosition;

		// If our joystick is to the right
		if( tension.x > 0 )
		{
			// Then we lerp our color according to our X position
			tensionAccentRight.color = Color.Lerp( tensionColorNone, tensionColorFull, tension.x );
			
			// If our tensionAccentLeft is not tensionColorNone, we want to make it so
			if( tensionAccentLeft.color != tensionColorNone )
				tensionAccentLeft.color = tensionColorNone;
		}
		// else our joystick is to the left
		else
		{
			// Multiply by -1 since tension.x is currently a negative number, this will give us a positive number to work with
			tension.x *= -1;
			tensionAccentLeft.color = Color.Lerp( tensionColorNone, tensionColorFull, tension.x );

			if( tensionAccentRight.color != tensionColorNone )
				tensionAccentRight.color = tensionColorNone;
		}

		// If our joystick is up
		if( tension.y > 0 )
		{
			// Then we lerp our color according to our Y position
			tensionAccentUp.color = Color.Lerp( tensionColorNone, tensionColorFull, tension.y );

			// If our tensionAccentDown is not tensionColorNone, we want to make it so
			if( tensionAccentDown.color != tensionColorNone )
				tensionAccentDown.color = tensionColorNone;
		}
		// else it is down
		else
		{
			// Multiply by -1 to give us a positive number
			tension.y *= -1;
			tensionAccentDown.color = Color.Lerp( tensionColorNone, tensionColorFull, tension.y );

			if( tensionAccentUp.color != tensionColorNone )
				tensionAccentUp.color = tensionColorNone;
		}
	}
	
	function TensionAccentReset ()
	{
		// This resets our tension colors back to default
		tensionAccentUp.color = tensionColorNone;
		tensionAccentDown.color = tensionColorNone;
		tensionAccentLeft.color = tensionColorNone;
		tensionAccentRight.color = tensionColorNone;
	}
	
	function HandleFade ( fadeAction : String )
	{
		// Temporary float to hold our modifier for our color.a
		var alphaMod : float;

		// Based on our fadeAction, we will modify our alphaMod for use
		if( fadeAction == "Touched" )
			alphaMod = fadeTouched;
		else if( fadeAction == "Untouched" )
			alphaMod = fadeUntouched;
		else
			alphaMod = 1.0f;

		// We need to check if both these are assigned
		if( joystickBase != null && joystick != null )
		{
			// And get a temporary color that is the same as our joystickBase, then change the alpha to our alphaMod
			var joystickColor : Color = joystickBase.color;
			joystickColor.a = alphaMod;

			// Now apply the temporary color to our joystickBase and joystick
			joystickBase.color = joystickColor;
			joystick.GetComponent( UnityEngine.UI.Image ).color = joystickColor;
		}

		// Check if we are truely using our highlights
		if( joystickStyle == JoystickStyle.UltimateJoystick && showHighlight == true )
		{
			// Check each Image and repeat what we did above with our joystick and base
			if( highlightBase != null )
			{
				var highlightBaseColor : Color = highlightBase.color;
				highlightBaseColor.a = alphaMod;
				highlightBase.color = highlightBaseColor;
			}
			if( highlightJoystick != null )
			{
				var highlightJoyColor : Color = highlightJoystick.color;
				highlightJoyColor.a = alphaMod;
				highlightJoystick.color = highlightJoyColor;
			}
		}
	}
}