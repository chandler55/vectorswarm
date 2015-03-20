/* Written by Kaz Crowe */
/* UltimateJoystick.cs ver. 1.5 */
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UltimateJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
	/* Basic Variables */
	public RectTransform joystick;
	public RectTransform joystickSizeFolder;
	Vector3 joystickCenter;
	Vector2 textureCenter;
	Vector2 defaultPos;
	/* Size and Placement */
	public enum Position
	{
		Left,
		Right
	}
	public Position position;
	public enum JoystickTouchSize
	{
		Default,
		Medium,
		Large
	}
	public JoystickTouchSize joystickTouchSize;
	public float joystickSize = 1.75f;
	public float radiusModifier = 4.5f;
	float radius;
	public bool touchBasedPositioning;
	public Vector2 joystickSpacing = new Vector2( 5.0f, 5.0f );
	/* Style and Options */
	public enum JoystickStyle
	{
		UltimateJoystick,
		Standard
	}
	public JoystickStyle joystickStyle;
	public bool showHighlight = false;
	public Color highlightColor = new Color( 1, 1, 1, 1 );
	public Image highlightBase;
	public Image highlightJoystick;
	public bool showTension = false;
	public Color tensionColorNone = new Color( 1, 1, 1, 1 );
	public Color tensionColorFull = new Color( 1, 1, 1, 1 );
	public Image tensionAccentUp;
	public Image tensionAccentDown;
	public Image tensionAccentLeft;
	public Image tensionAccentRight;
	public enum Axis
	{
		Both,
		X,
		Y
	}
	public Axis axis;
	public enum Boundary
	{
		Circular,
		Square
	}
	public Boundary boundary;
	/* Touch Actions */
	public bool useAnimation;
	public Animator joystickAnimator;
	public bool useFade;
	public float fadeUntouched = 1.0f;
	public float fadeTouched = 0.5f;
	public Image joystickBase;

	
	void Start ()
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
	public Vector2 JoystickPosition
	{
		get
		{
			// Make a temporary Vector2 and divide it by our radius to give us a value between -1 and 1
			Vector2 tempVec = joystick.position - joystickCenter;
			return tempVec / radius;
		}
	}

	// This means we have touched, so process where we have touched
	public void OnPointerDown ( PointerEventData touchInfo )
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
	public void OnDrag ( PointerEventData touchInfo )
	{
		// We need to call our UpdateJoystick and process where we are touching
		UpdateJoystick( touchInfo );
	}

	// This means we have let go
	public void OnPointerUp ( PointerEventData touchInfo )
	{
		// If we are using Touch Based Positioning, then we need to change a few things
		if( touchBasedPositioning == true )
		{
			// we need to set our folder back to defaultPos
			joystickSizeFolder.position = defaultPos;

			// set our joystickCenter again since it changed when we touched down
			joystickCenter = ( Vector2 )joystickSizeFolder.position + textureCenter;
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
	void UpdateJoystick ( PointerEventData touchInfo )
	{
		// Create a new Vector2 to equal the vector from our curret touch to the center of joystick
		Vector2 tempVector = touchInfo.position - ( Vector2 )joystickCenter;

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
		joystick.transform.position = ( Vector2 )joystickCenter + tempVector;

		// If we have our highlightJoystick then we want to move to the same place as our joystick
		if( joystickStyle == JoystickStyle.UltimateJoystick && highlightJoystick != null && showHighlight == true )
			highlightJoystick.transform.position = joystick.transform.position;

		// If we are using the style Ultimate Joystick and if showTension is true, display Tension
		if( joystickStyle == JoystickStyle.UltimateJoystick && showTension == true )
			TensionAccentDisplay();
	}

	// This function updates our options. It is public so we can call it from other scripts to update our positioning
	public void UpdatePositioning ( float screenWidth, float screenHeight )
	{
		// We want our joystick size to be larger when we have a larger number, so we need to calculate that out
		float textureSize = screenHeight * ( joystickSize / 10 );

		// Same with our radius modifier
		radius = textureSize * ( radiusModifier / 10 );

		// We need to store this object's RectTrans so that we can position it
		RectTransform baseTrans = GetComponent<RectTransform>();

		// We need to get a position for our joystick based on our position variable
		Vector2 joystickTexturePosition = ConfigureTexturePosition( textureSize, screenWidth );

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
			float fixedTouchSize;
			if( joystickTouchSize == JoystickTouchSize.Large )
				fixedTouchSize = 2.0f;
			else if( joystickTouchSize == JoystickTouchSize.Medium )
				fixedTouchSize = 1.51f;
			else
				fixedTouchSize = 1.01f;

			// Make a temporary Vector2
			Vector2 tempVector = new Vector2( textureSize, textureSize );

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
	Vector2 ConfigureTexturePosition ( float textureSize, float screenWidth )
	{
		// We need a few temporary Vector2's to work with
		Vector2 tempPosVector;

		// We need to fix our joystickSpacing Vector2
		Vector2 fixedVector = joystickSpacing / 10;

		// And apply that to some positionSpacer temporary variables
		float positionSpacerX = textureSize * fixedVector.x;
		float positionSpacerY = textureSize * fixedVector.y;

		// If it's left, we can simply apply our positionxSpacerX
		if( position == Position.Left )
			tempPosVector.x = positionSpacerX;
		// else it's to the right, we need to calculate out from our right side and apply our positionSpaceX
		else
			tempPosVector.x = ( screenWidth - textureSize ) - positionSpacerX;

		// Here we just apply our positionSpacerY
		tempPosVector.y = positionSpacerY;
		return tempPosVector;
	}

	// This function is called to set up our Highlight Images
	public void SetHighlight ()
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
	public void SetTensionAccent ()
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

	// This function is called when our joystick is moving
	void TensionAccentDisplay ()
	{
		// We need a Vector2 to store our JoystickPosition
		Vector2 tension = JoystickPosition;

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
			// Mathf.Abs gives us a positive number to work with
			tension.x = Mathf.Abs( tension.x );
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
			// Mathf.Abs gives us a positive number to work with
			tension.y = Mathf.Abs( tension.y );
			tensionAccentDown.color = Color.Lerp( tensionColorNone, tensionColorFull, tension.y );

			if( tensionAccentUp.color != tensionColorNone )
				tensionAccentUp.color = tensionColorNone;
		}
	}
	
	void TensionAccentReset ()
	{
		// Reset our tension colors back to tensionColorNone
		tensionAccentUp.color = tensionColorNone;
		tensionAccentDown.color = tensionColorNone;
		tensionAccentLeft.color = tensionColorNone;
		tensionAccentRight.color = tensionColorNone;
	}

	void HandleFade ( string fadeAction )
	{
		// Temporary float to hold our modifier for our color.a
		float alphaMod;

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
			Color joystickColor = joystickBase.color;
			joystickColor.a = alphaMod;

			// Now apply the temporary color to our joystickBase and joystick
			joystickBase.color = joystickColor;
			joystick.GetComponent<Image>().color = joystickColor;
		}

		// Check if we are truely using our highlights
		if( joystickStyle == JoystickStyle.UltimateJoystick && showHighlight == true )
		{
			// Check each Image and repeat what we did above with our joystick and base
			if( highlightBase != null )
			{
				Color highlightBaseColor = highlightBase.color;
				highlightBaseColor.a = alphaMod;
				highlightBase.color = highlightBaseColor;
			}
			if( highlightJoystick != null )
			{
				Color highlightJoyColor = highlightJoystick.color;
				highlightJoyColor.a = alphaMod;
				highlightJoystick.color = highlightJoyColor;
			}
		}
	}
}