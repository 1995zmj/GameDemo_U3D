using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class PlayerC : MonoBehaviour
{
    void Start()
    {
        EventManager.StartListening("I_AM_PLAYER", gameObject, MyListenerCallBack);

        EventManager.StartListening("CALL_ALL_PLAYERS", gameObject, WhoIAmCallBack);
    }

    void MyListenerCallBack()
    {
        Debug.Log("I am: PlayerC.");
        var sender = EventManager.GetSender("I_AM_PLAYER");
        if (sender != null)
        {
            GameObject go = (GameObject)sender;
            Debug.Log("...and I know that the guy who called me is: " + go.name);
        }
    }

    void WhoIAmCallBack()
    {
        Debug.Log("I'm PlayerC, called by the new filter engine!");
    }
}
