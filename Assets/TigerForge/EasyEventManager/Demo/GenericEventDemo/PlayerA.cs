using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class PlayerA : MonoBehaviour
{
    void Start()
    {
        EventManager.StartListening("I_AM_PLAYER_A", MyListenerCallBack);

        EventManager.StartListening("PLAYER_A_ONCE_EVENT", OnceListenerCallBack, true);

        EventManager.StartListening("CALL_ALL_PLAYERS", gameObject, WhoIAmCallBack);
    }

    void MyListenerCallBack()
    {
        Debug.Log("I am: PlayerA - My age is: " + EventManager.GetInt("I_AM_PLAYER_A"));
    }

    void OnceListenerCallBack()
    {

        Debug.Log("I am: PlayerA - My real name is: " + EventManager.GetString("PLAYER_A_ONCE_EVENT"));
        Debug.Log("If you press F again, nothing will happen because I stopped the listening, just specifying the event name (new feature).");

        EventManager.StopListening("PLAYER_A_ONCE_EVENT");

    }

    void WhoIAmCallBack()
    {
        Debug.Log("I'm PlayerA!");
    }
}
