using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class PlayerB : MonoBehaviour
{
    void Start()
    {
        EventManager.StartListening("I_AM_PLAYER", gameObject, MyListenerCallBack);

        EventManager.StartListening("CALL_ALL_PLAYERS", gameObject, WhoIAmCallBack);
    }

    void MyListenerCallBack()
    {
        Debug.Log("I am: PlayerB.");
    }

    void WhoIAmCallBack()
    {
        Debug.Log("I'm PlayerB!");
    }
}
