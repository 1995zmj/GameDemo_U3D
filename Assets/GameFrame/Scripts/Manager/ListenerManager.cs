using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ListenerManager : Singleton<ListenerManager>
{
    private Dictionary<string, Dictionary<Object, UnityEvent<Object>>> eventDictionary;

    private List<UnityEvent> listeners = new List<UnityEvent>();
    // Start is called before the first frame update

    public void Raise()
    {

        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].Invoke();
        }
    }

    public void RegisterListener(UnityEvent listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(UnityEvent listener)
    {
        listeners.Remove(listener);
    }
}
