using UnityEngine;
using System.Collections;

public class Kongregate : MonoBehaviour
{
    KongregateAPI kongregate = null;

    // Use this for initialization
    void Start()
    {
        kongregate = KongregateAPI.Create();
        Messenger.AddListener<long>( Events.GameEvents.PostGameOverScore, OnScorePosted );
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<long>( Events.GameEvents.PostGameOverScore, OnScorePosted );
    }

    void OnScorePosted( long score )
    {
        kongregate.SubmitStats( "Score", (int)score );
    }
}
