using UnityEditor;

[CustomEditor( typeof( InstantiateEntity ) )]
public class InstantiateEntityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        InstantiateEntity myTarget = (InstantiateEntity)target;
        myTarget.entityType = (EntityDatabase.EntityType)EditorGUILayout.EnumPopup( "Entity Type", myTarget.entityType );

        switch ( myTarget.entityType )
        {
            case EntityDatabase.EntityType.EntityType_SimpleEnemy:
                myTarget.simpleMoveRight = EditorGUILayout.Toggle( "Move Right", myTarget.simpleMoveRight );
                break;
        }
    }
}
