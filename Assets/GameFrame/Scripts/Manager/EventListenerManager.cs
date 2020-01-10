using System;
using System.Collections;  
using System.Collections.Generic;
using UnityEngine;  
using UnityEngine.Events;  

public class EventListenerManager  : Singleton<EventListenerManager>
{
    public delegate void Callback();
    public delegate void CallbackPrm(params object[] prm);
    private Dictionary<string, CallbackPrm> _myEventCallback = new Dictionary<string, CallbackPrm>();

    public void AddEventListener(string eventName, CallbackPrm callback)
    {
        if (_myEventCallback.ContainsKey(eventName))
        {
            Debug.Log("Events already exist (CallbackPrm)");
        }
        else
        {
            _myEventCallback.Add(eventName, callback);
        }
    }

    public void AddEventListener(string eventName, Callback callback)
    {
        if (_myEventCallback.ContainsKey(eventName))
        {
            Debug.Log("Events already exist (callback)");
        }
        else
        {
            _myEventCallback.Add(eventName,(object[] o)=> { callback(); });
        }
    }

    private void AddEvent(string eventName, Callback callback)
    {
        if (_myEventCallback.ContainsKey(eventName))
        {
            Debug.Log("Events already exist (callback)");
        }
        else
        {
            _myEventCallback.Add(eventName, (O) => { callback(); });
        }
        
    }
    
    public void RemoveEventListener(string eventName)  
    {  
        if (_myEventCallback.ContainsKey(eventName))
        {
            _myEventCallback.Remove(eventName);
        }
    }
    
    public void Trigger(string eventName,params object[] o)
    {
        if (_myEventCallback.ContainsKey(eventName))
        {
            _myEventCallback[eventName](o);
        }
        else {
            Debug.Log("err : 未注册 " + eventName);
        }
    }
}