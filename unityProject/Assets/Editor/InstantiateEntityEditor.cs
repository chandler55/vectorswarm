using UnityEditor;

[CustomEditor( typeof( InstantiateEntity ) )]
public class InstantiateEntityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        bool renameObjectToEntityName = false;

        InstantiateEntity myTarget = (InstantiateEntity)target;
        myTarget.entityType = (EntityDatabase.EntityType)EditorGUILayout.EnumPopup( "Entity Type", myTarget.entityType );

        renameObjectToEntityName = EditorGUILayout.Toggle( "Rename Object", renameObjectToEntityName );

        if ( renameObjectToEntityName )
        {
            renameObjectToEntityName = false;

            switch ( myTarget.entityType )
            {
                case EntityDatabase.EntityType.EntityType_SimpleEnemy:
                    myTarget.gameObject.name = "SimpleEnemy";
                    break;
                case EntityDatabase.EntityType.EntityType_FollowEnemy:
                    myTarget.gameObject.name = "FollowEnemy";
                    break;
                case EntityDatabase.EntityType.EntityType_ReverseEnemy:
                    myTarget.gameObject.name = "ReverseEnemy";
                    break;
                case EntityDatabase.EntityType.EntityType_SineEnemy:
                    myTarget.gameObject.name = "SineEnemy";
                    break;
                case EntityDatabase.EntityType.EntityType_StationaryEnemy:
                    myTarget.gameObject.name = "StationaryEnemy";
                    break;
                case EntityDatabase.EntityType.EntityType_LeverEnemy:
                    myTarget.gameObject.name = "LeverEnemy";
                    break;
                case EntityDatabase.EntityType.EntityType_ShortPathEnemy:
                    myTarget.gameObject.name = "ShortPathEnemy";
                    break;
                case EntityDatabase.EntityType.EntityType_VerticalEnemy:
                    myTarget.gameObject.name = "VerticalEnemy";
                    break;
                case EntityDatabase.EntityType.EntityType_SlowerSimpleEnemy:
                    myTarget.gameObject.name = "SlowerSimpleEnemy";
                    break;
                case EntityDatabase.EntityType.EntityType_MovingStationaryEnemy:
                    myTarget.gameObject.name = "MovingStationaryEnemy";
                    break;
                case EntityDatabase.EntityType.EntityType_HourGlassEnemy:
                    myTarget.gameObject.name = "HourGlassEnemy";
                    break;
                case EntityDatabase.EntityType.EntityType_TriangleSineEnemy:
                    myTarget.gameObject.name = "TriangleSineEnemy";
                    break;
                case EntityDatabase.EntityType.EntityType_MultiplierItem:
                    myTarget.gameObject.name = "MultiplierItem";
                    break;
            }
        }

        switch ( myTarget.entityType )
        {
            case EntityDatabase.EntityType.EntityType_SimpleEnemy:
                myTarget.simpleMoveRight = EditorGUILayout.Toggle( "Move Right", myTarget.simpleMoveRight );
                break;
            case EntityDatabase.EntityType.EntityType_SlowerSimpleEnemy:
                myTarget.simpleMoveRight = EditorGUILayout.Toggle( "Move Right", myTarget.simpleMoveRight );
                break;
        }


    }
}
