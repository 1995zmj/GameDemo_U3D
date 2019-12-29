using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField]
    private GameEvent gameEvent;
    [SerializeField]
    private UnityEvent response;


    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }
    // Start is called before the first frame update
    void OnDisabel()
    {
        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        response.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
