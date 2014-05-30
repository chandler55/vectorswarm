using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CanEditMultipleObjects]
[CustomEditor(typeof(tk2dRadialSprite))]
class tk2dRadialSpriteEditor : tk2dSpriteEditor
{
	tk2dRadialSprite[] targetRadialSprites = new tk2dRadialSprite[0];
	
	new void OnEnable() {
		base.OnEnable();
		targetRadialSprites = GetTargetsOfType<tk2dRadialSprite>( targets );
	}
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
	
		tk2dRadialSprite target = targetRadialSprites[0];

		tk2dRadialSprite.Direction newRotationDirection = (tk2dRadialSprite.Direction)EditorGUILayout.EnumPopup("Rotation Direction", target.RotationDirection);
		if (newRotationDirection != target.RotationDirection) {
			tk2dUndo.RecordObjects(targetRadialSprites, "Radial Rotation Direction");
			foreach (tk2dRadialSprite spr in targetRadialSprites) {
				spr.RotationDirection = newRotationDirection;
			}
		}

		float newVisibleAmount = EditorGUILayout.FloatField("Visible Amount", target.VisibleAmount);
		if (newVisibleAmount != target.VisibleAmount) {
			tk2dUndo.RecordObjects(targetRadialSprites, "Radial Visible Amount");
			foreach (tk2dRadialSprite spr in targetRadialSprites) {
				spr.VisibleAmount = newVisibleAmount;
			}
		}

		// Something has changed, so we force a rebuild
		if (GUI.changed)
		{
			foreach (tk2dRadialSprite spr in targetRadialSprites) {
				spr.Build();
				EditorUtility.SetDirty(spr);
			}
		}
	
	}
	
	public new void OnSceneGUI() {
		if (tk2dPreferences.inst.enableSpriteHandles == false) return;
		
		tk2dRadialSprite spr = (tk2dRadialSprite)target;
		var sprite = spr.CurrentSprite;
		
		if (sprite == null) {
			return;
		}
		
		Transform t = spr.transform;
		Bounds b = spr.GetUntrimmedBounds();
		Rect localRect = new Rect(b.min.x, b.min.y, b.size.x, b.size.y);
		
		// Draw rect outline
		Handles.color = new Color(1,1,1,0.5f);
		tk2dSceneHelper.DrawRect (localRect, t);
		
		Handles.BeginGUI ();
		// Resize handles
		if (tk2dSceneHelper.RectControlsToggle ()) {
			EditorGUI.BeginChangeCheck ();
			Rect resizeRect = tk2dSceneHelper.RectControl (999888, localRect, t);
			if (EditorGUI.EndChangeCheck ()) {
				tk2dUndo.RecordObjects(new Object[] {t, spr}, "Resize");
				spr.ReshapeBounds(new Vector3(resizeRect.xMin, resizeRect.yMin) - new Vector3(localRect.xMin, localRect.yMin),
				                  new Vector3(resizeRect.xMax, resizeRect.yMax) - new Vector3(localRect.xMax, localRect.yMax));
				EditorUtility.SetDirty(spr);
			}
		}
		// Rotate handles
		if (!tk2dSceneHelper.RectControlsToggle ()) {
			EditorGUI.BeginChangeCheck();
			float theta = tk2dSceneHelper.RectRotateControl (888999, localRect, t, new List<int>());
			if (EditorGUI.EndChangeCheck()) {
				tk2dUndo.RecordObject (t, "Rotate");
				if (Mathf.Abs(theta) > Mathf.Epsilon) {
					t.Rotate(t.forward, theta, Space.World);
				}
			}
		}
		Handles.EndGUI ();
		
		// Sprite selecting
		tk2dSceneHelper.HandleSelectSprites();
		
		// Move targeted sprites
		tk2dSceneHelper.HandleMoveSprites(t, localRect);
		
		if (GUI.changed) {
			EditorUtility.SetDirty(target);
		}
	}
	
	[MenuItem("GameObject/Create Other/tk2d/Radial Sprite", false, 12901)]
	static void DoCreateSlicedSpriteObject()
	{
		tk2dSpriteGuiUtility.GetSpriteCollectionAndCreate( (sprColl) => {
			GameObject go = tk2dEditorUtility.CreateGameObjectInScene("Radial Sprite");
			tk2dRadialSprite sprite = go.AddComponent<tk2dRadialSprite>();
			sprite.SetSprite(sprColl, sprColl.FirstValidDefinitionIndex);
			sprite.Build();
			Selection.activeGameObject = go;
			Undo.RegisterCreatedObjectUndo(go, "Create Radial Sprite");
		} );
	}
}

