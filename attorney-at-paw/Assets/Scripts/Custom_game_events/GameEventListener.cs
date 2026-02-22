using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomGameEvent : UnityEvent<Component, object> { } 

public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent; //should be the particular Event 'trigger' (serialised in each broadcaster)

    public CustomGameEvent response; //Should be the intended action by the mono behaviour

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }
    public void OnEventRaised(Component sender, object data)
    {
        response.Invoke(sender, data); //listeners need responses w/ signature (Component, object)!!!
    }

}