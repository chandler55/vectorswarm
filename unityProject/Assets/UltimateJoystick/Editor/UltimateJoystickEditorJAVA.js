/* Written by Kaz Crowe */
/* UltimateJoystickEditorJAVA.js ver. 1.0 */
#pragma strict
@CustomEditor( UltimateJoystickJAVA )


public class UltimateJoystickEditorJAVA extends Editor
{
	// Warning strings
	private var tensionWarnMessage : String;
	private var tensionWarnMessageDefault : String;
	// We use this camera for updating our positioning on our joystick in edit mode
	private var sceneCamera : Camera;
	// These variables we use to store the last known option and if it's different we update it here and apply it to the joystick
	private var highlightColor : Color;
	private var tensionColor : Color;
	private var screenSize : Vector2;

	/*
	For more information on the OnInspectorGUI and adding your own variables
	in the UltimateJoystick.cs script and displaying them in this script,
	see the EditorGUILayout section in the Unity Documentation to help out.
	 */
	 
	function OnInspectorGUI ()
	{
		// Get our camera in our scene in order to get the right dimentions
		if( sceneCamera == null )
			sceneCamera = GameObject.FindObjectOfType( Camera ) as Camera;
			
		// Store the joystick that we are selecting
		var joystick : UltimateJoystickJAVA = target as UltimateJoystickJAVA;
		
		/* BASIC VARIABLES */
		EditorGUILayout.Space();
		// First we use a Foldout so that this section can be collapsed
		EditorPrefs.SetBool( ( "basicVar" ), EditorGUILayout.Foldout( EditorPrefs.GetBool( "basicVar" ), "Basic Variables" ) );
		if( EditorPrefs.GetBool( "basicVar" ) == true )
		{
			// Show our Basic Variables
			joystick.joystick = EditorGUILayout.ObjectField( "Joystick", joystick.joystick, RectTransform, true ) as RectTransform;
			joystick.joystickSizeFolder = EditorGUILayout.ObjectField( "Size Folder", joystick.joystickSizeFolder, RectTransform, true ) as RectTransform;
			EditorGUILayout.Space();
		}
		
		/* SIZE AND PLACEMENT */
		EditorPrefs.SetBool( ( "sizeAndPlace" ), EditorGUILayout.Foldout( EditorPrefs.GetBool( "sizeAndPlace" ), "Size and Placement" ) );
		if( EditorPrefs.GetBool( "sizeAndPlace" ) == true)
		{
			// Arrange our joystick variables to be shown the way we want
			joystick.position = System.Convert.ToInt32( EditorGUILayout.EnumPopup( "Joystick Position", joystick.position ) );
			joystick.joystickTouchSize = System.Convert.ToInt32( EditorGUILayout.EnumPopup( "Joystick Touch Size", joystick.joystickTouchSize ) );
			joystick.joystickSize = EditorGUILayout.Slider( "Joystick Size", joystick.joystickSize, 0.5f, 3.0f );
			joystick.radiusModifier = EditorGUILayout.Slider( "Radius", joystick.radiusModifier, 2.0f, 7.0f );
			joystick.touchBasedPositioning = EditorGUILayout.ToggleLeft( "Touch Based Positioning", joystick.touchBasedPositioning );
			joystick.joystickSpacing = EditorGUILayout.Vector2Field( "Custom Spacing", joystick.joystickSpacing );
			EditorGUILayout.Space();
		}
		
		/* STYLE AND OPTIONS */
		EditorPrefs.SetBool( ( "styleAndOptions" ), EditorGUILayout.Foldout( EditorPrefs.GetBool( "styleAndOptions" ), "Style and Options" ) );
		if( EditorPrefs.GetBool( "styleAndOptions" ) == true)
		{
			// Joystick Style
			joystick.joystickStyle = System.Convert.ToInt32( EditorGUILayout.EnumPopup( "Joystick Style", joystick.joystickStyle ) );
			if( joystick.joystickStyle == UltimateJoystickJAVA.JoystickStyle.UltimateJoystick )
			{
				// Show Highlight
				joystick.showHighlight = EditorGUILayout.ToggleLeft( "Show Highlight", joystick.showHighlight );
				if( joystick.showHighlight == true )
				{
					// Highlight Options
					joystick.highlightColor = EditorGUILayout.ColorField( "Highlight Color", joystick.highlightColor );
					joystick.highlightBase = EditorGUILayout.ObjectField( "Base Highlight", joystick.highlightBase, UnityEngine.UI.Image, true ) as UnityEngine.UI.Image;
					joystick.highlightJoystick = EditorGUILayout.ObjectField( "Joystick Highlight", joystick.highlightJoystick, UnityEngine.UI.Image, true ) as UnityEngine.UI.Image;

					// If any of the variables are unassigned, we want to tell them
					if( joystick.highlightBase == null && joystick.highlightJoystick == null )
						EditorGUILayout.HelpBox( "Base and Joystick Highlight will not be displayed.", MessageType.Warning );
					else if( joystick.highlightBase == null && joystick.highlightJoystick != null )
						EditorGUILayout.HelpBox( "Base Highlight will not be displayed", MessageType.Warning );
					else if( joystick.highlightBase != null && joystick.highlightJoystick == null )
						EditorGUILayout.HelpBox( "Joystick Highlight will not be displayed", MessageType.Warning );

					EditorGUILayout.Space();
				}

				// Show Tension
				joystick.showTension = EditorGUILayout.ToggleLeft( "Show Tension", joystick.showTension );
				if( joystick.showTension == true )
				{
					// Tension Options and Variables
					joystick.tensionColorNone = EditorGUILayout.ColorField( "Tension None", joystick.tensionColorNone );
					joystick.tensionColorFull = EditorGUILayout.ColorField( "Tension Full", joystick.tensionColorFull );
					joystick.tensionAccentUp = EditorGUILayout.ObjectField( "Tension Up", joystick.tensionAccentUp, UnityEngine.UI.Image, true ) as UnityEngine.UI.Image;
					joystick.tensionAccentDown = EditorGUILayout.ObjectField( "Tension Down", joystick.tensionAccentDown, UnityEngine.UI.Image, true ) as UnityEngine.UI.Image;
					joystick.tensionAccentLeft = EditorGUILayout.ObjectField( "Tension Left", joystick.tensionAccentLeft, UnityEngine.UI.Image, true ) as UnityEngine.UI.Image;
					joystick.tensionAccentRight = EditorGUILayout.ObjectField( "Tension Right", joystick.tensionAccentRight, UnityEngine.UI.Image, true ) as UnityEngine.UI.Image;
					
					// Here we are going to check each tension accent
					if( joystick.tensionAccentUp == null || joystick.tensionAccentDown == null || joystick.tensionAccentLeft == null || joystick.tensionAccentRight == null )
					{
						tensionWarnMessage = "Some Tension Accents are unassigned:";
						tensionWarnMessageDefault = tensionWarnMessage;
						if( joystick.tensionAccentUp == null )
						{
							tensionWarnMessage = tensionWarnMessage + " Tension Up";
						}
						if( joystick.tensionAccentDown == null )
						{
							if( tensionWarnMessage != tensionWarnMessageDefault )
								tensionWarnMessage = tensionWarnMessage + ", Tension Down";
							else
								tensionWarnMessage = tensionWarnMessage + " Tension Down";
						}
						if( joystick.tensionAccentLeft == null )
						{
							if( tensionWarnMessage != tensionWarnMessageDefault )
								tensionWarnMessage = tensionWarnMessage + ", Tension Left";
							else
								tensionWarnMessage = tensionWarnMessage + " Tension Left";
						}
						if( joystick.tensionAccentRight == null )
						{
							if( tensionWarnMessage != tensionWarnMessageDefault )
								tensionWarnMessage = tensionWarnMessage + ", Tension Right";
							else
								tensionWarnMessage = tensionWarnMessage + " Tension Right";
						}
						tensionWarnMessage = tensionWarnMessage + ".";
						EditorGUILayout.HelpBox( tensionWarnMessage, MessageType.Warning );
					}
					EditorGUILayout.Space();
				}
			}
			else
			{
				// We are not using the Ultimate Joystick Style, so turn of highlight and tension
				if( joystick.showHighlight == true || joystick.showTension == true )
				{
					joystick.showHighlight = false;
					joystick.showTension = false;
				}
			}

			// This if for using constraints
			joystick.axis = System.Convert.ToInt32( EditorGUILayout.EnumPopup( "Use Axis", joystick.axis ) );
			joystick.boundary = System.Convert.ToInt32( EditorGUILayout.EnumPopup( "Use Boundary", joystick.boundary ) );
			EditorGUILayout.Space();
		}
		
		EditorPrefs.SetBool( ( "touchActions" ), EditorGUILayout.Foldout( EditorPrefs.GetBool( "touchActions" ), "Touch Actions" ) );
		if( EditorPrefs.GetBool( "touchActions" ) == true)
		{
			// This is for implementing our touch actions with animations
			joystick.useAnimation = EditorGUILayout.ToggleLeft( "Use Animation", joystick.useAnimation );
			if( joystick.useAnimation == true )
			{
				joystick.joystickAnimator = EditorGUILayout.ObjectField( "Animator", joystick.joystickAnimator, Animator, true ) as Animator;
				if( joystick.useAnimation == true && joystick.joystickAnimator == null )
					EditorGUILayout.HelpBox( "Joystick Animator needs to be assigned.", MessageType.Error );
				EditorGUILayout.Space();

				if( joystick.joystickAnimator != null )
					if( joystick.joystickAnimator.enabled == false )
						joystick.joystickAnimator.enabled = true;
			}
			else
			{
				if( joystick.joystickAnimator != null )
					if( joystick.joystickAnimator.enabled == true )
						joystick.joystickAnimator.enabled = false;
			}

			// This is for implementing color fading with touch
			joystick.useFade = EditorGUILayout.ToggleLeft( "Use Fade", joystick.useFade );
			if( joystick.useFade == true )
			{
				joystick.fadeUntouched = EditorGUILayout.Slider( "Fade Untouched", joystick.fadeUntouched, 0.0f, 1.0f );
				joystick.fadeTouched = EditorGUILayout.Slider( "Fade Touched", joystick.fadeTouched, 0.0f, 1.0f );
				joystick.joystickBase = EditorGUILayout.ObjectField( "Joystick Base", joystick.joystickBase, UnityEngine.UI.Image, true ) as UnityEngine.UI.Image;
				if( joystick.joystickStyle == UltimateJoystickJAVA.JoystickStyle.UltimateJoystick && joystick.showTension == true )
					EditorGUILayout.HelpBox( "The alpha of Tension Color will not fade. If you want to change the alpha of the Tension Color, modify it with the Tension Color property directly.", MessageType.Warning );

				EditorGUILayout.Space();
			}
		}
		
		/* Resets */
		EditorPrefs.SetBool( ( "resetOption" ), EditorGUILayout.Foldout( EditorPrefs.GetBool( "resetOption" ), "Restore To Default" ) );
		if( EditorPrefs.GetBool( "resetOption" ) == true)
		{
			// In this section, we just are setting up hard coded values to be able to reset our options to
			if( GUILayout.Button( "Size and Placement" ) )
			{
				joystick.joystickTouchSize = UltimateJoystickJAVA.JoystickTouchSize.Default;
				joystick.joystickSize = 1.75f;
				joystick.radiusModifier = 4.5f;
				joystick.joystickSpacing = new Vector2( 5, 5 );
				joystick.touchBasedPositioning = false;
			}
			if( GUILayout.Button( "Style and Options" ) )
			{
				if( joystick.joystickStyle == UltimateJoystickJAVA.JoystickStyle.UltimateJoystick )
				{
					joystick.showHighlight = true;
					joystick.highlightColor = new Color( 0.118f, 0.992f, 0.0f, 1.0f );
					joystick.showTension = true;
					joystick.tensionColorNone = new Color( 0.118f, 0.992f, 0.0f, 0.0f );
					joystick.tensionColorFull = new Color( 0.118f, 0.992f, 0.0f, 1.0f );
				}
				joystick.axis = UltimateJoystickJAVA.Axis.Both;
				joystick.boundary = UltimateJoystickJAVA.Boundary.Circular;
			}
			if( GUILayout.Button( "Touch Actions" ) )
			{
				joystick.useAnimation = false;
				joystick.useFade = false;
				joystick.fadeUntouched = 1.0f;
				joystick.fadeTouched = 0.5f;
			}
		}
		
		// This is for showing helpful tips to help them avoid errors
		if( joystick.joystick == null )
			EditorGUILayout.HelpBox( "Joystick needs to be assigned in 'Basic Variables'!", MessageType.Error );
		if( joystick.joystickSizeFolder == null )
			EditorGUILayout.HelpBox( "Joystick Size Folder needs to be assigned in 'Basic Variables'!", MessageType.Error );

		// This will apply these variables to the selected script
		if( GUI.changed )
		{
			ApplyChanges( joystick );
			EditorUtility.SetDirty( target );
		}
		
		// We need to check our screen size to see if it has changed
		var currentScreenSize : Vector2 = new Vector2( sceneCamera.pixelWidth, sceneCamera.pixelHeight );
		if( screenSize != currentScreenSize )
		{
			screenSize = currentScreenSize;
			ApplyChanges( joystick );
		}

		// Apply Colors
		if( highlightColor != joystick.highlightColor && joystick.showHighlight == true )
		{
			highlightColor = joystick.highlightColor;
			joystick.SetHighlight();
		}
		if( tensionColor != joystick.tensionColorNone && joystick.showTension == true )
		{
			tensionColor = joystick.tensionColorNone;
			joystick.SetTensionAccent();
		}
	}

	function ApplyChanges ( joystick : UltimateJoystickJAVA )
	{
		// if we have a camera in the scene, then we can apply the changes to see them
		if( sceneCamera != null )
			joystick.UpdatePositioning( sceneCamera.pixelWidth, sceneCamera.pixelHeight );
		else
			Debug.LogError( "There is no camera in the scene, you will not be able to see any changes made to the Ultimate Joystick." );
	}
}